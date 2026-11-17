# 03 Final check before review

Blocked by: 02.

## Do

1. Run the four batch commands in `AGENTS.md`, in order: build, EditMode, PlayMode, screenshots.
2. Read every Level screenshot (10 to 58) at both sizes. Fix any overlap, cut-off text, missing picture or wrong Setting by changing the builder, then run step 1 again.
3. Check with `git diff <design commit> --stat` that no PNG, `.aseprite`, audio or Scenario file changed after the first commit on this branch (the one that added the Hard design, art and Scenarios), apart from new `.meta` files.
4. Report:
   - the EditMode and PlayMode totals;
   - the tests added on this branch;
   - anything in the spec you could not build.

## Done when

The build and both test runs are green, all screenshots are read, and the report is written. A Claude code review of this branch against `main` comes next.
