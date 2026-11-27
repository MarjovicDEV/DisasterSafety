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
| `icon_cover` | Duck and cover your head | Easy 2 |
| `icon_picture` | Hang a big picture over the bed | Normal 1 |
| `icon_low_shelf` | Put heavy things on a low shelf | Normal 1 |
| `icon_books` | Put books on top of the tall cabinet | Normal 1 |
| `icon_bag_hidden` | Hide it under the boxes | Normal 2 |
| `icon_go_bag` | Pack water, food, a flashlight and a whistle | Normal 2 |
| `icon_snack_toy` | Pack only snacks and toys | Normal 2 |
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
| `icon_route_post` | Take the path by the post | Hard 2 |
| `icon_route_tree` | Walk the path under the tree | Hard 2 |
| `icon_route_field` | Take the route across the field | Hard 2 |
| `icon_route_later` | Find the way when it happens | Hard 2 |
| `icon_watch_waves` | Stay and watch the waves | Normal 5 |
| `icon_high_ground` | Go to high ground right away | Normal 5 |
| `icon_wait_beach` | Wait on the beach for news | Normal 5 |
| `icon_door` (reused) | Run to the mall exit | Hard 3 |
| `icon_elevator` | Ride the elevator down | Hard 3 |
| `icon_shop_window` | Stand by the glass shop window | Hard 3 |
| `icon_strong_table` (reused) | Duck under the food court table | Hard 3 |
| `icon_stairs` (reused) | Take the stairs to the assembly area; Run to the stairs | Hard 4, Easy 2 |
| `icon_elevator` (reused) | Take the elevator down | Hard 4 |
| `icon_run_push` | Run and push to get out | Hard 4 |
| `icon_mall_wall` | Wait by the mall wall outside | Hard 4 |
| `icon_pull_up` | Pull them up to walk | Hard 5 |
| `icon_call_adult` | Call a grown-up for help | Hard 5 |
| `icon_wait_for_mom` | Wait for Mom to get up. | Hard 5 |
| `icon_door` (reused) | Leave them and go outside | Hard 5 |
| `icon_no_go_bag` | Leave without the go bag | Hard 6 |
| `icon_toy_car` | Run back in for your toys | Hard 6 |
| `icon_push_past_mom` | Push past Mom at the door. | Hard 6 |
| `icon_go_bag` (reused) | Grab the go bag and walk out | Hard 6 |

## Corridor (`ArtSource/scripts/corridor_v2.lua`, part `corridor_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=corridor_v2 --script ArtSource/scripts/build.lua`.
Writes `Level/v2/bg_corridor` (480x270). It has no hanging pots: the Unsafe Choice is running to the stairs.

