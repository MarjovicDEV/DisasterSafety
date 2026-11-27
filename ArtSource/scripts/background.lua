-- The shared menu art (C3, C4, C15): one 480x270 background for each menu screen, plus the
-- flag, clouds and birds that move in front of the outdoor backgrounds. The drill ground and the
-- fire station are Episode backgrounds; the classroom, schoolyard, covered court and station
-- noticeboard are Difficulty backgrounds (CONTEXT.md).
-- Canvas is 480x270 art pixels (1920x1080 at 4x). v2 style (docs/adr/0001-v2-art-and-no-bantay.md):
-- one dark outline around each object, three shades per colour, light from the left.
-- Details come from the school photo inventory (earthquake-changes-scratch/photo-facts.md);
-- no photographed people, seals or fine print are drawn. The one vehicle is the parked blue
-- tricycle in front of the school wall, as on the street photo of the gate.
--
-- Screen-safe areas, from ProjectBuilder.BuildMain and BuildMenuStudent:
--   * the title and buttons cover x 22..165, y 7..235 on the left;
--   * the greeting bubble covers about x 308..465 and starts at y 162 (1920 wide) or y 146
--     (2400 wide), so the school sign sits above y 146;
--   * at 2400x1080 the envelope crop keeps rows 27..243 only.

