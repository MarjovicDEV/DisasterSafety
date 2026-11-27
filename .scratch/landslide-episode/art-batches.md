# Landslide art batches

Art for the Landslide Episode (docs/design/landslide-episode.md), drawn by Codex with the Aseprite Lua scripts in `ArtSource/scripts/` and approved batch by batch in its Setting before import. The full Codex prompt is `.scratch/landslide-episode/codex-art-prompt.md`; it overrides this summary where they differ. Follow the Flood workflow in `.scratch/flood-episode/codex-art-prompt.md` ("Hard rules", "How to work") with these changes:

- **Slopes, not water.** The Slope, Slide Signs and Fallen Soil are the new pictures. Fallen Soil is always still; the only moving soil is the far slide in Hard 2 (at most 48x40 art pixels, 6 frames, far plane, then still; docs/adr/0006-landslide-safety-rules.md). Every actor start, stop and ending is on flat ground at least 24 art pixels from the Slope's foot, any crack, the spring, Muddy Runoff, Fallen Soil and the rope, checked by each batch's `review.py`.
- **Rain and Dark Sky are reused from Typhoon**, with a new Dark Sky tint per new Setting. No Wind, no lightning, no rumble, no rescuers, nobody in danger.
- **Scratch folder:** `D:/GameDEV/landslide-episode-scratch/art/<nn-batch>/`. Render with `out=` there first; each batch has `preview.png`, a placement sheet per Level with the 2400x1080 crop and the Caption, Hint and Pause boxes, `phone-size.png`, `motion.gif` for anything that moves, `placements.md` and `review.py`.
- **Gates:** every new part sits behind its own `only=` part in `ArtSource/scripts/build.lua`, checked with `only.<part>`, so a full build without `only=` writes nothing new.
- **Import** only after approval: rerun without `out=` (keep `only=`), check `git status` shows only new art plus the scripts, and add `## Landslide Batch <n> — <name> (approved)` to `ArtSource/README-rollout.md`.

| # | Batch | Levels | Parts | New pieces |
|---|---|---|---|---|
| 1 | `hillside` and its signs | E1, N1, H4 | `only=landslide_hillside`, `only=icons_landslide_signs` | `bg_hillside`, `landslide_dark_sky_hillside`, `hillside_crack`, `hillside_spring_0` to `_3`, `hillside_soil`, `hillside_rope`, `hillside_sign`, `hillside_toy_soil`; 7 icons. |
| 2 | `hillside` habits, rain and after | E2, E3, N2, N5, H1, H5, H7 | `only=landslide_hillside_more`, `only=grownups_landslide`, `only=icons_landslide_hillside` | garden box and pot, leaning tree, wet Slope, shovel, pail, banana leaf (optional); `dad_shovel`; layouts for H1 and H7; about 20 icons. |
| 3 | `hill_road` | N4, H2 | `only=hill_road`, `only=icons_landslide_road` | `bg_hill_road`, `landslide_dark_sky_hill_road`, Muddy Runoff (4 frames), `hill_road_soil`, the far slide (6 frames); about 7 icons. |
| 4 | Home, classroom and Evacuation Center | H6, N3, H3 | `only=landslide_home`, `only=icons_landslide_indoor` | `home_wall_crack`, `home_poster`, `home_window_hill`; layouts for N3 (`classroom`) and H3 (`evacuation_area`); about 11 icons. |
| 5 | Menu backgrounds | menus | `only=menu_landslide` | Landslide Episode background and three Difficulty backgrounds. Ask first: design not settled. |
| 6 | Safety Reminder | end of game | `only=safety_reminder` | One 480x270 background for the end-of-game page. Ask first: design not settled. |
