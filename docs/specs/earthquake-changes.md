# Spec: Earthquake gameplay and art changes

Make each Difficulty easier to read, make the Student's movements match the Choices, and give the school and menus a recognizable setting. This specification covers the fifteen changes approved in the researcher interview on November 25, 2026. The researcher approved this specification before ticket writing.

## Scope

Work in the redesign checkout on the `earthquake-rollout` branch. All implementation follows rollout ticket 15. Use the completed cleanup as the visual baseline for new tickets. The Phase 1 captures include retired Choice cards and serve as historical comparisons. Scope and vocabulary follow the [approved design](../design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project glossary](../../CONTEXT.md).

C5 sets a Choice count for every Episode. The playable content in this release is Earthquake. Fire receives menu artwork and a destination from Level Completed. C3, C4 and C15 share menu references. C6, C9 and C10 belong together in the schoolyard. C12 and C13 form one implementation ticket for the shared home.

## Hard rules

- Use “Duck, Cover, and Hold” for the earthquake action. Keep each safety sentence tied to an existing finding in the [Earthquake research](../research/earthquake-safety.md). Bring unsupported wording to the researcher before using it.
- Each Scenario has one Safe Choice. An Unsafe Act-out stops before contact, harm or departure, on clear ground outside the hazard and doorway. A Safe ending must be visibly different from the nearest Unsafe stop. Each Reaction encourages the Learner and stays free of judgments and Stars.
- Keep existing rules for Attempts, Hints, Try Again, Stars, Open and Locked content, and Saved Progress. Preserve existing saves. Check affected controls, routes, faces, props and signs at 1920 × 1080 and 2400 × 1080, clear of the Caption, Hint and Pause.
- Use [ADR 0001](../adr/0001-v2-art-and-no-bantay.md) for all drawn art. Use one dark outer outline, three shades per colour, light from the left and the shared palette. Each Setting measures 480 × 270 art pixels, with four reference UI units per art pixel. Coordinates below measure the centre of the character's feet from the upper left.

## Behaviour

### C1. Next Episode on Level Completed

**Menu and Gameplay rule.** When the last outstanding Earthquake Hard Level opens Fire, put the Fire emblem and “Next Episode” in the Next position between Replay and Levels. This applies whichever Hard Level the Learner finishes last. Also show it after a Hard 7 replay if Fire is already Open. Finishing Hard 7 early follows the existing progression. Tapping the button opens Episode selection with Fire visible. Retain the existing coming-soon message while Fire has no Playable Levels. Check early Hard 7, out-of-order completion and replay through the actual Student flow.

### C2. Music by Episode and Difficulty

**Audio.** Play two welcoming instrumentals in turn in the menus. In a Level, play the gameplay track for its Episode's music set and its Difficulty: light on Easy, moderate on Normal and intense on Hard. Loop each menu and gameplay track after 45–90 seconds. On Level Completed, play the set's calm 10–20 second cue once. Set A plays in Earthquake, Typhoon and Landslide: Cherry Blossom on Easy, Happy Alley on Normal, Prelude and Action on Hard and Light Sting on Level Completed. Set B plays in Fire and Flood: Cheery Monday on Easy, B-Roll on Normal, Machinations on Hard and Piano Between on Level Completed. The menus play Somewhere Sunny and Easy Lemon (60 second) in turn, one after the other. Alternating the sets between neighbouring Episodes keeps the music new to the Learner. This release plays Set A, since Earthquake is the only Playable Episode. Starting or replaying a Level restores its music. Returning to the menus restores theirs. Respect Music and Sounds preferences, the existing fades and the reduction during shaking so the rumble stays audible.

