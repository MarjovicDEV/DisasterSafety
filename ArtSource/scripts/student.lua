-- The v2 Student (boy and girl), 32x50 frames, in the style the user approved in
-- StyleSample/student2.py (docs/adr/0001-v2-art-and-no-bantay.md):
--   * one dark outline (o) around the outside only,
--   * three shades per colour with light from the left; the outer right edge of each base
--     colour falls into shadow,
--   * the 20-colour palette below and nothing else,
--   * front views built from mirrored 16-pixel half maps, so they are perfectly symmetric.
-- The raised arms of surprised and happy are limbs (a chain of small squares, shaded
-- light-left / shadow-right and outlined); walk, duck and wait are hand-placed maps.
-- Every frame is bottom aligned: the lowest row (y = 49) is the outline under the shoes.

return function(L)
  local pc = app.pixelColor
  local M = {}

  local PAL = {
    o = { 40, 28, 44 },
    j = { 92, 70, 98 }, H = { 52, 38, 60 }, h = { 30, 22, 40 },        -- hair light/base/shadow
    t = { 246, 192, 150 }, S = { 228, 156, 116 }, s = { 186, 112, 86 }, -- skin
    W = { 250, 252, 255 }, w = { 220, 228, 240 }, v = { 160, 176, 206 }, -- white top
    m = { 72, 98, 160 }, N = { 42, 58, 114 }, n = { 26, 36, 76 },       -- navy
    b = { 80, 70, 90 }, B = { 46, 38, 54 },                             -- shoes
    x = { 66, 44, 40 },                                                 -- belt
    e = { 30, 22, 38 }, k = { 255, 255, 255 }, q = { 128, 40, 52 }, r = { 240, 130, 126 },
    p = { 214, 64, 64 },
  }
  local KEY = {}
  for ch, c in pairs(PAL) do KEY[ch] = pc.rgba(c[1], c[2], c[3], 255) end
  M.PAL, M.KEY = PAL, KEY
  local SHADOW = { H = "h", w = "v", W = "w", N = "n", S = "s" }

  local FW, FH = 32, 50

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
  local function fromRows(rows, name)
    local g = newGrid(#rows[1], #rows)
    for y, r in ipairs(rows) do
      assert(#r == g.w, (name or "map") .. " row " .. (y - 1) .. " is " .. #r .. " wide: " .. r)
      for x = 1, #r do g[y][x] = r:sub(x, x) end
    end
    return g
  end
  local function clone(g)
    local c = newGrid(g.w, g.h)
    for y = 1, g.h do for x = 1, g.w do c[y][x] = g[y][x] end end
    return c
  end
  local function cat(...)
    local out = {}
    for _, t in ipairs({ ... }) do for _, r in ipairs(t) do out[#out + 1] = r end end
    return out
  end
  local function mirror(half, name)
    local out = {}
    for i, r in ipairs(half) do
      assert(#r == 16, (name or "half") .. " row " .. (i - 1) .. " is " .. #r .. " wide: " .. r)
      out[i] = r .. r:reverse()
    end
    return out
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
  -- Set a pixel and its mirror twin.
  local function sym(g, x, y, c)
    setc(g, x, y, c)
    setc(g, g.w - 1 - x, y, c)
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
  -- Grow to FH rows by adding empty rows on top (the girl is 47 rows tall).
  local function padTop(g)
    local c = newGrid(g.w, FH)
    paste(c, g, 0, FH - g.h)
    return c
  end
  -- The sample's light rule: from column x0 on, the outer right edge of a base colour
  -- (next to the outline or the outside) takes the shadow tone.
  local function shade(g, x0)
    local src = clone(g)
    for y = 0, g.h - 1 do
      for x = x0 or 0, g.w - 2 do
        local c = getc(src, x, y)
        local right = getc(src, x + 1, y)
        if SHADOW[c] and (right == "." or right == "o") then setc(g, x, y, SHADOW[c]) end
      end
    end
  end

  -- Limbs ------------------------------------------------------------------
  -- A limb is a chain of size x size squares along the points {x0, y0, x1, y1, ...}.
  local function square(g, cx, cy, size, c)
    local x0 = math.floor(cx + 0.5) - size // 2
    local y0 = math.floor(cy + 0.5) - size // 2
    for y = y0, y0 + size - 1 do for x = x0, x0 + size - 1 do setc(g, x, y, c) end end
  end
  local function chain(g, pts, size, c)
    for i = 1, #pts - 2, 2 do
      local ax, ay, bx, by = pts[i], pts[i + 1], pts[i + 2], pts[i + 3]
      local n = math.max(math.abs(bx - ax), math.abs(by - ay), 1)
      for s = 0, n do square(g, ax + (bx - ax) * s / n, ay + (by - ay) * s / n, size, c) end
    end
  end
  local function rect(g, x, y, w, h, c)
    for j = y, y + h - 1 do for i = x, x + w - 1 do setc(g, i, j, c) end end
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

  -- An arm: sleeve from the shoulder, bare forearm and a hand at the end.
  -- pts run shoulder -> elbow -> hand; sleeveTo is how far along the first segment (0-1)
  -- the sleeve reaches.
  local function arm(pts, opts)
    opts = opts or {}
    local g = newGrid(FW, FH)
    chain(g, pts, opts.width or 3, "S")
    local n = #pts
    if opts.hand then
      opts.hand(g, pts[n - 1], pts[n])
    else
      square(g, pts[n - 1], pts[n], 4, "S")
    end
    local sx = pts[1] + (pts[3] - pts[1]) * (opts.sleeveTo or 0.5)
    local sy = pts[2] + (pts[4] - pts[2]) * (opts.sleeveTo or 0.5)
    chain(g, { pts[1], pts[2], sx, sy }, 4, "w")
    shadeLayer(g, "S", "t", "s")
    shadeLayer(g, "w", "W", "v")
    outline(g)
    return g
  end

  -- Front view maps (half maps, mirrored) ----------------------------------
  local BOY_HEAD = {
    "...........ooooo",
    ".........oojjjjj",
    "........ojjjHHHH",
    ".......ojHHHHHHH",
    ".......oHHHHHHHH",
    ".......oHHHHHHHH",
    ".......oHHHHHHHH",
    ".......oHHHhHHHh",
    ".......oHsssHsss",
    ".......oHSSSSSSS",
    ".......oHSSkeSSS",
    ".......oSSSeeSSS",
    ".......oSSSeeSSS",
    ".......oSSrrSSSS",
    ".......oSSSSSqSS",
    "........oSSSSSqq",
    "........osSSSSSS",
    ".........ossSSSS",
    "..........ooosss",
  }
  local BOY_TORSO = {
    "......ooooooWWss",
    ".....oWWWWWWWWvs",
    "....oWWWvwwwwwwv",
    "....oWWWvwwwwwwv",
    "....owWWvwwwwwwk",
    "....owwwvwwwwwwv",
    "....ovvvvwwwwwwv",
    "....otSSowwwwwwv",
    "....otSSowwwwwwk",
    "....oSSSowwwwwwv",
    "....oSSSowwwwwwv",
    "....oSSsowwwwwwv",
    "....oSSsowwwwwwk",
    "...otSSSovvvvvvv",
    "...oSSSSoxxxxxxx",
    "...osSSsoNNNNNNN",
    "....ooo.oNNNNNNN",
  }
  -- The same torso with the arms taken off, for poses whose arms are limbs.
  local BOY_TORSO_BARE = {
    "......ooooooWWss",
    ".....oWWWWWWWWvs",
    "....oWWWvwwwwwwv",
    "....oWWWvwwwwwwv",
    "........owwwwwwk",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwk",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwk",
    "........ovvvvvvv",
    "........oxxxxxxx",
    "........oNNNNNNN",
    "........oNNNNNNN",
  }
  local BOY_LEGS = {
    "........omNNNNNN",
    "........omNNNNNN",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........omNNNNno",
    "........onnnnnno",
    ".......obBBBBBo.",
    "......obBBBBBBo.",
    "......ooooooooo.",
  }

  local GIRL_HEAD = {
    "...........ooooo",
    ".........oojjjjj",
    "........ojjjHHHH",
    ".......ojHHHHHHH",
    "......ojHHHHHHHH",
    "......oHHHHHHHHH",
    ".....oHHHHHHHHHH",
    ".....oHHHHHHHHHH",
    ".....oHhHsssssss",
    ".....oHhHSSSSSSS",
    ".....oHhHSSkeSSS",
    ".....oHhHSSeeSSS",
    ".....oHhHSSeeSSS",
    ".....oHhHSrrSSSS",
    ".....oHhHSSSSqSS",
    ".....oHhHSSSSSqq",
    ".....oHhHsSSSSSS",
    ".....oHhHHssSSSS",
    ".....oHhHHooosss",
  }
  local GIRL_TORSO = {
    ".....oHhHHooWWss",
    ".....oHhHHWWWWvs",
    "....oWHhHHwwwwwv",
    "....oWWWoowwwwwv",
    "....owWWvwwwwwwk",
    "....owwwvwwwwwwv",
    "....ovvvvwwwwwwv",
    "....otSSowwwwwwv",
    "....otSSowwwwwwk",
    "....oSSSowwwwwwv",
    "....oSSSowwwwwwv",
    "....oSSsowwwwwwv",
    "....oSSsowwwwwwk",
    "...otSSSovvvvvvv",
    "...oSSSSoNNNNNNN",
    "...osSSsoNNNNNNN",
    "....oooomNNNNNNN",
  }
  local GIRL_TORSO_BARE = {
    ".....oHhHHooWWss",
    ".....oHhHHWWWWvs",
    "....oWHhHHwwwwwv",
    "....oWWWoowwwwwv",
    "........owwwwwwk",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwk",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwv",
    "........owwwwwwk",
    "........ovvvvvvv",
    "........oNNNNNNN",
    "........oNNNNNNN",
    ".......omNNNNNNN",
  }
  local GIRL_LEGS = {
    ".......omNNNnNNN",
    "......omNNNnNNNN",
    "......onnnnnnnnn",
    "......oooooooooo",
    ".........otSSso.",
    ".........otSSso.",
    ".........oWWWvo.",
    ".........oWWWvo.",
    ".......obBBBBBo.",
    "......obBBBBBBo.",
    "......ooooooooo.",
  }

  local FRONT = {
    boy = { head = BOY_HEAD, torso = BOY_TORSO, bare = BOY_TORSO_BARE, legs = BOY_LEGS, waist = 29 },
    girl = { head = GIRL_HEAD, torso = GIRL_TORSO, bare = GIRL_TORSO_BARE, legs = GIRL_LEGS, waist = 29 },
  }

  -- Faces: set on the left half and mirrored. Every face first clears the sample eyes and mouth.
  local function clearFace(g)
    for _, p in ipairs({ { 11, 10 }, { 12, 10 }, { 11, 11 }, { 12, 11 }, { 11, 12 }, { 12, 12 }, { 13, 14 }, { 14, 15 }, { 15, 15 } }) do
      sym(g, p[1], p[2], "S")
    end
  end
  local FACES = {
    smile = function(g) end,
    wide = function(g) -- surprised: tall eyes, round mouth
      clearFace(g)
      for y = 9, 12 do sym(g, 11, y, "e"); sym(g, 12, y, "e") end
      sym(g, 11, 9, "k"); sym(g, 11, 10, "k")
      sym(g, 15, 14, "q"); sym(g, 15, 15, "q")
    end,
    joy = function(g) -- happy: closed smiling eyes, open smile
      clearFace(g)
      sym(g, 10, 11, "e"); sym(g, 11, 10, "e"); sym(g, 12, 11, "e")
      for x = 13, 15 do sym(g, x, 14, "q") end
      sym(g, 14, 15, "q"); sym(g, 15, 15, "p")
    end,
  }

  local CHEST = { { 19, 22 }, { 20, 22 }, { 19, 23 }, { 20, 23 } }
  local function chest(g)
    for _, p in ipairs(CHEST) do setc(g, p[1], p[2], "p") end
  end
  -- The girl's small red clip, on her left side above the fringe (3 reds: light, base, dark).
  local function clip(g, x, y)
    setc(g, x, y, "r"); setc(g, x + 1, y, "p")
    setc(g, x, y + 1, "p"); setc(g, x + 1, y + 1, "q")
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

  -- Front poses ----------------------------------------------------------------
  -- Each kid keeps the sample's height (boy 50, girl 47); frames are bottom aligned.
  local function front(kind, bare, face)
    local F = FRONT[kind]
    local g = fromRows(mirror(cat(F.head, bare and F.bare or F.torso, F.legs), kind))
    FACES[face or "smile"](g)
    if kind == "girl" then clip(g, 21, 6) end
    return g
  end
  local function finish(g)
    chest(g)
    shade(g, 16)
    return g
  end
  -- For the girl, the hair that falls in front of her shoulders goes back on top of the arms.
  local function hairOnTop(g, kind)
    if kind ~= "girl" then return end
    local src = fromRows(mirror(cat(GIRL_HEAD, GIRL_TORSO)))
    for y = 8, 22 do
      for x = 0, FW - 1 do
        if x <= 9 or x >= 22 then
          local c = getc(src, x, y)
          if c == "H" or c == "h" then setc(g, x, y, c) end
        end
      end
    end
  end
  local function heightOf(kind) return kind == "boy" and 50 or 47 end
  -- Limb points are given in the 50-row frame; the girl's grid is 47 rows, so shift up 3.
  local function fit(layer, kind)
    if kind == "boy" then return layer end
    local g = newGrid(FW, 47)
    paste(g, layer, 0, -3)
    return g
  end

  function M.idle(kind, f)
    local g = finish(front(kind, false))
    if f == 1 then g = squash(g, FRONT[kind].waist) end
    return padTop(g)
  end

  function M.portrait(kind) return M.idle(kind, 0) end

  -- "Whoa!": arms flung out and down, hands open, eyes wide.
  function M.surprised(kind, f)
    local g = front(kind, true, "wide")
    local hy = (f == 1) and 30 or 31
    local la = fit(arm({ 6, 22, 3, 27, 1, hy }, { sleeveTo = 0.7 }), kind)
    paste(g, la, 0, 0); paste(g, flipX(la), 0, 0)
    hairOnTop(g, kind)
    return padTop(finish(g))
  end

  -- "I'm safe!": both arms up, hands high beside the head, closed happy eyes.
  function M.happy(kind, f)
    local g = front(kind, true, "joy")
    local hy = (f == 1) and 5 or 6
    local la = fit(arm({ 6, 22, 3, 15, 2, hy }, { sleeveTo = 0.5 }), kind)
    paste(g, la, 0, 0); paste(g, flipX(la), 0, 0)
    hairOnTop(g, kind)
    return padTop(finish(g))
  end

  -- "Hmm, wait...": the thinker pose. The fist rests under the chin, its elbow sits on the
  -- other arm, which crosses the waist. Eyes glance up, a small "hmm" mouth, and a "?" beside
  -- the head that bobs between the 2 frames. Hand-placed rows: the left half from row 16,
  -- the right half from row 23.
  local WAIT_LEFT = {
    "........oooooSSS",
    ".......otttttoSS",
    ".......otSSSSoss",
    "......ootSSSsoWs",
    ".....oWWotSSoWvs",
    "....oWWWotSSowwv",
    "....oWWWotSSowwv",
    "....owWWotSSowwk",
    "....owwwotSSowwv",
    "....ovvvotSSowwv",
    "....otSSotSSowwv",
    "....oSSSSSSsowwk",
    "...ooooooooooooo",
    "..ottttttttttttt",
    "..otSSSSSSSSSSSS",
    "..osSSssssssssss",
    "...ooooooooooooo",
  }
  local WAIT_LEFT_GIRL = {
    ".....oHhoooooSSS",
    ".....oHotttttoSS",
    ".....oHotSSSSoss",
    ".....oHotSSSsoWs",
    ".....oHootSSoWvs",
    "....oWHhotSSowwv",
  }
  local WAIT_RIGHT = {
    "kwwwwwwvWWwo....",
    "vwwwwwwvwwwo....",
    "vwwwwwwvvvvo....",
    "vwwwwwwoSSto....",
    "kwwwwwwoSSSo....",
    "ooooooooSSSo....",
    "tttttttSSSSo....",
    "SSSSSSSSSSSo....",
    "ssssssssssso....",
    "ooooooooooo.....",
  }
  local QUESTION = { ".oo.", "o..o", "..o.", ".o..", "....", ".o.." }
  local function putRows(g, rows, x0, y0)
    for i, r in ipairs(rows) do
      assert(#r == 16, "wait row " .. i .. " is " .. #r .. " wide: " .. r)
      for x = 0, 15 do setc(g, x0 + x, y0 + i - 1, r:sub(x + 1, x + 1)) end
    end
  end
  function M.wait(kind, f)
    local F = FRONT[kind]
    local g = front(kind, false)
    -- below the arms, the bare torso (no idle hands)
    local bare = fromRows(mirror(cat(F.head, F.bare, F.legs)))
    for y = 32, 35 do for x = 0, FW - 1 do setc(g, x, y, getc(bare, x, y)) end end
    putRows(g, WAIT_LEFT, 0, 16)
    if kind == "girl" then putRows(g, WAIT_LEFT_GIRL, 0, 16) end
    putRows(g, WAIT_RIGHT, 16, 23)
    clearFace(g)
    for _, x in ipairs({ 11, 19 }) do
      setc(g, x, 10, "e"); setc(g, x + 1, 10, "k"); setc(g, x, 11, "e"); setc(g, x + 1, 11, "e")
    end
    setc(g, 17, 15, "q"); setc(g, 18, 15, "q"); setc(g, 19, 14, "q")
    g = padTop(finish(g))
    paste(g, fromRows(QUESTION), 27, FH - heightOf(kind) + 2 - f)
    return g
  end

  -- Duck, Cover, and Hold: crouched low, forearms sloping up over the head in clean 2-pixel
  -- steps with the hands meeting on top, eyes shut. Hand-placed half maps (boy 25 rows, girl 26), so the
  -- figure fits under the v2 desk. On the second frame (hold) the arms and back press down
  -- 1 pixel while the knees and shoes stay.
  local DUCK_TOP = {
    "...........ooooo",
    ".........ootttto",
    ".......oottSSSSo",
    ".....oottSSSSSSs",
    "....otSSSSSssHHH",
    "...otSSSSssHHHHH",
    "...oSSSssHHHHHHH",
  }
  local BOY_DUCK = cat(DUCK_TOP, {
    "...osWWoHHHhHHHh",
    "...owwwoHsssHsss",
    "...owwwoHSSSSSSS",
    "...ovvvoSSSeeSSS",
    "..oWWWWWoSrrSSSS",
    "..owwwwwwoSSSSSq",
    "..owwwwwwwooosss",
    "..owwwwwwwwwwWWW",
    ".omNNNowwwwwwwww",
    ".omNNNNowwwwwwww",
    ".omNNNNowwwwwwww",
    ".omNNNNovvvvvvvv",
    ".omNNNNoxxxxxxxx",
    ".omNNNNoNNNNNNNN",
    ".onnnnnoNNNNNNNN",
    ".obBBBBBoooooooo",
    ".obBBBBBBo......",
    ".oooooooooo.....",
  })
  local GIRL_DUCK = cat(DUCK_TOP, {
    "...osWWoHhHHHHHH",
    "...owwwoHhHsssss",
    "...owwwoHhSSSSSS",
    "...ovvvoHhSeeSSS",
    "..oWWWWWohrrSSSS",
    "..owwwwwwohSSSSq",
    "..owwwwwwwohosss",
    "..owwwwwwwwwwWWW",
    ".omNNNowwwwwwwww",
    ".omNNNNowwwwwwww",
    ".omNNNNowwwwwwww",
    ".omNNNNovvvvvvvv",
    ".omNNNNoNNNNNNNN",
    ".omNNnNoNNNnNNNN",
    ".onnnnnonnnnnnnn",
    ".owWWWvooooooooo",
    ".obBBBBBo.......",
    ".obBBBBBBo......",
    ".oooooooooo.....",
  })
  function M.duck(kind, f)
    local g = fromRows(mirror(kind == "boy" and BOY_DUCK or GIRL_DUCK, kind .. " duck"))
    if kind == "girl" then clip(g, 20, 6) end
    -- brave, squeezed-shut eyes: > <
    sym(g, 11, 10, "S"); sym(g, 12, 10, "S")
    sym(g, 11, 9, "e"); sym(g, 12, 10, "e"); sym(g, 11, 11, "e")
    shade(g, 16)
    if f == 1 then g = squash(g, 14) end
    local top = FH - g.h -- first frame row of the figure
    g = padTop(g)
    -- effort lines over the head; they flick outward on the hold frame
    local d = f
    for _, p in ipairs({ { 9 - d, top - 4 - d }, { 10 - d, top - 3 - d }, { 13, top - 6 - d }, { 13, top - 5 - d } }) do
      sym(g, p[1], p[2], "o")
    end
    -- a small sweat drop on the left, sliding down 1 pixel on the hold frame
    local DROP = { "..o..", ".oWo.", "oWwvo", "owvvo", ".ooo." }
    paste(g, fromRows(DROP), 1, top - 3 + d)
    return g
  end

  -- Side view (facing right) ----------------------------------------------------
  -- Hand-placed maps: the head and body, three arm swings and legs drawn as one span per
  -- row that steps 1 pixel every 2 rows, so every diagonal stays clean.
  local BOY_SIDE = {
    "...........oooooo...............",
    ".........oojjjjjjoo.............",
    "........ojjjjHHHHHHHo...........",
    "........ojHHHHHHHHHHHHo.........",
    "........oHHHHHHHHHHHHHHo........",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHhHHHhHHo.......",
    "........oHHHHHHHsssssssso.......",
    "........oHHHHHtSSSSSSkeSo.......",
    "........oHHHHHSsSSSSSeeSo.......",
    "........oHHHHHSsSSSSSeeSo.......",
    "........oHHHHHHSSSSrrSSSo.......",
    "........oHHHHHHSSSSSSqqSo.......",
    "........oHHHHHsSSSSSSSSSo.......",
    "........ohhHHsSSSSSSSSSo........",
    ".........ohhhsssSSSSSo..........",
    "..........oooossssoo............",
    ".........ooWWWWsssWWWo..........",
    "........oWWWWWWWWWWWWWo.........",
    "........oWWWWWWWwwwwwwo.........",
    "........oWWwwwwwwwwppwo.........",
    "........owWwwwwwwwwppwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........ovvvvvvvvvvvvvo.........",
    "........oxxxxxxxxxxxxxo.........",
    "........omNNNNNNNNNNNNo.........",
    "........omNNNNNNNNNNNNo.........",
  }
  local GIRL_SIDE = {
    "...........oooooo...............",
    ".........oojjjjjjoo.............",
    "........ojjjjHHHHHHHo...........",
    "........ojHHHHHHHHHHHHo.........",
    "........oHHHHHHHHHHHHHHo........",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHHHHHHHHo.......",
    "........oHHHHHHHHhHHHhHHo.......",
    "........oHHHHHHHsssssssso.......",
    "........oHHHHHtSSSSSSkeSo.......",
    "........oHHHHHSsSSSSSeeSo.......",
    "........oHHHHHSsSSSSSeeSo.......",
    "........oHHHHHHSSSSrrSSSo.......",
    "........oHHHHHHSSSSSSqqSo.......",
    "........ohHHHHsSSSSSSSSSo.......",
    "........ohHHHHHsSSSSSSSo........",
    "........ohHHHHHHssSSSo..........",
    "........ohHHHHHHossoo...........",
    "........ohHHHHWWWsWWWWo.........",
    "........ohHHHHWWWWWWWWo.........",
    "........ohHHHHWWwwwwwwo.........",
    "........ohHHHHwwwwwppwo.........",
    "........ohHHHowwwwwppwo.........",
    "........ohhhhowwwwwwwwo.........",
    "........owoooowwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........owwwwwwwwwwwwwo.........",
    "........ovvvvvvvvvvvvvo.........",
    "........oNNNNNNNNNNNNNo.........",
    "........omNNNNNNNNNNNNo.........",
    ".......omNNNnNNNNnNNNNNo........",
    "......omNNNNnNNNNnNNNNNNo.......",
    "......onnnnnnnnnnnnnnnnno.......",
    "......ooooooooooooooooooo.......",
  }
  -- Near arm swings, drawn at (7, 21) on the boy's body ("." keeps what is under it).
  local ARMS = {
    hang = {
      ".....oWWWo",
      ".....oWWwo",
      ".....owwwo",
      ".....ovvvo",
      ".....otSSo",
      ".....otSSo",
      ".....oSSSo",
      ".....oSSSo",
      ".....oSSso",
      ".....oSSso",
      ".....oSSSo",
      ".....osSso",
      "......ooo.",
    },
    fwd = {
      ".....oWWWo.....",
      ".....oWWWo.....",
      "......owwwo....",
      "......ovvvo....",
      ".......otSSo...",
      ".......otSSo...",
      "........otSSo..",
      "........oSSSo..",
      ".........oSSso.",
      ".........oSSSSo",
      ".........oSSSSo",
      ".........osSSso",
      "..........oooo.",
    },
    back = {
      ".....oWWWo",
      ".....oWWWo",
      "....owwwo.",
      "....ovvvo.",
      "...otSSo..",
      "...otSSo..",
      "..otSSo...",
      "..oSSSo...",
      ".oSSso....",
      "oSSSSo....",
      "oSSSSo....",
      "osSSso....",
      ".oooo.....",
    },
  }
  -- One leg: xs holds the left x of each row from y0 down; the shoe follows the last row.
  -- tiptoe: the heel lifts one row and the toes reach down to the floor in front.
  local function legSpan(kind, far, y0, xs, tiptoe)
    local g = newGrid(FW, FH)
    local w = (kind == "boy") and 5 or 4
    local base = (kind == "boy") and "N" or "S"
    for i, xl in ipairs(xs) do
      for x = xl, xl + w - 1 do setc(g, x, y0 + i - 1, base) end
    end
    local yEnd = y0 + #xs - 1
    if kind == "girl" then
      for y = yEnd - 1, yEnd do for x = 0, FW - 1 do if getc(g, x, y) == "S" then setc(g, x, y, "w") end end end
    end
    if tiptoe then
      local toe = xs[#xs] + w - 1
      rect(g, xs[#xs] - 1, yEnd, w, 1, "B")
      rect(g, xs[#xs], yEnd + 1, w + 2, 1, "B")
      rect(g, toe, yEnd + 2, 3, 1, "B")
    else
      rect(g, xs[#xs] - 1, yEnd + 1, w + 3, 2, "B")
    end
    if kind == "boy" then
      if far then
        for y = 0, FH - 1 do for x = 0, FW - 1 do if getc(g, x, y) == "N" then setc(g, x, y, "n") end end end
        shadeLayer(g, "n", "N", nil)
      else
        shadeLayer(g, "N", "m", "n")
      end
    elseif far then
      for y = 0, FH - 1 do for x = 0, FW - 1 do
        local c = getc(g, x, y)
        if c == "S" then setc(g, x, y, "s") elseif c == "w" then setc(g, x, y, "v") end
      end end
    else
      shadeLayer(g, "S", "t", "s"); shadeLayer(g, "w", "W", "v")
    end
    shadeLayer(g, "B", "b", nil)
    outline(g)
    return g
  end
  local function rep(x, n) local t = {} for i = 1, n do t[i] = x end return t end

  -- Walk cycle, 4 frames: contact (near leg ahead), passing, contact (near leg behind),
  -- passing. Contact frames sit 1 pixel lower; the near arm swings against the near leg.
  local AHEAD_B = { 14, 15, 15, 16, 17, 17, 18, 19, 19, 20, 21 }
  local BEHIND_B = { 9, 9, 8, 7, 7, 6, 5, 5, 4, 3, 3 }
  local WALK = {
    boy = {
      { down = 1, arm = "back", far = { 36, BEHIND_B }, near = { 36, AHEAD_B } },
      { down = 0, arm = "hang", far = { 35, { 13, 13, 12, 12, 11, 11, 10, 10, 9 } }, near = { 35, rep(14, 12) } },
      { down = 1, arm = "fwd", far = { 36, AHEAD_B }, near = { 36, BEHIND_B } },
      { down = 0, arm = "hang", far = { 35, rep(14, 12) }, near = { 35, { 14, 15, 15, 16, 16, 17, 17, 17 } } },
    },
    girl = {
      { down = 1, arm = "back", far = { 40, { 13, 12, 12, 11, 11, 10, 10 } }, near = { 40, { 14, 15, 15, 16, 16, 17, 18 } } },
      { down = 0, arm = "hang", far = { 40, { 13, 13, 12, 12, 11 } }, near = { 40, rep(14, 7) } },
      { down = 1, arm = "fwd", far = { 40, { 14, 15, 15, 16, 16, 17, 18 } }, near = { 40, { 13, 12, 12, 11, 11, 10, 10 } } },
      { down = 0, arm = "hang", far = { 40, rep(14, 7) }, near = { 40, { 14, 15, 15, 16, 17 } } },
    },
  }
  function M.walk(kind, f)
    local spec = WALK[kind][f + 1]
    local g = newGrid(FW, FH)
    local oy = FH - heightOf(kind) + spec.down
    paste(g, legSpan(kind, true, spec.far[1], spec.far[2]), 0, 0)
    paste(g, legSpan(kind, false, spec.near[1], spec.near[2]), 0, 0)
    paste(g, fromRows(kind == "boy" and BOY_SIDE or GIRL_SIDE, kind .. " side"), 0, oy)
    local a = ARMS[spec.arm]
    local w = 0
    for _, r in ipairs(a) do w = math.max(w, #r) end
    local padded = {}
    for i, r in ipairs(a) do padded[i] = r .. string.rep(".", w - #r) end
    local before = clone(g)
    paste(g, fromRows(padded, "arm " .. spec.arm), 7, 21 + oy)
    for y = 0, FH - 1 do for x = 0, FW - 1 do
      local was = getc(before, x, y)
      if getc(g, x, y) == "o" and (was == "w" or was == "W" or was == "v") then setc(g, x, y, "v") end
    end end
    shade(g, 0)
    return g
  end

  -- Getting under the desk ------------------------------------------------------
  local function ellipse(g, cx, cy, rx, ry, c)
    for y = cy - ry, cy + ry do
      for x = cx - rx, cx + rx do
        local dx, dy = (x - cx) / rx, (y - cy) / ry
        if dx * dx + dy * dy <= 1.0 then setc(g, x, y, c) end
      end
    end
  end
  -- Paste a limb layer: its outline only goes on the outside or as a 'v' fold over the shirt,
  -- so the join with the body stays clean.
  local function pasteLimb(g, layer)
    for y = 0, layer.h - 1 do
      for x = 0, layer.w - 1 do
        local c = getc(layer, x, y)
        if c ~= "." then
          local was = getc(g, x, y)
          if c ~= "o" or was == "." or was == "o" then setc(g, x, y, c)
          elseif was == "w" or was == "W" or was == "v" then setc(g, x, y, "v") end
        end
      end
    end
  end
  local function sideHead(kind)
    local src = kind == "boy" and BOY_SIDE or GIRL_SIDE
    local rows = {}
    for i = 1, 19 do rows[i] = src[i] end
    return fromRows(rows, kind .. " side head")
  end

  -- Crouch-shuffle (facing right): knees bent low, back hunched, one hand on the knee, so the
  -- Student fits under the desk top while moving in. The head top is at row 18 (art y 198),
  -- under the desk top (art y 194). On the second frame the body bobs down 1 pixel and the
  -- near foot slides back 1 pixel.
  function M.crouch(kind, f)
    local girl = kind == "girl"
    local d = f
    local b = newGrid(FW, FH)
    local leg = girl and "S" or "N"
    rect(b, 3, 46, 6, 3, "B")                      -- far foot, on its toes behind
    ellipse(b, 11, 36 + d, 7, 6, "w")              -- hunched back
    rect(b, 4, 40 + d, 10, 5, "N")                 -- seat
    rect(b, 9, 41 + d, 13, 4, leg)                 -- near thigh
    rect(b, 18 - f, 44 + d, 4, 3 - d, leg)         -- near shin
    if girl then
      rect(b, 3, 39 + d, 12, 4, "N")               -- skirt over the thigh
      rect(b, 18 - f, 45, 4, 1, "w")               -- sock
      ellipse(b, 9, 33 + d, 4, 3, "H")             -- long hair down the back
    else
      rect(b, 4, 39 + d, 10, 1, "x")               -- belt
    end
    rect(b, 17 - f, 46, 9, 3, "B")                 -- near shoe
    shadeLayer(b, "w", "W", "v")
    shadeLayer(b, "N", "m", "n")
    shadeLayer(b, "S", "t", "s")
    shadeLayer(b, "H", "j", "h")
    shadeLayer(b, "B", "b", nil)
    outline(b)
    local g = newGrid(FW, FH)
    paste(g, b, 0, 0)
    pasteLimb(g, arm({ 14, 37 + d, 19, 41 + d, 23, 43 + d }, { sleeveTo = 0.55 }))
    paste(g, sideHead(kind), 4, 18 + d)
    shade(g, 0)
    return g
  end

  -- Duck, Cover, and Hold under the desk (front view, kneeling): one forearm over the head,
  -- the other arm reaching out to the left to hold the desk leg (EQ-11, and the picture in
  -- docs/design/first-playable-level.md). Built from the duck maps: the left forearm and
  -- sleeve are taken off, the head top is filled with hair, and a reaching arm is added.
  local HOLD_TOP_LEFT = {
    "................",
    "...............o",
    "............oooo",
    "..........oojjjH",
    "........oojjHHHH",
    ".......ojHHHHHHH",
    ".......oHHHHHHHH",
  }
  function M.hold(kind, f)
    local g = fromRows(mirror(kind == "boy" and BOY_DUCK or GIRL_DUCK, kind .. " hold"))
    for i, r in ipairs(HOLD_TOP_LEFT) do
      for x = 0, 15 do setc(g, x, i - 1, r:sub(x + 1, x + 1)) end
    end
    for y = 7, 10 do for x = 0, 6 do setc(g, x, y, ".") end end
    for x = 2, 8 do setc(g, x, 11, "o") end
    if kind == "girl" then clip(g, 20, 6) end
    sym(g, 11, 10, "S"); sym(g, 12, 10, "S")
    sym(g, 11, 9, "e"); sym(g, 12, 10, "e"); sym(g, 11, 11, "e")
    local oy = g.h - FH
    local reach = arm({ 6, 39 + oy, 3, 38 + oy, 1, 38 + oy }, { sleeveTo = 0.6 })
    pasteLimb(g, reach)
    shade(g, 16)
    if f == 1 then g = squash(g, 14) end
    local top = FH - g.h
    g = padTop(g)
    -- effort lines over the covered head; they flick outward on the hold frame
    for _, p in ipairs({ { 22 + f, top - 3 - f }, { 21 + f, top - 4 - f }, { 18, top - 6 - f }, { 18, top - 5 - f } }) do
      setc(g, p[1], p[2], "o")
    end
    return g
  end

  -- Rollout poses (docs/design/gameplay-redesign.md, "New poses") --------------------
  -- Side poses reuse the walk's head, body and legs with the arm drawn as a limb; front poses
  -- reuse the front maps. Limb points are given in the boy's 50-row frame (shoulder at 13, 22
  -- in side view); `lift` moves a layer down by the girl's extra top rows.
  local function lift(layer, dy)
    if dy == 0 then return layer end
    local g = newGrid(layer.w, layer.h)
    paste(g, layer, 0, dy)
    return g
  end
  -- Standing still in side view: both legs straight, the far one 2 pixels behind.
  local STAND = {
    boy = { down = 0, far = { 35, rep(12, 12) }, near = { 35, rep(14, 12) } },
    girl = { down = 0, far = { 40, rep(12, 7) }, near = { 40, rep(14, 7) } },
  }
  -- The walk's frame f (or "stand") without its arm. Returns the grid and the body's top offset.
  local function sideBase(kind, f)
    local spec = f == "stand" and STAND[kind] or WALK[kind][f + 1]
    local g = newGrid(FW, FH)
    local oy = FH - heightOf(kind) + spec.down
    paste(g, legSpan(kind, true, spec.far[1], spec.far[2]), 0, 0)
    paste(g, legSpan(kind, false, spec.near[1], spec.near[2]), 0, 0)
    paste(g, fromRows(kind == "boy" and BOY_SIDE or GIRL_SIDE, kind .. " side"), 0, oy)
    return g, oy
  end
  -- A side pose: the walk frame's legs and body plus the given arms (each a list of points).
  local function sidePose(kind, f, arms, sleeveTo)
    local g, oy = sideBase(kind, f)
    for _, pts in ipairs(arms) do
      pasteLimb(g, lift(arm(pts, { sleeveTo = sleeveTo or 0.5 }), oy))
    end
    shade(g, 0)
    return g, oy
  end

  -- Action poses (carry, reach-up, push) -------------------------------------------------
  -- Move the head and neck together, then ease the lean through the shirt to the hips.
  local function leanBody(kind, g, oy, lean)
    local src = kind == "boy" and BOY_SIDE or GIRL_SIDE
    for i, row in ipairs(src) do
      local y = i - 1
      local dx = math.floor(lean * math.min(1, math.max(0, (35 - y) / 16)) + 0.5)
      for x = 1, #row do
        local c = row:sub(x, x)
        if c ~= "." then setc(g, x - 1 + dx, y + oy, c) end
      end
    end
  end

  -- The far arm sits in shadow: one tone darker than the near arm.
  local FAR_TONE = { S = "s", t = "S", W = "w", w = "v" }
  local function farArm(layer)
    for y = 0, layer.h - 1 do
      for x = 0, layer.w - 1 do
        local c = FAR_TONE[getc(layer, x, y)]
        if c then setc(layer, x, y, c) end
      end
    end
    return layer
  end
  -- Frame-space arm points keep the two Students' grips on the same prop.
  local function actPose(kind, spec, lean, far, near, options)
    local g = newGrid(FW, FH)
    local oy = FH - heightOf(kind) + spec.down
    paste(g, legSpan(kind, true, spec.far[1], spec.far[2], options.farTiptoe), 0, 0)
    paste(g, legSpan(kind, false, spec.near[1], spec.near[2], options.nearTiptoe), 0, 0)
    pasteLimb(g, farArm(arm(far, options)))
    leanBody(kind, g, oy, lean)
    pasteLimb(g, arm(near, options))
    shade(g, 0)
    return g
  end

  local function palmUnderLoad(g, x, y)
    rect(g, x - 2, y - 1, 4, 3, "S")
    rect(g, x + 1, y - 2, 2, 2, "S")
  end

  -- The hips follow the walk; the shoulders sit behind them and the elbows stay under the load.
  -- Grip (23, 30) in the right-facing frame, one row lower on the contact frames.
  function M.carry(kind, f)
    local spec = WALK[kind][f + 1]
    local b = spec.down
    local shoulder = 22 + FH - heightOf(kind) + b
    return actPose(kind, spec, -3,
      { 15, shoulder, 15, 28 + b, 25, 28 + b },
      { 11, shoulder, 12, 32 + b, 23, 31 + b },
      { sleeveTo = 0.43, hand = palmUnderLoad })
  end

  -- Reaching forward with one arm at shoulder height (a lift button, a hand to hold).
  function M.reach(kind, f)
    return (sidePose(kind, "stand", { { 13, 22, 19, 23, 25 + f, 23 - f } }, 0.4))
  end

  local function reachingHand(g, x, y)
    rect(g, x - 1, y - 1, 3, 3, "S")
    rect(g, x, y - 3, 2, 2, "S")
    setc(g, x - 2, y - 1, "S")
  end

  -- Reach in front of the face on lifted heels. Only the forearms and hands stretch in frame 2.
  local TIPTOE = {
    boy = { down = 0, far = { 35, rep(10, 12) }, near = { 35, rep(15, 12) } },
    girl = { down = 0, far = { 40, rep(10, 7) }, near = { 40, rep(15, 7) } },
  }
  function M.reachup(kind, f)
    local shoulder = 22 + FH - heightOf(kind)
    return actPose(kind, TIPTOE[kind], 0,
      { 18, shoulder, 29, 19, 29, 5 - f },
      { 15, shoulder + 1, 25, 22, 26, 8 - f },
      { sleeveTo = 0.36, width = 2, hand = reachingHand, farTiptoe = true, nearTiptoe = true })
  end

  -- The Home card is pinned with the near hand. The far arm stays bent below the card.
  local function pinchingHand(g, x, y)
    rect(g, x - 1, y - 1, 3, 3, "S")
    if y <= 7 then
      rect(g, x, y - 2, 2, 1, "S")
      setc(g, x + 2, y, "S")
    end
  end

  function M.homeReachUpFrames(kind)
    local shoulder = 22 + FH - heightOf(kind)
    local frames = {}
    for f = 0, 1 do
      frames[#frames + 1] = toImage(actPose(kind, TIPTOE[kind], 0,
        { 18, shoulder, 22, shoulder + 5, 24, shoulder + 2 },
        { 16, shoulder, 22, 17, 28, 6 - f },
        { sleeveTo = 0.42, width = 3, hand = pinchingHand, farTiptoe = true, nearTiptoe = true }))
    end
    return frames
  end

  local function flatPalm(g, x, y)
    rect(g, x, y - 3, 2, 6, "S")
    setc(g, x - 1, y - 2, "t")
  end

  local BRACE = {
    boy = {
      down = 1, far = { 36, { 11, 10, 10, 9, 9, 8, 8, 7, 6, 6, 5 } },
      near = { 36, { 16, 17, 18, 18, 19, 19, 18, 18, 17, 17, 17 } },
    },
    girl = {
      down = 1, far = { 40, { 12, 11, 10, 9, 8, 7, 6 } },
      near = { 40, { 16, 17, 18, 19, 19, 18, 18 } },
    },
  }
  -- The back foot pushes off while the front leg takes the weight. The palms stay on the bed
  -- as the shoulders move a pixel closer to it.
  function M.push(kind, f)
    local shoulder = 23 + FH - heightOf(kind)
    return actPose(kind, BRACE[kind], 3 + f,
      { 18 + f, shoulder - 1, 25, 23, 29, 23 },
      { 15 + f, shoulder, 23, 27, 29, 25 },
      { sleeveTo = 0.40, hand = flatPalm, farTiptoe = true })
  end

  -- Calling out with a hand at the mouth; two sound lines in front of the face pulse.
  function M.call(kind, f)
    -- the elbow sits below the chin, so only the cupped hand meets the face, beside the mouth
    local g, oy = sidePose(kind, "stand", { { 13, 22, 20, 24, 25, 15 } }, 0.4)
    local lines = f == 0 and { { 28, 12 }, { 28, 17 }, { 29, 14 }, { 29, 15 } } or { { 29, 11 }, { 29, 18 }, { 30, 14 }, { 30, 15 } }
    for _, p in ipairs(lines) do setc(g, p[1], p[2] + oy, "o") end
    return g
  end

  -- Duck and cover without a desk. The Student folds the shins beneath the body,
  -- tucks the chin, and shields the head and neck with overlapping forearms.
  function M.cover(kind, f)
    local girl = kind == "girl"
    local g = newGrid(FW, FH)
    local legs = newGrid(FW, FH)
    ellipse(legs, 9, 42, 6, 4, "N")
    chain(legs, { 10, 42, 21, 45, 20, 47, 8, 47 }, 4, girl and "S" or "N")
    if girl then
      rect(legs, 6, 41, 12, 4, "N")
      rect(legs, 8, 46, 4, 3, "w")
    end
    rect(legs, 3, 46, 6, 3, "B")
    rect(legs, 10, 47, 7, 2, "B")
    shadeLayer(legs, "N", "m", "n")
    shadeLayer(legs, "S", "t", "s")
    shadeLayer(legs, "B", "b", nil)
    paste(g, legs, 0, 0)

    local upper = newGrid(FW, FH)
    ellipse(upper, 12, 37, 8, 6, "w")
    shadeLayer(upper, "w", "W", "v")
    chain(upper, { 17, 34, 26, 35, 26, 30, 22, 28 }, 3, "s")
    ellipse(upper, 22, 35, 6, 7, "H")
    if girl then ellipse(upper, 15, 37, 4, 5, "H") end
    ellipse(upper, 25, 39, 3, 3, "S")
    shadeLayer(upper, "H", "j", "h")
    pasteLimb(upper, arm({ 13, 35, 20, 39, 19, 34, 18, 30, 21, 28 }, { sleeveTo = 0.5 }))
    setc(upper, 23, 29, "t") -- far hand at the crown
    setc(upper, 24, 29, "S")
    setc(upper, 26, 39, "e") -- closed eye under the forearm
    setc(upper, 27, 40, "s")
    paste(g, upper, 0, f)
    outline(g)
    return g
  end

  -- Packing or writing at table height (front): both hands together in front of the waist,
  -- dipping 1 pixel on the second frame.
  function M.pack(kind, f)
    local g = front(kind, true)
    local la = fit(arm({ 6, 22, 5, 28, 11, 31 + f }, { sleeveTo = 0.6 }), kind)
    paste(g, la, 0, 0); paste(g, flipX(la), 0, 0)
    hairOnTop(g, kind)
    -- eyes look down at the hands
    clearFace(g)
    sym(g, 11, 11, "e"); sym(g, 12, 11, "e"); sym(g, 11, 12, "e"); sym(g, 12, 12, "e")
    sym(g, 14, 14, "q"); sym(g, 15, 14, "q")
    return padTop(finish(g))
  end

  -- Holding a phone in front of the chest with both hands, thumb tapping (front). The phone is
  -- part of the pose; its screen light moves on the second frame.
  function M.phone(kind, f)
    local g = front(kind, true)
    local la = fit(arm({ 6, 22, 4, 28, 11, 29 }, { sleeveTo = 0.6 }), kind)
    paste(g, la, 0, 0); paste(g, flipX(la), 0, 0)
    hairOnTop(g, kind)
    clearFace(g)
    sym(g, 11, 11, "e"); sym(g, 12, 11, "e"); sym(g, 11, 12, "e"); sym(g, 12, 12, "e")
    sym(g, 14, 14, "q"); sym(g, 15, 14, "q")
    g = finish(g)
    local top = kind == "boy" and 0 or -3
    rect(g, 13, 24 + top, 6, 8, "o")               -- phone
    rect(g, 14, 25 + top, 4, 5, "m")               -- screen
    setc(g, 14 + f * 2, 26 + top, "k"); setc(g, 15 + f * 2, 26 + top, "k")
    setc(g, 15, 28 + top, "w"); setc(g, 16, 28 + top, "w")
    rect(g, 12, 29 + top, 2, 3, "S"); rect(g, 18, 29 + top, 2, 3, "S")   -- hands round the phone
    return padTop(g)
  end

  -- Sitting cross-legged on the floor or a mat (front), hands on the knees. The second frame
  -- breathes 1 pixel.
  local BOY_SIT = {
    "....oooomNNNNNNN",
    "..oomNNNNNNNNNNN",
    ".omNNNNNNNNNnNNN",
    "obBBomNNNNNNNNNN",
    "obBBBonnnnnnnnnn",
    ".ooooooooooooooo",
  }
  local GIRL_SIT = {
    "....oooomNNNnNNN",
    "..oomNNNnNNNNnNN",
    ".omNNNnNNNNnNNNN",
    "obWBomNNNNNNNNNN",
    "obBBBonnnnnnnnnn",
    ".ooooooooooooooo",
  }
  function M.sit(kind, f)
    local F = FRONT[kind]
    local torso = {}
    for i = 1, #F.torso - 1 do torso[i] = F.torso[i] end
    local g = fromRows(mirror(cat(F.head, torso, kind == "boy" and BOY_SIT or GIRL_SIT), kind .. " sit"))
    if kind == "girl" then clip(g, 21, 6) end
    g = finish(g)
    if f == 1 then g = squash(g, F.waist - 2) end
    return padTop(g)
  end

  -- All frames ------------------------------------------------------------------
  M.POSES = {
    { name = "idle", frames = 2, ms = 500 },
    { name = "surprised", frames = 2, ms = 160 },
    { name = "walk", frames = 4, ms = 140 },
    { name = "duck", frames = 2, ms = 300 },
    { name = "wait", frames = 2, ms = 450 },
    { name = "happy", frames = 2, ms = 260 },
    { name = "crouch", frames = 2, ms = 180 },
    { name = "hold", frames = 2, ms = 300 },
    { name = "carry", frames = 4, ms = 140 },
    { name = "reach", frames = 2, ms = 300 },
    { name = "reachup", frames = 2, ms = 300 },
    { name = "push", frames = 2, ms = 260 },
    { name = "call", frames = 2, ms = 300 },
    { name = "cover", frames = 2, ms = 300 },
    { name = "pack", frames = 2, ms = 300 },
    { name = "phone", frames = 2, ms = 350 },
    { name = "sit", frames = 2, ms = 500 },
  }
  function M.frames(kind, pose)
    local out = {}
    for _, p in ipairs(M.POSES) do
      if p.name == pose then
        for f = 0, p.frames - 1 do out[#out + 1] = toImage(M[pose](kind, f)) end
      end
    end
    return out
  end
  function M.portraitImage(kind) return toImage(M.portrait(kind)) end

  return M
end
