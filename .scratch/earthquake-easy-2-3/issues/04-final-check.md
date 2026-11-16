# 04 Final check before review

Blocked by: 03.

## Do

1. Run the four batch commands in `AGENTS.md`, in order: build, EditMode, PlayMode, screenshots.
2. Read every screenshot from `10-level` to `32-level-completed-normal-open` at both sizes. Fix any overlap, missing picture or wrong Setting by changing the builder, then run step 1 again.
3. Check with `git diff main --stat` that the diff touches no file under `Assets/Art`, `Assets/Audio`, `ArtSource` or `Assets/Resources/Scenarios`. The only exceptions are `.meta` files that Unity created for the new art and sound.
4. Report:
   - the EditMode and PlayMode totals (passed and failed);
   - the list of new tests;
   - anything in the spec that you could not build.

## Done when

The build and both test runs are green, all screenshots are read, and the report is written. A Claude code review of this branch against `main` comes next.
