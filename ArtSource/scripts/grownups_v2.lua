-- The three v2 Grown-ups (Teacher, Mom, Dad), 36x62 frames, in the style of student.lua
-- (docs/adr/0001-v2-art-and-no-bantay.md): the Student palette and nothing else, one dark
-- outline around the outside only, three shades per colour with light from the left, front
-- views built from mirrored half maps (so they are perfectly symmetric) and side walks of 4
-- frames. A head is about 1 to 3.5 of the body (the 50-pixel Student's is 1 to 2.6), so the
-- Student reads as a child beside them.
--   Teacher: navy blazer over a white blouse with a red scarf, navy skirt, hair in a bun, glasses.
--   Mom:     red top, navy trousers, long hair.
--   Dad:     white shirt with a red tie and a belt, navy trousers, short hair.
-- Every frame is bottom aligned: the lowest row (y = 61) is the outline under the shoes.

return function(L, ST)
  local M = {}
  local KEY = ST.KEY
  local FW, FH = 36, 62

  -- Char grids (0-based x, y) ------------------------------------------------
  local function newGrid(w, h)
    local g = { w = w, h = h }
    for y = 1, h do
      local row = {}
      for x = 1, w do row[x] = "." end
      g[y] = row
    end
    return g
  end
  local function getc(g, x, y)
    if x < 0 or y < 0 or x >= g.w or y >= g.h then return "." end
    return g[y + 1][x + 1]
  end
  local function setc(g, x, y, c)
    if x >= 0 and y >= 0 and x < g.w and y < g.h then g[y + 1][x + 1] = c end
  end
  local function clone(g)
    local c = newGrid(g.w, g.h)
    for y = 1, g.h do for x = 1, g.w do c[y][x] = g[y][x] end end
    return c
  end
  -- Copy src onto dst at (ox, oy). "." keeps dst, "_" clears it.
  local function paste(dst, src, ox, oy)
    for y = 0, src.h - 1 do
      for x = 0, src.w - 1 do
        local c = getc(src, x, y)
        if c == "_" then setc(dst, ox + x, oy + y, ".")
        elseif c ~= "." then setc(dst, ox + x, oy + y, c) end
      end
    end
  end
  local function flipX(g)
    local f = newGrid(g.w, g.h)
    for y = 0, g.h - 1 do for x = 0, g.w - 1 do setc(f, g.w - 1 - x, y, getc(g, x, y)) end end
    return f
  end
  -- Drop row y and add an empty row on top: the upper body sinks 1 pixel, the feet stay.
  local function squash(g, y)
    local c = clone(g)
    table.remove(c, y + 1)
    local blank = {}
    for x = 1, c.w do blank[x] = "." end
    table.insert(c, 1, blank)
    return c
  end
  local function rect(g, x, y, w, h, c)
    for j = y, y + h - 1 do for i = x, x + w - 1 do setc(g, i, j, c) end end
  end
  local function ellipse(g, cx, cy, rx, ry, c)
    for y = cy - ry, cy + ry do
      for x = cx - rx, cx + rx do
        local dx, dy = (x - cx) / rx, (y - cy) / ry
        if dx * dx + dy * dy <= 1.0 then setc(g, x, y, c) end
      end
    end
  end
  -- Light from the left on one colour of a layer: left edge light, right edge shadow.
  local function shadeLayer(g, base, light, dark)
    local src = clone(g)
    for y = 0, g.h - 1 do
      for x = 0, g.w - 1 do
        if getc(src, x, y) == base then
          if getc(src, x - 1, y) == "." and light then setc(g, x, y, light)
          elseif getc(src, x + 1, y) == "." and dark then setc(g, x, y, dark) end
        end
      end
    end
  end
  -- 4-neighbour outline around a layer.
  local function outline(g)
    local src = clone(g)
    for y = 0, g.h - 1 do
      for x = 0, g.w - 1 do
        if getc(src, x, y) == "." then
          local function filled(a, b) local c = getc(src, a, b); return c ~= "." and c ~= "o" end
          if filled(x - 1, y) or filled(x + 1, y) or filled(x, y - 1) or filled(x, y + 1) then setc(g, x, y, "o") end
        end
      end
    end
  end
  local TONES = {
    w = { "W", "v" }, N = { "m", "n" }, p = { "r", "q" }, S = { "t", "s" }, H = { "j", "h" }, B = { "b", nil },
  }
  local function shadeAll(g, bases)
    for _, b in ipairs(bases) do shadeLayer(g, b, TONES[b][1], TONES[b][2]) end
  end
  local function fromRows(rows)
    local w = 0
    for _, r in ipairs(rows) do w = math.max(w, #r) end
    local g = newGrid(w, #rows)
    for y, r in ipairs(rows) do
      for x = 1, #r do g[y][x] = r:sub(x, x) end
    end
    return g
  end

  local function toImage(g)
    local img = L.newImage(FW, FH)
    local oy = FH - g.h
    for y = 0, g.h - 1 do
      for x = 0, g.w - 1 do
        local c = KEY[getc(g, x, y)]
        if c then img:drawPixel(x, y + oy, c) end
      end
    end
    return img
  end

  -- Half maps ----------------------------------------------------------------
  -- Right-aligned tails: the last character is the column next to the middle of the figure
  -- (x = 17), and is mirrored to x = 18. "." is empty. Row i is drawn at y0 + i - 1.
  local function halfRows(g, tails, y0)
    for i, t in ipairs(tails) do
      assert(#t <= 18, "half row " .. i .. " is " .. #t .. " wide: " .. t)
      local x0 = 18 - #t
      for k = 1, #t do
        local c = t:sub(k, k)
        if c ~= "." then
          setc(g, x0 + k - 1, y0 + i - 1, c)
          setc(g, FW - 1 - (x0 + k - 1), y0 + i - 1, c)
        end
      end
    end
  end
  -- Fill the half row from xl to the middle, and its mirror.
  local function hrow(g, y, xl, c)
    for x = xl, 17 do setc(g, x, y, c); setc(g, FW - 1 - x, y, c) end
  end
  local function sym(g, x, y, c)
    setc(g, x, y, c)
    setc(g, FW - 1 - x, y, c)
  end

  -- Heads, front (rows start at y = 1) ---------------------------------------
  local DAD_HEAD = {
    "jjjjjjjj",
    "jjjHHHHHHH",
    "jHHHHHHHHH",
    "HHHHHHHHHH",
    "HHHHHHHHHH",
    "HHHHHHHHHH",
    "HHssssssss",
    "HSSSSSSSSS",
    "HSSHHHSSSS",
    "SSSSkeSSSS",
    "SSSSeeSSSS",
    "SSSSSSSSSS",
    "SSSSSSSSSs",
    "SSSSSqSSSS",
    "SSSSSSqqqq",
    "SSSSSSSSS",
    "SSSSSSSS",
  }
  local MOM_HEAD = {
    "jjjjjjjj",
    "jjjjHHHHHH",
    "jjjHHHHHHHH",
    "jjHHHHHHHHH",
    "jHHHHHHHHHH",
    "HHHHHHHHHHH",
    "HHHHsssssss",
    "HHSSSSSSSSS",
    "HHSShhhSSSS",
    "HHSSSkeSSSS",
    "HHSSSeeSSSS",
    "HHSSSSSSSSS",
    "HHSrrSSSSSs",
    "HHSSSSqSSSS",
    "HHSSSSSqqqq",
    "HHSSSSSSSSS",
    "HHHSSSSSSSS",
    "HHHH.......",
    "HHHH.......",
    "HHHH.......",
    "HHHH.......",
    "HHHH.......",
    "HHHH.......",
    "HHHH.......",
    "HHHh.......",
    ".HHh.......",
    ".hhh.......",
  }
  -- The Teacher's bun sits on top, so her face is 2 rows lower than Mom's and Dad's.
  local TEACHER_HEAD = {
    "jjjj",
    "jjjHHH",
    "jHHHHH",
    "jjjHHHHHHH",
    "jjHHHHHHHH",
    "jHHHHHHHHH",
    "HHHHHHHHHH",
    "HHHHHHHHHH",
    "HHssssssss",
    "HSSSSSSSSS",
    "HSSvvvvSSS",
    "HSvvtevvvv",
    "HSSvtevSSS",
    "HSSvvvvSSS",
    "SSSSSSSSSs",
    "SSSSSqSSSS",
    "SSSSSSqqqq",
    "SSSSSSSSS",
    "SSSSSSSS",
  }
  local FACE = { dad = { ey = 10 }, mom = { ey = 10 }, teacher = { ey = 12 } }
  local TOP = { dad = 20, mom = 20, teacher = 22 } -- the shoulder row

  local function headLayer(who)
    local g = newGrid(FW, FH)
    halfRows(g, who == "dad" and DAD_HEAD or who == "mom" and MOM_HEAD or TEACHER_HEAD, 1)
    if who == "dad" then for y = 10, 12 do sym(g, 7, y, "S") end end -- ears
    shadeAll(g, { "H", "S" })
    return g
  end

  local function neckLayer(who)
    local g = newGrid(FW, FH)
    local top = TOP[who]
    local half = who == "dad" and 14 or 15
    for y = top - 2, top - 1 do hrow(g, y, half, "S") end
    shadeAll(g, { "S" })
    hrow(g, top - 2, half + 1, "s") -- the shadow under the chin
    return g
  end

  -- Faces: each edits the head layer after it is drawn ---------------------------
  local function clearEyes(g, ey)
    for _, x in ipairs({ 11, 12, 13, 14 }) do for y = ey - 1, ey + 1 do sym(g, x, y, "S") end end
  end
  local function clearMouth(g, ey)
    for x = 13, 17 do for y = ey + 4, ey + 5 do sym(g, x, y, "S") end end
  end
  local FACES = {
    joy = function(g, ey) -- happy: closed smiling eyes, open smile with a tongue
      clearEyes(g, ey)
      sym(g, 11, ey + 1, "e"); sym(g, 12, ey, "e"); sym(g, 13, ey, "e"); sym(g, 14, ey + 1, "e")
      clearMouth(g, ey)
      for x = 13, 17 do sym(g, x, ey + 4, "q") end
      sym(g, 14, ey + 5, "q"); sym(g, 15, ey + 5, "q"); sym(g, 16, ey + 5, "p"); sym(g, 17, ey + 5, "p")
    end,
    hurt = function(g, ey) -- pain: eyes squeezed shut as > <, a small frown
      clearEyes(g, ey)
      sym(g, 11, ey - 1, "e"); sym(g, 12, ey, "e"); sym(g, 11, ey + 1, "e")
      clearMouth(g, ey)
      for x = 16, 17 do sym(g, x, ey + 4, "q") end
      sym(g, 14, ey + 5, "q"); sym(g, 15, ey + 5, "q")
    end,
  }

  -- Torsos, front --------------------------------------------------------------
  local function torsoLayer(who)
    local g = newGrid(FW, FH)
    local top = TOP[who]
    if who == "dad" then
      hrow(g, 20, 12, "w"); hrow(g, 21, 10, "w")
      for y = 22, 37 do hrow(g, y, 9, "w") end
      shadeAll(g, { "w" })
      hrow(g, 38, 10, "x")
      -- the collar and the V of the neck, then the red tie
      for _, x in ipairs({ 16, 17 }) do sym(g, x, 20, "S") end
      sym(g, 17, 21, "S")
      for _, p in ipairs({ { 14, 20 }, { 15, 20 }, { 15, 21 }, { 16, 21 } }) do setc(g, p[1], p[2], "W") end
      for _, p in ipairs({ { 21, 20 }, { 20, 20 }, { 20, 21 }, { 19, 21 } }) do setc(g, p[1], p[2], "w") end
      setc(g, 17, 22, "r"); setc(g, 18, 22, "p")
      for y = 23, 26 do setc(g, 17, y, "p"); setc(g, 18, y, "q") end
      for y = 27, 30 do
        setc(g, 16, y, "p"); setc(g, 17, y, "p"); setc(g, 18, y, "q"); setc(g, 19, y, "q")
      end
      setc(g, 17, 31, "p"); setc(g, 18, 31, "q")
    elseif who == "mom" then
      hrow(g, 20, 12, "p"); hrow(g, 21, 10, "p")
      for y = 22, 29 do hrow(g, y, 9, "p") end
      for y = 30, 36 do hrow(g, y, 10, "p") end
      shadeAll(g, { "p" })
      hrow(g, 36, 10, "q") -- the hem
      for x = 15, 17 do sym(g, x, 20, "S") end
      for x = 16, 17 do sym(g, x, 21, "S") end
      sym(g, 17, 22, "S")
      shadeLayer(g, "S", "t", "s")
    else
      hrow(g, top, 12, "N"); hrow(g, top + 1, 10, "N")
      for y = top + 2, 31 do hrow(g, y, 9, "N") end
      for y = 32, 39 do hrow(g, y, 10, "N") end
      shadeAll(g, { "N" })
      hrow(g, 39, 10, "n") -- the hem of the blazer
      -- the white blouse between the lapels, narrowing to the waist
      local blouse = newGrid(FW, FH)
      for y = top, 31 do hrow(blouse, y, 15, "w") end
      for y = 32, 38 do hrow(blouse, y, 16, "w") end
      shadeAll(blouse, { "w" })
      paste(g, blouse, 0, 0)
      for y = top + 1, 30 do setc(g, 14, y, "m"); setc(g, 21, y, "n") end -- the lapels' edges
      -- the red scarf: a knot and two tails
      for x = 15, 17 do sym(g, x, top, "p") end
      for x = 16, 17 do sym(g, x, top + 1, "p") end
      setc(g, 17, top + 2, "r"); setc(g, 18, top + 2, "q")
      setc(g, 17, top + 3, "p"); setc(g, 18, top + 3, "q")
      setc(g, 17, top + 4, "p"); setc(g, 18, top + 4, "q")
    end
    return g
  end

  -- Legs, front ------------------------------------------------------------------
  local function legsLayer(who)
    local g = newGrid(FW, FH)
    if who == "teacher" then
      -- the skirt, an A-line to the knee, over bare legs with white socks
      local skirt = newGrid(FW, FH)
      for y = 40, 50 do
        local xl = y <= 42 and 10 or y <= 45 and 9 or y <= 48 and 8 or 7
        hrow(skirt, y, xl, "N")
      end
      shadeAll(skirt, { "N" })
      hrow(skirt, 50, 7, "n")
      for y = 51, 57 do for x = 12, 15 do sym(g, x, y, (y >= 56) and "w" or "S") end end
      shadeAll(g, { "w", "S" })
      local shoes = newGrid(FW, FH)
      for y = 58, 60 do for x = 11, 16 do sym(shoes, x, y, "B") end end
      shadeAll(shoes, { "B" })
      paste(g, skirt, 0, 0)
      paste(g, shoes, 0, 0)
      return g
    end
    local ptop = (who == "dad") and 39 or 37
    for y = ptop, 46 do hrow(g, y, 10, "N") end
    for y = 47, 57 do for x = 10, 16 do sym(g, x, y, "N") end end
    shadeAll(g, { "N" })
    for x = 10, 16 do sym(g, x, 57, "n") end -- the cuffs
    local shoes = newGrid(FW, FH)
    for y = 58, 60 do for x = 9, 16 do sym(shoes, x, y, "B") end end
    shadeAll(shoes, { "B" })
    paste(g, shoes, 0, 0)
    return g
  end

  -- Limbs ------------------------------------------------------------------------
  local function square(g, cx, cy, size, c)
    local x0 = math.floor(cx + 0.5) - size // 2
    local y0 = math.floor(cy + 0.5) - size // 2
    for y = y0, y0 + size - 1 do for x = x0, x0 + size - 1 do setc(g, x, y, c) end end
  end
  -- Points every half pixel along a polyline, with how far along it each is (0 to 1).
  local function along(pts)
    local len = 0
    for i = 1, #pts - 2, 2 do len = len + math.sqrt((pts[i + 2] - pts[i]) ^ 2 + (pts[i + 3] - pts[i + 1]) ^ 2) end
    local out, run = {}, 0
    for i = 1, #pts - 2, 2 do
      local ax, ay, bx, by = pts[i], pts[i + 1], pts[i + 2], pts[i + 3]
      local seg = math.sqrt((bx - ax) ^ 2 + (by - ay) ^ 2)
      local n = math.max(math.ceil(seg * 2), 1)
      for s = 0, n do
        out[#out + 1] = { ax + (bx - ax) * s / n, ay + (by - ay) * s / n, (run + seg * s / n) / len }
      end
      run = run + seg
    end
    return out
  end
  local WHO_SLEEVE = { teacher = { "N", 0.85 }, mom = { "p", 0.45 }, dad = { "w", 0.5 } }
  local LIGHT = { w = "W", N = "m", p = "r" }
  local DARK = { w = "v", N = "n", p = "q" }
  -- An arm along pts (shoulder -> elbow -> hand): sleeve, bare forearm and a hand. Unshaded.
  local function armBase(who, pts)
    local sleeve, f = WHO_SLEEVE[who][1], WHO_SLEEVE[who][2]
    local g = newGrid(FW, FH)
    local samples = along(pts)
    for _, s in ipairs(samples) do square(g, s[1], s[2], 3, "S") end
    for _, s in ipairs(samples) do if s[3] <= f then square(g, s[1], s[2], 4, sleeve) end end
    local last = samples[#samples]
    square(g, last[1], last[2], 4, "S")
    return g
  end
  local function shadeArm(who, g)
    local sleeve = WHO_SLEEVE[who][1]
    shadeLayer(g, "S", "t", "s")
    shadeLayer(g, sleeve, LIGHT[sleeve], DARK[sleeve])
    outline(g)
    return g
  end
  -- The fold line where an arm's outline crosses a garment.
  local FOLD = { W = "v", w = "v", v = "v", m = "n", N = "n", n = "n", r = "q", p = "q", q = "q" }
  local function pasteLimb(g, layer)
    for y = 0, layer.h - 1 do
      for x = 0, layer.w - 1 do
        local c = getc(layer, x, y)
        if c ~= "." then
          local was = getc(g, x, y)
          if c ~= "o" or was == "." or was == "o" then setc(g, x, y, c)
          elseif FOLD[was] then setc(g, x, y, FOLD[was]) end
        end
      end
    end
  end
  -- Both arms from the left arm's points (mirrored); each is shaded on its own, so the light
  -- stays on the left of both.
  local function bothArms(g, who, pts)
    local base = armBase(who, pts)
    pasteLimb(g, shadeArm(who, clone(base)))
    pasteLimb(g, shadeArm(who, flipX(base)))
  end
  local function oneArm(g, who, pts)
    pasteLimb(g, shadeArm(who, armBase(who, pts)))
  end
  local function lowerPts(who, pts) -- the Teacher's shoulders sit 2 rows lower
    if who ~= "teacher" then return pts end
    local out = {}
    for i = 1, #pts, 2 do out[i] = pts[i]; out[i + 1] = pts[i + 1] + ((i == 1) and 2 or 0) end
    return out
  end
  local HANG = { 7, 23, 6, 31, 6, 37 }

  -- Front figure ------------------------------------------------------------------
  -- Standing, with the arms given by the arms function. face is a FACES name or nil.
  local function front(who, face, arms)
    local g = newGrid(FW, FH)
    local head = headLayer(who)
    if face then FACES[face](head, FACE[who].ey) end
    paste(g, legsLayer(who), 0, 0)
    paste(g, torsoLayer(who), 0, 0)
    paste(g, neckLayer(who), 0, 0)
    arms(g)
    paste(g, head, 0, 0)
    outline(g)
    return g
  end

  function M.idle(who, f)
    local g = front(who, nil, function(gr) bothArms(gr, who, lowerPts(who, HANG)) end)
    if f == 1 then g = squash(g, 30) end
    return g
  end

  -- The Teacher waves with the arm on the right of the picture; the hand swings out and in.
  function M.wave(who, f)
    return front(who, nil, function(gr)
      oneArm(gr, who, lowerPts(who, HANG))
      local hx = (f == 1) and 33 or 30
      oneArm(gr, who, { 28, 24, 33, 17, hx, 8 })
    end)
  end

  -- "We're safe!": both arms up beside the head, closed happy eyes.
  function M.happy(who, f)
    local hy = (f == 1) and 5 or 6
    return front(who, "joy", function(g) bothArms(g, who, { 7, 23, 3, 15, 2, hy }) end)
  end

  -- On the floor ----------------------------------------------------------------
  -- Sitting poses are the standing upper body moved down, over a lap drawn here.
  local SIT_OY = 17
  local function upperAt(who, face, oy)
    local g = newGrid(FW, FH)
    local head = headLayer(who)
    if face then FACES[face](head, FACE[who].ey) end
    paste(g, torsoLayer(who), 0, oy)
    paste(g, neckLayer(who), 0, oy)
    return g, head
  end

  local SIT_LAP = {
    "NNNNNNNNNN",
    "NNNNNNNNNNNNN",
    "NNNNNNNNNNNNNNN",
    "NNNNNNNNNNNNNNNN",
    "bBBNNNNNNNNNNNNNN",
    "bBBBnnnnnnnnnnnnn",
    "BBBBBnnnnnnnnnnnn",
  }
  -- Cross-legged on the floor, hands on the knees (front). The second frame breathes.
  function M.sit(who, f)
    local g, head = upperAt(who, nil, SIT_OY)
    local lap = newGrid(FW, FH)
    halfRows(lap, SIT_LAP, 54)
    shadeAll(lap, { "N", "B" })
    paste(g, lap, 0, 0)
    bothArms(g, who, { 7, SIT_OY + 23, 5, SIT_OY + 31, 5, SIT_OY + 39 })
    paste(g, head, 0, SIT_OY)
    outline(g)
    if f == 1 then g = squash(g, SIT_OY + 30) end
    return g
  end

  -- Hurt: sitting on the floor and holding her left shin with both hands, the knee up, the
  -- other leg stretched out to the right, eyes squeezed shut. No blood. The second frame winces.
  function M.hurtsit(who, f)
    local g, head = upperAt(who, "hurt", SIT_OY)
    local legs = newGrid(FW, FH)
    rect(legs, 8, 54, 20, 2, "N")                  -- hips
    rect(legs, 16, 56, 15, 4, "N")                 -- the stretched leg, along the floor
    rect(legs, 31, 54, 4, 6, "B")                  -- its shoe, toe up
    rect(legs, 5, 49, 9, 6, "N")                   -- the knee, up in front of her
    rect(legs, 6, 55, 7, 4, "N")                   -- the shin
    rect(legs, 4, 58, 9, 3, "B")                   -- the foot
    shadeAll(legs, { "N", "B" })
    for x = 6, 12 do setc(legs, x, 54, "n") end    -- the fold behind the knee
    paste(g, legs, 0, 0)
    -- both arms reach down the left side to the shin
    local up = (f == 1) and 1 or 0
    pasteLimb(g, shadeArm(who, armBase(who, { 7, SIT_OY + 23, 3, SIT_OY + 31, 8, 55 })))
    pasteLimb(g, shadeArm(who, armBase(who, { 12, SIT_OY + 27, 11, 52, 9, 56 + up })))
    paste(g, head, 0, SIT_OY + up)
    outline(g)
    -- a drop of sweat at the left of her head
    local DROP = fromRows({ "..o..", ".oWo.", "oWwvo", "owvvo", ".ooo." })
    paste(g, DROP, 1, SIT_OY + 6 + up)
    return g
  end

  -- Side view (facing right) ---------------------------------------------------------
  local SIDE_HEAD = {
    "....jjjjjjjj",
    "...jjjHHHHHHHH",
    "..jjHHHHHHHHHHHH",
    "..jHHHHHHHHHHHHHH",
    "..HHHHHHHHHHHHHHH",
    "..HHHHHHHHHHHHHHH",
    "..HHHHHHHHHssssss",
    "..HHHHHHHHSSSSSSS",
    "..HHHHHHHHSSSHHHS",
    "..HHHHHHHHSSSSkeS",
    "..HHHHHHHHSSSSeeS",
    "..HHHHHHHHSSSSSSSS",
    "..HHHHHHHHSSSSSSSs",
    "..HHHHHHHSSSSSSqq",
    "..HHHHHHhSSSSSSSS",
    "...HHHHHHSSSSSSS",
    "....hhhhhSSSSSS",
  }
  local MOM_BACK = { -- long hair down the back, over the shoulder blade
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHH",
    ".......HHHHHHh",
    "........HHHHhh",
    ".........HHhh",
    "..........hh",
  }
  local HEAD_X = 8
  local function sideHead(who)
    local g = newGrid(FW, FH)
    local rows = {}
    local shift = (who == "teacher") and 2 or 0
    for i = 1, shift do rows[i] = "" end
    for _, r in ipairs(SIDE_HEAD) do rows[#rows + 1] = r end
    paste(g, fromRows(rows), HEAD_X, 1)
    if who == "mom" then
      setc(g, HEAD_X + 12, 1 + 12, "r")
      paste(g, fromRows(MOM_BACK), HEAD_X - 1, 17)
    elseif who == "teacher" then
      -- a bun at the back of her head, and glasses over the eye
      local bun = newGrid(FW, FH)
      ellipse(bun, 4, 4, 4, 3, "H")
      shadeAll(bun, { "H" })
      setc(bun, 3, 2, "j"); setc(bun, 4, 2, "j"); setc(bun, 5, 3, "j")
      paste(g, bun, HEAD_X - 3, 0)
      local gy = 1 + 10 + shift - 1
      for x = 13, 16 do setc(g, HEAD_X + x, gy, "v"); setc(g, HEAD_X + x, gy + 3, "v") end
      for y = gy + 1, gy + 2 do setc(g, HEAD_X + 13, y, "v"); setc(g, HEAD_X + 16, y, "v") end
      for x = 9, 12 do setc(g, HEAD_X + x, gy + 1, "v") end
    end
    shadeAll(g, { "H", "S" })
    return g
  end

  -- Torso and hips (side), drawn so the legs' top is at y = 44.
  local function sideBody(who)
    local g = newGrid(FW, FH)
    local top = TOP[who]
    local function span(y, xl, xr, c) for x = xl, xr do setc(g, x, y, c) end end
    if who == "teacher" then
      span(top, 13, 21, "N"); span(top + 1, 12, 22, "N")
      for y = top + 2, 38 do span(y, 11, 22, "N") end
      span(39, 11, 22, "n")
      shadeAll(g, { "N" })
      for y = top + 1, 38 do setc(g, 21, y, "w"); setc(g, 22, y, "W") end -- the blouse at the front
      for y = top + 3, 38 do setc(g, 21, y, "w") end
      for y = top + 1, top + 3 do for x = 20, 22 do setc(g, x, y, "p") end end -- the scarf
      setc(g, 21, top + 4, "q"); setc(g, 22, top + 4, "q")
      -- the skirt
      for y = 40, 50 do
        local xl = y <= 42 and 11 or y <= 46 and 10 or 9
        local xr = y <= 42 and 22 or y <= 46 and 23 or 24
        span(y, xl, xr, "N")
      end
      shadeLayer(g, "N", "m", "n")
      span(50, 9, 24, "n")
    else
      local c = (who == "dad") and "w" or "p"
      local hemY = (who == "dad") and 37 or 36
      span(top, 13, 21, c); span(top + 1, 12, 22, c)
      for y = top + 2, hemY do span(y, 11, 22, c) end
      shadeAll(g, { c })
      if who == "dad" then
        span(38, 11, 22, "x")
        for y = 22, 30 do setc(g, 21, y, "p"); setc(g, 22, y, "q") end -- the tie
      else
        span(36, 11, 22, "q")
        for y = 21, 22 do setc(g, 22, y, "S") end -- the neckline
      end
      for y = hemY + 1 + ((who == "dad") and 1 or 0), 43 do span(y, 11, 22, "N") end
      shadeLayer(g, "N", "m", "n")
    end
    return g
  end
  local function sideNeck(who)
    local g = newGrid(FW, FH)
    local top = TOP[who]
    for y = top - 2, top - 1 do for x = 15, 20 do setc(g, x, y, "S") end end
    shadeAll(g, { "S" })
    return g
  end

  -- One leg: xs holds the left x of each row from y0 down; the shoe follows the last row.
  local function legSpan(who, far, y0, xs)
    local g = newGrid(FW, FH)
    local w = 6
    local skirt = (who == "teacher")
    local base = skirt and "S" or "N"
    for i, xl in ipairs(xs) do
      for x = xl, xl + w - 1 do setc(g, x, y0 + i - 1, base) end
    end
    local yEnd = y0 + #xs - 1
    if skirt then
      for y = yEnd - 1, yEnd do for x = 0, FW - 1 do if getc(g, x, y) == "S" then setc(g, x, y, "w") end end end
    else
      for x = 0, FW - 1 do if getc(g, x, yEnd) == "N" then setc(g, x, yEnd, "n") end end
    end
    rect(g, xs[#xs] - 1, yEnd + 1, w + 3, 3, "B")
    if skirt then
      if far then
        for y = 0, FH - 1 do for x = 0, FW - 1 do
          local c = getc(g, x, y)
          if c == "S" then setc(g, x, y, "s") elseif c == "w" then setc(g, x, y, "v") end
        end end
      else
        shadeLayer(g, "S", "t", "s"); shadeLayer(g, "w", "W", "v")
      end
    elseif far then
      for y = 0, FH - 1 do for x = 0, FW - 1 do if getc(g, x, y) == "N" then setc(g, x, y, "n") end end end
      shadeLayer(g, "n", "N", nil)
    else
      shadeLayer(g, "N", "m", "n")
    end
    shadeLayer(g, "B", "b", nil)
    outline(g)
    return g
  end
  local function legXs(n, x0, slope)
    local xs = {}
    for i = 0, n - 1 do xs[#xs + 1] = x0 + math.floor(i * slope + (slope >= 0 and 0.5 or -0.5)) end
    return xs
  end

  -- Walk cycle, 4 frames: contact (near leg ahead), passing, contact (near leg behind),
  -- passing. Contact frames sit 1 pixel lower; the near arm swings against the near leg.
  local ARM_PTS = {
    hang = { 16, 23, 16, 30, 16, 37 },
    fwd = { 16, 23, 20, 30, 24, 35 },
    back = { 16, 23, 12, 30, 9, 35 },
  }
  local WALK = {
    { down = 1, arm = "back", far = { 45, legXs(13, 13, -0.55) }, near = { 45, legXs(13, 13, 0.6) } },
    { down = 0, arm = "hang", far = { 44, legXs(10, 13, -0.4) }, near = { 44, legXs(14, 13, 0) } },
    { down = 1, arm = "fwd", far = { 45, legXs(13, 13, 0.6) }, near = { 45, legXs(13, 13, -0.55) } },
    { down = 0, arm = "hang", far = { 44, legXs(14, 13, 0) }, near = { 44, legXs(10, 13, 0.4) } },
  }
  local function sideFigure(who, spec)
    local g = newGrid(FW, FH)
    paste(g, legSpan(who, true, spec.far[1], spec.far[2]), 0, 0)
    paste(g, legSpan(who, false, spec.near[1], spec.near[2]), 0, 0)
    local body = newGrid(FW, FH)
    paste(body, sideBody(who), 0, 0)
    paste(body, sideNeck(who), 0, 0)
    paste(body, sideHead(who), 0, 0)
    paste(g, body, 0, spec.down)
    local pts = {}
    for i, v in ipairs(ARM_PTS[spec.arm]) do pts[i] = v end
    for i = 2, #pts, 2 do pts[i] = pts[i] + spec.down end
    if who == "teacher" then pts[2] = pts[2] + 2 end
    pasteLimb(g, shadeArm(who, armBase(who, pts)))
    outline(g)
    return g
  end
  function M.walk(who, f)
    return sideFigure(who, WALK[f + 1])
  end

  -- Dad kneels (side, facing right) beside Mom: one knee on the floor, the other leg bent
  -- forward, leaning over with a hand reaching down to her. The second frame pats her shoulder.
  function M.kneel(who, f)
    local g = newGrid(FW, FH)
    local oy = 11
    local legs = newGrid(FW, FH)
    rect(legs, 3, 58, 12, 3, "n")                  -- the kneeling shin, along the floor
    rect(legs, 1, 56, 5, 5, "B")                   -- its shoe
    rect(legs, 12, 50, 7, 11, "n")                 -- the kneeling thigh, down to the floor
    shadeLayer(legs, "n", "N", nil)
    shadeLayer(legs, "B", "b", nil)
    local fwd = newGrid(FW, FH)
    rect(fwd, 13, 51, 14, 5, "N")                  -- the forward thigh
    rect(fwd, 23, 51, 5, 9, "N")                   -- the shin, down to the foot
    rect(fwd, 22, 58, 11, 3, "B")                  -- the foot flat on the floor
    shadeLayer(fwd, "N", "m", "n")
    shadeLayer(fwd, "B", "b", nil)
    outline(legs); outline(fwd)
    paste(g, legs, 0, 0)
    paste(g, fwd, 0, 0)
    -- the body leans forward: each row shifts right as it goes up
    local body = sideBody(who)
    local lean = newGrid(FW, FH)
    for y = 0, FH - 1 do
      local shift = math.max(0, math.floor((38 - y) * 0.2 + 0.5))
      for x = 0, FW - 1 do
        local c = getc(body, x, y)
        if c ~= "." then setc(lean, x + shift, y, c) end
      end
    end
    local torso = newGrid(FW, FH)
    paste(torso, lean, 0, oy)
    paste(torso, sideNeck(who), 3, oy)
    paste(torso, sideHead(who), 4, oy + 1)
    paste(g, torso, 0, 0)
    local down = (f == 1) and 1 or 0
    pasteLimb(g, shadeArm(who, armBase(who, { 21, oy + 24, 27, oy + 30, 30, oy + 37 + down })))
    outline(g)
    return g
  end

  -- All frames ---------------------------------------------------------------------
  M.CHARACTERS = {
    teacher = { "idle", "wave", "walk" },
    mom = { "idle", "happy", "walk", "sit", "hurtsit" },
    dad = { "walk", "kneel" },
  }
  M.POSES = {
    { name = "idle", frames = 2, ms = 500 },
    { name = "wave", frames = 2, ms = 260 },
    { name = "walk", frames = 4, ms = 140 },
    { name = "happy", frames = 2, ms = 260 },
    { name = "sit", frames = 2, ms = 500 },
    { name = "hurtsit", frames = 2, ms = 450 },
    { name = "kneel", frames = 2, ms = 400 },
  }
  function M.frames(who, pose)
    local out = {}
    for _, p in ipairs(M.POSES) do
      if p.name == pose then
        for f = 0, p.frames - 1 do out[#out + 1] = toImage(M[pose](who, f)) end
      end
    end
    return out
  end

  return M
end
