# Writefix

A small always-on-top Windows menu that corrects spelling and grammar **in every app** you type in. No browser extension, no account, no API key.

Common typos fix on space. After a short pause, grammar and spelling are patched without replacing the whole sentence. A period is added only when the current clause looks finished — not after verbs like `starts` or `jumped`.

The word you are still typing is never rewritten. Each focus change starts a fresh buffer.

Download the standalone Windows exe from [Releases](https://github.com/toasta-xx/WriteFix/releases).

## Menu

- **Enabled everywhere** — master switch
- **Grammar, close thoughts, punctuate** — grammar after a pause, and a period on a finished thought
- Drag the card anywhere. Hide sends it to the tray.

## Notes

- Works by watching keystrokes and typing corrections back, so it works in Discord, browsers, editors, and most other Windows apps.
- If you type in an app that is running as Administrator, start Writefix as Administrator too.
- Windows may warn on an unsigned exe the first time. That is expected until you sign it.
- Logs: `%AppData%\Writefix\run.txt` and `error.txt`.
