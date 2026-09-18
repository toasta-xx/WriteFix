using System.IO;
using System.Text;
using System.Text.Json;

namespace Writefix;

public sealed class Tok
{
    readonly Node _root = new();
    readonly string[] _pieces;

    public Tok(Stream json)
    {
        using var doc = JsonDocument.Parse(json);
        var vocab = doc.RootElement.GetProperty("model").GetProperty("vocab");
        _pieces = new string[32128];
        var id = 0;
        foreach (var row in vocab.EnumerateArray())
        {
            var piece = row[0].GetString() ?? "";
            var score = row[1].GetSingle();
            if (id < _pieces.Length) _pieces[id] = piece;
            Insert(piece, id, score);
            id++;
        }
    }

    public List<int> Encode(string text)
    {
        var s = "\u2581" + text.Replace(' ', '\u2581');
        var n = s.Length;
        var best = new float[n + 1];
        var prev = new int[n + 1];
        var choose = new int[n + 1];
        Array.Fill(best, float.NegativeInfinity);
        best[0] = 0;
        for (var i = 0; i < n; i++)
        {
            if (float.IsNegativeInfinity(best[i])) continue;
            var node = _root;
            for (var j = i; j < n; j++)
            {
                if (!node.Next.TryGetValue(s[j], out node)) break;
                if (node.Id >= 0 && best[i] + node.Score > best[j + 1])
                {
                    best[j + 1] = best[i] + node.Score;
                    prev[j + 1] = i;
                    choose[j + 1] = node.Id;
                }
            }
        }
        var ids = new List<int>();
        if (float.IsNegativeInfinity(best[n]))
        {
            ids.Add(2);
        }
        else
        {
            var at = n;
            var stack = new Stack<int>();
            while (at > 0)
            {
                stack.Push(choose[at]);
                at = prev[at];
            }
            ids.AddRange(stack);
        }
        ids.Add(1);
        return ids;
    }

    public string Decode(IEnumerable<int> ids)
    {
        var sb = new StringBuilder();
        foreach (var id in ids)
        {
            if (id is 0 or 1 or 2 || id >= _pieces.Length) continue;
            var piece = _pieces[id];
            if (string.IsNullOrEmpty(piece)) continue;
            sb.Append(piece);
        }
        return sb.ToString().Replace('\u2581', ' ').Trim();
    }

    void Insert(string piece, int id, float score)
    {
        var node = _root;
        foreach (var ch in piece)
        {
            if (!node.Next.TryGetValue(ch, out var next))
            {
                next = new Node();
                node.Next[ch] = next;
            }
            node = next;
        }
        node.Id = id;
        node.Score = score;
    }

    sealed class Node
    {
        public readonly Dictionary<char, Node> Next = new();
        public int Id = -1;
        public float Score;
    }
}
