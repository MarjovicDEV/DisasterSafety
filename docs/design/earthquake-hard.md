# Earthquake / Hard (Levels 1 to 7): agreed design

Settled in the design interview on 2026-11-17 (the user accepted every recommendation, asked for the best option for the game, and asked for clean, wonderful art). Terms follow CONTEXT.md. Safety content traces to docs/research/earthquake-safety.md (finding IDs EQ-xx). docs/design/first-playable-level.md, docs/design/earthquake-easy-2-3.md and docs/design/earthquake-normal.md still hold unless this file changes them.

Claude makes the design, the art, the Scenario files and the spec. ChatGPT/Codex writes the C# from the spec. A Claude code review of that branch against main is the planned check afterward.

## Decisions

- Q1: Hard uses the same Level kind as Easy and Normal: one Scenario, 4 Choices, exactly one Safe Choice, a Hint, Stars for fewer tries. Hard is harder because the Unsafe Choices are half-right: each sounds sensible but misses one part of the safe action. No new mechanic and no Core rule change.
- Q2: Hard follows the Learner from planning to meeting the family, in order before, during, after:
  1. Home, before: make the family plan.
  2. School grounds, before: learn the safe evacuation route.
  3. Mall, during: shaking at the food court.
  4. Mall, after: leaving the mall.
  5. Home after the shaking: someone in the family is hurt.
  6. Home after the shaking: leave the house with the go bag.
  7. Evacuation area, after: the family is together; wait for trusted news.

  Hard has one "during" Level. Hard 7 is the last Level of the Earthquake Episode.
- Q3: Finishing Hard 7 completes the Episode and opens Fire (existing `LevelCompletion` rule). It reuses the existing ribbon "Fire is open!", the jingle, and Bantay's `BantayOpenedSoon` line ("Fire is open! New levels are coming soon."), as Normal 5 does for Hard. Next stays hidden. The Safety Reminder screen named in `LevelCompletion` is a later milestone with its own design.
- Q4: Hard uses existing findings only. EQ-96 to EQ-99 stay free; the two-digit source rule in `ScenarioRules` is unchanged.
- Q5: Same split as Normal (see the top of this file).
- Q6 and Q14: Settings.

  | Setting | Status | Used by | Moving parts |
  |---|---|---|---|
  | `home` | reused | Hard 1 (before) | still: a "before" Scenario does not shake |
  | `schoolyard` | reused | Hard 2 (before) | still |
  | `mall_a` | new art | Hard 3 (during) | can shake; wobblers: a hanging shop sign and a glass shop display |
  | `mall_b` | new art | Hard 4 (after) | still; shows the mall exit, the stairs and the open parking area |
  | `home_after` | new art | Hard 5 and Hard 6 (after) | still; the living room after the shaking, with fallen things |
  | `evacuation_area` | new art | Hard 7 (after) | still; an open field with families, away from buildings, trees and posts |

  `mall_a` is the only new Setting that can shake.
- Q7 and Q12: Vehicle and slope Levels were dropped (the user replaced them with the malls). EQ-83 (vehicles) is not used in the Episode. EQ-82 (steep slopes) is kept for the Landslide Episode.
- Q10: The malls are drawn as local-looking General Santos City malls inspired by KCC Mall of Gensan (`mall_a`) and SM City General Santos (`mall_b`). The art uses made-up store signs and no real logos, and no learner text names a mall. This keeps real trademarks out of the game.
- Q13: No escalators. No primary source covers them; the elevator findings (EQ-78, EQ-92) cover "do not take the machine".
- Q9: No new sound. Reuse the shake rumble and the jingles.
- Q16: Every Choice gets a new picture drawn for its Hard scene (28 pictures). Normal pictures are not reused, because they show other Settings.
- Q17: Hard 5 names no family member: "Someone in your family" and "them", so it fits every Learner. The picture shows a family member sitting with a hurt leg.
- Art direction: clean, bright, friendly pixel art in the existing 480x270 style and palette, "fun, never scary" (first-playable-level.md Q11). No blood, no crying faces, no collapsed buildings. Damage in `home_after` is shown with fallen books, a tipped chair and a cracked picture frame only. Keep the space where the Choices panel, the Goal banner, Bantay and the Hint sit free of important objects, as in docs/design/earthquake-normal.md "Layout follow-up".

## Hard 1: Make a family plan (home, before)

