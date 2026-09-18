using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Writefix;

public partial class Menu : Window
{
    readonly Settings _settings;
    bool _ready;
    bool _boot = true;
    bool _testGuard;
    System.Windows.Threading.DispatcherTimer? _testIdle;
    public Model? Brain { get; set; }
    public event EventHandler? Hidden;
    public event EventHandler? Moved;

    public Menu(Settings settings)
    {
        _settings = settings;
        InitializeComponent();
        var mark = LoadMark();
        if (mark != null)
        {
            Icon = mark;
            BrandMark.Source = mark;
        }
        Left = settings.Left;
        Top = settings.Top;
        OnBox.IsChecked = settings.Enabled;
        EndBox.IsChecked = settings.AutoEnd || settings.LiveRevisions;
        _boot = false;
        SetLoad("Loading model", 2);
    }

    static BitmapFrame? LoadMark()
    {
        using var stream = typeof(Menu).Assembly.GetManifestResourceStream("writefix.ico");
        if (stream == null) return null;
        var copy = new MemoryStream();
        stream.CopyTo(copy);
        copy.Position = 0;
        return BitmapDecoder.Create(copy, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0];
    }

    double _pct;

    public void SetLoad(string label, double pct)
    {
        _pct = Math.Clamp(pct, 0, 100);
        StatusText.Text = label;
        PctText.Text = $"{_pct:0}%";
        PctText.Foreground = _pct >= 42
            ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(22, 21, 19))
            : (System.Windows.Media.Brush)FindResource("Brass");
        LoadChip.Visibility = Visibility.Visible;
        PaintBar();
        Dot.Fill = (System.Windows.Media.Brush)FindResource("Brass");
    }

    public void SetReady(string? proof = null)
    {
        _ready = true;
        SetLoad(proof is { Length: > 0 } ? $"Ready · {proof}" : "Ready", 100);
        var hide = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(2.4) };
        hide.Tick += (_, _) =>
        {
            hide.Stop();
            LoadChip.Visibility = Visibility.Collapsed;
            SetStatus(_settings.Enabled ? "Listening" : "Paused");
        };
        hide.Start();
        if (proof is { Length: > 0 })
            HintText.Text = $"Model loaded offline: {proof}";
    }

    public void SetStatus(string text)
    {
        if (!_ready && text is "Listening" or "Paused") return;
        StatusText.Text = text;
        if (text.Contains('→')) HintText.Text = text;
        Dot.Fill = text is "Listening" or "Revising" or "Grammar" or "Fixing" || text.Contains('→') || text.StartsWith("Closed")
            ? (System.Windows.Media.Brush)FindResource("Brass")
            : new SolidColorBrush(System.Windows.Media.Color.FromRgb(140, 135, 124));
    }

    void BarSized(object sender, SizeChangedEventArgs e) => PaintBar();

    void PaintBar()
    {
        var width = LoadChip.ActualWidth > 1 ? LoadChip.ActualWidth : 280;
        LoadFill.Width = Math.Max(8, width * _pct / 100);
    }

    void Drag(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.TextBox) return;
        if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        _settings.Left = Left;
        _settings.Top = Top;
        Moved?.Invoke(this, EventArgs.Empty);
    }

    void Changed(object sender, RoutedEventArgs e)
    {
        if (_boot) return;
        _settings.Enabled = OnBox.IsChecked == true;
        var polish = EndBox.IsChecked == true;
        _settings.LiveRevisions = polish;
        _settings.AutoEnd = polish;
        _settings.Save();
        if (_ready) SetStatus(_settings.Enabled ? "Listening" : "Paused");
    }

    void TestChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_testGuard || !_settings.Enabled) return;
        var text = TestBox.Text;
        if (text.Length == 0) return;
        if (!Writefix.Language.IsBreak(text[^1])) return;
        try
        {
            var next = Writefix.Language.InstantCompleted(text);
            if (next != text) WriteTest(next, $"{text.Trim()} → {next.Trim()}");
            ArmTestModel();
        }
        catch (Exception ex)
        {
            _testGuard = false;
            Log.Write(ex);
            SetStatus("Test box error");
        }
    }

    void ArmTestModel()
    {
        _testIdle?.Stop();
        if (!_settings.AutoEnd) return;
        _testIdle = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(420) };
        _testIdle.Tick += (_, _) =>
        {
            _testIdle.Stop();
            try { RunTestIdle(); }
            catch (Exception ex) { Log.Write(ex); }
        };
        _testIdle.Start();
    }

    void RunTestIdle()
    {
        var live = TestBox.Text;
        var n = Writefix.Language.EndOfCompleted(live);
        if (n >= 3 && Brain is { Ready: true })
        {
            var head = live[..n];
            var slice = Writefix.Language.ModelSlice(head);
            var last = Writefix.Language.LastWord(slice);
            var wc = Writefix.Language.WordCount(slice);
            if (slice.Length >= 3 && wc >= 1 && !(wc < 3 && last.Length < 5))
            {
                var brain = Brain;
                Task.Run(() =>
                {
                    try
                    {
                        var modeled = Writefix.Language.TidyPunct(brain.Revise(slice));
                        var patched = Writefix.Language.GrammarPatch(slice, modeled);
                        if (last.Length >= 2 && Writefix.Language.LastWord(patched).Equals(last, StringComparison.Ordinal))
                            patched = Writefix.Language.ApplyLastSpell(patched, last, brain.Revise(last));
                        return patched;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                        return slice;
                    }
                }).ContinueWith(t => Dispatcher.BeginInvoke(() =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var patched = (t.Result ?? "").TrimEnd();
                        if (patched.Length > 0 && !patched.Equals(slice, StringComparison.Ordinal))
                        {
                            var now = TestBox.Text;
                            var i = now.LastIndexOf(slice, StringComparison.Ordinal);
                            if (i >= 0)
                            {
                                var next = now[..i] + patched + now[(i + slice.Length)..];
                                if (next != now)
                                {
                                    var before = Writefix.Language.LastWord(slice);
                                    var after = Writefix.Language.LastWord(patched);
                                    WriteTest(next, before != after ? $"{before} → {after}" : "Grammar");
                                }
                            }
                        }
                    }
                    TryCloseTest();
                }));
                return;
            }
        }
        TryCloseTest();
    }

    void TryCloseTest()
    {
        var now = TestBox.Text;
        if (Writefix.Language.CloseIfReady(now, out var closed))
            WriteTest(closed, "Closed thought.");
    }

    void WriteTest(string next, string status)
    {
        _testGuard = true;
        TestBox.Text = next;
        TestBox.CaretIndex = next.Length;
        _testGuard = false;
        SetStatus(status);
    }

    void HideClick(object sender, RoutedEventArgs e)
    {
        Hidden?.Invoke(this, EventArgs.Empty);
    }

    void QuitClick(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
}
