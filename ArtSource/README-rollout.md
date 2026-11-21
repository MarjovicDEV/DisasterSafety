# Rollout art notes

New art for the Earthquake rollout. Existing files stay as they are; `ArtSource/README.md` describes the pilot.

## Grown-ups (`ArtSource/scripts/grownups_v2.lua`, part `grownups_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=grownups_v2 --script ArtSource/scripts/build.lua`
(add `--script-param out=<folder>` to render to a scratch folder first). Sources go to `ArtSource/GrownUp/`, PNGs to `Assets/Art/GrownUp/`.

Every frame is 36x62 and bottom aligned: x 17 and 18 are the middle of the figure, and the lowest row (y 61) is the outline under the shoes. Place a Grown-up like the Student (`PoseActor`): the centre of the feet on the floor line. One art pixel is four UI units.

| Files | Frames | Frame ms | View |
|---|---|---|---|
| `teacher_idle`, `teacher_wave` | 2 each | 500, 260 | front |
| `teacher_walk`, `mom_walk`, `dad_walk` | 4 | 140 | side, facing right |
| `mom_idle`, `mom_happy`, `mom_sit`, `mom_hurtsit` | 2 each | 500, 260, 500, 450 | front |
| `dad_kneel` | 2 | 400 | side, facing right |

- Teacher: navy blazer, white blouse, red scarf, navy skirt, hair in a bun, glasses. Mom: red top, navy trousers, long hair. Dad: white shirt, red tie, belt, navy trousers, short hair.
- `mom_sit` is cross-legged on the floor or a mat; `mom_hurtsit` holds her left shin, no blood. Both sit on the frame's bottom row, so place them with the floor line as the Student's feet.
- Dad has no idle: a Dad standing still shows a passing frame of `dad_walk` (`GrownUpFrames.For`).
- `GrownUpActor` faces left by mirroring; the side poses face right by default.
