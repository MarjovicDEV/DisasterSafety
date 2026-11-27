# 03: Smoke and Flames layers, Normal 2 smoky corridor

**What to build:** Per-Level Smoke and Flames layers, chosen by the Level's start entry on its Setting. Smoke is a soft grey band in two or three loose layers that drifts across the top 40% of the scene. Flames are a small loop of two to four frames at a named point. In a Fire "during" Level they fade in over about 1.5 seconds before the Fire Bell or the shout, and Pause stops them. Fire Normal 2 plays in the `corridor` with Smoke: "Stay low and follow the Teacher" (Safe: the Student holds a handkerchief to the nose and walks low behind the Teacher to the stairs), "Run through the smoke" and "Hide in the comfort room".

**Blocked by:** 01.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- PlayMode: the layers appear only in the Levels that name them, fade in before the Action Bar, stop on Pause and resume after.
- Fire Normal 2's Safe Choice completes the Level. Each Unsafe Choice stops on clear ground before the Smoke or the comfort room door and resets on Try Again.
- The Smoke band is readable at both sizes, including when the 2400 x 1080 view crops the top 54 rows.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/03-smoke-flames-normal-2-corridor/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
