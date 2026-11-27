# 06: Kitchen: Easy 3 pot and Normal 4 oil fire

**What to build:** The new `kitchen` Setting serves two Levels. In Easy 3 ("before"), a pot boils over and Mom is outside: "Call Mom" (Safe: the Student stays away from the stove and calls "Mom, the pot!"; Mom comes and turns the heat down) and "Move the pot yourself". In Normal 4 ("during", with small Flames in the pan), someone shouts "Fire!": "Shout "Fire!" and go out with Mom" (Safe: Mom covers the pan with its lid and turns off the stove, then both go out and close the door), "Throw water on it" and "Carry the pan outside". The Student never touches the pan.

**Blocked by:** 03.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- Each Level's Safe Choice completes it. Each Unsafe Choice stops before the stove and resets on Try Again, with the lid, the pot and Mom back in place.
- Normal 4 uses the Flames layer from ticket 03, in the pan only.
- The user plays Fire Easy in the Unity Editor after this ticket.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/06-kitchen-easy-3-normal-4/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
