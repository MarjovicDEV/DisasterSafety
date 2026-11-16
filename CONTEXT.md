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

## Characters

**Bantay**:
The game's guide character, a friendly dog in a yellow hard hat who greets and instructs the Learner. Bantay is decoration and guidance only and never scores or judges.
_Avoid_: Mascot, helper, NPC
