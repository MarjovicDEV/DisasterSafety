# 12: Hard 6 neighbor's fire and Hard 7 Firefighter on the street

**What to build:** Two new Settings. `street` is a Baluan barangay street with a neighbor's house showing Flames and Smoke at a distance. `street_after` shows the Student's own house with the fire out and the fire truck parked. Hard 6's Choices are "Tell an adult to call 911" (Safe: the Student stays back and says "Please call 911!" to Dad, who calls), "Go near to watch", "Film it" and "Run inside to help". In Hard 7 ("after"), a Firefighter walks over. Its Choices are "Go to the Firefighter" (Safe: the Firefighter kneels and waves the family to the Meeting Place), "Hide from the Firefighter", "Go back in for the cat" and "Run back home".

**Blocked by:** 03, 09.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- Each Safe Choice completes its Level. Each Unsafe Choice stops well clear of the burning house, the doorway or the hiding place, and resets on Try Again.
- Fire Hard 7 shows no Next Episode while Typhoon has no Playable Levels.
- The user plays Fire Hard in the Unity Editor after this ticket.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/12-hard-6-7-street/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
