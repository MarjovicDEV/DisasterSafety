# Disaster Safety Game

An offline, single-player Android game that teaches Grade 3 to Grade 6 learners of Baluan Elementary School what to do before, during, and after a disaster. This glossary fixes the words used in code, tests, the manuscript, and the vault.

## Learning structure

**Episode**:
One disaster topic and all of its content. There are exactly five, always in this order: Earthquake, Fire, Typhoon, Flood, Landslide.
_Avoid_: Chapter, world, stage, module

**Difficulty**:
One of three tiers inside an Episode: Easy (3 Levels), Normal (5 Levels), Hard (7 Levels).
_Avoid_: Mode, tier, grade level

**Episode background**:
The picture behind Difficulty selection for a particular Episode. For example, Earthquake uses a school drill ground.

**Difficulty background**:
The picture behind Level selection for a particular Episode and Difficulty. For example, Earthquake Easy uses a classroom with green chairs.

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
The situation one Level presents: a Setting, a goal, a short intro, an optional Hint, and Choices that the Student acts out. Each Level has exactly one Scenario. In every Episode, the approved design gives each Easy Scenario two Choices, each Normal Scenario three, and each Hard Scenario four.
_Avoid_: Question, quiz, puzzle

**Setting**:
The place a Scenario happens, such as the classroom or the corridor. The Setting picks the scene art. Each Setting has a key: `classroom`, `corridor`, `classroom_after`, `bedroom`, `home`, `schoolyard`, `beach`, `mall_a`, `mall_b`, `home_after`, or `evacuation_area`. The Fire Episode adds `kitchen`, `home_night`, `yard`, `street` and `street_after` (docs/design/fire-episode.md). The Typhoon Episode adds `street_storm` and `coast` (docs/design/typhoon-episode.md). The Flood Episode adds `creek` (docs/design/flood-episode.md). The Landslide Episode adds `hillside` and `hill_road` (docs/design/landslide-episode.md).
_Avoid_: Location, map, background (the background is only the picture)

**Phase**:
When a Scenario happens compared with the disaster: before, during, or after. In an Earthquake "during" Scenario the scene shakes; in a Fire "during" Scenario Smoke drifts in, the Fire Bell rings or someone shouts "Fire!", and small Flames burn at one side, with no shake; in a Typhoon "during" Scenario Rain, Wind and Dark Sky fade in, a Wind Signal warning is given, and nothing shakes; in a Landslide "during" Scenario Rain and Dark Sky fade in with a Slide Sign or Muddy Runoff, and nothing shakes. A "before" or "after" Scenario starts still, though a Typhoon one may already show Dark Sky or light Wind.
_Avoid_: Stage, step, timing

**Evacuation Area**:
In the school drill, the open space near the school building where classes gather after the shaking stops so teachers can count every Learner. In Earthquake Hard 7, the family later gathers at a fictional designated evacuation centre depicted as a covered basketball court.
_Avoid_: Safe zone, assembly point

**Evacuation Center**:
A building the local government names as a shelter, such as a school, a gym or a covered court, where families stay during and after a typhoon until officials say it is safe to go home. The `evacuation_area` Setting shows one. Not the Evacuation Area of the school drill.
_Avoid_: Shelter, evacuation site, safe house

**Go Bag**:
The emergency supply kit a family packs before an earthquake or a typhoon: water, food, a flashlight, a whistle and other things for 3 days, kept where it is easy to reach. It never holds a candle or matches in the game (docs/adr/0005-typhoon-safety-rules.md).
_Avoid_: Survival kit, emergency bag (PHIVOLCS uses both; the game says go bag)

**Choice**:
One action the Learner can pick in a Scenario, shown as a short command in the Action Bar and acted out by the Student. Exactly one Choice per Scenario is the Safe Choice; every other Choice is an Unsafe Choice.
_Avoid_: Answer, option, Action (the game says Choice; "action" is only the animation)

