# 12: Hard 5 wet bread and Hard 6 power after the flood, in `home_after`

**What to build:** Flood Hard 5 and Hard 6 are "after" Scenarios in `home_after` with the mud layer on the floor from the first frame. Both start still and show the Action Bar after 1 second. In Hard 5 the bread in the low cupboard got wet. The four Choices are "Give it to Mom" (Safe: the Student plays `crouch`, picks up the wet bread, walks to Mom and plays `reach`, and Mom takes it), "Eat it" (the Student picks up the bread and stops; no eating is shown), "Wipe it and eat it" (the Student picks it up and stops at a different x) and "Feed it to the cat" (the Student walks toward the cat and stops) (FL-14). In Hard 6 the power is off, the floor is wet and the Student wants to watch TV. The four Choices are "Wait for the electrician's check" (Safe: the Student walks to Mom and sits beside her), "Turn on the TV" (the Student walks toward the TV, plays `reach` and stops short), "Plug in the fan" (the Student walks toward the outlet and stops far from it) and "Flip the switch box" (the Student walks toward the switch box, plays `reach-up` and stops short) (FL-15). This ticket adds the mud layer, the wet bread and the low cupboard, the TV off and the switch box states, and the cat (`street_cat` if it fits indoors, otherwise the Batch 6 cat). Each Choice gets its own `home_after` action key and icon.

**Blocked by:** 05 (the Hard text). Art Batch 6 (after the flood) must be approved and imported.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Hard 5 and Hard 6 each offer four Choices with their approved ids and Hints.
- PlayMode: each Safe Choice completes its Level with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student, Mom and the bread to their start.
- In Hard 6 no hand reaches the TV, the outlet or the switch box, and the TV stays off in every Act-out.
- Every Earthquake `home_after` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/12-hard-5-6-home-after/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
