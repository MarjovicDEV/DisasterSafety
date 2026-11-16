# Disaster Safety Game

An offline, single-player Android game that teaches Grade 3 to Grade 6 learners of Baluan Elementary School what to do before, during, and after a disaster. This glossary fixes the words used in code, tests, the manuscript, and the vault.

## Learning structure

**Episode**:
One disaster topic and all of its content. There are exactly five, always in this order: Earthquake, Fire, Typhoon, Flood, Landslide.
_Avoid_: Chapter, world, stage, module

**Difficulty**:
One of three tiers inside an Episode: Easy (3 Levels), Normal (5 Levels), Hard (7 Levels).
_Avoid_: Mode, tier, grade level

**Level**:
One numbered scenario inside a Difficulty of an Episode. The game has 75 Levels in total.
_Avoid_: Stage, round, mission, question

**Playable Level**:
A Level whose scenario content exists in the build and can be started. A Level shown in Level Selection is not necessarily playable.
_Avoid_: Available level, enabled level

**Completed Level**:
A Level the learner has finished at least once. Starting or quitting a Level does not complete it.
_Avoid_: Cleared, passed, done

## Gameplay

**Scenario**:
The situation one Level presents: a picture, a goal, short instructions, an optional Hint, and three or four Choices. Each Level has exactly one Scenario.
_Avoid_: Question, quiz, puzzle

**Setting**:
The place a Scenario happens, such as the classroom or the corridor. The Setting picks the scene art. Each Setting has a key: `classroom`, `corridor`, or `classroom_after`.
_Avoid_: Location, map, background (the background is only the picture)

**Phase**:
When a Scenario happens compared with the disaster: before, during, or after. In a "during" Scenario the scene shakes. An "after" Scenario starts still.
_Avoid_: Stage, step, timing

**Evacuation Area**:
The open space near the school building where classes gather after the shaking stops, so teachers can count every learner.
_Avoid_: Safe zone, assembly point

**Choice**:
One action the Learner can pick in a Scenario. Exactly one Choice per Scenario is the Safe Choice; every other Choice is an Unsafe Choice.
_Avoid_: Answer, option

**Safe Choice**:
The Choice that matches official safety guidance for the Scenario. A Level is completed only when the Learner picks the Safe Choice.
_Avoid_: Correct answer, right answer

**Unsafe Choice**:
A Choice that official guidance warns against. Picking one shows Feedback and lets the Learner choose again in the same Attempt.
_Avoid_: Wrong answer, mistake

**Feedback**:
The short, kind explanation shown after every Choice: why it is safe or unsafe, and the safety principle behind it.
_Avoid_: Result, verdict, correction

**Hint**:
Optional in-level help the Learner may ask for during a Scenario. It points toward the Safe Choice without naming it.
_Avoid_: Tip (reserved for Bantay's lines), clue

**Attempt**:
One play of a Level, from its start until Level Completed, Restart, or Quit to Menu. Nothing about an unfinished Attempt is saved.
_Avoid_: Try, run, session

**Star**:
The 1 to 3 mark a Completed Level earns. Each Unsafe Choice and using the Hint costs one Star, and a Completed Level always earns at least one. Only the best Stars for a Level are kept. Stars report how the activity went, not what the Learner learned.
_Avoid_: Score, points, grade

**Level Completed**:
The screen shown after the Safe Choice, once completion and Stars are saved. It shows the Stars, the Safe Choice, its explanation, and the Display Name.
_Avoid_: Results screen, victory screen, game over

**Duck, Cover, and Hold**:
The protective action during earthquake shaking: duck under a sturdy desk or table, cover, and hold on until the shaking stops. This is the wording PHIVOLCS school materials, DepEd, and the nationwide school drills use, and the only wording the game uses.
_Avoid_: Drop, Cover, and Hold On; Drop, Cover, and Hold

## Access

**Open**:
The state of an Episode or Difficulty the learner may enter. On a new save, only Earthquake and its Easy Difficulty are Open.
_Avoid_: Unlocked, available

**Locked**:
The state of an Episode or Difficulty the learner may see but not enter.
_Avoid_: Disabled, hidden

## Learner record

**Learner**:
The child playing on this device. The game has one Learner per device.
_Avoid_: User, player account, student profile

**Display Name**:
The short name the Learner types once and the game uses to greet them. A Display Name is not Saved Progress.
_Avoid_: Username, nickname, profile name

**Saved Progress**:
The stored record that at least one Level is a Completed Level. Continue is offered only when Saved Progress exists and at least one Playable Level exists.
_Avoid_: Save game, profile, progress data

**Last Played**:
The Episode and Difficulty of the most recently Completed Level. Continue returns the Learner to Level Selection for the Last Played Episode and Difficulty.
_Avoid_: Checkpoint, resume point

**Preserved Save**:
A save file the game could not read, kept beside the new one under a different name instead of being deleted.
_Avoid_: Backup, corrupt save, broken save

## Guidance

**Tutorial Page**:
One picture-and-caption step of How to Play, showing the Learner one thing to do. How to Play is a short sequence of Tutorial Pages.
_Avoid_: Slide, tutorial step, help screen

**Tip**:
One short line of advice from Bantay. A Tip on a Tutorial Page explains how to play; a Tip on the Main Menu is a short safety fact.
_Avoid_: Hint (reserved for in-level help), message

## Characters

**Bantay**:
The game's guide character, a friendly dog in a yellow hard hat who greets and instructs the Learner. Bantay is decoration and guidance only and never scores or judges: Bantay may cheer a Completed Level and encourage the Learner after an Unsafe Choice, but never calls a Choice wrong and never mentions Stars.
_Avoid_: Mascot, helper, NPC
