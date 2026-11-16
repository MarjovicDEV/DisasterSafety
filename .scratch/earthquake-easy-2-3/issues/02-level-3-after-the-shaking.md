# 02 Level 3 plays still, in the classroom after the shaking

Blocked by: 01. Blocks: 03.

Spec: `docs/specs/earthquake-easy-2-3.md` (behaviours 1 and 2 for `classroom_after`). Rules: `AGENTS.md`.

## Build

- Editor: in `ProjectBuilder.Level.cs`, add the `classroom_after` setting group with `Level/bg_classroom_after`. The teacher (`Level/teacher`, 2 frames) is at (418, 126) and always animates at 3 fps. It has no wobblers, and its `shaker` may be null.
- Game: `LevelController`, for `phase` "after", has no shake, no rumble and no wobble. Bantay says the intro at once, and the Choices appear 1.1 s later. Try Again and Choose never call the shaker for a still Scenario.
- Screenshots: add `30-level3` and `31-level3-feedback-unsafe` (the `stay` Choice, with the heading "Time to go").
- PlayMode: the first half of `EasyDifficulty_CompletingAllThreeLevels_OpensNormal`, up to Level 3 being Finished:
  - Level 1 desk, then Next;
  - Level 2 stairs, Try Again, cover, Finish, then Next;
  - Level 3 shows `classroom_after` with nothing shaking;
  - pick walk, then Finish.

  Ticket 03 adds the Normal assertions.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green.
- `30-level3` shows the open door with the teacher in it, at both sizes. Bantay and the Hint button do not cover the teacher's face.
