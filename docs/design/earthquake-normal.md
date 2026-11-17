# Earthquake / Normal (Levels 1 to 5): agreed design

Settled in the design interview on 2026-11-17 (the user accepted every recommendation and asked for the best option for the game). Terms follow CONTEXT.md. Safety content traces to docs/research/earthquake-safety.md (finding IDs EQ-xx). docs/design/first-playable-level.md and docs/design/earthquake-easy-2-3.md still hold unless this file changes them.

Claude made the design, the art, the Scenario files and the handoff. ChatGPT/Codex writes the C# from docs/specs/earthquake-normal.md. A Claude code review of that branch against main is the planned check afterward.

## Decisions

- Q1: Normal follows the Learner away from the classroom, in order before, during, after:
  1. Bedroom, before: make your room safer.
  2. Living room, before: get the go bag ready.
  3. Living room, during: shaking at home.
  4. School grounds, during: shaking at recess, outside.
  5. Beach, after: a tsunami sign.

  Vehicles, slopes, checking the house after the shaking and meeting the family are left for Hard. A "before" Scenario starts still, like an "after" one: no shake and no rumble.
- Q2: There are four new Settings, each with new art:

  | Setting | Phase use | Moving parts |
  |---|---|---|
  | `bedroom` | before (Normal 1) | none; still |
  | `home` | before (Normal 2) and during (Normal 3) | wobblers: the hanging lamp and the glass cabinet |
  | `schoolyard` | during (Normal 4) | wobblers: the big tree and the electric post |
  | `beach` | after (Normal 5) | a gentle wave loop, always animating; it is not a wobbler |

  The schoolyard is new art, not the Main Menu courtyard, because the Choices need an open field, a big tree, an electric post and the school building in view. `home` and `schoolyard` can shake; `bedroom` and `beach` cannot.
- Q3 to Q7: the five scripts below, every safety line with its finding.
- Q4: In Normal 2 the Unsafe Choices are about when and where the go bag is kept ("accessible at all times", "easy to find", EQ-68), plus "only snacks and toys", which misses the 3-day rule of EQ-68 and EQ-69. Two of them carry the gentle heading "Not ready yet".
- Q5: "Lie down beside the sofa" is how PHIVOLCS's rejection of the "triangle of life" (EQ-79) appears in the game.
- Q6: "Branches can fall" is left out: no primary source gives that reason.
- Q7: Baluan Elementary School is in General Santos City, on Sarangani Bay, and the research cites the 2026 offshore Sarangani earthquake (S5), so the tsunami Level is local. The content test rejects the word "drop", so the sea sign is worded "the sea is going out".
- Q8: No new sound. A wave sound next to a tsunami lesson could feel scary ("fun, never scary", first-playable-level.md Q11). Finishing Normal opens Hard, and the existing "<Difficulty> is open!" ribbon, jingle and Bantay line from Easy show "Hard is open!" with no change.

## Normal 1: Make your room safer (bedroom, before)

| Part | Text | Source |
|---|---|---|
| Goal | Make your room safer before an earthquake. | EQ-52 |
| Bantay intro | Your family is getting ready for earthquakes. Help fix your room! | story line, no safety claim |
| Hint | Heavy things can fall. Where are they safest? | EQ-61 |
| Safe Choice: Put heavy things on a low shelf (`choice_low_shelf`) | Keep heavy things on low shelves, not high ones. | EQ-61 |
| Unsafe Choice: Hang a big picture over the bed (`choice_picture_bed`) | A heavy picture can fall on your bed. Hang it somewhere else. | EQ-62 |
| Unsafe Choice: Move the bed under the window (`choice_bed_window`) | Window glass can break. Keep your bed away from windows. | EQ-62, EQ-14 |
| Unsafe Choice: Put books on top of the tall cabinet (`choice_books_top`) | Things up high can fall when it shakes. Keep them low. | EQ-61, EQ-31 |
| Level Completed lesson | Before an earthquake, make your home safer. Keep heavy things low and beds away from windows. | EQ-61, EQ-62 |

## Normal 2: Get the go bag ready (home, before)

| Part | Text | Source |
|---|---|---|
| Goal | Get your go bag ready. | EQ-68 |
| Bantay intro | Your family is packing a go bag. Let's help! | story line |
| Hint | Think of what you need for 3 days. | EQ-68 |
| Safe Choice: Pack water, food, a flashlight and a whistle (`choice_go_bag`) | These help your family for 3 days. Keep the bag easy to reach. | EQ-68, EQ-69 |
| Unsafe Choice: Pack it later, when it shakes (`choice_pack_later`, heading "Not ready yet") | Get it ready now. Keep it where you can grab it. | EQ-52, EQ-68 |
| Unsafe Choice: Hide it under the boxes (`choice_bag_hidden`) | Keep your go bag in a place that is easy to find. | EQ-68 |
| Unsafe Choice: Pack only snacks and toys (`choice_bag_toys`, heading "Not ready yet") | A go bag needs water, food and a light for 3 days. | EQ-68, EQ-69 |
| Level Completed lesson | Pack a go bag before an earthquake. It should last 3 days and be easy to reach. | EQ-68 |

