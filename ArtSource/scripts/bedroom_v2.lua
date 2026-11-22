-- The v2 bedroom (480x270, shown at 4x = 1920x1080) for Earthquake Normal 1 (phase before), in the
-- v2 style of docs/adr/0001-v2-art-and-no-bantay.md: one dark outline around each object, three
-- shades per colour with light from the left, the classroom palette. The Student's feet stand on
-- y = 230, as in classroom_v2.lua. Nothing shakes here.
-- Left to right: the window, the bed, the tall cabinet, the low shelf, the start spot.
-- Things the Student moves are separate sprites: the bed (it slides), the heavy box, the big
-- picture and the stack of books, and box_placed, the box sitting in the low shelf.
-- Placements (top-left, art pixels) are in M.LAYOUT.

return function(L, ST)
  local M = {}
  local O = ST.KEY.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end
  -- Ramps: light, base, dark (the classroom_v2.lua ramps)
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    floor = { rgb(214, 198, 168), rgb(194, 176, 146), rgb(166, 148, 118) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    paper = { ST.KEY.W, ST.KEY.w, ST.KEY.v },
    navy = { ST.KEY.m, ST.KEY.N, ST.KEY.n },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    pot = { rgb(232, 140, 96), rgb(200, 104, 70), rgb(156, 72, 52) },
    cork = { rgb(222, 178, 120), rgb(198, 150, 96), rgb(166, 120, 74) },
    red = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) },
  }
  local SUN = rgb(252, 209, 22)
  local CEIL = rgb(232, 216, 178)

  -- Every prop stands on the back of the floor (its bottom row at y 212 or 213), like the
  -- furniture; the Student walks in front of them on y 230. Left to right: the picture under the
  -- window, the bed, the cabinet, the start spot with the books beside it, the box, the shelf.
  M.LAYOUT = {
    floorY = 230,
    picture = { 10, 183 },    -- 36x29, leaning on the wall left of the bed
    window = { 48, 54 },      -- 72x92 glass plus frame, curtains and sill
    bed = { 104, 194 },       -- 112x31, headboard on the right
    bedSlide = -8,            -- toward the window
    cabinet = { 236, 116 },   -- 46x98, drawn in the background
    start = 306,
    books = { 322, 201 },     -- 16x12, just right of the start spot
    box = { 350, 196 },       -- 24x17, on the floor left of the low shelf
    lowShelf = { 392, 166 },  -- 64x48, drawn in the background
    boxPlaced = { 397, 193 }, -- 24x17, in the lower left compartment
  }
  -- Act-out points drafted for the preview (the Student's centre x on y 230, and facing):
  -- box: pick up at 343 right, set down at 384 right (beside the shelf, the placed box shows);
  -- push: 229 left (palms on the headboard at x 215); picture: pick up at 34 left, reach up over
  -- the bed at 140 right; books: pick up at 316 right, reach up at the cabinet at 272 left.
  -- Hold points in the Student's frame: carry about (23, 30), reach-up about (28, 3).
  -- Only the walk to the box passes a prop: the books just right of the start.

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end

  -- Props ----------------------------------------------------------------------------------

  -- The heavy box: a taped cardboard box, 24x17.
  local function heavyBox(inShelf)
    local img = L.newImage(24, 17)
    box(img, 0, 0, 24, 17, R.cork)
    L.hline(img, 1, 22, 5, R.cork[3])                 -- the lid's edge
    L.rect(img, 10, 1, 4, 5, R.paper[2])              -- tape over the lid
    L.vline(img, 10, 1, 5, R.paper[1])
    L.rect(img, 4, 9, 8, 4, R.paper[1])               -- a label
    L.hline(img, 5, 10, 10, R.metal[3])
    L.hline(img, 5, 9, 12, R.metal[3])
    if inShelf then
      -- the shelf board above casts a shadow on the top rows
      for x = 1, 22 do
        for y = 1, 2 do
          local c = img:getPixel(x, y)
          if c == R.cork[1] or c == R.cork[2] or c == R.paper[2] or c == R.paper[1] then
            img:drawPixel(x, y, R.cork[3])
          end
        end
      end
    end
    return img
  end
  M.box = function() return heavyBox(false) end
  M.boxPlaced = function() return heavyBox(true) end

  -- The big picture: a wooden frame around a hill and a sun, 36x29.
  function M.picture()
    local img = L.newImage(36, 29)
    box(img, 0, 0, 36, 29, R.wood)
    L.rect(img, 3, 3, 30, 23, O)
    L.rect(img, 4, 4, 28, 21, R.glass[1])
    L.circle(img, 25, 9, 3, SUN)
    L.poly(img, { 4, 17, 13, 11, 22, 16, 31, 13, 31, 24, 4, 24 }, R.leaf[2])
    L.line(img, 4, 17, 13, 11, R.leaf[1])
    L.line(img, 13, 11, 22, 16, R.leaf[1])
    L.poly(img, { 4, 21, 18, 18, 31, 21, 31, 24, 4, 24 }, R.leaf[3])
    L.hline(img, 4, 31, 4, R.paper[1])
    return img
  end

  -- A stack of three books, 16x12.
  function M.books()
    local img = L.newImage(16, 12)
    local function book(x, y, w, ramp)
      box(img, x, y, w, 4, ramp)
      L.hline(img, x + 2, x + w - 3, y + 2, R.paper[1])     -- the pages' edge
    end
    book(0, 8, 16, R.navy)
    book(2, 4, 13, R.red)
    book(1, 0, 14, R.leaf)
    return img
  end

  -- One bed sprite, including the contact shadows. The right headboard is chest high.
  function M.bed()
    local img = L.newImage(112, 31)
    -- Solid silhouette first so the mattress, rail and wood share one outside outline.
    L.rect(img, 4, 13, 6, 16, R.wood[2])
    L.rect(img, 102, 6, 8, 23, R.wood[2])
    L.rect(img, 10, 22, 92, 3, R.wood[2])
    L.rect(img, 10, 14, 92, 8, R.paper[2])
    L.poly(img, { 13, 12, 75, 12, 77, 14, 77, 18, 75, 20, 13, 20 }, R.navy[2])
    L.poly(img, { 77, 14, 79, 12, 83, 11, 95, 11, 99, 12, 101, 14,
      100, 16, 79, 16 }, R.paper[2])

    L.hline(img, 13, 75, 12, R.navy[1])
    L.hline(img, 13, 74, 20, R.navy[3])
    L.vline(img, 14, 13, 18, R.navy[1])
    L.line(img, 65, 15, 74, 16, R.navy[3])
    L.hline(img, 17, 74, 21, R.paper[3])
    L.hline(img, 11, 101, 21, R.paper[3])
    L.line(img, 79, 14, 83, 12, R.paper[1])
    L.hline(img, 83, 95, 12, R.paper[1])
    L.line(img, 90, 16, 99, 15, R.paper[3])
    L.hline(img, 10, 101, 22, R.wood[1])
    L.hline(img, 10, 101, 24, R.wood[3])
    L.vline(img, 4, 13, 28, R.wood[1])
    L.vline(img, 9, 13, 28, R.wood[3])
    L.vline(img, 102, 6, 28, R.wood[1])
    L.vline(img, 109, 6, 28, R.wood[3])
    L.hline(img, 103, 108, 7, R.wood[1])
    L.outline(img, O)
    L.hline(img, 2, 12, 30, R.floor[3])
    L.hline(img, 99, 111, 30, R.floor[3])
    return img
  end

  -- Background --------------------------------------------------------------------------------

  local function drawFloor(img, w, h)
    -- The concrete floor of classroom_v2.lua (Easy art follow-up): continuous, with flecks and
    -- the window's light fanning across it.
    local light, base, shadow = table.unpack(R.floor)
    L.rect(img, 0, 208, w, h - 208, base)
    L.hline(img, 0, w - 1, 208, shadow)
    L.poly(img, { 54, 209, 81, 209, 85, 236, 41, 236 }, light)
    L.poly(img, { 85, 209, 114, 209, 138, 236, 92, 236 }, light)
    for i = 1, 72 do
      local x = (97 * i + 19 * i * i) % (w - 8) + 4
      local y = (17 * i + 7 * i * i) % (h - 212) + 211
      local pixel = img:getPixel(x, y)
      L.px(img, x, y, pixel == light and base or (i % 3 == 0 and shadow or light))
    end
  end

  -- The tall cabinet (a wardrobe), 46x98: two doors with knobs and a top board.
  local function cabinet(img, x, y)
    box(img, x - 2, y, 50, 7, R.wood)
    box(img, x, y + 6, 46, 92, R.wood)
    box(img, x + 4, y + 10, 19, 78, R.wood)
    box(img, x + 23, y + 10, 19, 78, R.wood)
    L.rect(img, x + 19, y + 46, 2, 4, O)
    L.rect(img, x + 25, y + 46, 2, 4, O)
    box(img, x + 3, y + 88, 40, 8, { R.wood[2], R.wood[3], R.wood[3] })   -- the plinth
  end

  -- The low shelf, 64x48: two tiers of two compartments. The lower left one is empty for the box,
  -- so the Student can set the box down standing beside the shelf.
  local function lowShelf(img, x, y)
    box(img, x, y, 64, 48, R.wood)
    -- dark insides of the four compartments
    for _, c in ipairs({ { 3, 3 }, { 33, 3 }, { 3, 25 }, { 33, 25 } }) do
      L.rect(img, x + c[1], y + c[2], 28, 19, R.wood[3])
      L.hline(img, x + c[1], x + c[1] + 27, y + c[2], O)
    end
    L.rect(img, x + 31, y + 3, 2, 41, R.wood[2])          -- the middle board
    L.vline(img, x + 31, y + 3, y + 43, R.wood[1])
    L.rect(img, x + 3, y + 22, 58, 3, R.wood[2])          -- the tier board
    L.hline(img, x + 3, x + 60, y + 22, R.wood[1])
    -- upper left: a small plant; upper right: a toy car; lower right: folded clothes
    box(img, x + 8, y + 14, 10, 8, R.pot)
    L.circle(img, x + 11, y + 10, 3, R.leaf[2]); L.circle(img, x + 15, y + 9, 3, R.leaf[2])
    L.circle(img, x + 10, y + 9, 1, R.leaf[1]); L.circle(img, x + 14, y + 8, 1, R.leaf[1])
    box(img, x + 37, y + 15, 18, 6, R.red)
    box(img, x + 41, y + 12, 9, 4, R.red)
    L.rect(img, x + 39, y + 20, 3, 2, O); L.rect(img, x + 50, y + 20, 3, 2, O)
    box(img, x + 36, y + 37, 22, 7, R.paper)
    box(img, x + 37, y + 32, 20, 6, R.glass)
  end

  -- The window, 72x96: glass with sky and a hill, a middle post, a sill and two curtains.
  local function window(img, x, y)
    box(img, x, y, 72, 92, R.wood)
    L.rect(img, x + 4, y + 4, 64, 84, O)
    L.rect(img, x + 5, y + 5, 62, 82, R.glass[1])
    L.rect(img, x + 5, y + 62, 62, 25, R.leaf[2])
    L.poly(img, { x + 5, y + 66, x + 26, y + 56, x + 46, y + 64, x + 66, y + 58, x + 66, y + 66, x + 5, y + 66 }, R.leaf[2])
    L.line(img, x + 5, y + 66, x + 26, y + 56, R.leaf[1])
    L.line(img, x + 26, y + 56, x + 46, y + 64, R.leaf[1])
    L.line(img, x + 46, y + 64, x + 66, y + 58, R.leaf[1])
    L.circle(img, x + 52, y + 22, 6, SUN)
    -- glints on the glass
    L.line(img, x + 9, y + 26, x + 21, y + 12, R.paper[1])
    L.line(img, x + 40, y + 30, x + 50, y + 18, R.glass[2])
    L.rect(img, x + 34, y + 4, 4, 84, O)
    L.rect(img, x + 35, y + 4, 2, 84, R.wood[2])
    L.rect(img, x + 4, y + 44, 64, 3, O)
    L.hline(img, x + 5, x + 66, y + 45, R.wood[2])
    box(img, x - 4, y + 90, 80, 6, R.wood)                -- the sill
    -- curtains, light on the left
    for _, cx in ipairs({ x - 6, x + 64 }) do
      box(img, cx, y - 4, 14, 70, R.red)
      L.vline(img, cx + 5, y, y + 64, R.red[3])
      L.vline(img, cx + 9, y, y + 64, R.red[1])
    end
    L.rect(img, x - 10, y - 7, 92, 3, O)                  -- the curtain rod
    L.hline(img, x - 9, x + 80, y - 6, R.metal[1])
  end

  function M.background()
    local W, H = 480, 270
    local img = L.newImage(W, H)
    -- ceiling, wall with a dado rail, lower wall, baseboard, floor
    L.rect(img, 0, 0, W, 14, CEIL)
    L.hline(img, 0, W - 1, 14, O)
    L.rect(img, 0, 15, W, 150, R.wall[2])
    L.rect(img, 0, 15, W, 2, R.wall[1])
    L.rect(img, 0, 165, W, 38, R.glass[1])
    L.hline(img, 0, W - 1, 164, R.wall[3])
    L.hline(img, 0, W - 1, 165, R.paper[1])
    for x = 6, W - 1, 12 do L.vline(img, x, 167, 202, R.glass[2]) end    -- wallpaper stripes
    L.rect(img, 0, 203, W, 4, R.wood[3])
    L.hline(img, 0, W - 1, 207, O)
    drawFloor(img, W, H)

    -- ceiling light
    L.rect(img, 239, 14, 2, 6, R.metal[3])
    L.ellipse(img, 240, 22, 9, 3, O)
    L.ellipse(img, 240, 22, 8, 2, R.paper[2])
    L.hline(img, 234, 244, 21, R.paper[1])

    -- a calendar and a star poster over the low shelf (the wall over the bed stays clear)
    local cx, cy = 398, 80
    box(img, cx, cy, 26, 34, R.paper)
    L.rect(img, cx + 1, cy + 1, 24, 7, R.red[2])
    for yy = cy + 12, cy + 28, 5 do
      for xx = cx + 4, cx + 20, 5 do L.rect(img, xx, yy, 2, 2, R.metal[2]) end
    end
    local px, py = 432, 86
    box(img, px, py, 22, 28, R.navy)
    L.poly(img, { px + 11, py + 6, px + 14, py + 13, px + 20, py + 13, px + 15, py + 17, px + 17, py + 24,
      px + 11, py + 20, px + 5, py + 24, px + 7, py + 17, px + 2, py + 13, px + 8, py + 13 }, SUN)

    -- A clock above the cabinet.
    L.circle(img, 259, 64, 9, O)
    L.circle(img, 259, 64, 8, R.paper[1])
    L.circle(img, 259, 64, 7, R.paper[2])
    L.vline(img, 259, 59, 64, O)
    L.hline(img, 259, 263, 64, O)

    cabinet(img, M.LAYOUT.cabinet[1], M.LAYOUT.cabinet[2])
    lowShelf(img, M.LAYOUT.lowShelf[1], M.LAYOUT.lowShelf[2])
    window(img, M.LAYOUT.window[1], M.LAYOUT.window[2])
    return img
  end

  return M
end