**Action Bar**:
The bar at the bottom of a Level that lists the Scenario's Choices as short commands, each with a small icon. In the approved Earthquake design, Easy and Normal center their Choice buttons in one row. In a "during" Scenario it slides up after the first shake.
_Avoid_: Choice cards, menu, answer panel

**Act-out**:
The Student playing a Choice in the scene, step by step, the way the real drill is done. A Safe Choice's Act-out plays in full; an Unsafe Choice's Act-out starts, stops partway, and the Student says "Hmm, wait...". Nothing falls and nobody gets hurt.
_Avoid_: Animation, cutscene, jump cut

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
One play of a Level, from its start until Level Completed, Restart, or departure from the Level. In the approved Pause design, choosing Levels or Main Menu ends the Attempt. Resume continues it. Nothing about an unfinished Attempt is saved.
_Avoid_: Try, run, session

**Star**:
The 1 to 3 mark a Completed Level earns. Each Unsafe Choice and using the Hint costs one Star, and a Completed Level always earns at least one. Only the best Stars for a Level are kept. Stars report how the activity went, not what the Learner learned.
_Avoid_: Score, points, grade

**Level Completed**:
The screen shown after the Safe Choice, once completion and Stars are saved. It shows the Stars, the Safe Choice, its explanation, and the Display Name. In the approved Earthquake design, it also offers Next Episode when Fire opens and on subsequent completions of Earthquake Hard 7. The button opens Episode selection.
_Avoid_: Results screen, victory screen, game over

**Smoke**:
In a Fire Level, the grey layer that drifts in from the top of the scene, filling up to the top 40%, so the Student must stay low under it. A per-Level layer on top of the Setting art, not a separate Setting.
_Avoid_: Fog, haze

**Flames**:
In a Fire Level, small fire drawn at one side of the scene, never on a person. A per-Level layer like Smoke.
_Avoid_: Blaze, inferno

**Fire Bell**:
The alarm that starts a school or mall fire Scenario. Home Levels use a Grown-up shouting "Fire!" instead, because smoke alarms are not confirmed as common in Philippine homes.
_Avoid_: Fire alarm (the game says Fire Bell), siren

**Crawl Low**:
Moving on hands and knees under the Smoke to get out. BFP's own words are "crawl down on your knees" and "Crawl low on the floor".
_Avoid_: Get low and go, fall and crawl

**Stop, Drop, and Roll**:
What to do when clothes catch fire: stop, drop to the ground, cover the face with the hands, and roll until the flame is out. BFP's wording; the face cover follows NFPA.
_Avoid_: Stop, Drop and Cover

**Meeting Place**:
The spot outside the home where the family gathers after getting out of a fire, so everyone is counted. At school the class goes to the Evacuation Area instead.
_Avoid_: Safe zone, assembly point

**Wind Signal**:
PAGASA's Tropical Cyclone Wind Signal, No. 1 to No. 5, raised for an area before typhoon winds arrive. The game says "Wind Signal No. 2" in Captions and "SIGNAL NO. 2" on a TV, and shows only Signals 1 to 3. For Grades 3 to 6, Signal No. 2 or higher means no face-to-face classes.
_Avoid_: Storm signal, typhoon signal, PSWS, TCWS (in Learner text)

**Rain**:
In a Typhoon Level, the light or heavy streaks of falling rain drawn over the scene. A per-Level layer like Smoke, not a separate Setting.
_Avoid_: Storm, downpour

**Wind**:
In a Typhoon Level, the layer that makes trees, curtains, clothes and loose things sway and small leaves blow across the top of the scene. Nothing heavier than a leaf moves through the air, and nothing passes near a person.
_Avoid_: Gust layer, storm effect

**Dark Sky**:
In a Typhoon Level, the grey tint on the sky and on the windows before, during and just after the storm. A per-Level layer; Night is the Fire layer for darkness indoors.
_Avoid_: Overcast, gloom

