# 02: Flood menus and Next Episode into Flood

**What to build:** Flood's Difficulty selection shows the Flood Episode background, and each Flood Level selection shows its own Difficulty background: Easy, Normal and Hard. Their design is the one the user chose at art Batch 7, and the `MenuBackground` entries are named after that choice. With the one Next Episode rule from Typhoon, Typhoon Hard 7's Level Completed offers Next Episode into Flood once Flood has a Playable Level, and Flood Hard 7 offers none while Landslide has none. Flood keeps music Set B.

**Blocked by:** None among the Flood tickets, so it can run beside 01. Art Batch 7 (menu backgrounds) must be approved and imported, and Typhoon ticket 02 (Typhoon's menu backgrounds and the one Next Episode rule) must be built first.

**Status:** ready-for-agent

Read the [Flood specification](../../../docs/specs/flood-episode.md), the [Flood design](../../../docs/design/flood-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Flood research](../../../docs/research/flood-safety.md), the [Typhoon research](../../../docs/research/typhoon-safety.md), the [Flood art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `MenuNavigatorTests` check the Flood Episode background on Difficulty selection and the three Flood Difficulty backgrounds on Level selection, with every other Episode's backgrounds unchanged.
- `LevelCompletionTests` check that Typhoon Hard 7 offers Next Episode into Flood once Flood has a Playable Level, that it offers none while Flood has none, and that Flood Hard 7 offers none while Landslide has none.
- The menu screenshots show the four Flood backgrounds at both sizes, with any text on them readable and clear of the buttons, and every other menu screenshot stays byte-identical.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only Flood art that the user approved in its Setting and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons). A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites an FL or TY finding where it states safety, and the user approved the wording before the Scenario files were committed.
- Nobody is drawn standing in water. Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel. No Unsafe Act-out reaches the water's edge, a plug, the tap, the TV or the switch box, no hand touches one, and nobody is shown hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, with every Safe ending and Unsafe stop clear of the Caption, Hint and Pause. Every Earthquake, Fire and Typhoon screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/flood-episode-scratch/02-flood-menus/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
