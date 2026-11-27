# 07: Normal 4 moving Floodwater at the `schoolyard` gate

**What to build:** Flood Normal 4 is a "during" Scenario in `schoolyard`: heavy Rain, Dark Sky and moving Floodwater on the road past the gate fade in over about 1.5 seconds. Then the Teacher's Orange warning plays as a speech bubble with the voice blip, the only warning bubble in Flood (F4). Then the Student plays the surprised pose and the Action Bar slides up. The three Choices are "Wait with the Teacher under the roof" (Safe: the Student walks to the Teacher at the doorway under the roof and stands with the Teacher), "Cross the water" (the Student walks toward the moving water and stops before its edge) and "Take off your shoes and wade" (the Student walks toward it, plays `crouch` and stops at a different x) (TY-14, FL-11). Each Choice gets its own `schoolyard` action key and icon. This ticket adds the moving form of the Floodwater layer, whose ripple lines run with the current on their own frames.

**Blocked by:** 01. Art Batch 3 (outdoor Floodwater) must be approved and imported.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Normal 4 offers three Choices with its approved ids and Hint. The Teacher's Orange warning is a `UIStrings` line with its finding ID in a comment.
- PlayMode: the layers fade in before the Teacher's bubble, and the bubble plays before the Action Bar. The Safe Choice completes Normal 4 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and the Teacher to their start. The moving water's ripples stop on Pause and continue on Resume.
- Every Earthquake and Fire `schoolyard` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/07-normal-4-schoolyard-gate/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