**Floodwater**:
In a Flood Level, the brown-grey water drawn over the scene: a seep under the closed front door indoors, or shallow or moving water over the yard, the road or a path outdoors. It never fills a room, and no one is ever drawn standing in it. A per-Level layer like Smoke, not a separate Setting.
_Avoid_: Flood (the Episode), puddle (a small rain puddle in Typhoon), baha (in code and Captions)

**Slope**:
In a Landslide Level, the steep side of a hill, or a road cut into a hill, that soil and rock can slide down. No actor ever stands, stops or plays on a Slope or below it; every start and stop is on flat ground at least 24 art pixels from its foot (docs/adr/0006-landslide-safety-rules.md).
_Avoid_: Hill (the whole landform), cliff, mountain

**Slide Sign**:
A sign that a Slope may slide: a new crack in the ground or a wall, a tree or post that leans more than before, or muddy water coming out of the ground. The Student tells a Grown-up; nobody goes near it.
_Avoid_: Warning sign (the posted "LANDSLIDE ZONE" board is a sign of its own), clue

**Fallen Soil**:
In a Landslide Level, the soil, mud and small rocks that came down a Slope and stopped, drawn at rest at the Slope's foot, across a path or at a road edge, far from any home and person. The only moving soil in the game is the far slide in Landslide Hard 2. A per-Level layer like Floodwater, not a separate Setting.
_Avoid_: Debris and mudslide (in Learner text), landslide (the event and the Episode), pagguho (in code and Captions)

**Muddy Runoff**:
In a Landslide Level, the thin brown water that runs off a Slope and over the road in heavy rain. No one is drawn standing in it.
_Avoid_: Floodwater (Flood's water layer), puddle, mud (the Fallen Soil)

**Storm Surge**:
The rise of the sea during a typhoon that floods low coasts. Taught only in the Typhoon `coast` Level; the sea pulling back before a tsunami is an Earthquake topic.
_Avoid_: Tidal wave, big waves (in Feedback)

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

**Safety Reminder**:
The one-page end-of-game screen, opened by Finish on Level Completed the first time the Learner completes Landslide Hard: the Student with the Display Name, the five Episode emblems and one cited Tip per Episode (docs/design/landslide-episode.md, L7).
_Avoid_: Ending, credits, certificate, game over

## Characters

**Student**:
The Learner's character: a boy or a girl in the school uniform, picked once with the Display Name. The Student acts out every Choice in the Level, greets the Learner on the Main Menu and gives the Tips in How to Play. In the approved Main Menu design, the Student greets the Learner in front of the Baluan Elementary School gate.
_Avoid_: Avatar, player, hero

**Reaction**:
A short line in the Student's speech bubble during a Level, such as "Whoa!", "Hmm, wait..." or "I'm safe!". Reactions never call a Choice wrong and never mention Stars.
_Avoid_: Feedback (the explanation after a Choice), dialog

**Grown-up**:
An adult who appears and acts in a Level: the Teacher, Mom, Dad, in Fire the Firefighter (in full gear, face visible and friendly), or in Typhoon and Landslide the Barangay Tanod. A Grown-up is never the Learner's character, and, like a Reaction, never judges a Choice or mentions Stars.
_Avoid_: NPC, adult character, parent (Mom and Dad are named)

**Barangay Tanod**:
In Typhoon and Landslide, the barangay watchman who gives the barangay's warning through a megaphone, leads families on the evacuation route and says when it is safe to go home. Navy shirt, cap, yellow reflective vest reading "TANOD", friendly visible face.
_Avoid_: Barangay official, rescuer, guard

**Bantay** (retired):
The former guide dog. Removed by docs/adr/0001-v2-art-and-no-bantay.md; the Caption, the Student and Reactions took over its jobs. The Earthquake rollout cleanup (ticket 15) removed its code, art, sound and lines from the game.
_Avoid_: using the name for anything new