All values are art pixels; one art pixel is four UI units. The floor line (the Student's feet) is y 230, as in the v2 classroom.

| Part | Place (top-left) or x |
|---|---|
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
| `stairs` | run to 394, stop | 394, at the top step (x 404) |

## Classroom after (`ArtSource/scripts/classroom_after_v2.lua`, part `classroom_after_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=classroom_after_v2 --script ArtSource/scripts/build.lua`.
It writes `Level/v2/bg_classroom_after` (480x270) and `Level/v2/classroom_after_desk_front` (84x42). The separate desk front draws over the Student. Use `only=icons_v2` and `icons=stay,teacher_walk` for the two 16x16 Choice icons.

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
| Try Again | From the door, walk back to (182, 230) before crouching. From the peek point, shuffle directly back to (222, 230). Resume Hold at (222, 230). | Resume the start position and Wave. |

## Easy art follow-up

Easy 1 and 3 share the concrete floor drawn by `classroom_v2.lua`. The floor art begins at y 208, with window light at x 305–427, y 209–236. Easy 3 also keeps the light from the open door. The Student's floor line stays at y 230.

In Easy 2, both Students use the redrawn `cover` pose. Each `boy_cover` and `girl_cover` source has two 32x50 frames at 300 ms. The Student kneels with shins folded, chin tucked and forearms over the head and neck. The upper body lowers one art pixel in the second frame, with the feet fixed.

## Bedroom (`ArtSource/scripts/bedroom_v2.lua`, part `bedroom_v2`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=bedroom_v2 --script ArtSource/scripts/build.lua`.
It writes `Level/v2/bg_bedroom` (480x270) and five props, each its own sprite: `bedroom_bed` (112x31), `bedroom_box` and `bedroom_box_placed` (24x17), `bedroom_picture` (36x29) and `bedroom_books` (16x12). Use `only=icons_v2` and `icons=picture,low_shelf,books` for the three Choice icons. Normal 1 is a "before" Level: nothing shakes and the Student starts standing calm.

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

Act-out points use the centre of the Student's feet (`BuildBedroomV2` in `ProjectBuilder.Level.cs`). A held prop is a child of the Student's body, so it mirrors with the Student. Its `holdOffset` puts its centre on the carry pose's hands at about (23, 30) in the 32x50 frame. The reach-up palms are columns 22-25 and 27-30, with the fingertips on row 4 in frame 1 and row 3 in frame 2. The picture and the books are put up with their bottom row on frame row 3 (art y 183), so both palms touch its bottom edge and the fingertips rise at most 1 pixel over it at the peak.

| Choice (`action`) | Steps and points | Stop |
|---|---|---|
| Start | `idle` at 306, facing front | |
| Safe: `low_shelf` | walk to 343, crouch and lift the box, carry it right to 384 (beside the shelf), the box shows in the shelf, crouch, stand | |
| `picture` | walk to 34, face left, reach and pick up the picture, carry it right to 140, reach up; the picture is held against the wall over the bed at (134, 155) | reaching up at 140 |
| `books_top` | walk to 316, crouch and pick up the books, carry them left to 272, reach up on tiptoe; the books are held up against the cabinet at (251, 172) | reaching up at 272 |
| Try Again | The Student walks back to 306. The bed slides back; the box, the picture and the books fade back to their start; the placed box hides. | |

Only the walk to the box passes a prop (the books beside the start). With three floor props and a bed on one floor line, one prop always sits between the start and something else, so the smallest prop takes that place.

The Student's `carry`, `reachup` and `push` poses were redrawn for this slice (`student.lua`, "Action poses"): `carry` holds the load on level forearms and leans back; `reachup` raises both arms in front of the face on tiptoe; `push` leans forward with flat palms and the back foot pushing off. Frame counts stay 4, 2 and 2. After the rollout (C7) the `reachup` hands were redrawn: the head leans back 2 pixels, the near forearm rises in front of the face with the palm over the wrist, and the far arm bends at the elbow. An outline separates the two palms, and the face keeps its outline beside the near forearm. Hard 1 keeps its own `home_reachup` frames.

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
| `hidden` | Pick up the open bag from x 308, carry it to x 425, and crouch at the gap under the boxes. |
| `toys` | Pick up the toy from x 322, carry it left to x 308, and stop in Pack over the bag. |
| Normal 3 start | Student x 294. No Grown-up or loose props. |
| `table` | Walk left to x 282, show the table front, crouch-shuffle to x 240, face left, and Hold the right front leg until the room calms. |
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
| Evacuation Area sign | (15, 116). The sign uses two lines of the shared pixel font. |
| Tree trunk and shade | The trunk forks above its roots near (350, 190). The shade is centred at (356, 194). |
| Tree canopy | (300, 83), 119 × 75 pixels. Two frames, 160 ms each. |
| Electric post and lines | (418, 95), 62 × 107 pixels. Two frames, 160 ms each. |
| Normal 4 cast and Reaction | The Student starts alone at (190, 230) and plays Surprised during the opening shake. The Unsafe Reaction sits 31 art pixels to the right and 2 above the Student frame. The field Reaction is lower to clear the sign. |
| Hard 2 cast and Reaction | The Student starts Idle at (306, 230). The Teacher waves inside the doorway at (217, 230). The Unsafe Reaction sits 24 art pixels to the right and 2 above the Student frame. The Safe Reaction sits farther right to clear the Teacher. |

The canopy and electric post sway only while Normal 4 shakes. Hard 2 uses their rest frames. Each Level has its own `LevelStart`. There are no movable props.

| Level and Choice | Act-out points |
|---|---|
| Normal 4 · `tree` | Walk along the foreground to (287, 230), then turn toward (320, 199) and stop at the edge of the shade. |
| Normal 4 · `field` | Walk 160 art pixels in two seconds to (30, 230) on open grass west of the path, face left, crouch, and Cover for one second as the yard shakes. Then calm the shaking. |
| Normal 4 · `building` | Run toward the door and stop at (207, 236), on the forecourt outside the door frame. |
| Hard 2 · `route_post` | Walk via (336, 244) and stop at (393, 244), short of the pole. |
| Hard 2 · `route_tree` | Walk to (323, 215) and stop before the shade. |
| Hard 2 · `route_field` | The Teacher steps out to (217, 243) before the Student joins at (251, 243). They cross to (60, 243) and (94, 243), respectively. Both turn upfield. The Teacher waves at (52, 206) and the Student stops at (84, 210). |
| Hard 2 · `route_later` | Turn toward the school and stop at (260, 230). |

The approved Normal 4 field stop is 118 art pixels left of the wall. The Student crouches and covers there. The field Reaction appears below the Evacuation Area sign. In Hard 2, both walkers clear the left corner before they turn toward the sign. The Safe Reaction appears to the right of the Teacher. Try Again retraces the foreground bends before returning the Student to the Level start.

The school frontage uses the Baluan classroom entrance and corridor as references for the green door, barred windows, and shaded concrete walkway. The tree separates its trunk and branches from the canopy. The post has a crossarm, insulators and visible wires.

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
| Exit and crowd | Exit at (261, 144), with the EXIT sign above the opening. The two still figures are drawn into the background with feet at (342, 212) and (361, 210). |
| Mall wall and open parking | The outer wall sign and glass are at (333, 131). The outer wall ends at x 373. The assembly-area sign stands at (452, 173), out in the open parking area. |
| Exterior corner (C11) | The exterior wall runs from (333, 0) to the pavement in the darker wall shade. Its corner at x 373 is outlined from the cornice (rows 0 to 8, out to x 377) down to a stone plinth (rows 208 to 215, out to x 375). |
| Curb and parking lines (C11) | A low curb closes the far edge of the lot from x 374: an outline on rows 189 and 195, a lit top face and a stone front face. The asphalt starts on row 196. The painted lines are 2 art pixels thick and lean away from a vanishing point at (240, 0), like the pavement edge. The back stalls have dividers from x 384, 412 and 440 and end on rows 213 and 214. The front stall around the Safe ending lies between dividers from (398, 217) and (452, 217) and ends on rows 246 and 247. |
| Assembly box (C11) | A green painted box (the sign green) inside that stall runs from (409, 220) and (452, 220) to (412, 242) and (466, 242). The Safe ending stands inside it, under the sign. |
| Student and Reaction | The Student starts Idle at (129, 178). The 67 by 26 art-pixel Reaction sits 20 art pixels left of centre and 2 above the Student frame. The offset leaves the parking sign visible. |

| Choice | Act-out points |
|---|---|
| `stairs` | Walk through the shared stair and exit route, then stand in open parking at (430, 230), inside the green assembly box, left of the sign and clear of the mall frontage. |
| `elevator` | Walk to (96, 178) and reach left toward the call button. |
| `run_push` | Run down the stair route and stop at (270, 221), short of the nearest person. |
| `mall_wall` | Walk through the shared stair and exit route to (357, 230), on the pavement against the outer wall's plinth. |

The stair route uses (155, 178), (191, 196), (239, 224), and (251, 230) as its slope changes. The straight sections between those points follow the intervening treads. The shared exit route continues through (305, 221), (317, 233), and (352, 230). The route crosses the open threshold and continues past the mall frontage. The Safe Choice crosses the curb into the open parking area.

Try Again retraces the chosen Act-out's walk or run destinations in reverse at walking speed. It climbs the stairs from the crowd and wall stops and ends on the upper landing at (129, 178).

## Home after (ArtSource/scripts/home_v2.lua, part home_after_v2)

The Setting uses the same 480 by 270 living room as Normal 2, Normal 3 and Hard 1. It uses the same palette, furniture and one-pixel outlines. Neither Hard 5 nor Hard 6 shakes. Sprite points refer to their top-left corners. Actor points give the centre of the feet, measured from the top left of the art. Row 230 is the floor reference, so actor height in the builder is 230 minus the art row.

| Item | Placement |
|---|---|
| Front door and path | The original doorway at (18, 107) is open onto a sunlit path. The bottom of the threshold is at row 212. |
| Table, sofa, window and cabinet | These stay at their original points. The table front sits at (174, 188). |
| Inner room | A dim doorway at (429, 113) replaces the storage bench. A toy car sits on a shelf inside. |
| Fallen things | The tilted picture starts at (208, 144), the books at (394, 217), and the toppled chair at (187, 248). The chair lies on its side, seen from the front, as a 43 by 19 pixel shape of straight bars: a slatted back, the seat on edge and two legs, so it cannot be mistaken for an animal. |
| Go bag | Hard 6 uses the closed go bag from Normal 2 at (73, 191). |
| Family note and pen | In Hard 6 the written note is on the table at (232, 180) from the start. The pen is at (245, 181). |

### Hard 5, help for Mom

The Student starts in Idle at (286, 230). Mom sits at (322, 233) in HurtSit, facing left. Dad's start is (-24, 238), beyond the left edge even with his full 36-pixel frame. He enters only in the Safe Act-out and uses the existing walk and kneel frames. The 67 by 26 art-pixel Reaction sits 31 art pixels right of centre and 14 above the Student frame. The doorway call uses an offset of 51 art pixels to the right to keep the door and Mom visible.

| Choice | Act-out points |
|---|---|
| call_adult | Walk to (284, 242), crouch facing Mom and say "Stay still, Mom." Stand and walk through (269, 242) to (56, 212). Face left in Call and say "Help!" Dad crosses in front of the Student at (56, 249), walks to (265, 241), then kneels at (286, 244). |
| pull_up | Walk to (284, 234), then reach toward Mom's hand and stop short of touching her. |
| wait_for_mom | Walk to (284, 234) beside Mom and pause with hands down. |
| leave | Walk through (269, 242) and stop inside at (100, 232). |

The leftward route passes in front of the table to keep the Student's feet clear of its legs. Dad follows the foreground aisle below the doorway, the Student and the table legs before kneeling by Mom. The fallen chair is farther forward than the aisle. The two Act-out lines use the Student's usual speech bubble and one voice blip each.

### Hard 6, leave with Mom and the go bag

The Student starts in Idle at (286, 230). Mom starts in Idle at (38, 215). The Reaction is 67 by 26 art pixels, 55 art pixels right of centre and 2 above the Student frame. It mirrors to the left near the right edge to leave the inner-room doorway visible. This also keeps the front door clear when the Student carries the bag through it.

| Choice | Act-out points |
|---|---|
| go_bag | Cross the foreground aisle to pick up the bag at (106, 225). Mom steps ahead to (32, 183) during the approach. Carry it through the threshold at (56, 212) to (56, 206). Mom walks through (35, 178) to (40, 171) on the outside path. |
| no_go_bag | Follow the aisle and stop inside at (114, 232) with empty hands. |
| toys | Run through (363, 241) and stop at (441, 235), before the inner-room threshold. |
| push_past_mom | Follow the aisle to the bag at (106, 225), pick it up, and carry it to (91, 232). The Student stops on indoor floor before reaching Mom. The written note remains on the table. |

The aisle uses (282, 236) and (162, 236). It passes in front of both table legs. The Go Bag stays by the door until the Student has cleared the table. The route toward the inner room passes in front of the sofa and cabinet, clear of the fallen books at floor level. At the front door, Mom leads on the left side and the Student follows with the bag on a separate track. The unsafe stops end on indoor floor.

Try Again returns the Student to the starting pose and position and restores the props to their starting state. Mom and Dad remain at their starting places during every Unsafe Act-out.

## Evacuation area (`ArtSource/scripts/evacuation_area_v2.lua`, part `evacuation_area_v2`)

The Setting shows a later gathering at a fictional evacuation centre: an open-sided covered basketball court in daylight, after the earthquake. It is 480 by 270 art pixels and does not shake. Sprite points refer to their top-left corners. Actor points give the centre of the feet, measured from the top left of the art. Row 230 is the floor reference, so actor height in the builder is 230 minus the art row. The positions are also in `M.LAYOUT` in the script.

The court uses `school-covered-court.jpg` for its architecture only: a broad rust-red metal roof, exposed trusses and pale columns with open sides. The roof and column colours match the Hard menu court. The photo does not show that the real court is an evacuation centre. The figures are original, and the photographed people, vehicles and map controls are left out. The researcher approved the preview on 2026-11-26.

At 2400 by 1080 the art fills the width and is anchored at the bottom, so the top 54 rows are cut off. The roof therefore comes down to row 64, with the trusses and bottom chord down to row 80. The HINT button covers x 212 to 268 on rows 120 to 138 at that size, so nothing sits at the back between the columns at x 204 and 300.

| Item | Placement |
|---|---|
| Roof and trusses | The roof's underside fills rows 0 to 63 from the left edge to its end at x 438. A zigzag truss runs below it, with the bottom chord at row 80. The sky shows beyond the end of the roof. |
| Columns | Five pale columns, 10 pixels wide, stand at x 12, 108, 204, 300 and 396 from the chord to the back of the slab at row 154. Trees, hills and grass show between them. |
| Sign | The plain "EVACUATION CENTER" sign hangs from the chord on two wires at (14, 86), 88 by 26. It has dark letters on a pale board, below the Level label and left of the Caption at both sizes. |
| Hoop | The poles stand at (28, 207) and (414, 207). Slim orange rims face inward on row 153. |
| Court | The side-view court has a centre circle and a painted free-throw lane at each end. |
| Tents and people | A blue tent stands at the back from x 118 to 168 and an orange tent from x 310 to 362. Both bases are on row 158. An adult and a child stand at the blue tent's opening, with feet at (128, 160) and (140, 161). They stay left of the Reaction at the Safe ending. Another adult and child stand at the back on the right, at (374, 156) and (388, 157). |
| Mat | The woven mat runs from x 96 to 208 at row 226 and from x 86 to 218 at row 246. |
| Mom | Mom sits on the mat in `Sit` with her feet at (118, 236). She is a Grown-up actor, not part of the background. |
| Radio | The radio is two props at (136, 208), both 28 by 30: `evacuation_area_radio` (off) and `evacuation_area_radio_on` (two frames of 260 milliseconds, with yellow sound arcs above the radio). The sound arcs stay inside the mat's x range. |
| Neighbours | Three neighbours chat at (272, 164), 86 by 62, in two frames of 600 milliseconds: a nod and a hand gesture. They reuse the v2 Grown-up frames, with new clothes below the shoulders and grey hair on the older neighbour. |
| Court edge | Grass borders the open right edge of the concrete slab. |

### Hard 7, listen to the radio

The Student starts in Idle at (228, 230). Mom sits on the mat and stays there in every Act-out. The Reaction uses the default place above the Student, except at three places. At the go home and rumor stops it sits beside the Student, so the neighbours stay in view. At the Safe ending it sits 24 art pixels to the right, so the blue tent stays in view.

| Choice | Act-out points |
|---|---|
| radio | Walk to (182, 236) and sit beside Mom. The radio then turns on beside her, because Mom has no reaching pose. |
| go_home | Walk to (376, 248) and stop on the court, right of the neighbours, at the open side. The Student stands inside the court paint, and the right edge (x 392) stays well left of the grass border (x 432). The Reaction sits 20 art pixels to the right of the Student. |
| rumor | Walk to (254, 228), face the neighbours, take the `Call` pose and say "Did you hear...?" The Student's right edge (x 270) stays left of the first neighbour. The Reaction sits 18 art pixels to the left of the Student. |
| phone_all_day | Take the `Phone` pose in place at the start. |

Try Again returns the Student to the start in Idle and turns the radio off.

## Tutorial Pages (v2) (`ArtSource/scripts/tutorial_v2.lua`, part `tutorial_v2`)

How to Play has seven Tutorial Pages. Pages 1, 2 and 7 keep the pilot pictures from `tutorial.lua` (topic, difficulty and stars), which this script does not change. Pages 3 to 6 show the Action Bar flow with four new pictures. Each is 136 by 76 art pixels with two frames of 400 milliseconds, saved as `Assets/Art/Tutorial/tutorial_v2_<name>_0.png` and `_1.png`. They use the v2 classroom, the v2 Student frames and the v2 Choice icons.

| Picture | Student | Frame 0 | Frame 1 |
|---|---|---|---|
| `v2_actionbar` | Boy | The boy stands at the classroom start spot above an Action Bar of two buttons (door, desk). | The desk button is pressed: it sits lower with a yellow ring. |
| `v2_actout` | Girl | The girl holds the desk leg under the desk in `Hold`, with the desk front drawn over her. | The second `Hold` frame. |
| `v2_feedback` | Boy | The Feedback panel with a green check and the "Safe!" heading, and the boy in `Happy` beside it. | The same, with two small sparkles. |
| `v2_hint` | Girl | The Caption bar with Easy 1's Hint ("Find something strong to hide under.") and the HINT button below it. The girl stands on the right. | The HINT button is pressed. |

## Menu backgrounds (`ArtSource/scripts/background.lua`, part `menu`)

Run: `aseprite.exe -b --script-param root=<project> --script-param only=menu --script ArtSource/scripts/build.lua`. It writes `Background/bg_menu_<name>.png` and its source for each `MenuBackground`. The v1 part still draws the flag, clouds and birds from the same script. The courtyard `bg_courtyard` was removed when the gate replaced it.

Each background is 480x270 and uses the v2 style: one dark outline, three shades and light from the left. The researcher approved the previews on 2026-11-25. The reference photos stay in `earthquake-changes-scratch/references`. The art omits the photographed people, vehicles, seals, map controls and fine print.

| Background | Screen | Reference details used |
|---|---|---|
| `gate` | Main Menu and its dialogs | `baluan-gate.jpg`: dark horizontal metal gate between brick pillars, red perimeter wall with a blue top edge, tall palms, power lines, open driveway. `school-perimeter-sign.jpg`: long pale sign reading "BALUAN ELEMENTARY SCHOOL" on the red wall, palm above it. |
| `noticeboard` | Episode selection | No photo. A neutral cork board on a cream wall with green trim, with blank pinned papers. |
| `drill_ground` | Earthquake Episode background, on Difficulty selection | No photo. An open field with painted lines, class markers, cones, the flagpole and the Evacuation Area sign, in front of cream school blocks with green doors and barred windows. |
| `fire_station` | Fire Episode background, on Difficulty selection | No photo. A station with an open bay, a shut red door, a hose tower and a parked engine on the forecourt. |
| `classroom` | Earthquake Easy Difficulty background, on Level selection | `classroom-chair-rows.jpg`, `classroom-front-left.jpg`: bright green slatted chairs and a dark ruled board. `classroom-board.jpg`: wall screen over the board and green curtains at its ends. `classroom-windows.jpg`: green window frames with patterned curtains. `classroom-front-right.jpg`: cream wall with green trim. `classroom-window-corner.jpg`: low green shelving. |
| `schoolyard` | Earthquake Normal Difficulty background, on Level selection | `classroom-entrance.jpg`: bright green doors and barred windows in a cream wall. `classroom-corridor.jpg`: shaded concrete walkway with pale posts. |
| `covered_court` | Earthquake Hard Difficulty background, on Level selection | `school-covered-court.jpg`: broad rust-red metal roof, exposed trusses, pale columns and a low green fence. The photo does not show that the court is a shelter. |
| `station_noticeboard` | Fire Difficulty background, on Level selection while Fire has no Playable Level | No photo. A simple cork board with a red frame, a hose reel and an extinguisher. |

The menu switches the moving layers with the background (`MenuWorld`). The flag flies on `gate` and `drill_ground`. Clouds and birds appear on every outdoor background. The two walking children appear on `gate`, `drill_ground` and `schoolyard`. Leaves fall on `drill_ground` and `schoolyard`. The sign sits at y 124 to 143, above the greeting bubble at both 1920 and 2400 widths.

## Removed art (cleanup, 2026-11-22)

No screen showed these, and no ticket plans them (see `.scratch/cleanup/inventory.md`):

- `Icons/icon_cross` and `Icons/icon_pencil`: the PNGs and their sources. `ui.lua` no longer draws them.
- `UI/button_gray_pressed`: the PNG and its source. Gray is only the disabled and Tried look, so `build.lua` saves no pressed state for it. `button_gray` stays.
- The third-party `Assets/8K Skybox Pack Free` and `Assets/Clean Vector Icons` packs, which the game never used.

## Fire Batch 1 — Difficulty backgrounds (approved)

Run from the project root with Aseprite:

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=menu_fire --script ArtSource/scripts/build.lua
```

Each basename below has an editable source in `ArtSource/Background/<basename>.aseprite` and an export in `Assets/Art/Background/<basename>.png`. All three are 480×270, one still frame each, with no animation timing. Coordinates below are art pixels from the upper left. The pictures are backdrops for Level selection. Student and Grown-up starts and Act-out points do not apply.

| Basename | Floor and landmark placement |
| --- | --- |
| bg_menu_fire_easy | Floor starts at y=208. Door (112,104), cabinet (12,104), window (308,92), sofa (321,167), mat top edge (302,222) to (395,222). |
| bg_menu_fire_normal | Floor starts at y=210. Doors (48,74) and (284,74), windows (125,89) and (358,89), Fire Bell box (16,99), EXIT sign (293,56). |
| bg_menu_fire_hard | Road starts at y=201. Truck body (25,177), rear wheel centre (51,223), pole (94,86), shop counter (367,190), palm base (390,191). |

Approved comparison sheet: `D:/GameDEV/fire-episode-scratch/art/01-menu-fire/preview.png`. Each widescreen view shows rows 54–269 at 5×.

## Fire Batch 2 — Fire effects and Fire Bell (approved)

From the project root run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=fire_fx --script ArtSource/scripts/build.lua`. Editable files use `ArtSource/Level/v2/<name>.aseprite`. PNGs use `Assets/Art/Level/v2/` and the filenames below. Number ranges include both endpoints.

| Source name | PNGs | Art pixels | Frames | Timing per frame | Placement |
| --- | --- | --- | --- | --- | --- |
| fire_bell | fire_bell_0.png through fire_bell_1.png | 14×14 | 2 | 140 ms | Top-left wall mounts in the preview: classroom (104,129), corridor (263,117), mall_a (158,126). |
| smoke_band | smoke_band_0.png through smoke_band_2.png | 480×110 | 3 | 220 ms | Scene top-left (0,0). Repeat horizontally every 480 pixels. |
| flames_small | flames_small_0.png through flames_small_3.png | 20×24 | 4 | 150 ms | Base centre (10,23) in sprite space. Align to the Level's flame source. |
| flames_pan | flames_pan_0.png through flames_pan_2.png | 16×10 | 3 | 150 ms | Base centre (8,9). Align to the pan rim. |
| sparks | sparks_0.png through sparks_2.png | 14×12 | 3 | 110 ms | Centre (7,6). Align to the outlet. |
| night_overlay | night_overlay.png | 480×270 | 1 | Still | Bedroom top-left (0,0). Window starts at (48,54), lamp centre (240,22). |

These overlays introduce no floor, Student or Grown-up starts, or Act-out points. Existing Settings retain their placements. Approved sheet: `D:/GameDEV/fire-episode-scratch/art/02-fire-fx/preview.png`.

## Fire Batch 3 — Student poses (approved)

Run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=student_fire --script ArtSource/scripts/build.lua`. To export individual poses, add `--script-param poses=crawl,signal` with the desired names. The shared overlay selects as `sleeve_flame`.

Each row except the overlay names two source files, `ArtSource/Student/boy_<pose>.aseprite` and `ArtSource/Student/girl_<pose>.aseprite`. Each frame exports to `Assets/Art/Student/<boy|girl>_<pose>_<index>.png`. Coordinates are art pixels from the top left of the frame. The floor is the bottom edge, y=50. Anchors below are registration points for placing the pose against the floor.

| Pose | Frame indices | Size | Duration per frame | Registration and playback |
| --- | --- | --- | --- | --- |
| crawl | 0–3 | 32×50 | 160 ms | Ground (16,49) throughout. Loop facing right. |
| walk_low | 0–3 | 32×50 | 160 ms | Feet (16,49) throughout. Handkerchief included. |
| feel_door | 0–1 | 32×50 | 300 ms | Ground (16,49) both frames. Place the door surface at local x=31 for the extended hand. |
| signal | 0–1 | 32×50 | 220 ms | Feet (16,49) both frames. Waving cloth included. |
| stop_drop_roll | 0–5 | 32×50 | 240 ms | Frame 0 feet (16,49). Frame 1 ground (16,49). Frames 2, 3, 4 and 5 each use hip (8,41), eight pixels above ground registration. Remove the sleeve flame before frame 1. |
| sleeve_flame | 0–2 | 6×8 | 150 ms | One shared source `ArtSource/Student/sleeve_flame.aseprite`. Exports `Assets/Art/Student/sleeve_flame_0.png` through `_2.png`. Place its top-left at (1,23) on surprised or (9,21) on stop, relative to the unmirrored Student. Mirror with the Student. |

The poses introduce no Setting, prop in the world, or Grown-up start. Approved sheet: `D:/GameDEV/fire-episode-scratch/art/03-student/preview.png`.

## Fire Batch 4 — Firefighter and Grown-up poses (approved)

From the project root run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=grownups_fire --script ArtSource/scripts/build.lua`. An optional `poses=` list selects pose names. Each basename below has one source in `ArtSource/GrownUp/<basename>.aseprite` and one PNG per listed frame in `Assets/Art/GrownUp/<basename>_<index>.png`. Every frame measures 36×62. Register actors at (18,61), measured from the upper left. For kneeling this marks ground under the figure.

| Basename | Frames | Duration per frame | Placement notes |
| --- | --- | --- | --- |
| firefighter_idle | 0–1 | 500 ms | Feet stay planted. |
| firefighter_walk | 0–3 | 140 ms | Right-facing loop. |
| firefighter_wave | 0–1 | 260 ms | Standing greeting. |
| firefighter_kneel | 0–1 | 400 ms | Right-facing. Glove reaches (33,37) then (33,36). |
| mom_lid | 0–1 | 400 ms | Align lid handle to (33,33), then stove knob to (33,40). |
| mom_reach | 0–1 | 400 ms | Candle hand at (32,33). Flashlight included in hand at (32,28) for the handover. |
| dad_switch | 0–1 | 300 ms | Align wall switch to (32,9), then (32,12). |
| dad_phone | 0–1 | 350 ms | Mobile phone included at the ear. |
| teacher_count | 0–1 | 350 ms | Clipboard and moving pencil included. |

World starts and Act-out destinations belong to the Level layouts. Approved sheet: `D:/GameDEV/fire-episode-scratch/art/04-grownups/preview.png`.

## Fire Batch 5 — Easy Settings, props and Choices (approved revision)

From the project root run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=home_night,kitchen,icons_fire_easy --script ArtSource/scripts/build.lua`. Use `icons=fire_candle` or a comma-separated list to restrict the icon part. Every basename has a source `ArtSource/<folder>/<basename>.aseprite` and exports under `Assets/Art/<folder>/`. Stills use the basename plus `.png`. Pairs export `_0.png` and `_1.png`.

| Folder | Basenames | Size in art pixels | Frames and timing |
| --- | --- | --- | --- |
| Level/v2 | bg_home_night, bg_kitchen | 480×270 each | Still |
| Level/v2 | home_lighter | 6×4 | Still |
| Level/v2 | home_night_plate, home_night_flashlight | 18×5, 12×8 | Still |
| Level/v2 | home_night_candle, home_night_curtain | 6×12, 15×100 | Two each, 240 ms and 420 ms respectively |
| Level/v2 | kitchen_pot, kitchen_burner | 28×20, 22×6 | Two each, 260 ms and 180 ms respectively |
| Level/v2 | kitchen_pan, kitchen_pan_lid, kitchen_cup | 37×10, 24×9, 11×12 | Still |
| UI | icon_fire_give_mom, icon_fire_lighter, icon_fire_tell_mom, icon_fire_candle, icon_fire_call_mom, icon_fire_move_pot | 16×16 each | Still |

Coordinates below use art pixels from the upper left. Actor coordinates are the centre of the feet. Prop coordinates are top-left. Approved poses and placement snapshots: `D:/GameDEV/fire-episode-scratch/art/05-easy/preview.png`.

- Easy 1, home: floor back y=208, walking line y=230. Student starts (237,230), picks up the lighter at (201,230), then gives it to Mom from (179,230). Mom stands at (156,230). The lighter starts at (196,229), moves to (190,223) at pickup and (168,209) at handover.
- Easy 2, home_night: floor back y=208. Student starts at (147,230) and receives the flashlight at (206,230). Mom starts at (243,226), then hands it over from (178,221). Curtain (292,65), candle table (261,177), candle (289,166), plate (193,180), moved candle (199,170). The flashlight is drawn in Mom's hand for handover. Use the separate 12×8 prop once released.
- Easy 3, kitchen: floor back y=206. Student starts and calls at (176,230). Mom starts outside the screen door and comes to the revised control position (295,210). The left knob centre is (310,189). Burner (314,170), boiling pot (311,155). Hide the burner when Mom turns down the heat.
- Normal 4, kitchen: Student starts and calls at (176,230). Mom starts at (258,211). Cup (422,152), pan (301,166), small pan Flames (317,160), lid (254,154), covered lid (313,162). Use the same revised control position (295,210). Exit at (59,213) for the Student and (41,210) for Mom. The LPG tank and both hose fittings are drawn into the background. Actors stay in the open aisle.

## Fire Batch 6 — Normal props and Choice icons (approved)

Run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=fire_normal_props,icons_fire_normal --script ArtSource/scripts/build.lua`. The icon part accepts a comma-separated `icons=` list of names beginning with `fire_`. Each basename has a matching editable source in `ArtSource/<folder>/` and PNG in `Assets/Art/<folder>/`. Coordinates are art pixels from the upper left.

| Folder | Basename | Size | Frames and timing | Placement in the review sheet |
| --- | --- | --- | --- | --- |
| Level/v2 | classroom_bag | 18×23 | Still | Top left (284,187), on the chair. |
| Level/v2 | corridor_cr_door | 50×94 | Still | Top left (22,114), threshold at the back of the floor. |
| Level/v2 | corridor_handkerchief | 8×7 | Still | Separate hand prop. The walk_low pose already includes a cloth at the nose. |
| Level/v2 | home_outlet | 34×30 | Three frames, 150 ms each | Top left (271,149). PNGs home_outlet_0.png through home_outlet_2.png. |
| Level/v2 | home_switch_box | 22×29 | Still | Top left (148,161). Lever aligned with Dad's raised hand. |
| UI | icon_fire_line_up, icon_fire_bag, icon_fire_stay_low, icon_fire_run_smoke, icon_fire_hide_cr, icon_fire_stay_class, icon_fire_shout, icon_fire_water, icon_fire_carry_pan, icon_fire_tell_dad, icon_fire_pull_plugs | 16×16 each | Still | Choice icons. |

The existing classroom and corridor walking lines are y=230. In the corridor preview the Student's feet are at (307,230), following the Teacher at (348,230). In the home preview the Student stands back at (104,235) and Dad operates the switch from (144,221). This batch adds props to existing Settings. Approved sheet: `D:/GameDEV/fire-episode-scratch/art/06-normal/preview.png`.

### Original three-frame Smoke preview

Batch 10 supplies the final Smoke animations and playback guidance. The original Smoke source contains three distinct 480×110 frames at 220 ms each. Their patterns shift left by eight art pixels per frame. For continuous drift, play frames 0, 1 and 2 in order, then shift the tiled layer left by 24 art pixels when returning to frame 0. Wrap the displacement at 480 pixels. This produces a seamless 60-step, 13.2-second cycle. The art preview uses the imported PNGs over the Hard 1 bedroom: `D:/GameDEV/fire-episode-scratch/art/07-bedroom/smoke-drift.gif`. Unity playback belongs to the gameplay integration ticket.

## Fire Batch 7 — Hard bedroom props and Choices (approved)

Run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=bedroom_fire,icons_fire_bedroom --script ArtSource/scripts/build.lua`. The icon part accepts an optional `icons=` list. Each basename has a matching editable source under `ArtSource/<folder>/` and PNG under `Assets/Art/<folder>/`.

| Folder | Basename | Art pixels | Frames | Top-left placement in the bedroom |
| --- | --- | --- | --- | --- |
| Level/v2 | bedroom_door | 58×107 | _0 closed, _1 open, 300 ms source timing. Hold the appropriate state. | (319,102) |
| Level/v2 | bedroom_closet_door | 46×92 | Still | (236,122) |
| Level/v2 | bedroom_toy | 16×18 | Still | (287,198) |
| Level/v2 | bedroom_blanket | 31×15 | Still | (148,191) on bed |
| Level/v2 | bedroom_blanket_gap | 55×6 | Still | (320,205) below closed door |
| Level/v2 | bedroom_cloth | 17×13 | Still | (394,155) on shelf. The signal pose includes the raised cloth. |
| Level/v2 | bedroom_window_open | 68×86 | Still | (50,57) within existing window |
| UI | icon_fire_crawl, icon_fire_hide_bed, icon_fire_run_standing, icon_fire_toy, icon_fire_feel_door, icon_fire_open_door, icon_fire_closet, icon_fire_wait_bed, icon_fire_signal, icon_fire_jump, icon_fire_video | 16×16 each | Still | Choice icons |

Actor coordinates below refer to the centre of the feet or ground registration in art pixels. The existing bedroom walking line is y=230. The bed is at (104,194). In the Hard 1 preview the Student crawls toward the open door from (299,230). Hard 2 shows the door check at (304,216) and the second exit at (86,221). Hard 3 shows the Student signaling at (86,215). The Firefighter appears outside the window, cropped to the opening at (66,99); the visible portion is 36×41. Approved sheet: `D:/GameDEV/fire-episode-scratch/art/07-bedroom/preview.png`.

## Fire Batch 8 — Yard and siga (approved)

Run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=yard,icons_fire_yard --script ArtSource/scripts/build.lua`. Use `icons=` to select individual Choice icons. Each basename has an editable source under `ArtSource/<folder>/` and PNG exports under `Assets/Art/<folder>/`.

| Folder | Basename | Art pixels | Frames and timing |
| --- | --- | --- | --- |
| Level/v2 | bg_yard | 480×270 | Still |
| Level/v2 | yard_siga | 46×31 | Four frames, _0 through _3, 160 ms each |
| UI | icon_fire_stop_drop_roll, icon_fire_run_help, icon_fire_beat_hands, icon_fire_wave_arms | 16×16 each | Still |

Art coordinates from the upper left: the lawn begins at y=169 and the Student uses ground y=230. Place the siga at top-left (94,194). The Student starts at (181,230), stops and drops there, then moves across the clear grass. The six ground registration points in order are (181,230), (181,230), (181,230), (196,230), (211,230), and (226,230). Use the Batch 3 frame anchors. Show the shared sleeve flame on the stop frame at local (9,21); remove it on drop. No Grown-up enters this Act-out. The mango tree is drawn at (8,18), the clothesline spans x=252–442, and the water drum stands at (408,148). Approved sheet: `D:/GameDEV/fire-episode-scratch/art/08-yard/preview.png`.

## Fire Batch 9 — Mall art (approved)

Run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=mall_fire,icons_fire_mall --script ArtSource/scripts/build.lua`. Each basename has one editable source under `ArtSource/<folder>/` and one PNG under `Assets/Art/<folder>/`. All three are still frames.

| Folder | Basename | Art pixels | Placement |
| --- | --- | --- | --- |
| Level/v2 | mall_exit_sign | 48×15 | Top-left (30,110) above the food-court doorway. |
| Level/v2 | mall_toy_shop_toy | 27×15 | Top-left (317,147) on the toy-shop shelf. |
| UI | icon_fire_exit_stairs | 16×16 | Choice icon. |

Both mall backgrounds retain their existing layout. The preview places the Fire Bell at (158,126) and light Smoke at (0,0). Student and Mom start at (229,230) and (253,230), measured at their feet. The food-court walking line is y=230. The landing in mall_b is y=178; use the existing stair points in mall_v2.lua. Preview mid-descent positions are (179,190) and (201,203). The outside Meeting Place is (430,230). Approved sheet: `D:/GameDEV/fire-episode-scratch/art/09-mall/preview.png`.

## Fire Batch 10 — Street, street after and revised Smoke (approved)

From the project root run `aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=street,street_after,icons_fire_street,fire_smoke_motion --script ArtSource/scripts/build.lua`. The icon part accepts a comma-separated `icons=` list. Each basename has one editable source under `ArtSource/<folder>/` and exports under `Assets/Art/<folder>/`.

| Folder | Basenames | Art pixels | Frames and timing |
| --- | --- | --- | --- |
| Level/v2 | bg_street, bg_street_after | 480×270 | Still |
| Level/v2 | street_cat, street_phone, street_hiding_box | 21×17, 7×11, 43×34 | Still |
| Level/v2 | smoke_drift | 480×110 | Frames 0–23, 80 ms each |
| Level/v2 | street_roof_smoke | 67×73 | Frames 0–23, 80 ms each |
| UI | icon_fire_call_911, icon_fire_watch, icon_fire_film, icon_fire_run_inside, icon_fire_go_firefighter, icon_fire_hide_firefighter, icon_fire_cat, icon_fire_run_home | 16×16 each | Still |

Placement coordinates use art pixels from the upper left. Actor coordinates mark the centre of the feet. Prop and effect coordinates mark the top left.

- **Hard 6, street.** The near pavement begins at y=188. The Student starts at (168,233), steps back to (150,233), and calls to Dad at (126,234). Dad's phone pose includes the phone. For another pose, align the separate phone to the raised hand. The distant window Flames start at (291,110) and the roof plume at (279,17). The isawan cart is drawn at (213,133) and the vendor at (259,150), in the daytime backdrop.
- **Hard 7, street_after.** The Student starts at (191,230) and meets the kneeling Firefighter at (347,230). The Firefighter enters the preview at (265,229) and kneels at (320,229). Place the cat at (71,183) and the crates at (411,174). The parked engine body starts at (109,137). The Meeting Place sign is at (337,166). The stain above the home's window is part of the backdrop.
- **Smoke playback.** Use the shared ceiling band in the corridor, the night bedroom for Hard 1–3, and both mall Settings. Anchor it at (0,0), repeat horizontally every 480 art pixels, and advance the texture left two pixels per 80 ms frame. Each sprite cycle lasts 1.92 seconds. After ten cycles both the frame and scroll position return to their starting points (19.2 seconds). The mall preview uses half the band opacity. The street plume cycles in place at its roof anchor. Hard 7 shows the fire out. For Unity integration, advance frame and scroll from the same pausable Level clock and apply the entry fade specified in the Fire design.

Approved Batch 10 sheet: `D:/GameDEV/fire-episode-scratch/art/10-street/preview.png`. Animated Smoke comparison: `D:/GameDEV/fire-episode-scratch/art/10-street/smoke-motion.gif`.

## Fire art file inventory

The ten approved batches contain 210 PNGs and 105 editable Aseprite sources. Paths below are relative to the project root.

<details>
<summary>01-menu-fire (6 files)</summary>

```text
ArtSource/Background/bg_menu_fire_easy.aseprite
ArtSource/Background/bg_menu_fire_hard.aseprite
ArtSource/Background/bg_menu_fire_normal.aseprite
Assets/Art/Background/bg_menu_fire_easy.png
Assets/Art/Background/bg_menu_fire_hard.png
Assets/Art/Background/bg_menu_fire_normal.png
```

</details>

<details>
<summary>02-fire-fx (22 files)</summary>

```text
ArtSource/Level/v2/fire_bell.aseprite
ArtSource/Level/v2/flames_pan.aseprite
ArtSource/Level/v2/flames_small.aseprite
ArtSource/Level/v2/night_overlay.aseprite
ArtSource/Level/v2/smoke_band.aseprite
ArtSource/Level/v2/sparks.aseprite
Assets/Art/Level/v2/fire_bell_0.png
Assets/Art/Level/v2/fire_bell_1.png
Assets/Art/Level/v2/flames_pan_0.png
Assets/Art/Level/v2/flames_pan_1.png
Assets/Art/Level/v2/flames_pan_2.png
Assets/Art/Level/v2/flames_small_0.png
Assets/Art/Level/v2/flames_small_1.png
Assets/Art/Level/v2/flames_small_2.png
Assets/Art/Level/v2/flames_small_3.png
Assets/Art/Level/v2/night_overlay.png
Assets/Art/Level/v2/smoke_band_0.png
Assets/Art/Level/v2/smoke_band_1.png
Assets/Art/Level/v2/smoke_band_2.png
Assets/Art/Level/v2/sparks_0.png
Assets/Art/Level/v2/sparks_1.png
Assets/Art/Level/v2/sparks_2.png
```

</details>

<details>
<summary>03-student (50 files)</summary>

```text
ArtSource/Student/boy_crawl.aseprite
ArtSource/Student/boy_feel_door.aseprite
ArtSource/Student/boy_signal.aseprite
ArtSource/Student/boy_stop_drop_roll.aseprite
ArtSource/Student/boy_walk_low.aseprite
ArtSource/Student/girl_crawl.aseprite
ArtSource/Student/girl_feel_door.aseprite
ArtSource/Student/girl_signal.aseprite
ArtSource/Student/girl_stop_drop_roll.aseprite
ArtSource/Student/girl_walk_low.aseprite
ArtSource/Student/sleeve_flame.aseprite
Assets/Art/Student/boy_crawl_0.png
Assets/Art/Student/boy_crawl_1.png
Assets/Art/Student/boy_crawl_2.png
Assets/Art/Student/boy_crawl_3.png
Assets/Art/Student/boy_feel_door_0.png
Assets/Art/Student/boy_feel_door_1.png
Assets/Art/Student/boy_signal_0.png
Assets/Art/Student/boy_signal_1.png
Assets/Art/Student/boy_stop_drop_roll_0.png
Assets/Art/Student/boy_stop_drop_roll_1.png
Assets/Art/Student/boy_stop_drop_roll_2.png
Assets/Art/Student/boy_stop_drop_roll_3.png
Assets/Art/Student/boy_stop_drop_roll_4.png
Assets/Art/Student/boy_stop_drop_roll_5.png
Assets/Art/Student/boy_walk_low_0.png
Assets/Art/Student/boy_walk_low_1.png
Assets/Art/Student/boy_walk_low_2.png
Assets/Art/Student/boy_walk_low_3.png
Assets/Art/Student/girl_crawl_0.png
Assets/Art/Student/girl_crawl_1.png
Assets/Art/Student/girl_crawl_2.png
Assets/Art/Student/girl_crawl_3.png
Assets/Art/Student/girl_feel_door_0.png
Assets/Art/Student/girl_feel_door_1.png
Assets/Art/Student/girl_signal_0.png
Assets/Art/Student/girl_signal_1.png
Assets/Art/Student/girl_stop_drop_roll_0.png
Assets/Art/Student/girl_stop_drop_roll_1.png
Assets/Art/Student/girl_stop_drop_roll_2.png
Assets/Art/Student/girl_stop_drop_roll_3.png
Assets/Art/Student/girl_stop_drop_roll_4.png
Assets/Art/Student/girl_stop_drop_roll_5.png
Assets/Art/Student/girl_walk_low_0.png
Assets/Art/Student/girl_walk_low_1.png
Assets/Art/Student/girl_walk_low_2.png
Assets/Art/Student/girl_walk_low_3.png
Assets/Art/Student/sleeve_flame_0.png
Assets/Art/Student/sleeve_flame_1.png
Assets/Art/Student/sleeve_flame_2.png
```

</details>

<details>
<summary>04-grownups (29 files)</summary>

```text
ArtSource/GrownUp/dad_phone.aseprite
ArtSource/GrownUp/dad_switch.aseprite
ArtSource/GrownUp/firefighter_idle.aseprite
ArtSource/GrownUp/firefighter_kneel.aseprite
ArtSource/GrownUp/firefighter_walk.aseprite
ArtSource/GrownUp/firefighter_wave.aseprite
ArtSource/GrownUp/mom_lid.aseprite
ArtSource/GrownUp/mom_reach.aseprite
ArtSource/GrownUp/teacher_count.aseprite
Assets/Art/GrownUp/dad_phone_0.png
Assets/Art/GrownUp/dad_phone_1.png
Assets/Art/GrownUp/dad_switch_0.png
Assets/Art/GrownUp/dad_switch_1.png
Assets/Art/GrownUp/firefighter_idle_0.png
Assets/Art/GrownUp/firefighter_idle_1.png
Assets/Art/GrownUp/firefighter_kneel_0.png
Assets/Art/GrownUp/firefighter_kneel_1.png
Assets/Art/GrownUp/firefighter_walk_0.png
Assets/Art/GrownUp/firefighter_walk_1.png
Assets/Art/GrownUp/firefighter_walk_2.png
Assets/Art/GrownUp/firefighter_walk_3.png
Assets/Art/GrownUp/firefighter_wave_0.png
Assets/Art/GrownUp/firefighter_wave_1.png
Assets/Art/GrownUp/mom_lid_0.png
Assets/Art/GrownUp/mom_lid_1.png
Assets/Art/GrownUp/mom_reach_0.png
Assets/Art/GrownUp/mom_reach_1.png
Assets/Art/GrownUp/teacher_count_0.png
Assets/Art/GrownUp/teacher_count_1.png
```

</details>

<details>
<summary>05-easy (40 files)</summary>

```text
ArtSource/Level/v2/bg_home_night.aseprite
ArtSource/Level/v2/bg_kitchen.aseprite
ArtSource/Level/v2/home_lighter.aseprite
ArtSource/Level/v2/home_night_candle.aseprite
ArtSource/Level/v2/home_night_curtain.aseprite
ArtSource/Level/v2/home_night_flashlight.aseprite
ArtSource/Level/v2/home_night_plate.aseprite
ArtSource/Level/v2/kitchen_burner.aseprite
ArtSource/Level/v2/kitchen_cup.aseprite
ArtSource/Level/v2/kitchen_pan.aseprite
ArtSource/Level/v2/kitchen_pan_lid.aseprite
ArtSource/Level/v2/kitchen_pot.aseprite
ArtSource/UI/icon_fire_call_mom.aseprite
ArtSource/UI/icon_fire_candle.aseprite
ArtSource/UI/icon_fire_give_mom.aseprite
ArtSource/UI/icon_fire_lighter.aseprite
ArtSource/UI/icon_fire_move_pot.aseprite
ArtSource/UI/icon_fire_tell_mom.aseprite
Assets/Art/Level/v2/bg_home_night.png
Assets/Art/Level/v2/bg_kitchen.png
Assets/Art/Level/v2/home_lighter.png
Assets/Art/Level/v2/home_night_candle_0.png
Assets/Art/Level/v2/home_night_candle_1.png
Assets/Art/Level/v2/home_night_curtain_0.png
Assets/Art/Level/v2/home_night_curtain_1.png
Assets/Art/Level/v2/home_night_flashlight.png
Assets/Art/Level/v2/home_night_plate.png
Assets/Art/Level/v2/kitchen_burner_0.png
Assets/Art/Level/v2/kitchen_burner_1.png
Assets/Art/Level/v2/kitchen_cup.png
Assets/Art/Level/v2/kitchen_pan.png
Assets/Art/Level/v2/kitchen_pan_lid.png
Assets/Art/Level/v2/kitchen_pot_0.png
Assets/Art/Level/v2/kitchen_pot_1.png
Assets/Art/UI/icon_fire_call_mom.png
Assets/Art/UI/icon_fire_candle.png
Assets/Art/UI/icon_fire_give_mom.png
Assets/Art/UI/icon_fire_lighter.png
Assets/Art/UI/icon_fire_move_pot.png
Assets/Art/UI/icon_fire_tell_mom.png
```

</details>

<details>
<summary>06-normal (34 files)</summary>

```text
ArtSource/Level/v2/classroom_bag.aseprite
ArtSource/Level/v2/corridor_cr_door.aseprite
ArtSource/Level/v2/corridor_handkerchief.aseprite
ArtSource/Level/v2/home_outlet.aseprite
ArtSource/Level/v2/home_switch_box.aseprite
ArtSource/UI/icon_fire_bag.aseprite
ArtSource/UI/icon_fire_carry_pan.aseprite
ArtSource/UI/icon_fire_hide_cr.aseprite
ArtSource/UI/icon_fire_line_up.aseprite
ArtSource/UI/icon_fire_pull_plugs.aseprite
ArtSource/UI/icon_fire_run_smoke.aseprite
ArtSource/UI/icon_fire_shout.aseprite
ArtSource/UI/icon_fire_stay_class.aseprite
ArtSource/UI/icon_fire_stay_low.aseprite
ArtSource/UI/icon_fire_tell_dad.aseprite
ArtSource/UI/icon_fire_water.aseprite
Assets/Art/Level/v2/classroom_bag.png
Assets/Art/Level/v2/corridor_cr_door.png
Assets/Art/Level/v2/corridor_handkerchief.png
Assets/Art/Level/v2/home_outlet_0.png
Assets/Art/Level/v2/home_outlet_1.png
Assets/Art/Level/v2/home_outlet_2.png
Assets/Art/Level/v2/home_switch_box.png
Assets/Art/UI/icon_fire_bag.png
Assets/Art/UI/icon_fire_carry_pan.png
Assets/Art/UI/icon_fire_hide_cr.png
Assets/Art/UI/icon_fire_line_up.png
Assets/Art/UI/icon_fire_pull_plugs.png
Assets/Art/UI/icon_fire_run_smoke.png
Assets/Art/UI/icon_fire_shout.png
Assets/Art/UI/icon_fire_stay_class.png
Assets/Art/UI/icon_fire_stay_low.png
Assets/Art/UI/icon_fire_tell_dad.png
Assets/Art/UI/icon_fire_water.png
```

</details>

<details>
<summary>07-bedroom (37 files)</summary>

```text
ArtSource/Level/v2/bedroom_blanket.aseprite
ArtSource/Level/v2/bedroom_blanket_gap.aseprite
ArtSource/Level/v2/bedroom_closet_door.aseprite
ArtSource/Level/v2/bedroom_cloth.aseprite
ArtSource/Level/v2/bedroom_door.aseprite
ArtSource/Level/v2/bedroom_toy.aseprite
ArtSource/Level/v2/bedroom_window_open.aseprite
ArtSource/UI/icon_fire_closet.aseprite
ArtSource/UI/icon_fire_crawl.aseprite
ArtSource/UI/icon_fire_feel_door.aseprite
ArtSource/UI/icon_fire_hide_bed.aseprite
ArtSource/UI/icon_fire_jump.aseprite
ArtSource/UI/icon_fire_open_door.aseprite
ArtSource/UI/icon_fire_run_standing.aseprite
ArtSource/UI/icon_fire_signal.aseprite
ArtSource/UI/icon_fire_toy.aseprite
ArtSource/UI/icon_fire_video.aseprite
ArtSource/UI/icon_fire_wait_bed.aseprite
Assets/Art/Level/v2/bedroom_blanket.png
Assets/Art/Level/v2/bedroom_blanket_gap.png
Assets/Art/Level/v2/bedroom_closet_door.png
Assets/Art/Level/v2/bedroom_cloth.png
Assets/Art/Level/v2/bedroom_door_0.png
Assets/Art/Level/v2/bedroom_door_1.png
Assets/Art/Level/v2/bedroom_toy.png
Assets/Art/Level/v2/bedroom_window_open.png
Assets/Art/UI/icon_fire_closet.png
Assets/Art/UI/icon_fire_crawl.png
Assets/Art/UI/icon_fire_feel_door.png
Assets/Art/UI/icon_fire_hide_bed.png
Assets/Art/UI/icon_fire_jump.png
Assets/Art/UI/icon_fire_open_door.png
Assets/Art/UI/icon_fire_run_standing.png
Assets/Art/UI/icon_fire_signal.png
Assets/Art/UI/icon_fire_toy.png
Assets/Art/UI/icon_fire_video.png
Assets/Art/UI/icon_fire_wait_bed.png
```

</details>

<details>
<summary>08-yard (15 files)</summary>

```text
ArtSource/Level/v2/bg_yard.aseprite
ArtSource/Level/v2/yard_siga.aseprite
ArtSource/UI/icon_fire_beat_hands.aseprite
ArtSource/UI/icon_fire_run_help.aseprite
ArtSource/UI/icon_fire_stop_drop_roll.aseprite
ArtSource/UI/icon_fire_wave_arms.aseprite
Assets/Art/Level/v2/bg_yard.png
Assets/Art/Level/v2/yard_siga_0.png
Assets/Art/Level/v2/yard_siga_1.png
Assets/Art/Level/v2/yard_siga_2.png
Assets/Art/Level/v2/yard_siga_3.png
Assets/Art/UI/icon_fire_beat_hands.png
Assets/Art/UI/icon_fire_run_help.png
Assets/Art/UI/icon_fire_stop_drop_roll.png
Assets/Art/UI/icon_fire_wave_arms.png
```

</details>

<details>
<summary>09-mall (6 files)</summary>

```text
ArtSource/Level/v2/mall_exit_sign.aseprite
ArtSource/Level/v2/mall_toy_shop_toy.aseprite
ArtSource/UI/icon_fire_exit_stairs.aseprite
Assets/Art/Level/v2/mall_exit_sign.png
Assets/Art/Level/v2/mall_toy_shop_toy.png
Assets/Art/UI/icon_fire_exit_stairs.png
```

</details>

<details>
<summary>10-street (76 files)</summary>

```text
ArtSource/Level/v2/bg_street.aseprite
ArtSource/Level/v2/bg_street_after.aseprite
ArtSource/Level/v2/smoke_drift.aseprite
ArtSource/Level/v2/street_cat.aseprite
ArtSource/Level/v2/street_hiding_box.aseprite
ArtSource/Level/v2/street_phone.aseprite
ArtSource/Level/v2/street_roof_smoke.aseprite
ArtSource/UI/icon_fire_call_911.aseprite
ArtSource/UI/icon_fire_cat.aseprite
ArtSource/UI/icon_fire_film.aseprite
ArtSource/UI/icon_fire_go_firefighter.aseprite
ArtSource/UI/icon_fire_hide_firefighter.aseprite
ArtSource/UI/icon_fire_run_home.aseprite
ArtSource/UI/icon_fire_run_inside.aseprite
ArtSource/UI/icon_fire_watch.aseprite
Assets/Art/Level/v2/bg_street.png
Assets/Art/Level/v2/bg_street_after.png
Assets/Art/Level/v2/smoke_drift_0.png
Assets/Art/Level/v2/smoke_drift_1.png
Assets/Art/Level/v2/smoke_drift_10.png
Assets/Art/Level/v2/smoke_drift_11.png
Assets/Art/Level/v2/smoke_drift_12.png
Assets/Art/Level/v2/smoke_drift_13.png
Assets/Art/Level/v2/smoke_drift_14.png
Assets/Art/Level/v2/smoke_drift_15.png
Assets/Art/Level/v2/smoke_drift_16.png
Assets/Art/Level/v2/smoke_drift_17.png
Assets/Art/Level/v2/smoke_drift_18.png
Assets/Art/Level/v2/smoke_drift_19.png
Assets/Art/Level/v2/smoke_drift_2.png
Assets/Art/Level/v2/smoke_drift_20.png
Assets/Art/Level/v2/smoke_drift_21.png
Assets/Art/Level/v2/smoke_drift_22.png
Assets/Art/Level/v2/smoke_drift_23.png
Assets/Art/Level/v2/smoke_drift_3.png
Assets/Art/Level/v2/smoke_drift_4.png
Assets/Art/Level/v2/smoke_drift_5.png
Assets/Art/Level/v2/smoke_drift_6.png
Assets/Art/Level/v2/smoke_drift_7.png
Assets/Art/Level/v2/smoke_drift_8.png
Assets/Art/Level/v2/smoke_drift_9.png
Assets/Art/Level/v2/street_cat.png
Assets/Art/Level/v2/street_hiding_box.png
Assets/Art/Level/v2/street_phone.png
Assets/Art/Level/v2/street_roof_smoke_0.png
Assets/Art/Level/v2/street_roof_smoke_1.png
Assets/Art/Level/v2/street_roof_smoke_10.png
Assets/Art/Level/v2/street_roof_smoke_11.png
Assets/Art/Level/v2/street_roof_smoke_12.png
Assets/Art/Level/v2/street_roof_smoke_13.png
Assets/Art/Level/v2/street_roof_smoke_14.png
Assets/Art/Level/v2/street_roof_smoke_15.png
Assets/Art/Level/v2/street_roof_smoke_16.png
Assets/Art/Level/v2/street_roof_smoke_17.png
Assets/Art/Level/v2/street_roof_smoke_18.png
Assets/Art/Level/v2/street_roof_smoke_19.png
Assets/Art/Level/v2/street_roof_smoke_2.png
Assets/Art/Level/v2/street_roof_smoke_20.png
Assets/Art/Level/v2/street_roof_smoke_21.png
Assets/Art/Level/v2/street_roof_smoke_22.png
Assets/Art/Level/v2/street_roof_smoke_23.png
Assets/Art/Level/v2/street_roof_smoke_3.png
Assets/Art/Level/v2/street_roof_smoke_4.png
Assets/Art/Level/v2/street_roof_smoke_5.png
Assets/Art/Level/v2/street_roof_smoke_6.png
Assets/Art/Level/v2/street_roof_smoke_7.png
Assets/Art/Level/v2/street_roof_smoke_8.png
Assets/Art/Level/v2/street_roof_smoke_9.png
Assets/Art/UI/icon_fire_call_911.png
Assets/Art/UI/icon_fire_cat.png
Assets/Art/UI/icon_fire_film.png
Assets/Art/UI/icon_fire_go_firefighter.png
Assets/Art/UI/icon_fire_hide_firefighter.png
Assets/Art/UI/icon_fire_run_home.png
Assets/Art/UI/icon_fire_run_inside.png
Assets/Art/UI/icon_fire_watch.png
```

</details>

## Typhoon Batch 1 — Menu backgrounds (approved)

Run from the project root with Aseprite.

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=menu_typhoon --script ArtSource/scripts/build.lua
```

Each basename has an editable source in `ArtSource/Background/<basename>.aseprite` and an export in `Assets/Art/Background/<basename>.png`. Each background is a single still frame with no animation timing. All coordinates are art pixels from the upper left. Backgrounds use the bottom anchor. Sign coordinates give the horizontal centre and top row.

| Basename | Size | Floor and landmarks |
| --- | --- | --- |
| bg_menu_barangay_hall | 480×270 | Ground starts at y=177. Porch steps start at y=190. Door (157,121). Four window frames start at x=27, 101, 232 and 282, y=115. BARANGAY HALL sign (58,62). SIGNAL NO. 2 board (403,217). Bare flagpole top centre (358,56) for the moving flag. Palm base (441,191). |
| bg_menu_typhoon_easy | 480×270 | Floor starts at y=208. Closed door (293,62). Closed window (382,75), glass (385,78), 79×64. Go Bag (338,199), radio (380,165), stool top (375,194), flashlight (362,215). The art reuses home_bag_closed, evacuation_area_radio and home_flashlight. |
| bg_menu_typhoon_normal | 480×270 | Floor starts at y=201. Board (12,81). Window frame (120,59), sill (115,166). Seven closed jalousie bays start at x=126, with 48 pixels between bays. Each has six 44×16 panes starting at y=65, spaced 16 pixels apart. Rain is drawn into the glass. Chair backs start at (42,177), (110,177), (142,218) and (389,211). |
| bg_menu_typhoon_hard | 480×270 | Interior floor starts at y=175, threshold at y=211, wet pavement at y=213. Entrance (235,102), door leaves (228,103) and (401,103), window bank (52,99). EVACUATION CENTER sign (398,78). Interior lights at x=250, 305 and 360, y=114. Blue tent (250,152), rust tent (309,147). Mat top edges start at x=257, 301 and 345, y=187. |

[Approved comparison sheet](D:/GameDEV/typhoon-episode-scratch/art/01-menu-typhoon/preview.png). The widescreen views retain rows 54–269 and show the menu bounds.

## Typhoon Batch 2 — Storm layers and window views (approved)

Run from the project root with Aseprite.

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=typhoon_fx --script ArtSource/scripts/build.lua
```

Editable sources live in `ArtSource/Level/v2/`. PNG exports live in `Assets/Art/Level/v2/`. Numbered ranges include both endpoints.

| Source (.aseprite) | PNG basename(s) | Size | Frames | Time per frame |
| --- | --- | --- | --- | --- |
| bedroom_window_rattle | bedroom_window_rattle_0 … bedroom_window_rattle_1 | 72×92 | 2 | 120 ms |
| bedroom_window_storm | bedroom_window_storm_0 … bedroom_window_storm_3 | 62×82 | 4 | 180 ms |
| classroom_window_storm | classroom_window_storm_0 … classroom_window_storm_3 | 68×82 | 4 | 180 ms |
| home_night_window_storm | home_night_window_storm_0 … home_night_window_storm_3 | 60×65 | 4 | 180 ms |
| home_window_bike | home_window_bike | 60×65 | 1 | Still |
| home_window_calm | home_window_calm | 60×65 | 1 | Still |
| home_window_clothes | home_window_clothes_0 … home_window_clothes_3 | 60×65 | 4 | 180 ms |
| home_window_grey | home_window_grey | 60×65 | 1 | Still |
| home_window_mangoes | home_window_mangoes | 60×65 | 1 | Still |
| home_window_mask | home_window_mask | 60×65 | 1 | Still |
| home_window_storm | home_window_storm_0 … home_window_storm_3 | 60×65 | 4 | 180 ms |
| typhoon_dark_sky_evacuation_area | typhoon_dark_sky_evacuation_area | 480×270 | 1 | Still |
| typhoon_dark_sky_yard | typhoon_dark_sky_yard | 480×270 | 1 | Still |
| typhoon_leaf | typhoon_leaf_0 … typhoon_leaf_3 | 4×3 | 4 | 180 ms |
| typhoon_rain_heavy | typhoon_rain_heavy_0 … typhoon_rain_heavy_2 | 480×270 | 3 | 120 ms |
| typhoon_rain_light | typhoon_rain_light_0 … typhoon_rain_light_2 | 480×270 | 3 | 120 ms |

Coordinates use art pixels from the upper left. Outdoor Rain and Dark Sky anchor at (0,0). Light and heavy Rain loop at 120 ms per frame, 35% and 55% alpha. Each drop moves down-left. The tumbling leaf and all swaying window views use 180 ms per frame. Bedroom rattle uses 120 ms. Still layers hold.

### Window registration

Classroom (341,67), 68×82. Home and home at night (304,72), 60×65. Bedroom (53,59), 62×82 inside the 72×92 frame. The rattle anchors at (48,54). Mount all home window inserts and the Tanod glass mask at (304,72). Preserve alpha around curtains, fixed sash, and the classroom sill plant. Draw the Tanod behind the mullion.

### Leaf bands

Ranges include the entire 4×3 leaf. Classroom x=348–400, y=71–73. Home and home at night x=311–358, y=75–78, within the clear glass only. Bedroom x=59–108, y=63–67, clipped by glass. Only the classroom view includes a baked leaf. Keep leaves off during a window visit by the Tanod and during still, calm or after views. Yard x=180–300, y=63–72. Evacuation Center x=440–473, y=58–68 outside the roof, unused during Hard 5. Reserve x=12–115, y=57–66 for street_storm and coast pending their own Setting and actor reviews. A band ending at row 72 requires all heads to remain at row 112 or lower. Apply the same 40-pixel check against the bottom of each other band.

### Candidate Act-out checks

The linked sheets show clearance against existing art. Proposed feet positions are for art review. Props, poses and Settings scheduled in later batches need their own final layout checks. Existing Grown-up poses mark scale. Each sheet shows both menu-independent Level crops with the Caption, Hint and Pause bounds.

[Easy 1](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-easy-1.png). start (144,230), call Mom (180,230), reach toward TV (111,230).

[Easy 2](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-easy-2.png). start (180,230), pick up pail (194,230), carry to house (42,230), set down (42,230), stop at ball (260,230).

[Easy 3](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-easy-3.png). start (141,230), pick up flashlight (161,230), pack Go Bag (190,230), play with flashlight (161,230).

[Normal 1](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-normal-1.png). start (146,230), meet Teacher (270,230), sit at desk (242,230), meet Mom (93,230), walk home stop (114,230), gate stop (72,230).

[Normal 2](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-normal-2.png). start (144,230), put down bag (180,230), sit and write (235,230), school stop (89,230), rain stop (119,230).

[Normal 3](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-normal-3.png). start (290,230), walk to Mom (323,230), exit (349,230), bed end (221,230), reach short of latch (139,230).

[Normal 4](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-normal-4.png). start (165,230), sit by Mom (292,230), mango stop (113,230), clothes stop (85,230).

[Normal 5](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-normal-5.png). start (177,230), call Mom (143,230), pick up Go Bag (215,230), bring to Mom (92,230), sit with toy (286,230), window stop (329,230).

[Hard 3](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-hard-3.png). start (168,230), shelf pickup (267,230), sit with Mom (148,230), candle stop (241,230), door stop (89,230), phone stop (168,230).

[Hard 4](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-hard-4.png). start (166,230), sit by radio (280,230), run-start (119,230), bike stop (87,230), friend stop (146,230).

[Hard 5](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-hard-5.png). start on mat (185,230), sit by Mom (161,230), court edge stop (395,230), open side stop (75,230), between tents (241,230).

[Hard 7](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/placement-hard-7.png). start (154,230), stand by Mom (127,230), roof sheet stop (190,230), shed door stop (226,230), broom stop (87,230).

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/preview.png) · [Motion preview](D:/GameDEV/typhoon-episode-scratch/art/02-storm-layers/motion.gif)


## Typhoon Batch 3 — Barangay Tanod and Grown-up poses (approved)

Run from the project root with Aseprite.

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=grownups_typhoon --script ArtSource/scripts/build.lua
```

Editable sources live in `ArtSource/GrownUp/`. PNG exports live in `Assets/Art/GrownUp/`. Numbered ranges include both endpoints.

| Source (.aseprite) | PNG basename(s) | Size | Frames | Time per frame |
| --- | --- | --- | --- | --- |
| dad_carry | dad_carry_0 … dad_carry_3 | 36×62 | 4 | 140 ms |
| dad_idle | dad_idle_0 … dad_idle_1 | 36×62 | 2 | 500 ms |
| mom_umbrella | mom_umbrella_0 … mom_umbrella_3 | 36×62 | 4 | 140 ms |
| mom_walk_gobag | mom_walk_gobag_0 … mom_walk_gobag_3 | 36×62 | 4 | 140 ms |
| tanod_idle | tanod_idle_0 … tanod_idle_1 | 36×62 | 2 | 500 ms |
| tanod_megaphone | tanod_megaphone_0 … tanod_megaphone_1 | 36×62 | 2 | 350 ms |
| tanod_walk | tanod_walk_0 … tanod_walk_3 | 36×62 | 4 | 140 ms |
| tanod_wave | tanod_wave_0 … tanod_wave_1 | 36×62 | 2 | 260 ms |

Each frame is 36×62. Register the feet at (18,61), measured from the upper left. The walking poses face right. For a horizontal mirror, replace x with 35−x. Animation timing and frame counts are labeled on the comparison sheet.

| Pose | Local placement in art pixels |
| --- | --- |
| tanod_idle | Hands (6,37) and (29,37). Hip megaphone bell (14,35). |
| tanod_walk | Grip (24,36+d). Hip bell (32,33+d). |
| tanod_megaphone | Grip hand (11,20). Open hand (33,24). Mouthpiece (15,16). Project the speech bubble from the open bell at (2,17) then (2,16). |
| tanod_wave | Megaphone hand (6,37). Wave hand (30,8) then (33,8). Hip bell (14,35). |
| mom_umbrella | Grip (23,34+d). Umbrella point (32,56+d). |
| mom_walk_gobag | Bag drawn at (2,18+d). Swinging hand (9,35+d), (16,37+d), (24,35+d), (16,37+d). |
| dad_carry | Chair grips (25,31+d) and (28,28+d). Hide the ground chair during carry. |
| dad_idle | Hands (6,37) and (29,37). |

For walks, d is 1,0,1,0. During the Normal 5 window pass, the feet move from (284,135) to (386,135) in Setting coordinates. Composite the Tanod through the Batch 2 glass mask at (304,72). The route begins and ends behind the wall. Keep the leaf effect off during his pass.

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/03-grownups/preview.png) · [Motion preview](D:/GameDEV/typhoon-episode-scratch/art/03-grownups/motion.gif) · [Normal 5 window walk](D:/GameDEV/typhoon-episode-scratch/art/03-grownups/window-walk.gif)


