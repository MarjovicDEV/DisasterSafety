-- How to Play Tutorial Page pictures. Each page is 136x76 art pixels with two frames,
-- shown at 4x (544x304 UI units). Page order must match UIStrings.TutorialPages.

return function(L, CH, UI)
  local P = L.P
  local M = {}
  local W, H = 136, 76

  local icons = UI.icons()
  local emblems = UI.episodes()
  local sparkle = UI.sparkle()

  -- Copy src at half size (every second pixel), skipping transparent pixels.
  local function blitHalf(dst, src, x, y)
    for j = 0, src.height - 1, 2 do
      for i = 0, src.width - 1, 2 do
        local c = src:getPixel(i, j)
        if app.pixelColor.rgbaA(c) > 0 then L.px(dst, x + i // 2, y + j // 2, c) end
      end
    end
  end

  local HAND = {
    "....##.....",
    "...#OO#....",
    "...#OO#....",
    "...#OO###..",
    "...#OO#OO##",
    ".###OOOOOO#",
    "#OO#OOOOOO#",
    "#OOOOOOOOO#",
    ".#OOOOOOOO#",
    "..#OOOOOO#.",
    "...#OOOO#..",
    "...######..",
  }
  local function hand(img, x, y) L.map(img, x, y, HAND, { ["#"] = P.ink, ["O"] = P.white }) end

  local function card(img, x, y, w, h, fill, lip)
    L.rect(img, x + 1, y, w - 2, h, P.ink)
    L.rect(img, x, y + 1, w, h - 2, P.ink)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, lip)
    L.rect(img, x + 1, y + 1, w - 2, h - 4, fill)
  end

  local function tinyLock(img, x, y)
    L.rect(img, x + 1, y, 4, 1, P.gray2)
    L.vline(img, x + 1, y, y + 3, P.gray2)
    L.vline(img, x + 4, y, y + 3, P.gray2)
    L.rect(img, x, y + 3, 6, 5, P.yellow)
    L.px(img, x + 2, y + 5, P.ink); L.px(img, x + 3, y + 5, P.ink)
    L.px(img, x - 1, y + 3, P.ink); L.px(img, x + 6, y + 3, P.ink)
  end

  local function badge(img, cx, cy, good)
    L.circle(img, cx, cy, 6, P.ink)
    L.circle(img, cx, cy, 5, good and P.green or P.red)
    if good then
      L.line(img, cx - 3, cy, cx - 1, cy + 2, P.white, 2)
      L.line(img, cx - 1, cy + 2, cx + 3, cy - 3, P.white, 2)
    else
      L.line(img, cx - 3, cy - 3, cx + 2, cy + 2, P.white, 2)
      L.line(img, cx + 2, cy - 3, cx - 3, cy + 2, P.white, 2)
    end
  end

  local function backdrop()
    local img = L.newImage(W, H)
    L.vgrad(img, 0, 0, W, H, { P.sky4, P.sky5, P.white })
    return img
  end

  -- 1. Pick a topic ------------------------------------------------------
  local function episodes(f)
    local img = backdrop()
    local colors = { P.orange, P.orange2 }
    local names = { "earthquake", "fire", "typhoon", "flood", "landslide" }
    for i = 1, 5 do
      local x, y = 5 + (i - 1) * 26, 20
      if i == 1 and f == 1 then y = y + 2 end
      local c = (i == 1) and colors or { P.gray2, P.gray3 }
      card(img, x, y, 22, 30, c[1], c[2])
      blitHalf(img, emblems[names[i]], x + 1, y + 3)
      if i > 1 then tinyLock(img, x + 8, y + 18) end
    end
    if f == 1 then
      L.blit(img, sparkle, 2, 10)
      L.blit(img, sparkle, 20, 12)
      hand(img, 12, 46)
    else
      hand(img, 14, 50)
    end
    return img
  end

  -- 2. Start with Easy ----------------------------------------------------
  local function difficulty(f)
    local img = backdrop()
    local sets = { { P.green, P.green3, "Easy", 1 }, { P.orange, P.orange2, "Normal", 2 }, { P.red, P.red2, "Hard", 3 } }
    for i = 1, 3 do
      local s = sets[i]
      local x, y = 4 + (i - 1) * 44, 12
      if i == 1 and f == 1 then y = y + 2 end
      card(img, x, y, 40, 44, s[1], s[2])
      local tw = L.F.width(s[3], 1)
      L.text(img, x + (40 - tw) // 2, y + 6, s[3], P.white, 1)
      for h = 1, s[4] do
        local hx = x + 20 - s[4] * 5 + (h - 1) * 10
        L.ellipse(img, hx + 4, y + 26, 4, 3, P.ink)
        L.ellipse(img, hx + 4, y + 26, 3, 2, P.yellow)
        L.rect(img, hx, y + 27, 9, 2, P.yellow2)
      end
      if i > 1 then tinyLock(img, x + 32, y - 2) end
    end
    if f == 1 then
      L.blit(img, sparkle, 0, 4)
      L.blit(img, sparkle, 36, 6)
    end
    hand(img, 20, f == 1 and 48 or 52)
    return img
  end

  -- 3. Read the story -----------------------------------------------------
  local function story(f)
    local img = L.newImage(W, H)
    local dx = (f == 1) and 2 or 0
    L.rect(img, 0, 0, W, 56, P.wall)
    L.rect(img, 0, 52, W, 4, P.wall2)
    L.vgrad(img, 0, 56, W, 20, { P.brown3, P.brown })
    for x = 0, W, 17 do L.vline(img, x, 56, 75, P.brown2) end
    -- blackboard
    L.rect(img, 30 + dx, 8, 62, 26, P.brown2)
    L.rect(img, 32 + dx, 10, 58, 22, P.green3)
    L.text(img, 38 + dx, 13, "Be safe!", P.white, 1)
    L.hline(img, 38 + dx, 80 + dx, 25, P.green2)
    -- window
    L.rect(img, 102 + dx, 10, 24, 22, P.teal2)
    L.rect(img, 104 + dx, 12, 20, 18, P.glass2)
    L.vline(img, 114 + dx, 12, 29, P.teal2)
    -- shelf with a book that falls when shaking
    L.rect(img, 6 + dx, 22, 18, 2, P.brown)
    L.rect(img, 8 + dx, 15, 4, 7, P.red)
    if f == 0 then L.rect(img, 13, 15, 4, 7, P.blue) else L.rect(img, 15, 30, 7, 4, P.blue) end
    -- desk
    L.rect(img, 54 + dx, 46, 36, 4, P.brown)
    L.hline(img, 54 + dx, 89 + dx, 46, P.brown3)
    L.rect(img, 56 + dx, 50, 3, 14, P.brown2)
    L.rect(img, 85 + dx, 50, 3, 14, P.brown2)
    -- student
    blitHalf(img, CH.student(0, false), 36 + dx, 44)
    if f == 1 then
      for _, s in ipairs({ { 2, 40 }, { 3, 48 }, { 131, 40 }, { 130, 48 } }) do L.vline(img, s[1], s[2], s[2] + 5, P.ink2) end
      L.rect(img, 44, 34, 2, 6, P.red)
      L.rect(img, 44, 41, 2, 2, P.red)
    end
    return img
  end

  -- 4. Tap the safest choice ---------------------------------------------
  local function choice(f)
    local img = backdrop()
    -- left: duck, cover, and hold under a desk
    card(img, 6, 6, 58, 58, f == 1 and P.yellow3 or P.white, P.gray2)
    if f == 1 then
      L.rect(img, 5, 5, 60, 1, P.yellow); L.rect(img, 5, 64, 60, 1, P.yellow)
      L.rect(img, 5, 5, 1, 60, P.yellow); L.rect(img, 64, 5, 1, 60, P.yellow)
    end
    blitHalf(img, CH.student(0, false), 26, 34)
    L.rect(img, 16, 30, 40, 4, P.brown)
    L.hline(img, 16, 55, 30, P.brown3)
    L.rect(img, 18, 34, 3, 22, P.brown2)
    L.rect(img, 51, 34, 3, 22, P.brown2)
    badge(img, 55, 54, true)
    -- right: running around
    card(img, 72, 6, 58, 58, P.white, P.gray2)
    blitHalf(img, CH.student(1, true), 96, 26)
    for _, s in ipairs({ { 82, 34 }, { 80, 40 }, { 83, 46 } }) do L.hline(img, s[1], s[1] + 6, s[2], P.ink2) end
    badge(img, 121, 54, false)
    if f == 1 then
      L.blit(img, sparkle, 2, 0)
      L.blit(img, sparkle, 54, 2)
    end
    hand(img, 30, f == 1 and 56 or 60)
    return img
  end

  -- 5. Earn stars ---------------------------------------------------------
  local function stars(f)
    local img = backdrop()
    local filled = (f == 1) and 3 or 2
    local function big(cx, cy, on)
      local s = on and icons.star or icons.starEmpty
      -- drawn at 2x so the stars read from across a classroom
      for j = 0, s.height - 1 do
        for i = 0, s.width - 1 do
          local c = s:getPixel(i, j)
          if app.pixelColor.rgbaA(c) > 0 then L.rect(img, cx - 16 + i * 2, cy - 16 + j * 2, 2, 2, c) end
        end
      end
    end
    big(32, 36, filled >= 1)
    big(68, 30, filled >= 2)
    big(104, 36, filled >= 3)
    if f == 1 then
      L.blit(img, sparkle, 92, 10)
      L.blit(img, sparkle, 118, 22)
      L.blit(img, sparkle, 6, 18)
    end
    blitHalf(img, CH.bantay(f == 1 and "talk" or "wag"), 112, 52)
    return img
  end

  M.pages = {
    { name = "tutorial_topic", draw = episodes },
    { name = "tutorial_difficulty", draw = difficulty },
    { name = "tutorial_story", draw = story },
    { name = "tutorial_choice", draw = choice },
    { name = "tutorial_stars", draw = stars },
  }

  return M
end
