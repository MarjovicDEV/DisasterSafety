# 04 Android pilot build and the phone check

Blocked by: 03.

Spec: `docs/specs/gameplay-redesign-pilot.md` (Code: `BuildAndroidBatch`; Acceptance criteria). Rules: `AGENTS.md`.

## Build

- `ProjectBuilder.BuildAndroidBatch`: run `BuildAll`, then build `Builds/DisasterSafety-pilot.apk` for Android (landscape, the existing bundle id, development build off). Add `Builds/` to `.gitignore`.
- Add the batch command to `AGENTS.md` next to the others.
- Run it (Unity closed) and confirm the APK exists and its size.

## The phone check (the user)

Give the user short steps to install the APK (copy it to the phone and allow installs from this source, or `adb install`), then a checklist to judge on the device:

- Is the Student easy to see and are the poses clear (idle, whoa, walk, duck, wait, happy)?
- Are the Action Bar buttons easy to read and tap?
- Does the shake feel right, not scary?
- Is the Caption readable?
- Anything that feels slow or confusing.

## Done when

The APK is built, the user has played Easy 1 on their phone, and their answers are written into `docs/design/gameplay-redesign.md` under a new "Pilot result" section, with the decisions they lead to (go on with the rollout, or what to change first).
