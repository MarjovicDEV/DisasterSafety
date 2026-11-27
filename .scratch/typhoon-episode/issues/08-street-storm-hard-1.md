# 08: The `street_storm` Setting and Hard 1 the evacuation route

**What to build:** A new Setting, `street_storm`: the Fire barangay `street` redrawn with the shutters closed, no food vendor, an evacuation route sign and a path toward the creek, at the v2 bar, never shaking. Typhoon Hard 1 plays there as a "before" Scenario with light Rain and Dark Sky from the first frame; it starts still and shows the Action Bar after 1 second. The Caption tells that the Barangay Tanod says families near the creek must go to the Evacuation Center now; the Tanod stands in his megaphone pose, and Mom holds the Go Bag. The Choices are "Walk with Mom on the evacuation route" (Safe: the Student walks with Mom behind the Tanod to the evacuation route sign), "Run back for your toy" (the Student turns, walks back toward the house and stops), "Take the shortcut by the creek" (the Student walks toward the creek path and stops at its start) and "Stay home, our house is strong" (the Student walks back toward the door and stops at a different point from the toy stop) (TY-21, TY-47, TY-51, TY-52, TY-53). This ticket adds `street_storm` with its action keys and placements in `ArtSource/README-rollout.md`, the evacuation route sign, Mom with the Go Bag, and one icon per Choice. It drafts the text of all seven Hard Levels for the researcher.

**Blocked by:** 07. Fire ticket 12 (the `street` Setting it redraws) must be built first.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `street_storm` is a known Setting with its action keys, and it never shakes.
- The drafted text for Hard 1 to Hard 7 is in the evidence folder with a TY ID on every safety line, and the researcher approved it before the Hard 1 Scenario file was committed.
- The Safe Choice completes Hard 1 with "All ready!". Each Unsafe Choice stops on clear ground, short of the creek path and the house door, and resets on Try Again, with Mom and the Tanod back at their start.
- The content tests check four Choices per Hard Level. Fire Hard 6 and 7 on `street` and `street_after` play as before.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/08-street-storm-hard-1/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
