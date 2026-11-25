-- How to Play Tutorial Page pictures. Each page is 136x76 art pixels with two frames,
-- shown at 4x (544x304 UI units). Page order must match UIStrings.TutorialPages.

return function(L, UI)
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
    return img
  end

  M.pages = {
    { name = "tutorial_topic", draw = episodes },
    { name = "tutorial_difficulty", draw = difficulty },
    { name = "tutorial_stars", draw = stars },
  }

  return M
end
