# 06: Normal 3 away from the bedroom window

**What to build:** Typhoon Normal 3 plays as a "during" Scenario in the `bedroom` with heavy Rain, Wind and Dark Sky on the window; the window rattles on the Wind frames, and there is no warning bubble. The Choices are "Go to the living room with Mom" (Safe: the Student walks to the bedroom door, the door opens and the Student walks out; Mom waits in the doorway), "Watch the storm at the window" (the Student walks toward the window and stops at the bed's end) and "Open the window" (the Student walks toward the window, plays `reach-up` toward the latch and stops well short of it) (TY-04, TY-05, TY-32, TY-33). This ticket adds the rattling window frames, new `bedroom` action keys and one icon per Choice, including the first window icon (`icon_window` does not exist yet). The bedroom door is the one Fire ticket 09 adds; reuse it.

**Blocked by:** 01. Fire ticket 09 (the bedroom door) must be built first.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- The Safe Choice completes Normal 3 with "I'm safe!". Each Unsafe Choice stops on clear floor, apart from the window and the latch, and resets on Try Again, with the bedroom door closed and Mom back at her start.
- The window stays closed in every Act-out, and no Typhoon Choice tapes or covers it.
- Earthquake Normal 1 and Fire Hard 1 to 3 in the `bedroom` play as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/06-normal-3-bedroom/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
