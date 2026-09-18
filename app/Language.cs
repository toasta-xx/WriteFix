using System.Text.RegularExpressions;

namespace Writefix;

public static class Language
{
    static readonly Dictionary<string, string> Typos = new(StringComparer.OrdinalIgnoreCase)
    {
        ["abreviate"] = "abbreviate", ["absense"] = "absence", ["accross"] = "across", ["accomodate"] = "accommodate",
        ["acheive"] = "achieve", ["adress"] = "address", ["agressive"] = "aggressive", ["aquire"] = "acquire",
        ["alot"] = "a lot", ["amoung"] = "among", ["arguement"] = "argument", ["awsome"] = "awesome",
        ["basicly"] = "basically", ["becuase"] = "because", ["becuse"] = "because", ["becouse"] = "because",
        ["beacuse"] = "because", ["begining"] = "beginning", ["beleive"] = "believe", ["belive"] = "believe",
        ["buisness"] = "business", ["calender"] = "calendar", ["comming"] = "coming", ["completly"] = "completely",
        ["definately"] = "definitely", ["definatly"] = "definitely", ["defiantly"] = "definitely",
        ["definetly"] = "definitely", ["dilemna"] = "dilemma", ["dont"] = "don't", ["doesnt"] = "doesn't",
        ["cant"] = "can't", ["wont"] = "won't", ["isnt"] = "isn't", ["arent"] = "aren't", ["wasnt"] = "wasn't",
        ["werent"] = "weren't", ["havent"] = "haven't", ["hasnt"] = "hasn't", ["hadnt"] = "hadn't",
        ["wouldnt"] = "wouldn't", ["couldnt"] = "couldn't", ["shouldnt"] = "shouldn't", ["didnt"] = "didn't",
        ["theyre"] = "they're", ["youre"] = "you're", ["youve"] = "you've", ["theyve"] = "they've",
        ["weve"] = "we've", ["ive"] = "I've", ["im"] = "I'm", ["thats"] = "that's", ["whats"] = "what's",
        ["heres"] = "here's", ["theres"] = "there's", ["embarass"] = "embarrass", ["enviroment"] = "environment",
        ["existance"] = "existence", ["experiance"] = "experience", ["freind"] = "friend", ["goverment"] = "government",
        ["grammer"] = "grammar", ["helo"] = "hello", ["hlep"] = "help", ["hte"] = "the", ["imediately"] = "immediately",
        ["independant"] = "independent", ["inteligent"] = "intelligent", ["intresting"] = "interesting",
        ["knowlege"] = "knowledge", ["libary"] = "library", ["neccessary"] = "necessary", ["occured"] = "occurred",
        ["occurence"] = "occurrence", ["oppurtunity"] = "opportunity", ["peice"] = "piece", ["percieve"] = "perceive",
        ["proably"] = "probably", ["probaly"] = "probably", ["priviledge"] = "privilege", ["recieve"] = "receive",
        ["recieved"] = "received", ["reccomend"] = "recommend", ["refered"] = "referred", ["relevent"] = "relevant",
        ["remeber"] = "remember", ["restaraunt"] = "restaurant", ["seperate"] = "separate", ["similiar"] = "similar",
        ["succesful"] = "successful", ["suprise"] = "surprise", ["teh"] = "the", ["thier"] = "their",
        ["tommorow"] = "tomorrow", ["tommorrow"] = "tomorrow", ["tounge"] = "tongue", ["truely"] = "truly",
        ["unfortunatly"] = "unfortunately", ["untill"] = "until", ["usefull"] = "useful", ["wether"] = "whether",
        ["wierd"] = "weird", ["wich"] = "which", ["whitch"] = "which", ["writting"] = "writing", ["yuo"] = "you",
        ["yuor"] = "your", ["realy"] = "really", ["somthing"] = "something", ["anyting"] = "anything",
        ["everyting"] = "everything", ["nothign"] = "nothing", ["sucess"] = "success", ["reciept"] = "receipt",
        ["thankyou"] = "thank you", ["aswell"] = "as well", ["eachother"] = "each other", ["atleast"] = "at least",
        ["nevermind"] = "never mind", ["noone"] = "no one",
        ["tgis"] = "this", ["tgat"] = "that", ["taht"] = "that", ["thsi"] = "this",
        ["shoudl"] = "should", ["shoud"] = "should", ["atcually"] = "actually",
        ["actualy"] = "actually", ["becasue"] = "because", ["jsut"] = "just",
        ["wokr"] = "work", ["worlk"] = "work", ["lisetning"] = "listening",
        ["listenign"] = "listening", ["messsage"] = "message", ["proff"] = "proof",
        ["independnt"] = "independent", ["tge"] = "the", ["adn"] = "and",
        ["waht"] = "what", ["whta"] = "what",
        ["puncter"] = "punctuation", ["punctuaton"] = "punctuation", ["punctiation"] = "punctuation",
        ["punctuatong"] = "punctuation", ["puctuation"] = "punctuation",
        ["exapmle"] = "example", ["exmaple"] = "example",
        ["ddint"] = "didn't", ["purposful"] = "purposeful",
        ["activly"] = "actively", ["gramma"] = "grammar",
        ["instea"] = "instead", ["insted"] = "instead",
        ["shich"] = "which", ["godo"] = "good", ["hes"] = "he's",
        ["askss"] = "asks", ["thgink"] = "think", ["thgought"] = "thought",
        ["somethig"] = "something", ["hsi"] = "his",
        ["frmo"] = "from", ["wiht"] = "with", ["hwo"] = "how",
        ["wnat"] = "want", ["ahve"] = "have", ["hvae"] = "have",
        ["onyl"] = "only", ["nad"] = "and", ["thna"] = "than"
    };