## Typhoon Batch 4 — Student flashlight pose (approved)

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=student_typhoon --script-param poses=flashlight --script ArtSource/scripts/build.lua
```

| Editable source in ArtSource/Student | PNG exports in Assets/Art/Student | Size | Frames | Time per frame |
| --- | --- | --- | --- | --- |
| boy_flashlight.aseprite | boy_flashlight_0.png, boy_flashlight_1.png | 32×50 | 2 | 350 ms |
| girl_flashlight.aseprite | girl_flashlight_0.png, girl_flashlight_1.png | 32×50 | 2 | 350 ms |

Both front poses use two 32Ã—50 frames, 350 ms each, registered at the feet (16,49). The beam is part of the standing pose. The walk uses the existing side-view frames and the existing home_night_flashlight prop.

| Walk frame | Boy hand | Girl hand |
| --- | --- | --- |
| 0 | (9,32) | (9,35) |
| 1 | (14,31) | (14,34) |
| 2 | (19,32) | (19,35) |
| 3 | (14,31) | (14,34) |

Coordinates are art pixels from the top left. For a right-facing walk, align the prop grip (3,4) to the hand. Mirror both together for a left-facing walk. Preview Student feet use (161,230) in Easy 3 and (267,230) in Hard 3.

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/04-student-flashlight/preview.png).

## Typhoon Batch 5 — Easy props and icons (approved)

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=home_typhoon,yard_typhoon,icons_typhoon_easy --script ArtSource/scripts/build.lua
```

