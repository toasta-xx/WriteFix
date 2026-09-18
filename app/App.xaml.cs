using System.Windows;
using System.Windows.Forms;

namespace Writefix;

public partial class App : System.Windows.Application
{
    Mutex? _mutex;
    Menu? _menu;
    Input? _input;
    Model? _model;
    NotifyIcon? _tray;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += (_, ev) =>
        {
            Log.Write(ev.Exception);
            ev.Handled = true;
            _menu?.SetStatus("Error logged");
        };
        AppDomain.CurrentDomain.UnhandledException += (_, ev) =>
        {
            if (ev.ExceptionObject is Exception ex) Log.Write(ex);
        };
        TaskScheduler.UnobservedTaskException += (_, ev) =>
        {
            Log.Write(ev.Exception);
            ev.SetObserved();
        };
        _mutex = new Mutex(true, "Writefix.Singleton", out var created);
        if (!created)
        {
            System.Windows.MessageBox.Show(
                "Writefix is already running. Quit the tray icon first, then launch again so the model can reload.",
                "Writefix");
            Shutdown();
            return;
        }

        var settings = Settings.Load();
        Log.Info("boot");
        var check = Language.SelfCheck();
        Log.Info(check == null ? "selfcheck ok" : "selfcheck fail " + check);
        _menu = new Menu(settings);
        if (check != null)
            _menu.HintText.Text = "Self-check failed: " + check;
        _input = new Input(settings, Dispatcher) { Status = _menu.SetStatus };
        Log.Info(_input.HookOk ? "raw ready" : "raw missing");
        _menu.Hidden += (_, _) => _menu.Hide();
        _menu.Moved += (_, _) => settings.Save();
        _menu.Show();
        if (!_input.HookOk)
            _menu.SetLoad("Raw input failed", 0);

        _tray = new NotifyIcon
        {
            Text = "Writefix",
            Visible = true,
            Icon = LoadIcon()
        };
        _tray.MouseClick += (_, ev) =>
        {
            if (ev.Button == MouseButtons.Left)
            {
                _menu.Show();
                _menu.Activate();
            }
        };
        _tray.ContextMenuStrip = new ContextMenuStrip();
        _tray.ContextMenuStrip.Items.Add("Show", null, (_, _) => { _menu.Show(); _menu.Activate(); });
        _tray.ContextMenuStrip.Items.Add("Exit", null, (_, _) => Shutdown());

        Task.Run(() =>
        {
            try
            {
                _model = new Model((label, pct) =>
                {
                    Dispatcher.Invoke(() => _menu.SetLoad(label, pct));
                });
                Dispatcher.Invoke(() => _menu.SetLoad("Warming up", 94));
                var proof = _model.Revise("she dont like apples");
                Log.Info("warmup " + proof);
                Dispatcher.Invoke(() =>
                {
                    _input.Brain = _model;
                    _menu.Brain = _model;
                    if (!_input.HookOk)
                    {
                        _menu.SetLoad("Model ready, input failed", 100);
                        _menu.SetStatus("Raw input failed");
                        return;
                    }
                    _menu.SetReady(string.IsNullOrWhiteSpace(proof) ? "model loaded" : proof);
                });
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Dispatcher.Invoke(() =>
                {
                    _menu.SetLoad("Model failed", 0);
                    _menu.SetStatus("Model failed");
                    _menu.HintText.Text = ex.Message;
                });
            }
        });
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _input?.Settings.Save();
        _input?.Dispose();
        FocusEdit.Stop();
        _model?.Dispose();
        if (_tray != null) _tray.Visible = false;
        _tray?.Dispose();
        _mutex?.Dispose();
        base.OnExit(e);
    }

    static System.Drawing.Icon LoadIcon()
    {
        try
        {
            var path = Environment.ProcessPath;
            if (!string.IsNullOrEmpty(path))
            {
                using var extracted = System.Drawing.Icon.ExtractAssociatedIcon(path);
                if (extracted != null)
                    return (System.Drawing.Icon)extracted.Clone();
            }
        }
        catch (Exception ex) { Log.Write(ex); }
        try
        {
            var asm = typeof(App).Assembly;
            using var stream = asm.GetManifestResourceStream("writefix.ico");
            if (stream != null)
                return new System.Drawing.Icon(stream);
        }
        catch (Exception ex) { Log.Write(ex); }
        return System.Drawing.SystemIcons.Application;
    }
}
