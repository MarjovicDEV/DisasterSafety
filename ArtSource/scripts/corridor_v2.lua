-- The v2 corridor (480x270, shown at 4x = 1920x1080) for Earthquake Easy 2, in the v2 style of
-- docs/adr/0001-v2-art-and-no-bantay.md and the same ramps as classroom_v2.lua: one dark outline
-- around each object, three shades per colour with light from the left. The Student's feet stand
-- on y = 230, as in the classroom.
-- Left to right: the hall window, the start spot, the plain inner wall (where the Student takes
-- cover), and the stairs going down on the right. Placements (top-left, art pixels) are in
-- M.LAYOUT and ArtSource/README-rollout.md.

return function(L, ST)
  local M = {}
  local O = ST.KEY.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end
  -- Ramps: light, base, dark (the classroom's, so both Settings share one palette)
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    lower = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    floor = { rgb(214, 198, 168), rgb(194, 176, 146), rgb(166, 148, 118) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    paper = { ST.KEY.W, ST.KEY.w, ST.KEY.v },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    step = { rgb(226, 220, 206), rgb(200, 192, 176), rgb(160, 150, 134) },
  }
  local CEIL = rgb(232, 216, 178)

  M.LAYOUT = {
    floorY = 230,
    window = { 84, 66, 92, 92 },
    innerWall = { 240, 15, 92, 192 },
    stairsX = 404,             -- the top step starts here
    start = 214,
    -- Act-out points (the Student's centre x): the cover spot at the inner wall, and the stops
    spots = { cover = 286, stairs = 394, window = 190 },
  }

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end

  -- A hall window: two jalousie panes with the sky, a hill and a tree beyond.
  local function hallWindow(img, x, y, w, h)
    box(img, x, y, w, h, R.wood)
    L.rect(img, x + 4, y + 4, w - 8, h - 8, O)
    L.rect(img, x + 5, y + 5, w - 10, h - 10, R.glass[1])
    L.rect(img, x + 5, y + h - 22, w - 10, 17, R.leaf[2])
    L.rect(img, x + 5, y + h - 22, w - 10, 2, R.leaf[1])
    -- a tree beyond the glass
    L.rect(img, x + 22, y + h - 30, 3, 10, R.wood[3])
    L.circle(img, x + 23, y + h - 34, 7, R.leaf[3])
    L.circle(img, x + 22, y + h - 35, 5, R.leaf[2])
    for sy = y + 8, y + h - 8, 7 do
      L.hline(img, x + 5, x + w - 6, sy, R.glass[3])
      L.hline(img, x + 5, x + w - 6, sy - 1, R.glass[2])
    end
    L.rect(img, x + w // 2 - 2, y + 4, 4, h - 8, O)
    L.rect(img, x + w // 2 - 1, y + 4, 2, h - 8, R.wood[2])
    box(img, x - 4, y + h - 2, w + 8, 6, R.wood)
  end

  function M.background()
    local W, H = 480, 270
    local img = L.newImage(W, H)
    local Y = M.LAYOUT.floorY
    -- ceiling, wall, lower wall, baseboard, floor (as in the classroom)
    L.rect(img, 0, 0, W, 14, CEIL)
    L.hline(img, 0, W - 1, 14, O)
    L.rect(img, 0, 15, W, 150, R.wall[2])
    L.rect(img, 0, 15, W, 2, R.wall[1])
    L.rect(img, 0, 165, W, 38, R.lower[2])
    L.hline(img, 0, W - 1, 165, R.lower[1])
    L.hline(img, 0, W - 1, 164, R.wall[3])
    L.rect(img, 0, 203, W, 4, R.wood[3])
    L.hline(img, 0, W - 1, 207, O)
    -- A continuous concrete floor, with a contact shadow at the baseboard and sparse texture.
    L.rect(img, 0, 208, W, H - 208, R.floor[2])
    L.hline(img, 0, W - 1, 208, R.floor[3])
    L.poly(img, { 83, 209, 177, 209, 206, 232, 56, 232 }, R.floor[1])
    for _, p in ipairs({
      { 20, 241 }, { 43, 219 }, { 61, 258 }, { 78, 246 }, { 94, 265 },
      { 112, 237 }, { 128, 257 }, { 146, 243 }, { 152, 219 }, { 166, 267 },
      { 177, 250 }, { 193, 262 }, { 213, 241 }, { 235, 218 }, { 252, 254 },
      { 269, 242 }, { 283, 266 }, { 301, 221 }, { 309, 251 }, { 336, 263 },
      { 349, 238 }, { 370, 256 }, { 380, 221 }, { 395, 249 },
    }) do L.px(img, p[1], p[2], R.floor[3]) end
    for _, p in ipairs({
      { 12, 252 }, { 34, 233 }, { 55, 267 }, { 101, 254 }, { 127, 244 },
      { 167, 236 }, { 187, 252 }, { 216, 264 }, { 229, 224 }, { 246, 241 },
      { 276, 255 }, { 296, 235 }, { 321, 263 }, { 326, 242 }, { 354, 249 },
      { 374, 266 }, { 388, 235 },
    }) do L.px(img, p[1], p[2], R.floor[1]) end
    -- ceiling lights
    for _, lx in ipairs({ 120, 300 }) do
      box(img, lx, 14, 40, 5, R.paper)
    end

    -- the hall window
    local w = M.LAYOUT.window
    hallWindow(img, w[1], w[2], w[3], w[4])

    -- the plain inner wall: a wide pillar standing out a little from the wall, with nothing on it
    local iw = M.LAYOUT.innerWall
    local ix, iy, iwW, iwH = iw[1], iw[2], iw[3], iw[4]
    L.rect(img, ix, iy, iwW, 150, R.wall[2])
    L.rect(img, ix, iy + 150, iwW, 38, R.lower[2])
    L.vline(img, ix, iy, iy + iwH - 1, O)
    L.vline(img, ix + iwW - 1, iy, iy + iwH - 1, O)
    L.rect(img, ix + 1, iy, 4, 150, R.wall[1])                 -- lit left face
    L.rect(img, ix + iwW - 6, iy, 5, 150, R.wall[3])           -- shaded right face
    L.vline(img, ix + 5, iy, iy + 149, R.wall[2])
    L.rect(img, ix + 1, iy + 150, 4, 38, R.lower[1])
    L.rect(img, ix + iwW - 6, iy + 150, 5, 38, R.lower[3])
    L.hline(img, ix + 1, ix + iwW - 2, iy + 150, R.lower[1])
    L.vline(img, ix + 5, iy, iy + 187, O)
    L.vline(img, ix + iwW - 7, iy, iy + 187, O)
    L.rect(img, ix + 1, iy + 188, iwW - 2, 4, R.wood[3])
    L.hline(img, ix, ix + iwW - 1, iy + 192, O)
    -- its shadow on the wall to the right (light from the left)
    L.rect(img, ix + iwW, iy, 4, 150, R.wall[3])
    L.rect(img, ix + iwW, iy + 150, 4, 38, R.lower[3])

    -- the Grade 4 classroom door behind the start spot, with its room sign
    box(img, 184, 112, 50, 92, R.wood)
    box(img, 189, 117, 40, 87, R.wood)
    box(img, 195, 124, 28, 20, R.glass)
    L.line(img, 199, 140, 207, 128, R.glass[1])
    box(img, 195, 152, 28, 16, R.wood)
    box(img, 195, 174, 28, 22, R.wood)
    box(img, 224, 158, 5, 5, { R.pot[1], rgb(252, 209, 22), R.wood[3] })
    box(img, 191, 96, 36, 13, R.paper)
    L.text(img, 197, 99, "Gr. 4", ST.KEY.N, 1)

    -- a bulletin board between the inner wall and the stairs
    box(img, 346, 68, 52, 44, R.wood)
    L.rect(img, 349, 71, 46, 38, rgb(198, 150, 96))
    L.vline(img, 349, 71, 108, rgb(222, 178, 120))
    box(img, 353, 75, 16, 20, R.paper)
    for yy = 79, 91, 3 do L.hline(img, 356, 365, yy, R.metal[3]) end
    box(img, 373, 78, 18, 14, { rgb(252, 209, 22), rgb(252, 209, 22), R.wood[1] })
    box(img, 372, 95, 20, 11, R.paper)
    for _, p in ipairs({ { 360, 75 }, { 381, 78 }, { 381, 95 } }) do L.px(img, p[1], p[2], rgb(206, 17, 38)) end

    -- Receding edges lean toward the centre of the corridor. The back of the
    -- landing reaches the baseboard, and the front meets the play floor at (404, 230).
    local frontX, frontY = M.LAYOUT.stairsX, Y
    local backX, backY = frontX - 9, 208
    local frontRun, frontDrop = 16, 6
    local backRun, backDrop = 14, 5
    local backEndY = backY + (W - 1 - backX) * backDrop // backRun

    -- The cutaway face of the floor and the solid stairwell wall behind the treads.
    L.poly(img, { backX, backY, W, backY, W, H, frontX, H, frontX, frontY }, R.step[3])
    L.vline(img, frontX - 1, frontY, H - 1, R.floor[3])
    local wallHeight = 22
    L.poly(img, { backX, backY - wallHeight, W, backEndY - wallHeight + 1,
                  W, backEndY + 2, backX, backY + 1 }, R.lower[3])
    L.line(img, backX, backY - wallHeight, W - 1, backEndY - wallHeight, O)
    L.line(img, backX, backY - wallHeight + 1, W - 1, backEndY - wallHeight + 1, R.lower[1])
    L.vline(img, backX, backY - wallHeight, backY, O)

    for step = (W - 1 - backX) // backRun, 0, -1 do
      local bx, by = backX + step * backRun, backY + step * backDrop
      local fx, fy = frontX + step * frontRun, frontY + step * frontDrop
      local nextBackX, nextFrontX = bx + backRun, fx + frontRun

      L.poly(img, { bx, by, nextBackX, by, nextFrontX, fy, fx, fy }, R.step[2])
      L.poly(img, { nextBackX, by, nextFrontX, fy,
                    nextFrontX, fy + frontDrop, nextBackX, by + backDrop }, R.step[3])
      L.line(img, nextBackX - 1, by, nextFrontX - 1, fy, R.step[1])
      L.line(img, nextBackX, by + backDrop, nextFrontX, fy + frontDrop, O)
      L.hline(img, fx, nextFrontX, fy, O)
      L.vline(img, nextFrontX, fy, fy + frontDrop, O)
    end

    -- the exit sign on the wall above the stairs
    box(img, 424, 110, 44, 15, R.leaf)
    L.text(img, 433, 113, "EXIT", ST.KEY.W, 1)
    box(img, 439, 128, 14, 12, R.paper)
    L.poly(img, { 442, 131, 450, 131, 446, 137 }, R.leaf[3])
    return img
  end

  return M
end
