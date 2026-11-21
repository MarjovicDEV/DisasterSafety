# 02 Shared Student poses, the icon script and the voice blip

Blocked by: none. Blocks: 04, 06, 07, 08, 10, 12.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- `ArtSource/scripts/student.lua`: add a `poses=` script parameter so a run writes only the named poses, then draw the nine new poses for the boy and the girl, as in the design doc's "New poses" table: `carry` (side, 4 frames), `reach`, `reachup`, `push`, `call`, `cover` (side, 2 each), `pack`, `phone` and `sit` (front, 2 each). Side views face right; the game mirrors them.
- `ArtSource/scripts/icons_v2.lua` (new, its own `only=icons_v2` part in `build.lua`): the drawing helpers for 16x16 icons in the style of the pilot's four icons. Each slice adds its own icons to it.
- `StudentPose` and `StudentFrames`: the nine poses; `ProjectBuilder.Level.cs` loads their frames for both kinds.
- `make_audio.py`: `sfx_blip` (a soft, very short syllable), written alone so no other sound file changes. `Sfx.Blip`, `AudioLibrary`, and `AudioManager.PlayBlip(StudentKind)` with a higher pitch for the girl. `PoseActor.Say` plays it once per line, under the Safe and Unsafe sounds, following the sound switch.
- Tests: every new pose has frames for both kinds; a Reaction plays the blip, and none plays when sound is off.

## Check

A preview sheet, enlarged: every new pose for both Students next to the pilot's idle, walk, crouch and hold, and each pose composited in the pilot classroom. Look at it, fix what reads badly, and send it to the user. A Grade 3 learner must be able to tell carry, reach, reach-up, push, call, cover, pack, phone and sit apart.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures.
- `git status` shows only new files under `Assets/Art`, `Assets/Audio` and `ArtSource` (the edited scripts `student.lua`, `build.lua` and `make_audio.py` excepted).
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
