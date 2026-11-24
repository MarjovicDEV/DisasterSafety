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
| `icon_pack_later` | Pack it later, when it shakes | Normal 2 |
| `icon_bag_hidden` | Hide it under the boxes | Normal 2 |
| `icon_go_bag` | Pack water, food, a flashlight and a whistle | Normal 2 |
| `icon_snack_toy` | Pack only snacks and toys | Normal 2 |
| `icon_door` (pilot) | Run out the door | Normal 3 |
| `icon_glass_cabinet` | Stand by the glass cabinet | Normal 3 |
| `icon_sofa` | Lie down beside the sofa | Normal 3 |
| `icon_strong_table` | Duck under the strong table | Normal 3 |
| `icon_own_way` | Each of us finds our own way | Hard 1 |
| `icon_meet_park` | Agree on a meeting place | Hard 1 |
| `icon_meet_house` | Plan to meet back at home | Hard 1 |
| `icon_phone_numbers` | Keep numbers only in the phone | Hard 1 |
| `icon_tree` | Hide under the big tree | Normal 4 |
| `icon_field` | Go to the open field | Normal 4 |
| `icon_building` | Run back into the building | Normal 4 |
| `icon_post` | Stand by the electric post | Normal 4 |
| `icon_route_post` | Take the path by the post | Hard 2 |
| `icon_route_tree` | Walk the path under the tree | Hard 2 |
| `icon_route_field` | Take the route across the field | Hard 2 |
| `icon_route_later` | Find the way when it happens | Hard 2 |
| `icon_fish` | Pick up the fish on the sand | Normal 5 |
| `icon_watch_waves` | Stay and watch the waves | Normal 5 |
| `icon_high_ground` | Go to high ground right away | Normal 5 |
| `icon_wait_beach` | Wait on the beach for news | Normal 5 |
| `icon_door` (reused) | Run to the mall exit | Hard 3 |
| `icon_elevator` | Ride the elevator down | Hard 3 |
| `icon_shop_window` | Stand by the glass shop window | Hard 3 |
| `icon_strong_table` (reused) | Duck under the food court table | Hard 3 |
| `icon_stairs` (reused) | Walk down the stairs and out | Hard 4 |
| `icon_elevator` (reused) | Take the elevator down | Hard 4 |
| `icon_run_push` | Run and push to get out | Hard 4 |
| `icon_mall_wall` | Wait by the mall wall outside | Hard 4 |

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

## Home (`ArtSource/scripts/home_v2.lua`, part `home_v2`)

Run `aseprite.exe -b --script-param root=<project> --script-param only=home_v2 --script ArtSource/scripts/build.lua` to make the 480x270 room, the 96x42 table-front overlay, ten props, and both 36x65 lamp frames (160 ms per frame). Use `only=home_reachup` for the two Hard 1 Reach Up frames per Student and the editable sources. The `icons_v2` part takes the icon names from the table above without the `icon_` prefix.

All positions are top-left art pixels. One art pixel is four UI units. The concrete floor begins at y 208. The Student and Mom stand on y 230. Each Setting sprite uses the classroom palette.

| Part | Placement |
|---|---|
| Door, TV and meeting board | (18, 107), (87, 92) and (85, 149). The board starts empty, so the Hard 1 card is the only one pinned on it. |
| Window and hanging lamp | (300, 68) and (384, 14). The lamp sways only in Normal 3 while the room shakes. |
| Table front, sofa, glass cabinet and boxes | (174, 188), (308, 157), (382, 105) and (426, 156). |
| Normal 2 bag | Open at (302, 191), closed at (303, 191), under the boxes at (437, 191). |
| Normal 2 supplies | Water (341, 195), can (350, 202), flashlight (360, 206), and whistle (373, 207). The row ends before the glass cabinet. |
| Normal 2 snack and toy | Snack (346, 170) on the sofa seat, toy (325, 203) on the floor. |
| Go bag by the door | Closed at (73, 191). Hard 6 starts with the bag in this spot. |
| Hard 1 cards | Park (213, 176) and house (232, 176) on the table. The card reaches the board at (109, 172). |