| Source under ArtSource | PNGs under Assets/Art | Size | Frames | Timing |
| --- | --- | --- | --- | --- |
| Level/v2/home_tv_signal_1.aseprite | Level/v2/home_tv_signal_1.png | 50×27 | 1 | Still |
| Level/v2/home_tv_signal_2.aseprite | Level/v2/home_tv_signal_2.png | 50×27 | 1 | Still |
| Level/v2/yard_ball.aseprite | Level/v2/yard_ball.png | 8×8 | 1 | Still |
| Level/v2/yard_chair.aseprite | Level/v2/yard_chair.png | 18×22 | 1 | Still |
| Level/v2/yard_clothes.aseprite | Level/v2/yard_clothes_0.png … _3.png | 191×38 | 4 | 200 ms |
| Level/v2/yard_pail.aseprite | Level/v2/yard_pail.png | 12×12 | 1 | Still |
| Level/v2/yard_rope.aseprite | Level/v2/yard_rope.png | 36×12 | 1 | Still |
| Level/v2/yard_tree_sway.aseprite | Level/v2/yard_tree_sway_0.png … _3.png | 160×112 | 4 | 200 ms |
| UI/icon_typhoon_bring_inside.aseprite | UI/icon_typhoon_bring_inside.png | 16×16 | 1 | Still |
| UI/icon_typhoon_cartoons.aseprite | UI/icon_typhoon_cartoons.png | 16×16 | 1 | Still |
| UI/icon_typhoon_flashlight_bag.aseprite | UI/icon_typhoon_flashlight_bag.png | 16×16 | 1 | Still |
| UI/icon_typhoon_play_ball.aseprite | UI/icon_typhoon_play_ball.png | 16×16 | 1 | Still |
| UI/icon_typhoon_play_flashlight.aseprite | UI/icon_typhoon_play_flashlight.png | 16×16 | 1 | Still |
| UI/icon_typhoon_tell_news.aseprite | UI/icon_typhoon_tell_news.png | 16×16 | 1 | Still |

Coordinates are art pixels from the upper left. Actor coordinates mark the feet. Prop coordinates mark the top left. All Easy Student paths use y=230. Home floor begins at y=208 and the yard lawn at y=169. For the overlays use a single frame at a time.

| Asset | Placement and playback |
| --- | --- |
| home_tv_signal_1 and home_tv_signal_2 | 50Ã—27 at (90,95), three pixels inside the television frame at (87,92). Hold still. The two-line blue screen keeps the signal readable at 4Ã—. |
| evacuation_area_radio and evacuation_area_radio_on_0/_1 | Reuse at (199,151) on the home table for Easy 1 and Hard 4. All states share this anchor. |
| yard_pail | 12Ã—12, starts (197,216), ends (37,216). Support the base at local (6,11) on the Student carry palm (23,30), one row lower on frames 0 and 2. Right-facing top-left relative to the Student frame (17,19+d). Mirror the composite for left-facing carry. |
| yard_ball | 8Ã—8 at (274,221). Stop beside the ball. |
| yard_chair | 18Ã—22 at (347,205). Switch to the chair drawn in dad_carry when Dad lifts it. The house is beyond the left edge. |
| yard_rope | 36Ã—12 at (406,158), tied around the drum at (408,148). |
| yard_tree_sway and yard_clothes | 160Ã—112 at (8,18) and 191Ã—38 at (252,86). Four frames each, 200 ms per frame. Composite above the yard Dark Sky layer. The trunk base, line and clothespins stay fixed. |

### Actor routes

- Easy 1. Student starts (144,230). Mom stands at (181,221) on the left of the table. Call from (158,230) and switch the radio on. For cartoons, stop at (109,230) facing the TV and reach.
- Easy 2. Student starts (180,230), picks up from (191,230), carries left through (123,230), and sets down at (42,230). For the ball, stop at (260,230). Dad starts kneeling at (403,213) beside the knot, walks to (358,213) to lift the chair, then carries left through (219,213) and (92,213) toward the house.
- Easy 3. Student starts (141,230). Mom stands at (273,219), the open Go Bag is at (239,207), and the flashlight is at (167,221). Pick up at (161,230). Walk to (233,230), pack, and close the bag at the same anchor. For the play Choice, hold the lit flashlight at (161,230).

The existing home grey window is at (304,72). The yard leaf band spans x=180â€“300 and y=63â€“72. Batch 5 introduces six Choice icons and reuses the existing radio, Go Bag and flashlight props. The still props and icons have one frame each.

[Easy 1 placement sheet](D:/GameDEV/typhoon-episode-scratch/art/05-easy-props/easy-1-placements.png) Â· [Easy 2 placement sheet](D:/GameDEV/typhoon-episode-scratch/art/05-easy-props/easy-2-placements.png) Â· [Easy 3 placement sheet](D:/GameDEV/typhoon-episode-scratch/art/05-easy-props/easy-3-placements.png)

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/05-easy-props/preview.png).

## Typhoon Batch 6 — Normal props and icons (approved)

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=home_typhoon_normal,icons_typhoon_normal --script ArtSource/scripts/build.lua
```

| Source under ArtSource | Export under Assets/Art | Size | Frames | Timing |
| --- | --- | --- | --- | --- |
| Level/v2/home_module.aseprite | Level/v2/home_module.png | 14×10 | 1 | Still |
| UI/icon_typhoon_bring_clothes.aseprite | UI/icon_typhoon_bring_clothes.png | 16×16 | 1 | Still |
| UI/icon_typhoon_keep_playing.aseprite | UI/icon_typhoon_keep_playing.png | 16×16 | 1 | Still |
| UI/icon_typhoon_living_room.aseprite | UI/icon_typhoon_living_room.png | 16×16 | 1 | Still |
| UI/icon_typhoon_module.aseprite | UI/icon_typhoon_module.png | 16×16 | 1 | Still |
| UI/icon_typhoon_open_window.aseprite | UI/icon_typhoon_open_window.png | 16×16 | 1 | Still |
| UI/icon_typhoon_pick_mangoes.aseprite | UI/icon_typhoon_pick_mangoes.png | 16×16 | 1 | Still |
| UI/icon_typhoon_play_rain.aseprite | UI/icon_typhoon_play_rain.png | 16×16 | 1 | Still |
| UI/icon_typhoon_see_tanod.aseprite | UI/icon_typhoon_see_tanod.png | 16×16 | 1 | Still |
| UI/icon_typhoon_stay_inside.aseprite | UI/icon_typhoon_stay_inside.png | 16×16 | 1 | Still |
| UI/icon_typhoon_stay_teacher.aseprite | UI/icon_typhoon_stay_teacher.png | 16×16 | 1 | Still |
| UI/icon_typhoon_tell_tanod.aseprite | UI/icon_typhoon_tell_tanod.png | 16×16 | 1 | Still |
| UI/icon_typhoon_wait_gate.aseprite | UI/icon_typhoon_wait_gate.png | 16×16 | 1 | Still |
| UI/icon_typhoon_walk_home.aseprite | UI/icon_typhoon_walk_home.png | 16×16 | 1 | Still |
| UI/icon_typhoon_walk_school.aseprite | UI/icon_typhoon_walk_school.png | 16×16 | 1 | Still |
| UI/icon_typhoon_watch_window.aseprite | UI/icon_typhoon_watch_window.png | 16×16 | 1 | Still |

Coordinates are art pixels from the upper left. Actor points mark the feet. The walking line is y=230. The wide views retain rows 54â€“269.

**Normal 1.** Teacher (290,220), desk front (198,188), seated Student (242,207). Keep the classroom door closed until Mom arrives. Her entry starts at (46,214), then she stops at (79,214). The Student meets her at (103,230), inside the room. Both Unsafe stops keep the Student clear of the door. The placement sheet uses the existing classroom backdrop to mark her entry.

**Normal 2.** TV Signal No. 2 (90,95), school module (226,178), classroom_bag on the floor (172,215). Sit and write at (235,201), with the table front at (174,188). Show the bag in hand for the school stop. The home door at (18,107) stays closed.

**Normal 3.** Bed (104,194). Closed/open bedroom door frames (319,102). Mom waits at (363,211) for the Safe exit. Keep the window closed for every Choice. The reach stop at x=143 is right of the window frame; the bed-end stop at x=225 is beyond the headboard.

**Normal 4.** Mom (319,215). Mangoes and pinned clothes share the home window origin (304,72). Both outside Choices stop in the room, separated by 30 pixels.

**Normal 5.** Tanod crosses behind the glass mask from (284,135) to (386,135). Mom starts at (130,219) and meets the Student at the door side at (72,219). Go Bag (221,202), toy (300,218). The toy and window branches stop on clear floor.

### Act-out points

[Normal 1](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/normal-1-placements.png). Start (146,230), Meet Teacher (270,230), Sit at desk (242,207), Mom enters at door (242,207), Meet Mom inside (103,230), Walk home alone (114,230), Wait at gate (92,230).

[Normal 2](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/normal-2-placements.png). Start with school bag (144,230), Put bag down (180,230), Sit at table (235,201), Write in module (235,201), Walk to school (95,230), Play in rain (126,230).

[Normal 3](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/normal-3-placements.png). Start (290,230), Approach bedroom door (320,230), Exit with Mom (349,230), Watch from bed end (225,230), Reach short of latch (143,230).

[Normal 4](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/normal-4-placements.png). Start (165,230), Sit with Mom (292,230), Pick up mangoes (120,230), Bring in clothes (90,230).

[Normal 5](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/normal-5-placements.png). Tanod passes window (177,230), Call to Mom (155,230), Pick up Go Bag (215,230), Carry Go Bag (145,230), Bring bag to Mom (98,230), Keep playing (286,230), Go see Tanod (329,230).

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/06-normal-props/preview.png).

## Typhoon Batch 7 — Storm street (approved)

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=street_storm,icons_typhoon_street --script ArtSource/scripts/build.lua
```

| Source under ArtSource | Export under Assets/Art | Size | Frames | Timing |
| --- | --- | --- | --- | --- |
| Level/v2/bg_street_storm.aseprite | Level/v2/bg_street_storm.png | 480×270 | 1 | Still |
| Level/v2/street_storm_branch.aseprite | Level/v2/street_storm_branch.png | 42×19 | 1 | Still |
| Level/v2/street_storm_puddle.aseprite | Level/v2/street_storm_puddle_0.png … _1.png | 32×8 | 2 | 400 ms |
| Level/v2/street_storm_stick.aseprite | Level/v2/street_storm_stick.png | 20×8 | 1 | Still |
| Level/v2/street_storm_tree_sway.aseprite | Level/v2/street_storm_tree_sway_0.png … _3.png | 78×55 | 4 | 200 ms |
| Level/v2/street_storm_wire.aseprite | Level/v2/street_storm_wire.png | 85×163 | 1 | Still |
| Level/v2/typhoon_dark_sky_street_storm.aseprite | Level/v2/typhoon_dark_sky_street_storm.png | 480×270 | 1 | Still |
| UI/icon_typhoon_back_for_toy.aseprite | UI/icon_typhoon_back_for_toy.png | 16×16 | 1 | Still |
| UI/icon_typhoon_creek_shortcut.aseprite | UI/icon_typhoon_creek_shortcut.png | 16×16 | 1 | Still |
| UI/icon_typhoon_evac_route.aseprite | UI/icon_typhoon_evac_route.png | 16×16 | 1 | Still |
| UI/icon_typhoon_move_branch.aseprite | UI/icon_typhoon_move_branch.png | 16×16 | 1 | Still |
| UI/icon_typhoon_poke_stick.aseprite | UI/icon_typhoon_poke_stick.png | 16×16 | 1 | Still |
| UI/icon_typhoon_puddle.aseprite | UI/icon_typhoon_puddle.png | 16×16 | 1 | Still |
| UI/icon_typhoon_stay_home.aseprite | UI/icon_typhoon_stay_home.png | 16×16 | 1 | Still |
| UI/icon_typhoon_tell_dad_wire.aseprite | UI/icon_typhoon_tell_dad_wire.png | 16×16 | 1 | Still |

All coordinates are art pixels from the upper left. Actors register at the feet. Props and effects use their top-left corner. The road begins at y=148, near pavement at y=188 and the Student walking line at y=232. Bottom-anchor the Setting and retain rows 54â€“269 on wide screens.

### Setting layers

Background and sky mask (0,0), 480Ã—270. Tree sway (234,94), 78Ã—55, four frames at 200 ms above the Dark Sky. Light Rain tiles over the Hard 1 view. Allow the entire leaf only within x=12â€“115, y=57â€“66. The highest foreground Grown-up frame begins at y=157. Hard 6 uses the still background after the Rain. The route sign at (379,93) points right. The reed path begins near (124,198). The water is out of view.

### Hard 1

Student starts (210,232), Mom with Go Bag (246,226), Tanod (407,218). During the Safe walk they pass x=302,331,370 respectively, then finish at x=380,404,442 on their respective walking lines. Toy stop (87,232), creek path (124,232), stay-home stop (57,232). Use the existing 67Ã—26 Reaction above the Student, centred at (clamp(Student x,36,444),151).

### Hard 6

Wire (289,63), meets right-hand utility pole at (369,63). Branch (300,202) lies over the grounded wire. Puddle (214,222), 32Ã—8, two ripple frames at 400 ms. Stick (142,225), on the Student side. Dad (72,224), Tanod (435,219). Student starts (147,232), steps back to (123,232), then calls from (99,232) as Dad uses his phone. Stick stop (139,232), branch stop (158,232), puddle stop (154,232). Use the same Reaction registration as Hard 1.

Closest clearance from the entire swept Student frame rectangle is 42.0 px. Individual minimums are wire 118.0, branch 128.0, puddle 42.0. Closest puddle-to-wire edge distance is 46.0 px. Distances use occupied sprite pixels.

[Hard 1 placements](D:/GameDEV/typhoon-episode-scratch/art/07-street-storm/hard-1-placements.png) Â· [Hard 6 placements](D:/GameDEV/typhoon-episode-scratch/art/07-street-storm/hard-6-placements.png)

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/07-street-storm/preview.png).

## Typhoon Batch 8 — Coast (approved)

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=coast,icons_typhoon_coast --script ArtSource/scripts/build.lua
```

| Source under ArtSource | Export under Assets/Art | Size | Frames | Timing |
| --- | --- | --- | --- | --- |
| Level/v2/bg_coast.aseprite | Level/v2/bg_coast.png | 480×270 | 1 | Still |
| Level/v2/coast_palm_sway.aseprite | Level/v2/coast_palm_sway_0.png … _3.png | 77×49 | 4 | 200 ms |
| Level/v2/coast_shells.aseprite | Level/v2/coast_shells.png | 19×7 | 1 | Still |
| Level/v2/coast_waves.aseprite | Level/v2/coast_waves_0.png … _3.png | 205×39 | 4 | 180 ms |
| UI/icon_typhoon_shells.aseprite | UI/icon_typhoon_shells.png | 16×16 | 1 | Still |
| UI/icon_typhoon_swim.aseprite | UI/icon_typhoon_swim.png | 16×16 | 1 | Still |

All coordinates use art pixels from the upper left. Actor points are feet. Anchor the background at the bottom. The wide crop retains rows 54–269. The road starts at y=169 and the walking line starts at y=238.

| Asset | Size and placement |
| --- | --- |
| bg_coast | 480×270 at (0,0). Light Rain and overcast sky are painted in. |
| coast_waves | 205×39 at (0,85). Four opaque replacement frames, 180 ms each. Crests break offshore. |
| coast_palm_sway | 77×49 at (119,36). Four opaque replacement frames, 200 ms each. |
| coast_shells | 19×7 at (125,137) on sand at the foot of the steps. |
| evacuation_area_radio and radio_on_0/_1 | Reused at (209,150) on the doorstep, bottom at y=179. |

The near sea wall starts at (0,144). The right-hand return wall separates the cove from the house. The banca rests on sand around (60,137). The landing is at the road end of the stairs near (145,218). The sign spans (365,68) to (442,92). Leaf pixels stay within x=12–115 and y=55–57. The highest possible actor frame starts at y=98. Place the 67×26 Reaction above the Student for road stops. At the sign place its centre at (320,124) to clear the sign and controls.

### Act-outs

| Step | Student feet | Dad feet |
| --- | --- | --- |
| Start / radio at Lola’s house | (216, 238) | (252, 236) |
| Turn onto the uphill path | (262, 236) | (281, 222) |
| Climb with Dad | (320, 203) | (342, 190) |
| Reach the Evacuation Center sign | (364, 175) | (390, 159) |
| Watch from the road | (91, 235) | (252, 236) |
| Swim Choice / stop at the top | (157, 222) | (252, 236) |
| Shell Choice / crouch at the top | (123, 222) | (252, 236) |

Minimum distance from the whole animated wave rectangle to the swept actor frames: Student 50.0 px, Dad 59.7 px. The swim and shells stops are 34 px apart and both remain on the road side.

Reuse icon_high_ground for the uphill route and icon_watch_waves for watching. New icons are icon_typhoon_swim and icon_typhoon_shells, both 16×16.

[All stops at both screen sizes](D:/GameDEV/typhoon-episode-scratch/art/08-coast/hard-2-placements.png).

[Approved preview](D:/GameDEV/typhoon-episode-scratch/art/08-coast/preview.png).

## Typhoon Batch 9 — Hard home props and icons (approved)

Approved by the user on 2026-11-27. The imported PNGs are byte-identical to the approved review files. Run from the project root:

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=home_night_typhoon,home_typhoon_hard,icons_typhoon_home_hard --script ArtSource/scripts/build.lua
```

| Source under ArtSource | Export under Assets/Art | Size | Frames | Timing |
| --- | --- | --- | --- | --- |
| Level/v2/home_night_shelf.aseprite | Level/v2/home_night_shelf.png | 45×9 | 1 | Still |
| Level/v2/home_night_candle_unlit.aseprite | Level/v2/home_night_candle_unlit.png | 12×15 | 1 | Still |
| Level/v2/home_night_matches.aseprite | Level/v2/home_night_matches.png | 10×6 | 1 | Still |
| UI/icon_typhoon_flashlight_on.aseprite | UI/icon_typhoon_flashlight_on.png | 16×16 | 1 | Still |
| UI/icon_typhoon_light_candle.aseprite | UI/icon_typhoon_light_candle.png | 16×16 | 1 | Still |
| UI/icon_typhoon_check_lights.aseprite | UI/icon_typhoon_check_lights.png | 16×16 | 1 | Still |
| UI/icon_typhoon_phone_games.aseprite | UI/icon_typhoon_phone_games.png | 16×16 | 1 | Still |
| UI/icon_typhoon_go_play.aseprite | UI/icon_typhoon_go_play.png | 16×16 | 1 | Still |
| UI/icon_typhoon_get_bike.aseprite | UI/icon_typhoon_get_bike.png | 16×16 | 1 | Still |
| UI/icon_typhoon_visit_friend.aseprite | UI/icon_typhoon_visit_friend.png | 16×16 | 1 | Still |

Reused: `icon_radio` for "Stay inside and listen to the radio"; `bg_home_night`, `home_night_window_storm`, `home_night_flashlight`, `bg_home`, `home_window_storm`, `home_window_calm` and `home_window_bike`; the radio on the home table. The candle is unlit and the matchbox closed; neither is ever in a hand (ADR 0005). The phone-games icon shows game controls.

All coordinates are art pixels from the upper left. Actor points mark the feet. The home floor starts at y=208 and the walk line is y=230. The wide crop retains rows 54–269. The home and night windows use the glass at (304,72). In the storm the optional leaf band lies inside the glass at x=311–358, y=75–78. Disable Wind during the calm step.

### Hard 3 — Brownout (`home_night`)

Shelf (262,170), 45×9, just above the small table. Flashlight (264,162), unlit candle in holder (281,156), closed matchbox (296,164). Mom sits at (123,225). Only the flashlight leaves the shelf; the walk to Mom uses the approved Batch 4 mirrored walk grips. At the seated ending the flashlight rests beside the Student at (155,224). The Reaction uses the usual 67×26 frame centred at (Student x,151), shifted to (213,151) at the shelf to keep the items visible.

| Step | Student feet |
| --- | --- |
| Start in the brownout | (176,230) |
| Approach the shelf | (254,222) |
| Pick up the flashlight | (260,213) |
| Turn on the flashlight | (267,230) |
| Carry the light to Mom | (201,230) |
| Sit beside Mom | (151,230) |
| Candle stop (left of every shelf item; upright frame ends at x=250) | (235,230) |
| Lights stop at the closed door | (93,230) |
| Phone stop | (176,230) |

### Hard 4 — Eye of the storm (`home`)

Radio (199,151) on the home table; Mom sits at (314,220) on the sofa. Blend the storm glass to the calm glass with the bicycle overlay, then back to the storm glass during the Safe ending. The door at (18,107) stays closed. Reaction centre (Student x,151); at the radio use (265,151) to clear the radio and Mom.

| Step | Student feet |
| --- | --- |
| Start in the storm | (166,230) |
| Rain eases in the window | (166,230) |
| Walk to Mom by the radio | (254,230) |
| Sit and listen as the Rain returns | (282,230) |
| Play stop (run-start toward the door) | (119,230) |
| Bike stop | (87,230) |
| Friend stop, facing right toward the window | (146,230) |

[Hard 3 placements](D:/GameDEV/typhoon-episode-scratch/art/09-hard-home/hard-3-placements.png). [Hard 4 placements](D:/GameDEV/typhoon-episode-scratch/art/09-hard-home/hard-4-placements.png). [Approved preview](D:/GameDEV/typhoon-episode-scratch/art/09-hard-home/preview.png).

## Typhoon Batch 10 — Evacuation Center and damaged shed (approved)

Verified 15 exported PNGs and 12 Aseprite sources against the approved review files. The Tanod wears a plain yellow vest and grips his megaphone. Reuse icon_go_home for the return-home Choice.

```
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=evacuation_area_typhoon,yard_damage,icons_typhoon_after --script ArtSource/scripts/build.lua
```

[Placements, animation timing and clearances](D:/GameDEV/typhoon-episode-scratch/art/10-after/placements.md). [Corrected Tanod poses](D:/GameDEV/typhoon-episode-scratch/art/10-after/tanod-hand-poses.png).

## Flood Batch 1 — Floodwater at home (approved)

