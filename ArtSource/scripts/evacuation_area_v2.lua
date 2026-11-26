-- The v2 evacuation area for Earthquake Hard 7 (phase after, nothing shakes): a later gathering
-- at a fictional evacuation centre in an open-sided covered basketball court, in daylight. The
-- court follows the school court reference (a broad rust-red metal roof, exposed trusses and pale
-- columns with open sides). Left to right: the plain "EVACUATION CENTER" sign hanging from the
-- truss above the hoop, the family's woven mat (Mom sits on it as a Grown-up actor, and the radio
-- is a prop on it), two tents at the back of the court with an adult and a child at the first
-- tent's opening, the open court where the Student starts, the neighbours chatting (a two-frame
-- moving part), two more people at the back, and the open right side, where the path
-- leaves the court and climbs toward the houses. A single leaning road sign beside the path shows
-- that the earthquake has happened; there is no fire, smoke or hurt person.
-- Settings are 480x270 art pixels. Positions are art pixels from the top left, and character
-- points are at the feet. The Act-out points are recorded in ArtSource/README-rollout.md.
return function(L, ST, GU)
  local M = {}
  local P = L.P
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- Ramps (light, base, shadow). The roof and columns use the Hard menu court's colours.
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    lower = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    roof = { P.roof3, P.roof, P.roof2 },
    rust = { rgb(196, 102, 78), rgb(160, 72, 58), rgb(116, 48, 44) },
    column = { rgb(252, 242, 216), rgb(232, 218, 186), rgb(198, 182, 148) },
    floor = { P.concrete, P.concrete2, P.concrete3 },
    court = { rgb(170, 214, 190), rgb(142, 192, 168), rgb(112, 160, 140) },
    grass = { P.green4, P.grass, P.grass2 },
    board = { rgb(252, 252, 248), rgb(228, 232, 238), rgb(190, 196, 210) },
    tentBlue = { P.blue3, P.blue, P.blue2 },
    tentOrange = { rgb(255, 190, 120), P.orange, P.orange2 },
    mat = { rgb(244, 214, 150), rgb(222, 182, 112), rgb(184, 140, 80) },
    radio = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) },
    wave = rgb(252, 209, 22),
  }

  M.LAYOUT = {
    floorY = 230,
    roofBottom = 64, chordY = 80, backEdgeY = 154, roofEndX = 438,
    columns = { 12, 108, 204, 300, 396 }, columnWidth = 10,
    slabEdge = { 452, 155, 394, 269 },
    sign = { 14, 86, 88, 26 },
    hoop = { 24, 116 }, hoopBaseY = 168,
    -- Each tent is { left x, base row, width }. People stand with their feet at { x, row }.
    tents = { blue = { 118, 158, 50 }, orange = { 310, 158, 52 } },
    tentPeople = { adult = { 128, 160 }, child = { 140, 161 } },
    backPeople = { adult = { 374, 156 }, child = { 388, 157 } },
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

  local function noise(img, x0, y0, x1, y1, from, to, count, seed)
    L.seed(seed)
    for _ = 1, count do
      local x, y = L.randi(x0, x1), L.randi(y0, y1)
      if L.get(img, x, y) == from then L.px(img, x, y, to) end
    end
  end

  -- Draws into a clear layer, outlines the layer once and copies it onto the image.
  local function outlined(img, draw)
    local layer = L.newImage(img.width, img.height)
    draw(layer)
    L.outline(layer, O)
    L.blit(img, layer, 0, 0)
  end

  -- The outdoors seen through the open sides: sky, hills, a tree line and the far grass.
  local function outdoors(img)
    L.vgrad(img, 0, 0, 480, 140, { P.sky3, P.sky4, P.sky4, P.sky5 })
    L.poly(img, { 0, 128, 60, 116, 130, 112, 200, 120, 270, 110, 340, 114, 420, 104,
      479, 110, 479, 140, 0, 140 }, P.hill)
    outlined(img, function(layer)
      L.seed(1209)
      for x = -6, 486, 12 do
        local cx, cy, r = x + L.randi(-2, 2), 128 + L.randi(-3, 2), L.randi(7, 10)
        L.circle(layer, cx + 1, cy + 2, r, R.leaf[3])
        L.circle(layer, cx, cy, r - 1, R.leaf[2])
        L.circle(layer, cx - 2, cy - 2, math.max(1, math.floor(r * 0.5)), R.leaf[1])
      end
      L.rect(layer, 0, 128, 480, 14, R.leaf[3])
    end)
    L.rect(img, 0, 140, 480, A.backEdgeY - 140, R.grass[2])
    L.hline(img, 0, 479, 140, R.grass[3])
    noise(img, 0, 141, 479, A.backEdgeY - 1, R.grass[2], R.grass[1], 160, 1210)
  end

  -- The houses outside the open right side.
  local function houses(img)
    local function house(x, y, w, ramp)
      box(img, x, y, w, 20, R.wall)
      L.poly(img, { x - 4, y + 1, x + math.floor(w / 2), y - 12, x + w + 3, y + 1 }, O)
      L.poly(img, { x - 2, y, x + math.floor(w / 2), y - 10, x + w + 1, y }, ramp[2])
      L.line(img, x - 1, y - 1, x + math.floor(w / 2), y - 9, ramp[1])
      box(img, x + 4, y + 6, 8, 7, R.glass)
      L.rect(img, x + w - 10, y + 8, 6, 12, R.wood[3])
    end
    house(414, 118, 26, R.roof)
    house(448, 112, 30, R.lower)
  end

  -- The court floor: a concrete slab under the roof with a painted court and white lines. The
  -- slab's right edge runs from (452, 155) down to (394, 269); outside it is grass and the path.
  local function floor(img)
    local y0 = A.backEdgeY
    L.rect(img, 0, y0, 480, 270 - y0, R.grass[2])
    noise(img, 380, y0, 479, 269, R.grass[2], R.grass[1], 260, 1211)
    noise(img, 380, y0, 479, 269, R.grass[2], R.grass[3], 160, 1212)
    local ex0, ey0, ex1, ey1 = table.unpack(A.slabEdge)
    L.poly(img, { 0, y0, ex0 + 1, y0, ex1 + 1, 270, 0, 270 }, O)
    L.poly(img, { 0, y0 + 1, ex0, ey0, ex1, ey1, 0, 269 }, R.floor[2])
    L.hline(img, 0, ex0 - 1, y0 + 1, R.floor[3])
    noise(img, 0, y0 + 2, 450, 269, R.floor[2], R.floor[3], 260, 1213)
    noise(img, 0, y0 + 2, 450, 269, R.floor[2], R.floor[1], 200, 1214)
    -- The painted court inside its white boundary: the back sideline, the left baseline and the
    -- right baseline lean with the floor, and the front sideline is below the screen.
    L.poly(img, { 34, 162, 404, 162, 372, 269, 0, 269, 0, 196 }, P.white)
    L.poly(img, { 36, 164, 402, 164, 370, 269, 0, 269, 0, 199 }, R.court[2])
    noise(img, 0, 165, 400, 269, R.court[2], R.court[3], 220, 1215)
    noise(img, 0, 165, 400, 269, R.court[2], R.court[1], 160, 1216)
    -- The centre line and centre circle, and the key in front of the hoop at the left end.
    L.ellipse(img, 230, 205, 26, 9, P.white)
    L.ellipse(img, 230, 205, 25, 8, R.court[2])
    L.line(img, 236, 164, 222, 269, P.white)
    L.ellipse(img, 70, 187, 10, 16, P.white)
    L.ellipse(img, 70, 187, 9, 15, R.court[2])
    L.poly(img, { 28, 170, 76, 170, 60, 204, 0, 204, 0, 190 }, P.white)
    L.poly(img, { 29, 171, 75, 171, 59, 203, 0, 203, 0, 192 }, R.court[3])
  end

  -- The path back toward the houses: it starts on the bottom row at about x 395, just outside the
  -- court's slab, leaves the right edge, and a far stretch climbs to the houses. It is concrete,
  -- like the slab.
  local function path(img)
    local side, fill, light = R.floor[3], R.floor[2], R.floor[1]
    L.poly(img, { 448, 138, 456, 138, 462, 158, 480, 178, 480, 188, 470, 182, 452, 160 }, side)
    L.poly(img, { 449, 139, 455, 139, 460, 158, 480, 180, 480, 185, 469, 180, 453, 159 }, fill)
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

  -- The leaning road sign beside the path: a short post tilted to the left, a blank yellow board.
  local function roadSign(img)
    L.line(img, 474, 204, 466, 180, O, 3)
    L.line(img, 474, 203, 467, 181, R.metal[2])
    L.poly(img, { 457, 170, 472, 172, 470, 185, 455, 183 }, O)
    L.poly(img, { 458, 172, 471, 173, 469, 184, 456, 182 }, P.yellow)
    L.line(img, 458, 172, 456, 181, P.yellow3)
  end

  -- A pale column from the bottom chord to the back of the slab, with a small concrete base.
  local function column(img, x)
    local w = A.columnWidth
    box(img, x, A.chordY, w, A.backEdgeY - A.chordY + 2, R.column)
    box(img, x - 2, A.backEdgeY - 3, w + 4, 6, R.floor)
  end

  -- The broad rust-red metal roof seen from below, the end of the roof on the right with the
  -- open sky beyond it, and the exposed trusses with a bottom chord at row 80. The roof comes
  -- this low so that it still shows at 2400 x 1080, where the top 54 rows are cut off.
  local function roof(img)
    local e = A.roofEndX
    L.rect(img, 0, 0, e + 1, A.roofBottom + 2, O)
    L.rect(img, 0, 0, e, A.roofBottom, R.rust[2])
    -- The ribs of the corrugated sheets run toward the back of the court, each lit on its left.
    -- They are drawn on their own layer, so none spill past the end of the roof into the sky.
    local ribs = L.newImage(e, A.roofBottom)
    for x = -40, e + 40, 5 do
      local dx = math.floor((x - 220) * 0.16)
      L.line(ribs, x, 0, x + dx, A.roofBottom - 1, R.rust[3])
      L.line(ribs, x + 1, 0, x + 1 + dx, A.roofBottom - 1, R.rust[1])
    end
    L.blit(img, ribs, 0, 0)
    -- Dark purlins cross under the sheets.
    for _, y in ipairs({ 14, 30, 46 }) do
      L.hline(img, 0, e - 1, y, O)
      L.hline(img, 0, e - 1, y + 1, R.rust[3])
    end
    L.rect(img, 0, A.roofBottom - 3, e, 3, R.rust[1])
    L.hline(img, 0, e - 1, A.roofBottom - 1, R.rust[3])
    L.vline(img, e - 1, 0, A.roofBottom - 1, R.rust[3])
    -- Trusses: a zigzag web under the eave and a bottom chord.
    local top, chord = A.roofBottom + 1, A.chordY
    for x = 0, e - 16, 16 do
      L.line(img, x, top, x + 8, chord - 2, O, 2)
      L.line(img, x + 8, chord - 2, x + 16, top, O, 2)
      L.line(img, x + 1, top, x + 8, chord - 3, R.rust[1])
      L.line(img, x + 8, chord - 3, x + 15, top, R.rust[3])
    end
    L.rect(img, 0, chord - 2, e + 1, 4, O)
    L.hline(img, 0, e - 1, chord - 1, R.rust[1])
    L.hline(img, 0, e - 1, chord, R.rust[3])
    L.vline(img, e, top, chord + 1, O)
  end

  -- The plain sign hanging from the bottom chord on two wires: "EVACUATION CENTER" in dark
  -- letters on a pale board, left of the Caption and below the Level label at both screen sizes.
  local function evacuationSign(img)
    local x, y, w, h = table.unpack(A.sign)
    for _, off in ipairs({ 10, w - 11 }) do L.vline(img, x + off, A.chordY + 2, y - 1, O) end
    box(img, x, y, w, h, R.board)
    for i, label in ipairs({ "EVACUATION", "CENTER" }) do
      local left = x + math.floor((w - L.F.width(label, 1)) / 2)
      L.text(img, left, y + 5 + (i - 1) * 10, label, P.navy2)
    end
  end

  -- The hoop at the left end of the court: a pole, a pale backboard, an orange rim and a net. The
  -- pole stands on the court at row hoopBaseY.
  local function hoop(img)
    local x, y = table.unpack(A.hoop)
    local base = A.hoopBaseY
    L.rect(img, x + 15, y + 22, 5, base - (y + 22), O)
    L.vline(img, x + 16, y + 23, base - 2, R.metal[1])
    L.vline(img, x + 17, y + 23, base - 2, R.metal[2])
    L.vline(img, x + 18, y + 23, base - 2, R.metal[3])
    L.ellipse(img, x + 17, base, 6, 2, R.court[3])
    box(img, x, y, 34, 24, R.board)
    L.rect(img, x + 10, y + 8, 14, 10, O)
    L.rect(img, x + 11, y + 9, 12, 8, R.board[1])
    L.hline(img, x + 9, x + 25, y + 24, O)
    L.hline(img, x + 9, x + 25, y + 25, P.orange)
    L.hline(img, x + 10, x + 24, y + 26, P.orange2)
    for nx = x + 11, x + 23, 3 do L.line(img, nx, y + 27, nx + 1, y + 33, R.board[3]) end
    L.hline(img, x + 12, x + 23, y + 33, R.board[3])
  end

  -- A dome tent with a dark opening, seen from the front, at { left x, base row, width }.
  local function tent(img, at, ramp)
    local x, base, w = table.unpack(at)
    local h = math.floor(w * 0.62)
    local cx = x + math.floor(w / 2)
    local shoulder = base - math.floor(h * 0.6)
    L.ellipse(img, cx, base + 1, math.floor(w / 2) + 2, 2, R.floor[3])
    outlined(img, function(layer)
      L.poly(layer, { x, base, x + 4, shoulder, cx - 6, base - h, cx + 6, base - h,
        x + w - 4, shoulder, x + w, base }, ramp[2])
      L.poly(layer, { x + 2, base - 1, x + 5, shoulder, cx - 6, base - h + 1,
        cx - 3, base - h + 1, x + 9, shoulder + 2, x + 7, base - 1 }, ramp[1])
      L.poly(layer, { x + w - 7, base - 1, x + w - 6, shoulder + 2, cx + 6, base - h + 1,
        x + w - 3, shoulder, x + w - 1, base - 1 }, ramp[3])
      -- The poles cross over the top.
      L.line(layer, x + 3, base - 2, cx, base - h + 1, ramp[3])
      L.line(layer, x + w - 3, base - 2, cx, base - h + 1, ramp[3])
      -- The opening, with its flap tied back on the right.
      local top = base - math.floor(h * 0.7)
      L.poly(layer, { cx - 9, base, cx, top, cx + 9, base }, O)
      L.poly(layer, { cx - 7, base - 1, cx, top + 2, cx + 7, base - 1 }, P.ink2)
      L.poly(layer, { cx + 1, top + 1, cx + 10, base - 1, cx + 12, base - 6 }, ramp[1])
    end)
  end

  -- A small person in the background, seen from the front, with feet at { x, row }. An adult is 28
  -- pixels tall and a child 20. The outfit names the shirt, trousers and skin ramps and the hair
  -- colour. Original figures, not the photographed people.
  local function person(img, at, child, outfit)
    local x, y = table.unpack(at)
    local shirt, pants, hair, skin = outfit.shirt, outfit.pants, outfit.hair, outfit.skin
    local h = child and 20 or 28
    local t = L.newImage(14, h + 1)
    local head = child and 6 or 7
    local hx = 7 - math.floor(head / 2)
    L.rect(t, hx, 0, head, 3, hair)                     -- the hair
    L.rect(t, hx, 2, head, head - 1, skin[1])           -- the face
    L.vline(t, hx + head - 1, 2, head, skin[2])
    L.vline(t, hx, 2, 3, hair)
    L.vline(t, hx + head - 1, 2, 3, hair)
    L.px(t, hx + 1, 4, K.e); L.px(t, hx + head - 2, 4, K.e)
    local body = head + 1
    local legs = child and 6 or 9
    local torso = h - body - legs
    L.rect(t, 3, body, 8, torso, shirt[2])              -- the shirt
    L.vline(t, 3, body, body + torso - 1, shirt[1])
    L.vline(t, 10, body, body + torso - 1, shirt[3])
    L.rect(t, 1, body + 1, 2, torso - 2, skin[1])       -- the arms
    L.rect(t, 11, body + 1, 2, torso - 2, skin[2])
    L.rect(t, 4, body + torso, 6, legs - 2, pants[2])   -- the trousers or skirt
    L.vline(t, 4, body + torso, body + torso + legs - 3, pants[1])
    L.vline(t, 7, body + torso + 2, body + torso + legs - 3, pants[3])
    L.rect(t, 4, h - 2, 3, 2, K.B)                      -- the shoes
    L.rect(t, 8, h - 2, 3, 2, K.B)
    L.outline(t, O)
    L.ellipse(img, x, y, 7, 1, R.floor[3])
    L.blit(img, t, x - 7, y - h)
  end

  local LIGHT = { P.skin1, P.skin2 }
  local TAN = { P.tan, P.tan2 }

  -- The adult and child at the blue tent's opening, and two more people at the back on the right.
  -- Nothing sits between x 204 and 300 at the back, where the HINT button covers it at 2400 x 1080.
  local function people(img)
    person(img, A.tentPeople.adult, false, { shirt = { P.teal3, P.teal, P.teal2 },
      pants = { P.blue3, P.navy, P.navy2 }, hair = P.hair, skin = TAN })
    person(img, A.tentPeople.child, true, { shirt = { P.yellow3, P.yellow, P.yellow2 },
      pants = { P.red3, P.red, P.red2 }, hair = P.hair2, skin = LIGHT })
    person(img, A.backPeople.adult, false, { shirt = { P.red3, P.red, P.red2 },
      pants = { P.gray1, P.gray3, P.gray4 }, hair = P.hair2, skin = LIGHT })
    person(img, A.backPeople.child, true, { shirt = R.leaf,
      pants = { P.blue3, P.blue, P.blue2 }, hair = P.hair, skin = TAN })
  end

  -- The family's woven mat on the court floor, left of centre.
  local function mat(img)
    local m = A.mat
    local pts = { m.x0, m.top, m.x1, m.top, m.x1 + 10, m.bottom, m.x0 - 10, m.bottom }
    L.ellipse(img, (m.x0 + m.x1) / 2, m.bottom + 1, (m.x1 - m.x0) / 2 + 12, 3, R.court[3])
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
    outdoors(img)
    houses(img)
    floor(img)
    path(img)
    roadSign(img)
    for _, x in ipairs(A.columns) do column(img, x) end
    roof(img)
    evacuationSign(img)
    hoop(img)
    tent(img, A.tents.blue, R.tentBlue)
    tent(img, A.tents.orange, R.tentOrange)
    people(img)
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
    -- Their shadow falls on the painted court.
    L.ellipse(img, 43, 61, 42, 1, R.court[3])
    L.blit(img, neighbour("teacher", "idle", f, { PURPLE }, GRAY), 0, 0)
    L.blit(img, neighbour("dad", "idle", f, { YELLOW, BROWN }), 50, 0)
    L.blit(img, neighbour("mom", f == 1 and "wave" or "idle", f, { GREEN }), 25, 0)
    return img
  end

  function M.neighboursFrames() return { neighboursFrame(0), neighboursFrame(1) } end

  return M
end
