# 02: Typhoon Episode and Difficulty backgrounds, and Next Episode from Fire

**What to build:** Typhoon's Difficulty selection shows its Episode background, the barangay hall under a grey sky with a "SIGNAL NO. 2" board and a flag bending in the wind, in place of the noticeboard. Typhoon's Level selection shows its own Difficulty backgrounds: a home doorway with the packed Go Bag and the radio under a grey sky for Easy, the classroom windows with rain streaming down for Normal, and a school gym used as an Evacuation Center at night in the rain, lights on and an "EVACUATION CENTER" sign, for Hard. `MenuBackground` gains `BarangayHall`, `GoBagDoorway`, `RainyWindows` and `EvacuationGym`. Typhoon Levels are listed as Playable as soon as their Scenarios pass the rules. Level Completed's Next Episode becomes one rule for every Episode: after the last Hard Level it shows when the next Episode is Open and has a Playable Level. So Fire Hard 7 leads into Typhoon, Earthquake Hard 7 still leads into Fire, and Typhoon Hard 7 shows no Next Episode while Flood has none. Music Set A already plays in Typhoon.

**Blocked by:** None among the Typhoon tickets (it can run beside 01). Fire tickets 02 (Fire's `MenuBackground` entries in the same switch) and 12 (Fire Hard 7) must be built first.

**Status:** ready-for-agent

Read the [Typhoon specification](../../../docs/specs/typhoon-episode.md), the [Typhoon design](../../../docs/design/typhoon-episode.md), [ADR 0005](../../../docs/adr/0005-typhoon-safety-rules.md), the [Typhoon research](../../../docs/research/typhoon-safety.md) and the [project rules](../../../AGENTS.md). Terms follow [CONTEXT.md](../../../CONTEXT.md). Work on `main` in `D:/GameDEV/DisasterSafety` with Unity 6000.6.9f1 and the Editor closed for batch runs; the batch commands with this folder are at the end of the specification (`AGENTS.md` still names the old `DisasterSafety-redesign` folder).

## Done when

- `MenuNavigatorTests` choose the Typhoon Episode background on Difficulty selection and the three Typhoon Difficulty backgrounds on Level selection. Earthquake's and Fire's backgrounds stay unchanged.
- `LevelCompletionTests`: Fire Hard 7 offers Next Episode into Typhoon once Typhoon has a Playable Level, Typhoon Hard 7 offers none while Flood has none, and Earthquake Hard 7 still offers Fire.
- The menu screenshots include Typhoon Difficulty selection, each Typhoon Level selection and Fire Hard 7's Level Completed with Next Episode, at both sizes. The signs stay readable when the top 54 rows are cropped.
- Entering a Typhoon Level still plays its Set A track, and returning to the menus restores the menu music.
- Tests were written first, and a red run failed on an assertion before the behaviour was built.
- Every new Setting, pose, Grown-up, prop and icon follows [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) and the v2 art bar (480 x 270 backgrounds, Student 32 x 50, Grown-ups about 36 x 62, 16 x 16 icons), drawn from `ArtSource/`. A preview sheet beside the pilot classroom and Student was rendered to scratch and approved by the researcher before import. A missing picture or sound is named in the evidence and asked of the user.
- Every Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line cites a TY finding where it states safety, and the researcher approved the wording before the Scenario files were committed.
- No Unsafe Act-out reaches a wire, wave, flame, open doorway to the storm or broken thing, and nobody is shown hurt. Nothing heavier than a leaf moves through the air, and nothing passes within 40 art pixels of a person.
- `BuildAllBatch`, the full EditMode and PlayMode suites and the screenshot batch pass. Affected Levels are checked at 1920 x 1080 and 2400 x 1080, clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical. Evidence is stored in `D:/GameDEV/typhoon-episode-scratch/02-typhoon-menus/`.
- `ProjectSettings/ProjectSettings.asset`, `.vscode/settings.json` and untracked screenshots stay out of commits. Commit only when the user asks, after asking how many commits. Each message is one sentence in basic English with no co-author line or other attribution.