Approved sheet: D:/GameDEV/flood-episode-scratch/art/01-home-seep/preview.png. Run from the project root:

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=flood_home --script ArtSource/scripts/build.lua
~~~

All sources are in ArtSource/Level/v2 and exports in Assets/Art/Level/v2. Each name below has an .aseprite source. No Choice icons in this batch.

| Source basename | PNG basename | Canvas | Frames | Frame time |
|---|---|---|---:|---|
| home_flood_seep_1, home_flood_seep_2, home_flood_seep_3 | Each with suffix _0 and _1 | 120×30 | 2 each | 300 ms |
| home_fan | home_fan_0, home_fan_1 | 22×40 | 2 | 80 ms |
| home_outlet_low | home_outlet_low | 24×26 | 1 | Still |
| home_window_flood | home_window_flood_0 through _3 | 60×65 | 4 | 180 ms |

Art pixels from the upper left. Actor points mark the feet; prop points mark the top left. The floor starts at y 208; the walk line is y 232 in front of the table.

| Piece | Placement |
|---|---|
| home_flood_seep_1 to _3 (120x30, 2 frames, 300 ms) | (0,208). The sill (x 16 to 72) stays dry above row 212. Step 3 rightmost pixel is x 104. |
| home_outlet_low (24x26) | (72,188): plate at (76,190), cord along the back of the floor to the fan base. |
| home_fan (22x40, 2 frames, 80 ms; frame 0 held when the power is off) | (89,173), canvas bottom row 212. |
| home_window_flood (60x65, 4 frames, 180 ms) | (304,72); waterline at row 53 of the view. |
| home_switch_box (Fire) | (148,161). |
| Mom (mom_sit) | (314,220) on the sofa. |

### Act-out points and water clearance

| Step | Student pose and feet | Dad pose and feet | Student gap | Dad gap | Mom gap |
|---|---|---|---:|---:|---:|
| Start: water seeps in under the door | idle (214,232) | dad_idle (530,232) | 129.9 | off-screen | 220.2 |
| The water spreads: surprised | surprised (214,232) | dad_idle (530,232) | 115.3 | off-screen | 205.2 |
| Furthest step of the water | idle (214,232) | dad_idle (530,232) | 101.8 | off-screen | 192.1 |
| Unsafe stop: Unplug the fan | reach (162,232) | dad_idle (530,232) | 55.0 | off-screen | 192.1 |
| Unsafe stop: Wipe up the water | crouch (140,232) | dad_idle (530,232) | 30.8 | off-screen | 192.1 |
| Safe: walk away from the door | walk (330,232) | dad_idle (530,232) | 211.8 | off-screen | 192.1 |
| Safe: step back and call Dad, who walks in | call (402,232) | dad_walk (458,230) | 292.6 | 343.4 | 192.1 |
| Safe: Dad walks to the switch box | idle (402,232) | dad_walk (250,226) | 288.6 | 135.5 | 192.1 |
| Safe: Dad turns off the main switch | happy (402,232) | dad_switch (144,221) | 288.6 | 30.8 | 192.1 |

Minimum clearance across stops and pose frames: 30.8 art pixels.
The happy Reaction plays at (402,232). Dad enters from the right, faces left while walking, and uses the original switch pose.


## Flood Batch 2 — Home and kitchen (approved)

Approved sheet: D:/GameDEV/flood-episode-scratch/art/02-home-kitchen/preview.png. Run from the project root:

```text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=flood_home_more,flood_kitchen,icons_flood_home --script ArtSource/scripts/build.lua
```

Sources under ArtSource; PNGs in the matching Assets/Art subfolder.

| Source | PNGs | Canvas | Frames | Frame time |
|---|---|---|---:|---|
| Level/v2/home_shelf_high.aseprite | home_shelf_high.png | 40×8 | 1 | Still |
| Level/v2/home_tv_rainfall_red.aseprite | home_tv_rainfall_red.png | 50×27 | 1 | Still |
| Level/v2/home_window_flood_high.aseprite | home_window_flood_high_0.png, home_window_flood_high_1.png, home_window_flood_high_2.png, home_window_flood_high_3.png | 60×65 | 4 | 180 ms |
| Level/v2/home_workbook.aseprite | home_workbook.png | 12×7 | 1 | Still |
| Level/v2/kitchen_jug.aseprite | kitchen_jug.png | 15×18 | 1 | Still |
| Level/v2/kitchen_rain_drum.aseprite | kitchen_rain_drum.png | 24×32 | 1 | Still |
| Level/v2/kitchen_window_rain.aseprite | kitchen_window_rain_0.png, kitchen_window_rain_1.png, kitchen_window_rain_2.png, kitchen_window_rain_3.png | 90×55 | 4 | 180 ms |
| UI/icon_flood_bag_floor.aseprite | icon_flood_bag_floor.png | 16×16 | 1 | Still |
| UI/icon_flood_bag_up.aseprite | icon_flood_bag_up.png | 16×16 | 1 | Still |
| UI/icon_flood_boiled_water.aseprite | icon_flood_boiled_water.png | 16×16 | 1 | Still |
| UI/icon_flood_bridge.aseprite | icon_flood_bridge.png | 16×16 | 1 | Still |
| UI/icon_flood_leave_now.aseprite | icon_flood_leave_now.png | 16×16 | 1 | Still |
| UI/icon_flood_rain_drum.aseprite | icon_flood_rain_drum.png | 16×16 | 1 | Still |
| UI/icon_flood_riverbank.aseprite | icon_flood_riverbank.png | 16×16 | 1 | Still |
| UI/icon_flood_tap.aseprite | icon_flood_tap.png | 16×16 | 1 | Still |
| UI/icon_flood_unplug.aseprite | icon_flood_unplug.png | 16×16 | 1 | Still |
| UI/icon_flood_wait_water.aseprite | icon_flood_wait_water.png | 16×16 | 1 | Still |
| UI/icon_flood_wipe.aseprite | icon_flood_wipe.png | 16×16 | 1 | Still |

Coordinates are art pixels from the upper left. Actor anchors are the centre of the feet, Student (16,49) and Grown-up (18,61). All other anchors are top left. Floor begins at home y=208 and kitchen y=206. Main walking line y=232. Backgrounds are bottom anchored. The wide view drops rows 0–53.

Home shelf (166,163), 40×8. Bag on floor (120,209), on shelf (166,141); workbook top left bag + (4,-3). Right-facing carry bag offset from feet (-2,-40), one row lower on frames 0 and 2. Shelf height follows the reachup fingertips at feet (162,208). Mom at cabinet (372,214), box (388,171); Mom only changes pose at the ending. Normal 5 Mom (314,220), toy (353,218). Hard 1 Dad (36,215), Mom with Go Bag (61,218), waiting at door. Toy (396,217); phone (432,218), hidden during phone pose. The school bag stays on the shelf throughout evacuation. No Grown-up travels in these four layouts.

Home window (304,72), 60×65, four frames at 180 ms. High waterline y=116 (local 44). Red TV insert (90,95), 50×27 still. Kitchen glass (117,72), 90×55, four frames at 180 ms. Jug (422,145), 15×18; cup (437,151); drum (85,180), 24×32. Props and icons are still. Water gap is measured against the window Floodwater for Hard 1. Other Levels contain no Floodwater pixels. Rain and stored drinking water are excluded.

Choice icons reused: icon_fire_tell_dad (Normal 2), icon_typhoon_stay_home (Normal 5), icon_toy_car and icon_phone_all_day (Hard 1). The two belongings routes in Normal 5 follow the same Safe Choice.

### easy-1

| Act-out | Pose | Feet | Facing | Bag | Minimum water gap |
|---|---|---|---|---|---|
| Start | idle | (92,232) | right | floor | No Floodwater |
| Safe: pick up the school bag | crouch | (118,232) | right | floor | No Floodwater |
| Safe: carry the bag and workbook | carry | (146,222) | right | carry | No Floodwater |
| Safe: place the bag on the shelf | reachup | (162,208) | right | shelf | No Floodwater |
| Ending: Mom cheers | happy | (162,210) | right | shelf | No Floodwater |
| Leave it: stop toward the sofa | wait | (285,232) | right | floor | No Floodwater |

Level minimum: no Floodwater.

### normal-3

| Act-out | Pose | Feet | Facing | Bag | Minimum water gap |
|---|---|---|---|---|---|
| Start | idle | (243,232) | right | — | No Floodwater |
| Safe: walk to the counter | walk | (381,232) | right | — | No Floodwater |
| Safe: reach toward the covered jug and cup | reach | (455,209) | left | — | No Floodwater |
| Ending: drinking water choice | happy | (455,211) | right | — | No Floodwater |
| Rain drum: stop beside the screen door | reach | (112,221) | left | — | No Floodwater |
| Tap: stop short of the sink | reach | (187,216) | left | — | No Floodwater |

Level minimum: no Floodwater.

### normal-5

| Act-out | Pose | Feet | Facing | Bag | Minimum water gap |
|---|---|---|---|---|---|
| Start | idle | (188,232) | right | floor | No Floodwater |
| Safe: stay with Mom | walk | (259,232) | right | floor | No Floodwater |
| Safe: sit beside Mom | sit | (340,222) | right | floor | No Floodwater |
| Ending beside Mom | happy | (341,223) | right | floor | No Floodwater |
| Optional: pick up bag | crouch | (118,232) | right | floor | No Floodwater |
| Safe: carry the bag and workbook | carry | (146,222) | right | carry | No Floodwater |
| Safe: place the bag on the shelf | reachup | (162,208) | right | shelf | No Floodwater |
| Optional: sit beside Mom | sit | (340,222) | right | shelf | No Floodwater |
| Optional ending | happy | (341,223) | right | shelf | No Floodwater |
| Riverbank: stop before the closed door | wait | (76,232) | left | floor | No Floodwater |
| Bridge: stop farther back | wait | (108,232) | left | floor | No Floodwater |

Level minimum: no Floodwater.

### hard-1

| Act-out | Pose | Feet | Facing | Bag | Minimum water gap |
|---|---|---|---|---|---|
| Start: Red Rainfall Warning | surprised | (246,232) | right | shelf | 106.5 px |
| Safe: walk to the front door | walk | (154,232) | left | shelf | 165.3 px |
| Safe: join Mom and Dad | idle | (83,225) | left | shelf | 229.5 px |
| Ending: ready to leave | happy | (83,225) | right | shelf | 229.5 px |
| Wait: sit by the sofa | sit | (324,225) | right | shelf | 87.0 px |
| Toy: stop beside it | crouch | (389,232) | right | shelf | 94.8 px |
| Game: stop to use the phone | phone | (432,232) | right | shelf | 111.5 px |

Level minimum: 87.0 px.
Hard 1 Caption centre x=268 in both aspect ratios, 28 art pixels right of centre, to clear both TV lines. Normal 3 cup leaves the counter during the safe reach: cup top left at feet + (-19,-32), then (+5,-32) for the ending. This stages the cup in hand with the Student on dry floor.


## Flood Batch 3 — Outdoor Floodwater (approved)

Approved review: D:/GameDEV/flood-episode-scratch/art/03-outdoor/preview.png. Run from the project root:

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=flood_outdoor,icons_flood_outdoor --script ArtSource/scripts/build.lua
~~~

All coordinates are art pixels from the top left. Student feet register at (16,49), Grown-up feet at (18,61). Backgrounds anchor at the bottom. Full-size overlay top left (0,0). The 2400×1080 crop removes rows 0–53. Pink outlines are the Caption, Hint and Pause.

Yard: lawn behind the actors begins at y=175, walking line y=231. Ball and ripple top left (309,208), 16×11, two frames at 400 ms. Mom enters from off-screen left (-18,226) to (89,226).

Schoolyard: flooded road starts at y=166 on the far left; dry forecourt walking line y=238 rises to the doorway threshold at y=221. Teacher stays at (211,220).

Storm street: road y=148–187; near pavement y=188 onward. Walking lines Student y=234, Dad y=225. Bridge (96,158), 60×24, still. Deck shows the scrolling water underneath. Route sign (379,93) points right. Dad waits at his start on all Unsafe branches.

Overlay timing: yard two frames at 400 ms. Schoolyard and street four frames at 180 ms; current runs left. Light Rain covers each view, three frames at 120 ms, using the source alpha. Yard and storm street reuse their own Dark Sky layers. Reused Choice icon: icon_fire_tell_mom for Normal 1.

### normal-1

| Step | Student pose and feet | Grown-up pose and feet | Smallest gap (px) |
|---|---|---|---:|
| Start: ball on the water | idle (135,231) facing right | off-screen | 88.7 |
| Safe: turn away and call Mom | call (117,231) facing left | off-screen | 110.5 |
| Safe: Mom enters from the left | idle (117,231) facing left | mom_walk (20,226) facing right | 106.6 |
| Safe: Mom joins the Student | idle (117,231) facing left | mom_walk (89,226) facing right | 106.6 |
| Ending with Mom | happy (117,231) facing right | mom_idle (89,226) facing right | 106.6 |
| Wade for ball: stop on grass | reach (186,231) facing right | off-screen | 42.2 |
| Splash: stop on grass | walk (163,231) facing right | off-screen | 57.9 |

Minimum including walks: 35.4 art pixels. Happy Reaction top left (84,150), 67×26.

### normal-4

| Step | Student pose and feet | Grown-up pose and feet | Smallest gap (px) |
|---|---|---|---:|
| Start: road past gate flooded | idle (273,238) facing left | teacher_idle (211,220) facing right | 107.5 |
| Safe: walk toward Teacher | walk (246,232) facing left | teacher_idle (211,220) facing right | 107.5 |
| Safe: wait under roof | wait (231,221) facing left | teacher_idle (211,220) facing right | 107.5 |
| Ending at the doorway | happy (231,221) facing right | teacher_idle (211,220) facing right | 107.5 |
| Cross: stop before flooded exit | wait (151,238) facing left | teacher_idle (211,220) facing right | 43.2 |
| Wade: stop to reach for shoes | crouch (173,236) facing left | teacher_idle (211,220) facing right | 64.4 |

Minimum including walks: 40.3 art pixels. Happy Reaction top left (198,140), 67×26.

### hard-2

| Step | Student pose and feet | Grown-up pose and feet | Smallest gap (px) |
|---|---|---|---:|
| Start: low road flooded | idle (209,234) facing right | dad_idle (237,225) facing right | 36.0 |
| Safe: walk east with Dad | walk (291,234) facing right | dad_walk (323,225) facing right | 61.6 |
| Safe: follow evacuation sign | walk (377,234) facing right | dad_walk (407,225) facing right | 135.7 |
| Ending on the high road | happy (377,234) facing right | dad_idle (407,225) facing right | 140.7 |
| Cross: stop on near pavement | wait (175,229) facing left | dad_idle (237,225) facing right | 36.0 |
| Bridge: stop facing the deck | reach (123,228) facing left | dad_idle (237,225) facing right | 36.0 |
| Barefoot: stop to reach for shoes | crouch (152,231) facing left | dad_idle (237,225) facing right | 36.0 |

Minimum including walks: 36.0 art pixels. Happy Reaction top left (344,153), 67×26.

### File inventory

| Aseprite source under ArtSource | PNGs under matching Assets/Art folder | Size | Frames | Frame time |
|---|---|---|---:|---|
| Level/v2/flood_ball.aseprite | flood_ball_0.png, flood_ball_1.png | 16×11 | 2 | 400 |
| Level/v2/flood_schoolyard.aseprite | flood_schoolyard_0.png, flood_schoolyard_1.png, flood_schoolyard_2.png, flood_schoolyard_3.png | 480×270 | 4 | 180 |
| Level/v2/flood_street_storm.aseprite | flood_street_storm_0.png, flood_street_storm_1.png, flood_street_storm_2.png, flood_street_storm_3.png | 480×270 | 4 | 180 |
| Level/v2/flood_yard.aseprite | flood_yard_0.png, flood_yard_1.png | 480×270 | 2 | 400 |
| Level/v2/street_storm_bridge.aseprite | street_storm_bridge.png | 60×24 | 1 | Still |
| UI/icon_flood_cross_water.aseprite | icon_flood_cross_water.png | 16×16 | 1 | Still |
| UI/icon_flood_flooded_bridge.aseprite | icon_flood_flooded_bridge.png | 16×16 | 1 | Still |
| UI/icon_flood_high_road.aseprite | icon_flood_high_road.png | 16×16 | 1 | Still |
| UI/icon_flood_shoes_off.aseprite | icon_flood_shoes_off.png | 16×16 | 1 | Still |
| UI/icon_flood_splash.aseprite | icon_flood_splash.png | 16×16 | 1 | Still |
| UI/icon_flood_stay_teacher.aseprite | icon_flood_stay_teacher.png | 16×16 | 1 | Still |
| UI/icon_flood_wade_ball.aseprite | icon_flood_wade_ball.png | 16×16 | 1 | Still |
| UI/icon_flood_wade_barefoot.aseprite | icon_flood_wade_barefoot.png | 16×16 | 1 | Still |


## Flood Batch 4 — Provincial creek (approved)

Approved sheet: D:/GameDEV/flood-episode-scratch/art/04-creek/preview.png. Run from the project root:

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=creek,icons_flood_creek --script ArtSource/scripts/build.lua
~~~

All positions are art pixels from the upper left. The creek background is bottom anchored. Floodwater top left (0,99), 386×83, four frames at 180 ms. Flow is left, toward the foreground. The bridge is drawn into the background at (314,99). The water mask leaves its railings and deck clear. Rain has stopped. Stones (56,223), 21×9. Four new 16×16 Choice icons; no reused icons.

There is no flat indoor floor line. The dry dirt path slopes from the left foreground toward the houses. Safe walking line runs through (177,227), (212,218), (279,204) and (332,188). Mom holds mom_idle facing left at (352,180). Actor coordinates mark the feet: Student local (16,49), Mom (18,61). Happy Reaction top left (299,107), 67×26.

| Step | Student pose | Feet | Facing | Student gap | Mom gap |
|---|---|---|---|---:|---:|
| Start on the dirt path | idle | (177,227) | right | 83.0 px | 65.7 px |
| Safe: turn toward home | walk | (212,218) | right | 75.4 px | 65.7 px |
| Safe: follow the upper path | walk | (279,204) | right | 66.8 px | 65.7 px |
| Safe: join Mom | idle | (332,188) | right | 66.7 px | 65.7 px |
| Ending with Mom | happy | (332,188) | right | 66.7 px | 65.7 px |
| Swim: stop well back | wait | (109,222) | left | 56.8 px | 65.7 px |
| Fish: crouch on dry ground | crouch | (144,217) | left | 64.3 px | 65.7 px |
| Stones: stop before picking one up | reach | (82,232) | left | 58.5 px | 65.7 px |

Minimum over both Students, Mom, all pose frames and all six walk segments: 55.4 px.
### Provincial neighborhood

Eight homes are built into bg_creek. Facade left x and base y, far bank: (11,115), (58,118), (116,105), (156,109), (220,105), (272,102). Near the path: (439,127) and (377,139). Two covered porches face the lanes.

### File inventory

| Aseprite source under ArtSource | PNGs under matching Assets/Art folder | Size | Frames | Frame time |
|---|---|---|---:|---|
| Level/v2/bg_creek.aseprite | bg_creek.png | 480×270 | 1 | Still |
| Level/v2/creek_stones.aseprite | creek_stones.png | 21×9 | 1 | Still |
| Level/v2/creek_water.aseprite | creek_water_0.png, creek_water_1.png, creek_water_2.png, creek_water_3.png | 386×83 | 4 | 180 |
| UI/icon_flood_fish.aseprite | icon_flood_fish.png | 16×16 | 1 | Still |
| UI/icon_flood_swim.aseprite | icon_flood_swim.png | 16×16 | 1 | Still |
| UI/icon_flood_throw_stones.aseprite | icon_flood_throw_stones.png | 16×16 | 1 | Still |
| UI/icon_flood_walk_home.aseprite | icon_flood_walk_home.png | 16×16 | 1 | Still |


## Flood Batch 5 — Easy street and yard props (approved)

Approved sheet: D:/GameDEV/flood-episode-scratch/art/05-easy-props/preview.png. Run from the project root:

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=flood_street,flood_yard_easy,icons_flood_easy --script ArtSource/scripts/build.lua
~~~

Positions are art pixels from the upper left. Prop coordinates mark the top left. Student feet register at (16,49), Dad at (18,61). Backgrounds anchor at the bottom. Both Levels use daylight with Rain off. All new props and icons are stills; the Act-outs switch prop states. No new Student pose or reused Choice icon.

Easy 2: near pavement begins at y=191, walking line y=233–234. Dry grate (92,188), covered bin (304,199), bin slot near (312,205). The Student keeps the wrapper at the canal stop. Right-facing wrapper top-left offsets from the sprite top left for walk frames 0–3 are (8,32), (13,31), (18,32), (13,31) for the boy; add 3 to y for the girl. Reach (25+f,22-f); crouch (23,42+f), f=0 or 1. Mirror the full composite when facing left. At release draw only the top two rows at (312,204), then hide.

Easy 3: near lawn begins at y=175; walking line y=232–233. Upright can (193,220); tipped can (193,222). Full tire (302,217); tire on edge (302,203). Dad stays at (291,232), facing right. Switch to dad_kneel before changing the tire state. The existing crouch palm meets the left side of the can as it tips away from the Student. Wet patches extend to the right of both containers.

### easy-2

| Step | Student pose | Feet | Facing | Prop state | Floodwater gap |
|---|---|---|---|---|---|
| Start with wrapper | walk | (203,233) | right | held | No Floodwater |
| Safe: walk to the covered bin | walk | (260,234) | right | held | No Floodwater |
| Safe: reach over the slot | reach | (299,232) | right | held | No Floodwater |
| Safe: let the wrapper drop | reach | (299,232) | right | dropping | No Floodwater |
| Ending at the bin | happy | (299,232) | right | gone | No Floodwater |
| Canal: stop with wrapper in hand | crouch | (151,216) | left | held | No Floodwater |

Minimum Floodwater gap: not applicable; no Floodwater layer. The can and tire contain ordinary rainwater. Happy Reaction 67×26 at (266,151).

### easy-3

| Step | Student pose | Feet | Facing | Prop state | Floodwater gap |
|---|---|---|---|---|---|
| Start after the rain | idle | (142,233) | right | full | No Floodwater |
| Safe: walk to the can | walk | (174,232) | right | full | No Floodwater |
| Safe: crouch beside the can | crouch | (187,232) | right | full | No Floodwater |
| Safe: tip the can | crouch | (187,232) | right | can | No Floodwater |
| Safe: Dad kneels to tip the tire | idle | (208,232) | right | kneel | No Floodwater |
| Safe: tire on edge | idle | (208,232) | right | both | No Floodwater |
| Ending: both containers drained | happy | (208,232) | right | both | No Floodwater |
| Leave it: stop past the full can | wait | (235,232) | right | full | No Floodwater |

Minimum Floodwater gap: not applicable; no Floodwater layer. The can and tire contain ordinary rainwater. Happy Reaction 67×26 at (175,151).

### File inventory

| Aseprite source under ArtSource | PNGs under matching Assets/Art folder | Size | Frames | Frame time |
|---|---|---|---:|---|
| Level/v2/street_canal_grate.aseprite | street_canal_grate.png | 51×13 | 1 | Still |
| Level/v2/street_trash_bin.aseprite | street_trash_bin.png | 38×35 | 1 | Still |
| Level/v2/street_wrapper.aseprite | street_wrapper.png | 8×6 | 1 | Still |
| Level/v2/yard_can_tipped.aseprite | yard_can_tipped.png | 25×13 | 1 | Still |
| Level/v2/yard_can_water.aseprite | yard_can_water.png | 12×14 | 1 | Still |
| Level/v2/yard_tire_tipped.aseprite | yard_tire_tipped.png | 41×30 | 1 | Still |
| Level/v2/yard_tire_water.aseprite | yard_tire_water.png | 31×17 | 1 | Still |
| UI/icon_flood_canal.aseprite | icon_flood_canal.png | 16×16 | 1 | Still |
| UI/icon_flood_leave_water.aseprite | icon_flood_leave_water.png | 16×16 | 1 | Still |
| UI/icon_flood_tip_water.aseprite | icon_flood_tip_water.png | 16×16 | 1 | Still |
| UI/icon_flood_trash_bin.aseprite | icon_flood_trash_bin.png | 16×16 | 1 | Still |


