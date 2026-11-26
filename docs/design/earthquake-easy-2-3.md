# Earthquake / Easy / Levels 2 and 3: agreed design

Settled in the design interview on 2026-11-16 (the user accepted every recommendation). Terms follow CONTEXT.md. Safety content traces to docs/research/earthquake-safety.md (finding IDs EQ-xx). Everything in docs/design/first-playable-level.md still holds unless this file changes it.

Claude made the design, the art, the sounds and the Scenario files. ChatGPT/Codex writes the C# from docs/specs/earthquake-easy-2-3.md. A Claude code review of that branch against main is the planned check afterward.

## Decisions

- Q1: Level 2 happens in a school **corridor** (Setting key `corridor`). It has windows on one side, hanging flower pots, a bulletin board, classroom doors and stairs at the end. There is no desk (EQ-47). The Learner-facing word is "corridor" (Philippine school usage; EQ-30 says "corridors").
- Q2: The Level 2 Safe Choice is "Duck and cover your head". The Feedback names "arms or a book" (EQ-47, EQ-48, EQ-49). The picture shows a child crouched low with both arms over the head.
- Q3: Level 2 has 4 Choices. The Unsafe Choices are run down the stairs, stand by the window (this repeats Level 1 on purpose, to reinforce it) and stand under the hanging plants. The hanging pots are the corridor's wobble part.
- Update 2026-11-26: the hanging pots looked out of place in the corridor and were removed. Easy 2 now has 2 Choices: Duck and cover your head (Safe) and Run to the stairs (Unsafe, EQ-77, EQ-18). The Student runs toward the stairs and stops at the top step.
- Q4: Level 3 happens in the same classroom after the shaking (Setting key `classroom_after`). The door is open and the teacher waves from it. There are 3 Choices. No primary source supports a 4th classroom Choice, for example "get your bag", so there is none. The teacher is a new sprite: a woman teacher, because most Philippine elementary teachers are women.
- Q5: `Scenario.setting` selects the scene art. There are exactly three Settings:

  | Setting | Background | Moving parts | Used by |
  |---|---|---|---|
  | `classroom` | `Level/bg_classroom` | wobble: `Level/fan` (2 frames), `Level/cabinet` (2 frames) | Easy 1 |
  | `corridor` | `Level/bg_corridor` | wobble: `Level/pots` (2 frames), drawn twice | Easy 2 |
  | `classroom_after` | `Level/bg_classroom_after` | idle animation, not a wobble: `Level/teacher` (2 frames, a wave) | Easy 3 |

  An unknown Setting is a content error: ScenarioRules rejects it, so the Level is not a Playable Level.
- Q6: Phase decides shaking. `during` keeps the Level 1 behaviour (shake, rumble, then a faint wobble until a Choice). `after` starts still: no shake, no rumble, and Bantay speaks as soon as the scene opens. Aftershocks are taught in words only (EQ-42), never shown.
- Q7: The "Normal is open!" moment. The Level Completed that first opens Normal (`CompletionResult.Opened` contains `"Earthquake/Normal"`) shows extra things:
  - a gold ribbon under the Stars with the hard-hat icon and the text "Normal is open!";
  - after the cheer, Bantay says "Normal is open! New levels are coming soon.";
  - the new `sfx_opened` jingle, which plays after the Stars and the confetti.

  It appears only on the completion that opens something, never on a replay. Next stays hidden, because Normal 1 is not playable yet. The ribbon text comes from the first entry in `Opened`, as "<Difficulty> is open!" or "<Episode> is open!", so later Episodes reuse it.
- Q8: There is one new sound, `sfx_opened`: a soft rising 4-note jingle, quieter and shorter than `sfx_complete`. Level 2 reuses `sfx_rumble`. Level 3 has no rumble. There are no footsteps and no drill bell.
- Q11: CONTEXT.md gains **Setting**, **Phase** and **Evacuation Area**.
- Level 1's Next button now appears, because Level 2 is a Playable Level. The same goes for Level 2 to Level 3. This is existing behaviour and needs no new rule.

## Level 2 script (Earthquake / Easy / 2, "during", corridor)

| Part | Text | Source |
|---|---|---|
| Goal | Keep your head safe when the ground shakes. | EQ-28, EQ-48 |
| Bantay intro | You are in the corridor. The ground is shaking! | story line, no safety claim |
| Hint | There is no desk here. What can cover your head? | EQ-47, EQ-48 |
| Safe Choice: Duck and cover your head (`choice_cover`) | Duck down low. Cover your head with your arms or a book. | EQ-47, EQ-48, EQ-49 |
| Unsafe Choice: Run down the stairs (`choice_stairs`, heading "Not safe yet") | Stay where you are while it shakes. Walk out when it stops. | EQ-16, EQ-19, EQ-20 |
| Unsafe Choice: Stand by the window (`choice_hall_window`) | Window glass can break. Stay away from windows. | EQ-14, EQ-21, EQ-24 |
| Unsafe Choice: Stand under the hanging plants (`choice_plants`) | Hanging pots can swing and fall. Stay away from them. | EQ-30, EQ-31 |
| Level Completed lesson | No desk? Still Duck, Cover, and Hold. Cover your head until the shaking stops. | EQ-47, EQ-48, EQ-13 |

## Level 3 script (Earthquake / Easy / 3, "after", classroom_after)

| Part | Text | Source |
|---|---|---|
| Goal | Leave the classroom the safe way. | EQ-34 |
| Bantay intro | The shaking stopped. Your teacher is at the door. | story line, no safety claim |
| Hint | Follow your teacher, one step at a time. | EQ-35, EQ-36 |
| Safe Choice: Walk out with your teacher (`choice_walk`) | Walk out with your class. Follow your teacher to the evacuation area. | EQ-34, EQ-35, EQ-37 |
| Unsafe Choice: Run and push to the door (`choice_push`) | Walk. Do not run. Do not push. | EQ-20, EQ-36 |
| Unsafe Choice: Stay under the desk (`choice_stay`, heading "Time to go") | The shaking stopped. Now leave the classroom with your teacher. | EQ-19, EQ-34, EQ-35 |
| Level Completed lesson | When the shaking stops, walk out with your teacher. Aftershocks may come, so stay ready. | EQ-34, EQ-35, EQ-42 |

The Feedback card heading is "Safe!" or "Not safe" unless a heading is given, as in Level 1. "Stay under the desk" is not scolded. Its heading "Time to go" says that Duck, Cover, and Hold was right while the ground shook, and that now it is time to leave.

## New Learner text outside Scenario files

These go in `UIStrings.cs`:

- `Opened(string what)` gives "<what> is open!", for example "Normal is open!".
- `BantayNormalOpen` = "Normal is open! New levels are coming soon."

## Art and sound (made by Claude, committed; never regenerate)

- `Level/bg_corridor.png` (480x270)
- `Level/pots_0.png`, `pots_1.png`
- `Level/bg_classroom_after.png` (480x270), with the fan and cabinet drawn in, still
- `Level/teacher_0.png`, `teacher_1.png`
- Choice pictures, 72x52 each: `choice_cover`, `choice_stairs`, `choice_hall_window`, `choice_plants`, `choice_walk`, `choice_push`, `choice_stay`
- `Assets/Audio/Sfx/sfx_opened.wav`

The exact sizes and placements are in docs/specs/earthquake-easy-2-3.md.

## Not verified

- There has been no Android device test, motion review or listening check.
- OCD and NDRRMC lines are still not used (HTTP 403).