Normal 2 starts with the open go bag, four supplies, the snack, the toy, and Mom. The closed bag appears after packing. Normal 3 starts with the Student alone. Hard 1 starts with both cards and Mom. Props stand at the back of the floor, with their bottom row at y 212 or 213, rest on the table, or, for the snack, rest on the sofa seat. Normal 2 and Hard 1 start calm. Normal 3 begins with a shake, and the Student plays Surprised before the Action Bar appears.

The following x values mark the centre of the Student's feet at y 230. Walks and carries use the same floor line. In Normal 2 and Hard 1, the Reaction bubble sits 31 art pixels higher to clear the boxes and the meeting board. Normal 3 places the Reaction below Hint.

| Level and Choice | Act-out points |
|---|---|
| Normal 2 start | Student x 281, Mom x 158 in the clear gap between the board and the table. Both are Idle. |
| `go_bag` | Pack the four supplies at x 308, close the bag, and carry it left to x 108. Crouch to set it by the door. Mom plays Happy. |
| `later` | Turn away from the bag, walk left toward the TV, and stop at x 190, before Mom. |
| `hidden` | Pick up the open bag from x 308, carry it to x 425, and crouch at the gap under the boxes. |
| `toys` | Pick up the toy from x 322, carry it left to x 308, and stop in Pack over the bag. |
| Normal 3 start | Student x 294. No Grown-up or loose props. |
| `table` | Walk left to x 282, show the table front, crouch-shuffle to x 240, face left, and Hold the right front leg until the room calms. |
| `run` | Run left to x 91 and stop inside while the room still shakes. |
| `glass` | Walk to x 387 and stop beside the cabinet. |
| `sofa` | Walk to x 307 and stop in Crouch beside the sofa. |
| Hard 1 start | Student x 154, Mom x 290 in the clear gap between the table and the sofa. Both are Idle. |
| `meeting_place` | Pick up the park card from x 207, carry it left to x 136, reach up and pin it. Mom plays Happy. |
| `own_way` | Walk left toward the door and stop at x 91. |
| `meet_home` | Pick up the house card from x 226, carry it left to x 136, and stop in Reach Up at the board. |
| `phone_numbers` | Play Phone at x 154 and stop there. |

No Grown-up stands in front of furniture at a start, and no prop overlaps furniture. The Normal 2 door route passes Mom and the table; the bag must reach the door, and Mom must stand left of the start to leave the boxes route clear. The TV route stops before Mom. The boxes route passes the toy, water, can, flashlight, and whistle. The toy approach passes the open bag. The Normal 3 door route passes in front of the table, because the run must show the Student reaching the door, and the approach to the glass cabinet passes in front of the sofa. The Hard 1 house card route passes the park card in both directions. Try Again restores only the selected Level's props, Grown-ups, and start pose.

## Schoolyard (`ArtSource/scripts/schoolyard_v2.lua`, part `schoolyard_v2`)

Render with `aseprite.exe -b --script-param root=<project> --script-param only=schoolyard_v2 --script ArtSource/scripts/build.lua`.
The 480 × 270 schoolyard is shared by Normal 4 (during) and Hard 2 (before). The scene uses the v2 scale of four UI units per art pixel. Positions below are measured from the top left. Character positions mark the centre of the feet.

| Art or cast | Placement |
|---|---|
| Standing line and horizon | y 230 and y 132 |
| Building, roof, and doorway | (148, 120), (140, 98), and (191, 140). The roof stays below the Hint button at 1920x1080. |
| Evacuation Area sign | (15, 142). The sign uses two lines of the shared pixel font. |
| Tree trunk and shade | Trunk at (330, 127). Shade centred at (356, 194). |
| Tree canopy | (300, 83), 119 × 75 pixels. Two frames, 160 ms each. |
| Electric post and lines | (418, 95), 62 × 107 pixels. Two frames, 160 ms each. |
| Normal 4 cast and Reaction | The Student starts alone at (302, 230) and plays Surprised during the opening shake. The Reaction sits 31 art pixels to the right and 2 above the top of the Student frame. |
| Hard 2 cast and Reaction | The Student starts Idle at (306, 230). The Teacher waves inside the doorway at (217, 230). The Reaction sits 24 art pixels to the right and 2 above the top of the Student frame. |

