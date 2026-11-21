# 01 Act-out Steps: the engine every Level runs on

Blocked by: none. Blocks: 03, 04, 06, 10, 13.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build (test first)

- `ActOut` and `ActOutStep` (`Assets/Scripts/Game/UI/ActOut.cs`) with the step kinds in the spec (Walk, Run, Crouch, Pose, Say, Wait, Calm, PropShow, PropHide, PropPickUp, PropPlace, PropSlide, Overlay, GrownUp) and `stopAfter`. `LevelSetting` gains `startPose`, `props` and `actOuts` in place of `actionSpots`; the `grownUps` list is added here and filled in ticket 03.
- `PoseActor`: move the pose player, mirroring, walking and speech bubble out of `StudentActor`, and let walking follow a path that changes y as well as x (slopes and stairs), at walk, fast-walk or run speed.
- `LevelProp`: start place, pick up (follows the Student's hand anchor), place, slide, and reset on Try Again and Restart.
- `LevelController.ActOut`: play the steps; stop after `stopAfter` for an Unsafe Choice; on Try Again, walk the Student back while props and Grown-ups reset. A missing prop or Grown-up logs an error and skips the step.
- Move Easy 1 (`BuildClassroomV2`) onto Act-out Steps with the same positions and timing: the Safe `desk` Act-out walks to 184, shows `desk_front`, crouches to 222, plays `Hold` and calms the shake; each Unsafe Act-out walks toward its spot and stops where the pilot stopped.
- PlayMode: the existing Easy 1 tests pass unchanged; new tests, on a test Setting built in the test, for a prop that resets on Try Again and for a walk that changes y.
- `AGENTS.md`: the batch commands name Unity 6000.6.9f1 and `D:/GameDEV/DisasterSafety-redesign`.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures.
- Easy 1 plays exactly as the user approved (screenshots 60 to 67 unchanged), and every card-flow Level still plays as before.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
