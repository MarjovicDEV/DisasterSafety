# 15 Cleanup: remove Bantay, the Choice pictures and the card flow (Q17, R7)

Blocked by: 14.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- Delete Bantay: `BantayGuide.cs`, its sprites and their `.aseprite` sources, its `UIStrings`, and every reference in `ProjectBuilder*.cs`, `MenuScreenshots.cs` and `LevelScreenshots.cs`.
- Delete the Choice pictures (`Assets/Art/Level/choice_*` and their sources) and the `picture` field from `ScenarioChoice` and all 15 Earthquake Scenario files. No other Scenario field changes.
- Delete the card flow in `LevelController` and its UI in `ProjectBuilder.Level.cs`; `ScenarioRules` then rejects a Scenario without an `action` on every Choice.
- Delete the v1 Setting backgrounds and moving parts that no Level uses any more, and their v1 draw calls in `build.lua`. Keep `tutorial.lua` outputs only if How to Play still uses them.
- Remove the screenshots and tests that only covered the card flow; update `CONTEXT.md` (the Bantay entry) and the docs that still describe Choice cards.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures.
- A search for `Bantay`, the `choice_` pictures and the `picture` field finds nothing outside the history docs.
- All 15 Earthquake Levels still play end to end.
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
