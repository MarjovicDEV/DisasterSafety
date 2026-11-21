# 07 Home: Normal 2, Normal 3 and Hard 1

Blocked by: 02, 03. Blocks: 09, 11.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- `ArtSource/scripts/home_v2.lua` (new): the living room at 480x270, drawn so `home_after` (ticket 11) can reuse its layout: a strong table with a `table_front` overlay, a sofa, a glass cabinet, the door, a TV, boxes in a corner, a "Meet here" wall board and a hanging lamp that sways. Props: the go bag (open and closed), water, a can, a flashlight, a whistle, a snack, a toy, the park card and the house card. Icons for all twelve Choices.
- One Setting serves three Levels: each Level shows only its own props and Grown-ups (Mom in Normal 2 and Hard 1, nobody in Normal 3), and `ScenarioSettings.ActionSpots("home")` lists the keys of all three.
- Preview sheet beside the pilot classroom, with the Student and Mom at the key points of all three Levels. Send it to the user.

Then, test first, one Level at a time:

- Give every Choice of the Level an `icon` and an `action` in its Scenario file (no text change), and list the new `action` keys in `ScenarioSettings.ActionSpots`.
- Build the Setting in `ProjectBuilder.Level.cs` as `Build<Setting>V2`, with the start spot, start pose, Grown-ups, props and one Act-out per Choice exactly as the design doc's table says. Record every point in `ArtSource/README.md`.
- PlayMode: `<Level>_SafeChoice_ActsOutAndCompletes` and `<Level>_UnsafeChoice_StopsAndResets` (see the spec, Tests). EditMode: the Level now uses the Student flow.
- Screenshots `<l>-actionbar`, `<l>-safe` and `<l>-stop` at both sizes; read them and fix overlaps.

Act-outs: "Normal 2, home", "Normal 3, home" and "Hard 1, home" in the design doc. Normal 2's Safe Act-out leaves the go bag by the door, the same place where Hard 6 starts it.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures, with the new tests present and every older test still passing.
- The preview sheet matches the pilot (v2 outline, three shades, light from the left, same palette and pixel scale) and has been sent to the user.
- The screenshots of this slice read clearly at 1920x1080 and 2400x1080, and the screenshots of Levels outside this slice are unchanged.
- `git status` shows only new files under `Assets/Art`, `Assets/Audio` and `ArtSource` (edited generator scripts excepted), and no Scenario text changed.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
