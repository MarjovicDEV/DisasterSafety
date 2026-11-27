# 11: Hard 5 Fire Bell in the mall

**What to build:** Fire Hard 5 plays in `mall_a` and `mall_b`, the way Earthquake Hard 3 and 4 do, with the Fire Bell and light Smoke. The Choices are "Follow the EXIT sign to the stairs with Mom" (Safe: the Student holds Mom's hand, follows the EXIT sign down the stairs and goes out to the assembly area), "Take the elevator", "Run and push" and "Go back for the toy".

**Blocked by:** 03.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- The Safe Choice completes the Level. The elevator Choice stops before the doors open, and every Unsafe Choice resets on Try Again.
- Earthquake Hard 3 and 4 keep their art, Act-outs and screenshots.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/11-hard-5-mall/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
