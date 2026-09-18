using System.IO;

namespace Writefix;

static class Log
{
    static readonly object Gate = new();
    static readonly string Dir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Writefix");
    static readonly string Err = Path.Combine(Dir, "error.txt");
    static readonly string Run = Path.Combine(Dir, "run.txt");

    public static void Write(Exception ex)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(Dir);
                if (File.Exists(Err) && new FileInfo(Err).Length > 2_000_000)
                    File.WriteAllText(Err, "");
                File.AppendAllText(Err, $"{DateTime.Now:o} {ex}\r\n");
            }
        }
        catch { }
    }

    public static void Info(string text)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(Dir);
                if (File.Exists(Run) && new FileInfo(Run).Length > 2_000_000)
                    File.WriteAllText(Run, "");
                File.AppendAllText(Run, $"{DateTime.Now:HH:mm:ss.fff} {text}\r\n");
            }
        }
        catch { }
    }
}
