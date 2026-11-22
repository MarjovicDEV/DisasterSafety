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

## Icons (`ArtSource/scripts/icons_v2.lua`, part `icons_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=icons_v2 --script-param icons=<names> --script ArtSource/scripts/build.lua`.
The `icons=` filter writes only the named icons, so each slice adds its icons without rewriting the ones committed before. Sources go to `ArtSource/UI/`, PNGs to `Assets/Art/UI/icon_<name>.png`, 16x16.

| Icon | Choice | Slice |
|---|---|---|
| `icon_stairs` | Run down the stairs | Easy 2 |
| `icon_cover` | Duck and cover your head | Easy 2 |
| `icon_plants` | Stand under the hanging plants | Easy 2 |
| `icon_picture` | Hang a big picture over the bed | Normal 1 |
| `icon_low_shelf` | Put heavy things on a low shelf | Normal 1 |
| `icon_bed` | Move the bed under the window | Normal 1 |
| `icon_books` | Put books on top of the tall cabinet | Normal 1 |

Easy 2 reuses the pilot's `icon_window` for "Stand by the window".

## Corridor (`ArtSource/scripts/corridor_v2.lua`, part `corridor_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=corridor_v2 --script ArtSource/scripts/build.lua`.
Writes `Level/v2/bg_corridor` (480x270) and `Level/v2/corridor_pots_0/1` (72x110, 160 ms, the pots sway while the ground shakes).

