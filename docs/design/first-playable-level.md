# First Playable Level: agreed design

Settled in the design interview on 2026-11-16. Terms follow CONTEXT.md. Safety content traces to docs/research/earthquake-safety.md (finding IDs EQ-xx).

## Repository housekeeping
- Q0: Discard the uncommitted Boot.unity and MainMenu.unity changes (fileID renumbering only) on the new branch; ProjectBuilder regenerates the scenes.
- main is level with origin/main at 3095e51 (not 8 ahead as the prompt said).

## Gameplay rules
- Q1: One Scenario per Level with 3 or 4 Choices, exactly one Safe Choice. A Level is completed only by picking the Safe Choice. Hazard-identification activities (draft line 15) are a possible later Level kind; not modelled now.
- Q3: After an Unsafe Choice, show Feedback, then "Try Again" returns to the same Choices in the same Attempt. Tried Unsafe Choices stay visible with a soft grey "tried" mark (never a red X).
- Q2: Stars start at 3; each Unsafe Choice costs 1; using the Hint costs 1; minimum 1 on completion.
- Q4: No locks inside a Difficulty. Every Level of an Open Difficulty can be started if it is Playable.
- Q5: Replay keeps the best Stars (current ProgressState behaviour).
- Q9: Closing the app mid-Attempt saves nothing.
- Q12: SaveData unchanged; saveVersion stays 1; no migration.

## Flow
- Q6: Level Completed buttons: Replay, Next (only when the next Level is Open and Playable; hidden this milestone), Levels (Level Selection for the same Episode and Difficulty), Main Menu (through the Continue check). Draft Part 4 says "Episode Selection"; record in the workspace note that the agreed destination is Level Selection. Do not edit the chapter text.
- Pause: Resume, Restart, Quit to Menu. Android Back in a Level opens Pause; Back on Pause resumes.
- Save completion and Stars before Level Completed is shown.

## Content and structure
- Q7: Scenarios are JSON TextAssets, one per Level, e.g. Resources/Scenarios/Earthquake/Easy/1.json, parsed in Core with JsonUtility. Every Choice, Feedback line, Hint, and explanation carries its source IDs. A Filipino version can be a second file later.
- Q8: One reusable Level scene loads any Scenario by LevelId. Each Level is still shown as one scene, which satisfies the vault rule "one scenario scene per Level".
- Q10: Content is Earthquake / Easy / Level 1 only. The Easy-complete-opens-Normal rule is built and unit-tested but not playable. No Safety Reminder this milestone.

## Feel (make it fun, never scary)
- Q11: Shake the scene art only (never buttons or text), about 2 art pixels (8 UI units), about 2 s at Level start, then a faint wobble until a Choice is made. Soft, low rumble at quiet volume on the SFX setting. No flashing, no screams. Fan and cabinet wobble; nothing falls.
- Q14: Bantay cheers (jump and wag) on Level Completed with the Display Name ("You did it, Ana!") and encourages after an Unsafe Choice ("Let's think again together!"). Bantay never says "wrong" and never mentions Stars. The Feedback card does the explaining.
- Q15: Big picture Choice cards with an icon, a short label, and a press bounce. Safe Feedback: a green "Safe!" card with a sparkle and a small picture of the safe action. Unsafe Feedback: a warm orange "Not safe" card (never red), one or two short sentences. Level Completed: Stars pop in one at a time with rising chimes, then a confetti burst, with the Display Name in the headline. No timer. At most 2 short sentences per screen. Hint shown in Bantay's speech bubble. Bantay speaks with speech bubbles and a short "bark-blip" sound, no synthesised voice.

## Safety wording
- Q16: Use "Duck, Cover, and Hold" everywhere (EQ-01 to EQ-04, EQ-06, EQ-08, EQ-09). Change the Main Menu Tip and Tutorial Page 4 from "DROP, COVER, and HOLD ON" to "DUCK, COVER, and HOLD!".
- Tutorial Page "Earn Stars" caption becomes: "Pick the safe choice first, with no Hint, for 3 stars!" (code review: the shorter approved caption left out the Hint cost).
- Q18: Bantay Main Menu Tips keep only sourced lines: the earthquake Tip, "Know your school's evacuation area." (EQ-37), "Listen to your teacher during drills." (EQ-35). Hide the six fire, smoke, typhoon, flood, landslide, and go-bag Tips until each Episode is researched.

## Level 1 script (Earthquake / Easy / 1, "during" phase, classroom)

| Part | Text | Source |
|---|---|---|
| Goal | Stay safe when the ground shakes. | EQ-28 |
| Bantay | The ground is shaking! What will you do? | story line, no safety claim |
| Hint | Find something strong to hide under. | EQ-10 |
| Safe Choice: Duck under the desk | Safe! Duck under your desk. Hold on to it. Stay there until the shaking stops. | EQ-10, EQ-11, EQ-13 |
| Unsafe Choice: Run outside | (heading "Not safe yet") Stay inside while it shakes. When it stops, walk out with your teacher. | EQ-16, EQ-19, EQ-20, EQ-35 |
| Unsafe Choice: Go to the window | Not safe. Window glass can break. Stay away from windows. | EQ-14, EQ-21, EQ-24 |
| Unsafe Choice: Stand by the tall cabinet | Not safe. Cabinets and shelves can fall. Stay away from heavy things. | EQ-14, EQ-31 |
| Level Completed explanation | When the ground shakes: Duck, Cover, and Hold! Stay put until the shaking stops. | EQ-09, EQ-13 |

- "Cry and stay still" was dropped: no primary source names crying or freezing, and it risks shaming.
- No reason is given for why running is unsafe; no primary source states one.
- Art: classroom with desks, window, tall cabinet, ceiling fan, door, teacher's table. The safe picture shows the Learner under the desk holding the desk leg (PHIVOLCS pictures; text says only "hold on to it").

## Not verified
- OCD and NDRRMC pages returned HTTP 403; no OCD or NDRRMC line is used as a primary source.
- No Android device test, motion review, or listening check has been made.