The researcher chose ten natural-sounding recordings by Kevin MacLeod under CC BY 4.0. Claude downloaded them on 2026-11-26. Their sources are listed in `D:/GameDEV/earthquake-changes-scratch/10-licensed-music/tracks.md`. Sampled instruments are acceptable. Prefer lossless WAV or FLAC sources. Prepare 44.1 kHz WAVs for Unity, match loudness near −20 LUFS integrated, and keep true peaks at or below −1 dBTP. The C2 APK target is at most 50,000,000 bytes. The reference pilot measured 44,848,235 bytes. Measure the actual build and listen on a phone.

Follow [ADR 0003](../adr/0003-licensed-music.md). Accept a recording whose source page identifies CC0 or CC BY 4.0. During implementation, record each title, creator, source page and licence in `Assets/Audio/Music/LICENSES.md`. For CC BY recordings, supply the required attribution, source and licence links, and any edits in credits accessible in the offline game. Preserve the licensed assets when regenerating project audio.

### C3. Baluan Elementary School gate on the Main Menu

**Menu and Art only.** Draw the dark horizontal gate near the centre. Set the pale “Baluan Elementary School” sign on the red wall to its right, with blue trim, palms and sky. Keep the title and buttons left, the Student low right, and the sign above the greeting. Check with a long Display Name and both Student appearances. Use the photographic details specified in C15.

### C4. Backgrounds for selection screens

**Menu and Art only.** Episode selection uses a neutral school noticeboard with the existing emblems and locks. On Difficulty selection, the Episode background shows a school drill ground for Earthquake and a fire station forecourt with a parked engine for Fire. On Level selection, the Difficulty background shows a classroom with green chairs for Earthquake Easy, a schoolyard path for Normal and a covered court for Hard. Use a simple station noticeboard for empty Fire Level selection. Draw Fire's Difficulty variations when its Levels become playable. Check each screen on entry, Back and return from a Level.

### C5. Choice counts and the Action Bar

**Content, Gameplay rule, Act-out, Menu and Art only for Tutorial Page 3.** Enforce exactly two Choices on Easy, three on Normal and four on Hard across every Episode. Earthquake goes from 59 to 49 Choices across 15 Levels. Keep the approved Scenario order and one Safe Choice. The table gives the final target. C12 and C13 introduce their replacement Choices in the shared-home change. See [ADR 0002](../adr/0002-choice-counts-per-difficulty.md).

| Level | Choice IDs in Action Bar order |
|---|---|
| Easy 1 | `run`, `desk` (Safe Choice) |
| Easy 2 | `cover` (Safe Choice), `plants` |
| Easy 3 | `stay`, `walk` (Safe Choice) |
| Normal 1 | `picture`, `low_shelf` (Safe Choice), `books_top` |
| Normal 2 | `hidden`, `go_bag` (Safe Choice), `toys` |
| Normal 3 | `glass`, `sofa`, `table` (Safe Choice) |
| Normal 4 | `tree`, `field` (Safe Choice), `building` |
| Normal 5 | `watch`, `high_ground` (Safe Choice), `wait` |
| Hard 1 | `own_way`, `meeting_place` (Safe Choice), `meet_home`, `phone_numbers` |
| Hard 2 | `route_post`, `route_tree`, `route_field` (Safe Choice), `route_later` |
| Hard 3 | `run_exit`, `elevator`, `shop_window`, `table` (Safe Choice) |
| Hard 4 | `stairs` (Safe Choice), `elevator`, `run_push`, `mall_wall` |
| Hard 5 | `pull_up`, `call_adult` (Safe Choice), `wait_for_mom` (replaces `fix_pipe` under C12), `leave` |
| Hard 6 | `no_go_bag`, `toys`, `push_past_mom` (replaces `no_note` under C13), `go_bag` (Safe Choice) |
| Hard 7 | `go_home`, `radio` (Safe Choice), `rumor`, `phone` |

Center each two- or three-button Action Bar in one row. Keep each button 430 × 124 UI units with 20 units between buttons. Tutorial Page 3 shows “Run outside” followed by “Duck under the desk” in both frames. Retain the reviewed Hints and the Feedback findings for retained Choices except for the explicit C12 and C13 changes. Remove retired Choice bindings, Feedback and unique Act-outs. Check Scenario, builder, tutorial, capture and generator references before removing unused v2 art or editable sources. Preserve assets still used elsewhere.

