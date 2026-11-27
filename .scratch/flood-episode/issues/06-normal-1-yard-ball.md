# 06: Normal 1 ball in still Floodwater, in `yard`

**What to build:** Flood Normal 1 is a "during" Scenario in `yard`: heavy Rain, Dark Sky and still Floodwater over the low part of the yard fade in over about 1.5 seconds, with the ball floating on the water, then the Student plays the surprised pose and the Action Bar slides up. The three Choices are "Leave it and tell Mom" (Safe: the Student turns away from the water and calls "Mom, my ball!"; Mom walks in from the left and stands with the Student), "Wade in for the ball" (the Student walks toward the water and stops before its edge) and "Splash in the water" (the Student starts to run toward the water and stops at a different x, before its edge) (FL-10). Each Choice gets its own `yard` action key and icon. This ticket adds the still form of the Floodwater layer, with its slow sheen frames, and the floating ball.

**Blocked by:** 01. Art Batch 3 (outdoor Floodwater) must be approved and imported, and Fire ticket 10 (`yard`) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Normal 1 offers three Choices with its approved ids and Hint.
- PlayMode: the Safe Choice completes Normal 1 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and Mom to their start, with the still Floodwater and the ball as they were when the Action Bar showed. The still water's motion stops on Pause and continues on Resume.
- Every Fire `yard` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/06-normal-1-yard-ball/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
