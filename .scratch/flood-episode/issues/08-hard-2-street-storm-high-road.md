# 08: Hard 2 the high road, on `street_storm`

**What to build:** Flood Hard 2 is a "during" Scenario on the Typhoon `street_storm`: heavy Rain, Dark Sky and moving Floodwater on the low road ahead fade in, with the flooded bridge beyond. The Student is on the evacuation route with Dad. The four Choices are "Take the high road with Dad" (Safe: the Student walks with Dad along the near pavement to the EVACUATION ROUTE sign), "Cross the water" (the Student walks toward the moving water and stops before its edge), "Walk on the flooded bridge" (the Student walks toward the bridge and stops at the start of its path) and "Wade barefoot" (the Student plays `crouch` and stops) (FL-11, TY-52). Each Choice gets its own `street_storm` action key and icon, and the moving Floodwater from 07 is placed on this road.

**Blocked by:** 05 (the Hard text) and 07 (moving Floodwater). Art Batch 3 (outdoor Floodwater) must be approved and imported, and Typhoon ticket 08 (`street_storm`) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Hard 2 offers four Choices with its approved ids and Hint.
- PlayMode: the Safe Choice completes Hard 2 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and Dad to their start.
- The bridge stop is on dry ground before the bridge, and no actor stands on it.
- Every Typhoon `street_storm` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/08-hard-2-street-storm-high-road/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
