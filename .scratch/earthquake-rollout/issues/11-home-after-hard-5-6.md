# 11 Home after the earthquake: Hard 5 and Hard 6

Blocked by: 07. Blocks: 14.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- In `home_v2.lua` (from ticket 07), a new `home_after` drawing from the same layout: things fallen and tilted (books on the floor, a tilted picture, a toppled chair), a broken pipe that drips (2 frames), and the go bag by the door where Normal 2 left it. No file from ticket 07 is regenerated. Props: the note, paper and pen. Icons for all eight Choices.
- Hard 5: Mom in `hurt-sit`; Dad stays off screen until the Safe Act-out brings him in. Hard 6: Mom at the door.
- Preview sheet beside the `home` Setting and the pilot classroom. Send it to the user.

Then, test first, one Level at a time:

- Give every Choice of the Level an `icon` and an `action` in its Scenario file (no text change), and list the new `action` keys in `ScenarioSettings.ActionSpots`.
- Build the Setting in `ProjectBuilder.Level.cs` as `Build<Setting>V2`, with the start spot, start pose, Grown-ups, props and one Act-out per Choice exactly as the design doc's table says. Record every point in `ArtSource/README.md`.
- PlayMode: `<Level>_SafeChoice_ActsOutAndCompletes` and `<Level>_UnsafeChoice_StopsAndResets` (see the spec, Tests). EditMode: the Level now uses the Student flow.
- Screenshots `<l>-actionbar`, `<l>-safe` and `<l>-stop` at both sizes; read them and fix overlaps.

Act-outs: "Hard 5, home_after" and "Hard 6, home_after" in the design doc. "Stay still, Mom." and "Help!" are Act-out lines in `UIStrings`.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures, with the new tests present and every older test still passing.
- The preview sheet matches the pilot (v2 outline, three shades, light from the left, same palette and pixel scale) and has been sent to the user.
- The screenshots of this slice read clearly at 1920x1080 and 2400x1080, and the screenshots of Levels outside this slice are unchanged.
- `git status` shows only new files under `Assets/Art`, `Assets/Audio` and `ArtSource` (edited generator scripts excepted), and no Scenario text changed.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