    static readonly HashSet<string> Open = new(StringComparer.OrdinalIgnoreCase)
    {
        "a","an","the","to","for","of","and","or","but","if","when","because","so","as","at","in","on","from","by",
        "with","about","into","like","than","then","my","your","our","their","this","that","these","those","is","are",
        "was","were","be","been","being","have","has","had","do","does","did","not","just","very","really","too",
        "also","can","could","would","should","will","may","might","must","am","i","he","she","we","they","you",
        "although","though","while","whether","either","neither","nor","yet","still","even","rather","instead",
        "until","unless","after","before","since","plus","much","well","now","only","already","ever","never",
        "perhaps","maybe","kinda","onto","upon","via","despite","toward","towards","among","between","during",
        "without","within","across","along","around","against","besides","except","including","let","versus",
        "always","sometimes","often","here","there","how","who","whom","whose","which","what","why",
        "start","starts","started","starting","head","heading","headed","jump","jumps","jumped","jumping",
        "grab","grabs","grabbed","grabbing","come","comes","coming","go","goes","going","get","gets","getting",
        "keep","keeps","keeping","try","tries","trying","want","wants","need","needs","repeat","repeats",
        "over","off","out","up","down","back","away","again"
    };

    static readonly HashSet<string> Grammar = new(StringComparer.OrdinalIgnoreCase)
    {
        "is","are","was","were","be","am","has","have","had","do","does","did",
        "don't","doesn't","didn't","can't","won't","isn't","aren't","wasn't","weren't",
        "dont","doesnt","didnt","cant","wont","isnt","arent","wasnt","werent",
        "a","an"
    };

    static readonly HashSet<string> Small = new(StringComparer.OrdinalIgnoreCase)
    {
        "a","an","the","to","of","in","on","at","for"
    };

    static readonly HashSet<string> Verbs = new(StringComparer.OrdinalIgnoreCase)
    {
        "is","am","are","was","were","be","been","have","has","had","do","does","did","don't","doesn't","can't",
        "won't","go","goes","went","going","get","got","make","made","want","wanted","need","needed","see","saw",
        "know","knew","think","thought","say","said","like","liked","love","hate","come","came","take","took",
        "give","gave","find","found","tell","told","work","works","worked","try","tried","ask","asked","feel",
        "felt","become","became","leave","left","put","mean","keep","let","begin","seem","help","show","hear",
        "play","run","move","live","believe","hold","bring","happen","write","provide","sit","stand","lose","pay",
        "meet","include","continue","set","learn","change","lead","understand","watch","follow","stop","create",
        "speak","read","spend","grow","open","walk","win","offer","remember","love","consider","appear","buy",
        "wait","serve","die","send","build","stay","fall","cut","reach","kill","remain","receive","received"
    };

