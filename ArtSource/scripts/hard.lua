-- Earthquake / Hard art: four new Settings (480x270, shown at 4x), the mall_a moving parts
-- (2 frames each) and one 72x52 picture per Hard Choice (docs/design/earthquake-hard.md).
-- Clean, bright and gentle: parts sway by 1 art pixel, nothing falls, no one is shown hurt
-- beyond a bandaged leg, and the malls carry made-up signs with no real logos or names.

return function(L, CH, LV, NM)
  local P = L.P
  local H = LV.helpers
  local PW, PH = H.PW, H.PH
  local M = {}
  local alpha = app.pixelColor.rgbaA

  -- People ----------------------------------------------------------------------
  local function recolorClothes(s, girl, shirt, shade)
    local bag = girl and { P.teal, P.teal2 } or { P.red, P.red2 }
    L.recolor(s, { [P.white] = shirt, [P.gray1] = shade, [P.gray2] = shade, [bag[1]] = shade, [bag[2]] = shade,
      [P.blue] = shirt })
  end

  -- A family member: a student recoloured into everyday clothes and stretched 6 px taller.
  local function adult(frame, girl, shirt, shade)
    local s = CH.student(frame, girl)
    recolorClothes(s, girl, shirt, shade)
    local img = L.newImage(28, 50)
    for y = 0, 49 do
      local sy = y <= 30 and y or (y < 36 and 30 or y - 6)
      for x = 0, 27 do
        local c = s:getPixel(x, sy)
        if alpha(c) > 0 then img:drawPixel(x, y, c) end
      end
    end
    return img
  end

  -- A person sitting on the floor with the legs stretched out (head, torso, straight legs).
  local function sitter(girl, shirt, shade, bandage)
    local s = CH.student(0, girl)
    if shirt then recolorClothes(s, girl, shirt, shade) end
    local img = L.newImage(34, 34)
    for y = 0, 29 do
      for x = 0, 27 do
        local c = s:getPixel(x, y)
        if alpha(c) > 0 then img:drawPixel(x, y, c) end
      end
    end
    L.rect(img, 9, 28, 10, 3, P.navy)
    L.rect(img, 18, 29, 12, 2, P.skin2)
    L.rect(img, 18, 27, 12, 2, P.skin1)
    L.rect(img, 30, 26, 2, 5, P.ink2)
    if bandage then L.rect(img, 22, 27, 4, 2, P.white); L.rect(img, 22, 29, 4, 1, P.gray1) end
    L.outline(img, P.ink)
    return img
  end

  local function kid(f, girl) return CH.student(f, girl) end

  -- Small props shared by the scenes and the pictures --------------------------------
  local function goBag(img, x, y)
    L.rect(img, x + 1, y + 2, 14, 16, P.orange)
    L.rect(img, x + 1, y + 2, 14, 3, P.orange2)
    L.rect(img, x + 3, y + 9, 10, 2, P.yellow3)
    L.rect(img, x + 4, y + 12, 8, 5, P.orange2)
    L.hline(img, x + 5, x + 10, y, P.orange2)
    L.px(img, x + 4, y + 1, P.orange2); L.px(img, x + 11, y + 1, P.orange2)
  end
  local function smallBag(img, x, y)
    L.rect(img, x, y + 1, 9, 10, P.orange); L.rect(img, x, y + 1, 9, 2, P.orange2)
    L.rect(img, x + 2, y + 5, 5, 1, P.yellow3); L.hline(img, x + 2, x + 6, y, P.orange2)
  end
  local function radio(img, x, y)
    L.rect(img, x, y + 3, 16, 10, P.ink); L.rect(img, x + 1, y + 4, 14, 8, P.red)
    L.circle(img, x + 5, y + 8, 3, P.gray4); L.circle(img, x + 5, y + 8, 1, P.gray2)
    L.rect(img, x + 10, y + 6, 4, 2, P.yellow3); L.rect(img, x + 10, y + 9, 4, 1, P.red2)
    L.line(img, x + 13, y + 3, x + 17, y - 3, P.gray4)
  end
  local function notes(img, x, y)
    L.vline(img, x + 2, y, y + 4, P.ink2); L.rect(img, x, y + 4, 3, 2, P.ink2)
    L.vline(img, x + 8, y - 2, y + 2, P.ink2); L.rect(img, x + 6, y + 2, 3, 2, P.ink2)
    L.hline(img, x + 2, x + 8, y - 2, P.ink2)
  end
  local function phone(img, x, y)
    L.rect(img, x, y, 7, 11, P.ink); L.rect(img, x + 1, y + 1, 5, 8, P.teal3)
    L.hline(img, x + 2, x + 4, y + 3, P.ink2); L.hline(img, x + 2, x + 4, y + 5, P.ink2); L.hline(img, x + 2, x + 3, y + 7, P.ink2)
  end
  local function bubble(img, x, y, text, w)
    w = w or 14
    L.rect(img, x, y, w, 9, P.white); L.rect(img, x, y, w, 1, P.gray2)
    L.px(img, x + 2, y + 9, P.white); L.px(img, x + 3, y + 10, P.white)
    if text ~= "" then L.text(img, x + 2, y + 1, text, P.ink, 1) end
  end
  local function exitSign(img, x, y)
    L.rect(img, x, y, 26, 11, P.ink); L.rect(img, x + 1, y + 1, 24, 9, P.green2)
    L.text(img, x + 3, y + 2, "EXIT", P.white, 1)
  end
  local function elevatorDoors(img, x, y, w, h)
    L.rect(img, x - 3, y - 3, w + 6, h + 3, P.gray3)
    L.rect(img, x, y, w, h, P.metal)
    L.vline(img, x + w // 2, y, y + h - 1, P.metal2)
    L.vline(img, x + 2, y, y + h - 1, P.gray1)
    L.vline(img, x + w // 2 + 2, y, y + h - 1, P.gray1)
    -- up and down arrows above the door
    local ax = x + w // 2
    L.rect(img, ax - 5, y - 12, 11, 8, P.ink)
    L.poly(img, { ax - 3, y - 8, ax, y - 11, ax + 3, y - 8 }, P.yellow)
    L.poly(img, { ax - 3, y - 7, ax, y - 5, ax + 3, y - 7 }, P.yellow2)
  end
  local function shopFront(img, x, y, w, h, awning, awning2, goods)
    -- awning with a plain stripe pattern (the shop sign is an icon, no letters, no logos)
    for i = 0, w - 1, 8 do L.rect(img, x + i, y, 4, 8, awning); L.rect(img, x + i + 4, y, 4, 8, awning2) end
    L.hline(img, x, x + w - 1, y + 8, P.ink)
    L.rect(img, x, y + 9, w, h - 9, P.ink)
    L.rect(img, x + 2, y + 11, w - 4, h - 13, P.glass2)
    if goods then goods(img, x + 2, y + 11, w - 4, h - 13) end
    L.line(img, x + 6, y + 34, x + 20, y + 20, P.white)
    L.line(img, x + 10, y + 38, x + 26, y + 22, P.white)
  end

  -- mall_a: the food court inside a big General Santos mall (during) -----------------
  -- Left to right: a shop with a glass window (the glass display case is a sprite at
  -- x 66, y 112), the elevator, food court tables, food stalls, the exit.
  -- The hanging shop sign is a sprite at x 330, y 0.
  local function mallFloor(img, top, W, Hh)
    for ty = top, Hh - 1, 14 do
      for tx = 0, W - 1, 14 do
        local odd = ((tx // 14) + ((ty - top) // 14)) % 2 == 1
        L.rect(img, tx, ty, 14, 14, odd and P.gray1 or P.white)
      end
    end
    for ty = top, Hh - 1, 14 do L.hline(img, 0, W - 1, ty, P.gray2) end
    L.seed(91)
    for _ = 1, 30 do
      local x, y = L.randi(0, W - 8), L.randi(top + 2, Hh - 2)
      L.hline(img, x, x + 4, y, P.sky5)
    end
  end

  function M.mallA()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    -- ceiling with light panels
    L.rect(img, 0, 0, W, 14, P.concrete)
    L.hline(img, 0, W - 1, 14, P.concrete3)
    for x = 12, W, 60 do L.rect(img, x, 4, 30, 5, P.yellow3); L.hline(img, x, x + 29, 9, P.concrete2) end
    -- upper floor railing and a soft back wall
    L.vgrad(img, 0, 15, W, 157, { P.cream, P.wall, P.wall })
    L.rect(img, 0, 15, W, 18, P.wall2)
    for x = 0, W, 10 do L.vline(img, x, 15, 32, P.wall3) end
    L.rect(img, 0, 32, W, 3, P.wall3)
    L.rect(img, 0, 172, W, 4, P.gray3)
    mallFloor(img, 176, W, Hh)

    -- shop with a big glass window full of shirts and shoes
    shopFront(img, 10, 52, 118, 120, P.pink, P.white, function(g, gx, gy, gw, gh)
      for i, c in ipairs({ P.blue, P.yellow2, P.green, P.red3 }) do
        local sx = gx + 8 + (i - 1) * 27
        L.rect(g, sx, gy + 14, 16, 18, c); L.rect(g, sx - 3, gy + 14, 22, 5, c)
        L.rect(g, sx + 6, gy + 12, 4, 2, P.ink2)
      end
      for i = 0, 3 do L.rect(g, gx + 10 + i * 27, gy + 52, 12, 5, P.navy); L.rect(g, gx + 18 + i * 27, gy + 49, 4, 4, P.navy) end
      L.rect(g, gx, gy + gh - 8, gw, 8, P.paper2)
    end)
    -- made-up shop sign: a shirt icon on a round board
    L.circle(img, 69, 42, 10, P.ink); L.circle(img, 69, 42, 9, P.white)
    L.rect(img, 65, 39, 9, 8, P.pink); L.rect(img, 62, 39, 15, 3, P.pink)

    -- elevator
    elevatorDoors(img, 144, 98, 40, 74)

    -- food court: two strong tables with stools
    for _, tx in ipairs({ 204, 258 }) do
      L.rect(img, tx, 140, 46, 6, P.ink)
      L.rect(img, tx + 1, 141, 44, 4, P.brown3)
      L.hline(img, tx + 1, tx + 44, 141, P.tan)
      L.rect(img, tx + 4, 146, 4, 30, P.brown2); L.rect(img, tx + 38, 146, 4, 30, P.brown2)
      L.rect(img, tx + 8, 148, 30, 2, P.brown2)
      L.ellipse(img, tx + 14, 137, 5, 2, P.white); L.circle(img, tx + 14, 135, 2, P.orange)
      L.rect(img, tx + 30, 132, 5, 8, P.red3); L.rect(img, tx + 31, 129, 1, 3, P.white)
    end
    for _, sx in ipairs({ 196, 252, 306 }) do
      L.rect(img, sx - 1, 157, 10, 5, P.ink); L.rect(img, sx, 158, 8, 3, P.teal); L.rect(img, sx + 3, 162, 2, 14, P.gray4)
    end

    -- food stalls with picture menu boards
    local stalls = { { 318, P.orange, P.yellow3 }, { 376, P.green2, P.green4 } }
    for _, s in ipairs(stalls) do
      local sx = s[1]
      L.rect(img, sx, 56, 54, 26, P.ink); L.rect(img, sx + 1, 57, 52, 24, s[2])
      L.rect(img, sx + 4, 60, 20, 18, s[3]); L.rect(img, sx + 28, 60, 22, 18, s[3])
      L.ellipse(img, sx + 14, 72, 7, 3, P.white); L.ellipse(img, sx + 14, 70, 6, 2, P.tan)
      L.rect(img, sx + 35, 63, 8, 12, P.red3); L.rect(img, sx + 38, 60, 1, 4, P.white)
      L.rect(img, sx, 120, 54, 52, P.ink); L.rect(img, sx + 1, 121, 52, 51, P.brown3)
      L.rect(img, sx + 1, 121, 52, 6, P.tan)
      L.rect(img, sx + 6, 134, 42, 3, s[2])
    end

    -- exit at the far right
    exitSign(img, 442, 74)
    L.rect(img, 438, 90, 40, 82, P.ink)
    L.rect(img, 440, 92, 17, 80, P.glass2); L.rect(img, 459, 92, 17, 80, P.glass2)
    L.vline(img, 455, 120, 132, P.gray3); L.vline(img, 461, 120, 132, P.gray3)
    L.line(img, 444, 110, 452, 102, P.white)
    -- a potted plant by the elevator
    L.rect(img, 118, 158, 14, 14, P.ink); L.rect(img, 119, 159, 12, 13, P.brown)
    L.ellipse(img, 125, 150, 9, 9, P.green2); L.ellipse(img, 123, 147, 5, 5, P.green)
    return img
  end

  function M.mallSignFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(60, 50)
      local dx = f
      L.line(img, 14, 0, 14 + dx, 20, P.gray4); L.line(img, 46, 0, 46 + dx, 20, P.gray4)
      L.rect(img, 4 + dx, 20, 52, 26, P.purple)
      L.rect(img, 6 + dx, 22, 48, 22, P.yellow3)
      -- made-up sign: a big star and a shopping bag, no letters
      L.poly(img, { 18 + dx, 25, 21 + dx, 31, 27 + dx, 32, 22 + dx, 36, 24 + dx, 42, 18 + dx, 38, 12 + dx, 42, 14 + dx, 36, 9 + dx, 32, 15 + dx, 31 }, P.yellow2)
      L.rect(img, 34 + dx, 30, 14, 12, P.pink); L.rect(img, 37 + dx, 26, 8, 5, P.purple2)
      L.rect(img, 38 + dx, 27, 6, 3, P.yellow3)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  function M.shopDisplayFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(40, 64)
      local lean = f
      L.rect(img, 2, 40, 36, 24, P.brown2)
      L.rect(img, 4, 42, 32, 20, P.brown)
      L.rect(img, 2 + lean, 2, 36, 38, P.glass3)
      L.rect(img, 4 + lean, 4, 32, 34, P.glass2)
      L.rect(img, 4 + lean, 20, 32, 2, P.gray3)
      -- toys and a clock on the shelves
      L.circle(img, 12 + lean, 14, 4, P.red); L.rect(img, 22 + lean, 10, 8, 8, P.blue)
      L.circle(img, 14 + lean, 30, 4, P.yellow); L.circle(img, 14 + lean, 30, 2, P.white)
      L.rect(img, 24 + lean, 26, 6, 10, P.green)
      L.line(img, 7 + lean, 12, 13 + lean, 6, P.white); L.line(img, 26 + lean, 36, 33 + lean, 29, P.white)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- mall_b: outside the second mall after the shaking (still) --------------------------
  -- The mall with an outdoor stair from the upper entrance, the doors, and the open
  -- parking area on the right, where a family waits away from the building.
  function M.mallB()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 120, { P.sky2, P.sky3, P.sky4, P.sky5 })
    L.ellipse(img, 360, 26, 18, 6, P.white); L.ellipse(img, 376, 22, 11, 6, P.white)
    L.ellipse(img, 440, 48, 14, 4, P.white)
    L.ellipse(img, 420, 118, 130, 16, P.hill)

    -- the mall building
    L.rect(img, 0, 30, 236, 122, P.wall)
    L.rect(img, 0, 30, 236, 10, P.teal)
    L.rect(img, 0, 40, 236, 3, P.teal2)
    L.rect(img, 0, 92, 236, 4, P.wall2)
    -- made-up sign board: a big star and two bags, no letters or logos
    L.rect(img, 54, 48, 92, 28, P.ink); L.rect(img, 56, 50, 88, 24, P.blue)
    L.poly(img, { 78, 52, 82, 59, 90, 60, 84, 65, 86, 72, 78, 68, 70, 72, 72, 65, 66, 60, 74, 59 }, P.yellow)
    L.rect(img, 100, 58, 14, 12, P.pink); L.rect(img, 103, 55, 8, 4, P.white)
    L.rect(img, 120, 58, 14, 12, P.yellow3); L.rect(img, 123, 55, 8, 4, P.white)
    -- windows
    for _, x in ipairs({ 10, 156 }) do L.rect(img, x, 50, 22, 30, P.ink); L.rect(img, x + 1, 51, 20, 28, P.glass2) end
    -- ground doors
    L.rect(img, 70, 106, 64, 46, P.ink)
    L.rect(img, 72, 108, 29, 44, P.glass2); L.rect(img, 103, 108, 29, 44, P.glass2)
    L.line(img, 76, 122, 86, 112, P.white); L.line(img, 107, 122, 117, 112, P.white)
    exitSign(img, 89, 96)
    -- the upper entrance and the outdoor stair going down to the right
    L.rect(img, 186, 56, 40, 36, P.ink); L.rect(img, 188, 58, 36, 34, P.glass2)
    L.rect(img, 180, 92, 60, 4, P.concrete3)
    for i = 0, 9 do
      local sx, sy = 236 + i * 6, 96 + i * 6
      L.rect(img, sx, sy, 8, 56 - i * 6, P.concrete2)
      L.rect(img, sx, sy, 8, 2, P.concrete)
      L.hline(img, sx, sx + 7, sy, P.concrete3)
    end
    L.line(img, 236, 86, 296, 146, P.gray4, 2)
    L.vline(img, 236, 86, 96, P.gray4); L.vline(img, 266, 116, 126, P.gray4); L.vline(img, 296, 146, 156, P.gray4)

    -- pavement and the open parking area
    L.rect(img, 0, 152, W, 20, P.concrete)
    L.hline(img, 0, W - 1, 152, P.concrete3)
    L.rect(img, 300, 124, 180, 28, P.gray3)
    L.rect(img, 0, 172, W, Hh - 172, P.gray3)
    for x = 320, 470, 34 do L.vline(img, x, 128, 150, P.white) end
    for x = 20, 470, 40 do L.vline(img, x, 180, 230, P.white) end
    -- two parked cars at the far end of the parking area
    for _, c in ipairs({ { 424, P.red }, { 452, P.yellow2 } }) do
      local cx = c[1]
      L.rect(img, cx - 1, 125, 26, 19, P.ink)
      L.rect(img, cx, 133, 24, 10, c[2]); L.rect(img, cx + 4, 126, 15, 8, c[2])
      L.rect(img, cx + 6, 127, 11, 5, P.glass2)
      L.circle(img, cx + 5, 144, 3, P.ink); L.circle(img, cx + 19, 144, 3, P.ink)
    end
    -- a family waiting in the open, far from the wall
    L.blit(img, adult(0, true, P.purple, P.purple2), 340, 104)
    L.blit(img, kid(0, false), 364, 110)
    return img
  end

  -- home_after: the living room after the shaking (still) ------------------------------
  -- The Normal home with the lamp and cabinet drawn in, books and a cushion on the floor,
  -- a tipped stool, the family photo hanging crooked, and the go bag by the door.
  function M.homeAfter()
    local img = NM.home()
    L.blit(img, NM.lampFrames()[1], 232, 0)
    L.blit(img, NM.glassCabinetFrames()[1], 330, 64)
    -- the family photo hangs crooked
    for y = 54, 84 do
      local c = img:getPixel(150, y)
      for x = 156, 194 do img:drawPixel(x, y, c) end
    end
    L.line(img, 176, 46, 162, 57, P.gray4); L.line(img, 176, 46, 192, 52, P.gray4)
    L.poly(img, { 158, 57, 194, 51, 199, 82, 163, 88 }, P.ink)
    L.poly(img, { 160, 59, 192, 53, 197, 80, 165, 86 }, P.brown2)
    L.poly(img, { 164, 62, 189, 57, 193, 77, 168, 82 }, P.sky4)
    L.circle(img, 173, 72, 4, P.skin1); L.circle(img, 184, 70, 4, P.skin2)
    L.rect(img, 170, 76, 7, 4, P.red); L.rect(img, 181, 74, 7, 4, P.blue)
    -- a thin crack in the wall, small and calm
    L.line(img, 282, 30, 286, 40, P.wall3); L.line(img, 286, 40, 282, 50, P.wall3); L.line(img, 282, 50, 287, 60, P.wall3)
    -- books and a cushion on the floor, a tipped stool
    L.rect(img, 303, 167, 16, 6, P.ink); L.rect(img, 304, 168, 14, 4, P.red)
    L.rect(img, 311, 163, 14, 6, P.ink); L.rect(img, 312, 164, 12, 4, P.blue)
    L.rect(img, 319, 169, 14, 6, P.ink); L.rect(img, 320, 170, 12, 4, P.green)
    L.rect(img, 165, 165, 20, 10, P.ink); L.rect(img, 166, 166, 18, 8, P.yellow3); L.hline(img, 166, 183, 166, P.yellow2)
    L.rect(img, 371, 163, 18, 6, P.ink); L.rect(img, 372, 164, 16, 4, P.brown)
    L.rect(img, 385, 157, 5, 18, P.ink); L.rect(img, 386, 158, 3, 16, P.brown2)
    return img
  end

  -- evacuation_area: an open field where families wait together (still) --------------
  function M.evacuationArea()
    local W, Hh = 480, 270
    local img = L.newImage(W, Hh)
    L.vgrad(img, 0, 0, W, 100, { P.sky2, P.sky3, P.sky4, P.sky5 })
    L.ellipse(img, 120, 28, 18, 6, P.white); L.ellipse(img, 136, 24, 11, 6, P.white)
    L.ellipse(img, 340, 40, 15, 4, P.white); L.ellipse(img, 352, 37, 9, 4, P.white)
    L.ellipse(img, 240, 100, 260, 18, P.hill)
    -- far away: small houses and trees at the edge of the field
    for i, hx in ipairs({ 16, 58, 404, 446 }) do
      L.rect(img, hx, 82, 22, 14, (i % 2 == 0) and P.wall or P.paper)
      L.poly(img, { hx - 2, 82, hx + 11, 74, hx + 24, 82 }, (i % 2 == 0) and P.roof or P.teal)
      L.rect(img, hx + 9, 88, 5, 8, P.brown2)
    end
    for _, tx in ipairs({ 100, 380, 470 }) do
      L.rect(img, tx, 84, 3, 12, P.brown2); L.ellipse(img, tx + 1, 80, 7, 7, P.green2)
    end
    L.vgrad(img, 0, 96, W, Hh - 96, { P.grass, P.grass2, P.grass2, P.grass3 })
    L.seed(64)
    for _ = 1, 110 do
      local x, y = L.randi(0, W - 1), L.randi(100, Hh - 2)
      L.px(img, x, y, P.grass3); L.px(img, x + 1, y - 1, P.green4)
    end
    -- a light tent where helpers share water and news
    L.poly(img, { 328, 111, 380, 85, 432, 111 }, P.ink)
    L.poly(img, { 330, 110, 380, 86, 430, 110 }, P.white)
    L.poly(img, { 330, 110, 380, 86, 380, 110 }, P.gray1)
    L.rect(img, 332, 110, 96, 4, P.blue)
    L.vline(img, 336, 114, 150, P.gray4); L.vline(img, 424, 114, 150, P.gray4)
    L.rect(img, 351, 131, 58, 16, P.ink); L.rect(img, 352, 132, 56, 14, P.brown3); L.rect(img, 352, 132, 56, 2, P.tan)
    for i = 0, 3 do L.rect(img, 356 + i * 12, 122, 6, 10, P.water3); L.rect(img, 357 + i * 12, 120, 4, 2, P.blue2) end
    -- mats with families sitting together
    local mats = { { 112, 150, P.red3 }, { 214, 154, P.yellow3 } }
    for _, m in ipairs(mats) do
      L.rect(img, m[1], m[2], 74, 18, P.ink); L.rect(img, m[1] + 1, m[2] + 1, 72, 16, m[3])
      for x = m[1] + 6, m[1] + 70, 8 do L.vline(img, x, m[2] + 1, m[2] + 16, P.white) end
    end
    L.blit(img, sitter(true, P.teal3, P.teal), 116, 124)
    L.blit(img, sitter(false), 146, 126)
    L.blit(img, sitter(false, P.yellow, P.yellow2), 218, 128)
    L.blit(img, sitter(true), 248, 130)
    radio(img, 178, 148)
    goBag(img, 278, 146)
    return img
  end

  -- Choice pictures, 72x52 ----------------------------------------------------------
  local function homeBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 38, { P.wall, P.cream })
    H.tileFloor(img, 38, PW, PH, 7)
    return img
  end
  local function homeAfterBackdrop()
    local img = homeBackdrop()
    L.rect(img, 3, 46, 9, 3, P.red); L.rect(img, 7, 43, 8, 3, P.blue)
    return img
  end
  local function yardBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 30, { P.sky3, P.sky5 })
    L.vgrad(img, 0, 30, PW, PH - 30, { P.grass, P.grass2 })
    return img
  end
  local function mallBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 38, { P.cream, P.wall })
    L.rect(img, 0, 0, PW, 3, P.concrete)
    for ty = 38, PH - 1, 7 do
      for tx = 0, PW - 1, 7 do
        L.rect(img, tx, ty, 7, 7, (((tx // 7) + ((ty - 38) // 7)) % 2 == 1) and P.gray1 or P.white)
      end
    end
    return img
  end
  local function mallOutBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 24, { P.sky3, P.sky5 })
    L.rect(img, 0, 24, PW, 16, P.concrete)
    L.rect(img, 0, 40, PW, PH - 40, P.gray3)
    L.hline(img, 0, PW - 1, 40, P.concrete3)
    return img
  end
  local function fieldBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 22, { P.sky3, P.sky5 })
    L.ellipse(img, 36, 22, 50, 4, P.hill)
    L.vgrad(img, 0, 22, PW, PH - 22, { P.grass, P.grass2 })
    return img
  end
  local function paperMap(img, x, y)
    L.rect(img, x, y, 32, 24, P.ink); L.rect(img, x + 1, y + 1, 30, 22, P.paper)
    L.line(img, x + 3, y + 19, x + 28, y + 15, P.paper3)
  end
  local function redX(img, x, y)
    L.line(img, x - 3, y - 3, x + 3, y + 3, P.red, 2); L.line(img, x - 3, y + 3, x + 3, y - 3, P.red, 2)
  end
  local function tinyHouse(img, x, y, c)
    L.rect(img, x, y, 10, 7, c or P.wall); L.poly(img, { x - 1, y, x + 5, y - 5, x + 11, y }, P.roof)
    L.rect(img, x + 4, y + 3, 2, 4, P.brown2)
  end
  local function dashedPath(img, pts, c)
    for i = 1, #pts - 2, 2 do
      local x0, y0, x1, y1 = pts[i], pts[i + 1], pts[i + 2], pts[i + 3]
      local n = math.max(math.abs(x1 - x0), math.abs(y1 - y0))
      for s = 0, n do
        if (s // 3) % 2 == 0 then
          L.rect(img, x0 + (x1 - x0) * s / n, y0 + (y1 - y0) * s / n, 2, 2, c)
        end
      end
    end
  end
  local function arrowHead(img, x, y, c)
    L.poly(img, { x, y - 4, x + 5, y, x, y + 4 }, c)
  end
  local function door(img, x, y, h)
    L.rect(img, x, y, 18, h, P.ink); L.rect(img, x + 1, y + 1, 16, h - 1, P.brown)
    L.rect(img, x + 4, y + 5, 10, 8, P.brown3); L.circle(img, x + 14, y + h // 2 + 2, 1, P.yellow)
  end
  local function pipe(img, x, y)
    L.rect(img, x, y, 4, 32, P.metal2); L.rect(img, x, y + 12, 14, 4, P.metal2)
    L.rect(img, x + 3, y + 11, 3, 6, P.gray4)
    L.px(img, x + 9, y + 18, P.water); L.px(img, x + 10, y + 21, P.water); L.px(img, x + 9, y + 24, P.water3)
  end

  function M.choicePicturesHard()
    local pics = {}
    local mom = function() return adult(0, true, P.purple, P.purple2) end

    -- Hard 1 (home, before) -------------------------------------------------
    local meet = homeBackdrop()
    paperMap(meet, 36, 6)
    tinyHouse(meet, 41, 14)
    L.ellipse(meet, 58, 20, 8, 4, P.green4); redX(meet, 58, 19)
    L.blit(meet, kid(0, true), 6, 6)
    pics.choice_meeting_place = meet

    local own = homeBackdrop()
    L.blit(own, kid(0, false), 22, 6)
    L.line(own, 18, 22, 6, 14, P.red, 2); L.poly(own, { 2, 10, 9, 11, 5, 17 }, P.red)
    L.line(own, 52, 34, 64, 42, P.blue, 2); L.poly(own, { 69, 46, 62, 45, 66, 39 }, P.blue)
    L.line(own, 18, 36, 6, 44, P.green2, 2); L.poly(own, { 2, 48, 6, 41, 9, 47 }, P.green2)
    bubble(own, 46, 2, "?")
    pics.choice_own_way = own

    local mh = homeBackdrop()
    paperMap(mh, 36, 6)
    tinyHouse(mh, 47, 14, P.paper2); redX(mh, 52, 17)
    L.blit(mh, kid(0, false), 6, 6)
    pics.choice_meet_home = mh

    local nums = homeBackdrop()
    L.blit(nums, kid(0, true), 14, 6)
    phone(nums, 40, 18)
    bubble(nums, 50, 2, "?")
    pics.choice_phone_numbers = nums

    -- Hard 2 (schoolyard, before) -------------------------------------------
    local rf = yardBackdrop()
    L.ellipse(rf, 44, 40, 28, 7, P.green4)
    dashedPath(rf, { 2, 47, 30, 42, 62, 40 }, P.white)
    arrowHead(rf, 63, 40, P.white)
    L.blit(rf, kid(1, true), 22, 2)
    pics.choice_route_field = rf

    local rp = yardBackdrop()
    L.rect(rp, 50, 2, 3, 46, P.concrete3)
    L.rect(rp, 44, 5, 15, 2, P.gray4)
    L.line(rp, 0, 10, 44, 6, P.ink2); L.line(rp, 58, 6, PW - 1, 12, P.ink2)
    dashedPath(rp, { 2, 48, 30, 47, 56, 47, 70, 45 }, P.white)
    L.blit(rp, kid(1, false), 22, 2)
    pics.choice_route_post = rp

    local rt = yardBackdrop()
    L.rect(rt, 44, 16, 7, 32, P.brown2)
    L.ellipse(rt, 48, 12, 24, 12, P.green2); L.ellipse(rt, 44, 8, 14, 7, P.green)
    dashedPath(rt, { 2, 48, 30, 47, 58, 47, 70, 45 }, P.white)
    L.blit(rt, kid(1, true), 24, 4)
    pics.choice_route_tree = rt

    local rl = yardBackdrop()
    L.poly(rl, { 42, 12, 57, 4, 72, 12 }, P.roof)
    L.rect(rl, 44, 12, 28, 34, P.wall)
    L.rect(rl, 52, 26, 12, 20, P.ink); L.rect(rl, 53, 27, 10, 19, P.brown)
    L.blit(rl, kid(0, false), 12, 6)
    bubble(rl, 28, 0, "?")
    pics.choice_route_later = rl

    -- Hard 3 (mall_a, during) -----------------------------------------------
    local fct = mallBackdrop()
    L.rect(fct, 12, 12, 50, 4, P.brown3)
    L.hline(fct, 12, 61, 12, P.tan)
    L.hline(fct, 12, 61, 16, P.ink)
    L.rect(fct, 14, 17, 3, 30, P.brown2); L.rect(fct, 57, 17, 3, 30, P.brown2)
    L.circle(fct, 24, 9, 2, P.orange); L.rect(fct, 48, 6, 4, 6, P.red3)
    L.blit(fct, H.crouchingKid(), 31, 18)
    H.shakeLines(fct, 4, 2); H.shakeLines(fct, 66, 2)
    pics.choice_food_court_table = fct

    local rx = mallBackdrop()
    exitSign(rx, 44, 4)
    L.rect(rx, 46, 16, 26, 30, P.ink); L.rect(rx, 47, 17, 12, 29, P.glass2); L.rect(rx, 60, 17, 11, 29, P.glass2)
    L.blit(rx, kid(1, false), 16, 4)
    H.speedLines(rx, 2, { 18, 25, 32 })
    H.shakeLines(rx, 38, 2)
    pics.choice_run_exit = rx

    local ed = mallBackdrop()
    elevatorDoors(ed, 40, 18, 26, 28)
    L.blit(ed, kid(1, true), 14, 4)
    H.shakeLines(ed, 4, 4); H.shakeLines(ed, 68, 2)
    pics.choice_elevator_during = ed

    local sw = mallBackdrop()
    L.rect(sw, 36, 4, 36, 42, P.ink); L.rect(sw, 38, 6, 34, 38, P.glass2)
    L.rect(sw, 44, 14, 9, 10, P.blue); L.rect(sw, 42, 14, 13, 3, P.blue)
    L.rect(sw, 58, 14, 9, 10, P.yellow2); L.rect(sw, 56, 14, 13, 3, P.yellow2)
    L.line(sw, 40, 32, 50, 22, P.white); L.line(sw, 56, 40, 66, 30, P.white)
    L.blit(sw, kid(0, true), 12, 6)
    H.shakeLines(sw, 4, 4); H.shakeLines(sw, 32, 2)
    pics.choice_shop_window = sw

    -- Hard 4 (mall_b, after) ------------------------------------------------
    local st = mallOutBackdrop()
    L.rect(st, 0, 4, 24, 36, P.wall); L.rect(st, 0, 4, 24, 4, P.teal)
    for i = 0, 5 do
      local sx, sy = 14 + i * 6, 12 + i * 5
      L.rect(st, sx, sy, 8, 40 - sy, P.concrete2); L.rect(st, sx, sy, 8, 2, P.concrete); L.hline(st, sx, sx + 7, sy, P.concrete3)
    end
    L.line(st, 14, 6, 50, 36, P.gray4)
    L.blit(st, kid(2, false), 40, 2)
    pics.choice_mall_stairs = st

    local ea = mallOutBackdrop()
    L.rect(ea, 30, 2, 42, 38, P.wall); L.rect(ea, 30, 2, 42, 4, P.teal)
    elevatorDoors(ea, 42, 18, 24, 22)
    L.blit(ea, kid(0, false), 10, 4)
    pics.choice_elevator_after = ea

    local rpu = mallOutBackdrop()
    L.blit(rpu, kid(1, true), 38, 4)
    L.blit(rpu, kid(1, false), 18, 5)
    L.line(rpu, 34, 26, 42, 25, P.skin1, 2)
    H.speedLines(rpu, 2, { 18, 25, 32 })
    pics.choice_run_push = rpu

    local mw = mallOutBackdrop()
    L.rect(mw, 34, 0, 38, 44, P.wall); L.rect(mw, 34, 0, 38, 5, P.teal); L.rect(mw, 34, 5, 38, 2, P.teal2)
    L.rect(mw, 42, 12, 10, 12, P.glass2); L.rect(mw, 58, 12, 10, 12, P.glass2)
    L.line(mw, 52, 30, 56, 36, P.wall3); L.line(mw, 56, 36, 54, 42, P.wall3)
    L.blit(mw, kid(0, true), 14, 6)
    pics.choice_mall_wall = mw

    -- Hard 5 (home_after, after) --------------------------------------------
    local ca = homeAfterBackdrop()
    L.blit(ca, sitter(false, P.yellow, P.yellow2, true), 0, 18)
    L.blit(ca, kid(0, true), 28, 6)
    L.blit(ca, mom(), 46, 2, true)
    bubble(ca, 14, 2, "!")
    pics.choice_call_adult = ca

    local pu = homeAfterBackdrop()
    L.blit(pu, sitter(false, P.yellow, P.yellow2, true), 2, 18)
    L.blit(pu, kid(0, true), 30, 6, true)
    L.line(pu, 34, 27, 26, 33, P.skin1, 2)
    pics.choice_pull_up = pu

    local lh = homeAfterBackdrop()
    L.blit(lh, sitter(false, P.yellow, P.yellow2, true), 0, 18)
    door(lh, 54, 4, 36)
    L.blit(lh, kid(1, false), 32, 4)
    H.speedLines(lh, 24, { 14, 20 })
    pics.choice_leave_hurt = lh

    local fp = homeAfterBackdrop()
    pipe(fp, 52, 6)
    L.blit(fp, kid(0, false), 20, 6)
    L.line(fp, 38, 32, 48, 22, P.gray4, 2); L.rect(fp, 47, 18, 5, 4, P.gray4)
    pics.choice_fix_pipe = fp

    -- Hard 6 (home_after, after) --------------------------------------------
    local gb = homeAfterBackdrop()
    door(gb, 50, 4, 36)
    L.rect(gb, 54, 9, 10, 9, P.white); L.hline(gb, 55, 62, 12, P.ink2); L.hline(gb, 55, 61, 15, P.ink2)
    L.blit(gb, kid(1, true), 24, 4)
    smallBag(gb, 37, 25)
    pics.choice_go_bag_out = gb

    local bt = homeAfterBackdrop()
    L.circle(bt, 14, 30, 6, P.tan); L.circle(bt, 9, 24, 2, P.tan); L.circle(bt, 19, 24, 2, P.tan)
    L.px(bt, 12, 29, P.ink); L.px(bt, 16, 29, P.ink); L.px(bt, 14, 32, P.brown2)
    L.blit(bt, kid(1, false), 30, 4, true)
    H.speedLines(bt, 58, { 18, 25, 32 })
    pics.choice_back_for_toys = bt

    local ng = homeAfterBackdrop()
    door(ng, 50, 4, 36)
    smallBag(ng, 6, 34)
    L.blit(ng, kid(1, true), 28, 4)
    bubble(ng, 4, 16, "!")
    pics.choice_no_go_bag = ng

    local nn = homeAfterBackdrop()
    door(nn, 50, 4, 36)
    L.rect(nn, 6, 22, 12, 14, P.ink); L.rect(nn, 7, 23, 10, 12, P.white)
    L.line(nn, 18, 21, 23, 15, P.yellow2, 2)
    L.blit(nn, kid(1, false), 26, 4)
    smallBag(nn, 39, 25)
    bubble(nn, 2, 4, "?")
    pics.choice_no_note = nn

    -- Hard 7 (evacuation_area, after) ---------------------------------------
    local rd = fieldBackdrop()
    L.rect(rd, 2, 40, 68, 10, P.ink); L.rect(rd, 3, 41, 66, 8, P.red3)
    L.blit(rd, sitter(true, P.purple, P.purple2), 0, 12)
    L.blit(rd, sitter(false), 26, 14)
    radio(rd, 52, 30)
    notes(rd, 56, 14)
    pics.choice_radio = rd

    local gh = fieldBackdrop()
    tinyHouse(gh, 58, 16, P.paper2)
    L.line(gh, 61, 18, 63, 22, P.ink2)
    L.blit(gh, sitter(true, P.purple, P.purple2), 0, 18)
    L.blit(gh, kid(1, false), 32, 4)
    pics.choice_go_home = gh

    local rm = fieldBackdrop()
    L.blit(rm, kid(0, true), 12, 6)
    L.blit(rm, kid(0, false), 36, 6, true)
    bubble(rm, 25, 0, "!?", 18)
    pics.choice_rumor = rm

    local pa = fieldBackdrop()
    L.blit(pa, kid(0, true), 14, 6)
    phone(pa, 36, 18)
    bubble(pa, 44, 0, "")
    for i = 0, 2 do L.rect(pa, 47 + i * 3, 4, 2, 2, P.ink) end
    bubble(pa, 56, 14, "")
    for i = 0, 2 do L.rect(pa, 59 + i * 3, 18, 2, 2, P.ink) end
    pics.choice_phone_all_day = pa

    return pics
  end

  return M
end
