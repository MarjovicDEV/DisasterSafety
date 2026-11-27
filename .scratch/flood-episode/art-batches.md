# Flood art batches

## Batch 2 addition — keeping school things dry

- Easy 1 (FL-04). Show the edge of a workbook in the school bag. The Student carries the bag to the high shelf while Mom moves a box. The workbook stays visible in the ending.
- Normal 5 (FL-04, FL-12). On the Safe Choice branch, the Student can put the bag and workbook on the shelf on the way to sit beside Mom. Stage the bag, shelf and route on dry floor away from the closed front door. Preview this as an optional Act-out step for the gameplay agent.
- Hard 1 (FL-08, TY-09). Reuse the shelf with the school things already up. The evacuation Act-out starts immediately and goes straight to Mom and Dad.

Art for the Flood Episode (docs/design/flood-episode.md), drawn by Codex with the Aseprite Lua scripts in `ArtSource/scripts/` and approved batch by batch in its Setting before import. The full Codex prompt is `.scratch/flood-episode/codex-art-prompt.md`; it overrides this summary where they differ. Follow the Typhoon workflow in `.scratch/typhoon-episode/codex-art-prompt.md` ("Hard rules", "How to work") with these changes:

- **Floodwater, not storm.** Floodwater is muddy brown-grey with a pale grey-blue sheen (`flood_fx.lua`, `WATER`). Indoors it only seeps in under the closed front door (F2). Outdoors it may cover the yard, the road or a path. No person is ever drawn standing in water, and every actor stop is at least 24 art pixels from the nearest water pixel (checked by each batch's `review.py`).
- **No Wind and no Wind Signal.** Rain and Dark Sky are reused from Typhoon. No lightning, no rescuers, no boats, nobody in danger.
- **Scratch folder:** `D:/GameDEV/flood-episode-scratch/art/<nn-batch>/`. Render with `out=` there first; each batch has `preview.png`, a placement sheet with the 2400x1080 crop and the Caption, Hint and Pause boxes, `phone-size.png`, `motion.gif` for anything that moves, `placements.md` and `review.py`.
- **Import** only after approval: rerun without `out=` (keep `only=`), check `git status` shows only new art plus the scripts, and add `## Flood Batch <n> — <name> (approved)` to `ArtSource/README-rollout.md`.

| # | Batch | Levels | Part | New pieces |
|---|---|---|---|---|
| 1 | Floodwater at home | N2 | `only=flood_home` | `home_flood_seep_1` to `_3`, `home_fan`, `home_outlet_low`, `home_window_flood` (low water). Drawn; waiting for approval. |
| 2 | Home before and during | E1, N3, N5, H1 | `only=flood_home_more` | `home_window_flood_high` (H1), TV Red Rainfall Warning screen (the Orange warning is the Teacher's speech bubble, F4), boiled-water jug and cup, the bag on a high shelf spot, toy and game props reused. |
| 3 | Outdoor Floodwater | N1, N4, H2 | `only=flood_outdoor` | still shallow water for `yard`, moving water for the `schoolyard` gate road and the `street_storm` road, the flooded bridge, the floating ball. |
| 4 | Creek | H3 | `only=creek` | new Setting `bg_creek`, high fast creek water, bank, path home, stones. |
| 5 | Easy props | E2, E3 | `only=flood_easy` | canal grate, trash bin, wrapper (`street`); can and tire with rainwater, upright and tipped (`yard`); `tip` pose if `crouch` cannot show it. |
| 6 | After the flood | H4, H5, H6, H7 | `only=flood_after` | basin and dipper, `wash` pose; mud layer for `home_after` and `street_after`; wet bread and the low cupboard; TV off and switch box states. |
| 7 | Menu backgrounds | menus | `only=menu_flood` | Flood Episode background and three Difficulty backgrounds (design not settled). |
| 8 | Choice icons | all | `only=icons_flood_*` | about 40 `icon_flood_*`, reusing existing icons where they fit. |
