# 05 Redraw the reach-up hands in Normal 1

Changes: C7.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md), [01-choice-counts.md](01-choice-counts.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

ArtSource/scripts/student.lua, ArtSource/Student/{boy,girl}_reachup.aseprite, the corresponding generic reach-up frames in Assets/Art/Student/, Assets/Scripts/Editor/LevelScreenshots.cs and Assets/Tests/PlayMode/LevelFlowPlayTests.cs. Use the existing bedroom Act-out timings and prop placements.

## Tests to write first

Exercise ticket 01's retained picture and books Choices for both Students. First add a failing art regression assertion against the reviewed defective hand region at peak reach. After approval, use the approved frame crop as the visual expectation. Assert the two frames, contact with the prop in both facing directions, unchanged timing, and reset on Try Again. Record a red run that fails on an assertion before implementing the behavior.

## Build

In both 32 × 50 frames bend the far elbow, separate the forearms, align the nearer palm with the wrist and keep contact with the held object. Use the picture over the bed as the main example and the left-facing books at the cabinet as the mirror check. Present all four frames at native size and enlarged. Watch the highest reach in motion. Hard 1 uses separate home_reachup sprites.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow only Normal 1 frames that actually display generic reach-up, including new peak-reach captures for the picture and books. Protect n1-actionbar, n1-safe, the other Normal 1 views that do not show the reach, and all other Levels. Compare the Hard 1 card pose and all UI screens byte for byte. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- Both hands are distinguishable in both frames at game scale. Neither prop appears to float or pass through a palm during the peak. The researcher has approved the normal and mirrored preview.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
