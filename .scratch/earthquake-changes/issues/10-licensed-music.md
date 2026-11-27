# 10 Play licensed music by Episode and Difficulty

Changes: C2.

Blocked by: [15-cleanup.md](../../earthquake-rollout/issues/15-cleanup.md).

Blocks: None.

Researcher input: The researcher chose ten recordings on 2026-11-26. They are in `D:/GameDEV/earthquake-changes-scratch/10-licensed-music/source/`, and `tracks.md` beside them lists each title, source page, licence, length and needed edit.

Read the [approved specification](../../../docs/specs/earthquake-changes.md), [design decisions](../../../docs/design/gameplay-redesign.md#earthquake-changes-after-the-rollout) and [project rules](../../../AGENTS.md). Work on `earthquake-rollout` in `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1.

## Files

Assets/Audio/Music/ and its LICENSES.md, Assets/Resources/AudioLibrary.asset, Assets/Scripts/Game/Audio/{AudioLibrary,AudioManager}.cs, Assets/Scripts/Game/UI/LevelController.cs, Assets/Scripts/Editor/ProjectBuilder.cs, ArtSource/audio/make_audio.py, Assets/Tests/EditMode/AudioPreferencesTests.cs and Assets/Tests/PlayMode/{LevelFlowPlayTests,MenuFlowPlayTests}.cs. Add accessible offline credit text and menu wiring when a selected licence requires it.

## Tests to write first

First assert that an Earthquake Easy, Normal and Hard Level each play their own Set A gameplay track, and that opening Level Completed switches to the Set A cue, which plays once. Extend the existing menu/Level transition and music preference tests for shaking and recovery, mute, completion, remaining on results, Replay and return to Menu. Check the track for every Episode and Difficulty, both menu tracks taking turns, looping modes, independent sound effects, source formats and any required in-game credits. Record a red run that fails on an assertion before implementing the behavior.

## Build

Use the ten recordings in `tracks.md` and ADR 0003. Set A plays in Earthquake, Typhoon and Landslide: Cherry Blossom on Easy, Happy Alley on Normal, Prelude and Action on Hard and Light Sting on Level Completed. Set B plays in Fire and Flood: Cheery Monday on Easy, B-Roll on Normal, Machinations on Hard and Piano Between on Level Completed. The menus play Somewhere Sunny and Easy Lemon (60 second) in turn, one after the other. Map each Episode to its set in one place, so Fire, Typhoon, Flood and Landslide get their music when they become Playable. Document each track in Assets/Audio/Music/LICENSES.md. Provide required CC BY 4.0 credits, links and edits in accessible offline credits. CC0 credit is optional. The sources are MP3, since incompetech offers no lossless version. Prepare 44.1 kHz WAVs with matched loudness near −20 LUFS integrated and true peaks at or below −1 dBTP. Use natural-sounding instruments. Loop each menu and gameplay track after 45–90 seconds, trimming Cherry Blossom, Prelude and Action and Machinations to 90 seconds or less. Play each Level Completed cue once for 10–20 seconds, trimming Piano Between with a fade. List every trim and fade in the credits. Switch on panel transitions as well as scene changes. Respect preferences, existing fades and shake ducking. Keep generated audio jobs from replacing licensed music.

Reuse existing graphics where possible. If a new or revised graphic is needed, render it to scratch and show the researcher a preview sheet. Obtain researcher approval before importing it into the project. Follow [ADR 0001](../../../docs/adr/0001-v2-art-and-no-bantay.md) for the outline, three shades, light from the left, shared palette and pixel scale.

## Screenshots

Capture the surviving Student flow at **1920 × 1080** and **2400 × 1080**. Compare against a fresh baseline immediately before this ticket, after cleanup and any completed prerequisites. Store logs, previews, captures and comparisons in a ticket-specific folder under `D:/GameDEV/earthquake-changes-scratch/`.

Audio alone must leave all menu and Level screenshots byte-identical. If CC BY requires a credits entry point, allow only the specific menu panel containing that entry point and the new credits view. Capture credits at both sizes, verify offline access, and protect all gameplay, Feedback and completion captures. Every existing screenshot outside the allowed visible changes must remain byte-identical, including every untouched Level. Record the filenames of changed captures.

## Done when

- Listen on Android for clean loops, clear rumble and speech blips, a single completion cue and correct music after Replay. Run BuildAndroidBatch, record the exact APK byte count against 50,000,000 bytes, and record the measured loudness and peaks. The earlier 44,848,235-byte build is the comparison point. The researcher checks the revised build, including Student readability on a six-inch phone when available.
- Close the Editor before batch runs. After implementing the change, run `BuildAllBatch`, then the full EditMode and PlayMode suites and the screenshot batch. The build succeeds, both suites have zero failures, and the screenshot comparison passes. Inspect the affected screens at both sizes.
- Commit only when the researcher asks. Ask how to split this ticket into **6 to 9 commits**. Each message is one sentence in basic English describing what was done, with no user mention, co-author line or other attribution.
