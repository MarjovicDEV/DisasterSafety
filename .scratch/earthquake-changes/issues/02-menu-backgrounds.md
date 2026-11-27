# 02 Draw the gate and selection backgrounds

Changes: C3, C4, C15.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md).

Blocks: [03-next-episode.md](03-next-episode.md).

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

ArtSource/scripts/background.lua and build.lua, ArtSource/Background/, Assets/Art/Background/, Assets/Scripts/Editor/{ProjectBuilder,MenuScreenshots}.cs, Assets/Scripts/Game/UI/MainMenuController.cs, Assets/Tests/EditMode/MenuNavigatorTests.cs and Assets/Tests/PlayMode/MenuFlowPlayTests.cs. Record the reference details in ArtSource/README-rollout.md.

## Tests to write first

First assert that Main Menu, Episode selection, Difficulty selection and Level selection show the specified distinct artwork. Exercise each panel through navigation. Cover locked cards on a fresh save, opening Fire on an existing save, empty Fire Level selection, Back, and return from a Level. A test must fail on an incorrect background assignment before implementation. Record a red run that fails on an assertion before implementing the behavior.

## Build

Use one shared menu art script. Draw the central horizontal gate, palms, blue trim and pale Baluan Elementary School sign on the red wall. Leave the title and buttons left, sign above the greeting and Student low right. Use a neutral noticeboard for Episode selection. Difficulty selection shows a school drill ground for Earthquake or a station forecourt and parked fire engine for Fire. Earthquake Level selection uses a classroom with green slatted chairs, cream walls, green windows and dark board for Easy, a schoolyard path for Normal, and a covered court with rust-red roof and pale columns for Hard. Empty Fire Level selection uses a simple station noticeboard. Switch the artwork and relevant animated foreground layers on each transition. Keep emblems, locks and existing navigation. Use the 14-photo scratch inventory for background details only. Show both Students and a long Display Name in the gate preview.

Use the photo inventory in `D:/GameDEV/earthquake-changes-scratch/photo-facts.md`. Keep the photos in scratch and draw original characters.

Render the art to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Allow 01-main, 02-episodes, 03-difficulty, 04-levels and overlays where the new artwork is visible behind the panel, including Settings, How to Play, Exit, name entry and Student picker. Capture Earthquake Easy/Normal/Hard and Fire selection states. Protect all playable Level images and any menu overlay whose visible pixels are unaffected. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- Each screen shows its assigned background on entry and return. The school name stays visible beside the controls at both widths. Fire presents its empty content state correctly. The researcher recognizes the gate and the three Difficulty scenes from the approved references.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
