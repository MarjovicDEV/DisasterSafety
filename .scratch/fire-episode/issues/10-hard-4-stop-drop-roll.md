# 10: Hard 4 Stop, Drop, and Roll near the siga

**What to build:** The new `yard` Setting with a siga (a small yard bonfire), and the `stop_drop_roll` pose (drop, cover the face with the hands, roll: 6 frames, boy and girl). The Student's sleeve shows a small flame. The Choices are "Stop, Drop, and Roll" (Safe: the flame goes out in the roll, and Feedback adds "Cool the burn with cool water and tell an adult"), "Run for help", "Beat it with your hands" and "Wave your arms".

**Blocked by:** 03.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- The flame on the sleeve is small, never spreads, and is gone once the roll ends. No Unsafe stop lets it grow.
- The Safe Choice completes the Level. Each Unsafe Choice resets on Try Again, with the sleeve flame back at its start.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/10-hard-4-stop-drop-roll/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
