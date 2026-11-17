# Gameplay redesign: the Student acts it out

Settled in the design interview on 2026-11-17. The user found the gameplay flat and the art weak, especially the characters and the Choice pictures, and asked for a game where the Learner chooses what to do and then watches it happen. The user accepted every recommendation below. Terms follow CONTEXT.md. This file replaces the Choice-card and Bantay parts of docs/design/first-playable-level.md, earthquake-easy-2-3.md, earthquake-normal.md and earthquake-hard.md; their Scenario text, safety lines and EQ sources still hold.

The art direction and the removal of Bantay are recorded in docs/adr/0001-v2-art-and-no-bantay.md.

## Principle: lots of animation

The game is animated throughout. The Student, the scene and the reactions all move, and every Choice the Learner picks is acted out on screen. This needs real animation support, not only the two-frame wobble Settings use today: frame sets per pose and a small animator per character (see "Animation" below).

## The Level flow

1. **Scene.** The Learner's Student stands in the Setting. The Caption at the top shows the Scenario intro and the Goal.
2. **The earthquake** (a "during" Scenario only). The scene shakes for 2 to 3 seconds with the rumble, the moving parts sway, and the Student reacts ("Whoa!"). A "before" or "after" Scenario skips this step and starts still.
3. **The Action Bar** slides up at the bottom while the shaking goes on more gently. It shows the Scenario's 3 or 4 Choices as short commands, each with a 16x16 icon, such as "Duck under the desk" or "Run to the door". There are no Choice pictures.
4. **The Student acts it out.** The Learner taps a Choice and the Student does it in the scene.
   - **Safe Choice:** the full action plays, for example walking to the desk and doing Duck, Cover, and Hold. The shaking stops, the Student says "I'm safe!", then Feedback shows, then Level Completed.
   - **Unsafe Choice:** the Student starts the action, stops, and says "Hmm, wait...". Feedback explains why, and the Learner chooses again in the same Attempt. Nothing falls and nobody gets hurt ("fun, never scary" still holds).
5. **Hint.** The Hint button shows the Hint in the Caption.

Stars, Attempts, Open/Locked and Saved Progress do not change. `DisasterSafety.Core` keeps its rules; only how a Choice is shown and played changes.

## Decisions

| # | Question | Decision |
|---|---|---|
| Q1 | How the Learner chooses | An Action Bar of short commands at the bottom; the scene is where the Choice plays out. |
| Q1a | One Choice or a chain | Easy: one Choice per Level. Normal and Hard: a chain of 2 or 3 steps per Level, scoring decided after the pilot (open question below). |
| Q1b | The earthquake moment | A 2 to 3 second shake first, then the Action Bar slides up while a gentler shake continues. |
| Q1c | "Before" and "after" Levels | The same Action Bar, without the shake. The Student still acts out the Choice. |
| Q2 | The Learner's character | The Learner picks a boy or a girl Student once, with the Display Name. |
| Q3 | What an Unsafe Choice shows | The Student starts, stops, says "Hmm, wait...", then Feedback. A Safe Choice plays in full. |
| Q4 | How the art is made | The v2 style (see ADR 0001). Claude drafts every sprite in code; the user may polish any of them in Aseprite, and a hand-edited `.aseprite` file then becomes the source. |
| Q5 | Scope and order | Pilot Easy 1 (classroom) first, judged on a phone, then roll out to every Level. |
| Q6 | Earthquake Hard | Hard was merged into main before this interview ended, so the rollout covers all 15 Earthquake Levels. |
| Q7 | Who tells the story | The Caption tells the Scenario and shows the Hint. The Student's speech bubble carries short reactions. |
| Q8 | Who reacts to an Unsafe Choice | The Student: "Hmm, wait...". |
| Q9 | Main Menu and How to Play | The Learner's chosen Student replaces Bantay there. |
| Q10 | An Action Bar button | A short command plus a 16x16 icon. |
| Q11 | The Student's size | The 50-pixel Student acts in the scene. The 22-pixel chibi is kept for map-style screens. |
| Q12 | Backgrounds | Redrawn in the v2 style, classroom first, with furniture sized for the 50-pixel Student (a desk about 40 pixels tall). |
| Q13 | Facing | Front view for idle, reactions and Duck, Cover, and Hold; a side view, mirrored for left and right, for walking. |
| Q14 | Stars and Attempts | Unchanged. |
| Q15 | Chain scoring | Decided after the pilot. |
| Q16 | How the pilot is judged | An Android test build (APK) installed on the user's phone. |
| Q17 | Old art | Bantay and the Choice pictures stay in the repo until every Level is moved over, then go in one cleanup. |
| Q18 | Where the work happens | Branch `gameplay-redesign` from main. |

## Characters

- **Student** (boy and girl): the v2 sprites, 32x50 big and 16x22 chibi, in the school uniform: a white polo or blouse with the red school patch, navy pants with a belt or a navy pleated skirt with white socks, and black shoes. The samples are in `StyleSample/` in the source folder (`v2_big_boy`, `v2_big_girl`, `v2_chibi_boy`, `v2_chibi_girl`, `v2_chibi_duck`) with the script `student2.py`.
- **Bantay** is removed from the Levels, the Main Menu and How to Play.

## Animation for the pilot (Easy 1, classroom)

The big Student, boy and girl:

| Pose | Facing | Frames |
|---|---|---|
| Idle (breathing) | front | 2 |
| Surprised ("Whoa!") | front | 2 |
| Walk | side, mirrored | 4 |
| Duck, Cover, and Hold under the desk | front | 2 (hold loop) |
| Start of a run (Unsafe) | side, mirrored | 2 |
| "Hmm, wait..." | front | 2 |
| Safe and happy ("I'm safe!") | front | 2 |

The scene: the classroom redrawn in v2 style, its ceiling fan and cabinet wobble as today, the Student's speech bubble, the Caption banner and the Action Bar with its slide-up.

## Open questions

- Scoring of a 2 or 3 step chain in Normal and Hard (Q15), after the pilot.
- Whether the Student's reactions have sound (a short voice blip), after the pilot.

## Not verified

- No phone test of the new flow yet; the pilot exists to do that.
- The 50-pixel Student's readability on a 6-inch phone is assumed from the 4-units-per-art-pixel scale and still needs checking on the device.
