# 11: Hard 5 wait for the Tanod's all-clear

**What to build:** Typhoon Hard 5 plays as an "after" Scenario in `evacuation_area` with Dark Sky and no Rain from the first frame; the rain has stopped, and some neighbors are leaving across the court. It starts still and shows the Action Bar after 1 second. The Choices are "Wait for the Tanod to say it is safe" (Safe: the Student sits on the mat by Mom; the Tanod walks in, and his megaphone bubble says "It is safe to go home now."), "Go home with the neighbors" (the Student walks toward the court edge after them and stops), "Run out to play" (the Student run-starts toward the open side and stops) and "Look for your friend alone" (the Student walks off between the tents and stops) (TY-18, TY-54, TY-55). This ticket adds Typhoon's `evacuation_area` action keys and one icon per Choice (reusing `icon_go_home` only where the picture fits). If the researcher's text review replaces "Look for your friend alone", build the replacement instead.

**Blocked by:** 07 (the Tanod) and 08 (the Hard text approved there).

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- The Safe Choice completes Hard 5 with "I'm safe!". Each Unsafe Choice stops on clear ground inside the court, short of its edge, and resets on Try Again, with Mom, the Tanod and the neighbors back at their start.
- The Tanod's all-clear plays only in the Safe Act-out, after the Student sits.
- Earthquake Hard 7 in `evacuation_area` plays as before, with its Choices and the covered court unchanged.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/11-hard-5-evacuation-center/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
