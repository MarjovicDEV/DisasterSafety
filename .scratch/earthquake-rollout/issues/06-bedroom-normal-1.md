# 06 Bedroom: Normal 1

Blocked by: 01, 02. Blocks: 09.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- `ArtSource/scripts/bedroom_v2.lua` (new): the bedroom at 480x270: a bed that can slide, a window, a tall cabinet, a low shelf, and the props: the heavy box, the big picture and a stack of books, with the box's placed look on the low shelf. Icons: picture, low shelf, bed, books.
- Preview sheet beside the pilot classroom, with the Student in `carry`, `reach-up` and `push` at their points. Send it to the user.

Then, test first, one Level at a time:

- Give every Choice of the Level an `icon` and an `action` in its Scenario file (no text change), and list the new `action` keys in `ScenarioSettings.ActionSpots`.
- Build the Setting in `ProjectBuilder.Level.cs` as `Build<Setting>V2`, with the start spot, start pose, Grown-ups, props and one Act-out per Choice exactly as the design doc's table says. Record every point in `ArtSource/README.md`.
- PlayMode: `<Level>_SafeChoice_ActsOutAndCompletes` and `<Level>_UnsafeChoice_StopsAndResets` (see the spec, Tests). EditMode: the Level now uses the Student flow.
- Screenshots `<l>-actionbar`, `<l>-safe` and `<l>-stop` at both sizes; read them and fix overlaps.

Normal 1 Act-outs: "Normal 1, bedroom" in the design doc. Try Again slides the bed back and returns the box, the picture and the books to the floor.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures, with the new tests present and every older test still passing.
- The preview sheet matches the pilot (v2 outline, three shades, light from the left, same palette and pixel scale) and has been sent to the user.
- The screenshots of this slice read clearly at 1920x1080 and 2400x1080, and the screenshots of Levels outside this slice are unchanged.
- `git status` shows only new files under `Assets/Art`, `Assets/Audio` and `ArtSource` (edited generator scripts excepted), and no Scenario text changed.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