## Flood Batch 6 — After the flood (approved)

Approved sheet: D:/GameDEV/flood-episode-scratch/art/06-after/preview.png. Run from the project root:

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=flood_after,student_flood,icons_flood_after --script-param poses=wash --script ArtSource/scripts/build.lua
~~~

Actor anchors are the feet, local (16,49) for the Student and (18,61) for Mom. All props register at their top left. Coordinates are art pixels from the upper left. Backgrounds are bottom anchored. Pink outlines show the Caption, Hint and Pause in both screen crops. Mom holds her listed position throughout each Level.

Hard 4: dry slab begins at y=156. Walking line y=231–235. Basin with soap (293,216), stored dipper (298,213). Hide stored dipper during wash, when the Student holds it. Wash loops two frames at 300 ms. Rain and Floodwater off.

Hard 5 and 6: floor begins at y=208, main walking line y=232. Mud (0,184), matte smears confined to door area. Hard 5 bread (380,221), cat (34,190). Carried bread follows the feet at (-21,-32) facing left or (-2,-32) right. Handoff offset (-24,-32); Mom holds at (286,204). Crouching lift offset (+5,-12). Hard 6 switch box (148,161), outlet (72,188), fan (89,173), held off on frame 0.

Hard 7 uses bg_street because bg_street_after shows fire damage and a fire engine. Near pavement starts at y=191. Start line y=233; route to the home passes left of the cart. Mud (279,177), leftover water (281,149), two frames at 400 ms. One small slipper floats in the patch. Choice icon reused: icon_typhoon_keep_playing.

### hard-4

Mom: mom_sit at (118,236), facing right.

| Act-out | Pose | Feet | Facing | Prop state | Min. water gap |
|---|---|---|---|---|---|
| Start on the slab | idle | (224,232) | right | default | No Floodwater |
| Safe: walk to the basin | walk | (267,231) | right | default | No Floodwater |
| Safe: wash feet | wash | (278,231) | right | default | No Floodwater |
| Safe: walk to Mom on the mat | walk | (189,235) | left | default | No Floodwater |
| Safe: tell Mom | call | (145,235) | left | default | No Floodwater |
| Ending beside Mom | happy | (145,235) | right | default | No Floodwater |
| Shirt: stop crouching | crouch | (235,232) | right | default | No Floodwater |
| Play: stop toward open court | walk | (357,232) | right | default | No Floodwater |
| Dry: sit at the mat edge | sit | (205,237) | right | default | No Floodwater |

Happy Reaction top left (112,154), 67×26. Minimum including walks: no Floodwater.

### hard-5

Mom: mom_idle at (280,230), facing right.

| Act-out | Pose | Feet | Facing | Prop state | Min. water gap |
|---|---|---|---|---|---|
| Start by the cabinet | idle | (354,233) | right | default | No Floodwater |
| Safe: pick up damp bread | crouch | (374,232) | right | default | No Floodwater |
| Safe: carry to Mom | carry | (339,232) | left | carry | No Floodwater |
| Safe: hand over the bag | reach | (302,232) | left | handoff | No Floodwater |
| Ending: Mom holds the bread | happy | (305,232) | right | mom | No Floodwater |
| Eat: stop holding bread low | carry | (377,233) | right | carry | No Floodwater |
| Wipe: stop before wiping | crouch | (364,232) | right | lift | No Floodwater |
| Cat: stop inside the room | carry | (115,235) | left | carry | No Floodwater |

Happy Reaction top left (272,151), 67×26. Minimum including walks: no Floodwater.

### hard-6

Mom: mom_sit at (314,220), facing right.

| Act-out | Pose | Feet | Facing | Prop state | Min. water gap |
|---|---|---|---|---|---|
| Start with the power off | idle | (236,233) | right | default | No Floodwater |
| Safe: walk to Mom | walk | (273,232) | right | default | No Floodwater |
| Safe: sit beside Mom | sit | (338,223) | right | default | No Floodwater |
| Ending beside Mom | happy | (340,227) | right | default | No Floodwater |
| TV: stop short | reach | (136,229) | left | default | No Floodwater |
| Fan: stop before the outlet | reach | (125,236) | left | default | No Floodwater |
| Switch: stop before the box | reachup | (177,231) | left | default | No Floodwater |

Happy Reaction top left (307,146), 67×26. Minimum including walks: no Floodwater.

### hard-7

Mom: mom_idle at (201,183), facing right.

| Act-out | Pose | Feet | Facing | Prop state | Min. water gap |
|---|---|---|---|---|---|
| Start on the near pavement | idle | (212,233) | right | default | 81.0 px |
| Safe: walk toward home | walk | (183,213) | left | default | 81.0 px |
| Safe: pass the cart on its left | walk | (181,190) | right | default | 81.0 px |
| Safe: join Mom at the door | idle | (177,188) | right | default | 81.0 px |
| Ending at home | happy | (177,188) | right | default | 81.0 px |
| See: stop on the pavement | wait | (244,233) | right | default | 79.2 px |
| Mud: stop before the road edge | crouch | (300,224) | right | default | 58.0 px |
| Water: stop on dry pavement | reach | (313,216) | right | default | 50.0 px |

Happy Reaction top left (144,107), 67×26. Minimum including walks: 50.0 px.

### File inventory

| Aseprite source under ArtSource | PNGs under matching Assets/Art folder | Size | Frames | Frame time |
|---|---|---|---:|---|
| Level/v2/evacuation_area_basin.aseprite | evacuation_area_basin.png | 36×16 | 1 | Still |
| Level/v2/evacuation_area_dipper.aseprite | evacuation_area_dipper.png | 12×8 | 1 | Still |
| Level/v2/flood_street_leftover.aseprite | flood_street_leftover_0.png, flood_street_leftover_1.png | 65×17 | 2 | 400 |
| Level/v2/home_after_bread.aseprite | home_after_bread.png | 20×13 | 1 | Still |
| Level/v2/home_after_mud.aseprite | home_after_mud.png | 105×33 | 1 | Still |
| Level/v2/street_mud.aseprite | street_mud.png | 98×12 | 1 | Still |
| Student/boy_wash.aseprite | boy_wash_0.png, boy_wash_1.png | 32×50 | 2 | 300 |
| Student/girl_wash.aseprite | girl_wash_0.png, girl_wash_1.png | 32×50 | 2 | 300 |
| UI/icon_flood_eat_bread.aseprite | icon_flood_eat_bread.png | 16×16 | 1 | Still |
| UI/icon_flood_feed_cat.aseprite | icon_flood_feed_cat.png | 16×16 | 1 | Still |
| UI/icon_flood_give_bread.aseprite | icon_flood_give_bread.png | 16×16 | 1 | Still |
| UI/icon_flood_let_dry.aseprite | icon_flood_let_dry.png | 16×16 | 1 | Still |
| UI/icon_flood_pick_water.aseprite | icon_flood_pick_water.png | 16×16 | 1 | Still |
| UI/icon_flood_play_mud.aseprite | icon_flood_play_mud.png | 16×16 | 1 | Still |
| UI/icon_flood_plug_fan.aseprite | icon_flood_plug_fan.png | 16×16 | 1 | Still |
| UI/icon_flood_see_street.aseprite | icon_flood_see_street.png | 16×16 | 1 | Still |
| UI/icon_flood_stay_mom.aseprite | icon_flood_stay_mom.png | 16×16 | 1 | Still |
| UI/icon_flood_switch_box.aseprite | icon_flood_switch_box.png | 16×16 | 1 | Still |
| UI/icon_flood_turn_on_tv.aseprite | icon_flood_turn_on_tv.png | 16×16 | 1 | Still |
| UI/icon_flood_wait_check.aseprite | icon_flood_wait_check.png | 16×16 | 1 | Still |
| UI/icon_flood_wash_feet.aseprite | icon_flood_wash_feet.png | 16×16 | 1 | Still |
| UI/icon_flood_wipe_bread.aseprite | icon_flood_wipe_bread.png | 16×16 | 1 | Still |
| UI/icon_flood_wipe_shirt.aseprite | icon_flood_wipe_shirt.png | 16×16 | 1 | Still |


## Flood Batch 7 — Flood menu backgrounds (approved)

Approved review: D:/GameDEV/flood-episode-scratch/art/07-menu/preview.png. The Hard menu uses the revised raised road with a grassy embankment and a covered Evacuation Center entrance.

~~~text
aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=menu_flood --script ArtSource/scripts/build.lua
~~~

| Source in ArtSource/Background | Export in Assets/Art/Background | Canvas | Frames | Timing |
|---|---|---|---:|---|
| bg_menu_flood_easy.aseprite | bg_menu_flood_easy.png | 480×270 | 1 | Still |
| bg_menu_flood_episode.aseprite | bg_menu_flood_episode.png | 480×270 | 1 | Still |
| bg_menu_flood_hard.aseprite | bg_menu_flood_hard.png | 480×270 | 1 | Still |
| bg_menu_flood_normal.aseprite | bg_menu_flood_normal.png | 480×270 | 1 | Still |

All four stills are 480×270, bottom anchored, in Background/. Each has a matching Aseprite source. No actor starts, walk paths, Choice icons or animated art in this batch. No people are painted into the backgrounds. Water gaps for actors do not apply.

Placement previews reconstruct the header, Back, Difficulty cards and lock extents, Level slots including selection frame, Start, Menu Student and speech bubble from ProjectBuilder.cs. The Backdrop tint is included. Controls are outlined for visibility. 1920×1080 uses 4× art. 2400×1080 uses 5× art and crops rows 0–53. Both checks conservatively reserve the optional speech bubble. Distances below are between background landmarks and control rectangles, in art pixels.

### episode

Near grassy bank begins near y=170 at left and curves down to the lower right. Houses at x=93,153,236,371,433, upstream palm (69,131).

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| bridge (1, 119, 55, 176) | 12.5 | 30.7 |
| water gauge (46, 152, 54, 186) | 13.5 | 31.7 |
| visible current (263, 218, 284, 222) | 28.0 | 6.8 |

### easy

Door opening (15,71), floor/threshold y=202, outer step y=211, path starts y=222. Outdoor rain ends at the sheltered facade.

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| book and bag (41, 97, 61, 123) | 85.0 | 78.5 |
| shelf (29, 122, 75, 132) | 80.5 | 70.5 |
| left boot (42, 183, 59, 204) | 25.0 | 29.3 |
| right boot (62, 183, 79, 204) | 25.0 | 29.3 |

### normal

Road y=143–200, moving-looking water starts around y=150. The gate spans the threshold at y=201. Dry pavement begins at y=201.

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| left gatepost (22, 86, 40, 203) | 26.0 | 30.3 |
| right gatepost (466, 86, 479, 200) | 2.5 | 1.2 |
| flooded road sample (97, 187, 139, 191) | 28.5 | 4.0 |
| dry forecourt sample (104, 209, 146, 216) | 19.1 | 26.0 |

### hard

Paved road enters at (0,255), bends at (300,162), and meets the landing at (416,125). The uphill verge connects to the houses on the left. A grass-and-earth bank slopes down to the flooded field on the right. Center roof (355,57), porch landing (357,119), route board (99,198), far houses (42,112) and (113,107).

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| center sign (362, 73, 469, 90) | 1.5 | 21.4 |
| route sign and arrow (99, 198, 182, 222) | 3.2 | 10.6 |
| open road sample (194, 193, 214, 203) | 20.0 | 10.0 |
| flooded field sample (320, 228, 359, 240) | 25.0 | 21.1 |

## Flood art file inventory

<details>
<summary>Batch 1 — home seep (6 sources, 13 PNGs)</summary>

~~~text
ArtSource/Level/v2/home_fan.aseprite
ArtSource/Level/v2/home_flood_seep_1.aseprite
ArtSource/Level/v2/home_flood_seep_2.aseprite
ArtSource/Level/v2/home_flood_seep_3.aseprite
ArtSource/Level/v2/home_outlet_low.aseprite
ArtSource/Level/v2/home_window_flood.aseprite
Assets/Art/Level/v2/home_fan_0.png
Assets/Art/Level/v2/home_fan_1.png
Assets/Art/Level/v2/home_flood_seep_1_0.png
Assets/Art/Level/v2/home_flood_seep_1_1.png
Assets/Art/Level/v2/home_flood_seep_2_0.png
Assets/Art/Level/v2/home_flood_seep_2_1.png
Assets/Art/Level/v2/home_flood_seep_3_0.png
Assets/Art/Level/v2/home_flood_seep_3_1.png
Assets/Art/Level/v2/home_outlet_low.png
Assets/Art/Level/v2/home_window_flood_0.png
Assets/Art/Level/v2/home_window_flood_1.png
Assets/Art/Level/v2/home_window_flood_2.png
Assets/Art/Level/v2/home_window_flood_3.png
~~~

</details>

<details>
<summary>Batch 2 — home kitchen (18 sources, 24 PNGs)</summary>

~~~text
ArtSource/Level/v2/home_shelf_high.aseprite
ArtSource/Level/v2/home_tv_rainfall_red.aseprite
ArtSource/Level/v2/home_window_flood_high.aseprite
ArtSource/Level/v2/home_workbook.aseprite
ArtSource/Level/v2/kitchen_jug.aseprite
ArtSource/Level/v2/kitchen_rain_drum.aseprite
ArtSource/Level/v2/kitchen_window_rain.aseprite
ArtSource/UI/icon_flood_bag_floor.aseprite
ArtSource/UI/icon_flood_bag_up.aseprite
ArtSource/UI/icon_flood_boiled_water.aseprite
ArtSource/UI/icon_flood_bridge.aseprite
ArtSource/UI/icon_flood_leave_now.aseprite
ArtSource/UI/icon_flood_rain_drum.aseprite
ArtSource/UI/icon_flood_riverbank.aseprite
ArtSource/UI/icon_flood_tap.aseprite
ArtSource/UI/icon_flood_unplug.aseprite
ArtSource/UI/icon_flood_wait_water.aseprite
ArtSource/UI/icon_flood_wipe.aseprite
Assets/Art/Level/v2/home_shelf_high.png
Assets/Art/Level/v2/home_tv_rainfall_red.png
Assets/Art/Level/v2/home_window_flood_high_0.png
Assets/Art/Level/v2/home_window_flood_high_1.png
Assets/Art/Level/v2/home_window_flood_high_2.png
Assets/Art/Level/v2/home_window_flood_high_3.png
Assets/Art/Level/v2/home_workbook.png
Assets/Art/Level/v2/kitchen_jug.png
Assets/Art/Level/v2/kitchen_rain_drum.png
Assets/Art/Level/v2/kitchen_window_rain_0.png
Assets/Art/Level/v2/kitchen_window_rain_1.png
Assets/Art/Level/v2/kitchen_window_rain_2.png
Assets/Art/Level/v2/kitchen_window_rain_3.png
Assets/Art/UI/icon_flood_bag_floor.png
Assets/Art/UI/icon_flood_bag_up.png
Assets/Art/UI/icon_flood_boiled_water.png
Assets/Art/UI/icon_flood_bridge.png
Assets/Art/UI/icon_flood_leave_now.png
Assets/Art/UI/icon_flood_rain_drum.png
Assets/Art/UI/icon_flood_riverbank.png
Assets/Art/UI/icon_flood_tap.png
Assets/Art/UI/icon_flood_unplug.png
Assets/Art/UI/icon_flood_wait_water.png
Assets/Art/UI/icon_flood_wipe.png
~~~

</details>

<details>
<summary>Batch 3 — outdoor (13 sources, 21 PNGs)</summary>

~~~text
ArtSource/Level/v2/flood_ball.aseprite
ArtSource/Level/v2/flood_schoolyard.aseprite
ArtSource/Level/v2/flood_street_storm.aseprite
ArtSource/Level/v2/flood_yard.aseprite
ArtSource/Level/v2/street_storm_bridge.aseprite
ArtSource/UI/icon_flood_cross_water.aseprite
ArtSource/UI/icon_flood_flooded_bridge.aseprite
ArtSource/UI/icon_flood_high_road.aseprite
ArtSource/UI/icon_flood_shoes_off.aseprite
ArtSource/UI/icon_flood_splash.aseprite
ArtSource/UI/icon_flood_stay_teacher.aseprite
ArtSource/UI/icon_flood_wade_ball.aseprite
ArtSource/UI/icon_flood_wade_barefoot.aseprite
Assets/Art/Level/v2/flood_ball_0.png
Assets/Art/Level/v2/flood_ball_1.png
Assets/Art/Level/v2/flood_schoolyard_0.png
Assets/Art/Level/v2/flood_schoolyard_1.png
Assets/Art/Level/v2/flood_schoolyard_2.png
Assets/Art/Level/v2/flood_schoolyard_3.png
Assets/Art/Level/v2/flood_street_storm_0.png
Assets/Art/Level/v2/flood_street_storm_1.png
Assets/Art/Level/v2/flood_street_storm_2.png
Assets/Art/Level/v2/flood_street_storm_3.png
Assets/Art/Level/v2/flood_yard_0.png
Assets/Art/Level/v2/flood_yard_1.png
Assets/Art/Level/v2/street_storm_bridge.png
Assets/Art/UI/icon_flood_cross_water.png
Assets/Art/UI/icon_flood_flooded_bridge.png
Assets/Art/UI/icon_flood_high_road.png
Assets/Art/UI/icon_flood_shoes_off.png
Assets/Art/UI/icon_flood_splash.png
Assets/Art/UI/icon_flood_stay_teacher.png
Assets/Art/UI/icon_flood_wade_ball.png
Assets/Art/UI/icon_flood_wade_barefoot.png
~~~

</details>

<details>
<summary>Batch 4 — creek (7 sources, 10 PNGs)</summary>

~~~text
ArtSource/Level/v2/bg_creek.aseprite
ArtSource/Level/v2/creek_stones.aseprite
ArtSource/Level/v2/creek_water.aseprite
ArtSource/UI/icon_flood_fish.aseprite
ArtSource/UI/icon_flood_swim.aseprite
ArtSource/UI/icon_flood_throw_stones.aseprite
ArtSource/UI/icon_flood_walk_home.aseprite
Assets/Art/Level/v2/bg_creek.png
Assets/Art/Level/v2/creek_stones.png
Assets/Art/Level/v2/creek_water_0.png
Assets/Art/Level/v2/creek_water_1.png
Assets/Art/Level/v2/creek_water_2.png
Assets/Art/Level/v2/creek_water_3.png
Assets/Art/UI/icon_flood_fish.png
Assets/Art/UI/icon_flood_swim.png
Assets/Art/UI/icon_flood_throw_stones.png
Assets/Art/UI/icon_flood_walk_home.png
~~~

</details>

<details>
<summary>Batch 5 — easy props (11 sources, 11 PNGs)</summary>

~~~text
ArtSource/Level/v2/street_canal_grate.aseprite
ArtSource/Level/v2/street_trash_bin.aseprite
ArtSource/Level/v2/street_wrapper.aseprite
ArtSource/Level/v2/yard_can_tipped.aseprite
ArtSource/Level/v2/yard_can_water.aseprite
ArtSource/Level/v2/yard_tire_tipped.aseprite
ArtSource/Level/v2/yard_tire_water.aseprite
ArtSource/UI/icon_flood_canal.aseprite
ArtSource/UI/icon_flood_leave_water.aseprite
ArtSource/UI/icon_flood_tip_water.aseprite
ArtSource/UI/icon_flood_trash_bin.aseprite
Assets/Art/Level/v2/street_canal_grate.png
Assets/Art/Level/v2/street_trash_bin.png
Assets/Art/Level/v2/street_wrapper.png
Assets/Art/Level/v2/yard_can_tipped.png
Assets/Art/Level/v2/yard_can_water.png
Assets/Art/Level/v2/yard_tire_tipped.png
Assets/Art/Level/v2/yard_tire_water.png
Assets/Art/UI/icon_flood_canal.png
Assets/Art/UI/icon_flood_leave_water.png
Assets/Art/UI/icon_flood_tip_water.png
Assets/Art/UI/icon_flood_trash_bin.png
~~~

</details>

<details>
<summary>Batch 6 — after (23 sources, 26 PNGs)</summary>

~~~text
ArtSource/Level/v2/evacuation_area_basin.aseprite
ArtSource/Level/v2/evacuation_area_dipper.aseprite
ArtSource/Level/v2/flood_street_leftover.aseprite
ArtSource/Level/v2/home_after_bread.aseprite
ArtSource/Level/v2/home_after_mud.aseprite
ArtSource/Level/v2/street_mud.aseprite
ArtSource/Student/boy_wash.aseprite
ArtSource/Student/girl_wash.aseprite
ArtSource/UI/icon_flood_eat_bread.aseprite
ArtSource/UI/icon_flood_feed_cat.aseprite
ArtSource/UI/icon_flood_give_bread.aseprite
ArtSource/UI/icon_flood_let_dry.aseprite
ArtSource/UI/icon_flood_pick_water.aseprite
ArtSource/UI/icon_flood_play_mud.aseprite
ArtSource/UI/icon_flood_plug_fan.aseprite
ArtSource/UI/icon_flood_see_street.aseprite
ArtSource/UI/icon_flood_stay_mom.aseprite
ArtSource/UI/icon_flood_switch_box.aseprite
ArtSource/UI/icon_flood_turn_on_tv.aseprite
ArtSource/UI/icon_flood_wait_check.aseprite
ArtSource/UI/icon_flood_wash_feet.aseprite
ArtSource/UI/icon_flood_wipe_bread.aseprite
ArtSource/UI/icon_flood_wipe_shirt.aseprite
Assets/Art/Level/v2/evacuation_area_basin.png
Assets/Art/Level/v2/evacuation_area_dipper.png
Assets/Art/Level/v2/flood_street_leftover_0.png
Assets/Art/Level/v2/flood_street_leftover_1.png
Assets/Art/Level/v2/home_after_bread.png
Assets/Art/Level/v2/home_after_mud.png
Assets/Art/Level/v2/street_mud.png
Assets/Art/Student/boy_wash_0.png
Assets/Art/Student/boy_wash_1.png
Assets/Art/Student/girl_wash_0.png
Assets/Art/Student/girl_wash_1.png
Assets/Art/UI/icon_flood_eat_bread.png
Assets/Art/UI/icon_flood_feed_cat.png
Assets/Art/UI/icon_flood_give_bread.png
Assets/Art/UI/icon_flood_let_dry.png
Assets/Art/UI/icon_flood_pick_water.png
Assets/Art/UI/icon_flood_play_mud.png
Assets/Art/UI/icon_flood_plug_fan.png
Assets/Art/UI/icon_flood_see_street.png
Assets/Art/UI/icon_flood_stay_mom.png
Assets/Art/UI/icon_flood_switch_box.png
Assets/Art/UI/icon_flood_turn_on_tv.png
Assets/Art/UI/icon_flood_wait_check.png
Assets/Art/UI/icon_flood_wash_feet.png
Assets/Art/UI/icon_flood_wipe_bread.png
Assets/Art/UI/icon_flood_wipe_shirt.png
~~~

</details>

<details>
<summary>Batch 7 — menu (4 sources, 4 PNGs)</summary>

~~~text
ArtSource/Background/bg_menu_flood_easy.aseprite
ArtSource/Background/bg_menu_flood_episode.aseprite
ArtSource/Background/bg_menu_flood_hard.aseprite
ArtSource/Background/bg_menu_flood_normal.aseprite
Assets/Art/Background/bg_menu_flood_easy.png
Assets/Art/Background/bg_menu_flood_episode.png
Assets/Art/Background/bg_menu_flood_hard.png
Assets/Art/Background/bg_menu_flood_normal.png
~~~

</details>


## Landslide Batch 1 — hillside (approved)

All coordinates are art pixels from top left. The 480×270 Setting is bottom anchored. The flat packed-earth yard starts near x 210. Student feet register at (16,49), Grown-up feet at (18,61). The walking line is y 236. The slope toe ends at x 206, the coconut trunk meets the hillside at (84,151), and the low door step spans x 370–415 at y 206–209. The central Caption and the Pause and Hint boxes use the Flood HUD outlines. The wide view drops rows 0–53.

