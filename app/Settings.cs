using System.IO;
using System.Text.Json;

namespace Writefix;

public sealed class Settings
{
    public bool Enabled { get; set; } = true;
    public bool LiveRevisions { get; set; } = true;
    public bool AutoEnd { get; set; } = true;
    public double Left { get; set; } = 48;
    public double Top { get; set; } = 48;

    static string PathName => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Writefix", "settings.json");

    public static Settings Load()
    {
        try
        {
            var s = JsonSerializer.Deserialize<Settings>(File.ReadAllText(PathName)) ?? new Settings();
            s.AutoEnd = s.AutoEnd || s.LiveRevisions;
            s.LiveRevisions = s.AutoEnd;
            return s;
        }
        catch
        {
            return new Settings();
        }
    }

    public void Save()
    {
        var dir = System.IO.Path.GetDirectoryName(PathName)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(PathName, JsonSerializer.Serialize(this));
    }
}