    public static string? InstantWord(string word)
    {
        if (word.Length == 1 && word == "i") return "I";
        if (word is "id" or "ill" or "lets" or "hell") return null;
        if (Typos.TryGetValue(word, out var fix)) return Preserve(word, fix);
        return null;
    }

    public static bool LastTokenFix(string text, out string take, out string put)
    {
        take = "";
        put = "";
        var n = EndOfCompleted(text);
        if (n <= 0) return false;
        var head = text[..n];
        var i = n - 1;
        while (i >= 0 && IsBreak(head[i])) i--;
        if (i < 0) return false;
        var wordEnd = i + 1;
        while (i >= 0 && !IsBreak(head[i])) i--;
        var word = head[(i + 1)..wordEnd];
        var brk = head[wordEnd..];
        var fix = InstantWord(word);
        if (fix == null) return false;
        take = word + brk;
        put = fix + brk;
        return take != put;
    }

    public static string InstantCompleted(string text)
    {
        var n = EndOfCompleted(text);
        if (n <= 0) return text;
        return TidyPunct(InstantAll(text[..n])) + text[n..];
    }

    public static string TidyPunct(string text)
    {
        text = Regex.Replace(text, @"\s+([,.!?;:])", "$1");
        text = Regex.Replace(text, @"([,.!?;:])([A-Za-z])", "$1 $2");
        text = Regex.Replace(text, @"[ \t]{2,}", " ");
        return text;
    }

    public static string ModelSlice(string head)
    {
        var clause = CurrentClause(head).Trim();
        if (clause.Length <= 200) return clause;
        var cut = clause.Length - 160;
        while (cut < clause.Length && !char.IsWhiteSpace(clause[cut])) cut++;
        return clause[Math.Min(cut + 1, clause.Length)..].Trim();
    }

    public static int EndOfCompleted(string text)
    {
        if (text.Length == 0) return 0;
        if (IsBreak(text[^1])) return text.Length;
        var i = text.Length - 1;
        while (i >= 0 && !IsBreak(text[i])) i--;
        return Math.Max(0, i + 1);
    }

    public static string InstantAll(string text)
    {
        var sb = new System.Text.StringBuilder(text.Length + 8);
        var i = 0;
        while (i < text.Length)
        {
            if (IsWordChar(text[i]))
            {
                var s = i;
                while (i < text.Length && IsWordChar(text[i])) i++;
                var word = text[s..i];
                sb.Append(InstantWord(word) ?? word);
            }
            else sb.Append(text[i++]);
        }
        return InstantPhrase(sb.ToString());
    }

