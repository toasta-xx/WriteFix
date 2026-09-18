using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace Writefix;

static class FocusEdit
{
    static bool _prevReader;
    static bool _readerSet;

    public static void Listen()
    {
        try
        {
            SystemParametersInfo(0x0046, 0, out _prevReader, 0);
            SystemParametersInfo(0x0047, 1, IntPtr.Zero, 3);
            _readerSet = true;
            Automation.AddAutomationFocusChangedEventHandler((_, _) => { });
        }
        catch (Exception ex) { Log.Write(ex); }
    }

    public static void Stop()
    {
        try { Automation.RemoveAllEventHandlers(); } catch { }
        if (_readerSet && !_prevReader)
        {
            try { SystemParametersInfo(0x0047, 0, IntPtr.Zero, 3); } catch { }
            _readerSet = false;
        }
    }

    public static void Wake(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero) return;
        try
        {
            var iid = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
            AccessibleObjectFromWindow(hwnd, 0xFFFFFFFC, ref iid, out _);
            _ = AutomationElement.FromHandle(hwnd);
        }
        catch { }
    }

    public static bool TryRead(out string text)
    {
        text = "";
        try
        {
            var el = Editable();
            if (el == null) return false;
            if (TryValue(el, out text)) return true;
            if (el.TryGetCurrentPattern(TextPattern.Pattern, out var t) && t is TextPattern tp)
            {
                text = tp.DocumentRange.GetText(-1) ?? "";
                return text.Length > 0;
            }
        }
        catch (Exception ex) { Log.Write(ex); }
        return false;
    }

    public static bool TryWrite(string text)
    {
        try
        {
            var el = Editable();
            if (el == null) return false;
            if (el.TryGetCurrentPattern(ValuePattern.Pattern, out var raw) && raw is ValuePattern value && !value.Current.IsReadOnly)
            {
                value.SetValue(text);
                return true;
            }
        }
        catch (Exception ex) { Log.Write(ex); }
        return false;
    }

    public static bool Apply(string oldFull, string nextFull)
    {
        if (oldFull == nextFull) return true;
        var same = 0;
        while (same < oldFull.Length && same < nextFull.Length && oldFull[same] == nextFull[same]) same++;
        return ReplaceLast(oldFull.Length - same, nextFull[same..]);
    }

    public static bool ReplaceLast(int back, string insert)
    {
        if (back <= 0 && insert.Length == 0) return true;
        return WithFocus(() => TypeTail(back, insert));
    }

    static bool TryValue(AutomationElement el, out string text)
    {
        text = "";
        if (!el.TryGetCurrentPattern(ValuePattern.Pattern, out var raw) || raw is not ValuePattern value) return false;
        text = value.Current.Value ?? "";
        return true;
    }

    static AutomationElement? Editable()
    {
        try
        {
            var el = AutomationElement.FocusedElement;
            if (el == null) return null;
            if (CanEdit(el) || HasText(el)) return el;
            var walk = el;
            for (var i = 0; i < 6 && walk != null; i++)
            {
                walk = TreeWalker.ControlViewWalker.GetParent(walk);
                if (walk != null && (CanEdit(walk) || HasText(walk))) return walk;
            }
            var child = TreeWalker.ControlViewWalker.GetFirstChild(el);
            for (var n = 0; n < 8 && child != null; n++)
            {
                if (CanEdit(child) || HasText(child)) return child;
                child = TreeWalker.ControlViewWalker.GetNextSibling(child);
            }
            return el;
        }
        catch { return null; }
    }

    static bool CanEdit(AutomationElement el)
    {
        try
        {
            return el.TryGetCurrentPattern(ValuePattern.Pattern, out var raw) && raw is ValuePattern v && !v.Current.IsReadOnly;
        }
        catch { return false; }
    }

    static bool HasText(AutomationElement el)
    {
        try
        {
            return el.TryGetCurrentPattern(TextPattern.Pattern, out _);
        }
        catch { return false; }
    }

    static bool WithFocus(Func<bool> act)
    {
        var fg = GetForegroundWindow();
        var fgTid = GetWindowThreadProcessId(fg, out var pid);
        if (pid == (uint)Environment.ProcessId) return false;
        var self = GetCurrentThreadId();
        var attached = fgTid != 0 && fgTid != self && AttachThreadInput(self, fgTid, true);
        try { return act(); }
        finally
        {
            if (attached) AttachThreadInput(self, fgTid, false);
        }
    }

    static bool TypeTail(int back, string insert)
    {
        var list = new List<INPUT>(Math.Max(8, (back + insert.Length) * 4));
        for (var i = 0; i < back; i++)
        {
            list.Add(VkDown(0x08));
            list.Add(VkUp(0x08));
        }
        foreach (var ch in insert)
            AddChar(list, ch);
        return Flush(list);
    }

    static void AddChar(List<INPUT> list, char ch)
    {
        if (ch is '\r' or '\n') return;
        var code = VkKeyScan(ch);
        var vk = (byte)(code & 0xFF);
        var mods = (code >> 8) & 0xFF;
        if (code != -1 && vk != 0xFF && (mods & 0x06) == 0)
        {
            var shift = (mods & 1) != 0;
            if (shift) list.Add(VkDown(0x10));
            list.Add(VkDown(vk));
            list.Add(VkUp(vk));
            if (shift) list.Add(VkUp(0x10));
            return;
        }
        list.Add(Uni(ch, 0));
        list.Add(Uni(ch, KeyUp));
    }

    static bool Flush(List<INPUT> list)
    {
        if (list.Count == 0) return true;
        var data = list.ToArray();
        var sent = SendInput((uint)data.Length, data, Marshal.SizeOf<INPUT>());
        if (sent == 0)
        {
            Log.Write(new InvalidOperationException($"SendInput 0 gle {Marshal.GetLastWin32Error()} size {Marshal.SizeOf<INPUT>()}"));
            return false;
        }
        return true;
    }

    const int KeyUp = 2, Unicode = 4;

    static INPUT VkDown(ushort vk) => new()
    {
        Type = 1,
        Ki = new KEYBDINPUT { Vk = vk, Extra = (IntPtr)KeySink.Magic }
    };

    static INPUT VkUp(ushort vk) => new()
    {
        Type = 1,
        Ki = new KEYBDINPUT { Vk = vk, Flags = KeyUp, Extra = (IntPtr)KeySink.Magic }
    };

    static INPUT Uni(char c, int flags) => new()
    {
        Type = 1,
        Ki = new KEYBDINPUT { Scan = c, Flags = (uint)(Unicode | flags), Extra = (IntPtr)KeySink.Magic }
    };

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort Vk, Scan;
        public uint Flags, Time;
        public IntPtr Extra;
    }

    [StructLayout(LayoutKind.Explicit, Size = 40)]
    struct INPUT
    {
        [FieldOffset(0)] public int Type;
        [FieldOffset(8)] public KEYBDINPUT Ki;
    }

    [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
    [DllImport("kernel32.dll")] static extern uint GetCurrentThreadId();
    [DllImport("user32.dll")] static extern bool AttachThreadInput(uint a, uint b, bool attach);
    [DllImport("user32.dll", SetLastError = true)] static extern uint SendInput(uint n, INPUT[] i, int size);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern short VkKeyScan(char ch);
    [DllImport("user32.dll")] static extern bool SystemParametersInfo(uint act, uint param, out bool v, uint ini);
    [DllImport("user32.dll")] static extern bool SystemParametersInfo(uint act, uint param, IntPtr v, uint ini);
    [DllImport("oleacc.dll")]
    static extern int AccessibleObjectFromWindow(IntPtr hwnd, uint id, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object acc);
}
