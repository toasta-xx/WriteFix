using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;

namespace Writefix;

static class Access
{
    static int _screenBefore;
    static bool _screenOn;

    public static void Start()
    {
        try
        {
            SystemParametersInfo(0x46, 0, ref _screenBefore, 0);
            SystemParametersInfo(0x47, 1, IntPtr.Zero, 0x02);
            _screenOn = true;
        }
        catch (Exception ex) { Log.Write(ex); }
        try
        {
            Automation.AddAutomationFocusChangedEventHandler((_, _) => { });
        }
        catch (Exception ex) { Log.Write(ex); }
    }

    public static void Stop()
    {
        try { Automation.RemoveAllEventHandlers(); } catch { }
        if (_screenOn)
        {
            try { SystemParametersInfo(0x47, (uint)_screenBefore, IntPtr.Zero, 0x02); } catch { }
            _screenOn = false;
        }
    }

    public static void Wake(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero) return;
        WakeHwnd(hwnd);
        EnumChildWindows(hwnd, (h, _) =>
        {
            var sb = new StringBuilder(128);
            GetClassName(h, sb, 128);
            var name = sb.ToString();
            if (name.Contains("Chrome", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Render", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Widget", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Internet", StringComparison.OrdinalIgnoreCase))
                WakeHwnd(h);
            return true;
        }, IntPtr.Zero);
        try { _ = AutomationElement.FromHandle(hwnd); } catch { }
    }

    static void WakeHwnd(IntPtr hwnd)
    {
        SendMessage(hwnd, 0x003D, IntPtr.Zero, (IntPtr)(-4));
        SendMessage(hwnd, 0x003D, IntPtr.Zero, (IntPtr)(-25));
        var iid = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
        AccessibleObjectFromWindow(hwnd, 0xFFFFFFFC, ref iid, out _);
    }

    delegate bool EnumWnd(IntPtr h, IntPtr l);
    [DllImport("user32.dll")] static extern bool SystemParametersInfo(uint a, uint b, ref int c, uint d);
    [DllImport("user32.dll")] static extern bool SystemParametersInfo(uint a, uint b, IntPtr c, uint d);
    [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr h, int m, IntPtr w, IntPtr l);
    [DllImport("user32.dll")] static extern bool EnumChildWindows(IntPtr h, EnumWnd fn, IntPtr l);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetClassName(IntPtr h, StringBuilder s, int n);
    [DllImport("oleacc.dll")] static extern int AccessibleObjectFromWindow(IntPtr h, uint id, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object? acc);
}
