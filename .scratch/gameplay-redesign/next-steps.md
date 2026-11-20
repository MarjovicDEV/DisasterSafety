# Gameplay redesign: where we stopped (2026-11-18)

Worktree `D:\GameDEV\DisasterSafety-redesign`, branch `gameplay-redesign`, 4 commits ahead of `main`, not pushed.

| Ticket | State |
|---|---|
| 01 v2 art | done, `d4a1a2d` |
| 02 Student pick | done, `efe3892` |
| 03 pilot Level flow (Easy 1) | done, `04a3fa9` |
| 04 Android pilot build | in progress: APK built, phone check not done |

## Ticket 04, what is done

- `ProjectBuilder.BuildAndroidBatch` added (`Assets/Scripts/Editor/ProjectBuilder.cs`) and its batch line added to `AGENTS.md`. Both are **uncommitted**.
- APK built: `Builds/DisasterSafety-pilot.apk`, 44.5 MB, ARM64, release, bundle id `com.DefaultCompany.2D-URP`. `Builds/` is ignored by git.

## Next, in order

1. **Play Easy 1** on the phone (install the APK) and/or in the Unity Editor (open `D:\GameDEV\DisasterSafety-redesign` in Unity Hub with 6000.6.8f1, open `Assets/Scenes/Boot.unity`, Game view 1920x1080, Play). Answer:
   1. Is the Student easy to see, and are the poses clear (idle, "Whoa!", walk, duck, "Hmm, wait...", happy)?
   2. Are the Action Bar buttons easy to read and tap?
   3. Does the shake feel right, not scary?
   4. Is the Caption readable?
   5. Anything slow, confusing or broken?
2. **Write the answers** into `docs/design/gameplay-redesign.md` under a new "Pilot result" section, with the decisions (go on with the rollout, or what to change first), and say where each answer came from (phone or Editor).
3. **Decide on the leftovers** in this worktree (not committed by tickets 02-04): rebuilt scenes `Boot.unity`, `Level.unity`, `MainMenu.unity`; `Assets/Settings/UniversalRP.asset`, `UniversalRenderPipelineGlobalSettings.asset`; `ProjectSettings/*`; `.vscode/settings.json`; `Screenshots/*` (new 06-howtoplay pages and 10-67 shots). Commit the build output you want, revert the rest.
4. **Commit ticket 04**: `BuildAndroidBatch`, the `AGENTS.md` line and the Pilot result. Ask how many commits; one-sentence messages, no co-author line.
5. **If the pilot is OK, merge `gameplay-redesign` into `main`** in `D:\GameDEV\DisasterSafety` (fast-forward). First commit or revert the uncommitted changes there (`Boot.unity`, `Level.unity`, `MainMenu.unity`, two `06-howtoplay` screenshots), or the merge may be refused. If the pilot shows problems, fix them on `gameplay-redesign` first and build a new APK.
6. **Push** to `origin` only when the user says so.
7. **Rollout**: a new spec and tickets (`/to-spec`, `/to-tickets`) to move the other 14 Earthquake Levels to the Student flow, on a new branch from `main`. Open questions from the design: chain scoring for Normal/Hard, and voice blips for Reactions.

Close the Unity Editor before any batch run.
