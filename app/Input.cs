using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;

namespace Writefix;

public sealed class Input : IDisposable
{
    public Settings Settings { get; }
    public Model? Brain { get; set; }
    public Action<string>? Status { get; set; }
    public bool HookOk => _sink.Ready;

    readonly Dispatcher _ui;
    readonly StringBuilder _buf = new();
    readonly KeySink _sink;
    IntPtr _lastWin;
    int _hits;
    bool _busy;
    bool _again;
    bool _applying;
    DispatcherTimer? _idle;

    public Input(Settings settings, Dispatcher ui)
    {
        Settings = settings;
        _ui = ui;
        _sink = new KeySink(OnRaw);
    }

    void OnRaw(int vk, char ch)
    {
        if (!_ui.CheckAccess())
        {
            _ui.BeginInvoke(new Action(() => OnRaw(vk, ch)));
            return;
        }
        try { OnKey(vk, ch); }
        catch (Exception ex) { Log.Write(ex); }
    }

    void OnKey(int vk, char ch)
    {
        var hit = Interlocked.Increment(ref _hits);
        var fg = GetForegroundWindow();
        GetWindowThreadProcessId(fg, out var pid);
        var mine = pid == (uint)Environment.ProcessId;
        if (hit <= 6 || ch == ' ')
            Log.Info($"key#{hit} vk={vk} ch={(ch == 0 ? '?' : ch)} pid={pid} mine={mine}");
        if (mine) return;
        if (_applying) return;
        if (!Settings.Enabled) return;
        if (fg != _lastWin)
        {
            _buf.Clear();
            _lastWin = fg;
            Log.Info("focus " + pid);
        }
        if ((GetAsyncKeyState(0x11) & 0x8000) != 0 || (GetAsyncKeyState(0x12) & 0x8000) != 0) return;
        if (vk is 0x0D) { _buf.Clear(); StopIdle(); return; }
        if (vk is 0x09 or 0x1B) return;
        if (vk is 0x08)
        {
            if (_buf.Length > 0) _buf.Length--;
            StopIdle();
            return;
        }
        if (ch == 0) return;
        _buf.Append(ch);
        if (_buf.Length > 1600)
        {
            var cut = _buf.ToString().LastIndexOfAny(['.', '!', '?']);
            if (cut >= 0 && cut < _buf.Length - 80) _buf.Remove(0, cut + 1);
            else if (_buf.Length > 1600) _buf.Remove(0, _buf.Length - 800);
        }
        if (!Language.IsBreak(ch))
        {
            StopIdle();
            return;
        }
        var word = Language.LastWord(_buf.ToString());
        Status?.Invoke($"Heard: {(word.Length == 0 ? "space" : word)}  ·  {hit} keys");
        Log.Info("heard " + word);
        InstantPass();
        ArmIdle(420);
    }

    void InstantPass()
    {
        if (_applying) return;
        var text = _buf.ToString();
        if (!Language.InstantFix(text, out var take, out var put)) return;
        if (take.Length > text.Length || !text.EndsWith(take, StringComparison.Ordinal)) return;
        Log.Info($"token [{take}] -> [{put}]");
        _applying = true;
        try
        {
            if (!FocusEdit.ReplaceLast(take.Length, put))
            {
                Status?.Invoke(take.Trim() + " · apply failed");
                Log.Info("apply failed");
                return;
            }
            _buf.Length -= take.Length;
            _buf.Append(put);
            Status?.Invoke($"{take.Trim()} → {put.Trim()}");
        }
        finally { _applying = false; }
    }