Easy 1 uses hillside_crack at (176,218). Normal 1 uses the sky tint and light Rain at (0,0), and the spring at (136,200). Play its four frames once at 220 ms, then hold muddy frame 3. Rain repeats three frames at 120 ms. Hard 4 uses hillside_soil at (0,137), hillside_toy_soil at (161,207), hillside_sign at (147,168) and hillside_rope at (199,197). The toy reuses home_toy with its wheels covered. The board fits the shared font on two lines.

Use the existing boy and girl poses. A run-start is a left-facing walk stride held at the stop. Idle, happy and wait are front views. For Dad’s turn, hold mirrored dad_walk frame 0. Mom comes along the yard to the open door from the right, starting at (455,245); the Grown-ups stand at (392,245) where their faces clear the wide Hint. Each ending reserves the 67×26 Reaction above the Student at (325,150).

The mask includes the painted Slope, climbing path, foot path and active props. The Normal 1 mask unions all spring frames. Gaps use the lowest three opaque rows of each pose across its frames, both Students and both directions as staged. The route column checks each intermediate art pixel. A dash means no actor or no movement.

### Easy-1

| Step | From | Student pose, feet, facing | Grown-up pose, feet, facing | Student gap | Grown-up gap | Walking route minimum |
|---|---|---|---|---:|---:|---:|
| 0. Start by the yard | — | idle (300,236) front | dad_idle (392,245) front | 82.6 px | 174.7 px | — |
| 1. Jump / hold first stride | 0 | walk (243,236) left | dad_idle (392,245) front | 25.8 px | 174.7 px | 25.8 px |
| 2. Safe / turn away from the crack | 0 | walk (300,236) right | dad_idle (392,245) front | 77.5 px | 174.7 px | — |
| 3. Safe / walk to Dad | 2 | walk (358,236) right | dad_idle (392,245) front | 134.8 px | 174.7 px | 77.5 px |
| 4. Safe / call to Dad | 3 | call (358,236) right | dad_idle (392,245) front | 143.8 px | 174.7 px | — |
| 5. Dad turns toward the crack | 4 | idle (358,236) front | dad_walk (392,245) left | 139.9 px | 173.7 px | — |
| 6. Ending / stay with Dad | 5 | happy (358,236) front | dad_walk (392,245) left | 139.9 px | 173.7 px | — |

Minimum hazard gap including routes: 25.8 art pixels.

### Normal-1

| Step | From | Student pose, feet, facing | Grown-up pose, feet, facing | Student gap | Grown-up gap | Walking route minimum |
|---|---|---|---|---:|---:|---:|
| 0. Start / clear seep | — | idle (300,236) front | — | 91.5 px | — | — |
| 1. Notice the muddy spring | 0 | surprised (300,236) front | — | 91.5 px | — | — |
| 2. Splash / hold first stride | 1 | walk (235,236) left | — | 36.1 px | — | 36.1 px |
| 3. Look / stop on flat ground | 1 | wait (247,236) front | — | 46.7 px | — | 43.8 px |
| 4. Safe / walk to the door | 1 | walk (358,236) right | — | 141.7 px | — | 86.5 px |
| 5. Safe / call to Mom | 4 | call (358,236) right | — | 150.4 px | — | — |
| 6. Mom walks to the door from the right | 5 | call (358,236) right | mom_walk (455,245) left | 150.4 px | 242.2 px | — |
| 7. Ending / Mom joins the Student | 6 | happy (358,236) front | mom_idle (392,245) front | 146.7 px | 181.7 px | 180.7 px |

Minimum hazard gap including routes: 36.1 art pixels.

### Hard-4

| Step | From | Student pose, feet, facing | Grown-up pose, feet, facing | Student gap | Grown-up gap | Walking route minimum |
|---|---|---|---|---:|---:|---:|
| 0. Start / soil lies across the path | — | idle (300,236) front | mom_idle (392,245) front | 68.1 px | 160.3 px | — |
| 1. Climb / stop before the rope | 0 | walk (259,236) left | mom_idle (392,245) front | 25.5 px | 160.3 px | 25.5 px |
| 2. Toy / crouch on the yard side | 0 | crouch (270,236) left | mom_idle (392,245) front | 37.6 px | 160.3 px | 35.7 px |
| 3. Look / stop farther back | 0 | wait (282,236) front | mom_idle (392,245) front | 50.4 px | 160.3 px | 47.3 px |
| 4. Safe / walk back to Mom | 0 | walk (350,236) right | mom_idle (392,245) front | 112.5 px | 160.3 px | 63.0 px |
| 5. Safe / stand with Mom | 4 | idle (358,236) front | mom_idle (392,245) front | 125.6 px | 160.3 px | 112.5 px |
| 6. Ending / stay beside the door | 5 | happy (358,236) front | mom_idle (392,245) front | 125.6 px | 160.3 px | — |

Minimum hazard gap including routes: 25.5 art pixels.

### Choice icons

Reused icon_fire_tell_dad for telling Dad about the crack, and icon_fire_tell_mom for telling Mom about the spring. All icons are 16×16 and still.

### Imported art inventory

| Source / matching PNG stem | Size | Frames | Frame duration |
|---|---|---:|---|
| Level/v2/bg_hillside | 480×270 | 1 | still |
| Level/v2/hillside_crack | 36×9 | 1 | still |
| Level/v2/hillside_rope | 25×29 | 1 | still |
| Level/v2/hillside_sign | 57×36 | 1 | still |
| Level/v2/hillside_soil | 200×90 | 1 | still |
| Level/v2/hillside_spring | 56×24 | 4 | 220 ms |
| Level/v2/hillside_toy_soil | 15×9 | 1 | still |
| Level/v2/landslide_dark_sky_hillside | 480×270 | 1 | still |
| UI/icon_landslide_climb_soil | 16×16 | 1 | still |
| UI/icon_landslide_jump_crack | 16×16 | 1 | still |
| UI/icon_landslide_look_close | 16×16 | 1 | still |
| UI/icon_landslide_look_spring | 16×16 | 1 | still |
| UI/icon_landslide_splash_mud | 16×16 | 1 | still |
| UI/icon_landslide_stay_back | 16×16 | 1 | still |
| UI/icon_landslide_toy_soil | 16×16 | 1 | still |
`ArtSource/<stem>.aseprite` is the editable source. PNGs under `Assets/Art/<stem>.png` use the same stem, with frame suffixes `_0` to `_3` for the spring.

Approved [preview](D:/GameDEV/landslide-episode-scratch/art/01-hillside/preview.png).

Import command from the project root

```powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=landslide_hillside,icons_landslide_signs --script ArtSource/scripts/build.lua
```

## Landslide Batch 2 — Hillside habits, rain and after (approved)

Art pixels from upper left. Background 480×270, bottom anchored. Wide view crops rows 0–53. Actor feet are local (16,49) for a Student or (18,61) for a Grown-up. Props and effects use their top left. Slope toe and packed-earth yard follow the approved Batch 1 background. Student walk line y 236 except for the two H1 door stops and the H5 crouch beside the pail. Grown-up paths use their own feet below. Every segment interpolates in a straight line from the listed parent. The red mask outline is a review guide.

Pot top left relative to feet, facing right: carry (+5,−28) on passing frames or (+5,−27) on contact frames, crouch (+5,−14) frame 0 and (+5,−13) frame 1. Mirror around the feet for the left-facing Dig stop. Empty changes to full at the garden box. H1 and N2 use light Rain (0,0), 3×120 ms. Composite the N2 lean patch after Dark Sky so it restores sky behind the former crown. Hold the tree still. N5 follows the rain. H7 starts after the earthquake with a still, dry Setting.

The rearmost safe yard entry is on the left side of the yard, x 242. The Slope occupies the left screen edge. H5 Dad faces left as authored, stays planted and uses the 400 ms shovel frames. H1 family order from left to right is Student, Mom, Dad, Tanod. Continue to the offscreen coordinates for the ending. Reserve the H1 Reaction before the group exits.

### easy-2

Props: hillside_garden_box (362,215).

