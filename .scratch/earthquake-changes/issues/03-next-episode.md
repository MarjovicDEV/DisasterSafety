# 03 Open Episode selection from Level Completed

Changes: C1.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md), [02-menu-backgrounds.md](02-menu-backgrounds.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

Assets/Scripts/Core/{LevelCompletion,MenuNavigator}.cs where needed, Assets/Scripts/Game/{GameScenes,UIStrings}.cs, Assets/Scripts/Game/UI/{LevelController,MainMenuController}.cs, Assets/Scripts/Editor/{ProjectBuilder.Level,LevelScreenshots}.cs, Assets/Tests/EditMode/{LevelCompletionTests,MenuNavigatorTests}.cs and Assets/Tests/PlayMode/{LevelFlowPlayTests,MenuFlowPlayTests}.cs. Reuse the existing Fire emblem.

## Tests to write first

First assert through the Student flow that the completion which opens Fire offers Next Episode. Cover Hard 7 early, Hard 7 last, a different Hard Level last, subsequent Hard 7 replay, other replays and an Episode with no successor. Check that Fire opens once, existing Next-Level and save rules still work, and the button navigates to the completed Fire-visible selection screen from ticket 02. Record a red run that fails on an assertion before implementing the behavior.

## Build

Put the Fire emblem and “Next Episode” in the existing Next position between Replay and Levels when this completion first opens Fire, or when Hard 7 is replayed with Fire already Open. Preserve normal navigation on other completions. Open Episode selection with Fire visible. Keep the existing coming-soon text while Fire has no Playable Level. Verify a save loaded before replay as well as the initial completion.

Reuse existing graphics where possible. If a new or revised graphic is needed, render it to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow the affected Level Completed states for Earthquake Hard when Fire opens and Hard 7 replay. Capture the early Hard 7 case, out-of-order final completion and the Fire-visible destination in both sizes. Replace any surviving legacy 58-level-completed-fire-open staging with a Student-flow capture. Protect other completion states, all playable Level views outside the affected results, and ticket 02's normal menus. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- A Learner who clears Hard out of order, or later replays Hard 7, can reach Episode selection from the correct result. Completing Hard 7 early does not offer premature access.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
