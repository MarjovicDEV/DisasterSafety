-- The shared menu art (C3, C4, C15): one 480x270 background for each menu screen, plus the
-- flag, clouds and birds that move in front of the outdoor backgrounds. The drill ground and the
-- fire station are Episode backgrounds; the classroom, schoolyard, covered court and station
-- noticeboard are Difficulty backgrounds (CONTEXT.md).
-- Canvas is 480x270 art pixels (1920x1080 at 4x). v2 style (docs/adr/0001-v2-art-and-no-bantay.md):
-- one dark outline around each object, three shades per colour, light from the left.
-- Details come from the school photo inventory (earthquake-changes-scratch/photo-facts.md);
-- no photographed people, vehicles, seals or fine print are drawn.
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
    sign = { 316, 124, 150, 20 } }

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

    -- The gate: two leaves of horizontal bars in a dark frame, with gaps onto the yard.
    local gx0, gx1, top, bot = G.x0, G.x1 - 1, G.top, G.groundY - 2
    L.rect(img, gx0, top, gx1 - gx0 + 1, 3, O)
    L.hline(img, gx0 + 1, gx1 - 1, top + 1, R.gate[1])
    for y = top + 5, bot - 1, 5 do
      L.hline(img, gx0, gx1, y - 1, O)
      L.hline(img, gx0, gx1, y, R.gate[2])
      L.hline(img, gx0, gx1, y + 1, R.gate[3])
      L.hline(img, gx0, gx1, y + 2, O)
      L.hline(img, gx0, gx0 + 30, y, R.gate[1])
    end
    for _, x in ipairs({ gx0, gx0 + 30, (gx0 + gx1) // 2 - 1, gx1 - 30, gx1 - 2 }) do
      L.rect(img, x, top, 3, bot - top + 1, O)
      L.vline(img, x + 1, top + 1, bot - 1, R.gate[2])
    end
    L.rect(img, gx0, bot - 1, gx1 - gx0 + 1, 3, O)
    L.px(img, (gx0 + gx1) // 2 - 3, (top + bot) // 2, R.metal[1])
    L.px(img, (gx0 + gx1) // 2 + 3, (top + bot) // 2, R.metal[1])

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
    return img
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
    outlined(img, function(layer)
      local ex, ey = 150, 160
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

  return M
end
