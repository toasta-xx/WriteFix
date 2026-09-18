using System.Runtime.InteropServices;

namespace Writefix;

sealed class KeySink : System.Windows.Forms.NativeWindow, IDisposable
{
    public const uint Magic = 0x57F1CE;
    readonly Action<int, char> _onKey;
    bool _ok;

    public bool Ready => _ok && Handle != IntPtr.Zero;

    public KeySink(Action<int, char> onKey)
    {
        _onKey = onKey;
        var cp = new System.Windows.Forms.CreateParams
        {
            Caption = "WritefixRaw",
            Parent = new IntPtr(-3)
        };
        CreateHandle(cp);
        if (Handle == IntPtr.Zero)
        {
            cp = new System.Windows.Forms.CreateParams
            {
                Caption = "WritefixRaw",
                Style = unchecked((int)0x80000000),
                ExStyle = 0x08000080,
                X = -32000,
                Y = -32000,
                Width = 0,
                Height = 0
            };
            CreateHandle(cp);
        }
        var rid = new RAWINPUTDEVICE
        {
            UsagePage = 0x01,
            Usage = 0x06,
            Flags = 0x00000100,
            Target = Handle
        };
        _ok = RegisterRawInputDevices([rid], 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
        Log.Info($"raw hwnd={Handle} reg={_ok} gle={Marshal.GetLastWin32Error()}");
    }

    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
        if (m.Msg == 0x00FF) Read(m.LParam);
        base.WndProc(ref m);
    }

    void Read(IntPtr raw)
    {
        uint size = 0;
        var header = (uint)Marshal.SizeOf<RAWINPUTHEADER>();
        GetRawInputData(raw, 0x10000003, IntPtr.Zero, ref size, header);
        if (size == 0) return;
        var buf = Marshal.AllocHGlobal((int)size);
        try
        {
            if (GetRawInputData(raw, 0x10000003, buf, ref size, header) == uint.MaxValue) return;
            var head = Marshal.PtrToStructure<RAWINPUTHEADER>(buf);
            if (head.Type != 1) return;
            var kb = Marshal.PtrToStructure<RAWKEYBOARD>(IntPtr.Add(buf, (int)header));
            if ((kb.Flags & 1) != 0) return;
            if (kb.Extra == (UIntPtr)Magic) return;
            var vk = kb.VKey;
            if (vk is 0 or 0xFF) return;
            _onKey(vk, Writefix.Keys.FromVk(vk));
        }
        finally { Marshal.FreeHGlobal(buf); }
    }

    public void Dispose()
    {
        try
        {
            if (Handle != IntPtr.Zero)
            {
                var rid = new RAWINPUTDEVICE { UsagePage = 0x01, Usage = 0x06, Flags = 0x00000001, Target = Handle };
                RegisterRawInputDevices([rid], 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
                DestroyHandle();
            }
        }
        catch (Exception ex) { Log.Write(ex); }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTDEVICE
    {
        public ushort UsagePage, Usage;
        public uint Flags;
        public IntPtr Target;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTHEADER
    {
        public uint Type, Size;
        public IntPtr Device, Param;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RAWKEYBOARD
    {
        public ushort MakeCode, Flags, Reserved, VKey;
        public uint Message;
        public UIntPtr Extra;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] p, uint n, uint cb);

    [DllImport("user32.dll")]
    static extern uint GetRawInputData(IntPtr raw, uint cmd, IntPtr data, ref uint size, uint header);
}
