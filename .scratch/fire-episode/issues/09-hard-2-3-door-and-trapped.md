# 09: Hard 2 feel the door, Hard 3 trapped, the Firefighter

**What to build:** The `feel_door` and `signal` poses, and the Firefighter Grown-up: about 62 pixels tall, with a helmet, a coat with reflective stripes, boots, and the mask at the neck so the face shows. Poses: idle, walk, wave and kneel. Hard 2's Choices are "Feel the door first" (Safe: crawl to the door, put the back of the hand on it, say "Hot!", crawl to the window to the yard and go to the Meeting Place), "Open the door", "Hide in the closet" and "Wait in bed". Hard 3's Choices are "Block the gap and signal at the window" (Safe: push a blanket into the gap under the door, wave a bright cloth at the window and say "Help! I'm here!"; Mom outside calls 911, and the Firefighter reaches the window), "Jump from the window", "Open the door" and "Take a video".

**Blocked by:** 08.

**Status:** ready-for-agent

Read the [Fire specification](../../../docs/specs/fire-episode.md), the [Fire design](../../../docs/design/fire-episode.md), [ADR 0004](../../../docs/adr/0004-fire-safety-rules.md), the [Fire research](../../../docs/research/fire-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs.

## Done when

- Each Safe Choice completes its Level. Each Unsafe Choice stops before the door opens, at the closet door or at the sill, and resets on Try Again.
- The Firefighter looks friendly in every pose, and the face is never covered.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import.
- Every Caption, Hint, Feedback, Choice label and Act-out line cites an FI finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out touches Smoke, Flames, heat or a drop, and nobody is shown burning or hurt.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/fire-episode-scratch/09-hard-2-3-door-and-trapped/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
