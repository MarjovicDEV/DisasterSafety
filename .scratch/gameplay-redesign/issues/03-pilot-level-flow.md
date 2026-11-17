# 03 Pilot Level flow: Easy 1 acted out by the Student

Blocked by: 01, 02. Blocks: 04.

Spec: `docs/specs/gameplay-redesign-pilot.md` (behaviours 1 and 3 to 12, Data, Code, Tests). Design: `docs/design/gameplay-redesign.md`. Rules: `AGENTS.md`.

## Build (test first, one slice at a time)

1. Data: `ScenarioChoice.icon` and `.action`; the `ScenarioRules` checks; `Earthquake/Easy/1.json` gets `icon` and `action` per Choice (no text change). EditMode tests from the spec.
2. Scene: the `classroom_v2` Setting group (background, wobbling fan and cabinet, `desk_front` overlay), `LevelSetting.startX` and `actionSpots` from the README placements; the Caption banner, the Action Bar with 4 `ActionButton`s, the Student (`StudentActor` with all poses for both kinds, chosen from the save).
3. Flow in `LevelController`: the new-flow branch for behaviours 3 to 11; the old flow untouched. Reactions from `UIStrings`.
4. PlayMode tests from the spec (safe, unsafe, hint), and every existing PlayMode test still passing.
5. Screenshots `60` to `65` at both sizes; read them and fix overlaps (the Caption, Hint and Pause buttons, the Action Bar and the Student must not cover each other or the desk, door, window and cabinet).

## Done when

Build, EditMode and PlayMode pass with zero failures; screenshots 60-65 read clearly at 1920x1080 and 2400x1080; screenshots 10-57 are unchanged; no Scenario file other than Easy 1 changed; the work is committed (ask the user how many commits; one sentence each, no co-author line).
