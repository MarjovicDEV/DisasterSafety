# 02 Normal 4 and 5 play outside, and Normal opens Hard

Blocked by: 01. Blocks: 03.

Spec: `docs/specs/earthquake-normal.md` (behaviours 1, 2 and 4 for `schoolyard` and `beach`, and the Normal PlayMode test). Rules: `AGENTS.md`.

## Build

- `ProjectBuilder.Level.cs`: add `BuildSchoolyard` (tree and post wobblers) and `BuildBeach` (wave line, always animating) to `c.settings`.
- `LevelScreenshots.cs`: add `43-normal4`, `44-normal5`, `45-normal5-feedback-unsafe` and `46-level-completed-hard-open`.
- PlayMode: add `NormalDifficulty_CompletingAllFiveLevels_OpensHard` with every assertion in the spec.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green.
- Screenshots 43 to 46 at both sizes show the tree, post, open field, hill path and fish uncovered.
- In 46, the "Hard is open!" ribbon sits between the Stars and the Safe picture.
