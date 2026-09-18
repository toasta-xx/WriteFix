using System.Runtime.InteropServices;

namespace Writefix;

static class Keys
{
    public static char FromVk(int vk)
    {
        if (vk is 0x20) return ' ';
        if (vk is >= 0x41 and <= 0x5A)
        {
            var shift = Down(0x10);
            var caps = (GetKeyState(0x14) & 1) != 0;
            var c = (char)('a' + (vk - 0x41));
            return shift ^ caps ? char.ToUpperInvariant(c) : c;
        }
        if (vk is >= 0x30 and <= 0x39)
        {
            if (!Down(0x10)) return (char)vk;
            return ")!@#$%^&*("[vk - 0x30];
        }
        if (vk is >= 0x60 and <= 0x69) return (char)('0' + (vk - 0x60));
        return vk switch
        {
            0xBA => Down(0x10) ? ':' : ';',
            0xBB => Down(0x10) ? '+' : '=',
            0xBC => Down(0x10) ? '<' : ',',
            0xBD => Down(0x10) ? '_' : '-',
            0xBE => Down(0x10) ? '>' : '.',
            0xBF => Down(0x10) ? '?' : '/',
            0xC0 => Down(0x10) ? '~' : '`',
            0xDB => Down(0x10) ? '{' : '[',
            0xDC => Down(0x10) ? '|' : '\\',
            0xDD => Down(0x10) ? '}' : ']',
            0xDE => Down(0x10) ? '"' : '\'',
            0x6E => '.',
            _ => '\0'
        };
    }

    static bool Down(int vk) => (GetKeyState(vk) & 0x8000) != 0;

    [DllImport("user32.dll")] static extern short GetKeyState(int v);
}