### C6. Shorter walk in Earthquake Normal 4

**Act-out.** The current Student walks 192 art pixels in 2.4 seconds. Move the starting point nearer the existing open grass at (110, 230). Target about one second at the existing speed of 80 art pixels per second. End with the crouch and Cover. Select the exact start in the schoolyard preview. Check both retained Unsafe approaches, return to the new start on Try Again, and visibility above the Action Bar. The Safe Feedback retains EQ-80 and EQ-81.

### C7. Reach-up hands in Earthquake Normal 1

**Art only.** Redraw both 32 × 50 reach-up frames for each Student. Bend the far elbow, separate the forearms and align the near wrist with the palm. Hold the picture over the bed in contact with the hand throughout the reach. Check the opposite facing direction with the books at the cabinet. Preserve the two 300 ms frames, prop contact points and Hard 1's separate card-holding art. Review all four frames at native size and enlarged, then watch the peak reach in motion.

### C8. Pause screen

**Menu, Content and Gameplay rule.** Under “Paused,” replace “Stars you earned are kept.” with the actual Episode, Difficulty and Level, for example “Earthquake - Hard 4”. Use four labeled buttons in two rows.

| Row | Left | Right |
|---|---|---|
| Top | Resume uses the play triangle and continues the current Attempt. | Restart uses the replay arrow and begins a fresh Attempt immediately, with no confirmation or Star penalty. |
| Bottom | Levels uses the grid icon and opens Level selection for the current Episode and Difficulty. | Main Menu uses the home icon and returns to the title screen. |

Preserve previously saved Stars. Check input, pause and resume timing, Android Back, and both screen sizes over a playable Student scene.

### C9. Schoolyard landmarks in Normal 4 and Hard 2

**Art only.** Give the tree a recognizable trunk and branches. Make the electric post's crossarm and wires visible. Separate the path and Evacuation Area sign from the grass. Review still art in Hard 2 and swaying frames in Normal 4. The tree and post must remain individually recognizable when the Action Bar is open. Use C15 for the school facade and walkway.

### C10. Teacher and Student route in Hard 2

**Act-out and Art.** The current diagonal turn crosses the school wall near (151, 214). Have the Teacher step visibly out of the doorway and lead along a continuous foreground path. Leave space between the characters. Turn toward the Evacuation Area sign only after both have cleared the left corner. Select the bends in the preview, then check intermediate positions and the whole walk in motion. Retain the Safe route findings EQ-54 and EQ-56.

### C11. Outdoor assembly point in Hard 4

**Art only.** Define the mall exterior corner, add a low curb at the far edge of the lot and strengthen the painted parking lines. Keep open sky above the assembly sign. The Safe ending at (430, 230) must read as open parking. The Unsafe wall stop at (357, 230) must read as beside the mall. Check all four Choices, the crowd and sign, and the return up the stairs. Preserve the existing findings for the stairs (EQ-92, EQ-36, EQ-38) and wall (EQ-94, EQ-81).

### C12. Help for Mom in Hard 5

**Art, Act-out and Content.** Remove the pipe, puddle and animated drip from the shared Hard 5 and 6 room. The former utility Choice illustrated EQ-91 and EQ-72. Replace it in the third position with “Wait for Mom to get up.” (EQ-90). The Student pauses indoors on clear floor near Mom, hands down. Feedback reads “Keep Mom still and get help.” (EQ-90).

For “Call a grown-up for help” (EQ-90, EQ-72), crouch near Mom, say “Stay still, Mom.” (EQ-90), then walk to the open threshold near (56, 212). Face outside and call “Help!” Dad enters and kneels beside Mom. Check the two existing speech blips, Dad's approach, and the separation between this Safe doorway ending and each Unsafe indoor stop.

