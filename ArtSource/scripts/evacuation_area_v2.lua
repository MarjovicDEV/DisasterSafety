-- The v2 evacuation area for Earthquake Hard 7 (phase after, nothing shakes): an open, grassy
-- field near the school in daylight, calm after the earthquake. Left to right: the school in the
-- distance with a small Teacher holding a clipboard, the family's woven mat (Mom sits on it as a
-- Grown-up actor, and the radio is a prop on it), the open field where the Student starts, the
-- neighbours chatting (a two-frame moving part), and the path back toward the houses, which
-- leaves the right edge. A single leaning sign near the houses shows that the earthquake has
-- happened; there is no fire, smoke or hurt person.
-- Settings are 480x270 art pixels. Positions are art pixels from the top left, and character
-- points are at the feet. The Act-out points are recorded in ArtSource/README-rollout.md.
return function(L, ST, GU)
  local M = {}
  local P = L.P
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- The same ramps as the v2 schoolyard (light, base, shadow), plus the mat and the radio.
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
    mat = { rgb(244, 214, 150), rgb(222, 182, 112), rgb(184, 140, 80) },
    radio = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) },
    wave = rgb(252, 209, 22),
  }

  M.LAYOUT = {
    floorY = 230, horizonY = 132,
    school = { 14, 112 }, teacher = { 140, 170 },
    mat = { top = 226, bottom = 246, x0 = 96, x1 = 208 },
    mom = { 118, 236 },
    radio = { 136, 208 }, radioSize = { 28, 30 }, radioMs = 260,
    neighbours = { 272, 164 }, neighboursSize = { 86, 62 }, neighboursMs = 600,
    pathStartX = 395,
    hard7 = {
      start = { 228, 230 },
      sit = { 182, 236 },
      goHome = { 376, 248 },
      rumor = { 254, 228 },
    },
  }
  local A = M.LAYOUT

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
    L.poly(img, { 196, 80, 220, 80, 227, 75, 243, 75, 251, 80, 270, 80, 279, 85,
      292, 85, 292, 88, 196, 88 }, P.white)
    L.poly(img, { 371, 70, 383, 70, 389, 65, 408, 65, 415, 70, 427, 70, 437, 75,
      454, 75, 454, 78, 371, 78 }, P.white)
    L.poly(img, { 0, 120, 30, 108, 60, 104, 92, 112, 130, 114, 170, 124, 214, 121,
      250, 130, 0, 138 }, P.hill)
    L.poly(img, { 240, 132, 286, 118, 322, 110, 352, 112, 380, 118, 418, 108,
      450, 104, 479, 110, 479, 139, 240, 139 }, P.hill)
    L.poly(img, { 0, 130, 40, 125, 90, 128, 140, 134, 190, 131, 230, 138, 0, 146 }, P.hill2)
    L.poly(img, { 260, 138, 300, 130, 350, 128, 400, 131, 440, 126, 479, 124,
      479, 147, 260, 147 }, P.hill2)
  end

  local function ground(img)
    local y = A.horizonY
    L.rect(img, 0, y, 480, 270 - y, R.grass[2])
    L.poly(img, { 0, y, 480, y, 480, 143, 430, 145, 380, 142, 330, 148, 270, 144,
      210, 147, 150, 142, 90, 149, 30, 143, 0, 147 }, P.hill2)
    L.poly(img, { 0, 186, 60, 182, 120, 188, 150, 198, 120, 206, 40, 204, 0, 208 }, R.leaf[1])
    L.poly(img, { 0, 258, 70, 249, 140, 252, 220, 262, 300, 258, 390, 264, 479, 256,
      479, 270, 0, 270 }, R.grass[3])

    L.seed(1207)
    for i = 1, 560 do
      local x, py = L.randi(2, 477), L.randi(y + 3, 269)
      local base = L.get(img, x, py)
      local tuft = base == R.leaf[1] and R.grass[2] or R.grass[1]
      L.px(img, x, py, i % 3 == 0 and R.grass[3] or tuft)
      if i % 14 == 0 then
        L.px(img, x + 1, py - 1, tuft)
        L.px(img, x + 2, py, R.leaf[2])
      end
    end
  end

  local function window(img, x, y)
    box(img, x, y, 12, 11, R.wood)
    L.rect(img, x + 2, y + 2, 8, 7, R.glass[2])
    L.hline(img, x + 2, x + 9, y + 2, R.glass[1])
    L.vline(img, x + 6, y + 2, y + 8, R.wood[3])
  end

  -- The school stands far back on the left, so it is small and sits on the far grass.
  local function school(img)
    local x, y = table.unpack(A.school)
    box(img, x, y, 104, 48, R.wall)
    L.rect(img, x + 1, y + 30, 102, 16, R.lower[2])
    L.hline(img, x + 1, x + 102, y + 30, R.lower[1])
    L.hline(img, x + 2, x + 101, y + 46, R.lower[3])
    for _, wx in ipairs({ 8, 26, 66, 84 }) do window(img, x + wx, y + 10) end
    -- The doorway faces the field, with the flagpole beside it.
    box(img, x + 45, y + 22, 16, 26, R.wood)
    L.rect(img, x + 48, y + 25, 10, 22, R.sign[3])
    L.poly(img, { x - 6, y + 2, x + 10, y - 14, x + 94, y - 14, x + 110, y + 2,
      x + 110, y + 6, x - 6, y + 6 }, O)
    L.poly(img, { x - 4, y + 2, x + 11, y - 13, x + 93, y - 13, x + 108, y + 2,
      x + 108, y + 5, x - 4, y + 5 }, R.roof[2])
    L.line(img, x - 2, y + 1, x + 12, y - 12, R.roof[1])
    L.hline(img, x + 12, x + 92, y - 12, R.roof[1])
    L.hline(img, x - 3, x + 107, y + 4, R.roof[3])
    L.vline(img, x + 120, y - 6, y + 48, R.metal[3])
    L.vline(img, x + 119, y - 6, y + 48, R.metal[1])
    L.rect(img, x + 121, y - 6, 10, 6, P.blue)
    L.rect(img, x + 121, y - 3, 10, 3, P.red)
    L.ellipse(img, x + 52, y + 50, 50, 3, R.grass[3])
  end

  -- The Teacher in the distance: about half the Grown-ups' height, holding a clipboard.
  local function distantTeacher(img)
    local t = L.newImage(16, 30)
    L.rect(t, 6, 0, 4, 3, K.H)                -- the bun
    L.rect(t, 4, 2, 8, 6, K.H)                -- the hair
    L.rect(t, 5, 4, 6, 5, K.S)                -- the face
    L.hline(t, 5, 10, 4, K.t)
    L.px(t, 6, 6, K.e); L.px(t, 9, 6, K.e)
    L.rect(t, 3, 9, 10, 9, K.N)               -- the blazer
    L.vline(t, 3, 9, 17, K.m)
    L.rect(t, 7, 9, 2, 3, K.W)                -- the blouse
    L.px(t, 7, 11, K.p)
    L.rect(t, 4, 18, 8, 5, K.N)               -- the skirt
    L.vline(t, 12, 10, 16, K.n)
    L.rect(t, 5, 23, 2, 4, K.S)               -- the legs
    L.rect(t, 9, 23, 2, 4, K.S)
    L.rect(t, 4, 27, 3, 2, K.B)
    L.rect(t, 9, 27, 3, 2, K.B)
    L.rect(t, 9, 11, 6, 7, R.wood[2])         -- the clipboard
    L.rect(t, 10, 12, 4, 5, K.W)
    L.hline(t, 11, 13, 13, K.v)
    L.hline(t, 11, 13, 15, K.v)
    L.px(t, 8, 14, K.S)
    L.outline(t, O)
    local x, y = table.unpack(A.teacher)
    L.ellipse(img, x, y, 8, 2, R.grass[3])
    L.blit(img, t, x - 8, y - 29)
  end

  -- The houses beyond the path, with one road sign leaning after the earthquake.
  local function houses(img)
    local function house(x, y, w, ramp)
      box(img, x, y, w, 20, R.wall)
      L.poly(img, { x - 4, y + 1, x + math.floor(w / 2), y - 12, x + w + 3, y + 1 }, O)
      L.poly(img, { x - 2, y, x + math.floor(w / 2), y - 10, x + w + 1, y }, ramp[2])
      L.line(img, x - 1, y - 1, x + math.floor(w / 2), y - 9, ramp[1])
      box(img, x + 4, y + 6, 8, 7, R.glass)
      L.rect(img, x + w - 10, y + 8, 6, 12, R.wood[3])
    end
    house(408, 120, 30, R.roof)
    house(446, 116, 30, R.lower)
    L.ellipse(img, 442, 141, 40, 2, R.grass[3])
    -- The leaning sign: a short post tilted to the left, with a blank yellow board.
    L.line(img, 393, 158, 386, 136, O, 3)
    L.line(img, 393, 157, 387, 137, R.metal[2])
    L.poly(img, { 378, 126, 392, 128, 390, 140, 376, 138 }, O)
    L.poly(img, { 379, 128, 391, 129, 389, 139, 377, 137 }, P.yellow)
    L.line(img, 379, 128, 377, 136, P.yellow3)
  end

  -- The path back toward the houses: it starts on the field at about x 395 and leaves the
  -- right edge, then a far stretch climbs to the houses.
  local function path(img)
    local side, fill, light = R.path[3], R.path[2], R.path[1]
    L.poly(img, { 430, 160, 440, 142, 448, 142, 444, 162, 470, 200, 480, 204, 480, 214, 460, 206 }, side)
    L.poly(img, { 432, 159, 441, 143, 446, 143, 442, 161, 469, 199, 480, 206, 480, 210, 461, 204 }, fill)
    L.poly(img, { A.pathStartX, 270, 404, 256, 424, 240, 450, 226, 480, 216, 480, 270 }, side)
    L.poly(img, { A.pathStartX + 3, 270, 407, 257, 426, 242, 452, 228, 480, 219, 480, 270 }, fill)
    L.line(img, 408, 262, 428, 245, light)
    L.line(img, 430, 243, 478, 222, light)
    L.seed(1208)
    for _ = 1, 60 do
      local x, y = L.randi(400, 478), L.randi(222, 269)
      if L.get(img, x, y) == fill then L.px(img, x, y, light) end
    end
  end

  -- The family's woven mat on the grass, left of centre.
  local function mat(img)
    local m = A.mat
    local pts = { m.x0, m.top, m.x1, m.top, m.x1 + 10, m.bottom, m.x0 - 10, m.bottom }
    L.ellipse(img, (m.x0 + m.x1) / 2, m.bottom + 1, (m.x1 - m.x0) / 2 + 12, 3, R.grass[3])
    L.poly(img, { pts[1] - 1, pts[2] - 1, pts[3] + 1, pts[4] - 1, pts[5] + 2, pts[6] + 1,
      pts[7] - 2, pts[8] + 1 }, O)
    L.poly(img, pts, R.mat[2])
    for y = m.top, m.bottom do
      local t = (y - m.top) / (m.bottom - m.top)
      local x0 = math.floor(m.x0 - 10 * t)
      local x1 = math.floor(m.x1 + 10 * t)
      for x = x0, x1 do
        -- The weave: light and dark strips that cross every 4 pixels.
        local u = math.floor((x - m.x0 + 10 * t) / 4)
        local v = math.floor((y - m.top) / 3)
        if (u + v) % 2 == 0 and (x + y) % 2 == 0 then L.px(img, x, y, R.mat[1]) end
        if (y - m.top) % 3 == 2 then L.px(img, x, y, R.mat[3]) end
      end
    end
    L.hline(img, m.x0, m.x1, m.top, R.mat[1])
    L.hline(img, m.x0 - 9, m.x1 + 9, m.bottom, R.mat[3])
  end

  function M.background()
    local img = L.newImage(480, 270)
    sky(img)
    ground(img)
    houses(img)
    path(img)
    school(img)
    distantTeacher(img)
    mat(img)
    return img
  end

  -- The radio: a small red radio on the mat with a handle and an antenna. The "on" frames add
  -- yellow sound arcs above it, so the waves stay over the mat.
  local function radio(waves)
    local w, h = table.unpack(A.radioSize)
    local img = L.newImage(w, h)
    local body = L.newImage(w, h)
    -- The case, a speaker grille on the left, a round tuning dial on the right, a handle on top
    -- and an antenna leaning to the left.
    box(body, 2, 16, 23, 13, R.radio)
    L.rect(body, 5, 19, 9, 8, R.metal[3])
    for y = 19, 26, 2 do L.hline(body, 5, 13, y, R.metal[1]) end
    L.circle(body, 19, 22, 3, R.metal[2])
    L.circle(body, 19, 22, 1, R.metal[3])
    L.px(body, 18, 20, R.metal[1])
    L.hline(body, 8, 19, 12, R.metal[2])
    L.vline(body, 8, 13, 15, R.metal[2])
    L.vline(body, 19, 13, 15, R.metal[2])
    L.line(body, 5, 15, 2, 6, R.metal[2])
    L.outline(body, O)
    L.blit(img, body, 0, 0)
    if waves > 0 then
      -- Sound arcs over the handle: two in the first frame, three in the second.
      local c = R.wave
      local function arc(r)
        L.line(img, 13 - r, 9 - math.floor(r / 2), 13 - math.floor(r / 2), 9 - r, c)
        L.hline(img, 13 - math.floor(r / 2), 14 + math.floor(r / 2), 9 - r, c)
        L.line(img, 14 + math.floor(r / 2), 9 - r, 14 + r, 9 - math.floor(r / 2), c)
      end
      arc(2); arc(5)
      if waves > 1 then arc(8) end
    end
    return img
  end

  function M.radioOff() return radio(0) end
  function M.radioOnFrames() return { radio(1), radio(2) } end

  -- The neighbours: three Grown-up figures in new clothes, standing close together. Each one
  -- is a v2 Grown-up frame with its clothes recoloured below the shoulders (row 20), so the
  -- faces keep the Grown-ups' own colours. Frame 1 is a nod (the breathing frame of idle) and a
  -- hand gesture (Mom's wave).
  local function recolorRows(img, y0, y1, map)
    for y = y0, y1 do
      for x = 0, img.width - 1 do
        local c = img:getPixel(x, y)
        if map[c] then img:drawPixel(x, y, map[c]) end
      end
    end
  end

  local GREEN = { K.p, R.leaf[2], K.r, R.leaf[1], K.q, R.leaf[3] }
  local YELLOW = { K.W, P.yellow3, K.w, P.yellow, K.v, P.yellow2 }
  local BROWN = { K.m, R.wood[1], K.N, R.wood[2], K.n, R.wood[3] }
  local PURPLE = { K.m, rgb(170, 132, 220), K.N, P.purple, K.n, P.purple2 }
  local GRAY = { K.j, P.gray1, K.H, P.gray2, K.h, P.gray3 }
  local function toMap(list)
    local map = {}
    for i = 1, #list, 2 do map[list[i]] = list[i + 1] end
    return map
  end

  local function neighbour(who, pose, f, clothes, hair)
    local img = GU.frames(who, pose)[f + 1]
    for _, list in ipairs(clothes) do recolorRows(img, 20, img.height - 1, toMap(list)) end
    if hair then recolorRows(img, 0, 19, toMap(hair)) end
    return img
  end

  local function neighboursFrame(f)
    local w, h = table.unpack(A.neighboursSize)
    local img = L.newImage(w, h)
    -- An older neighbour with grey hair and glasses, a neighbour in green, and one in yellow.
    L.ellipse(img, 43, 61, 42, 1, R.grass[3])
    L.blit(img, neighbour("teacher", "idle", f, { PURPLE }, GRAY), 0, 0)
    L.blit(img, neighbour("dad", "idle", f, { YELLOW, BROWN }), 50, 0)
    L.blit(img, neighbour("mom", f == 1 and "wave" or "idle", f, { GREEN }), 25, 0)
    return img
  end

  function M.neighboursFrames() return { neighboursFrame(0), neighboursFrame(1) } end

  return M
end
