# 10 Mall: Hard 3 (food court) and Hard 4 (the way out)

Blocked by: 01, 02. Blocks: 14.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- `ArtSource/scripts/mall_v2.lua` (new): two Settings at 480x270 in one style. `mall_a`: the food court with a table and its `foodtable_front` overlay, the exit, an elevator with a button, a glass shop window and a hanging sign that sways. `mall_b`: the upper floor on the left, stairs down to the exit, a still crowd of 2 or 3 people at the exit, the mall wall outside and the open parking with a sign on the right. Icons for all eight Choices (reuse `icon_door` where it fits).
- Preview sheet beside the pilot classroom, with the Student under the food court table, at the elevator, on the stairs and at the parking sign. Send it to the user.

Then, test first, one Level at a time:

- Give every Choice of the Level an `icon` and an `action` in its Scenario file (no text change), and list the new `action` keys in `ScenarioSettings.ActionSpots`.
- Build the Setting in `ProjectBuilder.Level.cs` as `Build<Setting>V2`, with the start spot, start pose, Grown-ups, props and one Act-out per Choice exactly as the design doc's table says. Record every point in `ArtSource/README.md`.
- PlayMode: `<Level>_SafeChoice_ActsOutAndCompletes` and `<Level>_UnsafeChoice_StopsAndResets` (see the spec, Tests). EditMode: the Level now uses the Student flow.
- Screenshots `<l>-actionbar`, `<l>-safe` and `<l>-stop` at both sizes; read them and fix overlaps.

Act-outs: "Hard 3, mall_a" and "Hard 4, mall_b" in the design doc. The stairs are a walk line that falls (Act-out points with y).

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures, with the new tests present and every older test still passing.
- The preview sheet matches the pilot (v2 outline, three shades, light from the left, same palette and pixel scale) and has been sent to the user.
- The screenshots of this slice read clearly at 1920x1080 and 2400x1080, and the screenshots of Levels outside this slice are unchanged.
- `git status` shows only new files under `Assets/Art`, `Assets/Audio` and `ArtSource` (edited generator scripts excepted), and no Scenario text changed.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