The canopy and electric post sway only while Normal 4 shakes. Hard 2 uses their rest frames. Each Level has its own `LevelStart`. There are no movable props.

| Level and Choice | Act-out points |
|---|---|
| Normal 4 · `tree` | Walk to (320, 199) and stop at the edge of the shade. |
| Normal 4 · `field` | Walk to (110, 230) on open grass clear of the sign, face left, crouch, and Cover for one second as the yard shakes. Then calm the shaking. |
| Normal 4 · `building` | Run toward the door and stop at (260, 230), outside the door frame. |
| Normal 4 · `post` | Walk via (336, 244), then stop at (393, 244), short of the pole and its lines. |
| Hard 2 · `route_post` | Follow the same foreground path and stop at (393, 244), short of the pole. |
| Hard 2 · `route_tree` | Walk to (323, 215) and stop before the shade. |
| Hard 2 · `route_field` | The Student goes via (170, 230) to (133, 198), and the Teacher goes via (142, 225) to (101, 183) and waves. |
| Hard 2 · `route_later` | Turn toward the school and stop at (260, 230). |

The field walks cross in front of the school because the open field lies to the left of the building and both starts lie to its right. The Teacher moves ahead on the Hard 2 field path, leaving the doorway clear before the Student passes. The post path crosses the tree's x range in the foreground, clear of the canopy, trunk, and shade. The two tree approaches stop at or before the edge of the shade. Try Again after a post Choice takes the direct return to the start and passes in front of the tree trunk.

## Beach (`ArtSource/scripts/beach_v2.lua`, part `beach_v2`)

Render with `aseprite.exe -b --script-param root=<project> --script-param only=beach_v2 --script ArtSource/scripts/build.lua`.
Earthquake Normal 5 uses a 480 × 270 beach after the shaking stops. One art pixel is four UI units. Positions are measured from the top left. Student positions mark the centre of the feet.

| Art or cast | Placement |
|---|---|
| Floor line and horizon | y 230 and y 119. |
| Water line | Top left at (0, 136), with two 306 × 93 frames. Each frame lasts 420 ms, and the animation runs throughout the Level. |
| Wet sand | Exposed between the left shoreline and the dry sand, with rocks and shells. At art review, the researcher requested removal of the fish sprite. The existing Fish Choice, icon, and wording remain. |
| Flat beach mat | Top left at (175, 209). The Sit pose rests at (200, 219). |
| Tsunami evacuation sign | Top left at (405, 103). The small sign uses a wave and a person running uphill. |
| Student start and Reaction | The Student starts alone in Idle at (260, 230). The 67 × 26 art-pixel Reaction is centred above the Student and sits 2 art pixels above the frame. |
| Caption | The Goal uses the smaller pixel font so the Caption leaves the Level number and hilltop Reaction clear. |

| Choice | Act-out points |
|---|---|
| `fish` | Walk via (201, 247) to (144, 225) on the wet sand. Face left and crouch before reaching farther down the beach. |
| `watch` | Walk via (201, 247) and (113, 241) to (72, 216). Face the sea and stop on sand, clear of the water. |
| `high_ground` | Face right and fast-walk through (295, 230), (330, 209), (342, 178), and (367, 158) to (386, 151). Stand to the left of the sign. |
| `wait` | Walk to (200, 219) and sit on the mat. |

The Fish and Watch walks pass in front of the mat because the wet sand and shoreline lie to its left. Try Again from either wet-sand stop takes a direct walk in front of the mat. The Wait Choice walks onto the mat and starts its return there.

## Mall (`ArtSource/scripts/mall_v2.lua`, part `mall_v2`)

Render with `aseprite.exe -b --script-param root=<project> --script-param only=mall_v2 --script ArtSource/scripts/build.lua`.
The two Settings share a palette and both measure 480 by 270 pixels. One art pixel is four UI units. Art placements are measured from the top left. Student positions mark the centre of the feet. Neither Setting has a Grown-up actor.