return function(L, ST)
  local P = L.P
  local M = {}
  local pc = app.pixelColor
  local function rgb(r, g, b) return pc.rgba(r, g, b, 255) end
  local O = (ST and ST.KEY and ST.KEY.o) or P.ink

  M.W, M.H = 480, 270
  M.POLE_X = 200          -- flagpole left column
  M.FLAG_X, M.FLAG_Y = 202, 40 -- top-left of flag sprite

  -- Three-shade ramps: light, base, shadow.
  local R = {
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    palm = { rgb(132, 200, 96), rgb(86, 158, 72), rgb(52, 112, 60) },
    trunk = { rgb(196, 172, 140), rgb(160, 136, 108), rgb(118, 98, 80) },
    redwall = { rgb(222, 108, 96), rgb(194, 80, 72), rgb(150, 56, 56) },
    trim = { rgb(98, 132, 226), rgb(58, 90, 196), rgb(38, 60, 142) },
    gate = { rgb(98, 102, 118), rgb(60, 62, 76), rgb(36, 38, 50) },
    brick = { rgb(206, 156, 118), rgb(170, 120, 88), rgb(128, 88, 66) },
    sign = { rgb(252, 252, 248), rgb(228, 232, 238), rgb(190, 196, 210) },
    navy = { rgb(72, 90, 150), rgb(40, 54, 110), rgb(26, 36, 76) },
    cream = { rgb(252, 244, 222), rgb(240, 228, 198), rgb(218, 202, 166) },
    green = { rgb(150, 230, 100), rgb(102, 200, 66), rgb(64, 152, 52) },
    board = { rgb(70, 94, 96), rgb(46, 66, 70), rgb(30, 46, 50) },
    cork = { rgb(220, 174, 118), rgb(194, 146, 94), rgb(160, 114, 70) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    rust = { rgb(196, 102, 78), rgb(160, 72, 58), rgb(116, 48, 44) },
    column = { rgb(252, 242, 216), rgb(232, 218, 186), rgb(198, 182, 148) },
    concrete = { P.concrete, P.concrete2, P.concrete3 },
    road = { rgb(222, 206, 178), rgb(202, 184, 154), rgb(170, 150, 122) },
    grass = { P.green4, P.grass, P.grass2 },
    roofgreen = { rgb(142, 196, 150), rgb(104, 166, 120), rgb(74, 128, 94) },
    roof = { P.roof3, P.roof, P.roof2 },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    fire = { rgb(240, 96, 80), rgb(208, 56, 56), rgb(150, 36, 44) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    paper = { P.white, P.paper, P.paper2 },
    yellow = { P.yellow3, P.yellow, P.yellow2 },
    blue = { P.blue3, P.blue, P.blue2 },
    pink = { rgb(255, 196, 214), rgb(255, 150, 184), rgb(214, 104, 142) },
  }

  -- Helpers --------------------------------------------------------------------

  -- An outlined box lit from the left: light top and left edges, shadow bottom and right.
  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
  end

  -- Draw an organic shape on its own layer, outline it once, then copy it down.
  local function outlined(img, draw)
    local layer = L.newImage(M.W, M.H)
    draw(layer)
    L.outline(layer, O)
    L.blit(img, layer, 0, 0)
  end

  local function sky(img, h)
    L.vgrad(img, 0, 0, M.W, h, { P.sky1, P.sky2, P.sky3, P.sky4, P.sky5 })
  end

  local function noise(img, x0, y0, x1, y1, from, to, count, seed)
    L.seed(seed)
    for _ = 1, count do
      local x, y = L.randi(x0, x1), L.randi(y0, y1)
      if L.get(img, x, y) == from then L.px(img, x, y, to) end
    end
  end

  -- Leafy canopy from blobs {cx, cy, r}: shadow down-right, light up-left.
  local function canopy(layer, blobs)
    for _, b in ipairs(blobs) do L.circle(layer, b[1] + 1, b[2] + 2, b[3], R.leaf[3]) end
    for _, b in ipairs(blobs) do L.circle(layer, b[1], b[2], b[3] - 1, R.leaf[2]) end
    for _, b in ipairs(blobs) do
      L.circle(layer, b[1] - 2, b[2] - 2, math.max(1, math.floor(b[3] * 0.55)), R.leaf[1])
    end
  end

  local function tree(img, x, baseY, blobs, trunkH)
    outlined(img, function(layer)
      L.rect(layer, x - 2, baseY - trunkH, 5, trunkH, R.wood[2])
      L.vline(layer, x - 2, baseY - trunkH, baseY - 1, R.wood[1])
      L.vline(layer, x + 2, baseY - trunkH, baseY - 1, R.wood[3])
      canopy(layer, blobs)
    end)
  end

  -- A tall palm: a curved ringed trunk and drooping fronds.
  local function palm(img, x, baseY, topY, lean, size)
    size = size or 1
    outlined(img, function(layer)
      local n = baseY - topY
      for i = 0, n do
        local t = i / n
        local px = math.floor(x + lean * (t * t) + 0.5)
        local py = baseY - i
        local ring = i % 5 == 0
        L.px(layer, px - 1, py, ring and R.trunk[2] or R.trunk[1])
        L.px(layer, px, py, ring and R.trunk[3] or R.trunk[2])
        L.px(layer, px + 1, py, R.trunk[3])
      end
      local cx, cy = x + lean, topY
      local fronds = { -2.8, -2.35, -1.85, -1.3, -0.75, -0.25, 0.3 }
      for k, a in ipairs(fronds) do
        local len = math.floor((22 + (k % 3) * 5) * size)
        local vx = math.cos(a + 0.35)
        local vy = math.sin(a + 0.35) * 0.6 - 0.25
        local fx, fy = cx, cy
        local lit = vx < 0.15
        for s = 0, len do
          fx = fx + vx
          fy = fy + vy
          vy = vy + 0.06 / size
          L.px(layer, fx, fy, lit and R.palm[1] or R.palm[2])
          L.px(layer, fx, fy + 1, R.palm[2])
          L.px(layer, fx + 1, fy + 1, R.palm[2])
          -- leaflets hang under the rib and shorten toward the tip
          local hang = math.max(1, math.floor((len - s) / 6))
          if s > 2 then
            for d = 2, 1 + hang do
              L.px(layer, fx + (vx >= 0 and -1 or 1) * (d // 2), fy + d, d == 1 + hang and R.palm[3] or R.palm[2])
            end
          end
        end
      end
      L.circle(layer, cx, cy + 1, 2, R.wood[3])
    end)
  end

  local function bush(img, cx, cy, rx, ry, flower)
    outlined(img, function(layer)
      L.ellipse(layer, cx + 1, cy + 1, rx, ry, R.leaf[3])
      L.ellipse(layer, cx, cy, rx - 1, ry - 1, R.leaf[2])
      L.ellipse(layer, cx - 2, cy - 2, math.max(1, rx - 4), math.max(1, ry - 3), R.leaf[1])
    end)
    if flower then
      L.seed(cx * 7 + cy)
      for _ = 1, rx do L.px(img, cx + L.randi(-rx + 2, rx - 2), cy + L.randi(-ry + 2, ry - 2), flower) end
    end
  end

  -- A pinned paper with ruled lines and a coloured pin. No readable fine print.
  local function paper(img, x, y, w, h, ramp, pin)
    ramp = ramp or R.paper
    box(img, x, y, w, h, ramp)
    for row = y + 5, y + h - 4, 3 do
      L.hline(img, x + 3, x + w - 4 - ((row // 3) % 3) * 2, row, ramp[3])
    end
    L.rect(img, x + w // 2 - 1, y - 1, 3, 3, O)
    L.px(img, x + w // 2, y, pin or P.red)
  end

  local function flagpole(img, groundY)
    local px = M.POLE_X
    L.vline(img, px - 1, 39, groundY, O)
    L.vline(img, px, 40, groundY, P.metal)
    L.vline(img, px + 1, 40, groundY, P.metal2)
    L.vline(img, px + 2, 39, groundY, O)
    L.circle(img, px + 0.5, 37, 2, P.yellow)
    L.px(img, px, 36, P.yellow3)
  end

  local function powerLines(img, y0, y1)
    for x = 0, M.W - 1 do
      local t = x / (M.W - 1)
      local sag = 6 * math.sin(t * math.pi)
      L.px(img, x, y0 + (y1 - y0) * t + sag, R.gate[3])
      L.px(img, x, y0 + 8 + (y1 - y0) * t + sag * 1.2, R.gate[3])
    end
  end

  local function treeLine(img, y, seed)
    outlined(img, function(layer)
      L.seed(seed)
      local blobs = {}
      for x = -10, M.W + 10, 14 do
        blobs[#blobs + 1] = { x + L.randi(-3, 3), y + L.randi(-4, 3), L.randi(9, 13) }
      end
      canopy(layer, blobs)
      L.rect(layer, 0, y, M.W, 40, R.leaf[3])
    end)
  end

  -- A school block: a pitched roof, cream walls, green doors and barred windows.
  local function schoolBlock(img, x, y, w, h, roofRamp, doors)
    box(img, x, y, w, h, R.cream)
    L.poly(img, { x - 6, y + 1, x + w + 5, y + 1, x + w - 6, y - 12, x + 5, y - 12 }, O)
    L.poly(img, { x - 4, y, x + w + 3, y, x + w - 7, y - 11, x + 6, y - 11 }, roofRamp[2])
    L.hline(img, x + 6, x + w - 7, y - 10, roofRamp[1])
    L.hline(img, x - 3, x + w + 2, y - 1, roofRamp[3])
    for k = x + 2, x + w - 3, 4 do L.line(img, k, y - 9, k - 2, y - 2, roofRamp[3]) end
    local slot = 0
    for wx = x + 6, x + w - 18, 18 do
      slot = slot + 1
      if doors and slot % 3 == 2 then
        box(img, wx, y + h - 23, 12, 22, R.green)
        L.vline(img, wx + 6, y + h - 21, y + h - 3, R.green[3])
        L.px(img, wx + 9, y + h - 12, P.yellow)
      else
        box(img, wx, y + 6, 13, 12, R.green)
        L.rect(img, wx + 2, y + 8, 9, 8, R.glass[2])
        L.hline(img, wx + 2, wx + 10, y + 8, R.glass[1])
        for b = wx + 3, wx + 10, 2 do L.vline(img, b, y + 8, y + 15, R.green[3]) end
      end
    end
  end

  local function signText(img, s, cx, y, color)
    local w = L.F.width(s, 1)
    L.text(img, math.floor(cx - w / 2), y, s, color, 1)
    return w
  end

  local function sun(img, sx, sy)
    for a = 0, 15 do
      local ang = a * math.pi / 8
      local len = (a % 2 == 0) and 25 or 21
      for r = 18, len do L.px(img, sx + math.cos(ang) * r, sy + math.sin(ang) * r, P.yellow3) end
    end
    L.circle(img, sx, sy, 15, O)
    L.circle(img, sx, sy, 14, P.yellow)
    L.circle(img, sx - 3, sy - 3, 6, P.yellow3)
  end

  -- Main Menu (C3): the Baluan Elementary School gate --------------------------------
  -- The dark horizontal gate sits between brick pillars near the centre. The red perimeter
  -- wall with its blue top edge runs to the right and carries the pale school sign above the
  -- greeting. Palms rise against the sky; the schoolyard shows through the gate bars.
  M.GATE = { x0 = 176, x1 = 296, top = 128, groundY = 206, wallTop = 112,
    sign = { 316, 124, 150, 20 }, tricycle = { 330, 177 } }

  function M.gate()
    local G = M.GATE
    local img = L.newImage(M.W, M.H)
    sky(img, 176)
    powerLines(img, 84, 96)

    -- Beyond the wall: trees, a low school block with a green roof, the yard.
    treeLine(img, 104, 31)
    schoolBlock(img, 186, 160, 104, 34, R.roofgreen, true)
    L.rect(img, 0, 186, M.W, 22, R.concrete[2])
    L.hline(img, 0, M.W - 1, 186, R.concrete[1])
    noise(img, 0, 187, M.W - 1, 205, R.concrete[2], R.concrete[3], 260, 32)

    -- Palms behind the gate and the wall (drawn before the wall so the wall hides their feet).
    palm(img, 224, 186, 40, -6, 1.1)
    palm(img, 252, 186, 60, 5, 0.9)
    palm(img, 404, 150, 26, -8, 1.15)
    palm(img, 446, 150, 58, 6, 0.9)
    palm(img, 118, 150, 56, -4, 1.0)
    flagpole(img, 186)

    -- The red perimeter wall: left run, right run. Blue cap, panel seams, darker base.
    local function wall(x0, x1)
      L.rect(img, x0, G.wallTop, x1 - x0 + 1, G.groundY - G.wallTop, O)
      L.rect(img, x0, G.wallTop + 6, x1 - x0 + 1, G.groundY - G.wallTop - 7, R.redwall[2])
      L.vline(img, x0 + 1, G.wallTop + 6, G.groundY - 2, R.redwall[1])
      for x = x0 + 40, x1 - 4, 40 do
        L.vline(img, x, G.wallTop + 7, G.groundY - 3, R.redwall[3])
        L.vline(img, x + 1, G.wallTop + 7, G.groundY - 3, R.redwall[1])
      end
      L.rect(img, x0, G.groundY - 8, x1 - x0 + 1, 7, R.redwall[3])
      L.hline(img, x0, x1, G.groundY - 8, O)
      -- blue top edge
      L.rect(img, x0, G.wallTop + 1, x1 - x0 + 1, 5, R.trim[2])
      L.hline(img, x0, x1, G.wallTop + 1, R.trim[1])
      L.hline(img, x0, x1, G.wallTop + 5, R.trim[3])
      L.hline(img, x0, x1, G.wallTop + 6, O)
      -- light falls from the left: the run darkens toward its right end
      for y = G.wallTop + 7, G.groundY - 9 do
        for x = x1 - 10, x1 - 1 do
          if L.bayer(x, y) < (x - (x1 - 10)) / 14 then L.px(img, x, y, R.redwall[3]) end
        end
      end
    end
    wall(0, G.x0 - 12)
    wall(G.x1 + 12, M.W - 1)

    -- Brick pillars either side of the gate.
    local function pillar(x)
      box(img, x, G.top - 10, 12, G.groundY - G.top + 10, R.brick)
      for y = G.top - 7, G.groundY - 3, 4 do
        L.hline(img, x + 2, x + 9, y, R.brick[3])
        local off = ((y // 4) % 2 == 0) and 4 or 7
        L.px(img, x + off, y + 1, R.brick[3]); L.px(img, x + off, y + 2, R.brick[3])
      end
      box(img, x - 1, G.top - 13, 14, 4, R.concrete)
    end
    pillar(G.x0 - 12)
    pillar(G.x1)

    -- The gate leaves are their own sprite (M.gateFrames), so the guard can open them; the
    -- yard shows through the opening behind them.

    -- The pale school sign on the right wall.
    local sx, sy, sw, sh = M.GATE.sign[1], M.GATE.sign[2], M.GATE.sign[3], M.GATE.sign[4]
    L.hline(img, sx + 2, sx + sw + 1, sy + sh, R.redwall[3])
    box(img, sx, sy, sw, sh, R.sign)
    L.hline(img, sx + 3, sx + sw - 4, sy + 3, R.trim[2])
    L.hline(img, sx + 3, sx + sw - 4, sy + sh - 4, R.trim[2])
    signText(img, "BALUAN ELEMENTARY SCHOOL", sx + sw / 2, sy + 7, R.navy[2])

    -- Road and driveway in front.
    L.rect(img, 0, G.groundY, M.W, M.H - G.groundY, R.road[2])
    L.hline(img, 0, M.W - 1, G.groundY, R.road[3])
    L.poly(img, { G.x0 - 14, G.groundY, G.x1 + 14, G.groundY, G.x1 + 60, M.H, G.x0 - 60, M.H }, R.road[1])
    noise(img, 0, G.groundY + 2, M.W - 1, M.H - 1, R.road[2], R.road[3], 520, 33)
    noise(img, 0, G.groundY + 2, M.W - 1, M.H - 1, R.road[1], R.road[2], 260, 34)
    -- a strip of grass and small plants along the wall foot
    for x = 0, M.W - 1, 3 do
      if x < G.x0 - 14 or x > G.x1 + 14 then
        L.px(img, x, G.groundY, R.leaf[2]); L.px(img, x + 1, G.groundY - 1, R.leaf[1])
      end
    end
    bush(img, 30, G.groundY - 2, 10, 6, P.pink)
    bush(img, 470, G.groundY - 2, 12, 6, P.yellow)
    M.tricycle(img, M.GATE.tricycle[1], M.GATE.tricycle[2])
    return img
  end

  -- The gate leaves, 120x78 at (G.x0, G.top), in M.GATE_OPEN order: closed first, then each leaf
  -- swings in on its hinge at the pillar and looks narrower, until the gate stands open.
  M.GATE_OPEN = { 1.0, 0.72, 0.46, 0.24, 0.1 }
  function M.gateFrames()
    local G = M.GATE
    local W, top, bot = G.x1 - G.x0, 0, G.groundY - 2 - G.top
    local frames = {}
    for _, open in ipairs(M.GATE_OPEN) do
      local img = L.newImage(W, bot + 2)
      local half = W // 2
      local w = math.max(4, math.floor(half * open + 0.5))
      -- one leaf: x0..x0 + w - 1, lit on the left; the hinge post is at the pillar side
      local function leaf(x0, lit)
        local x1 = x0 + w - 1
        L.rect(img, x0, top, w, 3, O)
        L.hline(img, x0 + 1, x1 - 1, top + 1, R.gate[1])
        for y = top + 5, bot - 1, 5 do
          L.hline(img, x0, x1, y - 1, O)
          L.hline(img, x0, x1, y, R.gate[2])
          L.hline(img, x0, x1, y + 1, R.gate[3])
          L.hline(img, x0, x1, y + 2, O)
          if lit then L.hline(img, x0, x0 + math.min(30, w) * w // half, y, R.gate[1]) end
        end
        local posts = { x0, x1 - 2 }
        if w >= 20 then posts[#posts + 1] = x0 + w // 2 - 1 end
        for _, x in ipairs(posts) do
          L.rect(img, x, top, 3, bot - top + 1, O)
          L.vline(img, x + 1, top + 1, bot - 1, R.gate[2])
        end
        L.rect(img, x0, bot - 1, w, 3, O)
      end
      leaf(0, true)
      leaf(W - w, false)
      if open >= 1 then
        L.px(img, half - 3, (top + bot) // 2, R.metal[1])
        L.px(img, half + 2, (top + bot) // 2, R.metal[1])
      end
      frames[#frames + 1] = img
    end
    return frames
  end

  -- The parked blue tricycle (114x66, top-left at x, y), facing right as on the street photo: the
  -- covered blue sidecar with its oval window in front, the red motorcycle behind it, one roof
  -- over both and a windshield at the front.
  function M.tricycle(img, x, y)
    local blue = R.blue
    -- its shadow on the road
    L.ellipse(img, x + 57, y + 63, 54, 3, R.road[3])
    local t = L.newImage(76, 44)
    -- the motorcycle behind the sidecar: rear wheel, tank, seat, fork, front wheel, lamp
    L.circle(t, 50, 37, 5, O); L.circle(t, 50, 37, 2, R.metal[2])
    L.circle(t, 67, 37, 5, O); L.circle(t, 67, 37, 2, R.metal[2]); L.px(t, 66, 36, R.metal[1])
    L.line(t, 63, 20, 67, 36, R.metal[3], 2)
    L.poly(t, { 54, 20, 64, 18, 66, 24, 56, 26 }, P.red)
    L.hline(t, 56, 63, 19, P.red3)
    L.poly(t, { 64, 24, 72, 30, 70, 32, 62, 28 }, P.red2)
    L.line(t, 62, 15, 66, 14, O)
    L.rect(t, 68, 20, 2, 3, P.yellow)
    -- the roof over both, on a post at the back and the windshield at the front
    L.rect(t, 3, 0, 64, 6, blue[2])
    L.hline(t, 3, 66, 0, blue[1]); L.hline(t, 3, 66, 5, blue[3])
    L.poly(t, { 62, 6, 67, 6, 70, 17, 64, 17 }, R.glass[1])
    L.line(t, 64, 8, 66, 16, R.glass[2]); L.line(t, 66, 7, 68, 15, P.white)
    -- the sidecar wheel, mostly under the body
    L.circle(t, 14, 37, 5, O); L.circle(t, 14, 37, 2, R.metal[2])
    -- the covered sidecar body, rounded at the back
    L.poly(t, { 4, 6, 56, 6, 58, 12, 58, 35, 6, 35, 2, 30, 2, 10 }, blue[2])
    L.vline(t, 3, 10, 30, blue[1]); L.hline(t, 5, 55, 7, blue[1])
    L.rect(t, 4, 30, 54, 5, blue[3])
    L.hline(t, 4, 57, 29, blue[1])
    -- the oval window and the open side where the rider sits, with the red tank seen inside
    L.ellipse(t, 16, 14, 8, 4, O)
    L.ellipse(t, 16, 14, 7, 3, R.glass[3])
    L.hline(t, 12, 18, 12, R.glass[2])
    L.rect(t, 30, 9, 22, 17, O)
    L.rect(t, 31, 10, 20, 15, R.navy[3])
    L.poly(t, { 38, 17, 50, 15, 50, 21, 40, 22 }, P.red)
    L.hline(t, 31, 50, 10, R.navy[2])
    -- a white painted script line on the side
    for i = 0, 14, 2 do L.px(t, 9 + i, 24 + (i % 4 == 0 and 0 or 1), P.white) end
    L.outline(t, O)
    t:resize(114, 66)
    L.blit(img, t, x, y)
  end

  -- Episode selection (C4): a neutral school noticeboard ---------------------------------
  local function wallWithTrim(img, ramp, trimRamp)
    L.rect(img, 0, 0, M.W, M.H, ramp[2])
    L.vgrad(img, 0, 0, M.W, 30, { ramp[3], ramp[2] })
    L.rect(img, 0, 18, M.W, 6, trimRamp[2])
    L.hline(img, 0, M.W - 1, 18, trimRamp[1])
    L.hline(img, 0, M.W - 1, 23, trimRamp[3])
    for x = 0, M.W - 1, 8 do L.rect(img, x + 2, 20, 4, 2, trimRamp[1]) end
    L.rect(img, 0, 236, M.W, 34, R.concrete[2])
    L.hline(img, 0, M.W - 1, 236, O)
    L.rect(img, 0, 230, M.W, 6, trimRamp[2])
    L.hline(img, 0, M.W - 1, 230, trimRamp[1])
    L.hline(img, 0, M.W - 1, 235, trimRamp[3])
  end

  local function corkBoard(img, x, y, w, h, frame)
    box(img, x, y, w, h, frame or R.wood)
    box(img, x + 5, y + 5, w - 10, h - 10, R.cork)
    noise(img, x + 7, y + 7, x + w - 8, y + h - 8, R.cork[2], R.cork[3], w * h // 9, x + y)
    noise(img, x + 7, y + 7, x + w - 8, y + h - 8, R.cork[2], R.cork[1], w * h // 14, x + y + 1)
  end

  function M.noticeboard()
    local img = L.newImage(M.W, M.H)
    wallWithTrim(img, R.cream, R.green)
    corkBoard(img, 14, 36, 452, 188)
    -- Papers around the edges; the Episode cards cover the middle.
    paper(img, 26, 46, 30, 38, R.paper, P.red)
    paper(img, 62, 50, 26, 22, R.yellow, P.blue)
    paper(img, 26, 182, 40, 30, R.blue, P.yellow)
    paper(img, 412, 46, 40, 30, R.pink, P.green)
    paper(img, 420, 84, 30, 40, R.paper, P.red)
    paper(img, 406, 176, 44, 34, R.yellow, P.red)
    paper(img, 360, 50, 28, 22, R.paper, P.blue)
    paper(img, 96, 194, 30, 22, R.paper, P.green)
    -- A small star sticker and a calendar grid with no writing.
    box(img, 330, 192, 52, 22, R.paper)
    for gx = 333, 377, 7 do for gy = 196, 210, 5 do L.rect(img, gx, gy, 5, 3, R.paper[3]) end end
    L.rect(img, 333, 196, 5, 3, P.red)
    -- A wall clock and a plant on the floor beside the board.
    L.circle(img, 240, 10, 7, O); L.circle(img, 240, 10, 6, R.paper[1])
    L.line(img, 240, 10, 240, 6, O); L.line(img, 240, 10, 243, 11, O)
    box(img, 448, 232, 18, 16, R.brick)
    bush(img, 457, 224, 10, 9, nil)
    return img
  end

  -- Earthquake Episode background (C4): the school drill ground ---------------------------
  -- An open field in front of the school, with painted assembly lines, numbered class
  -- markers, the flagpole and a green Evacuation Area sign.
  function M.drillGround()
    local img = L.newImage(M.W, M.H)
    sky(img, 120)
    sun(img, 438, 36)
    treeLine(img, 106, 41)
    schoolBlock(img, 228, 96, 238, 46, R.roof, true)
    schoolBlock(img, -10, 104, 150, 38, R.roof, true)
    -- covered walkway in front of the block
    L.rect(img, 222, 138, 250, 6, O)
    L.rect(img, 222, 139, 250, 4, R.concrete[3])
    -- field
    L.rect(img, 0, 142, M.W, M.H - 142, R.grass[2])
    L.vgrad(img, 0, 142, M.W, 20, { R.grass[3], R.grass[2] })
    noise(img, 0, 150, M.W - 1, M.H - 1, R.grass[2], R.grass[3], 900, 42)
    noise(img, 0, 150, M.W - 1, M.H - 1, R.grass[2], R.grass[1], 500, 43)
    -- painted assembly lines receding toward the school
    for k = 0, 6 do
      local xTop = 140 + k * 34
      local xBot = 40 + k * 68
      L.line(img, xTop, 160, xBot, 266, R.paper[1])
    end
    L.hline(img, 130, 360, 160, R.paper[1])
    L.hline(img, 70, 420, 200, R.paper[1])
    -- class markers on short posts at the line heads
    for k = 0, 5 do
      local x = 152 + k * 34
      L.rect(img, x, 150, 1, 10, O)
      box(img, x - 4, 144, 10, 8, ({ R.yellow, R.blue, R.fire, R.green, R.pink, R.paper })[k + 1])
    end
    -- cones
    for _, c in ipairs({ { 96, 236 }, { 380, 236 }, { 120, 196 }, { 356, 196 } }) do
      L.poly(img, { c[1] - 4, c[2], c[1] + 4, c[2], c[1], c[2] - 9 }, O)
      L.poly(img, { c[1] - 3, c[2] - 1, c[1] + 3, c[2] - 1, c[1], c[2] - 8 }, P.orange)
      L.hline(img, c[1] - 2, c[1] + 1, c[2] - 4, P.white)
    end
    -- flagpole base and pole
    box(img, M.POLE_X - 12, 196, 26, 7, R.concrete)
    box(img, M.POLE_X - 8, 191, 18, 6, R.concrete)
    flagpole(img, 191)
    -- Evacuation Area sign at the left
    L.rect(img, 52, 176, 3, 28, O); L.vline(img, 53, 176, 203, R.wood[2])
    box(img, 36, 154, 36, 24, R.leaf)
    L.map(img, 47, 158, {
      "...##.......",
      "...##.......",
      ".#####......",
      "#.###.#..#..",
      "..##...######",
      ".#..#....#..",
      "#....#......",
      "......#.....",
    }, { ["#"] = P.white })
    bush(img, 470, 150, 14, 8, nil)
    tree(img, 18, 160, { { 12, 118, 18 }, { 30, 128, 14 }, { 0, 134, 14 } }, 30)
    return img
  end

  local function parkedEngine(img, ex, ey)
    outlined(img, function(layer)
      L.rect(layer, ex, ey, 150, 46, R.fire[2])
      L.hline(layer, ex, ex + 149, ey, R.fire[1])
      L.vline(layer, ex, ey, ey + 45, R.fire[1])
      L.rect(layer, ex, ey + 40, 150, 6, R.fire[3])
      -- cab
      L.rect(layer, ex + 108, ey - 20, 42, 22, R.fire[2])
      L.hline(layer, ex + 108, ex + 149, ey - 20, R.fire[1])
      L.rect(layer, ex + 116, ey - 16, 28, 12, R.glass[2])
      L.hline(layer, ex + 116, ex + 143, ey - 16, R.glass[1])
      -- ladder on top
      L.rect(layer, ex + 6, ey - 6, 98, 2, R.metal[2])
      L.rect(layer, ex + 6, ey - 2, 98, 2, R.metal[2])
      for k = ex + 8, ex + 102, 6 do L.vline(layer, k, ey - 5, ey - 2, R.metal[1]) end
      -- light bar
      L.rect(layer, ex + 118, ey - 24, 6, 4, P.blue)
      L.rect(layer, ex + 126, ey - 24, 6, 4, P.red)
      -- stripe and lockers
      L.rect(layer, ex, ey + 22, 150, 3, R.paper[1])
      for k = 0, 3 do L.rect(layer, ex + 6 + k * 24, ey + 6, 20, 14, R.fire[3]) end
      L.rect(layer, ex + 140, ey + 32, 10, 4, R.yellow[2])
      -- wheels
      for _, wx in ipairs({ ex + 26, ex + 74, ex + 126 }) do
        L.circle(layer, wx, ey + 46, 9, rgb(44, 40, 52))
        L.circle(layer, wx, ey + 46, 4, R.metal[2])
        L.px(layer, wx - 1, ey + 45, R.metal[1])
      end
    end)
  end

  M.parkedEngine = parkedEngine

  -- Fire Episode background (C4): the fire station forecourt and a parked engine ----------
  function M.fireStation()
    local img = L.newImage(M.W, M.H)
    sky(img, 140)
    treeLine(img, 128, 51)
    -- station building
    local bx, by, bw, bh = 120, 60, 330, 120
    box(img, bx, by, bw, bh, R.cream)
    box(img, bx - 4, by - 10, bw + 8, 12, R.fire)
    L.hline(img, bx - 2, bx + bw + 1, by - 8, R.fire[1])
    -- name board
    box(img, bx + bw / 2 - 52, by + 6, 104, 15, R.fire)
    signText(img, "FIRE STATION", bx + bw / 2, by + 10, P.white)
    -- two bays: the left one open with the engine, the right one shut
    box(img, bx + 18, by + 30, 128, 90, R.gate)
    L.rect(img, bx + 21, by + 33, 122, 86, rgb(50, 46, 58))
    box(img, bx + 170, by + 30, 128, 90, R.fire)
    for y = by + 36, by + 116, 6 do
      L.hline(img, bx + 172, bx + 295, y, R.fire[3])
      L.hline(img, bx + 172, bx + 295, y + 1, R.fire[1])
    end
    -- hose tower at the left
    box(img, 64, 30, 40, 150, R.cream)
    box(img, 60, 22, 48, 10, R.fire)
    for y = 46, 160, 26 do
      box(img, 76, y, 16, 12, R.green)
      L.rect(img, 78, y + 2, 12, 8, R.glass[2])
    end
    -- forecourt
    L.rect(img, 0, 180, M.W, M.H - 180, R.concrete[2])
    L.hline(img, 0, M.W - 1, 180, O)
    L.hline(img, 0, M.W - 1, 181, R.concrete[1])
    noise(img, 0, 182, M.W - 1, M.H - 1, R.concrete[2], R.concrete[3], 420, 52)
    for x = 30, M.W, 60 do L.line(img, x, 182, x - 40, M.H - 1, R.concrete[3]) end
    L.rect(img, 280, 212, 160, 4, P.yellow)
    -- the parked engine, pulled out of the open bay
    parkedEngine(img, 150, 160)
    -- a hydrant
    box(img, 452, 196, 10, 18, R.fire)
    box(img, 450, 192, 14, 5, R.fire)
    return img
  end

  -- Earthquake Easy Difficulty background (C4, C15): the classroom with green chairs -------
  -- Cream walls with green trim, the long dark board with green curtains, green window
  -- frames on the right, and rows of bright green slatted chairs seen from behind.
  local function chairBack(img, x, y, s)
    local w, h = math.floor(30 * s), math.floor(26 * s)
    outlined(img, function(layer)
      -- two posts and slats
      L.rect(layer, x, y, 3, h + math.floor(10 * s), R.green[2])
      L.rect(layer, x + w - 3, y, 3, h + math.floor(10 * s), R.green[3])
      for k = 0, 3 do
        local sy = y + 2 + k * math.floor(6 * s)
        L.rect(layer, x, sy, w, math.max(2, math.floor(3 * s)), R.green[2])
        L.hline(layer, x, x + w - 1, sy, R.green[1])
      end
      L.vline(layer, x, y, y + h, R.green[1])
    end)
  end

  function M.classroom()
    local img = L.newImage(M.W, M.H)
    -- cream wall and green trim band near the ceiling
    L.rect(img, 0, 0, M.W, 200, R.cream[2])
    L.vgrad(img, 0, 0, M.W, 24, { R.cream[3], R.cream[2] })
    L.rect(img, 0, 8, M.W, 6, R.green[2])
    L.hline(img, 0, M.W - 1, 8, R.green[1]); L.hline(img, 0, M.W - 1, 13, R.green[3])
    for x = 0, M.W - 1, 6 do L.px(img, x + 2, 10, R.green[1]); L.px(img, x + 3, 11, R.green[3]) end
    -- board with a green frame and curtains at both ends
    box(img, 70, 66, 290, 92, R.green)
    box(img, 76, 72, 278, 80, R.board)
    for y = 78, 148, 6 do
      L.hline(img, 78, 351, y, rgb(70, 110, 128))
      L.hline(img, 78, 351, y + 2, rgb(120, 70, 76))
    end
    L.vline(img, 214, 72, 151, R.green[2])
    -- wall screen above the middle of the board
    box(img, 176, 40, 74, 44, R.gate)
    L.rect(img, 179, 43, 68, 38, rgb(38, 40, 64))
    L.line(img, 182, 76, 196, 46, rgb(60, 64, 96))
    for _, cx in ipairs({ 62, 358 }) do
      outlined(img, function(layer)
        L.rect(layer, cx, 60, 14, 104, R.green[2])
        for k = cx + 2, cx + 12, 4 do L.vline(layer, k, 62, 162, R.green[3]) end
        L.vline(layer, cx + 1, 60, 163, R.green[1])
        L.rect(layer, cx - 2, 58, 18, 4, R.green[1])
      end)
    end
    -- green windows on the right wall with patterned curtains
    for _, wx in ipairs({ 392, 436 }) do
      box(img, wx, 50, 38, 74, R.green)
      L.rect(img, wx + 3, 53, 32, 68, R.glass[2])
      L.hline(img, wx + 3, wx + 34, 53, R.glass[1])
      for b = wx + 6, wx + 32, 5 do L.vline(img, b, 53, 120, R.green[3]) end
      L.rect(img, wx + 3, 53, 9, 50, R.pink[2])
      for y = 55, 101, 4 do L.px(img, wx + 6, y, R.paper[1]); L.px(img, wx + 9, y + 2, R.yellow[2]) end
    end
    -- low green shelving at the left, a stand fan
    box(img, 4, 120, 50, 80, R.green)
    L.hline(img, 6, 51, 146, R.green[3]); L.hline(img, 6, 51, 172, R.green[3])
    box(img, 12, 100, 20, 20, R.wood)
    bush(img, 22, 96, 10, 8, nil)
    -- floor
    L.rect(img, 0, 200, M.W, 70, R.concrete[2])
    L.hline(img, 0, M.W - 1, 200, O)
    L.vgrad(img, 0, 201, M.W, 20, { R.concrete[3], R.concrete[2] })
    noise(img, 0, 205, M.W - 1, M.H - 1, R.concrete[2], R.concrete[1], 300, 61)
    -- rows of green slatted chairs, back rows smaller
    for k = 0, 6 do chairBack(img, 70 + k * 50, 168, 0.8) end
    for k = 0, 5 do chairBack(img, 52 + k * 66, 196, 1.05) end
    for k = 0, 4 do chairBack(img, 20 + k * 98, 228, 1.4) end
    return img
  end

  -- Earthquake Normal Difficulty background (C4, C15): the schoolyard path ----------------
  -- A concrete path winds across the grass to the shaded walkway, the green doors and the
  -- barred windows of a classroom block.
  function M.schoolyard()
    local img = L.newImage(M.W, M.H)
    sky(img, 110)
    treeLine(img, 98, 71)
    -- classroom block with a covered walkway
    local bx, by, bw, bh = 150, 70, 320, 74
    box(img, bx, by, bw, bh, R.cream)
    L.poly(img, { bx - 10, by + 2, bx + bw + 10, by + 2, bx + bw - 6, by - 16, bx + 6, by - 16 }, O)
    L.poly(img, { bx - 8, by + 1, bx + bw + 8, by + 1, bx + bw - 7, by - 15, bx + 7, by - 15 }, R.roof[2])
    L.hline(img, bx + 7, bx + bw - 7, by - 14, R.roof[1])
    for k = bx, bx + bw, 5 do L.line(img, k, by - 13, k - 3, by, R.roof[3]) end
    -- shade under the walkway roof
    L.rect(img, bx + 1, by + 1, bw - 2, 8, R.cream[3])
    for k = 0, 7 do
      local x = bx + 14 + k * 40
      if k % 3 == 1 then
        box(img, x, by + 30, 18, 43, R.green)
        L.vline(img, x + 9, by + 32, by + 71, R.green[3])
        box(img, x + 3, by + 34, 12, 12, R.green)
        L.px(img, x + 14, by + 52, P.yellow)
      else
        box(img, x - 2, by + 18, 26, 26, R.green)
        L.rect(img, x + 1, by + 21, 20, 20, R.glass[2])
        L.hline(img, x + 1, x + 20, by + 21, R.glass[1])
        for b = x + 3, x + 20, 3 do L.vline(img, b, by + 21, by + 40, R.green[3]) end
      end
    end
    -- walkway posts and slab
    for x = bx + 4, bx + bw, 40 do
      L.rect(img, x, by + 8, 4, bh - 6, O)
      L.vline(img, x + 1, by + 9, by + bh, R.column[1])
      L.vline(img, x + 2, by + 9, by + bh, R.column[3])
    end
    box(img, bx - 6, by + bh, bw + 12, 6, R.concrete)
    -- grass
    L.rect(img, 0, 150, M.W, M.H - 150, R.grass[2])
    L.hline(img, 0, bx - 7, 150, R.grass[3])
    noise(img, 0, 151, M.W - 1, M.H - 1, R.grass[2], R.grass[3], 900, 72)
    noise(img, 0, 151, M.W - 1, M.H - 1, R.grass[2], R.grass[1], 500, 73)
    -- the winding path, edged and lit from the left
    local pts = { 300, 150, 340, 150, 330, 172, 280, 196, 240, 220, 230, 270, 120, 270, 150, 226, 210, 192, 280, 168 }
    L.poly(img, pts, O)
    local inner = { 304, 151, 336, 151, 327, 171, 277, 195, 236, 219, 226, 269, 125, 269, 153, 227, 212, 194, 282, 170 }
    L.poly(img, inner, R.concrete[2])
    L.line(img, 305, 152, 285, 167, R.concrete[1])
    L.line(img, 282, 170, 214, 193, R.concrete[1])
    L.line(img, 212, 194, 154, 228, R.concrete[1])
    for y = 160, 266, 12 do
      for x = 0, M.W - 1 do
        if L.get(img, x, y) == R.concrete[2] then L.px(img, x, y, R.concrete[3]) end
      end
    end
    -- trees, bushes, a bench
    tree(img, 60, 170, { { 52, 112, 22 }, { 78, 124, 18 }, { 34, 130, 16 }, { 62, 140, 16 } }, 44)
    bush(img, 120, 150, 14, 8, P.pink)
    bush(img, 430, 160, 16, 8, P.yellow)
    box(img, 372, 196, 46, 6, R.wood)
    L.rect(img, 376, 202, 3, 10, O); L.rect(img, 410, 202, 3, 10, O)
    return img
  end

  -- Earthquake Hard Difficulty background (C4, C15): the covered court ----------------------
  -- An open-sided court with a broad rust-red metal roof, exposed trusses and pale columns.
  function M.coveredCourt()
    local img = L.newImage(M.W, M.H)
    sky(img, 140)
    treeLine(img, 120, 81)
    schoolBlock(img, 20, 126, 120, 34, R.roofgreen, true)
    -- court floor
    L.rect(img, 0, 160, M.W, M.H - 160, R.concrete[2])
    L.hline(img, 0, M.W - 1, 160, R.concrete[3])
    noise(img, 0, 162, M.W - 1, M.H - 1, R.concrete[2], R.concrete[3], 500, 82)
    -- court lines and a centre circle
    L.poly(img, { 70, 176, 410, 176, 470, 262, 10, 262 }, P.white)
    L.poly(img, { 72, 177, 408, 177, 467, 261, 13, 261 }, rgb(120, 170, 150))
    L.poly(img, { 76, 180, 404, 180, 460, 258, 20, 258 }, rgb(142, 192, 168))
    L.vline(img, 240, 177, 261, P.white)
    L.ellipse(img, 240, 216, 26, 12, P.white)
    L.ellipse(img, 240, 216, 24, 10, rgb(142, 192, 168))
    -- hoop at the right end
    L.rect(img, 418, 130, 4, 60, O); L.vline(img, 419, 131, 189, R.metal[1]); L.vline(img, 420, 131, 189, R.metal[3])
    box(img, 400, 112, 34, 24, R.paper)
    L.rect(img, 410, 120, 14, 10, O); L.rect(img, 411, 121, 12, 8, R.paper[1])
    L.hline(img, 402, 416, 140, P.orange); L.hline(img, 403, 415, 141, R.fire[3])
    for x = 404, 414, 3 do L.line(img, x, 142, x + 1, 150, R.paper[2]) end
    -- pale columns
    for _, x in ipairs({ 34, 132, 236, 340, 444 }) do
      box(img, x, 70, 12, 120, R.column)
      box(img, x - 2, 186, 16, 6, R.concrete)
    end
    -- the broad rust-red roof with trusses underneath
    L.poly(img, { -10, 76, 490, 76, 470, 30, 10, 30 }, O)
    L.poly(img, { -8, 74, 488, 74, 468, 32, 12, 32 }, R.rust[2])
    for x = -8, 488, 4 do L.line(img, x, 33, x + (x - 240) * 0.06, 73, R.rust[3]) end
    L.hline(img, 12, 468, 32, R.rust[1])
    L.rect(img, -8, 70, 496, 5, R.rust[3])
    L.hline(img, -8, 487, 70, R.rust[1])
    L.hline(img, -8, 487, 75, O)
    -- trusses: a bottom chord and zigzag web under the eave
    L.hline(img, 0, M.W - 1, 86, R.rust[3])
    L.hline(img, 0, M.W - 1, 87, O)
    for x = 0, M.W - 1, 16 do
      L.line(img, x, 76, x + 8, 86, R.rust[3])
      L.line(img, x + 8, 86, x + 16, 76, R.rust[3])
    end
    -- low green fence in front
    L.rect(img, 0, 244, M.W, 14, O)
    L.rect(img, 0, 245, M.W, 12, R.leaf[2])
    for y = 247, 255, 3 do L.hline(img, 0, M.W - 1, y, R.leaf[3]) end
    L.hline(img, 0, M.W - 1, 245, R.leaf[1])
    L.rect(img, 0, 258, M.W, 12, R.concrete[3])
    bush(img, 160, 240, 18, 9, P.pink)
    bush(img, 330, 240, 16, 8, nil)
    return img
  end

  -- Fire Difficulty background while Fire has no Playable Level (C4): a station noticeboard
  function M.stationNoticeboard()
    local img = L.newImage(M.W, M.H)
    wallWithTrim(img, R.cream, R.fire)
    corkBoard(img, 40, 36, 400, 186, R.fire)
    paper(img, 56, 50, 34, 42, R.paper, P.red)
    paper(img, 380, 50, 42, 30, R.yellow, P.blue)
    paper(img, 60, 172, 44, 34, R.paper, P.green)
    paper(img, 372, 170, 50, 36, R.paper, P.red)
    -- a hose reel and an extinguisher on the wall beside the board
    L.circle(img, 20, 120, 13, O); L.circle(img, 20, 120, 12, R.fire[2]); L.circle(img, 20, 120, 5, R.fire[3])
    L.circle(img, 18, 118, 8, R.fire[1]); L.circle(img, 20, 120, 7, R.fire[2]); L.circle(img, 20, 120, 4, O)
    box(img, 454, 150, 14, 34, R.fire)
    box(img, 456, 144, 10, 7, R.gate)
    L.rect(img, 458, 160, 6, 8, R.paper[1])
    return img
  end

  -- Every background, in MenuBackground order: Background/bg_menu_<name>.png
  M.BACKGROUNDS = {
    { "gate", M.gate },
    { "noticeboard", M.noticeboard },
    { "drill_ground", M.drillGround },
    { "fire_station", M.fireStation },
    { "classroom", M.classroom },
    { "schoolyard", M.schoolyard },
    { "covered_court", M.coveredCourt },
    { "station_noticeboard", M.stationNoticeboard },
  }

  -- Philippine flag, 4 waving frames, 44x30.
  -- Fire Level Selection. The row of Level tiles occupies the middle of the wall.
  function M.fireEasy(home)
    return home.menuLivingRoom()
  end

  function M.fireNormal()
    local img = L.newImage(M.W, M.H)
    L.rect(img, 0, 0, M.W, 213, R.cream[2])
    for x = 1, M.W, 9 do
      L.vline(img, x, 23, 163, R.cream[3])
      L.vline(img, x + 1, 23, 163, R.cream[1])
    end
    box(img, -2, 0, 484, 24, R.wood)
    L.rect(img, 0, 164, M.W, 46, R.column[1])
    L.hline(img, 0, 479, 209, R.column[3])
    L.rect(img, 0, 210, M.W, 60, R.concrete[2])
    for x = -64, 544, 72 do
      L.line(img, x, 210, x + (x - 240) // 5, 269, R.concrete[3])
    end
    L.hline(img, 0, 479, 239, R.concrete[3])
    for _, x in ipairs({48, 284}) do
      box(img, x, 74, 61, 137, R.green)
      box(img, x+4, 79, 53, 128, R.roofgreen)
      box(img, x+10, 86, 41, 38, R.glass)
      box(img, x+10, 142, 41, 53, R.roofgreen)
      box(img, x+48, 131, 5, 4, R.yellow)
    end
    for _, x in ipairs({125, 358}) do
      box(img, x, 89, 94, 65, R.green)
      box(img, x+3, 92, 88, 59, R.glass)
      for k=x+9,x+86,10 do L.vline(img,k,93,149,R.green[3]) end
      for y=104,145,13 do L.hline(img,x+4,x+89,y,R.green[3]) end
    end
    box(img, 293, 56, 43, 14, R.leaf)
    signText(img, "EXIT", 314, 59, P.white)
    box(img, 16, 99, 23, 28, R.fire)
    L.circle(img, 27, 110, 8, O)
    L.circle(img, 27, 110, 7, R.fire[3])
    L.circle(img, 26, 109, 6, R.fire[2])
    L.circle(img, 24, 107, 2, R.fire[1])
    box(img, 26, 118, 5, 4, R.metal)
    return img
  end

  function M.fireHard()
    local img = L.newImage(M.W, M.H)
    L.vgrad(img, 0, 0, M.W, 167, {P.navy, P.purple, P.red3, P.paper2})
    L.circle(img, 424, 107, 14, P.yellow3)
    L.rect(img, 0, 167, M.W, 34, R.grass[3])
    treeLine(img, 151, 231)
    palm(img, 390, 191, 119, 6, 0.85)
    schoolBlock(img, -12, 152, 96, 48, R.rust, true)
    schoolBlock(img, 104, 157, 70, 40, R.roofgreen, true)
    schoolBlock(img, 353, 145, 127, 57, R.roof, false)
    box(img, 367, 161, 95, 35, R.wood)
    box(img, 371, 165, 87, 25, R.board)
    for k=0,12 do
      local x=374+6*k
      box(img,x,180-k%3,4,8+k%3,({R.yellow,R.green,R.fire})[k%3+1])
    end
    box(img, 364, 156, 101, 8, R.cream)
    for x=365,460,12 do L.rect(img,x,157,6,6,R.rust[2]) end
    box(img, 367, 190, 95, 7, R.wood)
    box(img, 94, 86, 6, 116, R.trunk)
    box(img, 84, 86, 26, 4, R.trunk)
    powerLines(img, 78, 106)
    L.rect(img,0,201,480,69,R.road[2])
    L.hline(img,0,479,201,R.road[3])
    noise(img,0,204,479,269,R.road[2],R.road[3],220,232)
    L.ellipse(img,100,231,78,4,R.road[3])
    parkedEngine(img, 25, 177)
    return img
  end

  function M.flagFrames()
    local frames = {}
    local FW, FH = 40, 20
    local function flat(u, v)
      -- white hoist triangle
      local half = FH / 2
      local tri = 17 * (1 - math.abs(v + 0.5 - half) / half)
      if u + 0.5 <= tri then
        local du, dv = u - 5, v - 9.5
        local d = math.sqrt(du * du + dv * dv)
        if d <= 2.4 then return P.flagSun end
        local ang = math.atan(dv, du)
        local k = ang / (math.pi / 4)
        if d <= 4.4 and math.abs(k - math.floor(k + 0.5)) < 0.18 then return P.flagSun end
        if (u == 1 and (v == 1 or v == 18)) or (u == 14 and (v == 9 or v == 10)) then return P.flagSun end
        return "w"
      end
      return v < half and "b" or "r"
    end
    for f = 0, 3 do
      local img = L.newImage(44, 30)
      local phase = f * math.pi / 2
      for u = 0, FW - 1 do
        local amp = 2.4 * (u / (FW - 1)) ^ 0.8
        local w = u / 6.5 - phase
        local dy = math.floor(amp * math.sin(w) + 0.5)
        local dark = math.cos(w) < -0.55 and u > 3
        local light = math.cos(w) > 0.75 and u > 3
        for v = 0, FH - 1 do
          local c = flat(u, v)
          if c == "w" then c = dark and P.flagWhite2 or P.flagWhite
          elseif c == "b" then c = dark and P.flagBlue2 or P.flagBlue
          elseif c == "r" then c = dark and P.flagRed2 or P.flagRed end
          if light and (c == P.flagBlue or c == P.flagRed) and v % 2 == 0 then
            c = (c == P.flagBlue) and L.hex("1a52c0") or L.hex("e02a3c")
          end
          L.px(img, 1 + u, 4 + v + dy, c)
        end
      end
      L.outline(img, P.ink2)
      frames[#frames + 1] = img
    end
    return frames
  end

  local function cloud(w, h, blobs)
    local img = L.newImage(w, h)
    local shade = L.hex("d8ecf8")
    for _, b in ipairs(blobs) do L.circle(img, b[1], b[2], b[3], P.white) end
    for y = 0, h - 1 do
      for x = 0, w - 1 do
        if L.opaque(img, x, y) and y > h * 0.62 then L.px(img, x, y, shade) end
      end
    end
    return img
  end

  function M.clouds()
    return {
      big = cloud(72, 28, { { 14, 18, 9 }, { 28, 12, 11 }, { 44, 14, 10 }, { 58, 19, 8 }, { 36, 20, 7 }, { 22, 21, 6 } }),
      small = cloud(44, 18, { { 10, 11, 6 }, { 21, 8, 7 }, { 32, 11, 6 }, { 20, 12, 5 } }),
    }
  end

  function M.birdFrames()
    local a = L.newImage(9, 6)
    L.map(a, 0, 0, { "#.......#", ".#.....#.", "..#...#..", "...#.#...", "....#...." }, { ["#"] = P.ink2 })
    local b = L.newImage(9, 6)
    L.map(b, 0, 0, { ".........", ".........", "###...###", "...#.#...", "....#...." }, { ["#"] = P.ink2 })
    return { a, b }
  end

  local function placard(img, text, cx, y)
    local w = L.F.width(text, 1) + 10
    box(img, cx - w // 2, y, w, 17, R.cream)
    signText(img, text, cx, y + 3, R.navy[3])
  end

  local function roof(img, x, y, w, h, ramp)
    L.poly(img, {x,y+h, x+20,y, x+w-20,y, x+w,y+h}, O)
    L.poly(img, {x+3,y+h-1, x+21,y+1, x+w-21,y+1, x+w-3,y+h-1}, ramp[2])
    for dx = 22,w-22,8 do
      L.line(img,x+dx,y+2,x+dx-5,y+h-2,ramp[1])
    end
    box(img,x,y+h,w,5,ramp)
  end

  function M.barangayHall()
    local img = L.newImage(480,270)
    L.vgrad(img,0,0,480,180,{R.metal[3],R.metal[2],R.sign[3],R.sign[2]})
    treeLine(img,153,317)
    L.rect(img,0,177,480,93,R.concrete[2])
    noise(img,0,178,479,269,R.concrete[2],R.concrete[3],550,18)
    for _,p in ipairs({{131,239,18},{389,222,13}}) do
      L.ellipse(img,p[1],p[2],p[3],3,R.metal[3])
      L.hline(img,p[1]-p[3]+3,p[1]+3,p[2],R.sign[2])
    end
    box(img,9,83,327,107,R.cream)
    roof(img,1,55,343,27,R.roofgreen)
    for _,x in ipairs({27,101,232,282}) do
      box(img,x,115,35,37,R.board)
      for y=118,146,7 do box(img,x+3,y,29,7,R.glass) end
    end
    box(img,157,121,48,68,R.wood)
    box(img,162,125,38,37,R.cork)
    L.circle(img,197,165,2,R.metal[1])
    roof(img,4,101,338,12,R.roofgreen)
    for _,x in ipairs({12,147,210,328}) do box(img,x,118,7,74,R.column) end
    box(img,6,190,334,5,R.concrete)
    box(img,2,195,342,5,R.concrete)
    placard(img,"BARANGAY HALL",58,62)
    box(img,357,56,3,158,R.metal)
    L.circle(img,358,56,2,R.metal[1])
    palm(img,441,191,93,17,0.91)
    box(img,401,222,5,30,R.wood)
    placard(img,"SIGNAL NO. 2",403,217)
    return img
  end

  local function roomFloor(img, y, ramp)
    L.rect(img,0,y,480,270-y,ramp[2])
    for _,row in ipairs({y+10,y+28,y+54}) do L.hline(img,0,479,row,ramp[3]) end
    for x=-120,600,55 do L.line(img,x,y,240+(x-240)*2,269,ramp[3]) end
    box(img,-2,y-6,484,7,R.wood)
  end

  function M.goBagDoorway(home,court)
    local img = L.newImage(480,270)
    L.vgrad(img,0,0,480,210,{R.cream[2],R.cream[1]})
    for x=0,479,28 do L.vline(img,x,0,202,R.cream[3]) end
    roomFloor(img,208,R.road)
    box(img,293,62,82,147,R.wood)
    for _,y in ipairs({70,137}) do box(img,300,y,68,56,R.cork) end
    L.circle(img,362,139,4,O)
    L.circle(img,362,138,3,R.yellow[2])
    local view=L.newImage(79,64)
    L.vgrad(view,0,0,79,64,{R.metal[3],R.metal[2],R.sign[3]})
    palm(view,38,80,26,10,0.65)
    box(img,382,75,85,70,R.wood)
    L.blit(img,view,385,78)
    box(img,423,78,3,64,R.wood)
    box(img,379,144,91,5,R.cork)
    for _,x in ipairs({380,409}) do box(img,x,199,5,24,R.wood) end
    box(img,375,194,44,7,R.cork)
    L.blit(img,court.radioOff(),380,165)
    L.blit(img,home.bagClosed(),338,199)
    L.blit(img,home.flashlight(),362,215)
    L.poly(img,{289,211,331,211,338,223,282,223},R.cork[3])
    for y=214,220,3 do L.hline(img,289,331,y,R.cork[1]) end
    return img
  end

  function M.rainyWindows()
    local img=L.newImage(480,270)
    L.vgrad(img,0,0,480,203,{R.cream[3],R.cream[2]})
    box(img,-2,17,484,8,R.roofgreen)
    roomFloor(img,201,R.concrete)
    box(img,12,81,96,69,R.wood)
    box(img,15,84,90,63,R.board)
    box(img,120,59,348,109,R.roofgreen)
    L.seed(271)
    for x=126,448,48 do
      for y=65,145,16 do
        L.rect(img,x,y,44,16,R.metal[2])
        L.hline(img,x,x+43,y,R.sign[2])
        L.hline(img,x,x+43,y+15,R.metal[3])
        for k=1,7 do
          local dx,dy=L.randi(3,41),L.randi(1,7)
          L.line(img,x+dx,y+dy,x+dx-2,y+dy+6,R.sign[3])
        end
      end
    end
    box(img,115,166,357,6,R.roofgreen)
    for _,p in ipairs({{42,177,0.8},{110,177,0.8},{142,218,1.1},{389,211,1.2}}) do
      local x,y,s=table.unpack(p)
      local w,h=math.floor(30*s),math.floor(26*s)
      L.poly(img,{x,y+h,x+w-1,y+h,x+w+7,y+h-7,x+8,y+h-7},R.green[3])
      box(img,x+w+4,y+h-7,3,12,R.roofgreen)
      chairBack(img,x,y,s)
    end
    return img
  end

  function M.evacuationGym()
    local img=L.newImage(480,270)
    L.vgrad(img,0,0,480,270,{R.navy[2],R.navy[1],R.metal[3]})
    L.rect(img,0,213,480,57,R.metal[3])
    L.seed(184)
    for i=1,540 do
      local x,y=L.randi(0,479),L.randi(0,266)
      L.line(img,x,y,x-1,y+4,R.metal[2])
    end
    box(img,30,72,388,140,R.concrete)
    roof(img,13,39,422,35,R.navy)
    box(img,52,99,164,72,R.board)
    for x=57,200,26 do box(img,x,104,22,60,R.cork) end
    box(img,235,102,168,111,R.wood)
    L.vgrad(img,240,107,158,96,{R.cork[2],R.cream[2],R.cream[1]})
    L.rect(img,240,175,158,36,R.road[2])
    for x=250,365,55 do box(img,x,114,24,4,R.paper) end
    for _,t in ipairs({{250,152,R.blue},{309,147,R.rust}}) do
      local x,y,r=t[1],t[2],t[3]
      L.poly(img,{x,y+28,x+19,y,x+38,y+1,x+52,y+28},O)
      L.poly(img,{x+2,y+26,x+19,y+2,x+33,y+26},r[1])
      L.poly(img,{x+21,y+2,x+37,y+3,x+49,y+26,x+35,y+26},r[2])
      L.poly(img,{x+8,y+26,x+18,y+10,x+26,y+26},r[3])
    end
    for x=257,345,44 do
      L.poly(img,{x,187,x+24,187,x+32,198,x-3,198},R.cork[3])
      for y=189,195,3 do L.hline(img,x,x+23,y,R.cork[1]) end
    end
    box(img,235,211,169,4,R.concrete)
    for _,x in ipairs({228,401}) do
      box(img,x,103,9,107,R.wood)
      box(img,x+2,111,5,38,R.glass)
      L.px(img,x+4,164,R.metal[1])
    end
    box(img,395,75,4,17,R.wood)
    placard(img,"EVACUATION CENTER",398,78)
    return img
  end

  -- Four still menus. Landmarks sit in the open bands around the menu controls.
  function M.floodMenus(root)
    local water=dofile(root.."/ArtSource/scripts/flood_fx.lua")(L,root).WATER
    local function landscape(dusk)
      local img=L.newImage(480,270)
      L.vgrad(img,0,0,480,155,dusk and {P.mount2,P.mount,P.paper2} or {P.mount,P.mount3,P.gray1})
      L.poly(img,{0,118,51,94,85,106,149,76,200,104,273,84,320,105,390,81,479,99,479,172,0,172},P.hill2)
      treeLine(img,133,717)
      L.rect(img,0,135,480,135,P.grass2)
      return img
    end
    local function cottage(img,x,y,w,paint)
      box(img,x,y,w,22,R.cream)
      roof(img,x-4,y-12,w+8,10,paint)
      box(img,x+4,y+6,10,8,R.glass)
      box(img,x+w-13,y+5,9,17,R.wood)
    end
    local function creekBand(img,upper,lower)
      for x=0,479 do
        local top,bottom=upper(x),lower(x)
        for y=top,bottom do
          local band=y%9
          local phase=x+(y//9)*17
          local c=water.mid
          if y==top or y==bottom then c=water.dark
          elseif band==2 and phase%47<17 then c=water.light
          elseif band==6 and phase%61<9 then c=water.sheen end
          L.px(img,x,y,c)
        end
      end
    end
    local function rain(img)
      local drops=Image{fromFile=root.."/Assets/Art/Level/v2/typhoon_rain_light_0.png"}
      img:drawImage(drops,Point(0,0))
    end
    local episode=landscape(false)
    palm(episode,69,131,71,9,0.57)
    for _,h in ipairs({{93,107,35},{153,111,42},{236,106,38},{371,111,43},{433,115,39}}) do
      cottage(episode,h[1],h[2],h[3],h[1]%2==0 and R.roofgreen or R.rust)
    end
    creekBand(episode,function(x) return 139+math.floor(x/25+3*math.sin(x/31)) end,
      function(x) return 170+math.floor(x/5+5*math.sin(x/35)) end)
    -- A small diagonal crossing spans the channel; foundations finish on the banks.
    for _,dx in ipairs({0,10}) do
      L.poly(episode,{2+dx,173,41+dx,132,45+dx,132,7+dx,175},P.gray4)
    end
    L.poly(episode,{3,171,41,131,54,132,16,175},P.concrete2)
    for _,dx in ipairs({0,12}) do
      for j=0,3 do box(episode,3+dx+j*12,161-j*13,3,11,R.concrete) end
      L.line(episode,4+dx,161,40+dx,122,P.ink,3)
      L.line(episode,4+dx,160,40+dx,121,P.metal,1)
    end
    box(episode,46,152,8,34,R.wood)
    for y=155,181,4 do L.hline(episode,47,50+(y%8==3 and 1 or 0),y,P.white) end
    L.rect(episode,47,153,6,3,P.red)
    for _,v in ipairs({{20,191},{84,199},{189,219},{296,248}}) do
      for j=-3,3 do
        L.line(episode,v[1]+j,v[2],v[1]+3*j,v[2]-8+(j*j)%5,P.green3)
      end
    end
    noise(episode,0,169,479,269,P.grass2,P.grass3,800,473)

    local easy=landscape(false)
    cottage(easy,379,114,46,R.roofgreen)
    palm(easy,345,151,77,10,0.7)
    rain(easy)
    -- Sheltered frontage, with rain visible only outside the eave.
    box(easy,-4,54,303,153,R.cream)
    for y=67,199,13 do L.hline(easy,0,297,y,R.cream[3]) end
    roof(easy,-23,27,332,31,R.rust)
    box(easy,15,71,99,135,R.wood)
    L.rect(easy,20,76,89,125,P.paper2)
    L.rect(easy,21,77,61,121,P.wall)
    L.poly(easy,{86,77,107,84,107,197,86,201},P.brown)
    L.line(easy,88,80,88,198,P.tan)
    L.circle(easy,102,145,2,P.yellow2)
    box(easy,29,122,46,5,R.wood)
    box(easy,35,126,4,6,R.wood)
    box(easy,67,126,4,6,R.wood)
    local function asset(n) return Image{fromFile=root.."/Assets/Art/Level/v2/"..n..".png"} end
    L.blit(easy,asset("home_workbook"),45,97)
    L.blit(easy,asset("classroom_bag"),41,100)
    for _,x in ipairs({147,220}) do
      box(easy,x,89,48,53,R.wood)
      box(easy,x+3,92,42,46,R.glass)
      box(easy,x+22,92,4,46,R.wood)
    end
    box(easy,-4,202,306,9,R.concrete)
    box(easy,-4,211,315,11,R.concrete)
    for _,x in ipairs({40,60}) do
      local boot=L.newImage(19,23)
      L.rect(boot,3,2,9,15,P.yellow2)
      L.rect(boot,4,4,4,12,P.yellow)
      L.poly(boot,{3,13,11,13,12,16,16,17,17,20,3,20},P.yellow2)
      L.hline(boot,3,16,20,P.brown2)
      L.hline(boot,3,11,2,P.brown2)
      L.outline(boot,P.ink)
      L.blit(easy,boot,x,182)
    end
    L.poly(easy,{35,222,91,222,157,269,9,269},P.tan)
    noise(easy,0,222,479,269,P.tan,P.tan2,450,237)
    bush(easy,325,192,12,8)
    bush(easy,347,195,14,9)

    local normal=landscape(false)
    cottage(normal,81,105,45,R.rust); cottage(normal,325,108,51,R.roofgreen)
    palm(normal,295,138,86,-7,0.52)
    L.rect(normal,0,143,480,58,P.gray3)
    creekBand(normal,function(x) return 150+math.floor(3*math.sin(x/28)) end,
      function(x) return 193+math.floor(2*math.sin(x/35)) end)
    L.rect(normal,0,201,480,69,P.concrete2)
    L.hline(normal,0,479,201,P.ink2)
    L.hline(normal,0,479,204,P.concrete)
    for x=8,480,63 do L.line(normal,x,205,x+(x-240)//6,269,P.concrete3) end
    L.hline(normal,0,479,240,P.concrete3)
    noise(normal,0,205,479,269,P.concrete2,P.concrete,480,829)
    box(normal,24,89,13,114,R.column);box(normal,22,86,17,6,R.concrete)
    box(normal,468,89,9,111,R.column);box(normal,466,86,13,6,R.concrete)
    box(normal,-3,116,28,4,R.roofgreen);box(normal,-3,186,28,4,R.roofgreen)
    for x=-2,20,6 do box(normal,x,113,3,78,R.roofgreen) end
    rain(normal)

    local hard=landscape(true)
    -- The low field is on the downhill side only. The uphill side remains
    -- continuous with the settlement, so the climb reads as a road on land.
    creekBand(hard,function(x) return 157+math.floor(4*math.sin(x/33)) end,function() return 269 end)
    L.poly(hard,{0,128,479,128,479,150,433,152,381,171,351,182,
      321,210,265,230,219,241,174,269,0,269},P.dirt2)
    L.poly(hard,{0,126,479,126,479,145,431,147,378,166,348,176,
      319,203,261,224,215,236,168,269,0,269},P.grass3)
    L.poly(hard,{0,127,479,127,479,139,426,140,372,158,341,169,
      311,196,258,216,210,230,158,269,0,269},P.grass2)
    -- Earthen slope: tufts follow its contour, with exposed stones at its toe.
    for _,v in ipairs({{172,261},{191,249},{218,233},{247,224},{276,214},
        {304,204},{329,190},{352,173},{390,160},{428,146},{459,141}}) do
      local x,y=v[1],v[2]
      L.line(hard,x,y,x-3,y-4,P.green3)
      L.line(hard,x,y,x+2,y-5,P.grass)
      L.ellipse(hard,x+2,y+4,3,1,P.rock2)
      L.hline(hard,x,x+2,y+3,P.rock3)
    end
    -- A paved bend with gravel shoulders and short perspective road markings.
    local verge={0,251,74,231,140,206,211,188,268,170,294,151,340,136,
      395,118,438,119,441,131,351,154,324,166,300,187,229,212,
      173,230,119,257,97,269,0,269}
    L.poly(hard,verge,P.concrete3)
    L.poly(hard,{0,255,76,234,141,209,213,191,269,173,296,153,341,138,
      396,120,436,121,437,128,350,151,322,163,298,184,228,209,
      172,226,117,254,90,269,0,269},P.gray3)
    for _,v in ipairs({{35,260,63,248},{101,234,122,225},{158,212,175,206},
        {215,194,230,189},{264,178,275,172},{299,156,307,152},{337,143,347,140}}) do
      L.line(hard,v[1],v[2],v[3],v[4],P.paper2,v[2]>200 and 2 or 1)
    end
    noise(hard,0,145,479,269,P.grass2,P.grass3,1200,83)
    noise(hard,0,142,479,269,P.gray3,P.gray4,320,74)
    cottage(hard,42,112,42,R.rust)
    cottage(hard,113,107,37,R.roofgreen)
    palm(hard,88,138,77,9,0.65)
    for x=8,82,12 do box(hard,x,147,3,11,R.wood) end
    L.hline(hard,8,82,151,P.brown)
    bush(hard,206,141,12,6)
    bush(hard,224,143,8,5)
    -- Covered entrance, illuminated transom, louvered windows and broad landing.
    box(hard,357,119,120,9,R.concrete)
    box(hard,362,76,110,42,R.cream)
    roof(hard,355,57,122,17,R.roofgreen)
    for _,x in ipairs({369,443}) do
      box(hard,x,94,21,16,R.wood)
      box(hard,x+2,96,17,11,R.yellow)
      L.vline(hard,x+10,96,106,P.brown2)
      L.hline(hard,x+2,x+18,101,P.tan)
      box(hard,x-1,110,23,3,R.concrete)
    end
    box(hard,400,91,33,28,R.wood)
    box(hard,403,93,27,5,R.glass)
    L.rect(hard,404,100,25,18,P.yellow3)
    L.vline(hard,417,100,117,P.tan2)
    L.px(hard,414,110,P.brown2)
    for _,x in ipairs({359,469}) do box(hard,x,90,4,29,R.column) end
    box(hard,397,118,39,4,R.concrete)
    box(hard,394,122,45,4,R.concrete)
    placard(hard,"EVACUATION CENTER",415,73)
    -- Route board planted on the dry grassy shoulder outside the paved bend.
    box(hard,107,217,5,16,R.wood)
    box(hard,99,198,83,24,R.board)
    L.text(hard,104,201,"EVACUATION",P.white,1)
    L.text(hard,104,212,"ROUTE",P.white,1)
    L.line(hard,163,215,174,204,P.white,3)
    L.poly(hard,{167,203,176,203,176,212},P.white)
    return {episode=episode,easy=easy,normal=normal,hard=hard}
  end

  return M
end
