# 05: Hard 1 Red Rainfall Warning, in `home`

**What to build:** Flood Hard 1 is a "before" Scenario in `home`: Rain and Dark Sky in the window, the window flood view with the water higher than in Normal 2 but still outside, and the TV showing the Red Rainfall Warning screen from the first frame. The Caption tells what the warning says; no warning bubble plays. The bag and the workbook are already on the high shelf. It starts still and shows the Action Bar after 1 second. The four Choices are "Leave now with Mom, Dad and the Go Bag" (Safe: the Student walks to Mom, in `mom_walk_gobag`, and Dad by the front door and stands with them; "All ready!"), "Wait until the water comes in" (the Student plays `sit` on the floor by the sofa and stops), "Go back for your toy" (the Student walks toward the toy and stops) and "Finish your game" (the Student plays `phone` and stops) (FL-08, TY-09, TY-51). Each Choice gets its own `home` action key and icon. This ticket also drafts the text of all seven Hard Levels for the user.

**Blocked by:** 01. Art Batch 2 (home and kitchen) must be approved and imported.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- The drafted text for Hard 1 to Hard 7 is in the evidence folder with a finding ID on every safety line. The GAP rows are worded as the specification says (Hard 3 never says the creek keeps rising after the rain; Hard 4 has no soap or doctor line). The user approved it before the Hard 1 Scenario file was committed.
- Content tests: Hard 1 offers four Choices with its approved ids and Hint.
- PlayMode: the Safe Choice completes Hard 1 with "All ready!". Each Unsafe Choice shows Feedback, and Try Again returns the Student, Mom and Dad to their start.
- The TV's "RED", "RAINFALL" and "WARNING" text is readable at 1920 x 1080 and 2400 x 1080.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/05-hard-1-red-rainfall-warning/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
