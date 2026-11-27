# 09: The `coast` Setting and Hard 2 Storm Surge

**What to build:** A new Setting, `coast`: a seaside road with high, rough waves behind a sea wall, steps down to the sand, a house, and a path up to an Evacuation Center sign, at the v2 bar, never shaking. The waves animate behind the wall and never reach the road. Typhoon Hard 2 plays there as a "before" Scenario with light Rain, Wind and Dark Sky from the first frame. The Caption tells that the Student is visiting Lola by the sea and the radio warns of a Storm Surge; Lola is named only, and Dad is drawn. The Choices are "Go up to the Evacuation Center with Dad" (Safe: the Student turns and walks with Dad up the path to the Evacuation Center sign), "Watch the big waves" (the Student walks toward the sea wall and stops well back from it), "Swim before the storm" (the Student walks toward the steps to the sand and stops at the top) and "Pick up shells on the sand" (the Student walks toward the steps, plays `crouch` and stops at the top, apart from the swim stop) (TY-10, TY-34, TY-50, TY-51). This ticket adds `coast` with its action keys and placements in `ArtSource/README-rollout.md`, the shells, the radio on the road side, and one icon per Choice (reusing `icon_watch_waves` and `icon_high_ground` only where the picture fits).

**Blocked by:** 08 (the Hard text approved there).

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `coast` is a known Setting with its action keys, and it never shakes. It reads as a Storm Surge coast, not as the Earthquake `beach` with the sea pulled back.
- The Safe Choice completes Hard 2 with "All ready!". Each Unsafe Choice stops on the road, well back from the sea wall and above the steps, and resets on Try Again, with Dad back at his start.
- No wave or spray comes within 40 art pixels of the Student or Dad.
- Earthquake Normal 5 on the `beach` plays as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/09-coast-hard-2/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
