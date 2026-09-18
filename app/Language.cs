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
        ["onyl"] = "only", ["nad"] = "and", ["thna"] = "than",
        ["wat"] = "what", ["wut"] = "what", ["nite"] = "night", ["thru"] = "through",
        ["ur"] = "your", ["agian"] = "again", ["alittle"] = "a little",
        ["infront"] = "in front", ["reccomended"] = "recommended",
        ["occassion"] = "occasion", ["whereever"] = "wherever",
        ["succed"] = "succeed", ["publically"] = "publicly",
        ["thign"] = "thing", ["thigns"] = "things", ["becuz"] = "because",
        ["whihc"] = "which", ["thansk"] = "thanks", ["pls"] = "please",
        ["ppl"] = "people", ["msg"] = "message", ["tomorow"] = "tomorrow",
        ["yesturday"] = "yesterday", ["woulda"] = "would have", ["coulda"] = "could have",
        ["shoulda"] = "should have", ["mighta"] = "might have", ["musta"] = "must have",
        ["anywheres"] = "anywhere", ["somewheres"] = "somewhere", ["nowheres"] = "nowhere",
        ["irregardless"] = "regardless", ["happend"] = "happened",
        ["geting"] = "getting", ["runing"] = "running", ["stoped"] = "stopped",
        ["useing"] = "using", ["makeing"] = "making", ["comeing"] = "coming",
        ["leaveing"] = "leaving", ["changeing"] = "changing",
        ["occassionally"] = "occasionally", ["embarassed"] = "embarrassed",
        ["accomodation"] = "accommodation", ["independance"] = "independence",
        ["seperated"] = "separated", ["beleived"] = "believed",
        ["excercise"] = "exercise", ["reccomendation"] = "recommendation",
        ["seperately"] = "separately", ["occassional"] = "occasional",
        ["supposably"] = "supposedly", ["expresso"] = "espresso",
        ["mischievious"] = "mischievous", ["relized"] = "realized",
        ["aint"] = "ain't", ["everytime"] = "every time",
        ["incase"] = "in case", ["inspite"] = "in spite",
        ["lemme"] = "let me", ["gimme"] = "give me",
        ["outta"] = "out of", ["lotta"] = "a lot of", ["dunno"] = "don't know"
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

    static readonly Dictionary<string, int> Flex = new(StringComparer.OrdinalIgnoreCase)
    {
        ["to"] = 1, ["too"] = 1,
        ["a"] = 2, ["an"] = 2,
        ["is"] = 3, ["are"] = 3, ["was"] = 3, ["were"] = 3, ["am"] = 3, ["be"] = 3, ["been"] = 3, ["being"] = 3,
        ["has"] = 4, ["have"] = 4, ["had"] = 4,
        ["do"] = 5, ["does"] = 5, ["did"] = 5,
        ["go"] = 6, ["goes"] = 6, ["went"] = 6, ["going"] = 6, ["gone"] = 6,
        ["see"] = 7, ["sees"] = 7, ["saw"] = 7, ["seen"] = 7, ["seeing"] = 7,
        ["give"] = 8, ["gives"] = 8, ["gave"] = 8, ["given"] = 8,
        ["take"] = 9, ["takes"] = 9, ["took"] = 9, ["taken"] = 9,
        ["come"] = 10, ["comes"] = 10, ["came"] = 10, ["coming"] = 10,
        ["get"] = 11, ["gets"] = 11, ["got"] = 11, ["getting"] = 11,
        ["make"] = 12, ["makes"] = 12, ["made"] = 12,
        ["say"] = 13, ["says"] = 13, ["said"] = 13,
        ["know"] = 14, ["knows"] = 14, ["knew"] = 14, ["known"] = 14,
        ["think"] = 15, ["thinks"] = 15, ["thought"] = 15,
        ["your"] = 16, ["you're"] = 16, ["youre"] = 16,
        ["their"] = 17, ["they're"] = 17, ["theyre"] = 17, ["there"] = 17,
        ["its"] = 18, ["it's"] = 18,
        ["who"] = 19, ["whom"] = 19,
        ["then"] = 20, ["than"] = 20,
        ["lose"] = 21, ["loose"] = 21,
        ["whose"] = 22, ["who's"] = 22, ["whos"] = 22,
        ["nothing"] = 23, ["anything"] = 23
    };

    static readonly Dictionary<string, string> DidntBare = new(StringComparer.OrdinalIgnoreCase)
    {
        ["saw"] = "see", ["went"] = "go", ["gone"] = "go", ["ate"] = "eat", ["did"] = "do",
        ["got"] = "get", ["came"] = "come", ["took"] = "take", ["ran"] = "run", ["wrote"] = "write",
        ["knew"] = "know", ["made"] = "make", ["told"] = "tell", ["left"] = "leave", ["felt"] = "feel",
        ["found"] = "find", ["heard"] = "hear", ["bought"] = "buy", ["thought"] = "think", ["said"] = "say",
        ["gave"] = "give", ["seen"] = "see", ["done"] = "do", ["taken"] = "take", ["broke"] = "break",
        ["chose"] = "choose", ["drove"] = "drive", ["spoke"] = "speak", ["lost"] = "lose", ["met"] = "meet",
        ["paid"] = "pay", ["slept"] = "sleep", ["sold"] = "sell", ["spent"] = "spend", ["taught"] = "teach",
        ["threw"] = "throw", ["woke"] = "wake", ["won"] = "win", ["wore"] = "wear", ["began"] = "begin"
    };

    static readonly Dictionary<string, string> HavePast = new(StringComparer.OrdinalIgnoreCase)
    {
        ["went"] = "gone", ["came"] = "come", ["did"] = "done", ["saw"] = "seen", ["took"] = "taken",
        ["ate"] = "eaten", ["ran"] = "run", ["wrote"] = "written", ["gave"] = "given", ["got"] = "gotten",
        ["spoke"] = "spoken", ["broke"] = "broken", ["chose"] = "chosen", ["drove"] = "driven",
        ["knew"] = "known", ["swore"] = "sworn", ["wore"] = "worn", ["tore"] = "torn"
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
        text = Regex.Replace(text, @",{2,}", ",");
        text = Regex.Replace(text, @"(?<!\.)\.\.(?!\.)", ".");
        text = Regex.Replace(text, @"\.\s+\.", ".");
        text = Regex.Replace(text, @"[ \t]{2,}", " ");
        text = Regex.Replace(text, @"([.!?])\s+([a-z])", m =>
            m.Groups[1].Value + " " + char.ToUpperInvariant(m.Groups[2].Value[0]) + m.Groups[2].Value[1..]);
        return text;
    }

    public static string ModelSlice(string head)
    {
        var clause = CurrentClause(head).Trim();
        if (clause.Length <= 400) return clause;
        var cut = 400;
        while (cut > 280 && cut < clause.Length && !char.IsWhiteSpace(clause[cut])) cut--;
        return clause[..Math.Clamp(cut, 1, clause.Length)].Trim();
    }

    public static string PatchClauses(string head, Func<string, string> revise)
    {
        if (head.Length < 3) return head;
        var sb = new System.Text.StringBuilder(head.Length + 24);
        var start = 0;
        for (var i = 0; i < head.Length; i++)
        {
            if (head[i] is not '.' and not '!' and not '?') continue;
            sb.Append(PatchPiece(head[start..i], revise, false));
            sb.Append(head[i]);
            start = i + 1;
        }
        if (start < head.Length)
            sb.Append(PatchPiece(head[start..], revise, true));
        return TidyPunct(sb.ToString());
    }

    static string PatchPiece(string piece, Func<string, string> revise, bool keepStop)
    {
        var trim = piece.Trim();
        if (trim.Length < 3 || WordCount(trim) < 1) return piece;
        if (piece.Length <= 400)
            return ApplyModel(piece, revise, keepStop);
        var cut = SplitClause(piece);
        return ApplyModel(piece[..cut], revise, true) + ApplyModel(piece[cut..], revise, keepStop);
    }

    static int SplitClause(string piece)
    {
        var mid = piece.Length / 2;
        var marks = new[] { ". ", "? ", "! ", "; ", ", ", " but ", " and ", " or ", " so " };
        var best = -1;
        var bestDist = int.MaxValue;
        foreach (var m in marks)
        {
            var i = 0;
            while (true)
            {
                var at = piece.IndexOf(m, i, StringComparison.OrdinalIgnoreCase);
                if (at < 0) break;
                var dist = Math.Abs(at - mid);
                if (dist < bestDist && at >= 40 && at <= piece.Length - 40)
                {
                    bestDist = dist;
                    best = at + m.Length;
                }
                i = at + m.Length;
            }
        }
        if (best > 0) return best;
        var cut = mid;
        while (cut < piece.Length && !char.IsWhiteSpace(piece[cut])) cut++;
        return Math.Clamp(cut, 1, piece.Length);
    }

    static string ApplyModel(string piece, Func<string, string> revise, bool keepStop)
    {
        var slice = ModelSlice(piece);
        if (slice.Length < 3) return piece;
        var last = LastWord(slice);
        var raw = revise(slice);
        var modeled = TidyPunct(string.IsNullOrWhiteSpace(raw) ? slice : raw);
        var patched = GrammarPatch(slice, modeled);
        if (last.Length >= 2 && LastWord(patched).Equals(last, StringComparison.Ordinal))
            patched = ApplyLastSpell(patched, last, revise(last));
        patched = patched.TrimEnd();
        if (!keepStop)
        {
            while (patched.Length > 0 && ".!?".Contains(patched[^1]))
                patched = patched[..^1].TrimEnd();
        }
        var idx = piece.LastIndexOf(slice, StringComparison.Ordinal);
        return idx < 0 ? patched : piece[..idx] + patched + piece[(idx + slice.Length)..];
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
        text = Regex.Replace(text, @"\b(shouldn't|wouldn't|couldn't|mustn't|mightn't) of\b", "$1 have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(should|would|could|might|must) not of\b", "$1 not have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhave never (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) && !m.Groups[1].Value.Equals("got", StringComparison.OrdinalIgnoreCase) ? "have never " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhave not (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) && !m.Groups[1].Value.Equals("got", StringComparison.OrdinalIgnoreCase) ? "have not " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bno one (\w+) (\w+) nothing\b", "no one $1 $2 anything", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bnobody (\w+) (\w+) nothing\b", "nobody $1 $2 anything", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bor sum\b", "or some", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it)\s+(don't)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "doesn't"), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(they|we|you)\s+(doesn't)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "don't"), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<!\band\s)(I)\s+(are)\b", m => m.Groups[1].Value + " " + Preserve(m.Groups[2].Value, "am"), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(your)\s+(the|a|an|so|too|very|really|not|welcome|going|coming|leaving|trying|running)\b", m => Preserve(m.Groups[1].Value, "you're") + " " + m.Groups[2].Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(its)\s+(a|an|going|coming|not|so|late|early)\b", m => Preserve(m.Groups[1].Value, "it's") + " " + m.Groups[2].Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(their)\s+(going|coming|running|leaving|trying|walking|talking|waiting|looking|sitting|eating|working|playing|driving|staying|watching|listening|thinking|helping|here|there|so|not)\b", m => Preserve(m.Groups[1].Value, "they're") + " " + m.Groups[2].Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bif I was (you|him|her|them)\b", m => (char.IsUpper(m.Value[0]) ? "If I were " : "if I were ") + m.Groups[1].Value.ToLowerInvariant(), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(didn't) (\w+)\b", m => DidntBare.TryGetValue(m.Groups[2].Value, out var v) ? m.Groups[1].Value + " " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(me and him|him and me)\s+(is|are)\b", "he and I are", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(me and her|her and me)\s+(is|are)\b", "she and I are", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(me and him|him and me) went\b", "he and I went", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(me and her|her and me) went\b", "she and I went", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(for|to|with|from) you and I\b", "$1 you and me", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bto (late|early|soon|bad|far)\b", "too $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(can't|won't|wouldn't|couldn't|shouldn't) never\b", "$1 ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhere's the (keys|people|guys|files|things)\b", "here are the $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bwhere's the (keys|people|guys|files|things)\b", "where are the $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthese kind of\b", "these kinds of", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthose kind of\b", "those kinds of", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|they|you) done\b", "$1 did", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\ban unique\b", "a unique", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bloose (my|your|his|her|our|their) (keys|mind|way|temper|job)\b", "lose $1 $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bin regards to\b", "in regard to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bwith regards to\b", "with regard to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(better|worse|more|less|rather|different|earlier|later|greater) then\b", "$1 than", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhelp with nothing\b", "help with anything", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhardly never\b", "hardly ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bsupposed to of\b", "supposed to have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bused to of\b", "used to have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bto have (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) && !m.Groups[1].Value.Equals("got", StringComparison.OrdinalIgnoreCase) ? "to have " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI gots\b", "I got", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) gots\b", "$1 has", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) gots\b", "$1 got", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bmore (better|faster|slower|stronger|weaker|worse|easier|harder)\b", "$1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\blets (go|see|do|get|eat|try|make|play|start|talk|hear|leave|stay)\b", m => "let's " + m.Groups[1].Value.ToLowerInvariant(), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bwhose (the|that|this|he|she|it|going|coming|leaving)\b", m => "who's " + m.Groups[1].Value.ToLowerInvariant(), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bwho's (book|car|house|bag|phone|idea|name|fault|dog|cat)\b", m => "whose " + m.Groups[1].Value.ToLowerInvariant(), RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\banyways\b", "anyway", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bno later then\b", "no later than", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bcase and point\b", "case in point", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bone in the same\b", "one and the same", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bless (people|friends|items|things|cars|days|hours)\b", "fewer $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bamount of (people|friends|persons|guys)\b", "number of $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI hasn't\b", "I haven't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) hasn't\b", "$1 haven't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) haven't\b", "$1 hasn't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthere's (two|three|four|five|six|seven|eight|nine|ten)\b", "there are $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthere's ([2-9]|\d{2,})\b", "there are $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bbe sure and\b", "be sure to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdifferent to\b", "different from", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(to|for|with|from) who\b", "$1 whom", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdon't got\b", "don't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdoesn't has\b", "doesn't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdon't has\b", "don't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it)\s+have\b", m => m.Groups[1].Value + " has", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(should|would|could|might|must|shouldn't|wouldn't|couldn't|mustn't) have (\w+)\b", m => HavePast.TryGetValue(m.Groups[2].Value, out var v) ? m.Groups[1].Value + " have " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhas went\b", "has gone", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhave went\b", "have gone", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhad of\b", "had", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhad (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) ? "had " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bwish I was\b", m => m.Value[0] == 'W' ? "Wish I were" : "wish I were", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bas if I was\b", m => m.Value[0] == 'A' ? "As if I were" : "as if I were", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bweather or not\b", "whether or not", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\beach of (them|us|you) are\b", "each of $1 is", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bone of the (\w+) are\b", "one of the $1 is", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthere's lots\b", "there are lots", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(don't|doesn't) have no\b", "$1 have any", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) wasn't\b", "$1 weren't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI ain't got\b", "I don't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) ain't got\b", "$1 doesn't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) ain't got\b", "$1 don't have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bsuppose to\b", "supposed to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\buse to\b", "used to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(I|we|they|you|he|she|it) seen\b", "$1 saw", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(you|we|they) is\b", "$1 are", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) are\b", "$1 is", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI is\b", "I am", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bloose weight\b", "lose weight", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\btry and\b", "try to", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bon accident\b", "by accident", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bfor all intensive purposes\b", "for all intents and purposes", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(don't|doesn't) (got|have) none\b", "$1 have any", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhave none\b", "have any", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdoesn't never\b", "doesn't ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdon't never\b", "don't ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bdidn't never\b", "didn't ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\ba (hour|honest|honor|honour|heir)\b", "an $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\ban (university|user|uniform|european|one)\b", "a $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthis things\b", "these things", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthese thing\b", "these things", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(don't|doesn't|didn't|can't|won't)\s+(\w+)\s+nothing\b", "$1 $2 anything", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI doesn't\b", "I don't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI has\b", "I have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|they|you) has\b", "$1 have", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) was\b", "$1 were", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI does\b", "I do", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|they|you) goes\b", "$1 go", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI goes\b", "I go", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<!\b(did|does|do|don't|didn't|let)\s)(he|she|it) do\b", "$2 does", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<!\bdid\s)(he|she|it) go (to|home|there|here|away|back|out)\b", "$1 goes $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI ain't never\b", "I'm never", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) ain't never\b", "$1 doesn't ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) ain't never\b", "$1 don't ever", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI ain't\b", "I'm not", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) ain't\b", "$1 isn't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|you|they) ain't\b", "$1 aren't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI been\b", "I have been", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(we|they|you) been\b", "$1 have been", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) been\b", "$1 has been", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI done\b", "I did", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(he|she|it) done\b", "$1 did", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bcould care less\b", "couldn't care less", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bbetween you and I\b", "between you and me", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthere's (many|several|few)\b", "there are $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bthe news are\b", "the news is", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bbased off of\b", "based on", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bbased off\b", "based on", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\boff of\b", "off", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\ball of the sudden\b", "all of a sudden", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bother then\b", "other than", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bquiet good\b", "quite good", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\baccept for\b", "except for", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\band than\b", "and then", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(that|this) don't\b", m => m.Groups[1].Value + " doesn't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(everybody|everyone|nobody|somebody|anybody|someone) don't\b", "$1 doesn't", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(everybody|everyone|nobody|somebody|anybody|someone) have\b", "$1 has", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(everybody|everyone|nobody|somebody|anybody|someone) are\b", "$1 is", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhaven't (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) && !m.Groups[1].Value.Equals("got", StringComparison.OrdinalIgnoreCase) ? "haven't " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhasn't (\w+)\b", m => HavePast.TryGetValue(m.Groups[1].Value, out var v) && !m.Groups[1].Value.Equals("got", StringComparison.OrdinalIgnoreCase) ? "hasn't " + v : m.Value, RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bnever seen\b", "never saw", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(me and them|them and me)\s+(is|are)\b", "they and I are", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bI says\b", "I say", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<!\b(did|does|do|don't|didn't|let)\s)(he|she|it) say\b", "$2 says", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(need|needs|needed|want|wants|buy|bought|get|got|bring|brought) (\w+) (\w+) (\w+) (\w+) and (\w+)\b", "$1 $2, $3, $4, $5, and $6", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(need|needs|needed|want|wants|buy|bought|get|got|bring|brought) (\w+) (\w+) (\w+) and (\w+)\b", "$1 $2, $3, $4, and $5", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(need|needs|needed|want|wants|buy|bought|get|got|bring|brought) (\w+) (\w+) and (\w+)\b", "$1 $2, $3, and $4", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(the \w+ \w+) (the \w+ \w+) and (the \w+ \w+)\b", "$1, $2, and $3", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(the (?:store|shop|house|office|school|park|mall|market|kitchen|table|room)|home|bed) I (bought|went|saw|got|slept|need|needed|want|wanted|think|thought|grabbed|took|found|left|called|started|sat)(?! was\b| were\b| is\b| are\b| had\b)\b", "$1. I $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<=\w{3,} )(today|yesterday) I (bought|went|saw|got|slept|need|needed|want|wanted|think|thought)\b", "$1. I $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(tired|hungry|ready) I (went|made|left|slept|need|wanted)\b", "$1. I $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(coming|going) I (already|just|still|never)\b", "$1. I $2", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"(?<=\w )(?<!\band )(?<!, )\bthen (I|he|she|we|they)\b", ". Then $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(yes|yeah|no|well|oh|okay|ok|wait|sorry|thanks|please|also|finally|next|lastly|honestly|actually) I\b", "$1, I", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\banyway I (?=\w+\s+\w)", "anyway, I ", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bfirst I\b", "first, I", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\bhowever I\b", "however, I", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\showever (he|she|we|they|it|you)\b", ". However, $1", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"\b(late|early) (we|I|they|you) (should|need|must|can|will)\b", "$1. $2 $3", RegexOptions.IgnoreCase);
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
                mapped[i] = ModelTwin(fw[i], tw[i]) ? KeepUserCase(fw[i], tw[i]) : fw[i];
        }
        else
        {
            var j = 0;
            for (var i = 0; i < fw.Count; i++)
            {
                mapped[i] = fw[i];
                if (j >= tw.Count) continue;
                if (ModelTwin(fw[i], tw[j]))
                {
                    mapped[i] = KeepUserCase(fw[i], tw[j]);
                    j++;
                    continue;
                }
                if (Small.Contains(tw[j]) && j + 1 < tw.Count && ModelTwin(fw[i], tw[j + 1]))
                {
                    mapped[i] = tw[j] + " " + KeepUserCase(fw[i], tw[j + 1]);
                    j += 2;
                    continue;
                }
                if (j + 1 < tw.Count && ModelTwin(fw[i], tw[j + 1]))
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
        if (NeedsStructure(rebuilt, to))
            rebuilt = MergeStructure(rebuilt, to);
        if (ClosedThought(from) && to.Length > 0 && ".!?".Contains(to[^1]))
        {
            var t = rebuilt.TrimEnd();
            if (t.Length > 0 && !".!?".Contains(t[^1]))
                rebuilt = t + to[^1];
        }
        return rebuilt;
    }

    static bool NeedsStructure(string from, string to)
    {
        static int Marks(string s) => s.Count(c => c is '.' or '!' or '?' or ',' or ';' or ':');
        return Marks(to) > Marks(from);
    }

    static bool GlueWord(string w) =>
        Small.Contains(w)
        || w.Equals("and", StringComparison.OrdinalIgnoreCase)
        || w.Equals("but", StringComparison.OrdinalIgnoreCase)
        || w.Equals("or", StringComparison.OrdinalIgnoreCase);

    static string MergeStructure(string from, string to)
    {
        var fw = WordsOf(from);
        if (fw.Count == 0) return from;
        var used = new bool[fw.Count];
        var sb = new System.Text.StringBuilder(to.Length + 8);
        var fi = 0;
        foreach (var (isWord, tok) in PunctTokens(to))
        {
            if (!isWord)
            {
                sb.Append(tok);
                continue;
            }
            var hit = -1;
            var until = Math.Min(fw.Count, fi + 5);
            for (var k = fi; k < until; k++)
            {
                if (used[k]) continue;
                if (!ModelTwin(fw[k], tok)) continue;
                hit = k;
                break;
            }
            if (hit < 0)
            {
                if (!GlueWord(tok)) return from;
                if (sb.Length > 0 && char.IsLetterOrDigit(sb[^1])) sb.Append(' ');
                sb.Append(tok);
                continue;
            }
            for (var g = fi; g < hit; g++)
            {
                if (used[g]) continue;
                if (!Small.Contains(fw[g])) continue;
                used[g] = true;
                if (sb.Length > 0 && char.IsLetterOrDigit(sb[^1])) sb.Append(' ');
                sb.Append(fw[g]);
            }
            used[hit] = true;
            fi = hit + 1;
            if (sb.Length > 0 && char.IsLetterOrDigit(sb[^1])) sb.Append(' ');
            sb.Append(KeepUserCase(fw[hit], tok));
        }
        var usedN = 0;
        for (var k = 0; k < used.Length; k++)
            if (used[k]) usedN++;
        if (usedN < Math.Max(1, (fw.Count * 3 + 3) / 4)) return from;
        for (var k = 0; k < fw.Count; k++)
        {
            if (used[k]) continue;
            if (fw[k].Any(char.IsDigit))
            {
                if (sb.Length > 0 && char.IsLetterOrDigit(sb[^1])) sb.Append(' ');
                sb.Append(fw[k]);
                continue;
            }
            if (GlueWord(fw[k]) || fw[k].Length <= 2) continue;
            if (sb.Length > 0 && char.IsLetterOrDigit(sb[^1])) sb.Append(' ');
            sb.Append(fw[k]);
        }
        return TidyPunct(sb.ToString()).TrimEnd();
    }

    static List<(bool word, string tok)> PunctTokens(string text)
    {
        var list = new List<(bool, string)>();
        var i = 0;
        while (i < text.Length)
        {
            if (IsWordChar(text[i]))
            {
                var s = i;
                while (i < text.Length && IsWordChar(text[i])) i++;
                list.Add((true, text[s..i]));
            }
            else if (".!?,;:".Contains(text[i]))
            {
                list.Add((false, text[i].ToString()));
                i++;
            }
            else i++;
        }
        return list;
    }

    public static string ApplyLastSpell(string text, string fromWord, string modeled)
    {
        if (fromWord.Length < 2) return text;
        var cand = WordsOf(TidyPunct(modeled ?? ""));
        if (cand.Count == 0) return text;
        string? hit = null;
        foreach (var w in cand)
        {
            if (!ModelTwin(fromWord, w)) continue;
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
        if (TidyPunct("home.. Then") != "home. Then") return "tidy dots";
        if (!InstantCompleted("first I went home then I slept ").Contains("Then I")) return "then I";
        if (InstantCompleted("first I went home then I slept ").Contains("..")) return "then dots";
        if (!InstantCompleted("I went to the store I bought milk ").Contains("store. I")) return "store split";
        if (!InstantCompleted("we need milk eggs bread cheese and juice ").Contains("milk, eggs, bread, cheese, and juice")) return "serial5";
        if (InstantCompleted("the house I saw was huge ").Contains("house. I")) return "rel house";
        if (!InstantCompleted("its late we should go ").Contains("late. We")) return "late split";
        if (!InstantCompleted("I want coffee tea and juice ").Contains("coffee, tea, and juice")) return "serial3";
        if (!GrammarPatch("oh, i forgot my bag at the school", "Oh, I forgot my bag at school.").Contains("the school")) return "keep the";
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
        if (!GrammarPatch("He go to school", "He goes to school").Contains("goes")) return "patch goes";
        if (!GrammarPatch("to much work", "too much work").Contains("too")) return "patch too";
        if (!GrammarPatch("Yesterday he go there", "Yesterday he went there").Contains("went")) return "patch went";
        if (GrammarPatch("your the best", "yours the best").Contains("yours")) return "patch yours";
        if (!InstantCompleted("your the best ").Contains("you're")) return "your the";
        if (!InstantCompleted("their going home ").Contains("they're")) return "their going";
        if (!InstantCompleted("if I was you ").Contains("were")) return "if i was";
        if (!InstantCompleted("me and him is going ").Contains("he and I are")) return "me and him";
        if (!InstantCompleted("better then this ").Contains("better than")) return "better then";
        if (!InstantCompleted("lets go ").Contains("let's go")) return "lets go";
        if (!InstantCompleted("she have went home ").Contains("has gone")) return "have went";
        if (!InstantCompleted("I are tired ").Contains("I am")) return "i are";
        if (!InstantCompleted("I should of went home ").Contains("should have gone")) return "should of went";
        if (InstantCompleted("on your right ").Contains("you're")) return "your right";
        if (!InstantCompleted("we was going ").Contains("were")) return "we was";
        if (!InstantCompleted("Your welcome ").Contains("You're welcome")) return "your welcome";
        if (!InstantCompleted("she dont got none ").Contains("doesn't have any")) return "dont got none";
        return null;
    }

    static bool IsWordChar(char c) => char.IsLetter(c) || char.IsDigit(c) || c is '\'' or '’';

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

    static bool ModelTwin(string a, string b)
    {
        var na = Bare(a);
        var nb = Bare(b);
        if (na.Length == 0 || nb.Length == 0) return false;
        if (FalsePossessive(na, nb)) return false;
        if (BlockApos(a, b)) return false;
        if (BlockTense(na, nb)) return false;
        if (BlockGrade(na, nb)) return false;
        if (BlockPerfect(na, nb)) return false;
        if (BlockBe(na, nb)) return false;
        if (BlockAspect(na, nb)) return false;
        if (BlockLy(na, nb)) return false;
        if (BlockSleep(na, nb)) return false;
        if (SpellTwin(a, b)) return true;
        if (Flex.TryGetValue(na, out var ga) && Flex.TryGetValue(nb, out var gb) && ga == gb) return true;
        if (Inflect(na, nb)) return true;
        var (shortw, longw) = na.Length <= nb.Length ? (na, nb) : (nb, na);
        return shortw.Length >= 4 && longw.StartsWith(shortw, StringComparison.Ordinal) && Dist(na, nb) <= 1;
    }

    static string Bare(string s) => s.Replace("'", "").Replace("’", "").ToLowerInvariant();

    static bool BlockTense(string a, string b)
    {
        if (Flex.TryGetValue(a, out var ga) && Flex.TryGetValue(b, out var gb) && ga == gb) return false;
        if (b.Length == a.Length + 1 && b == a + "d") return true;
        if (b.Length == a.Length + 2 && b == a + "ed") return true;
        if (a.Length == b.Length + 1 && a == b + "d") return true;
        if (a.Length == b.Length + 2 && a == b + "ed") return true;
        return false;
    }

    static bool BlockPerfect(string a, string b) =>
        (a, b) is ("has", "had") or ("had", "has") or ("have", "had") or ("had", "have");

    static readonly HashSet<string> BeNow = new(StringComparer.OrdinalIgnoreCase) { "is", "are", "am" };
    static readonly HashSet<string> BePast = new(StringComparer.OrdinalIgnoreCase) { "was", "were" };

    static bool BlockBe(string a, string b) =>
        (BeNow.Contains(a) && BePast.Contains(b)) || (BePast.Contains(a) && BeNow.Contains(b))
        || (a == "are" && b == "is");

    static bool BlockLy(string a, string b)
    {
        var (shortw, longw) = a.Length <= b.Length ? (a, b) : (b, a);
        return longw.Length == shortw.Length + 2 && longw == shortw + "ly";
    }

    static bool BlockSleep(string a, string b) =>
        (a, b) is ("slept", "asleep") or ("asleep", "slept") or ("sleep", "asleep") or ("asleep", "sleep");

    static bool BlockAspect(string a, string b)
    {
        if (!Flex.TryGetValue(a, out var ga) || !Flex.TryGetValue(b, out var gb) || ga != gb) return false;
        var ingA = a.EndsWith("ing", StringComparison.Ordinal);
        var ingB = b.EndsWith("ing", StringComparison.Ordinal);
        return ingA != ingB;
    }

    static bool BlockGrade(string a, string b) =>
        (a, b) is ("worse", "worst") or ("worst", "worse") or ("better", "best") or ("best", "better")
        or ("farther", "further") or ("further", "farther");

    static bool BlockApos(string a, string b)
    {
        var x = a.ToLowerInvariant();
        var y = b.ToLowerInvariant();
        return (x, y) is ("were", "we're") or ("we're", "were")
            or ("well", "we'll") or ("we'll", "well")
            or ("hell", "he'll") or ("he'll", "hell")
            or ("wed", "we'd") or ("we'd", "wed")
            or ("shed", "she'd") or ("she'd", "shed");
    }

    static bool FalsePossessive(string a, string b) =>
        (a, b) is ("your", "yours") or ("their", "theirs") or ("her", "hers") or ("our", "ours")
        or ("yours", "your") or ("theirs", "their") or ("hers", "her") or ("ours", "our");

    static bool Inflect(string a, string b)
    {
        if (a.Length < 2 || b.Length < 2) return false;
        var (shortw, longw) = a.Length <= b.Length ? (a, b) : (b, a);
        if (!longw.StartsWith(shortw, StringComparison.Ordinal)) return false;
        var tail = longw[shortw.Length..];
        return tail is "s" or "es" or "ing";
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
