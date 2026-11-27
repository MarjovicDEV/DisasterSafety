# 10: Easy 2 the canal, in `street`, and Easy 3 rainwater, in `yard`

**What to build:** Flood Easy 2 is a "before" Scenario in the Fire `street` with the canal grate: the Student has a snack wrapper beside the canal. The two Choices are "Put it in the trash bin" (Safe: the Student walks to the trash bin and plays `reach`, and the wrapper goes in; "All ready!") and the canal Choice (the Student walks to the canal grate, plays `crouch` and stops with the wrapper still in hand) (FL-05). The design's label "Drop it in the canal" fails the content test's "drop" rule, so the drafted Easy text from 03 gives it another label for the user to approve, such as "Throw it in the canal". Flood Easy 3 is an "after" Scenario in `yard`: the rain has stopped, and a can and an old tire hold rainwater. The two Choices are "Tip out the water" (Safe: the Student walks to the can and plays `crouch`, or `tip` if art Batch 5 drew it; the can tips and the water runs out, and Dad, in `dad_kneel`, tips the tire) and "Leave it" (the Student walks past the can and stops) (FL-19, TY-60). Both start still and show the Action Bar after 1 second. Each Choice gets its own action key and icon.

**Blocked by:** 03 (the Easy text). Art Batch 5 (Easy props) must be approved and imported, and Fire tickets 10 (`yard`) and 12 (`street`) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Easy 2 and Easy 3 each offer two Choices with their approved ids and Hints, and no label contains the word "drop".
- PlayMode: each Safe Choice completes its Level. Each Unsafe Choice shows Feedback, and Try Again returns the Student, Dad, the wrapper, the can and the tire to their start, with the water back in the can and the tire.
- The canal stop is on the pavement, and nothing goes into the canal.
- Every Fire `street` and `yard` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/10-easy-2-3-canal-and-rainwater/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