### Hard 3, food court (`mall_a`)

| Art or cast | Placement |
|---|---|
| Floor | Standing line at row 230 and back edge at row 208. |
| Exit | Door at (29, 126), with the exit symbol at (39, 111). |
| Food court table | Table and stools stand to the left of the Student. The 88 by 39 pixel `foodtable_front` at (106, 192) draws above the Student during the Safe Act-out. |
| Shop window | Glazed display at (274, 121), with goods on shelves. |
| Elevator | Closed doors at (387, 121). The call button is centred at (374, 206). |
| Hanging sign | Four 55 by 29 pixel frames at (118, 119), each lasting 160 ms. The sign sways while the ground shakes and rests when it stops. |
| Student and Reaction | The Student starts at (230, 230), plays Surprised during the opening shake, and then stands Idle. The 67 by 26 art-pixel Reaction sits 30 art pixels left of centre and 2 above the Student frame, so the shop window stop leaves the right half of the glass in view. |

| Choice | Act-out points |
|---|---|
| `run_exit` | Run via (214, 247) and stop at (66, 247), on open floor left of the stool and short of the door. |
| `elevator` | Walk to (363, 230) and reach right toward the button. |
| `shop_window` | Walk to (307, 230) and stop in the aisle in front of the glass. |
| `table` | Walk to (204, 230), show the table front, and crouch to (168, 230). Hold for 0.65 seconds and calm the shaking. |

The exit run and its direct Try Again return use the foreground aisle in front of the table and stools. The run stops left of the left stool, so no scenery sits behind the stopped Student except the exit wall. The elevator and shop window walks cross in front of the display because the elevator is to its right. The Safe Act-out enters the table from the right and uses the overlay to put the Student behind its front edge.

### Hard 4, the way out (`mall_b`)

| Art or cast | Placement |
|---|---|
| Upper landing and stairs | The upper landing is at row 178. The stairs descend to row 230. The Student's `startY` is 52 art pixels above the ground-floor line. |
| Elevator | Closed doors at (21, 99), with the floor indicator below the Level label. The call button is centred at (84, 154). |
| Exit and crowd | Exit at (261, 144), with the exit symbol at (281, 129). The two still figures are drawn into the background with feet at (296, 221) and (317, 218). |
| Mall wall and open parking | The outer wall sign and glass are at (333, 131). The outer wall ends at x 373. The assembly-area sign stands at (452, 173), out in the open parking area. |
| Student and Reaction | The Student starts Idle at (129, 178). The 67 by 26 art-pixel Reaction sits 20 art pixels left of centre and 2 above the Student frame. The offset leaves the parking sign visible. |

| Choice | Act-out points |
|---|---|
| `stairs` | Walk through the shared stair and exit route, then stand in open parking at (430, 230), left of the sign and clear of the mall frontage. |
| `elevator` | Walk to (96, 178) and reach left toward the call button. |
| `run_push` | Run down the stair route and stop at (270, 221), short of the nearest person. |
| `mall_wall` | Walk through the shared stair and exit route to (357, 230), beside the outer wall. |

The stair route uses (155, 178), (191, 196), (239, 224), and (251, 230) as its slope changes. The straight sections between those points follow the intervening treads. The shared exit route continues through (279, 235), (329, 235), and (352, 230). This foreground lane passes in front of the still crowd and continues past the mall frontage. The Safe Choice crosses the curb into the open parking area.

Try Again retraces the chosen Act-out's walk or run destinations in reverse at walking speed. It climbs the stairs from the crowd and wall stops and ends on the upper landing at (129, 178).

## Removed art (cleanup, 2026-11-22)

No screen showed these, and no ticket plans them (see `.scratch/cleanup/inventory.md`):

- `Icons/icon_cross` and `Icons/icon_pencil`: the PNGs and their sources. `ui.lua` no longer draws them.
- `UI/button_gray_pressed`: the PNG and its source. Gray is only the disabled and Tried look, so `build.lua` saves no pressed state for it. `button_gray` stays.
- The third-party `Assets/8K Skybox Pack Free` and `Assets/Clean Vector Icons` packs, which the game never used.
