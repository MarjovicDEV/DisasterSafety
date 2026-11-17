# 02 Student pick: boy or girl, saved with the Display Name

Blocked by: 01. Blocks: 03.

Spec: `docs/specs/gameplay-redesign-pilot.md` (behaviour 2, Data, Tests). Rules: `AGENTS.md`.

## Build (test first)

- EditMode `ProgressStateTests`: a new save has `StudentKind.None`; `SetStudent(StudentKind.Girl)` saves, and a reload reads `Girl`; an old save file without `student` loads as `None` with nothing else lost.
- `Core`: `StudentKind` enum, `SaveData.student`, `ProgressState.Student` and `SetStudent`. No MonoBehaviour in Core.
- Main Menu: after a valid Display Name, show the picker panel with the two `Student/*_portrait` sprites and the "Boy" and "Girl" buttons under the title "Who are you?". A pick saves and continues as today. A save with a Display Name and no pick shows the picker once at the next Main Menu.
- `UIStrings`: "Who are you?", "Boy", "Girl".
- `ProjectBuilder.cs`: build the picker panel; `MenuScreenshots`: add `66-student-picker`.
- PlayMode `MenuFlowPlayTests`: a new Display Name leads to the picker; picking the girl saves `Girl`.

## Done when

Build, EditMode and PlayMode pass with zero failures, `66-student-picker` reads well at both sizes, every other menu screenshot is unchanged, and the work is one commit (one sentence, no co-author line).
