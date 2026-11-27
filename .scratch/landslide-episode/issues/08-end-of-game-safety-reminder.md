# 08: The end of the game: Game finished and the Safety Reminder

**What to build:** The first time the Learner completes every Landslide Hard Level (in any order), Level Completed shows a "Game finished" ribbon and a Finish button in the Next position. Finish opens a one-page Safety Reminder: the art Batch 6 background, the Student with the Display Name, the five Episode emblems in Episode order, one short Tip under each emblem, and a Main Menu button that returns to the Main Menu. Later completions of Landslide Hard Levels show Replay, Levels and Main Menu with no Finish. The rule lives in the gameplay core: `LevelCompletion` sets a new `CompletionResult` flag (for example `GameFinished`) on the completion that first finishes Landslide Hard, the same moment `EpisodeCompleted` is set for Landslide; nothing new is saved. The five Tips are `UIStrings` lines, one per Episode, each with a finding ID comment (EQ, FI, TY, FL or LS), at most 12 words, pixel-font characters only and no word "drop"; this ticket drafts them for the user. The Student's line on the page follows the Reaction rule (no judging words, no Stars). The screen is built by the project builder; Pause and Hint do not appear on it.

**Blocked by:** 01 (Landslide content in the build). Art Batch 6 (the Safety Reminder background) must be approved and imported. 02, 04, 05 and 06 are not needed to build or test the rule, but the user's end-to-end check in the Editor waits for every Hard Level.

**Status:** ready-for-agent

Read the [Landslide specification](../../../docs/specs/landslide-episode.md) ("The end of the game"), the [Landslide design](../../../docs/design/landslide-episode.md) (L7), the five research notes in `docs/research/`, the [Landslide art batches](../art-batches.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md) (Safety Reminder, Level Completed, Display Name, Tip). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `LevelCompletionTests` check the game-finished flag: set on the completion that first makes every Landslide Hard Level a Completed Level, whatever the order; not set on earlier Landslide Hard completions; not set on any later completion; never set by another Episode or Difficulty. The existing unlock and Next Episode tests still pass.
- An EditMode test checks the five Tips: one per Episode in Episode order, each citing a finding that exists in the research notes, at most 12 words, pixel-font characters only, no word "drop".
- PlayMode: Level Completed shows the ribbon and Finish when the flag is set and not otherwise; Finish opens the Safety Reminder with the Display Name, the Student and the five emblems; Main Menu returns to the Main Menu.
- The drafted Tips are in the evidence folder, and the user approved them before they were committed.
- Screenshots of Level Completed with Finish and of the Safety Reminder at 1920 x 1080 and 2400 x 1080, with nothing the Learner must read in the cropped rows. Every other screenshot stays byte-identical.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- The ticket uses only art that the user approved and that was imported from `ArtSource/` under [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md); the Episode emblems are reused. A missing picture or sound is named in the evidence and asked of the user; it is not drawn here.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Evidence is stored in `D:/GameDEV/landslide-episode-scratch/08-end-of-game-safety-reminder/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, in 6 to 9 commits as `AGENTS.md` says. Each message is one sentence in basic English with no co-author line or other attribution.
