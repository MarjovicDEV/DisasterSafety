# 13 Main Menu and How to Play with the Student

Blocked by: 01. Blocks: 14.

Spec: `docs/specs/earthquake-rollout.md`. Design: `docs/design/gameplay-redesign.md` (R1 to R11 and the Act-out tables). Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`. Branch: `earthquake-rollout` in `D:\GameDEV\DisasterSafety-redesign`.

## Build (test first)

- Main Menu: the Learner's Student (boy or girl from the save; the boy until a pick exists) replaces `BantayGuide`, with the same greetings and Tips in the Student's speech bubble and the voice blip. Rename the `UIStrings` keys that start with `Bantay` to Student keys without changing their text.
- How to Play: the Student gives the Tips, and the Tutorial Pages show the Action Bar flow (the Action Bar, a Choice being acted out, Feedback, the Hint in the Caption, Stars). New Tutorial Page pictures are drawn by a new script; no existing `tutorial.lua` output is rewritten.
- Bantay's files stay until ticket 15; after this ticket only the card flow references them.
- PlayMode `MenuFlowPlayTests`: the Main Menu and How to Play show the Student, not Bantay, for both kinds.
- Screenshots: the Main Menu and every How to Play page at both sizes; read them.

## Done when

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures.
- The menu and How to Play screenshots read clearly; `git status` shows only new files under `Assets/Art` and `ArtSource` (edited generator scripts excepted).
- Nothing is committed until the user asks; then ask how many commits (one simple sentence each, written as a history entry, no mention of the user, no co-author line).
