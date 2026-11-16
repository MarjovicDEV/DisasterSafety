# 03 "Normal is open!" on finishing Easy

Blocked by: 02. Blocks: 04.

Spec: `docs/specs/earthquake-easy-2-3.md` (behaviour 5, `UIStrings`, the audio entries). Rules: `AGENTS.md`.

## Build

- Game:
  - Add `UIStrings.Opened`, `BantayOpenedSoon` and `BantayOpened`, word for word from the spec.
  - Add `Sfx.Opened` and `AudioLibrary.opened`.
  - In `LevelController`, add `openedRibbon` and `openedText`, and implement behaviour 5 in `ShowCompleted`: the ribbon when `Opened` is not empty, `sfx_opened` after the Stars are done, and Bantay's opened line about 2.5 s after the cheer.
- Editor: in `ProjectBuilder.cs`, add `lib.opened = Clip("Sfx/sfx_opened")`. In `ProjectBuilder.Level.cs`, build the ribbon (`UI/button_yellow` sliced, `Icons/icon_hardhat`, 520x80 at (-370, -428) from the panel's top centre, label in `f4` Ink), inactive by default.
- Screenshot: add `32-level-completed-normal-open`.
- PlayMode: finish `EasyDifficulty_CompletingAllThreeLevels_OpensNormal` with every assertion listed in the spec, including that the ribbon is hidden on a Replay of Level 3.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green.
- In `32-level-completed-normal-open`, the ribbon sits between the Stars and the Safe picture with no overlap at either size.
