# 01 Set Choice counts by Difficulty

Changes: C5.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md).

Blocks: [05-normal-1-reach.md](05-normal-1-reach.md), [06-schoolyard.md](06-schoolyard.md), [08-home-after.md](08-home-after.md).

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

Assets/Scripts/Core/ScenarioRules.cs and ScenarioSettings.cs, the eight Easy and Normal JSON files under Assets/Resources/Scenarios/Earthquake/, Assets/Scripts/Game/UI/ActionBar.cs and Assets/Scripts/Editor/ProjectBuilder.Level.cs where required, LevelScreenshots.cs, MenuScreenshots.cs, ArtSource/scripts/tutorial_v2.lua and affected icon or Setting generator entries, Tutorial Page 3 assets and proven unused v2 art and sources. Work in Assets/Tests/EditMode/{ScenarioTests,TestScenarios,AttemptTests,ScenarioContentTests}.cs and Assets/Tests/PlayMode/{LevelFlowPlayTests,MenuFlowPlayTests}.cs.

## Tests to write first

Start with a valid two-Choice Easy Scenario failing validation in ScenarioTests. Assert the exact count by Difficulty for Earthquake and synthetic Scenarios from other Episodes. Reject wrong counts and zero or multiple Safe Choices. Move multi-Unsafe Attempt cases to a suitable Difficulty. Add checks for all 15 ordered lists, Hints, source IDs, icons and Act-outs. In PlayMode check centering, tap targets, Tried state, same-Attempt Try Again, Hint costs, Stars, unlocks and existing saves. Record a red run that fails on an assertion before implementing the behavior.

## Build

Apply the C5 table in the spec in Scenario order. Easy loses five Choices and Normal loses five. Keep the current four Choices in Hard 5 and Hard 6 until ticket 08 delivers the replacements. Hide unused buttons and center a single row of 430 × 124 UI-unit buttons with 20-unit gaps. Check long labels at three buttons. Tutorial Page 3 shows the door and desk Choices in that order in both frames. Preserve existing Hints and citations. Remove retired bindings and Feedback. Before deleting an icon, source or pose, check its use in other Levels, tutorials, builder, capture code and generators. Keep shared assets. Update captures that stage the retired window, stairs, push, bed, door and fish actions to stage retained Choices. Use retained action IDs for behavior tests.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow changes to Easy 1–3 and Normal 1–5 Action Bar views, their captures showing a removed Choice or its Feedback, and Tutorial Page 3. In particular review 62-pilot-actionbar, 64-pilot-wait, 67-pilot-feedback, e2/e3/n1–n5 actionbar and the affected stop views. Capture both tutorial frames. Protect all Hard Level views, menus other than the affected tutorial, and any Easy or Normal view whose visible content is identical. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- All 15 Levels remain playable with 49 Choices in total. Every Level has one Safe Choice. Easy 1 can award one Star after the Hint and one Unsafe Choice. Two centered buttons and all three centered buttons can be read and tapped.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
