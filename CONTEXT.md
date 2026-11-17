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
The situation one Level presents: a Setting, a goal, a short intro, an optional Hint, and three or four Choices that the Student acts out. Each Level has exactly one Scenario.
_Avoid_: Question, quiz, puzzle

**Setting**:
The place a Scenario happens, such as the classroom or the corridor. The Setting picks the scene art. Each Setting has a key: `classroom`, `corridor`, `classroom_after`, `bedroom`, `home`, `schoolyard`, `beach`, `mall_a`, `mall_b`, `home_after`, or `evacuation_area`.
_Avoid_: Location, map, background (the background is only the picture)

**Phase**:
When a Scenario happens compared with the disaster: before, during, or after. In a "during" Scenario the scene shakes. An "after" Scenario starts still.
_Avoid_: Stage, step, timing

**Evacuation Area**:
The open space near the school building where classes gather after the shaking stops, so teachers can count every learner.
_Avoid_: Safe zone, assembly point

**Go Bag**:
The emergency supply kit a family packs before an earthquake: water, food, a flashlight, a whistle and other things for 3 days, kept where it is easy to reach.
_Avoid_: Survival kit, emergency bag (PHIVOLCS uses both; the game says go bag)

**Choice**:
One action the Learner can pick in a Scenario, shown as a short command in the Action Bar and acted out by the Student. Exactly one Choice per Scenario is the Safe Choice; every other Choice is an Unsafe Choice.
_Avoid_: Answer, option, Action (the game says Choice; "action" is only the animation)

**Action Bar**:
The bar at the bottom of a Level that lists the Scenario's Choices as short commands, each with a small icon. In a "during" Scenario it slides up after the first shake.
_Avoid_: Choice cards, menu, answer panel

**Caption**:
The banner at the top of a Level that tells the Scenario intro and the Goal, and shows the Hint when the Learner asks for it.
_Avoid_: Narrator, subtitle, dialog

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
Optional in-level help the Learner may ask for during a Scenario, shown in the Caption. It points toward the Safe Choice without naming it.
_Avoid_: Tip (reserved for How to Play and Main Menu lines), clue

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
One short line of advice in the Student's speech bubble. A Tip on a Tutorial Page explains how to play; a Tip on the Main Menu is a short safety fact.
_Avoid_: Hint (reserved for in-level help), message

## Characters

**Student**:
The Learner's character: a boy or a girl in the school uniform, picked once with the Display Name. The Student acts out every Choice in the Level, greets the Learner on the Main Menu and gives the Tips in How to Play.
_Avoid_: Avatar, player, hero

**Reaction**:
A short line in the Student's speech bubble during a Level, such as "Whoa!", "Hmm, wait..." or "I'm safe!". Reactions never call a Choice wrong and never mention Stars.
_Avoid_: Feedback (the explanation after a Choice), dialog

**Bantay** (retired):
The former guide dog. Removed by docs/adr/0001-v2-art-and-no-bantay.md; the Caption, the Student and Reactions took over its jobs. Its sprites stay in the repo only until every Level uses the new flow.
_Avoid_: using the name for anything new