    void ArmIdle(int ms)
    {
        StopIdle();
        if (!Settings.AutoEnd) return;
        _idle = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(ms) };
        _idle.Tick += (_, _) =>
        {
            StopIdle();
            try { QueueModel(); }
            catch (Exception ex) { Log.Write(ex); }
        };
        _idle.Start();
    }

    bool MaybeClose()
    {
        if (_applying || !Settings.AutoEnd) return false;
        var text = _buf.ToString();
        if (!Language.CloseIfReady(text, out var next) || next == text) return false;
        var same = 0;
        while (same < text.Length && same < next.Length && text[same] == next[same]) same++;
        _applying = true;
        try
        {
            if (!FocusEdit.ReplaceLast(text.Length - same, next[same..])) return false;
            _buf.Clear();
            _buf.Append(next);
            Status?.Invoke("Closed thought.");
            Log.Info("period");
            return true;
        }
        finally { _applying = false; }
    }

    void IdleListen()
    {
        if (!MaybeClose())
            Status?.Invoke(Settings.Enabled ? "Listening" : "Paused");
    }

    void QueueModel()
    {
        if (_applying) return;
        if (_busy)
        {
            _again = true;
            return;
        }
        if (Brain is not { Ready: true })
        {
            IdleListen();
            return;
        }
        var text = _buf.ToString();
        var n = Language.EndOfCompleted(text);
        if (n < 3)
        {
            IdleListen();
            return;
        }
        var head = text[..n];
        var slice = Language.ModelSlice(head);
        var last = Language.LastWord(slice);
        var wc = Language.WordCount(slice);
        if (slice.Length < 3 || wc < 1 || (wc < 3 && last.Length < 5))
        {
            IdleListen();
            return;
        }
        _busy = true;
        _again = false;
        Status?.Invoke("Fixing");
        var brain = Brain;
        var shot = slice;
        Task.Run(() =>
        {
            try
            {
                var modeled = Language.TidyPunct(brain.Revise(shot));
                var patched = Language.GrammarPatch(shot, modeled);
                if (last.Length >= 4 && Language.LastWord(patched).Equals(last, StringComparison.Ordinal))
                    patched = Language.ApplyLastSpell(patched, last, brain.Revise(last));
                return patched;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return shot;
            }
        }).ContinueWith(t =>
        {
            _ui.BeginInvoke(new Action(() =>
            {
                _busy = false;
                try
                {
                    var patched = t.Status == TaskStatus.RanToCompletion ? (t.Result ?? "").TrimEnd() : shot;
                    if (patched.Length == 0 || patched.Equals(shot, StringComparison.Ordinal)
                        || (wc < 3 && patched.Equals(shot, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (_again)
                        {
                            _again = false;
                            QueueModel();
                            return;
                        }
                        IdleListen();
                        return;
                    }
                    var now = _buf.ToString();
                    var i = now.LastIndexOf(shot, StringComparison.Ordinal);
                    if (i < 0)
                    {
                        if (_again)
                        {
                            _again = false;
                            QueueModel();
                            return;
                        }
                        IdleListen();
                        return;
                    }
                    var next = now[..i] + patched + now[(i + shot.Length)..];
                    if (next == now)
                    {
                        if (_again)
                        {
                            _again = false;
                            QueueModel();
                            return;
                        }
                        IdleListen();
                        return;
                    }
                    var same = 0;
                    while (same < now.Length && same < next.Length && now[same] == next[same]) same++;
                    _applying = true;
                    try
                    {
                        if (!FocusEdit.ReplaceLast(now.Length - same, next[same..]))
                        {
                            Log.Info("t5 apply failed");
                            if (_again)
                            {
                                _again = false;
                                QueueModel();
                            }
                            else IdleListen();
                            return;
                        }
                        _buf.Clear();
                        _buf.Append(next);
                        var before = Language.LastWord(shot);
                        var after = Language.LastWord(patched);
                        Status?.Invoke(before != after ? $"{before} → {after}" : "Grammar");
                        Log.Info($"t5 [{shot}] -> [{patched}]");
                    }
                    finally { _applying = false; }
                    if (_again)
                    {
                        _again = false;
                        QueueModel();
                    }
                    else IdleListen();
                }
                catch (Exception ex) { Log.Write(ex); }
            }));
        });
    }

    void StopIdle()
    {
        _idle?.Stop();
        _idle = null;
    }

    public void Dispose()
    {
        StopIdle();
        _sink.Dispose();
    }

    [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
    [DllImport("user32.dll")] static extern short GetAsyncKeyState(int v);
}