    public static string InstantPhrase(string text)
    {
        text = Regex.Replace(text, @"\b(should|would|could|might|must) of\b", "$1 have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bor sum\b", "or some", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it)\s+(don't)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "doesn't"), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(they|we|you)\s+(doesn't)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "don't"), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(I)\s+(are)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "am"), RegexOptions.IgnoreCase);
        return text;
    }

    public static bool InstantFix(string text, out string take, out string put)
    {
        take = "";
        put = "";
        var n = EndOfCompleted(text);
        if (n <= 0) return false;
        var head = text[..n];
        var rest = text[n..];
        var next = TidyPunct(InstantAll(head));
        if (next == head) return false;
        var same = 0;
        while (same < head.Length && same < next.Length && head[same] == next[same]) same++;
        take = head[same..] + rest;
        put = next[same..] + rest;
        return take != put;
    }

    public static bool ClosedThought(string text)
    {
        var clause = CurrentClause(text);
        var clean = Regex.Replace(clause.Trim(), @"\s+", " ");
        if (clean.Length < 28) return false;
        if (clean.EndsWith(',')) return false;
        if ((clean.Count(c => c is '"') & 1) == 1) return false;
        var words = WordsOf(clean);
        if (words.Count < 8) return false;
        var last = words[^1];
        if (Open.Contains(last)) return false;
        if (Verbs.Contains(last)) return false;
        if (last.EndsWith("ing", StringComparison.OrdinalIgnoreCase)) return false;
        if (last.EndsWith("ed", StringComparison.OrdinalIgnoreCase)) return false;
        return words.Any(w => Verbs.Contains(w) || w.EndsWith("ed", StringComparison.OrdinalIgnoreCase));
    }

    public static string GrammarPatch(string from, string to)
    {
        from = from.TrimEnd();
        to = (to ?? "").TrimEnd();
        if (from.Length == 0) return from;
        var fw = WordsOf(from);
        var tw = WordsOf(to);
        if (fw.Count == 0) return from;
        var mapped = new string[fw.Count];
        if (fw.Count == tw.Count)
        {
            for (var i = 0; i < fw.Count; i++)
                mapped[i] = SpellTwin(fw[i], tw[i]) ? KeepUserCase(fw[i], tw[i]) : fw[i];
        }
        else
        {
            var j = 0;
            for (var i = 0; i < fw.Count; i++)
            {
                mapped[i] = fw[i];
                if (j >= tw.Count) continue;
                if (SpellTwin(fw[i], tw[j]))
                {
                    mapped[i] = KeepUserCase(fw[i], tw[j]);
                    j++;
                    continue;
                }
                if (Small.Contains(tw[j]) && j + 1 < tw.Count && SpellTwin(fw[i], tw[j + 1]))
                {
                    mapped[i] = tw[j] + " " + KeepUserCase(fw[i], tw[j + 1]);
                    j += 2;
                    continue;
                }
                if (j + 1 < tw.Count && SpellTwin(fw[i], tw[j + 1]))
                {
                    j++;
                    mapped[i] = KeepUserCase(fw[i], tw[j]);
                    j++;
                }
            }
        }
        var rebuilt = TidyPunct(InstantPhrase(SwapWords(from, mapped))).TrimEnd();
        var first = FirstLetter(from);
        if (fw.Count >= 4 && first >= 0 && char.IsUpper(from[first]))
            rebuilt = MatchStartCase(from, to, rebuilt);
        if (ClosedThought(from) && to.Length > 0 && ".!?".Contains(to[^1]))
        {
            var t = rebuilt.TrimEnd();
            if (t.Length > 0 && !".!?".Contains(t[^1]))
                rebuilt = t + to[^1];
        }
        return rebuilt;
    }

    public static string ApplyLastSpell(string text, string fromWord, string modeled)
    {
        if (fromWord.Length < 3) return text;
        var cand = WordsOf(TidyPunct(modeled ?? ""));
        if (cand.Count == 0) return text;
        string? hit = null;
        foreach (var w in cand)
        {
            if (!SpellTwin(fromWord, w)) continue;
            hit = w;
            break;
        }
        if (hit == null) return text;
        var pick = KeepUserCase(fromWord, hit);
        if (pick.Equals(fromWord, StringComparison.Ordinal)) return text;
        return ReplaceLastWord(text, fromWord, pick);
    }

    public static string CurrentClause(string buffer)
    {
        var cut = Math.Max(buffer.LastIndexOfAny(['.', '!', '?']), -1);
        return buffer[(cut + 1)..];
    }

    public static bool Conservative(string from, string to)
    {
        from = from.Trim(); to = to.Trim();
        if (from.Length == 0 || to.Length == 0 || from.Equals(to, StringComparison.Ordinal)) return false;
        if (to.Length > from.Length + Math.Max(12, from.Length / 3)) return false;
        if (from.Length > to.Length + Math.Max(12, from.Length / 3)) return false;
        var a = WordsOf(from);
        var b = WordsOf(to);
        if (a.Count >= 3 && Math.Abs(a.Count - b.Count) > 2) return false;
        if (a.Count >= 3)
        {
            var keep = a.Count(w => b.Any(x => x.Equals(w, StringComparison.OrdinalIgnoreCase)));
            if (keep < Math.Max(2, (a.Count + 1) / 2)) return false;
        }
        if (to.Count(c => c is '!') > from.Count(c => c is '!')) return false;
        return true;
    }

    public static string EnsurePeriod(string text)
    {
        text = text.TrimEnd();
        return text.Length == 0 || ".!?".Contains(text[^1]) ? text : text + ".";
    }

    public static bool CloseIfReady(string text, out string next)
    {
        next = text;
        var n = EndOfCompleted(text);
        if (n < 8) return false;
        var head = text[..n];
        var tail = text[n..];
        if (!ClosedThought(head)) return false;
        var clause = CurrentClause(head);
        var trimmed = clause.Trim();
        if (trimmed.Length == 0 || ".!?".Contains(trimmed[^1])) return false;
        var prefix = head.Length >= clause.Length ? head[..^clause.Length] : "";
        var lead = clause[..(clause.Length - clause.TrimStart().Length)];
        var nextHead = prefix + lead + EnsurePeriod(trimmed) + " ";
        if (nextHead == head) return false;
        next = nextHead + tail;
        return true;
    }

    static string Preserve(string src, string dst)
    {
        if (src.Length > 1 && src.All(char.IsUpper)) return dst.ToUpperInvariant();
        if (char.IsUpper(src[0])) return char.ToUpperInvariant(dst[0]) + dst[1..];
        return dst;
    }

    static string KeepUserCase(string src, string dst)
    {
        if (src.Equals(dst, StringComparison.OrdinalIgnoreCase)) return src;
        return Preserve(src, dst);
    }

    static List<string> WordsOf(string text)
    {
        var words = new List<string>();
        var i = 0;
        while (i < text.Length)
        {
            if (!IsWordChar(text[i])) { i++; continue; }
            var s = i;
            while (i < text.Length && IsWordChar(text[i])) i++;
            words.Add(text[s..i]);
        }
        return words;
    }

    public static int WordCount(string text) => WordsOf(text).Count;

    public static string LastWord(string text)
    {
        var t = text.TrimEnd();
        if (t.Length == 0) return "";
        var i = t.Length - 1;
        while (i >= 0 && !IsBreak(t[i])) i--;
        return t[(i + 1)..];
    }

    public static string? SelfCheck()
    {
        if (InstantWord("tgis") != "this") return "tgis";
        if (InstantWord("hsi") != "his") return "hsi";
        if (InstantWord("dont") != "don't") return "dont";
        if (InstantWord("godo") != "good") return "godo";
        if (InstantWord("shich") != "which") return "shich";
        if (InstantCompleted("she dont ") != "she doesn't ") return "she dont";
        if (!InstantCompleted("she dont like apples ").Contains("doesn't")) return "she dont like";
        if (!InstantCompleted("or sum ").Contains("or some")) return "or sum";
        string Applied(string src)
        {
            if (!InstantFix(src, out var t, out var p)) return src;
            if (!src.EndsWith(t, StringComparison.Ordinal)) return src;
            return src[..^t.Length] + p;
        }
        if (Applied("she dont ") != "she doesn't ") return "fix she";
        if (!Applied("she dont like apples ").Contains("doesn't")) return "fix she like";
        if (!Applied("grabs hsi ").Contains("his")) return "fix hsi";
        if (!Applied("a godo ").Contains("good")) return "fix godo";
        if (!Applied("or sum ").Contains("or some")) return "fix sum";
        if (TidyPunct("cow .He") != "cow. He") return "tidy";
        if (ClosedThought("starts heading")) return "starts";
        if (ClosedThought("the quick brown fox jumped")) return "jumped";
        if (ClosedThought("and then he grabs hsi work stuff and starts")) return "starts clause";
        if (!ClosedThought("the fox then made the work stuff quickly")) return "should close";
        if (!CloseIfReady("the fox then made the work stuff quickly ", out var closed) || !closed.Contains('.')) return "close period";
        var patched = GrammarPatch("she don't like apples", "She doesn't like apples.");
        if (!patched.Contains("doesn't", StringComparison.OrdinalIgnoreCase)) return "patch dont";
        if (patched.StartsWith("She")) return "patch case";
        var fox = GrammarPatch("have a godo time", "have a good time");
        if (!fox.Contains("good")) return "patch godo";
        var cap = GrammarPatch("She dont like apples", "She doesn't like apples.");
        if (!cap.StartsWith("She") || !cap.Contains("doesn't", StringComparison.OrdinalIgnoreCase)) return "patch cap";
        return null;
    }

    static bool IsWordChar(char c) => char.IsLetter(c) || c is '\'' or '’';

    public static bool IsBreak(char c) => char.IsWhiteSpace(c) || ".!?,;:".Contains(c);

    static string ReplaceLast(string text, string from, string to)
    {
        var i = text.LastIndexOf(from, StringComparison.Ordinal);
        return i < 0 ? text : text[..i] + to + text[(i + from.Length)..];
    }

    static string SwapWords(string from, string[] neu)
    {
        var sb = new System.Text.StringBuilder(from.Length + 8);
        var i = 0;
        var w = 0;
        while (i < from.Length)
        {
            if (IsWordChar(from[i]))
            {
                while (i < from.Length && IsWordChar(from[i])) i++;
                if (w < neu.Length) sb.Append(neu[w++]);
            }
            else sb.Append(from[i++]);
        }
        return sb.ToString();
    }

    static string MatchStartCase(string from, string to, string rebuilt)
    {
        var fi = FirstLetter(from);
        var ti = FirstLetter(to);
        var ri = FirstLetter(rebuilt);
        if (fi < 0 || ti < 0 || ri < 0) return rebuilt;
        if (char.IsUpper(to[ti]) && char.IsLower(rebuilt[ri]) && char.ToLowerInvariant(to[ti]) == char.ToLowerInvariant(rebuilt[ri]))
            return rebuilt[..ri] + char.ToUpperInvariant(rebuilt[ri]) + rebuilt[(ri + 1)..];
        return rebuilt;
    }

    static int FirstLetter(string text)
    {
        for (var i = 0; i < text.Length; i++)
            if (char.IsLetter(text[i])) return i;
        return -1;
    }

    static bool GrammarTwin(string a, string b)
    {
        if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;
        var na = a.Replace("'", "").Replace("’", "");
        var nb = b.Replace("'", "").Replace("’", "");
        if (na.Equals(nb, StringComparison.OrdinalIgnoreCase)) return true;
        if (Grammar.Contains(na) && Grammar.Contains(nb)) return true;
        if (na.Length < 4 || nb.Length < 4) return false;
        if (Math.Abs(na.Length - nb.Length) > 2) return false;
        var d = Dist(na.ToLowerInvariant(), nb.ToLowerInvariant());
        if (d <= 1) return true;
        return d <= 2 && Math.Min(na.Length, nb.Length) >= 6;
    }

    static bool SpellTwin(string a, string b)
    {
        if (GrammarTwin(a, b)) return true;
        var na = a.Replace("'", "").Replace("’", "").ToLowerInvariant();
        var nb = b.Replace("'", "").Replace("’", "").ToLowerInvariant();
        if (na.Length < 4 || nb.Length < 4) return false;
        if (Math.Abs(na.Length - nb.Length) > 4) return false;
        var d = Dist(na, nb);
        var maxD = Math.Max(2, Math.Min(na.Length, nb.Length) / 3);
        if (d <= maxD) return true;
        var ca = Cons(na);
        var cb = Cons(nb);
        if (ca.Length >= 3 && Dist(ca, cb) <= 1 && Math.Abs(na.Length - nb.Length) <= 3) return true;
        return Overlap(na, nb) >= 0.78 && d <= 4;
    }

    static string Cons(string s)
    {
        var sb = new System.Text.StringBuilder(s.Length);
        foreach (var c in s)
            if (c is not 'a' and not 'e' and not 'i' and not 'o' and not 'u' and not 'y')
                sb.Append(c);
        return sb.ToString();
    }

    static double Overlap(string a, string b)
    {
        var bag = new int[128];
        foreach (var c in a)
            if (c < 128) bag[c]++;
        var hit = 0;
        foreach (var c in b)
        {
            if (c < 128 && bag[c] > 0)
            {
                bag[c]--;
                hit++;
            }
        }
        return hit / (double)Math.Max(a.Length, b.Length);
    }

    static string ReplaceLastWord(string text, string from, string to)
    {
        var t = text.TrimEnd();
        var i = t.Length - 1;
        while (i >= 0 && IsBreak(t[i])) i--;
        var end = i + 1;
        while (i >= 0 && !IsBreak(t[i])) i--;
        var word = t[(i + 1)..end];
        if (!word.Equals(from, StringComparison.Ordinal)) return text;
        return t[..(i + 1)] + to + text[end..];
    }

    static int Dist(string a, string b)
    {
        var m = new int[a.Length + 1, b.Length + 1];
        for (var i = 0; i <= a.Length; i++) m[i, 0] = i;
        for (var j = 0; j <= b.Length; j++) m[0, j] = j;
        for (var i = 1; i <= a.Length; i++)
            for (var j = 1; j <= b.Length; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + cost);
            }
        return m[a.Length, b.Length];
    }
}
