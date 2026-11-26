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
| Hoop | The backboard is at (24, 116). The orange rim is on row 141, and the pole stands on the court at x 41. |
| Court | A concrete slab runs from row 154 to the bottom. Its right edge runs from (466, 155) to (430, 269). The green court has white lines, a centre circle and a key in front of the hoop. Its right baseline runs from (430, 162) to (410, 269), so the go home stop stands inside the paint. |
| Tents and people | A blue tent stands at the back from x 118 to 168 and an orange tent from x 310 to 362. Both bases are on row 158. An adult and a child stand at the blue tent's opening, with feet at (128, 160) and (140, 161). They stay left of the Reaction at the Safe ending. Another adult and child stand at the back on the right, at (374, 156) and (388, 157). |
| Mat | The woven mat runs from x 96 to 208 at row 226 and from x 86 to 218 at row 246. |
| Mom | Mom sits on the mat in `Sit` with her feet at (118, 236). She is a Grown-up actor, not part of the background. |
| Radio | The radio is two props at (136, 208), both 28 by 30: `evacuation_area_radio` (off) and `evacuation_area_radio_on` (two frames of 260 milliseconds, with yellow sound arcs above the radio). The sound arcs stay inside the mat's x range. |
| Neighbours | Three neighbours chat at (272, 164), 86 by 62, in two frames of 600 milliseconds: a nod and a hand gesture. They reuse the v2 Grown-up frames, with new clothes below the shoulders and grey hair on the older neighbour. |
| Path and houses | Through the open right side, a sandy earth path starts just outside the slab at x 432 on the bottom row. It climbs the grass beside the slab to the door of the right-hand house and flares to the right edge at the bottom. Its colour sets it apart from the concrete slab. |
| After the earthquake | A road sign at the right edge, beside the path, leans to the left. There is no fire, smoke or hurt person. |

### Hard 7, listen to the radio

The Student starts in Idle at (228, 230). Mom sits on the mat and stays there in every Act-out. The Reaction uses the default place above the Student, except at three places. At the go home and rumor stops it sits beside the Student, so the neighbours stay in view. At the Safe ending it sits 24 art pixels to the right, so the blue tent stays in view.

| Choice | Act-out points |
|---|---|
| radio | Walk to (182, 236) and sit beside Mom. The radio then turns on beside her, because Mom has no reaching pose. |
| go_home | Walk to (376, 248) and stop on the court, right of the neighbours, at the open side. The Student stands inside the court paint, and the right edge (x 392) stays well left of the path start (x 432). The Reaction sits 20 art pixels to the right of the Student. |
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
