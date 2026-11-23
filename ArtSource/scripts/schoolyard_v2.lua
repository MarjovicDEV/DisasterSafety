-- The shared v2 schoolyard for Earthquake Normal 4 and Hard 2.
-- The canopy and electric post have two frames and rest when the ground is still.
return function(L, ST)
  local M = {}
  local P = L.P
  local O = ST.KEY.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- Use the classroom ramps for the building and the rollout ramps for the tree and sign.
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    lower = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    sign = { rgb(88, 138, 104), rgb(64, 112, 84), rgb(48, 90, 68) },
    roof = { P.roof3, P.roof, P.roof2 },
    path = { P.concrete, P.concrete2, P.concrete3 },
    grass = { P.green4, P.grass, P.grass2 },
  }

  -- Positions are in art pixels from the top left. Character points are at the feet.
  M.LAYOUT = {
    floorY = 230, horizonY = 132,
    building = { 148, 111 }, roof = { 140, 88 }, door = { 191, 140 },
    sign = { 15, 142 }, trunk = { 330, 127 }, shade = { 356, 194 },
    canopy = { 300, 83 }, post = { 418, 95 }, swayMs = 160,
    normal4 = { start = { 302, 230 }, field = { 86, 230 },
      tree = { 320, 199 }, building = { 260, 230 },
      postBend = { 336, 244 }, postApproach = { 393, 244 }, post = { 414, 225 } },
    hard2 = { start = { 306, 230 }, teacher = { 217, 230 }, later = { 260, 230 },
      fieldBend = { 170, 230 }, teacherBend = { 142, 225 },
      field = { 133, 198 }, teacherField = { 101, 183 },
      tree = { 323, 215 },
      postBend = { 336, 244 }, postApproach = { 393, 244 }, post = { 414, 225 } },
  }
  local A = M.LAYOUT

  -- Normal 4 crosses the forecourt to the open field. The tree walk ends at the
  -- front-left edge of the shade, and the building run stops outside the doorway.
  -- The post route bends through the foreground, clear of the trunk and shade.
  -- In Hard 2 the Teacher stays ahead on the field path and stops to the right of
  -- the sign. The tree route ends before the shade, and the late route faces the door.

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
  end

  local function sky(img)
    L.rect(img, 0, 0, 480, A.horizonY, P.sky4)
    for y = 57, A.horizonY - 1 do
      local fade = math.min(1, (y - 57) / 29)
      for x = 0, 479 do
        if L.bayer(x, y) < fade then L.px(img, x, y, P.sky5) end
      end
    end
    L.poly(img, { 0, 82, 27, 82, 33, 78, 46, 78, 54, 83, 72, 83, 82, 88,
      99, 88, 99, 91, 0, 91 }, P.white)
    L.poly(img, { 371, 72, 383, 72, 389, 67, 408, 67, 415, 72, 427, 72, 437, 77,
      454, 77, 454, 80, 371, 80 }, P.white)
    L.poly(img, { 0, 123, 22, 110, 42, 103, 54, 102, 74, 106, 97, 115, 123, 116,
      150, 128, 183, 123, 223, 132, 0, 138 }, P.hill)
    L.poly(img, { 247, 132, 293, 120, 328, 108, 345, 107, 370, 116, 403, 119,
      436, 105, 451, 105, 479, 114, 479, 139, 247, 139 }, P.hill)
    L.poly(img, { 0, 129, 29, 124, 57, 126, 82, 126, 122, 135, 165, 130, 194, 139,
      0, 147 }, P.hill2)
    L.poly(img, { 283, 137, 327, 127, 351, 125, 383, 130, 429, 122, 453, 123,
      479, 120, 479, 147, 283, 147 }, P.hill2)
  end

  local function ground(img)
    local y = A.horizonY
    L.rect(img, 0, y, 480, 270 - y, R.grass[2])
    L.poly(img, { 0, y, 480, y, 480, 143, 437, 142, 393, 146, 354, 143,
      308, 150, 246, 144, 202, 145, 154, 141, 89, 150, 33, 143, 0, 148 }, P.hill2)
    L.poly(img, { 0, 213, 50, 210, 115, 215, 157, 226, 170, 244, 0, 247 }, R.leaf[1])
    L.poly(img, { 0, 258, 67, 247, 112, 248, 151, 259, 221, 265, 285, 261,
      369, 265, 479, 254, 479, 270, 0, 270 }, R.grass[3])

    L.seed(808)
    for i = 1, 570 do
      local x, py = L.randi(2, 477), L.randi(y + 3, 269)
      local base = L.get(img, x, py)
      local tuft = base == R.leaf[1] and R.grass[2] or R.grass[1]
      local soil = i % 9 == 0 and R.wood[1] or R.grass[3]
      L.px(img, x, py, i % 3 == 0 and soil or tuft)
      if i % 14 == 0 then
        L.px(img, x + 1, py - 1, tuft)
        L.px(img, x + 2, py, i % 9 == 0 and R.wood[1] or R.leaf[2])
      end
    end
  end

  local function paths(img)
    local side, fill, light = R.path[3], R.path[2], R.path[1]

    -- The tree and post paths fork to the right at different depths.
    L.poly(img, { 295, 224, 314, 210, 333, 196, 345, 188, 351, 191,
      345, 198, 330, 211, 312, 233, 296, 235 }, side)
    L.poly(img, { 299, 225, 316, 210, 334, 196, 345, 189, 350, 191,
      344, 197, 328, 211, 310, 232, 299, 233 }, fill)
    L.line(img, 306, 223, 329, 201, light)

    L.poly(img, { 296, 225, 337, 236, 388, 237, 410, 218, 432, 200, 437, 202,
      423, 224, 395, 252, 333, 254, 297, 237 }, side)
    L.poly(img, { 300, 228, 337, 238, 389, 239, 412, 219, 432, 202, 436, 202,
      421, 223, 394, 250, 333, 252, 300, 236 }, fill)
    L.line(img, 337, 239, 388, 240, light)
    L.line(img, 392, 238, 423, 209, light)

    -- A wider turn leaves room for the Student and Teacher beside the sign.
    L.poly(img, { 81, 167, 91, 165, 117, 179, 139, 194, 159, 216, 300, 221,
      315, 232, 300, 242, 149, 242, 130, 234, 116, 207, 99, 186 }, side)
    L.poly(img, { 83, 168, 91, 166, 115, 180, 137, 195, 158, 218, 299, 223,
      313, 232, 299, 240, 149, 240, 132, 232, 118, 205, 101, 185 }, fill)
    L.line(img, 89, 168, 113, 181, light)
    L.line(img, 160, 219, 296, 224, light)

    L.seed(809)
    for _ = 1, 205 do
      local x, y = L.randi(84, 436), L.randi(181, 251)
      if L.get(img, x, y) == fill then L.px(img, x, y, light) end
    end
  end

  local function window(img, x, y, w)
    box(img, x, y, w, 29, R.wood)
    L.rect(img, x + 3, y + 3, w - 6, 23, R.glass[2])
    for row = y + 5, y + 25, 5 do
      L.hline(img, x + 3, x + w - 4, row, R.glass[1])
      L.hline(img, x + 3, x + w - 4, row + 1, R.glass[3])
    end
    L.vline(img, x + math.floor(w / 2), y + 3, y + 25, R.wood[3])
    L.hline(img, x - 1, x + w, y + 28, O)
    L.hline(img, x, x + w - 1, y + 27, R.wood[1])
  end

  local function building(img)
    local x, y = table.unpack(A.building)
    box(img, x, y, 132, 113, R.wall)
    L.rect(img, x + 1, y + 8, 130, 5, R.wall[3])
    L.rect(img, x + 1, y + 74, 130, 36, R.lower[2])
    L.hline(img, x + 1, x + 130, y + 74, R.lower[1])
    L.hline(img, x + 2, x + 129, y + 110, R.lower[3])
    window(img, x + 8, y + 32, 27)
    window(img, x + 100, y + 32, 24)

    local rx, ry = table.unpack(A.roof)
    L.poly(img, { rx, y, rx + 21, ry, rx + 120, ry, rx + 145, y,
      rx + 145, y + 8, rx, y + 8 }, O)
    L.poly(img, { rx + 2, y - 1, rx + 22, ry + 1, rx + 119, ry + 1,
      rx + 143, y - 1, rx + 143, y + 2, rx + 2, y + 2 }, R.roof[2])
    L.line(img, rx + 4, y - 2, rx + 23, ry + 2, R.roof[1])
    L.hline(img, rx + 23, rx + 117, ry + 2, R.roof[1])
    for rib = 31, 114, 11 do
      local edge = rx + rib + math.floor((rib - 70) / 5)
      L.line(img, rx + rib, ry + 3, edge, y - 1, R.roof[3])
      L.line(img, rx + rib - 1, ry + 3, edge - 1, y - 1, R.roof[1])
    end
    L.hline(img, rx + 1, rx + 144, y + 4, R.roof[1])
    L.hline(img, rx + 1, rx + 144, y + 7, R.roof[3])

    local dx, dy = table.unpack(A.door)
    -- The dark opening is wider than the Teacher's frame. The threshold is at the floor line.
    box(img, dx, dy, 52, A.floorY - dy, R.wood)
    L.rect(img, dx + 5, dy + 6, 42, 78, P.teal2)
    L.rect(img, dx + 7, dy + 8, 37, 67, R.lower[3])
    L.rect(img, dx + 8, dy + 8, 32, 60, R.sign[3])
    L.vline(img, dx + 44, dy + 7, dy + 82, R.wood[3])
    L.hline(img, dx + 5, dx + 46, dy + 83, R.path[3])
    L.hline(img, dx + 4, dx + 47, dy + 86, R.path[1])
    L.hline(img, dx + 1, dx + 50, dy + 88, R.path[3])
  end

  local function shadeAndTrunk(img)
    local sx, sy = table.unpack(A.shade)
    L.ellipse(img, sx, sy, 39, 10, R.grass[3])
    L.ellipse(img, sx + 3, sy + 1, 34, 7, R.leaf[2])
    local x, y = table.unpack(A.trunk)
    L.poly(img, { x, y + 5, x + 5, y, x + 19, y + 16, x + 33, y + 2,
      x + 39, y + 8, x + 27, y + 27, x + 25, y + 55, x + 34, y + 62,
      x + 25, y + 62, x + 18, y + 59, x + 8, y + 63, x + 3, y + 62,
      x + 13, y + 53, x + 14, y + 26 }, O)
    L.poly(img, { x + 2, y + 5, x + 6, y + 2, x + 19, y + 19, x + 33, y + 4,
      x + 37, y + 8, x + 25, y + 26, x + 23, y + 56, x + 30, y + 61,
      x + 25, y + 61, x + 18, y + 58, x + 8, y + 61, x + 15, y + 53,
      x + 16, y + 26 }, R.wood[3])
    L.line(img, x + 6, y + 8, x + 18, y + 27, R.wood[2], 3)
    L.line(img, x + 18, y + 28, x + 16, y + 55, R.wood[2], 3)
    L.line(img, x + 17, y + 32, x + 15, y + 54, R.wood[1])
    L.line(img, x + 14, y + 56, x + 9, y + 60, R.wood[1])
  end

  local function evacuationSign(img)
    local x, y = table.unpack(A.sign)
    L.ellipse(img, x + 35, y + 33, 30, 3, R.grass[3])
    box(img, x + 8, y + 20, 5, 14, R.wood)
    box(img, x + 55, y + 20, 5, 14, R.wood)
    box(img, x, y, 68, 24, R.sign)
    for i, label in ipairs({ "EVACUATION", "AREA" }) do
      local textX = x + math.floor((68 - L.F.width(label, 1)) / 2)
      L.text(img, textX, y + 4 + (i - 1) * 9, label, P.white)
    end
  end

  function M.background()
    local img = L.newImage(480, 270)
    sky(img)
    ground(img)
    paths(img)
    shadeAndTrunk(img)
    building(img)
    evacuationSign(img)
    return img
  end

  local function canopyFrame(sway)
    local img = L.newImage(119, 75)
    local dark, base, light = R.leaf[3], R.leaf[2], R.leaf[1]
    L.poly(img, { 3 + sway, 39, 11 + sway, 30, 20 + sway, 24, 31 + sway, 23,
      38 + sway, 11, 49 + sway, 6, 58 + sway, 6, 69 + sway, 1, 82 + sway, 4,
      90 + sway, 11, 95 + sway, 21, 105 + sway, 25, 111 + sway, 34, 113 + sway, 46,
      110 + sway, 55, 102 + sway, 60, 87, 61, 79, 66, 66, 67, 57, 72,
      43, 72, 34, 67, 24, 67, 15, 63, 6 + sway, 61, 2 + sway, 53 }, dark)
    L.poly(img, { 5 + sway, 40, 14 + sway, 29, 25 + sway, 24, 36 + sway, 25,
      41 + sway, 12, 50 + sway, 8, 62 + sway, 7, 72 + sway, 4, 82 + sway, 6,
      89 + sway, 16, 91 + sway, 26, 103 + sway, 28, 108 + sway, 36,
      110 + sway, 44, 103 + sway, 52, 93, 54, 84, 51, 75, 59, 67, 59,
      57, 66, 44, 65, 37, 57, 24, 63, 14, 58, 7 + sway, 53 }, base)
    L.poly(img, { 8 + sway, 40, 16 + sway, 31, 27 + sway, 26, 37 + sway, 27,
      34 + sway, 33, 23 + sway, 34, 17 + sway, 42, 12 + sway, 46 }, light)
    L.poly(img, { 40 + sway, 28, 43 + sway, 16, 52 + sway, 10, 61 + sway, 9,
      68 + sway, 7, 75 + sway, 8, 69 + sway, 14, 58 + sway, 14, 51 + sway, 21,
      49 + sway, 28 }, light)
    L.poly(img, { 27 + sway, 49, 35 + sway, 41, 45 + sway, 37, 55 + sway, 38,
      50 + sway, 42, 41 + sway, 44, 36 + sway, 51, 29 + sway, 54 }, light)
    L.poly(img, { 64 + sway, 33, 72 + sway, 23, 81 + sway, 20, 90 + sway, 24,
      86 + sway, 28, 77 + sway, 28, 72 + sway, 34 }, light)
    L.poly(img, { 74 + sway, 47, 82 + sway, 42, 90 + sway, 43, 87 + sway, 47,
      81 + sway, 50 }, dark)
    L.poly(img, { 51, 54, 58, 49, 67, 50, 63, 54, 59, 59, 53, 58 }, dark)
    L.outline(img, O)
    return img
  end

  function M.canopyFrames()
    return { canopyFrame(0), canopyFrame(3) }
  end

  local function postFrame(sway)
    local img = L.newImage(62, 107)
    local x = 21 + sway
    -- The street-side spans run to the right edge. The foot of the post stays fixed.
    for i = 0, 2 do
      local y = 11 + 5 * i
      L.line(img, x + 5 + 5 * i, y, 46, y + 5 - sway, O)
      L.line(img, 46, y + 5 - sway, 61, y + 3, O)
    end
    L.poly(img, { x - 3, 4, x + 3, 4, 25, 105, 16, 105 }, O)
    L.poly(img, { x - 2, 5, x + 2, 5, 24, 104, 17, 104 }, R.metal[2])
    L.line(img, x - 2, 6, 17, 104, R.metal[1])
    L.line(img, x + 2, 6, 23, 104, R.metal[3])
    box(img, x - 12, 24, 35, 5, R.metal)
    for i = 0, 2 do
      local y = 10 + i * 5
      local ix = x + 5 + i * 5
      L.line(img, ix, y + 3, ix, 24, R.metal[3])
      box(img, ix - 2, y, 5, 4, R.glass)
    end
    return img
  end

  function M.postFrames()
    return { postFrame(0), postFrame(2) }
  end

  return M
end
