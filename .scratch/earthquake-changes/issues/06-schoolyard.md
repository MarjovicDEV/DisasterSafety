# 06 Bring the field closer and lead the class around the school

Changes: C6, C9, C10, C15.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md), [01-choice-counts.md](01-choice-counts.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

ArtSource/scripts/schoolyard_v2.lua and its filtered build entry, ArtSource/Level/v2/ and Assets/Art/Level/v2/ schoolyard backdrop, canopy and post layers, ArtSource/README-rollout.md, Assets/Scripts/Editor/{ProjectBuilder.Level,LevelScreenshots}.cs, Assets/Tests/EditMode/ScenarioContentTests.cs and Assets/Tests/PlayMode/LevelFlowPlayTests.cs.

## Tests to write first

First demonstrate the old Normal 4 walk exceeds the approved approximately one-second target. For Hard 2 add an intermediate footprint check that fails where the old diagonal crosses the wall near (151, 214). Verify Hard 2's Teacher leaves the doorway before crossing, spacing between both walkers, staging, Safe and Unsafe stops and reset. Cover the retained Normal 4 Choices after ticket 01, and the still versus shaking landmark frames. Record a red run that fails on an assertion before implementing the behavior.

## Build

Start Normal 4 about 80 art pixels from the current Safe endpoint (110, 230), at 80 art pixels per second. Keep crouch and Cover there. The tree and building stops remain on distinct open ground. In Hard 2 the Teacher steps out first. Both follow the foreground until clear of the building's left corner before turning toward the sign. Choose precise starting and bend points in the approved preview, then record them. Clarify the tree trunk and branches, post crossarm and wires, grass, path and Evacuation Area sign. Use the school reference's green doors, barred windows and shaded concrete walkway. Keep the approved Scenario text and findings EQ-80/EQ-81 and EQ-54/EQ-56.

Use the photo inventory in `D:/GameDEV/earthquake-changes-scratch/photo-facts.md`. Keep the photos in scratch and draw original characters.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow only Normal 4 and Hard 2 views showing the changed yard or routes, including n4/h2 actionbar, safe and stop. Capture the Teacher leaving the door and both walkers clearing the corner. Check the still and swaying landmark frames. Protect every other Level and all menu backgrounds, including the separate schoolyard menu illustration. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- Normal 4 reaches the field in about one second and resets to its approved start. Both Hard 2 characters clear the wall throughout the walk. The researcher reviews motion, landmark readability and HUD clearance at both sizes.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