### C13. Walk around the table in Hard 6

**Art, Act-out and Content.** Show the written family note from the start. For “Grab the go bag and walk out” (EQ-93, EQ-92), take the foreground aisle past the table, collect the Go Bag near (106, 225) and leave with Mom near (56, 206). Replace the Safe Feedback with “Your family left a note. Bring your go bag and walk out.” (EQ-93, EQ-92).

Replace “Leave without a note” with “Push past Mom at the door.” (EQ-92). Stop indoors before touching Mom or reaching the doorway. Feedback reads “Walk out. Do not run or push.” (EQ-92). Check the Student's feet and the carried bag through the entire walk past the right table leg. Check Mom and the bag at the threshold and restore the written note, bag and cast on Try Again.

### C14. Family evacuation centre in Hard 7

**Art and Act-out layout.** Use the existing `evacuation_area` Setting to depict a later gathering at a fictional designated centre. Draw an open-sided basketball court with a metal roof, pale columns, a hoop, court lines and the label “EVACUATION CENTER”. Include two tents and four additional background figures, including two children. Place an adult and a child at one tent opening. Leave the foreground clear for the Student, Mom, their mat and the radio. Keep the neighbours and a route toward home visible through the open side.

Preserve all four Choices. The Safe Choice still involves sitting with Mom and listening to trusted news (EQ-95, EQ-94). Keep the home, rumour and phone stops distinct. EQ-54 supports the designated area. The reference photograph supplies architectural details. It does not establish the real court's designation. Hard 2 continues to depict the immediate outdoor assembly. Check the radio, paths, signs and Reactions during play.

### C15. School photo references

**Art only.** Apply the fourteen references to the approved menu, schoolyard and court artwork. Use the red wall, blue trim, gate, pale sign and palms for the Main Menu. Use the separate green sign only as an optional planting or colour reference. Use green slatted chairs, a dark board, cream walls and green window frames for the Easy classroom menu illustration. Use green doors, barred windows and a shaded concrete walkway in the revised schoolyard. Use the rust-red roof, exposed trusses and pale columns in the Hard menu and Hard 7 court.

The source inventory is `D:/GameDEV/earthquake-changes-scratch/photo-facts.md`. Keep the photographs in scratch. Draw original characters and omit photographed people, vehicles, seals, map interface elements and fine print. Compare each affected art preview with its named references and its gameplay or menu overlays.

## Data and code

Validate counts in `ScenarioRules` against the Level's Difficulty. Update the Earthquake Scenario assets, Setting action bindings and builder together so the action order and runtime buttons agree. Put learner-facing text in `UIStrings` or the Scenario files. Use the pixel font's supported characters. Generate scenes through the project builders. Menu navigation must retain the selected Episode and Difficulty on return. Select the completion music on the Level Completed panel transition, since the active Unity scene remains the Level. Handle the next scene and the next Attempt without leaving the completion cue playing.

## Tests

The following are required test cases for implementation. Existing suite names identify where to extend coverage.

