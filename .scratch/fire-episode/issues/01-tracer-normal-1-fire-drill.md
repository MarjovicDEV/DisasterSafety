# 01: Tracer: Normal 1 classroom fire drill

**What to build:** The Learner opens Fire Normal 1 and plays it end to end. Fire Scenarios load from their own Episode folder, and Fire Normal 1 shows as a Playable Level. A Fire "during" Scenario does not shake. Instead the Fire Bell rings (a recorded, human-made CC0 school bell, credited like the music), the Student plays the surprised pose, and the Action Bar slides up. The bell keeps ringing quietly until the Safe Act-out is out of danger, and Pause stops it. The three Choices are "Line up and walk out" (Safe: the Student leaves the bag, lines up behind the Teacher and walks out of the door without running), "Duck under the desk" (reuses the Earthquake classroom `desk` Act-out up to its stop) and "Get your bag". This ticket adds the Fire content rules and the ADR 0004 rule test, so later tickets only add Levels.

**Blocked by:** None (can start immediately). The researcher approves the Fire Level list (design F7) before this ticket starts.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- `Scenario.Shakes` is true only for an Earthquake "during" Scenario, and a Fire "during" Scenario starts the Fire effect instead (EditMode tests for both Episodes).
- The content tests cover Fire files: three Choices on Normal, one Safe Choice, icons, actions and FI citations. A rule test fails if a Fire Safe Choice uses an extinguisher, an elevator or water.
- PlayMode: the Safe Choice completes Fire Normal 1. An Unsafe Choice shows Feedback, and Try Again returns the Student, the bag and the Teacher. The Fire Bell starts, and Pause stops it.
- Every Earthquake Level plays and shakes as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/01-tracer-normal-1-fire-drill/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
