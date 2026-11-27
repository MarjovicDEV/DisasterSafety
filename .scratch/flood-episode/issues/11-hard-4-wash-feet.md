# 11: Hard 4 washing feet, at `evacuation_area`

**What to build:** Flood Hard 4 is a "during" Scenario at `evacuation_area`: the Student's feet got wet in floodwater on the way. With no layer to fade in, the Student plays the surprised pose and the Action Bar slides up. The four Choices are "Wash with clean water and tell Mom" (Safe: the Student walks to the basin, plays the new `wash` pose, pouring clean water over the feet with the dipper, then walks to Mom and calls "Mom, my feet got wet."), "Wipe them on your shirt" (the Student plays `crouch` and stops), "Keep playing" (the Student starts to run toward the open side and stops) and "Let them dry" (the Student plays `sit` alone on the edge of the mat and stops) (FL-18). There is no soap or doctor line (the GAP row). This ticket adds the `wash` pose for the boy and the girl, the basin and the dipper. Each Choice gets its own `evacuation_area` action key and icon.

**Blocked by:** 05 (the Hard text). Art Batch 6 (after the flood) must be approved and imported.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Hard 4 offers four Choices with its approved ids and Hint, and cites FL-18.
- PlayMode: the Safe Choice completes Hard 4 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and Mom to their start.
- Every Earthquake and Typhoon `evacuation_area` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/11-hard-4-wash-feet/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
