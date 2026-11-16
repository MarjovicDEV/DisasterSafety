-- UI kit: 9-slice buttons and panels, icons, episode emblems, title logo,
-- particles, and the pixel font atlas.

return function(L)
  local P = L.P
  local M = {}

  -- Button colour sets: body, highlight, lip (shadow)
  M.BUTTONS = {
    green = { P.green, P.green4, P.green3 },
    blue = { P.blue, P.blue3, P.blue2 },
    orange = { P.orange, P.yellow, P.orange2 },
    red = { P.red, P.red3, P.red2 },
    teal = { P.teal, P.teal3, P.teal2 },
    purple = { P.purple, L.hex("b894e8"), P.purple2 },
    brown = { P.brown3, P.tan, P.brown2 },
    yellow = { P.yellow, P.yellow3, P.yellow2 },
    gray = { P.gray2, P.gray1, P.gray3 },
    white = { P.white, P.white, P.gray2 },
  }
  M.BUTTON_SIZE = 24
  M.BUTTON_BORDER = 9 -- 9-slice border in art pixels (all sides)

  local function rounded(img, x, y, w, h, r, c)
    for j = y, y + h - 1 do
      for i = x, x + w - 1 do
        local dx = math.max(x + r - i, i - (x + w - 1 - r), 0)
        local dy = math.max(y + r - j, j - (y + h - 1 - r), 0)
        if dx * dx + dy * dy <= r * r + 0.5 then L.px(img, i, j, c) end
      end
    end
  end

  -- pressed = true draws the face 3 px lower with a thinner lip.
  function M.button(set, pressed)
    local S = M.BUTTON_SIZE
    local body, hi, lip = set[1], set[2], set[3]
    local img = L.newImage(S, S)
    local top = pressed and 3 or 0
    local lipH = pressed and 1 or 4
    rounded(img, 1, 1 + top, S - 2, S - 2 - top, 4, P.ink)
    rounded(img, 2, 2 + top, S - 4, S - 4 - top, 3, lip)
    rounded(img, 2, 2 + top, S - 4, S - 4 - top - lipH, 3, body)
    -- highlight band
    for i = 5, S - 6 do L.px(img, i, 4 + top, hi) end
    L.px(img, 4, 5 + top, hi); L.px(img, 4, 6 + top, hi)
    return img
  end

  function M.panel()
    local S = 32
    local img = L.newImage(S, S)
    rounded(img, 0, 0, S, S, 5, P.ink)
    rounded(img, 1, 1, S - 2, S - 2, 4, P.paper3)
    rounded(img, 2, 2, S - 4, S - 5, 3, P.paper)
    -- inner stitched border
    for i = 6, S - 7, 2 do
      L.px(img, i, 5, P.paper2); L.px(img, i, S - 7, P.paper2)
      L.px(img, 5, i, P.paper2); L.px(img, S - 6, i, P.paper2)
    end
    for _, c in ipairs({ { 4, 4 }, { S - 5, 4 }, { 4, S - 6 }, { S - 5, S - 6 } }) do
      L.px(img, c[1], c[2], P.brown)
    end
    return img
  end
  M.PANEL_BORDER = 10

  function M.bubble()
    local img = L.newImage(20, 20)
    rounded(img, 0, 0, 20, 20, 5, P.ink)
    rounded(img, 1, 1, 18, 18, 4, P.white)
    L.hline(img, 4, 15, 16, P.gray1)
    L.hline(img, 5, 14, 17, P.gray1)
    return img
  end
  M.BUBBLE_BORDER = 7

  function M.bubbleTail()
    local img = L.newImage(10, 9)
    L.map(img, 0, 0, {
      "#########.",
      "#OOOOOOO#.",
      ".#OOOOO#..",
      "..#OOOO#..",
      "...#OOO#..",
      "....#OO#..",
      ".....#O#..",
      "......##..",
      "..........",
    }, { ["#"] = P.ink, ["O"] = P.white })
    return img
  end

  function M.inputBox()
    local img = L.newImage(20, 20)
    rounded(img, 0, 0, 20, 20, 3, P.ink)
    rounded(img, 1, 1, 18, 18, 2, P.white)
    L.hline(img, 2, 17, 2, P.gray1)
    L.hline(img, 2, 17, 3, P.gray1)
    return img
  end
  M.INPUT_BORDER = 6

  function M.switch(on)
    local img = L.newImage(30, 16)
    rounded(img, 0, 1, 30, 14, 6, P.ink)
    rounded(img, 1, 2, 28, 12, 5, on and P.green or P.gray2)
    L.hline(img, 5, 24, 3, on and P.green4 or P.gray1)
    local kx = on and 21 or 8
    L.circle(img, kx, 8, 7, P.ink)
    L.circle(img, kx, 8, 6, P.white)
    L.circle(img, kx, 9, 4, P.gray1)
    L.circle(img, kx, 8, 4, P.white)
    return img
  end

  function M.solid()
    local img = L.newImage(4, 4)
    L.rect(img, 0, 0, 4, 4, P.flagWhite)
    return img
  end

  -- Icons --------------------------------------------------------------
  local function finishIcon(img, shade)
    if shade ~= false then
      for y = math.floor(img.height * 0.6), img.height - 1 do
        for x = 0, img.width - 1 do
          if L.get(img, x, y) == P.white then L.px(img, x, y, P.gray1) end
        end
      end
    end
    L.outline(img, P.ink, true)
    return img
  end

  function M.icons()
    local I = {}
    local W = { ["#"] = P.white }

    local play = L.newImage(16, 16)
    L.poly(play, { 4, 2, 4, 14, 14, 8 }, P.white)
    I.play = finishIcon(play)

    local cont = L.newImage(18, 16)
    L.poly(cont, { 2, 3, 2, 13, 9, 8 }, P.white)
    L.poly(cont, { 9, 3, 9, 13, 16, 8 }, P.white)
    I.continue = finishIcon(cont)

    local gear = L.newImage(16, 16)
    for a = 0, 7 do
      local ang = a * math.pi / 4
      L.rect(gear, math.floor(7.5 + math.cos(ang) * 5.5) - 1, math.floor(7.5 + math.sin(ang) * 5.5) - 1, 3, 3, P.white)
    end
    L.circle(gear, 7.5, 7.5, 5, P.white)
    L.circle(gear, 7.5, 7.5, 2, L.CLEAR)
    for y = 6, 9 do for x = 6, 9 do
      local dx, dy = x - 7.5, y - 7.5
      if dx * dx + dy * dy <= 3.2 then gear:drawPixel(x, y, L.CLEAR) end
    end end
    I.settings = finishIcon(gear)

    local q = L.newImage(14, 18)
    L.text(q, 2, 2, "?", P.white, 2)
    I.help = finishIcon(q)

    local door = L.newImage(18, 16)
    L.rect(door, 2, 1, 8, 14, P.white)
    L.rect(door, 3, 2, 6, 12, P.brown3)
    L.px(door, 7, 8, P.white)
    L.poly(door, { 10, 5, 13, 5, 13, 3, 17, 8, 13, 13, 13, 11, 10, 11 }, P.white)
    I.exit = finishIcon(door, false)

    local back = L.newImage(16, 16)
    L.poly(back, { 1, 8, 7, 2, 7, 5, 14, 5, 14, 11, 7, 11, 7, 14 }, P.white)
    I.back = finishIcon(back)

    local music = L.newImage(16, 16)
    L.map(music, 1, 2, {
      ".....#########",
      ".....#########",
      ".....##.....##",
      ".....##.....##",
      ".....##.....##",
      ".....##.....##",
      ".....##.....##",
      ".....##.....##",
      "..####...#####",
      ".#####..######",
      ".#####..######",
      "..###....####.",
    }, W)
    I.music = finishIcon(music, false)

    local sound = L.newImage(18, 16)
    L.rect(sound, 2, 5, 4, 6, P.white)
    L.poly(sound, { 5, 5, 10, 1, 10, 15, 5, 11 }, P.white)
    L.vline(sound, 13, 5, 10, P.white)
    L.vline(sound, 16, 3, 12, P.white)
    L.px(sound, 12, 4, P.white); L.px(sound, 12, 11, P.white)
    L.px(sound, 15, 2, P.white); L.px(sound, 15, 13, P.white)
    I.sound = finishIcon(sound, false)

    local lock = L.newImage(16, 18)
    L.rect(lock, 4, 2, 2, 7, P.gray2); L.rect(lock, 10, 2, 2, 7, P.gray2); L.rect(lock, 5, 1, 6, 2, P.gray2)
    L.rect(lock, 2, 8, 12, 9, P.yellow)
    L.rect(lock, 2, 14, 12, 3, P.yellow2)
    L.hline(lock, 3, 12, 9, P.yellow3)
    L.rect(lock, 7, 11, 2, 3, P.ink)
    L.outline(lock, P.ink, true)
    I.lock = lock

    local function star(fill, shade)
      local s = L.newImage(16, 16)
      local pts = {}
      for i = 0, 9 do
        local r = (i % 2 == 0) and 7.4 or 3.2
        local a = -math.pi / 2 + i * math.pi / 5
        pts[#pts + 1] = 7.5 + math.cos(a) * r
        pts[#pts + 1] = 8.2 + math.sin(a) * r
      end
      L.poly(s, pts, fill)
      for y = 9, 15 do for x = 0, 15 do if L.get(s, x, y) == fill then L.px(s, x, y, shade) end end end
      L.outline(s, P.ink, false)
      return s
    end
    I.star = star(P.yellow, P.yellow2)
    I.starEmpty = star(P.gray1, P.gray2)

    local check = L.newImage(16, 16)
    L.line(check, 2, 8, 6, 12, P.white, 3)
    L.line(check, 6, 12, 13, 3, P.white, 3)
    I.check = finishIcon(check, false)

    local cross = L.newImage(16, 16)
    L.line(cross, 3, 3, 12, 12, P.white, 3)
    L.line(cross, 12, 3, 3, 12, P.white, 3)
    I.cross = finishIcon(cross, false)

    local hat = L.newImage(18, 12)
    L.ellipse(hat, 8.5, 8, 7, 6, P.yellow)
    L.rect(hat, 0, 8, 18, 3, P.yellow2)
    L.rect(hat, 7, 2, 3, 7, P.yellow2)
    L.px(hat, 4, 5, P.yellow3); L.px(hat, 5, 4, P.yellow3)
    for x = 0, 17 do for y = 11, 11 do hat:drawPixel(x, y, L.CLEAR) end end
    L.outline(hat, P.ink, true)
    I.hardhat = hat

    local home = L.newImage(16, 16)
    L.poly(home, { 1, 8, 8, 1, 15, 8 }, P.white)
    L.rect(home, 3, 8, 10, 6, P.white)
    L.rect(home, 7, 10, 3, 4, P.brown3)
    I.home = finishIcon(home, false)

    local pencil = L.newImage(16, 16)
    L.line(pencil, 3, 12, 11, 4, P.yellow, 3)
    L.line(pencil, 11, 4, 12, 3, P.red3, 3)
    L.px(pencil, 2, 14, P.ink2); L.px(pencil, 3, 13, P.skin1)
    L.outline(pencil, P.ink, true)
    I.pencil = pencil

    return I
  end

  -- Episode emblems 40x40 ---------------------------------------------
  function M.episodes()
    local E = {}

    -- Earthquake: shaking house over cracked ground
    local eq = L.newImage(40, 40)
    L.rect(eq, 2, 29, 36, 9, P.dirt)
    L.hline(eq, 2, 37, 29, P.grass2); L.hline(eq, 2, 37, 30, P.grass2)
    L.line(eq, 20, 30, 17, 33, P.ink, 1); L.line(eq, 17, 33, 22, 35, P.ink, 1); L.line(eq, 22, 35, 19, 38, P.ink, 1)
    L.line(eq, 9, 31, 12, 35, P.dirt2); L.line(eq, 30, 31, 27, 36, P.dirt2)
    L.poly(eq, { 9, 16, 20, 6, 31, 16 }, P.roof)
    L.line(eq, 9, 16, 20, 6, P.roof3)
    L.rect(eq, 11, 16, 19, 13, P.wall)
    L.rect(eq, 11, 16, 19, 2, P.wall2)
    L.rect(eq, 18, 20, 5, 9, P.brown)
    L.rect(eq, 13, 19, 4, 4, P.glass); L.rect(eq, 24, 19, 4, 4, P.glass)
    for _, s in ipairs({ { 4, 12 }, { 3, 18 }, { 35, 12 }, { 36, 18 } }) do
      L.vline(eq, s[1], s[2], s[2] + 3, P.ink2)
    end
    L.rect(eq, 6, 4, 2, 2, P.rock); L.rect(eq, 32, 6, 2, 2, P.rock)
    L.outline(eq, P.ink)
    E.earthquake = eq

    -- Fire: layered flame on logs
    local fi = L.newImage(40, 40)
    L.line(fi, 7, 35, 32, 30, P.brown, 4)
    L.line(fi, 8, 30, 32, 35, P.brown2, 4)
    local function flame(img, cx, base, w, h, c)
      L.ellipse(img, cx, base - w * 0.8, w, w * 0.9, c)
      L.poly(img, { cx - w, base - w * 0.8, cx + w, base - w * 0.8, cx + w * 0.2, base - h, cx - w * 0.3, base - h * 0.75 }, c)
    end
    flame(fi, 20, 33, 11, 31, P.fire)
    flame(fi, 20, 33, 8, 23, P.fire2)
    flame(fi, 20, 33, 4.5, 14, P.fire3)
    L.px(fi, 9, 9, P.fire2); L.px(fi, 31, 12, P.fire2); L.px(fi, 28, 5, P.fire3)
    L.outline(fi, P.ink)
    E.fire = fi

    -- Typhoon: swirling storm cloud with rain
    local ty = L.newImage(40, 40)
    L.circle(ty, 13, 15, 8, P.gray2)
    L.circle(ty, 23, 11, 10, P.gray2)
    L.circle(ty, 31, 16, 7, P.gray2)
    L.rect(ty, 7, 15, 30, 8, P.gray2)
    L.circle(ty, 21, 9, 6, P.gray1)
    for t = 0, 40 do
      local a = t * 0.32
      local r = 1 + t * 0.19
      L.px(ty, 22 + math.cos(a) * r, 15 + math.sin(a) * r * 0.7, P.purple2)
    end
    for _, d in ipairs({ { 9, 27 }, { 15, 30 }, { 21, 27 }, { 27, 31 }, { 33, 27 }, { 12, 34 }, { 24, 35 } }) do
      L.line(ty, d[1], d[2], d[1] - 2, d[2] + 4, P.water)
    end
    L.outline(ty, P.ink)
    E.typhoon = ty

    -- Flood: house in rising water
    local fl = L.newImage(40, 40)
    L.poly(fl, { 9, 15, 20, 5, 31, 15 }, P.roof)
    L.rect(fl, 11, 15, 19, 12, P.wall)
    L.rect(fl, 14, 18, 4, 4, P.glass); L.rect(fl, 23, 18, 4, 4, P.glass)
    L.rect(fl, 2, 24, 36, 13, P.water)
    for x = 2, 37 do
      local y = 24 + math.floor(1.5 * math.sin(x / 2.2))
      L.vline(fl, x, y, 24, P.water)
      L.px(fl, x, y, P.water3)
      L.px(fl, x, 31 + math.floor(math.sin(x / 2.2 + 2)), P.water2)
    end
    L.outline(fl, P.ink)
    E.flood = fl

    -- Landslide: slope with tumbling rocks
    local ls = L.newImage(40, 40)
    L.poly(ls, { 2, 6, 14, 6, 38, 36, 2, 36 }, P.dirt)
    L.poly(ls, { 2, 6, 14, 6, 18, 11, 2, 11 }, P.grass2)
    L.line(ls, 14, 6, 38, 36, P.dirt2)
    for x = 4, 30, 5 do L.px(ls, x, 20 + (x % 3), P.dirt2) end
    L.line(ls, 7, 6, 4, 0, P.brown2); L.circle(ls, 4, 1, 2, P.green2)
    for _, r in ipairs({ { 24, 22, 4 }, { 31, 30, 3 }, { 18, 30, 3 }, { 34, 20, 2 } }) do
      L.circle(ls, r[1], r[2], r[3], P.rock)
      L.px(ls, r[1] - 1, r[2] - 1, P.rock3)
    end
    L.outline(ls, P.ink)
    E.landslide = ls

    return E
  end

  -- Title logo -----------------------------------------------------------
  function M.title()
    local s = 3
    local l1, l2 = "DISASTER", "SAFETY"
    local w1, w2 = L.F.width(l1, s), L.F.width(l2, s)
    local W = math.max(w1, w2) + 16
    local img = L.newImage(W, 70)
    L.fancyText(img, (W - w1) // 2, 4, l1, s, P.red3, P.red, P.ink, 2)
    L.fancyText(img, (W - w2) // 2, 33, l2, s, P.yellow3, P.yellow, P.ink, 2)
    return img
  end

  function M.subtitle()
    local text = "Be Ready! Be Safe!"
    local tw = L.F.width(text, 1)
    local W = tw + 30
    local img = L.newImage(W, 17)
    -- ribbon tails
    L.poly(img, { 0, 4, 9, 4, 9, 15, 0, 15, 3, 9.5 }, P.teal2)
    L.poly(img, { W - 9, 4, W, 4, W - 3, 9.5, W, 15, W - 9, 15 }, P.teal2)
    L.rect(img, 6, 2, W - 12, 12, P.teal)
    L.hline(img, 6, W - 7, 2, P.teal3)
    L.text(img, (W - tw) // 2, 5, text, P.white, 1)
    L.outline(img, P.ink)
    return img
  end

  -- Particles ------------------------------------------------------------
  function M.sparkle()
    local img = L.newImage(9, 9)
    L.map(img, 0, 0, {
      "....#....",
      "....#....",
      "...#O#...",
      "..#OOO#..",
      "##OOOOO##",
      "..#OOO#..",
      "...#O#...",
      "....#....",
      "....#....",
    }, { ["#"] = P.yellow, ["O"] = P.yellow3 })
    return img
  end

  function M.leaf(c1, c2)
    local img = L.newImage(6, 5)
    L.map(img, 0, 0, { "..##..", ".#OO#.", "#OOOO#", ".#OO#.", "..#..." }, { ["#"] = c2, ["O"] = c1 })
    return img
  end

  -- Font atlas -------------------------------------------------------------
  -- Returns atlas image and metrics lines "codepoint x y w" (cell height 11).
  function M.fontAtlas()
    local list = L.F.all()
    local cols = 16
    local cw, ch = 7, 12
    local W = 128
    local rows = math.ceil(#list / cols)
    local H = 8
    while H < rows * ch do H = H * 2 end
    local img = L.newImage(W, H)
    local lines = {}
    for i, glyph in ipairs(list) do
      local col = (i - 1) % cols
      local row = (i - 1) // cols
      local x, y = col * cw, row * ch
      local cell, w = L.F.cell(glyph)
      for r = 1, 11 do
        for c = 1, w do
          if cell[r]:sub(c, c) == "#" then L.px(img, x + c - 1, y + r - 1, P.flagWhite) end
        end
      end
      lines[#lines + 1] = string.format("%d %d %d %d", utf8.codepoint(glyph), x, y, w)
    end
    return img, lines
  end

  return M
end
