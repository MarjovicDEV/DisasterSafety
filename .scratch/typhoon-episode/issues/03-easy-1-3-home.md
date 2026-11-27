# 03: Easy 1 Signal No. 1 news and Easy 3 flashlight in the Go Bag

**What to build:** Typhoon Easy 1 and Easy 3 play as "before" Scenarios in the `home` living room with Dark Sky on the window from the first frame; they start still and show the Action Bar after 1 second. Easy 1: the TV screen prop reads "SIGNAL NO. 1". The Choices are "Tell Mom the news" (Safe: the Student walks to Mom and calls "Mom, a typhoon!"; Mom plays `happy` and turns the radio on) and "Change to cartoons" (the Student walks to the TV, reaches toward it and stops) (TY-01, TY-22, TY-31, TY-35). Easy 3: Mom is packing the open Go Bag and the flashlight lies on the floor. The Choices are "Put the flashlight in the Go Bag" (Safe: the Student picks up the flashlight, walks to the bag and packs it in; the bag closes) and "Play with the flashlight" (the Student picks it up, plays the new `flashlight` pose and stops) (TY-23, TY-24, TY-27). This ticket adds the Student's `flashlight` pose (front, a lit flashlight with a short beam, 2 frames, boy and girl), the TV "SIGNAL NO. 1" and "SIGNAL NO. 2" screens, new `home` action keys and one icon per Choice (reusing `icon_go_bag` only if its picture fits "Put the flashlight in the Go Bag"). It drafts the text of all three Easy Levels for the researcher.

**Blocked by:** 01.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- The `flashlight` pose exists for the boy and the girl and plays in Easy 3's Unsafe stop.
- The drafted text for Easy 1 to Easy 3 is in the evidence folder with a TY ID on every safety line, and the researcher approved it before these Scenario files were committed.
- Each Level's Safe Choice completes it with "All ready!". Each Unsafe Choice stops on clear floor and resets on Try Again: the flashlight returns to the floor, the Go Bag reopens, and the TV screen and the radio return to their start.
- The content tests check two Choices per Easy Level and Typhoon's new `home` keys. Earthquake's and Fire's `home` Act-outs play as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/03-easy-1-3-home/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
