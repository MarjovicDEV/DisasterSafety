# 09 Set Hard 7 in a covered basketball court

Changes: C14, C15.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

ArtSource/scripts/evacuation_area_v2.lua and its filtered build entry, evacuation_area art in ArtSource/Level/v2/ and Assets/Art/Level/v2/, ArtSource/README-rollout.md, Assets/Scripts/Editor/{ProjectBuilder.Level,LevelScreenshots}.cs, Assets/Tests/EditMode/ScenarioContentTests.cs and Assets/Tests/PlayMode/LevelFlowPlayTests.cs. Use the existing Hard/7.json content as the test reference.

## Tests to write first

First add an assertion that fails on the current open-field presentation, using the approved court layout or court art expectation. Extend HardLevel7 start, Safe and Unsafe tests to cover the family mat, seated Mom, radio initially off, radio response, neighbours, path toward home, all four endings or stops, Reactions and retry reset. Keep the current IDs and sources. Record a red run that fails on an assertion before implementing the behavior.

## Build

Replace the field artwork for the existing evacuation_area Setting. Show a later gathering at a fictional centre. Use the reference court's rust-red roof, exposed trusses, pale columns and open sides. Add a hoop, court markings, the plain sign “EVACUATION CENTER”, two tents and four additional background figures including two children. An adult and child appear at a tent entrance. Keep the foreground open for Student, Mom, mat and radio. Place neighbours and a visible route toward home where the old Choices still read clearly. Retain all four Choices and Feedback, including EQ-95/EQ-94 for the radio. EQ-54 supports the designated area. The school photo supplies architecture only. Keep the immediate outdoor assembly in Hard 2 distinct.

Use the photo inventory in `D:/GameDEV/earthquake-changes-scratch/photo-facts.md`. Keep the photos in scratch and draw original characters.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow only Hard 7 views showing the court, including h7-actionbar, h7-safe and h7-stop and any post-cleanup result where this Setting remains visible. Check radio, home, rumour and phone poses, tent and child visibility, sign legibility, and HUD clearance. Protect every other Level, especially Hard 2, and all menus including the Hard Difficulty background. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- All four Act-outs make sense in the covered court. The family, radio and routes remain legible at both sizes, and the researcher has checked the scene on screen and in motion.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