## Normal 3: Shaking at home (home, during)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe when the ground shakes at home. | EQ-74 |
| Bantay intro | You are in the living room. The ground is shaking! | story line |
| Hint | Is there something strong to hide under? | EQ-75 |
| Safe Choice: Duck under the strong table (`choice_table`) | Duck, Cover, and Hold under the table. Stay until the shaking stops. | EQ-74, EQ-75, EQ-13 |
| Unsafe Choice: Run out the door (`choice_run_home`, heading "Not safe yet") | Do not run while it shakes. Stay inside and take cover. | EQ-77, EQ-78, EQ-74 |
| Unsafe Choice: Stand by the glass cabinet (`choice_glass_cabinet`) | Glass and shelves can fall and break. Stay away from them. | EQ-76 |
| Unsafe Choice: Lie down beside the sofa (`choice_sofa`) | Lying beside furniture is not the safe way. Get under a strong table. | EQ-79, EQ-75 |
| Level Completed lesson | At home, Duck, Cover, and Hold under a strong table. Stay away from glass. | EQ-74, EQ-75, EQ-76 |

## Normal 4: Shaking outside at recess (schoolyard, during)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe when it shakes outside. | EQ-80 |
| Bantay intro | It is recess. You are outside when the ground shakes! | story line |
| Hint | Where is there nothing that can fall on you? | EQ-80, EQ-81 |
| Safe Choice: Go to the open field (`choice_open_field`) | Move to an open area. Stay away from trees, posts and buildings. | EQ-80, EQ-81 |
| Unsafe Choice: Hide under the big tree (`choice_tree`) | Stay away from trees when the ground shakes. | EQ-81 |
| Unsafe Choice: Stand by the electric post (`choice_post`) | Stay away from posts and power lines. | EQ-81 |
| Unsafe Choice: Run back into the building (`choice_building`, heading "Not safe yet") | Stay away from buildings while it shakes. Go to an open area. | EQ-81, EQ-80 |
| Level Completed lesson | Outside? Move to an open area, away from trees, posts and buildings. | EQ-80, EQ-81 |

## Normal 5: At the beach (beach, after)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe near the sea after a strong earthquake. | EQ-84 |
| Bantay intro | You are at the beach. The shaking stopped. The sea is going out! | story line, EQ-85 for the sea going out |
| Hint | The water can come back fast. Where can it not reach? | EQ-86, EQ-87 |
| Safe Choice: Go to high ground right away (`choice_high_ground`) | A strong earthquake near the sea can bring a tsunami. Move to high ground now. | EQ-84, EQ-86 |
| Unsafe Choice: Pick up the fish on the sand (`choice_fish`) | Never go down the beach for fish. Go to high ground. | EQ-88, EQ-86 |
| Unsafe Choice: Stay and watch the waves (`choice_watch_waves`) | Never stay to watch for a tsunami. Go to high ground. | EQ-88 |
| Unsafe Choice: Wait on the beach for news (`choice_wait_beach`) | Do not stay in low places by the sea. A warning may not come in time. | EQ-87 |
| Level Completed lesson | Near the sea, a strong earthquake can bring a tsunami. Go to high ground and stay until the all clear. | EQ-84, EQ-86, EQ-89 |

The Feedback heading is "Safe!" or "Not safe" unless a heading is given, as in Easy.

## Art (made by Claude, committed; never regenerate)

- Backgrounds, 480x270: `Level/bg_bedroom`, `Level/bg_home`, `Level/bg_schoolyard`, `Level/bg_beach`.
- Moving parts, 2 frames each: `Level/lamp` and `Level/glass_cabinet` (home), `Level/tree` and `Level/post` (schoolyard), `Level/waves` (beach).
- 20 Choice pictures, 72x52, named in the scripts above.

Sizes and placements are in docs/specs/earthquake-normal.md.

## Layout follow-up

- Raise the schoolyard field above the Choices at both screen sizes. Keep the schoolyard art aligned to the top and draw the field between the school and the tree, away from Bantay and the Hint. Place the tree at (334, 34) and the post at (390, 14), both at three UI units per art pixel, so their bases also clear the Choices.
- Use the smaller bitmap font and a 384-unit label width when a Choice label cannot fit at the regular size. This keeps the Choice pictures large and gives the longest labels two lines.
- Set the Level Completed lesson line spacing to 1.0. The Normal 5 lesson keeps the regular font and clears the buttons.

- Shorten and lower the Choice row as the bedroom, home and beach screens get wider. At 2400x1080 the row clears the furniture and the beach path. The card pictures are smaller and the label area keeps its original height.

## Not verified

- No Android device test, motion review or listening check has been made for any Level.
- OCD and NDRRMC pages still return HTTP 403; none of their lines are used.
