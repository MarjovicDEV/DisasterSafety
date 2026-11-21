# 14 The Hard check, the APK and the phone check

Blocked by: 05, 09, 10, 11, 12, 13. Blocks: 15.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build

- EditMode `ScenarioTests`: all 15 Earthquake Levels use the Student flow.
- Run the full screenshot batch and read every Level, menu and How to Play shot at both sizes.

## The Hard check (the user)

Ask the user to play Hard 1 to 7 in the Unity Editor. Write the answers into the design doc under "Rollout checks", and fix what the user asks for.

## The APK and the phone check (the user)

- Run `BuildAndroidBatch` (Unity closed) and confirm that `Builds/DisasterSafety-pilot.apk` exists, with its size.
- Give the user short install steps (copy the APK to the phone and allow installs from this source, or `adb install`) and a checklist: are the Student, the Grown-ups and the props easy to see; are the Action Bar buttons easy to read and tap; does every Act-out read as the real drill; does the shake feel right and not scary; is the Caption readable; is anything slow or confusing.
- Write the answers into the design doc under "Rollout checks", and close the "Not verified" items the phone answers.

## Done when

The user has approved all three Difficulties in the Editor and the APK on the phone, and the answers are recorded. Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
