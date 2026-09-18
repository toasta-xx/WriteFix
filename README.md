# Writefix

<img src="app/brand/writefix.png" width="96" alt="Writefix" />

A small always-on-top Windows menu that corrects spelling and grammar **in every app** you type in. No browser extension, no account, no API key.

Common typos fix on space. After a short pause, a local T5 model patches grammar and spelling without replacing the whole sentence. A period is added only when the current clause looks finished — not after verbs like `starts` or `jumped`.

The word you are still typing is never rewritten. Each focus change starts a fresh buffer.

## Run

Double-click `dist\Writefix.exe` after you publish, or press F5 in Visual Studio on `Writefix.sln`.

No extra installs. The 60.5M grammar model is inside the exe.

## Menu

- **Enabled everywhere** — master switch
- **Grammar, close thoughts, punctuate** — T5 after a pause, and a period on a finished thought
- Drag the card anywhere. Hide sends it to the tray.

## Publish a single exe

```bat
publish.bat
```

That writes a self-contained `dist\Writefix.exe` (64-bit Windows). ONNX weights in `Models\` are embedded at build time.

Open `Writefix.sln` in Visual Studio 2022+ with the .NET desktop workload.

## Model

- [Unbabel/gec-t5_small](https://huggingface.co/Unbabel/gec-t5_small) (T5-small, 60.5M, Apache 2.0)
- ONNX INT8: [TonyRaju/gec-t5-small-coedit-onnx-int8](https://huggingface.co/TonyRaju/gec-t5-small-coedit-onnx-int8)

Aimed at 8GB CPU-only Windows laptops. No GPU required. Expect about 1GB of RAM while it is running.

## Notes

- Works by watching keystrokes and typing corrections back, so it works in Discord, browsers, editors, and most other Windows apps.
- If you type in an app that is running as Administrator, start Writefix as Administrator too.
- Windows may warn on an unsigned exe the first time. That is expected until you sign it.
- Logs: `%AppData%\Writefix\run.txt` and `error.txt`.
