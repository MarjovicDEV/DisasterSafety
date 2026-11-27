# 07: The Barangay Tanod and Normal 5 the barangay warning

**What to build:** A new Grown-up, the Barangay Tanod, in the v2 style, about 36 x 62 pixels: navy shirt, cap, yellow reflective vest reading "TANOD", boots, a friendly visible face and a megaphone. Poses: idle, walk (side, 4 frames, mirrored), megaphone (front, 2 frames) and wave. Typhoon Normal 5 plays as a "during" Scenario in the `home` living room with heavy Rain, Wind and Dark Sky: the Tanod walks past outside the window, and his megaphone bubble says "Families near the creek, get ready!" with the voice blip. The Choices are "Tell Mom and bring the Go Bag" (Safe: the Student walks to Mom, calls "Mom, the Tanod!", picks up the Go Bag and carries it to Mom at the door), "Keep playing" (the Student sits with the toy and stops) and "Go out to see the Tanod" (the Student walks toward the window and stops on clear floor) (TY-18, TY-19, TY-20, TY-24, TY-51). This ticket adds the Tanod, his megaphone, new `home` action keys and one icon per Choice (reusing `icon_go_bag` or `icon_toy_car` only where the picture fits).

**Blocked by:** 01.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- The Tanod's preview sheet stands beside the Teacher, Mom, Dad and the Firefighter at game size, and his face reads as friendly.
- The Safe Choice completes Normal 5 with "I'm safe!". Each Unsafe Choice stops on clear indoor floor and resets on Try Again, with the Go Bag, the toy, Mom and the Tanod back at their start.
- The megaphone bubble plays after the layers fade in and stays readable at both sizes.
- The user plays Typhoon Normal 1 to 5 in the Unity Editor after this ticket and tickets 05 and 06, and the Normal check is recorded in the design's "Researcher review".
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/07-tanod-normal-5-home/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
