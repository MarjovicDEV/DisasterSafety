# 04 Show the current Level and four Pause buttons

Changes: C8.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

Assets/Scripts/Game/{UIStrings,GameScenes}.cs, Assets/Scripts/Game/UI/LevelController.cs, Assets/Scripts/Editor/{ProjectBuilder.Level,LevelScreenshots}.cs, and Assets/Tests/PlayMode/LevelFlowPlayTests.cs. Reuse the existing play, replay, grid and home icons.

## Tests to write first

Extend Pause_ResumeRestartAndQuit_KeepSavedProgress and the Student freeze test. First assert four visible controls and the current identifier, such as “Earthquake - Hard 4”. Cover all four destinations, Restart with saved Stars, Resume within the same Attempt, Android Back and pausing during an Act-out. Record a red run that fails on an assertion before implementing the behavior.

## Build

Replace the Star sentence with Episode, Difficulty and Level number under “Paused”. In the top row show Resume and Restart. In the bottom row show Levels and Main Menu. Resume continues. Restart immediately starts a fresh Attempt with no confirmation or Star penalty. Levels opens Level selection for the current Episode and Difficulty. Main Menu returns home. Preserve saved best Stars. Check the order, icons, full labels and touch areas at both widths.

Reuse existing graphics where possible. If a new or revised graphic is needed, render it to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow only the Pause overlay views, including 14-pause or its post-cleanup equivalent. Add an actual paused Hard 4 Student-flow capture. All underlying play, menu, Feedback and completion baselines and every Level outside the Pause views must stay byte-identical. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- A Learner can identify the current Level, use all four destinations and resume exactly where the Attempt paused. The two rows remain readable over a live Student scene.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
