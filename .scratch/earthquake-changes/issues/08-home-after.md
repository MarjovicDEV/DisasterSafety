# 08 Call at the doorway and leave with the Go Bag

Changes: C12, C13.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md), [01-choice-counts.md](01-choice-counts.md).

Blocks: None.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

Assets/Resources/Scenarios/Earthquake/Hard/{5,6}.json, Assets/Scripts/Core/ScenarioSettings.cs, Assets/Scripts/Editor/{ProjectBuilder.Level,LevelScreenshots}.cs, ArtSource/scripts/{home_v2,icons_v2,build}.lua, home_after and Choice icon sources and PNGs under ArtSource and Assets/Art, ArtSource/README-rollout.md, Assets/Tests/EditMode/ScenarioContentTests.cs and Assets/Tests/PlayMode/LevelFlowPlayTests.cs.

## Tests to write first

First assert that the third Hard 5 Choice has ID wait_for_mom and that the Safe call reaches the threshold. First Hard 6 assertions must cover the note already visible at start and a clear route at the right table leg. Check four Choices per Level in the C5 order, IDs, icons, Hints, exact Feedback and findings, speeches and blips, Dad entering, both Unsafe replacements, bag ownership and Try Again restoring the cast and props. Record a red run that fails on an assertion before implementing the behavior.

## Build

Remove the broken pipe, drip and puddle in both Levels. Hard 5 replaces fix_pipe with “Wait for Mom to get up.” Stop near Mom, hands down. Feedback is “Keep Mom still and get help.” (EQ-90). On call_adult (EQ-90, EQ-72), crouch by Mom, say “Stay still, Mom.”, walk to near (56, 212), face outside and call “Help!” Dad enters and kneels. Keep the Unsafe exit stop on open indoor floor. In Hard 6 put the written family note on the table at start. Keep “Grab the go bag and walk out”. Use the foreground aisle, collect the bag near (106, 225) and leave with Mom near (56, 206). Safe Feedback reads “Your family left a note. Bring your go bag and walk out.” (EQ-93, EQ-92). Replace no_note with push_past_mom, “Push past Mom at the door.” Stop inside before contact. Feedback reads “Walk out. Do not run or push.” (EQ-92). Update the two icons and references together. Check the bag, table leg and doorway during motion.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow Hard 5 and Hard 6 home_after views, including both actionbar/safe/stop groups. Add the doorway call, Dad's entrance, Hard 6 foreground crossing and the two replacement Unsafe stops. Protect all other Levels, particularly Normal 2, Normal 3 and Hard 1 in the Home Setting, and all menus. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- Both Levels remain playable with four Choices. Dad and the Student clear the doorway at the right times. The bag clears the table and Mom. All Unsafe endings stay visibly separate from the Safe exit. The shared art passes review in both Levels.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