| Part | Text | Source |
|---|---|---|
| Goal | Make a family plan for earthquakes. | EQ-64 |
| Bantay intro | Your family is making an earthquake plan. Join in! | story line, no safety claim |
| Hint | Where will your family meet after it shakes? | EQ-64 |
| Safe Choice: Agree on a meeting place (`choice_meeting_place`) | Good plan. Also remember one family phone number. | EQ-64, EQ-67 |
| Unsafe Choice: Each of us finds our own way (`choice_own_way`, heading "Not ready yet") | Agree on one place to meet. Then you can find each other. | EQ-64, EQ-65 |
| Unsafe Choice: Plan to meet back at home (`choice_meet_home`) | Your house may be damaged. Meet at an open place. | EQ-94, EQ-55 |
| Unsafe Choice: Keep numbers only in the phone (`choice_phone_numbers`, heading "Not ready yet") | Phones can be lost. Remember or write down a family number. | EQ-67 |
| Level Completed lesson | Plan with your family. Agree on a meeting place and know a family number. | EQ-64, EQ-67 |

## Hard 2: Learn the safe route (schoolyard, before)

| Part | Text | Source |
|---|---|---|
| Goal | Know your way out of school. | EQ-53, EQ-54 |
| Bantay intro | Your class is learning the evacuation route today. | story line |
| Hint | Which way has nothing that can fall? | EQ-56 |
| Safe Choice: Take the route across the field (`choice_route_field`) | This route goes to the evacuation area. Nothing can fall on it. | EQ-54, EQ-56 |
| Unsafe Choice: Take the path by the post (`choice_route_post`) | Posts can fall when it shakes. Pick a route away from them. | EQ-56, EQ-81 |
| Unsafe Choice: Walk the path under the tree (`choice_route_tree`) | Trees can fall too. Choose a route in the open. | EQ-81, EQ-56 |
| Unsafe Choice: Find the way when it happens (`choice_route_later`, heading "Not ready yet") | Learn the route now, in drills. Then you will know it. | EQ-53, EQ-71 |
| Level Completed lesson | Know your school exits. Learn the safe route to the evacuation area. | EQ-53, EQ-54 |

## Hard 3: Shaking at the mall (mall_a, during)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe when it shakes at the mall. | EQ-16 |
| Bantay intro | You are at the mall food court. The ground is shaking! | story line |
| Hint | Is there a strong table near you? | EQ-75 |
| Safe Choice: Duck under the food court table (`choice_food_court_table`) | Duck, Cover, and Hold. Stay until the shaking stops. | EQ-74, EQ-75, EQ-13 |
| Unsafe Choice: Run to the mall exit (`choice_run_exit`, heading "Not safe yet") | Do not run while it shakes. Take cover where you are. | EQ-77, EQ-16, EQ-74 |
| Unsafe Choice: Ride the elevator down (`choice_elevator_during`) | Never use the elevator in an earthquake. Take cover first. | EQ-78 |
| Unsafe Choice: Stand by the glass shop window (`choice_shop_window`) | Glass can break and fall. Stay away from shop windows. | EQ-14, EQ-76 |
| Level Completed lesson | In the mall, Duck, Cover, and Hold. Stay away from glass and elevators. | EQ-74, EQ-76, EQ-78 |

## Hard 4: Leaving the mall (mall_b, after)

| Part | Text | Source |
|---|---|---|
| Goal | Leave the mall safely after the shaking. | EQ-92 |
| Bantay intro | The shaking stopped. Everyone is leaving the mall. | story line |
| Hint | What is the safe way down after an earthquake? | EQ-92 |
| Safe Choice: Walk down the stairs and out (`choice_mall_stairs`) | Walk, do not run. Go to the open parking area. | EQ-92, EQ-36, EQ-38 |
| Unsafe Choice: Take the elevator down (`choice_elevator_after`) | Do not use elevators after an earthquake. Use the stairs. | EQ-92 |
| Unsafe Choice: Run and push to get out (`choice_run_push`, heading "Not safe yet") | Walk fast, but do not run or push. | EQ-92, EQ-36 |
| Unsafe Choice: Wait by the mall wall outside (`choice_mall_wall`, heading "Not safe yet") | Things can fall from the building. Move to the open area. | EQ-94, EQ-81 |
| Level Completed lesson | After the shaking, use the stairs and walk out. Wait in an open area. | EQ-92, EQ-38 |

## Hard 5: Someone is hurt (home_after, after)