All values are art pixels; one art pixel is four UI units. The floor line (the Student's feet) is y 230, as in the v2 classroom.

| Part | Place (top-left) or x |
|---|---|
| Hanging pots sprite | (6, 15); pot bodies about x 15-34 and 44-62, y 107-121; the spot under the pots is x 38 |
| Hall window | (84, 66), 92x92; its sill ends at x 180 |
| Grade 4 door and sign | door (184, 112), 50x92; sign (191, 96) |
| Plain inner wall | (240, 15), 92x192; its shadow falls on x 332-335 |
| Bulletin board | (346, 68), 52x44 |
| Exit sign | (424, 110) |
| Floor | Continuous concrete from y 208. The window casts light on the left side of the walkway. |
| Stairs | The first tread meets the walkway at (404, 230). The front edges descend 6 pixels every 16 pixels to the right. The back edges descend 5 pixels every 14 pixels, beginning at (395, 208), to align the flight with the corridor. A solid stairwell wall follows the flight. |

Act-out points (the Student's centre x, `BuildCorridorV2` in `ProjectBuilder.Level.cs`):

| Choice (`action`) | Steps | Point |
|---|---|---|
| Start | `idle` | 214, in front of the Grade 4 door |
| Safe: `cover` | walk to 270, crouch to 286, `cover` for 1.5 s, calm the shaking | 286, the middle of the inner wall |
| `stairs` | run-start to 394, stop | 394, the right foot on the top step |
| `window` | walk to 188, stop | 188, just right of the sill |
| `plants` | walk to 82, stop | 82, just short of the spot under the pots |

## Classroom after (`ArtSource/scripts/classroom_after_v2.lua`, part `classroom_after_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=classroom_after_v2 --script ArtSource/scripts/build.lua`.
It writes `Level/v2/bg_classroom_after` (480x270) and `Level/v2/classroom_after_desk_front` (84x42). The separate desk front draws over the Student. Use `only=icons_v2` and `icons=stay,teacher_walk,push` for the three 16x16 Choice icons.

All coordinates are art pixels measured from the top left. The floor line is y 230. One art pixel is four UI units. The bulletin board, flag, blackboard, clock, window, plant, desk, fan and tall cabinet keep their pilot placements. The room is still during this Phase.

| Part | Placement or bounds |
|---|---|
| Open door and EXIT sign | Door frame (16, 106), 62x98; opening x 26–70; door leaf x 7–26, y 104–215; EXIT sign (24, 92) |
| Threshold and daylight | Sill x 19–76, y 203–210; floor light x 25–113, y 208–238 |
| Desk front | (198, 188), 84x42; left front leg x 200–206 |
| Student Reaction bubble | 44 pixels to the right of its usual centred position, clear of the Teacher |
| Upright chair | (302, 187), 24x42 |
| Tipped chair | 54x24 layer at (340, 212) |
| Floor papers | (122, 216), (154, 230), (325, 224), (398, 232) |
| Still fan and tall cabinet | (204, 0) and (424, 100) |

Act-out points use the centre of each character's feet. `LevelSetting` stores height above y 230, so a 21-pixel rise places the Student's feet at y 209.

| Choice (`action`) | Student steps and points | Teacher steps and points |
|---|---|---|
| Start | Hold at (222, 230) under the desk | Wave at (77, 230) by the door |
| Safe: `walk` | Crouch-shuffle to (182, 230), stand briefly, walk to (113, 230), then follow the Teacher to (51, 209) and play Happy | Wave until the Student approaches, then turn left and lead to (36, 195) |
| `stay` | Hold for 0.45 s, shift to (220, 230), and peek left for 0.3 s. The Unsafe Reaction plays while the Student is still low under the desk. | Wave at (77, 230) |
| `push` | Crouch-shuffle to (182, 230), stand briefly, then run-start and stop at (110, 230) behind the Teacher. | Wave at (77, 230) |
| Try Again | From the door, walk back to (182, 230) before crouching. From the peek point, shuffle directly back to (222, 230). Resume Hold at (222, 230). | Resume the start position and Wave. |

## Easy art follow-up

Easy 1 and 3 share the concrete floor drawn by `classroom_v2.lua`. The floor art begins at y 208, with window light at x 305–427, y 209–236. Easy 3 also keeps the light from the open door. The Student's floor line stays at y 230.

In Easy 2, both Students use the redrawn `cover` pose. Each `boy_cover` and `girl_cover` source has two 32x50 frames at 300 ms. The Student kneels with shins folded, chin tucked and forearms over the head and neck. The upper body lowers one art pixel in the second frame, with the feet fixed.

## Bedroom (`ArtSource/scripts/bedroom_v2.lua`, part `bedroom_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=bedroom_v2 --script ArtSource/scripts/build.lua`.
It writes `Level/v2/bg_bedroom` (480x270) and five props, each its own sprite: `bedroom_bed` (112x31), `bedroom_box` and `bedroom_box_placed` (24x17), `bedroom_picture` (36x29) and `bedroom_books` (16x12). Use `only=icons_v2` and `icons=picture,low_shelf,bed,books` for the four Choice icons. Normal 1 is a "before" Level: nothing shakes and the Student starts standing calm.

All values are art pixels from the top left; one art pixel is four UI units. The floor line (the Student's feet) is y 230 and the floor art is the continuous concrete of the Easy art follow-up, lit from the window. Every prop stands on the back of the floor (bottom row at y 212 or 213), like the furniture.

| Part | Placement |
|---|---|
| Picture (leaning on the wall) | (10, 183) |
| Window, curtains and sill | (48, 54), 72x92 glass and frame |
| Bed (headboard on the right) | (104, 194); the headboard's right edge is x 215 |
| Tall cabinet | (236, 116), 46x98 |
| Books | (322, 201), just right of the start |
| Box on the floor | (350, 196) |
| Low shelf | (392, 166), 64x48; the lower left compartment is empty for the box |
| Box placed in the shelf | (397, 193) |
| Calendar, star poster, clock | (398, 80), (432, 86), clock centre (259, 64) |

Act-out points use the centre of the Student's feet (`BuildBedroomV2` in `ProjectBuilder.Level.cs`). A held prop is a child of the Student's body, so it mirrors with the Student. Its `holdOffset` puts its centre on the carry pose's hands at about (23, 30) in the 32x50 frame. The reach-up hands are about (28, 3).

| Choice (`action`) | Steps and points | Stop |
|---|---|---|
| Start | `idle` at 306, facing front | |
| Safe: `low_shelf` | walk to 343, crouch and lift the box, carry it right to 384 (beside the shelf), the box shows in the shelf, crouch, stand | |
| `picture` | walk to 34, face left, reach and pick up the picture, carry it right to 140, reach up; the picture is held against the wall over the bed at (134, 155) | reaching up at 140 |
| `bed_window` | walk to 229, face left, push: the bed slides 8 to the left (toward the window) and the Student steps with it | pushing at 221, palms on the headboard |
| `books_top` | walk to 316, crouch and pick up the books, carry them left to 272, reach up on tiptoe; the books are held up against the cabinet at (251, 172) | reaching up at 272 |
| Try Again | The Student walks back to 306. The bed slides back; the box, the picture and the books fade back to their start; the placed box hides. | |

Only the walk to the box passes a prop (the books beside the start). With three floor props and a bed to push on one floor line, one prop always sits between the start and something else, so the smallest prop takes that place.

The Student's `carry`, `reachup` and `push` poses were redrawn for this slice (`student.lua`, "Action poses"): `carry` holds the load on level forearms and leans back; `reachup` raises both arms in front of the face on tiptoe; `push` leans forward with flat palms and the back foot pushing off. Frame counts stay 4, 2 and 2.

## Removed art (cleanup, 2026-11-22)

No screen showed these, and no ticket plans them (see `.scratch/cleanup/inventory.md`):

- `Icons/icon_cross` and `Icons/icon_pencil`: the PNGs and their sources. `ui.lua` no longer draws them.
- `UI/button_gray_pressed`: the PNG and its source. Gray is only the disabled and Tried look, so `build.lua` saves no pressed state for it. `button_gray` stays.
- The third-party `Assets/8K Skybox Pack Free` and `Assets/Clean Vector Icons` packs, which the game never used.
