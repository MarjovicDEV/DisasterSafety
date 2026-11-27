# 02: Fire Difficulty backgrounds and Set B music

**What to build:** Fire's Level selection shows its own Difficulty backgrounds in place of the station noticeboard stand-in: the `home` living room for Easy, the school corridor with the red Fire Bell box for Normal, and the barangay street at dusk with a parked fire truck for Hard. Fire plays Set B: Cheery Monday on Easy, B-Roll on Normal, Machinations on Hard and Piano Between on Level Completed. The tracks are imported from the prepared WAVs and credited in the music licence list and the credits view, as ADR 0003 requires.

**Blocked by:** None (can start immediately).

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- Menu background tests choose the three Fire Difficulty backgrounds, and Earthquake's stay unchanged.
- Entering a Fire Level plays its Difficulty's Set B track, and returning to the menus restores the menu music. Music preferences, fades and ducking still work.
- The credits view lists the four Set B recordings. The APK size is measured and recorded.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/02-fire-menus-and-music/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
