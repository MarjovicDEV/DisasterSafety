# 03 Grown-ups: the Teacher, Mom and Dad

Blocked by: 01. Blocks: 05, 07, 08, 12.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- `ArtSource/scripts/grownups_v2.lua` (new, `only=grownups_v2`): the Teacher (idle, wave, walk), Mom (idle, happy, walk, sit, hurt-sit: sitting and holding her leg, no blood) and Dad (walk, kneel), about 36x62, in the v2 style: the Student's outline, three shades with light from the left and palette, front views from mirrored half maps, side walks with 4 frames. Saved under `ArtSource/GrownUp/` and `Assets/Art/GrownUp/`.
- `GrownUpActor` on `PoseActor`, `GrownUpPose`, and the `GrownUp` Act-out step (walk to a point, play a pose, turn). Grown-ups reset to their start place and pose on Try Again.
- PlayMode: a Grown-up walks, poses and resets on a test Setting.

## Check

A preview sheet: the three Grown-ups in every pose beside the boy and the girl Student and the pilot desk, at 1x and enlarged. The Student must read as a child next to them. Send it to the user.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures.
- `git status` shows only new files under `Assets/Art` and `ArtSource` (the edited `build.lua` excepted).
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