Reaction top left (315,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start with the empty pot | — | carry (298,236) right | — | 84.7 | — |
| 1. Dig / stop on flat ground | 0 | crouch (249,236) left | — | 46.7 | 45.3 |
| 2. Safe / carry the pot to the garden box | 0 | carry (348,236) right | — | 132.0 | 84.7 |
| 3. Safe / crouch beside the box | 2 | crouch (348,236) right | — | 133.0 | — |
| 4. Safe / fill the pot | 3 | crouch (348,236) right | — | 133.0 | — |
| 5. Ending / stand with the full pot | 4 | carry (348,236) right | — | 132.0 | — |

Minimum including both Students, every pose frame and all walks: 45.3 px.

### easy-3

Props: yard_ball (169,208), hillside_sign (143,163), hillside_rope (199,197).

Reaction top left (325,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start / ball past the rope | — | idle (292,236) front | mom_idle (392,245) front | 60.2, 160.3 | — |
| 1. Rope / crouch on the yard side | 0 | crouch (260,236) left | mom_idle (392,245) front | 28.2, 160.3 | 26.4 |
| 2. Safe / turn from the ball | 0 | walk (292,236) right | mom_idle (392,245) front | 55.1, 160.3 | — |
| 3. Safe / walk to Mom at the door | 2 | walk (358,236) right | mom_idle (392,245) front | 120.5, 160.3 | 55.1 |
| 4. Ending / stay with Mom | 3 | happy (358,236) front | mom_idle (392,245) front | 125.6, 160.3 | — |

Minimum including both Students, every pose frame and all walks: 26.4 px.

### normal-2

Props: landslide_dark_sky_hillside (0,0), hillside_tree_lean (60,54).

Reaction top left (359,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Enter at the left edge of the safe yard | — | walk (242,236) right | — | 39.1 | — |
| 1. Walk home / notice the leaning tree | 0 | idle (288,236) front | — | 80.5 | 39.1 |
| 2. Tree / stop before the foot | 1 | wait (246,236) front | — | 46.0 | 43.1 |
| 3. Coconut / reach up on the flat | 1 | reachup (263,236) left | — | 60.7 | 55.8 |
| 4. Safe / walk along the yard | 1 | walk (337,236) right | — | 121.5 | 75.6 |
| 5. Ending / arrive at the door | 4 | happy (392,236) front | — | 179.8 | 121.5 |

Minimum including both Students, every pose frame and all walks: 39.1 px.

### normal-5

Props: hillside_wet_slope (0,62), hillside_banana_leaf (221,225).

Reaction top left (290,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start after Rain | — | idle (286,236) front | — | 78.7 | — |
| 1. Slide / stop near the banana leaf | 0 | wait (248,236) front | — | 47.4 | 44.6 |
| 2. Climb / stop before the path | 0 | walk (262,236) left | — | 55.0 | 55.0 |
| 3. Safe / move to the middle of the yard | 0 | walk (323,236) right | — | 108.2 | 73.8 |
| 4. Ending / play on the flat | 3 | happy (323,236) front | — | 113.2 | — |

Minimum including both Students, every pose frame and all walks: 44.6 px.

### hard-1

Props: landslide_dark_sky_hillside (0,0), home_toy (391,192).

Reaction top left (341,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start / the Tanod calls | — | idle (306,236) front | mom_walk_gobag (406,250) right; dad_idle (433,249) front; tanod_megaphone (463,246) front | 97.1, 191.6, 222.5, 251.2 | — |
| 1. Rain / sit on the low step | 0 | sit (374,220) right | mom_walk_gobag (406,250) right; dad_idle (433,249) front; tanod_megaphone (463,246) front | 153.3, 191.6, 222.5, 251.2 | 92.1 |
| 2. Toy / stop before the open door | 0 | wait (366,221) front | mom_walk_gobag (406,250) right; dad_idle (433,249) front; tanod_megaphone (463,246) front | 151.6, 191.6, 222.5, 251.2 | 92.1 |
| 3. Look / stop on the flat | 0 | walk (248,236) left | mom_walk_gobag (406,250) right; dad_idle (433,249) front; tanod_megaphone (463,246) front | 44.6, 191.6, 222.5, 251.2 | 44.6 |
| 4. Safe / join Mom and Dad | 0 | walk (374,236) right | mom_walk_gobag (406,250) right; dad_idle (433,249) front; tanod_megaphone (463,246) front | 157.2, 191.6, 222.5, 251.2 | 92.1 |
| 5. Safe / follow the Tanod right | 4 | walk (411,236) right | mom_walk_gobag (446,250) right; dad_walk (476,249) right; tanod_walk (508,246) right | 193.4, 230.5, 259.7, 290.7 | 157.2 |
| 6. Ending / family leaves the yard | 5 | walk (504,236) right | mom_walk_gobag (539,250) right; dad_walk (569,249) right; tanod_walk (601,246) right | 285.3, 321.9, 351.4, 382.8 | 193.4 |

Minimum including both Students, every pose frame and all walks: 44.6 px.

### hard-5

Props: hillside_soil (0,137), hillside_sign (116,162), hillside_rope (173,172), hillside_shovel (322,166), hillside_pail (315,247).

Reaction top left (324,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start / Dad clears the edge | — | idle (304,236) front | dad_shovel (208,229) left; mom_idle (392,245) front | 95.3, 24.2, 181.7 | — |
| 1. Shovel / stop before the spare tool | 0 | wait (346,234) front | dad_shovel (208,229) left; mom_idle (392,245) front | 134.6, 24.2, 181.7 | 90.2 |
| 2. Pail / crouch short of the pail | 0 | crouch (345,257) left | dad_shovel (208,229) left; mom_idle (392,245) front | 139.8, 24.2, 181.7 | 90.2 |
| 3. Mud / stop on the flat | 0 | wait (257,236) front | dad_shovel (208,229) left; mom_idle (392,245) front | 54.0, 24.2, 181.7 | 51.1 |
| 4. Safe / walk to Mom | 0 | walk (357,236) right | dad_shovel (208,229) left; mom_idle (392,245) front | 140.7, 24.2, 181.7 | 90.2 |
| 5. Ending / stand beside Mom | 4 | idle (357,236) front | dad_shovel (208,229) left; mom_idle (392,245) front | 145.8, 24.2, 181.7 | — |

Minimum including both Students, every pose frame and all walks: 24.2 px.

### hard-7

Props: approved hillside only.

Reaction top left (281,148), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start at the door after shaking | — | idle (378,236) front | mom_idle (411,245) front | 166.2, 200.1 | — |
| 1. Slope / stop on the flat | 0 | wait (247,236) front | mom_idle (411,245) front | 46.7, 200.1 | 43.8 |
| 2. Tree / sit well back | 0 | sit (266,236) left | mom_idle (411,245) front | 55.8, 200.1 | 58.2 |
| 3. Path / stop before the climb | 0 | walk (284,236) left | mom_idle (411,245) front | 73.8, 200.1 | 73.8 |
| 4. Safe / walk together into the yard | 0 | walk (347,236) left | mom_walk (383,245) left | 133.0, 172.0 | 133.0 |
| 5. Ending / stand together in the open | 4 | happy (314,236) front | mom_idle (350,245) front | 104.7, 141.4 | 101.5 |

Minimum including both Students, every pose frame and all walks: 43.8 px.

### Icon reuse

Leave now → icon_flood_leave_now; stay with Mom → icon_flood_stay_mom; return for toy → icon_typhoon_back_for_toy; play in mud → icon_flood_play_mud.

### Imported art inventory

| Source stem in ArtSource / PNG stem in Assets/Art | Size | Frames | Timing |
|---|---|---:|---|
| GrownUp/dad_shovel | 36×62 | 2 | 400 ms |
| Level/v2/hillside_banana_leaf | 24×8 | 1 | still |
| Level/v2/hillside_garden_box | 44×20 | 1 | still |
| Level/v2/hillside_pail | 17×17 | 1 | still |
| Level/v2/hillside_pot_empty | 12×11 | 1 | still |
| Level/v2/hillside_pot_full | 12×11 | 1 | still |
| Level/v2/hillside_shovel | 17×38 | 1 | still |
| Level/v2/hillside_tree_lean | 108×108 | 1 | still |
| Level/v2/hillside_wet_slope | 209×154 | 1 | still |
| UI/icon_landslide_by_slope | 16×16 | 1 | still |
| UI/icon_landslide_climb_slope | 16×16 | 1 | still |
| UI/icon_landslide_coconut | 16×16 | 1 | still |
| UI/icon_landslide_dig_slope | 16×16 | 1 | still |
| UI/icon_landslide_flat_yard | 16×16 | 1 | still |
| UI/icon_landslide_garden_box | 16×16 | 1 | still |
| UI/icon_landslide_leave_ball | 16×16 | 1 | still |
| UI/icon_landslide_look_slope | 16×16 | 1 | still |
| UI/icon_landslide_open_yard | 16×16 | 1 | still |
| UI/icon_landslide_pail | 16×16 | 1 | still |
| UI/icon_landslide_path_up | 16×16 | 1 | still |
| UI/icon_landslide_shovel | 16×16 | 1 | still |
| UI/icon_landslide_slide_leaf | 16×16 | 1 | still |
| UI/icon_landslide_under_rope | 16×16 | 1 | still |
| UI/icon_landslide_under_tree | 16×16 | 1 | still |
| UI/icon_landslide_wait_rain | 16×16 | 1 | still |
| UI/icon_landslide_walk_house | 16×16 | 1 | still |

Approved [preview](D:/GameDEV/landslide-episode-scratch/art/02-hillside-habits/preview.png). Sources use .aseprite and exports use .png with _0 and _1 for the shovel animation.

```powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=landslide_hillside_more,grownups_landslide,icons_landslide_hillside --script-param poses=shovel --script ArtSource/scripts/build.lua
```

## Landslide Batch 3 — Two hill roads (approved)

Art pixels from upper left. Both 480×270 backgrounds are bottom anchored. Local feet are (16,49) for the Student and (18,61) for each Grown-up. Mask outlines and HUD outlines appear on placement sheets only. Both Rain and the matching Dark Sky tint register at (0,0). Light Rain loops at 120 ms. Render static background, sky tint, hazard overlays, actors in ascending foot-y order, then Rain.

Normal 4 uses bg_hill_road and landslide_dark_sky_hill_road. Its asphalt bends past the elevated woven-panel home and the plaster bungalow, with the sign at (184,121). The Student and Dad use the broad gravel shoulder at y 236 and 246. Muddy Runoff crosses the asphalt from the left cut, four frames at 180 ms looping toward the near edge; the adjacent soil is still. Start offscreen right, enter to notice the blockage, then turn back on the Safe Choice.

Hard 2 uses bg_hill_road_ridge and landslide_dark_sky_hill_road_ridge. The foreground is a broad concrete turnout with expansion joints. A guardrail separates the road from the valley. The sign is at (402,113). The distant bare hill across the valley is uninhabited. Play the six slide frames at 150 ms once and hold frame 5 through Choices and retries. The group walks toward the sign; the Unsafe run-ahead uses the foreground side of the turnout. Paths interpolate between the listed feet, one art pixel at a time. Walking lines vary from y 208 to 263, as recorded below.

### normal-4

Props: hill_road_runoff (84,176), hill_road_soil (151,190).

Reaction top left (349,120), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Enter from the right with Dad | — | walk (500,236) left | dad_walk (539,246) left | 282.7, 324.6 | — |
| 1. Notice the runoff across the bend | 0 | surprised (314,236) left | dad_walk (356,246) left | 103.1, 144.9 | 99.9 |
| 2. Cross / stop on dry gravel | 1 | walk (242,236) left | dad_walk (356,246) left | 35.6, 144.9 | 35.6 |
| 3. Soil / stop before the deposit | 1 | wait (262,236) front | dad_walk (356,246) left | 55.9, 144.9 | 52.8 |
| 4. Safe / turn back together | 1 | walk (314,236) right | dad_walk (356,246) right | 98.0, 141.1 | — |
| 5. Follow Dad back along the shoulder | 4 | walk (376,236) right | dad_walk (418,246) right | 158.1, 201.2 | 98.0 |
| 6. Ending / take the long way | 5 | happy (409,236) front | dad_idle (450,245) front | 195.6, 237.4 | 158.1 |

Minimum including both Students, every pose frame and all walks: 35.6 px.

### hard-2

Props: hill_road_far_slide (353,94).

Reaction top left (313,120), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start / notice movement across the valley | — | surprised (231,263) right | mom_walk_gobag (276,210) right; tanod_walk (317,208) right | 100.8, 100.9, 94.4 | — |
| 1. The distant slide comes to rest | 0 | surprised (231,263) right | mom_walk_gobag (276,210) right; tanod_walk (317,208) right | 100.8, 100.9, 94.4 | — |
| 2. Shortcut / stop before the cut Slope | 1 | walk (200,246) left | mom_walk_gobag (276,210) right; tanod_walk (317,208) right | 68.0, 100.9, 94.4 | 68.0 |
| 3. Watch / pause facing the far hill | 1 | wait (250,263) front | mom_walk_gobag (276,210) right; tanod_walk (317,208) right | 113.1, 100.9, 94.4 | 97.3 |
| 4. Ahead / stop past the Tanod on the turnout | 1 | walk (355,263) right | mom_walk_gobag (276,210) right; tanod_walk (317,208) right | 140.0, 100.9, 94.4 | 97.3 |
| 5. Safe / follow Mom and the Tanod | 1 | walk (300,251) right | mom_walk_gobag (342,230) right; tanod_walk (383,238) right | 139.8, 108.3, 115.0 | 94.0 |
| 6. Safe / reach the evacuation route sign | 5 | walk (373,245) right | mom_walk_gobag (412,246) right; tanod_walk (450,246) right | 122.0, 125.7, 138.6 | 108.0 |
| 7. Ending / stay with the group | 6 | happy (373,245) front | mom_walk_gobag (412,246) right; tanod_walk (450,246) right | 122.0, 125.7, 138.6 | — |

Minimum including both Students, every pose frame and all walks: 68.0 px.

### Reused Choices

Climb over the soil uses icon_landslide_climb_soil. Keep walking with the Tanod uses icon_typhoon_evac_route.

### Imported inventory

| Source stem and PNG stem | Size | Frames | Timing |
|---|---|---:|---|
| Level/v2/bg_hill_road | 480×270 | 1 | still |
| Level/v2/bg_hill_road_ridge | 480×270 | 1 | still |
| Level/v2/hill_road_far_slide | 30×33 | 6 | 150 ms |
| Level/v2/hill_road_runoff | 120×47 | 4 | 180 ms |
| Level/v2/hill_road_soil | 58×18 | 1 | still |
| Level/v2/landslide_dark_sky_hill_road | 480×270 | 1 | still |
| Level/v2/landslide_dark_sky_hill_road_ridge | 480×270 | 1 | still |
| UI/icon_landslide_cross_mud | 16×16 | 1 | still |
| UI/icon_landslide_long_way | 16×16 | 1 | still |
| UI/icon_landslide_run_ahead | 16×16 | 1 | still |
| UI/icon_landslide_shortcut | 16×16 | 1 | still |
| UI/icon_landslide_watch_hill | 16×16 | 1 | still |

Approved [preview](D:/GameDEV/landslide-episode-scratch/art/03-hill-road/preview.png). Sources use .aseprite, still exports use .png, and animated exports use zero-based frame suffixes.

```powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=hill_road,icons_landslide_road --script ArtSource/scripts/build.lua
```

## Landslide Batch 4 — Classroom, Evacuation Center and home (approved)

All coordinates are art pixels measured from upper left. Each new 480×270 background is bottom anchored. The wide crop removes rows 0–53. Student feet register at (16,49); Grown-up feet at (18,61). The tables list all actor coordinates and parent steps. Interpolate walking paths in a straight line between successive points. A dash in the route column means the actors hold position. “Clear” means no hazard pixels in this Setting.

Normal 3: classroom floor begins at y 204. The closed door is part of the background. Show the open door overlay only from Mom’s arrival; she first appears at (46,214). Rain loops inside the glass only, four frames at 180 ms. Draw the desk front behind standing Students and in front of the seated Student. The bench top is at (226,207). The Teacher stands 5 px farther right than in the Typhoon staging to clear the walk. The Student leaves the desk via the foreground aisle and meets Mom at (112,238).

Hard 3: pavilion slab begins at y 157. Mat top row 224, near edge 248. Two tents flank the central aisle. The small family follows the back aisle between the tents as the friend cue (4 frames, 140 ms). Top left moves from (255,129) to (279,129), three pixels per listed Act-out step. Fade the sky mask from full strength to clear by the announcement. The mask uses the Typhoon sky pixels clipped to the new pavilion openings. The Tanod enters at (442,235) and uses the aisle toward Mom. The Play stop uses the open right aisle to keep the run clear of seated Mom.

Hard 6: floor begins at y 202. Roll on side table, top left (226,162). For the left-facing carry pose its top left is (feet x−17, feet y−35), one row lower on contact frames 0 and 2. The crack and the distant hillside behind the window are still. The mask includes the crack and a conservative silhouette of the entire hill in the glass. Hold mirrored dad_walk frame 0 for his turn.

### normal-3

Props: classroom_window_storm (341,67), desk_front (198,188), classroom_landslide_door_open (20,103).

Reaction top left (90,145), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start inside the classroom | — | idle (146,230) front | teacher_idle (295,221) front | Clear, Clear | — |
| 1. Hill road / stop inside | 0 | walk (114,230) left | teacher_idle (295,221) front | Clear, Clear | Clear |
| 2. Gate / face the closed door | 0 | walk (94,230) left | teacher_idle (295,221) front | Clear, Clear | Clear |
| 3. Safe / walk to the Teacher | 0 | walk (263,238) right | teacher_idle (295,221) front | Clear, Clear | Clear |
| 4. Safe / sit behind the desk | 3 | sit (242,207) right | teacher_idle (295,221) front | Clear, Clear | Clear |
| 5. Mom arrives at the open door | 4 | sit (242,207) right | teacher_idle (295,221) front; mom_umbrella (46,214) right | Clear, Clear, Clear | — |
| 6. Mom steps into the classroom | 5 | sit (242,207) right | teacher_idle (295,221) front; mom_umbrella (79,214) right | Clear, Clear, Clear | Clear |
| 7. Safe / stand on the aisle side | 6 | walk (248,245) left | teacher_idle (295,221) front; mom_umbrella (79,214) right | Clear, Clear, Clear | Clear |
| 8. Walk to Mom inside | 7 | walk (112,238) left | teacher_idle (295,221) front; mom_umbrella (79,214) right | Clear, Clear, Clear | Clear |
| 9. Ending / together inside | 8 | happy (112,238) front | teacher_idle (295,221) front; mom_umbrella (79,214) right | Clear, Clear, Clear | — |

No active hazard pixels.

### hard-3

Props: landslide_dark_sky_evacuation_area (0,0), evacuation_area_leaving (255,129).

Reaction top left (193,120), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start on the mat | — | idle (185,236) front | mom_sit (132,236) right | Clear, Clear | — |
| 1. Friend / stop at the court edge | 0 | walk (388,229) right | mom_sit (132,236) right | Clear, Clear | Clear |
| 2. House / stop between the tents | 0 | wait (241,181) front | mom_sit (132,236) right | Clear, Clear | Clear |
| 3. Play / run-start toward the open aisle | 0 | walk (341,251) right | mom_sit (132,236) right | Clear, Clear | Clear |
| 4. Safe / sit with Mom | 0 | sit (173,236) right | mom_sit (132,236) right | Clear, Clear | Clear |
| 5. The Tanod enters from the right | 4 | sit (173,236) right | mom_sit (132,236) right; tanod_walk (442,235) left | Clear, Clear, Clear | — |
| 6. The Tanod crosses the hall | 5 | sit (173,236) right | mom_sit (132,236) right; tanod_walk (366,235) left | Clear, Clear, Clear | Clear |
| 7. Wait for the announcement | 6 | sit (173,236) right | mom_sit (132,236) right; tanod_megaphone (231,224) front | Clear, Clear, Clear | Clear |
| 8. Ending / the Tanod has given the all-clear | 7 | happy (173,236) front | mom_sit (132,236) right; tanod_megaphone (231,224) front | Clear, Clear, Clear | — |

No active hazard pixels.

### hard-6

Props: home_window_hill (304,72), home_wall_crack (139,154), home_poster (226,162).

Reaction top left (289,145), 67×26.

| Step | Parent | Student | Grown-ups | Pose gaps in actor order (px) | Smallest route gap (px) |
|---|---:|---|---|---|---:|
| 0. Start / notice the wall crack | — | surprised (279,234) right | dad_idle (407,244) front | 97.5, 111.3 | — |
| 1. Poster / collect the roll at the table | 0 | carry (245,216) left | dad_idle (407,244) front | 89.8, 111.3 | 89.8 |
| 2. Poster / stop short of the wall crack | 1 | carry (176,221) left | dad_idle (407,244) front | 33.8, 111.3 | 33.8 |
| 3. Pick / reach short of the crack | 0 | reach (163,221) left | dad_idle (407,244) front | 30.9, 111.3 | 28.6 |
| 4. Door / stop before the stuck door | 0 | reach (99,224) left | dad_idle (407,244) front | 49.7, 111.3 | 32.2 |
| 5. Safe / walk to Dad | 0 | walk (371,236) right | dad_idle (407,244) front | 98.0, 111.3 | 96.0 |
| 6. Safe / tell Dad | 5 | call (371,236) right | dad_idle (407,244) front | 98.0, 111.3 | — |
| 7. Dad turns to look at the wall | 6 | idle (371,236) front | dad_walk (407,244) left | 98.0, 111.0 | — |
| 8. Ending / stay with Dad | 7 | happy (371,236) front | dad_walk (407,244) left | 98.0, 111.0 | — |

Minimum including both Students, all poses and walks: 28.6 px.

### Reused Choices

Wait with the Teacher → icon_flood_stay_teacher. Wait for the Tanod → icon_typhoon_wait_tanod. Go home with your friend → icon_typhoon_visit_friend. Run out to play → icon_typhoon_run_play. All remaining Choices have new icons.

### Imported inventory

| Source and PNG stem | Size | Frames | Timing |
|---|---|---:|---|
| Level/v2/bg_classroom_landslide | 480×270 | 1 | still |
| Level/v2/bg_evacuation_area_landslide | 480×270 | 1 | still |
| Level/v2/bg_home_landslide | 480×270 | 1 | still |
| Level/v2/classroom_landslide_door_open | 54×98 | 1 | still |
| Level/v2/home_poster | 15×18 | 1 | still |
| Level/v2/home_wall_crack | 12×38 | 1 | still |
| Level/v2/home_window_hill | 60×65 | 1 | still |
| Level/v2/landslide_dark_sky_evacuation_area | 480×270 | 1 | still |
| UI/icon_landslide_check_house | 16×16 | 1 | still |
| UI/icon_landslide_gate | 16×16 | 1 | still |
| UI/icon_landslide_hill_road | 16×16 | 1 | still |
| UI/icon_landslide_pick_crack | 16×16 | 1 | still |
| UI/icon_landslide_poster | 16×16 | 1 | still |
| UI/icon_landslide_push_door | 16×16 | 1 | still |
| UI/icon_landslide_tell_dad_wall | 16×16 | 1 | still |

Approved [preview](D:/GameDEV/landslide-episode-scratch/art/04-indoor/preview.png). Each stem lists an editable .aseprite under ArtSource and a .png under Assets/Art.

```powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=landslide_home,icons_landslide_indoor --script ArtSource/scripts/build.lua
```

## Landslide Batch 5 — Menu backgrounds (approved)

Four 480×270 stills in Background/, each with an editable Aseprite source. Anchor bottom center (240,270). No frame timing or actor path. The menu renders its own Student. Foreground scenery and signs stay behind the menu controls. Distances are minimum art-pixel gaps between the listed detail and any control (including locks and the optional speech bubble).

Both standard 4× and wide 5× layouts use the Flood menu review geometry. Wide drops rows 0–53. The outlines show the header, Difficulty cards or Level slots, Back, Start where applicable, and the Menu Student and speech bubble.

### episode

Terraced hillside ends at y 161. The residential bench sits along the road; house facade origins (84,148), (148,147), (225,143), (294,133), (419,124). Road bends from (0,195) through (181,183) to (479,162). Foreground lane branches near (283,172) and leaves the bottom edge around x 126. Palm rooted at (374,158).

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| LANDSLIDE ZONE board (5, 102, 65, 125) | 2.5 | 20.7 |
| terrace strips at left (8, 82, 48, 98) | 19.5 | 40.5 |
| eastern home facade (419, 124, 471, 156) | 6.0 | 16.2 |
| lane in foreground (155, 230, 176, 240) | 40.0 | 18.8 |

### easy

Yard begins near y 203 at the left and y 180 at the right. Bamboo home sill at y 181, doorstep at (88,187), window at (13,99). Garden box at (31,196), bolted sign at (352,99), fence posts x 337, 379, 421, 463 and timber rails at y 131 and 144. Grassy Slope reaches its crest at (361,63).

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| bamboo window (13, 99, 64, 145) | 84.0 | 75.6 |
| garden box and shoots (31, 189, 75, 216) | 13.0 | 17.3 |
| timber fence and lashings (337, 121, 469, 158) | 4.0 | 15.4 |
| mounted warning board (352, 99, 454, 121) | 27.5 | 12.7 |

### normal

From inside the classroom: road along the hill at y 111–149; fence at y 149–165; dry school forecourt y 163–197; timber sill at y 198. Vertical window frame members x 0, 152, 320, 472. The raindrops are still marks in the glass. Chalk and eraser rest on the indoor tray.

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| hill road bend at left (14, 132, 69, 147) | 30.5 | 45.3 |
| rain on upper right glass (426, 77, 459, 90) | 45.5 | 68.8 |
| dry forecourt sample (76, 179, 91, 193) | 22.2 | 23.3 |
| chalk tray (10, 209, 76, 219) | 10.0 | 14.3 |

### hard

Porch landing y 174–181; lower steps at (41,182) and (34,189). Evacuation Center doorway at (51,93), window at (126,97), sign centred at (66,67), megaphone peg at (21,99). Rail follows the elevated approach from x 195 to x 343. Far hill crest at (404,60), static soil scar below; sunrise centred at (458,83).

| Landmark and bounds (left, top, right, bottom) | 1920 clearance | 2400 clearance |
|---|---:|---:|
| Evacuation Center sign (10, 67, 122, 84) | 7.5 | 17.4 |
| hanging megaphone (12, 99, 37, 128) | 6.5 | 27.2 |
| bare hill and soil scar (397, 65, 423, 86) | 5.5 | 37.1 |
| sunrise (445, 70, 472, 96) | 8.5 | 39.9 |


### Imported files

| File | Size | Frames | Timing |
|---|---|---:|---|
| ArtSource/Background/bg_menu_landslide_easy.aseprite | 480×270 | 1 | Still |
| ArtSource/Background/bg_menu_landslide_episode.aseprite | 480×270 | 1 | Still |
| ArtSource/Background/bg_menu_landslide_hard.aseprite | 480×270 | 1 | Still |
| ArtSource/Background/bg_menu_landslide_normal.aseprite | 480×270 | 1 | Still |
| Assets/Art/Background/bg_menu_landslide_easy.png | 480×270 | 1 | Still |
| Assets/Art/Background/bg_menu_landslide_episode.png | 480×270 | 1 | Still |
| Assets/Art/Background/bg_menu_landslide_hard.png | 480×270 | 1 | Still |
| Assets/Art/Background/bg_menu_landslide_normal.png | 480×270 | 1 | Still |

Approved [preview](D:/GameDEV/landslide-episode-scratch/art/05-menu/preview.png).

```powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=menu_landslide --script ArtSource/scripts/build.lua
```

## Landslide Batch 6 — Safety Reminder (approved)

The school gate at golden hour is the approved end-screen background. The review layout uses sample Tip wording. The implementation ticket owns final Tip text and the interactive screen.

| File | Size | Frames | Timing |
|---|---|---:|---|
| ArtSource/Background/bg_safety_reminder.aseprite | 480×270 | 1 | Still |
| Assets/Art/Background/bg_safety_reminder.png | 480×270 | 1 | Still |

Bottom-centre anchor (240,270). No actor walk or hazard in this background.

Approved [review sheet](D:/GameDEV/landslide-episode-scratch/art/06-safety-reminder/preview.png) and [placement sheet](D:/GameDEV/landslide-episode-scratch/art/06-safety-reminder/safety-reminder-placements.png).

~~~powershell
& "C:/Users/Marjovic/Desktop/aseprite/build/bin/aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script-param only=safety_reminder --script ArtSource/scripts/build.lua
~~~

### Artwork

Golden-hour light from the left. The sun is centred at (79,72). Pennant strings run across x 0–95 and 384–479, their ties at y 61 and low points near y 72. Gate caps start at (15,84) and (434,84). Gate leaves taper inside the piers to x 75 and 404. Near bases reach y 209. School wings flank the courtyard at y 111–148. The court starts at y 137, with the open path widening toward the bottom edge. The gate and bunting fit below the 54-row wide-screen crop. No lettering, emblems or actors are painted into the backdrop.

### Proposed layout for the implementation agent

Coordinates use art pixels from top left; rectangle limits are inclusive. The same anchors fit the standard view (4×) and wide crop (5× after removing rows 0–53). Header centred at (240,65), 2× shared art font. Free central panel (80,93)–(400,209). Emblem row y 99–138: five original 40×40 sprites with left edges 92, 156, 220, 284, 348. Episode labels start at y 143. Each Tip has a 58×52 area centred under its emblem, rows 156–207; line spacing 10. The panel fill is a preview UI layer. Student top left (82,213), feet (98,262); example Display Name centred at (149,231), with extra horizontal space through x 282 for a wider name or a Student line. Main Menu button (293,225)–(387,249). Earliest text y 65, 11 px below the wide crop. No Pause or Hint.

## Landslide art file inventory

<details>
<summary>Batch 1 — Hillside and its signs (15 sources, 18 PNGs)</summary>

~~~text
ArtSource/Level/v2/bg_hillside.aseprite
ArtSource/Level/v2/hillside_crack.aseprite
ArtSource/Level/v2/hillside_rope.aseprite
ArtSource/Level/v2/hillside_sign.aseprite
ArtSource/Level/v2/hillside_soil.aseprite
ArtSource/Level/v2/hillside_spring.aseprite
ArtSource/Level/v2/hillside_toy_soil.aseprite
ArtSource/Level/v2/landslide_dark_sky_hillside.aseprite
ArtSource/UI/icon_landslide_climb_soil.aseprite
ArtSource/UI/icon_landslide_jump_crack.aseprite
ArtSource/UI/icon_landslide_look_close.aseprite
ArtSource/UI/icon_landslide_look_spring.aseprite
ArtSource/UI/icon_landslide_splash_mud.aseprite
ArtSource/UI/icon_landslide_stay_back.aseprite
ArtSource/UI/icon_landslide_toy_soil.aseprite
Assets/Art/Level/v2/bg_hillside.png
Assets/Art/Level/v2/hillside_crack.png
Assets/Art/Level/v2/hillside_rope.png
Assets/Art/Level/v2/hillside_sign.png
Assets/Art/Level/v2/hillside_soil.png
Assets/Art/Level/v2/hillside_spring_0.png
Assets/Art/Level/v2/hillside_spring_1.png
Assets/Art/Level/v2/hillside_spring_2.png
Assets/Art/Level/v2/hillside_spring_3.png
Assets/Art/Level/v2/hillside_toy_soil.png
Assets/Art/Level/v2/landslide_dark_sky_hillside.png
Assets/Art/UI/icon_landslide_climb_soil.png
Assets/Art/UI/icon_landslide_jump_crack.png
Assets/Art/UI/icon_landslide_look_close.png
Assets/Art/UI/icon_landslide_look_spring.png
Assets/Art/UI/icon_landslide_splash_mud.png
Assets/Art/UI/icon_landslide_stay_back.png
Assets/Art/UI/icon_landslide_toy_soil.png
~~~

</details>

<details>
<summary>Batch 2 — Hillside habits, rain and after (26 sources, 27 PNGs)</summary>

~~~text
ArtSource/GrownUp/dad_shovel.aseprite
ArtSource/Level/v2/hillside_banana_leaf.aseprite
ArtSource/Level/v2/hillside_garden_box.aseprite
ArtSource/Level/v2/hillside_pail.aseprite
ArtSource/Level/v2/hillside_pot_empty.aseprite
ArtSource/Level/v2/hillside_pot_full.aseprite
ArtSource/Level/v2/hillside_shovel.aseprite
ArtSource/Level/v2/hillside_tree_lean.aseprite
ArtSource/Level/v2/hillside_wet_slope.aseprite
ArtSource/UI/icon_landslide_by_slope.aseprite
ArtSource/UI/icon_landslide_climb_slope.aseprite
ArtSource/UI/icon_landslide_coconut.aseprite
ArtSource/UI/icon_landslide_dig_slope.aseprite
ArtSource/UI/icon_landslide_flat_yard.aseprite
ArtSource/UI/icon_landslide_garden_box.aseprite
ArtSource/UI/icon_landslide_leave_ball.aseprite
ArtSource/UI/icon_landslide_look_slope.aseprite
ArtSource/UI/icon_landslide_open_yard.aseprite
ArtSource/UI/icon_landslide_pail.aseprite
ArtSource/UI/icon_landslide_path_up.aseprite
ArtSource/UI/icon_landslide_shovel.aseprite
ArtSource/UI/icon_landslide_slide_leaf.aseprite
ArtSource/UI/icon_landslide_under_rope.aseprite
ArtSource/UI/icon_landslide_under_tree.aseprite
ArtSource/UI/icon_landslide_wait_rain.aseprite
ArtSource/UI/icon_landslide_walk_house.aseprite
Assets/Art/GrownUp/dad_shovel_0.png
Assets/Art/GrownUp/dad_shovel_1.png
Assets/Art/Level/v2/hillside_banana_leaf.png
Assets/Art/Level/v2/hillside_garden_box.png
Assets/Art/Level/v2/hillside_pail.png
Assets/Art/Level/v2/hillside_pot_empty.png
Assets/Art/Level/v2/hillside_pot_full.png
Assets/Art/Level/v2/hillside_shovel.png
Assets/Art/Level/v2/hillside_tree_lean.png
Assets/Art/Level/v2/hillside_wet_slope.png
Assets/Art/UI/icon_landslide_by_slope.png
Assets/Art/UI/icon_landslide_climb_slope.png
Assets/Art/UI/icon_landslide_coconut.png
Assets/Art/UI/icon_landslide_dig_slope.png
Assets/Art/UI/icon_landslide_flat_yard.png
Assets/Art/UI/icon_landslide_garden_box.png
Assets/Art/UI/icon_landslide_leave_ball.png
Assets/Art/UI/icon_landslide_look_slope.png
Assets/Art/UI/icon_landslide_open_yard.png
Assets/Art/UI/icon_landslide_pail.png
Assets/Art/UI/icon_landslide_path_up.png
Assets/Art/UI/icon_landslide_shovel.png
Assets/Art/UI/icon_landslide_slide_leaf.png
Assets/Art/UI/icon_landslide_under_rope.png
Assets/Art/UI/icon_landslide_under_tree.png
Assets/Art/UI/icon_landslide_wait_rain.png
Assets/Art/UI/icon_landslide_walk_house.png
~~~

</details>

<details>
<summary>Batch 3 — Two hill roads (12 sources, 20 PNGs)</summary>

~~~text
ArtSource/Level/v2/bg_hill_road.aseprite
ArtSource/Level/v2/bg_hill_road_ridge.aseprite
ArtSource/Level/v2/hill_road_far_slide.aseprite
ArtSource/Level/v2/hill_road_runoff.aseprite
ArtSource/Level/v2/hill_road_soil.aseprite
ArtSource/Level/v2/landslide_dark_sky_hill_road.aseprite
ArtSource/Level/v2/landslide_dark_sky_hill_road_ridge.aseprite
ArtSource/UI/icon_landslide_cross_mud.aseprite
ArtSource/UI/icon_landslide_long_way.aseprite
ArtSource/UI/icon_landslide_run_ahead.aseprite
ArtSource/UI/icon_landslide_shortcut.aseprite
ArtSource/UI/icon_landslide_watch_hill.aseprite
Assets/Art/Level/v2/bg_hill_road.png
Assets/Art/Level/v2/bg_hill_road_ridge.png
Assets/Art/Level/v2/hill_road_far_slide_0.png
Assets/Art/Level/v2/hill_road_far_slide_1.png
Assets/Art/Level/v2/hill_road_far_slide_2.png
Assets/Art/Level/v2/hill_road_far_slide_3.png
Assets/Art/Level/v2/hill_road_far_slide_4.png
Assets/Art/Level/v2/hill_road_far_slide_5.png
Assets/Art/Level/v2/hill_road_runoff_0.png
Assets/Art/Level/v2/hill_road_runoff_1.png
Assets/Art/Level/v2/hill_road_runoff_2.png
Assets/Art/Level/v2/hill_road_runoff_3.png
Assets/Art/Level/v2/hill_road_soil.png
Assets/Art/Level/v2/landslide_dark_sky_hill_road.png
Assets/Art/Level/v2/landslide_dark_sky_hill_road_ridge.png
Assets/Art/UI/icon_landslide_cross_mud.png
Assets/Art/UI/icon_landslide_long_way.png
Assets/Art/UI/icon_landslide_run_ahead.png
Assets/Art/UI/icon_landslide_shortcut.png
Assets/Art/UI/icon_landslide_watch_hill.png
~~~

</details>

<details>
<summary>Batch 4 — Classroom, Evacuation Center and home (15 sources, 15 PNGs)</summary>

~~~text
ArtSource/Level/v2/bg_classroom_landslide.aseprite
ArtSource/Level/v2/bg_evacuation_area_landslide.aseprite
ArtSource/Level/v2/bg_home_landslide.aseprite
ArtSource/Level/v2/classroom_landslide_door_open.aseprite
ArtSource/Level/v2/home_poster.aseprite
ArtSource/Level/v2/home_wall_crack.aseprite
ArtSource/Level/v2/home_window_hill.aseprite
ArtSource/Level/v2/landslide_dark_sky_evacuation_area.aseprite
ArtSource/UI/icon_landslide_check_house.aseprite
ArtSource/UI/icon_landslide_gate.aseprite
ArtSource/UI/icon_landslide_hill_road.aseprite
ArtSource/UI/icon_landslide_pick_crack.aseprite
ArtSource/UI/icon_landslide_poster.aseprite
ArtSource/UI/icon_landslide_push_door.aseprite
ArtSource/UI/icon_landslide_tell_dad_wall.aseprite
Assets/Art/Level/v2/bg_classroom_landslide.png
Assets/Art/Level/v2/bg_evacuation_area_landslide.png
Assets/Art/Level/v2/bg_home_landslide.png
Assets/Art/Level/v2/classroom_landslide_door_open.png
Assets/Art/Level/v2/home_poster.png
Assets/Art/Level/v2/home_wall_crack.png
Assets/Art/Level/v2/home_window_hill.png
Assets/Art/Level/v2/landslide_dark_sky_evacuation_area.png
Assets/Art/UI/icon_landslide_check_house.png
Assets/Art/UI/icon_landslide_gate.png
Assets/Art/UI/icon_landslide_hill_road.png
Assets/Art/UI/icon_landslide_pick_crack.png
Assets/Art/UI/icon_landslide_poster.png
Assets/Art/UI/icon_landslide_push_door.png
Assets/Art/UI/icon_landslide_tell_dad_wall.png
~~~

</details>

<details>
<summary>Batch 5 — Menu backgrounds (4 sources, 4 PNGs)</summary>

~~~text
ArtSource/Background/bg_menu_landslide_easy.aseprite
ArtSource/Background/bg_menu_landslide_episode.aseprite
ArtSource/Background/bg_menu_landslide_hard.aseprite
ArtSource/Background/bg_menu_landslide_normal.aseprite
Assets/Art/Background/bg_menu_landslide_easy.png
Assets/Art/Background/bg_menu_landslide_episode.png
Assets/Art/Background/bg_menu_landslide_hard.png
Assets/Art/Background/bg_menu_landslide_normal.png
~~~

</details>

<details>
<summary>Batch 6 — Safety Reminder (1 sources, 1 PNGs)</summary>

~~~text
ArtSource/Background/bg_safety_reminder.aseprite
Assets/Art/Background/bg_safety_reminder.png
~~~

</details>

