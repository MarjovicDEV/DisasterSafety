# 09: Landslide APK check

**What to build:** One Android test APK with every Landslide Level, the Landslide menus and the end of the game, for the user to play on the phone. The user plays Landslide Easy, Normal and Hard and reaches the Safety Reminder on a 2400 x 1080 phone, and reports anything that reads badly at phone size: the Slope, the crack, the muddy spring, Fallen Soil, Muddy Runoff, the far slide, the LANDSLIDE ZONE sign, the Tanod's and the Teacher's bubbles, the Caption, Hint and Pause, and the Safety Reminder text.

**Blocked by:** 01, 02, 03, 04, 05, 06, 07 and 08.

**Status:** ready-for-agent

Read the [Landslide specification](../../../docs/specs/landslide-episode.md) and the [project rules](../../../AGENTS.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass on the final content.
- `BuildAndroidBatch` writes `Builds/DisasterSafety-pilot.apk` (ignored by git), and the build log shows no errors.
- Every Earthquake, Fire, Typhoon and Flood screenshot stays byte-identical to its last approved version.
- The user played the APK on the phone; every issue they reported is fixed in its own ticket or recorded as an open question in the Landslide design.
- Evidence (logs, screenshots, the user's notes) is stored in `D:/GameDEV/landslide-episode-scratch/09-landslide-apk-check/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json`, the APK and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
