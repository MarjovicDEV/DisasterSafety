# 02 Hard flow: play through Hard and finish the Episode

Blocked by: 01. Blocks: 03.

Spec: `docs/specs/earthquake-hard.md` (behaviours 4 and 5, and the Tests section). Rules: `AGENTS.md`.

## Build

- PlayMode `LevelFlowPlayTests.cs`:
  - in `NormalDifficulty_CompletingAllFiveLevels_OpensHard`, expect "Hard is open! Let's play!";
  - add `HardDifficulty_CompletingAllSevenLevels_CompletesTheEpisode`, as the spec describes: the Setting key and shaking per Level, Next through Hard 7, then `EpisodeCompleted`, "Fire/Easy" opened and saved, the "Fire is open!" ribbon, Next hidden, and Bantay's "Fire is open! New levels are coming soon." line.
- PlayMode `MenuFlowPlayTests.cs`: add `HardOpen_LevelSelection_StartsEveryHardLevel`.
- `LevelScreenshots.cs`:
  - `46-level-completed-hard-open` uses `UIStrings.BantayOpened(hardName)`;
  - add `58-level-completed-fire-open`.

No change in `LevelController`, `UIStrings` or `LevelCompletion`. To pick the Safe Choice, reuse what the Normal PlayMode test already uses.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green with every test from the spec present.
- Shot 46 shows "Hard is open! Let's play!", and shot 58 shows the "Fire is open!" ribbon with the "coming soon" line, at both sizes, with the lesson clear of the buttons.
