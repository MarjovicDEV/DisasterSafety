-- Earthquake / Normal art: four Settings (480x270, shown at 4x), their moving parts
-- (2 frames each) and one 72x52 picture per Choice (docs/design/earthquake-normal.md).
-- Gentle on purpose: parts only sway by 1 art pixel and nothing falls.

return function(L, CH, LV)
  local P = L.P
  local H = LV.helpers
  local PW, PH = H.PW, H.PH
  local M = {}

  -- Shared bits --------------------------------------------------------------
  local function woodFloor(img, top, W, Hh, seed)
    L.vgrad(img, 0, top, W, Hh - top, { P.tan, P.brown3, P.brown3, P.brown })
    for y = top + 8, Hh, 12 do L.hline(img, 0, W - 1, y, P.tan2) end
    L.seed(seed)
    for y = top, Hh, 12 do
      local off = L.randi(0, 40)
      for x = off, W, 64 do L.vline(img, x, y + 1, y + 11, P.tan2) end
    end
  end

  -- An orange go bag (backpack), 16x18, with a reflective strip.
  local function goBag(img, x, y, open)
    L.rect(img, x + 1, y + 2, 14, 16, P.orange)
    L.rect(img, x + 1, y + 2, 14, 3, P.orange2)
    L.rect(img, x + 3, y + 9, 10, 2, P.yellow3)
    L.rect(img, x + 4, y + 12, 8, 5, P.orange2)
    L.hline(img, x + 5, x + 10, y, P.orange2)
    L.px(img, x + 4, y + 1, P.orange2); L.px(img, x + 11, y + 1, P.orange2)
    if open then L.rect(img, x + 2, y + 2, 12, 2, P.brown2) end
  end

  local function bottle(img, x, y)
    L.rect(img, x, y + 2, 4, 9, P.water3); L.rect(img, x + 1, y, 2, 2, P.blue2); L.vline(img, x + 1, y + 3, y + 9, P.white)
  end
  local function can(img, x, y) L.rect(img, x, y, 5, 6, P.metal2); L.hline(img, x, x + 4, y + 2, P.red) end
  local function flashlight(img, x, y) L.rect(img, x, y, 9, 3, P.yellow2); L.rect(img, x + 8, y - 1, 3, 5, P.gray3); L.px(img, x + 10, y + 1, P.yellow3) end
  local function whistle(img, x, y) L.rect(img, x, y, 5, 3, P.red); L.rect(img, x + 4, y + 1, 2, 1, P.red2); L.px(img, x + 1, y + 1, P.red2) end

  -- Bedroom: bed on the left, window, low shelf, small picture, tall bookcase --------
  function M.bedroom()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 176, { P.sky5, P.sky4, P.sky5 })
    L.rect(img, 0, 0, W, 5, P.blue3)
    L.rect(img, 0, 172, W, 4, P.brown2)
    woodFloor(img, 176, W, Hh, 31)
    -- rug
    L.ellipse(img, 250, 206, 70, 12, P.pink)
    L.ellipse(img, 250, 206, 60, 8, P.red3)

    -- window with curtains (the bed is NOT under it)
    H.windowPane(img, 206, 36, 84, 70)
    L.rect(img, 196, 30, 104, 4, P.brown2)
    L.rect(img, 198, 34, 10, 78, P.pink); L.vline(img, 203, 34, 111, P.red3)
    L.rect(img, 288, 34, 10, 78, P.pink); L.vline(img, 293, 34, 111, P.red3)

    -- bed with headboard, pillow and a striped blanket
    L.rect(img, 22, 100, 12, 76, P.ink)
    L.rect(img, 23, 101, 10, 75, P.brown)
    L.rect(img, 34, 150, 144, 14, P.ink)
    L.rect(img, 35, 151, 142, 12, P.brown)
    L.rect(img, 34, 134, 144, 17, P.white)
    L.hline(img, 34, 177, 134, P.gray2)
    L.ellipse(img, 52, 132, 14, 6, P.white); L.ellipse(img, 52, 133, 13, 4, P.gray1)
    L.rect(img, 76, 128, 102, 23, P.blue)
    for x = 84, 172, 12 do L.rect(img, x, 128, 4, 23, P.blue3) end
    L.hline(img, 76, 177, 128, P.blue2)
    L.rect(img, 36, 164, 5, 12, P.brown2); L.rect(img, 170, 164, 5, 12, P.brown2)
    -- teddy on the bed
    L.circle(img, 120, 122, 6, P.tan); L.circle(img, 115, 116, 2, P.tan); L.circle(img, 125, 116, 2, P.tan)
    L.px(img, 118, 121, P.ink); L.px(img, 122, 121, P.ink); L.px(img, 120, 124, P.brown2)

    -- small, light picture on the wall above the low shelf
    H.framed(img, 318, 82, 34, 26, P.brown2, P.yellow3)
    L.poly(img, { 326, 100, 335, 90, 344, 100 }, P.roof)
    L.rect(img, 329, 100, 12, 4, P.wall)

    -- low shelf with a toy box and books lying down
    L.rect(img, 304, 138, 64, 38, P.ink)
    L.rect(img, 305, 139, 62, 36, P.brown)
    L.rect(img, 305, 155, 62, 3, P.brown3)
    L.rect(img, 310, 142, 20, 12, P.paper3); L.rect(img, 310, 142, 20, 3, P.paper2)
    L.rect(img, 336, 148, 26, 3, P.red); L.rect(img, 338, 151, 22, 3, P.green); L.rect(img, 335, 145, 24, 3, P.yellow2)
    L.rect(img, 310, 160, 16, 12, P.blue); L.circle(img, 348, 166, 5, P.orange)

    -- tall bookcase on the right
    L.rect(img, 390, 58, 46, 118, P.ink)
    L.rect(img, 391, 59, 44, 116, P.brown2)
    L.rect(img, 394, 62, 38, 110, P.brown)
    for i, y in ipairs({ 86, 112, 138, 164 }) do
      L.rect(img, 394, y, 38, 3, P.brown3)
      H.books(img, 396, y, 6, 120 + i)
    end
    return img
  end

  -- Living room ("home"): window, sofa, strong table, family photo, door, go bag ------
  -- The hanging lamp (x 232, y 0) and the glass cabinet (x 330, y 64) are separate sprites.
  function M.home()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 176, { P.wall2, P.wall, P.cream })
    L.rect(img, 0, 0, W, 6, P.wall3)
    L.rect(img, 0, 172, W, 4, P.brown2)
    H.tileFloor(img, 176, W, Hh, 16)
    L.rect(img, 230, 6, 30, 3, P.wall3) -- ceiling plate for the lamp

    H.windowPane(img, 30, 40, 92, 66)

    -- family photo
    H.framed(img, 156, 54, 38, 30, P.brown2, P.sky4)
    L.circle(img, 168, 70, 4, P.skin1); L.circle(img, 182, 70, 4, P.skin2)
    L.rect(img, 164, 74, 9, 6, P.red); L.rect(img, 178, 74, 9, 6, P.blue)

    -- sofa
    L.rect(img, 18, 118, 140, 58, P.ink)
    L.rect(img, 19, 119, 138, 30, P.teal)
    L.rect(img, 19, 147, 138, 16, P.teal2)
    L.rect(img, 19, 126, 12, 38, P.teal2); L.rect(img, 145, 126, 12, 38, P.teal2)
    L.vline(img, 88, 120, 146, P.teal2)
    L.hline(img, 32, 144, 147, P.teal3)
    L.rect(img, 24, 163, 5, 13, P.brown2); L.rect(img, 147, 163, 5, 13, P.brown2)
    L.rect(img, 44, 124, 18, 14, P.yellow3) -- cushion

    -- strong wooden table with a fruit bowl
    L.rect(img, 196, 138, 108, 8, P.ink)
    L.rect(img, 197, 139, 106, 6, P.brown)
    L.hline(img, 197, 302, 139, P.brown3)
    L.rect(img, 202, 146, 6, 30, P.brown2); L.rect(img, 292, 146, 6, 30, P.brown2)
    L.rect(img, 208, 146, 84, 3, P.brown2)
    L.ellipse(img, 250, 134, 12, 4, P.white)
    L.circle(img, 245, 130, 3, P.orange); L.circle(img, 252, 129, 3, P.yellow); L.circle(img, 258, 131, 3, P.green)

    -- door to the outside, and the go bag ready beside it
    H.framed(img, 410, 60, 50, 116, P.brown2, P.brown)
    L.rect(img, 418, 70, 34, 40, P.brown3)
    L.rect(img, 418, 118, 34, 50, P.brown3)
    L.circle(img, 452, 124, 2, P.yellow)
    goBag(img, 388, 158)
    return img
  end

  function M.lampFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(24, 44)
      local dx = f
      L.vline(img, 12, 0, 22, P.gray4)
      L.line(img, 12, 22, 12 + dx, 26, P.gray4)
      L.poly(img, { 6 + dx, 26, 18 + dx, 26, 22 + dx, 36, 2 + dx, 36 }, P.yellow2)
      L.hline(img, 7 + dx, 17 + dx, 27, P.yellow)
      L.ellipse(img, 12 + dx, 38, 3, 2, P.yellow3)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  function M.glassCabinetFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(46, 112)
      local lean = f
      L.rect(img, 1 + lean, 1, 44, 4, P.brown3)
      L.rect(img, 1, 4, 44, 107, P.brown2)
      L.rect(img, 4, 7, 38, 100, P.glass2)
      -- plates and cups behind the glass
      for i, y in ipairs({ 34, 60, 86 }) do
        L.rect(img, 4, y, 38, 3, P.brown3)
        local s = (f == 1 and i == 2) and 1 or 0
        L.circle(img, 13 + s, y - 7, 6, P.white); L.circle(img, 13 + s, y - 7, 3, P.blue3)
        L.rect(img, 25 + s, y - 8, 6, 8, P.white); L.rect(img, 31 + s, y - 6, 2, 3, P.white)
        L.rect(img, 35, y - 10, 4, 10, P.teal3)
      end
      L.vline(img, 22, 7, 106, P.brown2)
      L.line(img, 8, 20, 16, 12, P.white); L.line(img, 27, 50, 35, 42, P.white)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Schoolyard: sky, school building, open field; tree (x 260, y 34) and post (x 360, y 14) are sprites
  function M.schoolyard()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 150, { P.sky2, P.sky3, P.sky4, P.sky5 })
    L.ellipse(img, 120, 26, 16, 6, P.white); L.ellipse(img, 136, 22, 10, 6, P.white)
    L.ellipse(img, 360, 18, 14, 5, P.white)
    -- far hills
    L.ellipse(img, 300, 150, 140, 22, P.hill)
    L.ellipse(img, 460, 150, 90, 16, P.hill2)
    -- school building
    L.poly(img, { 0, 52, 96, 30, 196, 52 }, P.roof)
    L.rect(img, 0, 50, 196, 4, P.roof2)
    L.rect(img, 0, 54, 190, 102, P.wall)
    L.rect(img, 0, 96, 190, 4, P.wall2)
    for x = 10, 170, 36 do
      for _, y in ipairs({ 62, 106 }) do
        L.rect(img, x, y, 24, 22, P.ink); L.rect(img, x + 1, y + 1, 22, 20, P.glass2); L.vline(img, x + 12, y + 1, y + 20, P.teal2)
      end
    end
    L.rect(img, 150, 120, 30, 36, P.ink); L.rect(img, 151, 121, 28, 35, P.brown)
    L.rect(img, 0, 152, 196, 4, P.concrete3)
    -- flag pole by the building
    L.vline(img, 214, 40, 158, P.gray3)
    L.rect(img, 215, 42, 18, 6, P.flagBlue); L.rect(img, 215, 48, 18, 6, P.flagRed)
    L.poly(img, { 215, 42, 222, 48, 215, 54 }, P.flagWhite)
    -- ground: grass with a wide open field in the middle
    L.vgrad(img, 0, 156, W, Hh - 156, { P.grass, P.grass2, P.grass2, P.grass3 })
    L.ellipse(img, 250, 200, 120, 28, P.green4)
    L.ellipse(img, 250, 200, 110, 22, P.grass)
    L.seed(52)
    for _ = 1, 60 do
      local x, y = L.randi(0, W - 1), L.randi(160, Hh - 2)
      L.px(img, x, y, P.grass3); L.px(img, x, y - 1, P.grass3)
    end
    -- path from the building door
    L.poly(img, { 150, 156, 180, 156, 190, 270, 130, 270 }, P.concrete)
    return img
  end

  function M.treeFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(76, 128)
      local dx = f
      L.rect(img, 33, 64, 10, 64, P.brown2)
      L.vline(img, 36, 66, 126, P.brown)
      L.line(img, 38, 80, 52, 66, P.brown2, 3)
      for _, b in ipairs({ { 38, 34, 30, 24 }, { 20, 52, 18, 16 }, { 56, 50, 18, 16 }, { 38, 14, 18, 12 } }) do
        L.ellipse(img, b[1] + dx, b[2], b[3], b[4], P.green2)
      end
      for _, b in ipairs({ { 34, 28, 18, 12 }, { 22, 46, 10, 8 }, { 54, 44, 10, 8 } }) do
        L.ellipse(img, b[1] + dx, b[2], b[3], b[4], P.green)
      end
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  function M.postFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(60, 150)
      local dx = f
      L.rect(img, 27, 10, 6, 140, P.concrete3)
      L.vline(img, 28, 10, 148, P.concrete)
      L.rect(img, 12 + dx, 16, 36, 4, P.gray4)
      for _, x in ipairs({ 14, 28, 44 }) do L.rect(img, x + dx, 12, 3, 4, P.white) end
      -- wires running off both sides
      L.line(img, 0, 22, 14 + dx, 13, P.ink2); L.line(img, 46 + dx, 13, 59, 24, P.ink2)
      L.line(img, 0, 26, 29 + dx, 13, P.ink2)
      -- a transformer box
      L.rect(img, 33, 36, 10, 14, P.gray3); L.rect(img, 34, 37, 8, 3, P.gray2)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Beach: high ground with a path on the left, the sea going out, fish on the wet sand.
  -- The wave line (x 140, y 128) is a sprite.
  function M.beach()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 112, { P.sky2, P.sky3, P.sky4, P.sky5 })
    L.ellipse(img, 300, 30, 18, 6, P.white); L.ellipse(img, 318, 26, 10, 6, P.white)
    -- sea, far out
    L.vgrad(img, 0, 104, W, 28, { P.water3, P.water, P.water2 })
    -- wet sand left by the sea going out, then dry sand
    L.vgrad(img, 0, 132, W, 30, { P.dirt, P.tan2, P.tan })
    L.vgrad(img, 0, 162, W, Hh - 162, { P.tan, P.paper2, P.paper2 })
    L.seed(77)
    for _ = 1, 40 do L.px(img, L.randi(0, W - 1), L.randi(166, Hh - 2), P.paper3) end
    -- stranded fish and shells on the wet sand
    for _, fsh in ipairs({ { 250, 146 }, { 300, 152 }, { 352, 144 } }) do
      L.ellipse(img, fsh[1], fsh[2], 5, 2, P.metal2); L.poly(img, { fsh[1] + 5, fsh[2], fsh[1] + 9, fsh[2] - 3, fsh[1] + 9, fsh[2] + 3 }, P.metal2)
      L.px(img, fsh[1] - 3, fsh[2] - 1, P.ink)
    end
    L.circle(img, 420, 180, 2, P.pink); L.circle(img, 200, 190, 2, P.white)
    -- high ground: a green hill on the left with a zigzag path and coconut trees
    L.poly(img, { 0, 40, 40, 44, 90, 70, 140, 110, 180, 150, 196, 176, 0, 176 }, P.hill2)
    L.poly(img, { 0, 46, 40, 50, 86, 76, 132, 114, 168, 152, 180, 176, 0, 176 }, P.grass2)
    L.line(img, 170, 172, 120, 150, P.paper3, 3); L.line(img, 120, 150, 140, 122, P.paper3, 3)
    L.line(img, 140, 122, 90, 96, P.paper3, 3); L.line(img, 90, 96, 100, 74, P.paper3, 3)
    for _, t in ipairs({ { 40, 40 }, { 66, 58 } }) do
      L.line(img, t[1], t[2], t[1] + 4, t[2] - 30, P.brown2, 3)
      for _, d in ipairs({ { -14, 4 }, { 14, 4 }, { -10, -4 }, { 10, -4 } }) do
        L.line(img, t[1] + 4, t[2] - 30, t[1] + 4 + d[1], t[2] - 30 + d[2] + 6, P.green2, 2)
      end
    end
    -- a palm on the right
    L.line(img, 452, 176, 440, 120, P.brown2, 4)
    for _, d in ipairs({ { -22, 8 }, { 20, 10 }, { -14, -4 }, { 16, -4 } }) do
      L.line(img, 442, 118, 442 + d[1], 118 + d[2] + 8, P.green2, 3)
    end
    return img
  end

  function M.waveFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(340, 8)
      for x = 0, 339 do
        local y = 3 + math.floor(1.5 * math.sin((x + f * 6) / 9))
        L.px(img, x, y, P.white); L.px(img, x, y + 1, P.water3)
      end
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Choice pictures, 72x52 ----------------------------------------------------
  local function roomBackdrop(wallTop, wallBottom)
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 38, { wallTop, wallBottom })
    L.rect(img, 0, 38, PW, PH - 38, P.brown3)
    L.hline(img, 0, PW - 1, 38, P.brown2)
    L.hline(img, 0, PW - 1, 45, P.tan2)
    return img
  end
  local function bedroomBackdrop() return roomBackdrop(P.sky5, P.sky4) end
  local function homeBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 38, { P.wall, P.cream })
    H.tileFloor(img, 38, PW, PH, 7)
    return img
  end
  local function yardBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 30, { P.sky3, P.sky5 })
    L.vgrad(img, 0, 30, PW, PH - 30, { P.grass, P.grass2 })
    return img
  end
  local function beachBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 16, { P.sky3, P.sky5 })
    L.vgrad(img, 0, 16, PW, 8, { P.water, P.water2 })
    L.rect(img, 0, 24, PW, 10, P.tan2)
    L.vgrad(img, 0, 34, PW, PH - 34, { P.tan, P.paper2 })
    return img
  end
  local function smallBed(img, x, y, w)
    L.rect(img, x, y - 8, 3, 14, P.brown)
    L.rect(img, x + 3, y, w - 3, 4, P.brown)
    L.rect(img, x + 3, y - 5, w - 3, 5, P.white)
    L.rect(img, x + 12, y - 6, w - 15, 6, P.blue)
    L.ellipse(img, x + 7, y - 6, 4, 2, P.white)
    L.rect(img, x + 3, y + 4, 2, 4, P.brown2); L.rect(img, x + w - 3, y + 4, 2, 4, P.brown2)
  end
  local function smallWindow(img, x, y, w, h)
    L.rect(img, x, y, w, h, P.ink)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, P.teal2)
    L.vgrad(img, x + 3, y + 3, w - 6, h - 6, { P.sky3, P.sky5 })
    L.rect(img, x + w // 2, y + 3, 1, h - 6, P.teal2)
    L.line(img, x + 5, y + 10, x + 10, y + 5, P.white)
  end
  local function smallCabinet(img, x, y, w, h)
    L.rect(img, x, y, w, h, P.ink)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, P.brown2)
    L.rect(img, x + 3, y + 3, w - 6, h - 5, P.brown)
  end
  local function wallClock(img, x, y)
    L.circle(img, x, y, 5, P.ink); L.circle(img, x, y, 4, P.white)
    L.vline(img, x, y - 3, y, P.ink); L.hline(img, x, x + 2, y, P.ink)
  end
  local function smallTree(img, x, y)
    L.rect(img, x + 6, y + 14, 4, 18, P.brown2)
    L.ellipse(img, x + 8, y + 10, 10, 9, P.green2); L.ellipse(img, x + 6, y + 7, 6, 5, P.green)
  end
  local function smallPost(img, x, y)
    L.rect(img, x, y, 3, 40, P.concrete3)
    L.rect(img, x - 6, y + 3, 15, 2, P.gray4)
    L.line(img, 0, y + 8, x - 6, y + 4, P.ink2); L.line(img, x + 8, y + 4, PW - 1, y + 10, P.ink2)
  end
  local function smallFish(img, x, y)
    L.ellipse(img, x, y, 4, 2, P.metal2); L.poly(img, { x + 4, y, x + 7, y - 2, x + 7, y + 2 }, P.metal2); L.px(img, x - 2, y - 1, P.ink)
  end
  local function bubble(img, x, y, text)
    L.rect(img, x, y, 14, 8, P.white); L.rect(img, x, y, 14, 1, P.gray2)
    L.px(img, x + 2, y + 8, P.white)
    L.text(img, x + 2, y + 1, text, P.ink, 1)
  end

  function M.choicePicturesNormal()
    local pics = {}
    local kid = function(f, girl) return CH.student(f, girl) end

    -- Normal 1 (bedroom, before) -------------------------------------------
    local low = bedroomBackdrop()
    smallCabinet(low, 40, 28, 28, 12)
    L.rect(low, 44, 20, 14, 9, P.paper3); L.rect(low, 44, 20, 14, 2, P.paper2)
    L.blit(low, kid(0, false), 14, 6)
    pics.choice_low_shelf = low

    local pic = bedroomBackdrop()
    smallBed(pic, 8, 32, 50)
    L.rect(pic, 18, 4, 30, 20, P.ink); L.rect(pic, 19, 5, 28, 18, P.brown2); L.rect(pic, 21, 7, 24, 14, P.yellow3)
    L.poly(pic, { 24, 19, 31, 11, 38, 19 }, P.roof)
    L.blit(pic, kid(0, true), 46, 6, true)
    pics.choice_picture_bed = pic

    local bw = bedroomBackdrop()
    smallWindow(bw, 18, 2, 36, 22)
    smallBed(bw, 10, 33, 52)
    L.ellipse(bw, 20, 26, 3, 3, P.skin1); L.ellipse(bw, 20, 25, 3, 2, P.hair)
    pics.choice_bed_window = bw

    local top = bedroomBackdrop()
    smallCabinet(top, 42, 10, 22, 34)
    L.hline(top, 45, 60, 22, P.brown3); L.hline(top, 45, 60, 32, P.brown3)
    L.rect(top, 43, 4, 5, 6, P.red); L.rect(top, 48, 2, 5, 8, P.blue); L.rect(top, 53, 5, 6, 5, P.green)
    L.blit(top, kid(0, false), 16, 6)
    L.line(top, 39, 18, 44, 10, P.skin1, 2)
    pics.choice_books_top = top

    -- Normal 2 (home, before) ----------------------------------------------
    local kit = homeBackdrop()
    goBag(kit, 26, 22, true)
    bottle(kit, 10, 26); can(kit, 46, 34); flashlight(kit, 44, 24); whistle(kit, 12, 40)
    L.px(kit, 58, 10, P.yellow); L.px(kit, 60, 8, P.yellow); L.px(kit, 62, 11, P.yellow)
    pics.choice_go_bag = kit

    local later = homeBackdrop()
    wallClock(later, 56, 12)
    L.rect(later, 40, 40, 18, 4, P.orange); L.rect(later, 40, 40, 18, 1, P.orange2)
    L.blit(later, kid(0, false), 12, 6)
    bubble(later, 32, 4, "?")
    pics.choice_pack_later = later

    local hid = homeBackdrop()
    L.rect(hid, 22, 6, 24, 14, P.paper3); L.rect(hid, 22, 6, 24, 2, P.paper2)
    L.rect(hid, 18, 20, 32, 14, P.paper3); L.rect(hid, 18, 20, 32, 2, P.paper2)
    L.rect(hid, 14, 34, 40, 10, P.paper3); L.rect(hid, 14, 34, 40, 2, P.paper2)
    L.rect(hid, 50, 40, 6, 4, P.orange); L.px(hid, 55, 39, P.orange2)
    L.vline(hid, 34, 6, 43, P.paper2)
    pics.choice_bag_hidden = hid

    local toys = homeBackdrop()
    goBag(toys, 28, 22, true)
    L.circle(toys, 14, 36, 6, P.red); L.hline(toys, 8, 20, 36, P.white)
    L.circle(toys, 54, 34, 6, P.tan); L.circle(toys, 50, 28, 2, P.tan); L.circle(toys, 58, 28, 2, P.tan)
    L.px(toys, 52, 33, P.ink); L.px(toys, 56, 33, P.ink)
    L.rect(toys, 32, 16, 4, 4, P.pink); L.rect(toys, 38, 14, 4, 4, P.purple)
    pics.choice_bag_toys = toys

    -- Normal 3 (home, during) ----------------------------------------------
    local tbl = homeBackdrop()
    L.rect(tbl, 12, 12, 50, 4, P.brown)
    L.hline(tbl, 12, 61, 12, P.brown3)
    L.hline(tbl, 12, 61, 16, P.ink)
    L.rect(tbl, 14, 17, 3, 30, P.brown2)
    L.rect(tbl, 57, 17, 3, 30, P.brown2)
    L.blit(tbl, H.crouchingKid(), 31, 18)
    H.shakeLines(tbl, 4, 2)
    H.shakeLines(tbl, 66, 2)
    pics.choice_table = tbl

    local run = homeBackdrop()
    L.rect(run, 52, 4, 18, 40, P.ink); L.rect(run, 53, 5, 16, 38, P.brown)
    L.rect(run, 56, 9, 10, 8, P.brown3); L.circle(run, 66, 26, 1, P.yellow)
    L.blit(run, kid(1, false), 22, 4)
    H.speedLines(run, 6, { 18, 25, 32 })
    H.shakeLines(run, 2, 4)
    pics.choice_run_home = run

    local gc = homeBackdrop()
    L.rect(gc, 44, 2, 24, 44, P.ink); L.rect(gc, 45, 3, 22, 42, P.brown2); L.rect(gc, 47, 5, 18, 38, P.glass2)
    for _, y in ipairs({ 16, 30 }) do L.rect(gc, 47, y, 18, 2, P.brown3) end
    L.circle(gc, 52, 11, 3, P.white); L.rect(gc, 58, 22, 4, 6, P.white); L.circle(gc, 53, 37, 3, P.white)
    L.line(gc, 49, 12, 53, 8, P.white)
    L.blit(gc, kid(0, true), 16, 6)
    H.shakeLines(gc, 69, 4)
    H.shakeLines(gc, 40, 2)
    pics.choice_glass_cabinet = gc

    local sofa = homeBackdrop()
    L.rect(sofa, 2, 14, 34, 28, P.ink); L.rect(sofa, 3, 15, 32, 14, P.teal); L.rect(sofa, 3, 29, 32, 10, P.teal2)
    L.rect(sofa, 3, 18, 5, 22, P.teal2)
    L.blit(sofa, H.coverKid(), 34, 20)
    H.shakeLines(sofa, 66, 4)
    H.shakeLines(sofa, 40, 2)
    pics.choice_sofa = sofa

    -- Normal 4 (schoolyard, during) ----------------------------------------
    local field = yardBackdrop()
    smallTree(field, -4, 2)
    L.ellipse(field, 40, 44, 26, 5, P.green4)
    L.blit(field, kid(0, true), 28, 6)
    H.shakeLines(field, 62, 4)
    pics.choice_open_field = field

    local tree = yardBackdrop()
    L.rect(tree, 30, 18, 8, 30, P.brown2)
    L.ellipse(tree, 34, 12, 30, 14, P.green2); L.ellipse(tree, 30, 8, 18, 8, P.green)
    L.blit(tree, kid(0, false), 38, 6)
    H.shakeLines(tree, 66, 2)
    pics.choice_tree = tree

    local post = yardBackdrop()
    smallPost(post, 50, 6)
    L.blit(post, kid(0, true), 24, 6)
    H.shakeLines(post, 6, 4)
    pics.choice_post = post

    local bld = yardBackdrop()
    L.poly(bld, { 40, 10, 56, 2, 72, 10 }, P.roof)
    L.rect(bld, 42, 10, 30, 36, P.wall)
    L.rect(bld, 52, 24, 12, 22, P.ink); L.rect(bld, 53, 25, 10, 21, P.brown)
    L.rect(bld, 44, 14, 7, 7, P.glass2); L.rect(bld, 64, 14, 7, 7, P.glass2)
    L.blit(bld, kid(1, false), 16, 6)
    H.speedLines(bld, 2, { 18, 25, 32 })
    H.shakeLines(bld, 36, 2)
    pics.choice_building = bld

    -- Normal 5 (beach, after) ----------------------------------------------
    local hill = beachBackdrop()
    L.poly(hill, { 0, 4, 20, 6, 44, 30, 56, 52, 0, 52 }, P.grass2)
    L.line(hill, 40, 50, 22, 32, P.paper3, 2); L.line(hill, 22, 32, 10, 14, P.paper3, 2)
    L.blit(hill, kid(2, false), 12, 4, true)
    pics.choice_high_ground = hill

    local fish = beachBackdrop()
    smallFish(fish, 62, 46); smallFish(fish, 50, 40)
    L.blit(fish, H.crouchingKid(), 30, 20)
    pics.choice_fish = fish

    local watch = beachBackdrop()
    L.poly(watch, { 44, 24, 58, 6, 70, 2, 72, 24 }, P.water)
    L.line(watch, 52, 12, 64, 4, P.white, 2)
    L.blit(watch, kid(0, false), 16, 6)
    pics.choice_watch_waves = watch

    local wait = beachBackdrop()
    L.blit(wait, kid(0, true), 30, 6)
    bubble(wait, 46, 4, "")
    for i = 0, 2 do L.rect(wait, 50 + i * 3, 8, 2, 2, P.ink) end
    smallFish(wait, 14, 28)
    pics.choice_wait_beach = wait

    return pics
  end

  return M
end
