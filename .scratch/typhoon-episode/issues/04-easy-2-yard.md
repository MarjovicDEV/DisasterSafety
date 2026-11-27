# 04: Easy 2 bring light things inside

**What to build:** Typhoon Easy 2 plays as a "before" Scenario in the `yard` with Dark Sky and light Wind from the first frame: the trees and the clothes on the line sway, small leaves blow across the top of the scene, and the wind loop plays quietly. Dad is tying things down; a pail, a ball and a plastic chair are on the grass. The Choices are "Help bring light things inside" (Safe: the Student picks up the pail, carries it to the house with `carry` and sets it down; Dad carries the chair in with his new `carry` pose) and "Keep playing with the ball" (the Student walks to the ball and stops beside it) (TY-03, TY-28, TY-29). This ticket adds Dad's `carry`, the pail, the ball and the plastic chair props, new `yard` action keys and one icon per Choice.

**Blocked by:** 01 and 03 (the Easy text approved there). Fire ticket 10 (the `yard` Setting) must be built first.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Dad's `carry` plays with the chair held clear of the Student.
- The Safe Choice completes Easy 2 with "All ready!". The Unsafe Choice stops beside the ball on clear grass, and Try Again returns the Student, the pail, the chair and Dad.
- The light Wind sways the trees and the clothes; no leaf passes within 40 art pixels of the Student or Dad, and nothing heavier than a leaf moves through the air.
- Fire Hard 4 in the `yard` plays as before.
- The user plays Typhoon Easy 1 to 3 in the Unity Editor after this ticket, and the Easy check is recorded in the design's "Researcher review".
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/04-easy-2-yard/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