| Change | New or changed checks |
|---|---|
| C1 | Extend EditMode `LevelCompletionTests` and navigation coverage for Hard 7 early, last outstanding Hard Level in any order, the Hard 7 replay, and the final Episode without a successor. In PlayMode check button visibility and placement, Fire opening once, the empty Fire destination and return navigation. |
| C2 | Extend `LevelFlowPlayTests` music transitions and existing audio preference tests. Cover menu to Level, shaking and recovery, completion once, idle on the result screen, Replay and return to menu. Check the track for every Episode and Difficulty, the two menu tracks taking turns, source formats, measured loudness and peaks, attribution records, required in-game credits and final APK bytes. |
| C3–C4 | Extend `MenuNavigatorTests` and `MenuFlowPlayTests` for selection, Back and re-entry. Assert the correct background for each supported screen, Episode and Difficulty, including empty Fire. Capture the gate with a long greeting and the selected Student, then each distinct selection state at both sizes. |
| C5 | Update `ScenarioTests`, `TestScenarios`, `AttemptTests` and `ScenarioContentTests`. Accept exactly 2/3/4 for the matching Difficulty, test incorrect counts and zero or multiple Safe Choices, and include future Episodes through valid synthetic Scenarios. Verify every authored ID, order, Hint, icon, Act-out and finding. In PlayMode check button count, centered bounds, selection, Tried state, Try Again, Hint, Stars and progression. Move tests needing multiple Unsafe Choices to a suitable Difficulty. Capture both frames of Tutorial Page 3. Replace removed screenshot actions in Easy 1–3 and Normal 1, 3 and 5 with retained Choices. |
| C6, C9, C10 | Extend `ScenarioContentTests` and `LevelFlowPlayTests` for Normal 4 and Hard 2. Measure the Safe walk from the approved start, verify Cover and Try Again, and verify that the landmarks are still or sway in the appropriate Phase. Sample the Teacher and Student along the route, including the corner, as well as their endpoints. Review both Levels in motion. |
| C7 | Check both reach-up frames for both Students, prop contact in each facing direction, timing and reset in Normal 1. Capture the picture and books at peak reach. Compare the Hard 1 card pose with its baseline. |
| C8 | Extend `Pause_ResumeRestartAndQuit_KeepSavedProgress` and the Student freeze test. Exercise each destination, Android Back, the dynamic label, Resume within the same Attempt, restart and saved Stars. Check the four hit areas and button order in the Student-flow capture. |
| C11 | Use the Hard 4 start, Safe completion and Unsafe stop/reset tests in `LevelFlowPlayTests`. Review screenshots for the stairs, wall stop, parking ending, exit frame and crowd. Verify the return route in motion and Hard 3 image equality. |
| C12–C13 | Update Hard 5 and Hard 6 rows in `ScenarioContentTests` and their start, Safe and Unsafe flows in `LevelFlowPlayTests`. Check each replacement Choice, Feedback and finding, the missing pipe visuals, Dad's arrival, speech, note visible from the start, foreground crossing, carried bag, threshold, no contact on the push stop and Try Again. Capture the doorway call and the table crossing in addition to final poses. |
| C14 | Extend Hard 7 start, Safe and Unsafe tests for mat, radio, neighbours, route home, all four stops or endings, Reactions and reset. Review the designated centre sign, tents, children and phone-screen clarity through the Act-outs. |
| C15 | Compare the approved illustrations against the reference inventory in each affected art review. Check recognizable background details at native scale and compare untouched Levels with baseline PNGs. |

## Researcher checks and acceptance

1. Build each implementation change test-first. The red test must reach a failing assertion. Render art to scratch and obtain researcher approval of the preview before installing assets.
2. Close the Unity Editor before batch runs. Use the redesign checkout and Unity 6000.6.9f1. Run `BuildAllBatch` before EditMode, PlayMode and screenshots. Require a successful build and zero failed tests. Store evidence outside the repository root.
3. Review changed screens at both resolutions. Compare screenshots of every untouched Level byte for byte with its baseline. Play through the affected movements. Static ending shots alone cannot establish that an actor clears a corner, furniture or a doorway.
4. Review music and legibility on Android. The researcher already reported smooth gameplay and screen fit for the ticket 14 APK. That report applies to the earlier build. Confirm the changes on their own build.

## Not in scope

New playable Episodes, a fresh scoring system, a redesign of playable classrooms or the corridor, and a redraw of the separate Hard 1 hand pose are outside this release. The referenced court's official shelter status is outside the safety content. The original blank item numbered 13 introduced no additional change.

## Open questions

The interview resolved all design questions. The ten recordings were chosen and downloaded on 2026-11-26. The schoolyard preview will establish the precise Normal 4 start and Hard 2 bends. Readability of the 50-pixel Student on a six-inch phone remains unverified.
