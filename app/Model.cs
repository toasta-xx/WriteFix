using System.IO;
using System.Reflection;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Writefix;

public sealed class Model : IDisposable
{
    readonly InferenceSession _encoder;
    readonly InferenceSession _decoder;
    readonly SessionOptions _opt;
    readonly Tok _tok;
    readonly object _gate = new();
    public bool Ready { get; }

    public Model(Action<string, double>? progress = null)
    {
        void Step(string label, double pct) => progress?.Invoke(label, pct);
        var asm = Assembly.GetExecutingAssembly();
        Step("Reading tokenizer", 8);
        using var ts = new MemoryStream(Read(asm, "tokenizer.json"));
        Step("Building tokenizer", 22);
        _tok = new Tok(ts);
        _opt = new SessionOptions
        {
            GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
            IntraOpNumThreads = Math.Clamp(Environment.ProcessorCount / 2, 1, 4),
            InterOpNumThreads = 1
        };
        Step("Reading encoder", 40);
        var encoderBytes = Read(asm, "encoder.onnx");
        Step("Loading encoder", 52);
        _encoder = new InferenceSession(encoderBytes, _opt);
        Step("Reading decoder", 74);
        var decoderBytes = Read(asm, "decoder.onnx");
        Step("Loading decoder", 86);
        _decoder = new InferenceSession(decoderBytes, _opt);
        Step("Model ready", 100);
        Ready = true;
    }

    static byte[] Read(Assembly asm, string name)
    {
        using var s = asm.GetManifestResourceStream(name) ?? throw new InvalidOperationException(name);
        using var m = new MemoryStream();
        s.CopyTo(m);
        return m.ToArray();
    }

    public string Revise(string text)
    {
        lock (_gate)
        {
            var raw = text.Trim();
            if (raw.Length < 3) return raw;
            var ids = _tok.Encode("gec: " + raw).Select(i => (long)i).ToArray();
            if (ids.Length > 128)
                ids = [.. ids[..127], 1];
            var input = new DenseTensor<long>(ids, [1, ids.Length]);
            var mask = new DenseTensor<long>(Enumerable.Repeat(1L, ids.Length).ToArray(), [1, ids.Length]);
            using var encOut = _encoder.Run(
                new[]
                {
                    NamedOnnxValue.CreateFromTensor("input_ids", input),
                    NamedOnnxValue.CreateFromTensor("attention_mask", mask)
                },
                new[] { "last_hidden_state" });
            var src = encOut[0].AsTensor<float>();
            var hidden = new DenseTensor<float>(src.ToArray(), src.Dimensions.ToArray());
            var dec = new List<long> { 0 };
            var limit = Math.Min(128, Math.Max(16, ids.Length + 24));
            for (var step = 0; step < limit; step++)
            {
                var decTensor = new DenseTensor<long>(dec.ToArray(), [1, dec.Count]);
                using var result = _decoder.Run(
                    new[]
                    {
                        NamedOnnxValue.CreateFromTensor("input_ids", decTensor),
                        NamedOnnxValue.CreateFromTensor("encoder_attention_mask", mask),
                        NamedOnnxValue.CreateFromTensor("encoder_hidden_states", hidden)
                    },
                    new[] { "logits" });
                var logits = result[0].AsTensor<float>();
                var vocab = logits.Dimensions[2];
                var last = (dec.Count - 1) * vocab;
                var best = 0;
                var score = float.NegativeInfinity;
                for (var i = 0; i < vocab; i++)
                {
                    var v = logits.GetValue(last + i);
                    if (v > score) { score = v; best = i; }
                }
                if (best == 1) break;
                dec.Add(best);
            }
            var outText = _tok.Decode(dec.Skip(1).Select(i => (int)i));
            if (outText.StartsWith("gec:", StringComparison.OrdinalIgnoreCase))
                outText = outText[4..].TrimStart();
            return string.IsNullOrWhiteSpace(outText) ? raw : outText.Trim();
        }
    }

    public void Dispose()
    {
        lock (_gate)
        {
            _encoder.Dispose();
            _decoder.Dispose();
            _opt.Dispose();
        }
    }
}
