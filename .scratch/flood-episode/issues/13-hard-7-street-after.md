# 13: Hard 7 stay home, at `street_after`

**What to build:** Flood Hard 7 is an "after" Scenario at the Fire `street_after`, with mud and leftover still Floodwater on the street from the first frame. The Caption says the water went down and friends want to see the flooded part of town; the friends are not drawn. It starts still and shows the Action Bar after 1 second. The four Choices are "Stay home with Mom" (Safe: the Student walks to Mom at the front door and stands with her), "Go see the flooded street" (the Student walks toward the far side of the street and stops), "Play in the mud" (the Student walks toward the mud and stops before it) and "Pick things out of the water" (the Student walks toward the leftover water and stops before its edge) (FL-17, FL-10). Each Choice gets its own `street_after` action key and icon. Hard 7 is the last Flood Level.

**Blocked by:** 06 (still Floodwater) and 12 (the mud layer). Art Batch 6 (after the flood) must be approved and imported, and Fire ticket 12 (`street_after`) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Content tests: Hard 7 offers four Choices with its approved ids and Hint, and a Level-exists test finds all fifteen Flood Levels.
- PlayMode: the Safe Choice completes Hard 7 with "I'm safe!". Each Unsafe Choice shows Feedback, and Try Again returns the Student and Mom to their start.
- Flood Hard 7's Level Completed shows no Next Episode while Landslide has no Playable Level.
- Every Fire `street_after` Level plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/13-hard-7-street-after/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
