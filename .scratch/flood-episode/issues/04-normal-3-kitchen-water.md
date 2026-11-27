# 04: Normal 3 boiled water, in `kitchen`

**What to build:** Flood Normal 3 is a "during" Scenario in the Fire `kitchen`, with Rain in the kitchen window fading in over about 1.5 seconds and the rain loop under the music: the Student is thirsty. The three Choices are "Drink from the boiled-water jug" (Safe: the Student walks to the counter and plays `reach` for the jug and the cup), "Drink from the rain drum" (the Student walks toward the rain drum by the screen door and stops before it) and "Drink from the tap" (the Student walks to the sink, plays `reach` and stops short of the tap) (FL-13, TY-58). No one is shown drinking from the drum or the tap. The Feedback says boiled water is safe and never says tap water is unsafe (the GAP row). Each Choice gets its own `kitchen` action key and icon.

**Blocked by:** 01. Art Batch 2 (home and kitchen) must be approved and imported, and Fire ticket 06 (`kitchen`) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Normal 3 offers three Choices with its approved ids and Hint, and its Feedback cites only FL-13 and TY-58 for the water.
- PlayMode: the Safe Choice completes Normal 3 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and the jug and cup to their start.
- Every Fire `kitchen` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/04-normal-3-kitchen-water/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
