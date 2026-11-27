# 05: Easy 1 lighter and Easy 2 brownout candle

**What to build:** Fire Easy 1 and 2 play as "before" Scenarios with two Choices each. Easy 1 is in the `home` living room, where a lighter lies on the floor: "Give it to Mom" (Safe: the Student picks it up by the body, walks to Mom and says "Mom, a lighter!") and "Try to light it". Easy 2 is in the new `home_night` Setting during a brownout, lit by the candle and a window, where a candle burns near the curtain: "Tell Mom to move it" (Safe: Mom moves it to a heat-safe plate away from the curtain and hands the Student a flashlight) and "Play with the candle".

**Blocked by:** 01.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- `home_night` is a known Setting with its action keys, and it never shakes.
- Each Level's Safe Choice completes it with "All ready!". Each Unsafe Choice stops before the flame and resets on Try Again.
- The content tests check two Choices per Easy Level.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/05-easy-1-2-home/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
