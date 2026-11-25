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
    building = { 148, 120 }, roof = { 140, 98 }, door = { 191, 140 },
    sign = { 15, 116 }, shade = { 356, 194 },
    canopy = { 300, 83 }, post = { 418, 95 }, swayMs = 160,
    normal4 = { start = { 190, 230 }, field = { 30, 230 },
      treeBend = { 287, 230 }, tree = { 320, 199 }, building = { 207, 236 } },
    hard2 = { start = { 306, 230 }, teacher = { 217, 230 }, later = { 260, 230 },
      doorExit = { 217, 243 }, join = { 251, 243 },
      fieldBend = { 94, 243 }, teacherBend = { 60, 243 },
      field = { 84, 210 }, teacherField = { 52, 206 },
      tree = { 323, 215 },
      postBend = { 336, 244 }, post = { 393, 244 } },
  }
  local A = M.LAYOUT

  -- Normal 4 crosses the forecourt to the open field. The tree walk ends at the
  -- front-left edge of the shade, and the building run stops outside the doorway.
  -- The post route bends through the foreground, clear of the trunk and shade, and
  -- stops short of the pole and its lines. The field spot is on open grass, clear of the sign.
  -- In Hard 2 the Teacher leads along the foreground and stops in front of the sign. The tree route ends before the shade, and the late route faces the door.

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

    -- One continuous gravel walk runs along the front of the veranda and
    -- opens into a gathering place beside the sign. The Normal 4 field stop lies west of this path.
    local walk = { 33, 162, 42, 159, 65, 160, 74, 165, 74, 178, 88, 197, 101, 235, 248, 235, 300, 221,
      311, 229, 303, 239, 255, 252, 59, 252, 51, 247, 43, 207, 41, 182, 34, 173 }
    L.poly(img, walk, side)
    L.poly(img, { 35, 163, 43, 161, 64, 162, 72, 166, 72, 179, 86, 198, 100, 237, 249, 237, 300, 223,
      309, 229, 302, 237, 254, 250, 60, 250, 53, 246, 45, 206, 43, 182, 36, 172 }, fill)
    L.line(img, 53, 245, 45, 201, light)
    L.hline(img, 101, 248, 237, light)
    L.line(img, 251, 236, 298, 224, light)
    -- A short concrete apron connects the shaded threshold to the gravel.
    L.poly(img, { 147, 213, 279, 213, 286, 230, 250, 240, 205, 240,
      207, 232, 143, 232 }, side)
    L.poly(img, { 148, 214, 278, 214, 284, 229, 249, 238, 207, 238,
      209, 230, 145, 230 }, light)
    L.line(img, 149, 224, 278, 224, fill)
    L.line(img, 207, 233, 252, 233, fill)
    L.line(img, 172, 224, 170, 229, fill)
    L.line(img, 263, 224, 266, 231, fill)

    L.seed(809)
    for _ = 1, 205 do
      local x, y = L.randi(84, 436), L.randi(181, 251)
      if L.get(img, x, y) == fill then L.px(img, x, y, light) end
    end
  end

  local function window(img, x, y, w)
    box(img, x, y, w, 32, R.sign)
    L.rect(img, x + 2, y + 2, w - 4, 27, R.glass[3])
    L.rect(img, x + 3, y + 5, w - 6, 21, R.glass[2])
    for col = x + 5, x + w - 4, 5 do
      L.vline(img, col, y + 2, y + 29, R.sign[3])
      L.vline(img, col - 1, y + 3, y + 23, R.lower[1])
    end
    L.hline(img, x + 2, x + w - 3, y + 17, R.sign[3])
    L.hline(img, x - 1, x + w, y + 31, R.path[3])
    L.hline(img, x - 1, x + w - 1, y + 30, R.path[1])
  end

  local function building(img)
    local x, y = table.unpack(A.building)
    box(img, x, y, 132, 100, R.wall)
    L.rect(img, x + 1, y + 7, 130, 9, R.wall[3])
    L.rect(img, x + 1, y + 16, 130, 2, R.wall[2])
    L.rect(img, x + 1, y + 70, 130, 24, R.lower[2])
    L.hline(img, x + 1, x + 130, y + 70, R.lower[1])
    L.rect(img, x + 1, y + 93, 130, 6, R.path[3])
    L.poly(img, { 148, 218, 279, 218, 283, 223, 145, 223 }, R.path[2])
    L.hline(img, 146, 282, 223, R.path[1])
    window(img, x + 10, y + 25, 29)
    window(img, x + 98, y + 25, 27)

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
    box(img, dx, dy, 52, 83, R.sign)
    L.rect(img, dx + 3, dy + 4, 46, 76, R.sign[3])
    L.rect(img, dx + 6, dy + 8, 40, 69, P.ink2)
    L.rect(img, dx + 19, dy + 17, 26, 55, P.teal2)
    -- The ajar panel is seen at an angle. Keep the opening free for the Teacher.
    L.poly(img, { dx + 3, dy + 5, dx + 18, dy + 9, dx + 18, dy + 76,
      dx + 3, dy + 81 }, O)
    L.poly(img, { dx + 4, dy + 6, dx + 17, dy + 10, dx + 17, dy + 75,
      dx + 4, dy + 79 }, P.green2)
    for _, top in ipairs({ 13, 43 }) do
      L.rect(img, dx + 6, dy + top, 8, 22, P.green3)
      L.rect(img, dx + 7, dy + top + 1, 6, 19, P.green2)
      L.vline(img, dx + 7, dy + top + 1, dy + top + 18, P.green)
    end
    L.rect(img, dx + 14, dy + 38, 2, 2, P.yellow3)
    L.poly(img, { 192, 221, 242, 221, 245, 230, 190, 230 }, R.path[3])
    L.poly(img, { 193, 222, 241, 222, 244, 228, 191, 228 }, R.path[2])
    L.hline(img, 192, 243, 228, R.path[1])
    for _, cx in ipairs({ 149, 273 }) do
      L.rect(img, cx, 127, 5, 95, R.wall[3])
      L.rect(img, cx, 127, 2, 94, R.wall[1])
      L.rect(img, cx - 1, 216, 7, 7, R.path[2])
      L.hline(img, cx - 1, cx + 5, 216, R.path[1])
    end
  end

  local function shadeAndTrunk(img)
    local sx, sy = table.unpack(A.shade)
    L.ellipse(img, sx + 2, sy, 40, 9, R.grass[3])
    L.ellipse(img, sx + 4, sy, 31, 6, R.leaf[2])
    local wood = R.wood
    local function branch(x, y, ex, ey, width)
      L.line(img, x, y, ex, ey, O, width)
      L.line(img, x + 1, y + 1, ex + 1, ey + 1, wood[2], width - 2)
      L.line(img, x + 1, y + 1, ex + 1, ey + 1, wood[1])
    end
    branch(346, 160, 317, 127, 6)
    branch(327, 139, 330, 119, 4)
    branch(351, 155, 380, 129, 6)
    branch(367, 142, 374, 119, 4)
    L.poly(img, { 341, 145, 352, 152, 365, 141, 363, 152, 356, 162,
      355, 179, 359, 185, 370, 191, 361, 192, 351, 188, 340, 194,
      331, 193, 343, 184, 346, 177, 346, 159 }, O)
    L.poly(img, { 343, 148, 352, 155, 362, 146, 357, 156, 353, 161,
      353, 180, 357, 186, 365, 190, 360, 190, 351, 186, 339, 192,
      335, 192, 345, 184, 348, 177, 348, 158 }, wood[3])
    L.line(img, 349, 159, 348, 180, wood[2], 3)
    L.line(img, 348, 157, 347, 179, wood[1])
    L.line(img, 347, 182, 339, 190, wood[1])
  end

  local function evacuationSign(img)
    local x, y = table.unpack(A.sign)
    L.ellipse(img, x + 37, y + 46, 34, 4, R.grass[3])
    for _, off in ipairs({ 9, 62 }) do box(img, x + off, y + 20, 5, 27, R.metal) end
    box(img, x, y, 76, 28, R.sign)
    L.hline(img, x + 3, x + 72, y + 2, R.lower[1])
    for i, label in ipairs({ "EVACUATION", "AREA" }) do
      local left = x + math.floor((76 - L.F.width(label, 1)) / 2)
      L.text(img, left, y + 6 + (i - 1) * 10, label, P.white)
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
    -- Overlapping sprays leave wedges of sky between the exposed lower limbs.
    local crowns = { {21,37,18,12}, {37,26,24,16}, {56,20,20,17},
      {78,24,22,16}, {94,34,18,14}, {82,43,20,11}, {48,36,16,13} }
    for _, v in ipairs(crowns) do
      L.ellipse(img, v[1]+sway, v[2], v[3], v[4], dark)
    end
    for _, v in ipairs(crowns) do
      L.ellipse(img, v[1]-2+sway, v[2]-3, v[3]-3, v[4]-3, base)
    end
    for _, tip in ipairs({ {8,32,9,4}, {18,28,7,3}, {21,18,12,4},
      {37,12,9,3}, {47,7,10,3}, {63,10,8,3}, {72,15,9,3},
      {42,29,7,3}, {56,25,7,2}, {87,29,7,3}, {31,35,6,2} }) do
      L.ellipse(img, tip[1]+sway,tip[2],tip[3],tip[4],light)
    end
    for _, d in ipairs({ {22,39},{36,25},{53,17},{62,31},{72,39},{89,35},{97,40} }) do
      L.line(img, d[1]+sway, d[2], d[1]+sway+4, d[2]-2, dark)
    end
    L.outline(img, O)
    return img
  end

  function M.canopyFrames()
    return { canopyFrame(0), canopyFrame(3) }
  end

  local function postFrame(sway)
    local img = L.newImage(62, 107)
    local x = 21 + sway
    local metal = R.metal
    -- Two spans dip between pin insulators; the three pins stand on one wooden crossarm.
    for _, off in ipairs({ -14, 0, 15 }) do
      local px = x + off
      L.line(img, px, 10, px + 11, 14, O)
      L.line(img, px + 11, 14, px + 27, 16, O)
      L.line(img, px + 27, 16, 61, 11 + (off + 14) // 4, O)
    end
    L.poly(img, { x - 3, 7, x + 3, 7, 25, 105, 16, 105 }, O)
    L.poly(img, { x - 2, 8, x + 2, 8, 24, 104, 17, 104 }, metal[2])
    L.line(img, x - 2, 10, 17, 104, metal[1])
    L.line(img, x + 2, 10, 23, 104, metal[3])
    L.line(img, x - 13, 24, x, 37, metal[3], 2)
    L.line(img, x, 37, x + 15, 24, metal[3], 2)
    box(img, x - 19, 21, 40, 5, R.wood)
    for _, off in ipairs({ -14, 0, 15 }) do
      local px = x + off
      L.vline(img, px, 10, 20, O)
      L.rect(img, px - 2, 11, 5, 6, metal[3])
      L.hline(img, px - 3, px + 3, 12, R.glass[1])
      L.hline(img, px - 3, px + 3, 15, R.glass[1])
    end
    return img
  end

  function M.postFrames()
    return { postFrame(0), postFrame(2) }
  end

  return M
end
