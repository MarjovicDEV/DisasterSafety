# 09: Hard 3 the new `creek` Setting

**What to build:** The new `creek` Setting: a high, fast creek with moving Floodwater, a grassy bank, stones and a path home away from it. It is a known Setting that cannot shake. Flood Hard 3 is a "during" Scenario there: the rain has stopped, so no Rain layer fades in; the creek's moving water runs from the first frame, the Student plays the surprised pose and the Action Bar slides up. The four Choices are "Walk home, away from the creek" (Safe: the Student walks along the path to Mom, who waits on the path away from the creek), "Swim in the creek" (the Student walks toward the bank and stops well back from it), "Fish at the edge" (the Student plays `crouch` at the top of the bank and stops) and "Throw stones from the bank" (the Student plays `reach` and stops) (FL-12). The Feedback says to stay away from the creek and never says it keeps rising after the rain (the GAP row). Each Choice gets its own `creek` action key and icon.

**Blocked by:** 05 (the Hard text) and 07 (moving Floodwater). Art Batch 4 (creek) must be approved and imported.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `ScenarioTests` check that `creek` is a known Setting and is not in `CanShake`.
- Content tests: Hard 3 offers four Choices with its approved ids and Hint, and cites FL-12.
- PlayMode: the Safe Choice completes Hard 3 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and Mom to their start. The creek's ripples stop on Pause and continue on Resume.
- The `creek` placements are added to `ArtSource/README-rollout.md` beside the other Settings.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/09-hard-3-creek/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
