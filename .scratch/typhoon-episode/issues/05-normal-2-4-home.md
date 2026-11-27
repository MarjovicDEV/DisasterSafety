# 05: Normal 2 Signal No. 2 at home and Normal 4 mangoes in the yard

**What to build:** Typhoon Normal 2 and Normal 4 play as "during" Scenarios in the `home` living room with heavy Rain, Wind and Dark Sky on the window. Normal 2: the TV screen reads "SIGNAL NO. 2", and the warning bubble comes from the TV; the Student is in uniform with the school bag. The Choices are "Stay home and do your module" (Safe: the Student puts the bag down, sits at the table with the module booklet and writes with `pack`), "Walk to school anyway" (the Student carries the bag toward the door and stops) and "Go out to play in the rain" (the Student walks toward the door and stops) (TY-12, TY-15, TY-16, TY-32). Normal 4 has no warning bubble; through the window the mangoes lie on the grass under the swaying tree and the clothes flap on the line, and the door stays closed. The Choices are "Stay inside with Mom" (Safe: the Student walks away from the door to Mom and sits beside her), "Pick up the mangoes" (the Student walks toward the door and stops) and "Bring in the clothes" (the Student walks toward the door, reaches toward the knob and stops before touching it) (TY-03, TY-32, TY-46). This ticket adds the module booklet, the mangoes, the clothes on the line, the school bag (from Fire) in `home`, new `home` action keys and one icon per Choice. The two door stops in each Level stop at clearly different points.

**Blocked by:** 01.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- Each Level's Safe Choice completes it with "I'm safe!". Each Unsafe Choice stops on clear indoor floor before the closed door and resets on Try Again, with the bag, the module booklet and Mom back at their start.
- No mango falls through the air; the mangoes lie on the grass, and only the tree and the clothes sway.
- The TV bubble in Normal 2 plays after the layers fade in and before the surprised pose.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/05-normal-2-4-home/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
