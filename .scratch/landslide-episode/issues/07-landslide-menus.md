# 07: Landslide menus and Next Episode from Flood Hard 7

**What to build:** Landslide's Difficulty selection shows the Landslide Episode background, and each Landslide Level selection shows its own Difficulty background, chosen by the user at art Batch 5. Landslide Levels are listed as Playable. Flood Hard 7's Level Completed offers Next Episode into Landslide once Landslide has a Playable Level, under the one Next Episode rule. Landslide is the last Episode, so no Landslide Level ever offers Next Episode; a Landslide Hard Level ends with Replay, Levels and Main Menu (the game-finished ending is ticket 08). Music Set A plays as it already does.

**Blocked by:** 01 (Landslide Playable Levels). Art Batch 5 (menu backgrounds) must be approved and imported. The Flood ticket that adds the Flood menus and keeps the one Next Episode rule must be built first.

**Status:** ready-for-agent

Read the [Landslide specification](../../../docs/specs/landslide-episode.md), the [Landslide design](../../../docs/design/landslide-episode.md), [ADR 0006](../../../docs/adr/0006-landslide-safety-rules.md), [ADR 0003](../../../docs/adr/0003-licensed-music.md), the [Landslide art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `MenuNavigatorTests` check the Landslide Episode background on Difficulty selection and the three Landslide Difficulty backgrounds on Level selection, with every other Episode's backgrounds unchanged, and that Landslide Levels are listed as Playable.
- `LevelCompletionTests` check that Flood Hard 7 offers Next Episode into Landslide once Landslide has a Playable Level, offers none while it has none, and that no Landslide Level offers Next Episode.
- The menu screenshots show the four Landslide backgrounds at 1920 x 1080 and 2400 x 1080 with the menu panels readable. Every Earthquake, Fire, Typhoon and Flood menu screenshot stays byte-identical.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Landslide art that the user approved and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md). A missing picture is named in the evidence and asked of the user; it is not drawn here.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Evidence is stored in `D:/GameDEV/landslide-episode-scratch/07-landslide-menus/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