| Part | Text | Source |
|---|---|---|
| Goal | Help when someone is hurt after an earthquake. | EQ-90 |
| Bantay intro | The shaking stopped. Someone in your family hurt their leg. | story line |
| Hint | Who can help them the right way? | EQ-90 |
| Safe Choice: Call a grown-up for help (`choice_call_adult`) | Good. Keep them still and get help from an adult. | EQ-90, EQ-72 |
| Unsafe Choice: Pull them up to walk (`choice_pull_up`, heading "Not safe yet") | Do not move them. Keep them still and get help. | EQ-90 |
| Unsafe Choice: Leave them and go outside (`choice_leave_hurt`) | Stay and call for help. They need an adult. | EQ-90 |
| Unsafe Choice: Fix the broken pipe yourself (`choice_fix_pipe`) | Adults check water and electric lines. Help them first. | EQ-91, EQ-72 |
| Level Completed lesson | After the shaking, check yourself and others. Get help for anyone who is hurt. | EQ-90 |

## Hard 6: Leave the house (home_after, after)

| Part | Text | Source |
|---|---|---|
| Goal | Leave home safely after the earthquake. | EQ-92 |
| Bantay intro | Your family must leave the house. What do you bring? | story line |
| Hint | What did your family pack for 3 days? | EQ-68, EQ-93 |
| Safe Choice: Grab the go bag and walk out (`choice_go_bag_out`) | Bring the go bag. Leave a note saying where you went. | EQ-93 |
| Unsafe Choice: Run back in for your toys (`choice_back_for_toys`, heading "Not safe yet") | Do not go back in. Aftershocks can come. Walk out now. | EQ-43, EQ-42, EQ-92 |
| Unsafe Choice: Leave without the go bag (`choice_no_go_bag`, heading "Not ready yet") | Your go bag has what you need. Bring it with you. | EQ-93, EQ-68 |
| Unsafe Choice: Leave without a note (`choice_no_note`, heading "Not ready yet") | Leave a message so others know where you went. | EQ-93 |
| Level Completed lesson | Bring your go bag and walk out. Expect aftershocks. | EQ-93, EQ-92, EQ-42 |

## Hard 7: At the evacuation area (evacuation_area, after)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe at the evacuation area. | EQ-54 |
| Bantay intro | You are at the evacuation area. Your family is here! | story line |
| Hint | Where can you get true news? | EQ-95 |
| Safe Choice: Listen to the radio for news (`choice_radio`) | Listen to official news. Wait until it is safe. | EQ-95, EQ-94 |
| Unsafe Choice: Go home now to get things (`choice_go_home`, heading "Not safe yet") | Do not go back into a damaged house. Wait until it is safe. | EQ-94, EQ-43 |
| Unsafe Choice: Share news you heard from others (`choice_rumor`) | Get news from trusted sources. Do not spread rumors. | EQ-95 |
| Unsafe Choice: Call friends on the phone all day (`choice_phone_all_day`, heading "Not ready yet") | Use the phone only when you need it. | EQ-95 |
| Level Completed lesson | Stay with your family. Listen to trusted news and do not go back inside. | EQ-64, EQ-95, EQ-94 |

Completing Hard 7 for the first time shows the "Fire is open!" ribbon and Bantay's `BantayOpenedSoon` line (Q3).

## Art to make

All under `Assets/Art/Level`, made by Claude from `ArtSource/`, 480x270 for backgrounds, same size and style as the Normal Choice pictures.

- Backgrounds: `bg_mall_a.png`, `bg_mall_b.png`, `bg_home_after.png`, `bg_evacuation_area.png`.
- Wobblers for `mall_a`: `mall_sign_0.png`, `mall_sign_1.png`, `shop_display_0.png`, `shop_display_1.png`.
- Choice pictures (28): `choice_meeting_place`, `choice_own_way`, `choice_meet_home`, `choice_phone_numbers`, `choice_route_field`, `choice_route_post`, `choice_route_tree`, `choice_route_later`, `choice_food_court_table`, `choice_run_exit`, `choice_elevator_during`, `choice_shop_window`, `choice_mall_stairs`, `choice_elevator_after`, `choice_run_push`, `choice_mall_wall`, `choice_call_adult`, `choice_pull_up`, `choice_leave_hurt`, `choice_fix_pipe`, `choice_go_bag_out`, `choice_back_for_toys`, `choice_no_go_bag`, `choice_no_note`, `choice_radio`, `choice_go_home`, `choice_rumor`, `choice_phone_all_day`.

## Checks

- Every line keeps the content rules the EditMode content tests enforce: at most 12 words per sentence, pixel-font characters only, no "drop", every safety line cites a known finding.
- Choice labels stay short enough to fit 2 lines on the card, and lessons stay short enough to clear the Level Completed buttons (see the Normal "Layout follow-up").
