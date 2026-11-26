-- Walking students (boy and girl in school uniform) for the menu courtyard.

return function(L)
  local P = L.P
  local M = {}

  -- Walk cycle keyframes: near leg (A) and far leg (B) foot offsets and lifts.
  local CYCLE = {
    { a = 4, al = 0, b = -4, bl = 0, bob = 0 },
    { a = 2, al = 0, b = -1, bl = 2, bob = -1 },
    { a = 0, al = 0, b = 2, bl = 1, bob = -1 },
    { a = -4, al = 0, b = 4, bl = 0, bob = 0 },
    { a = -1, al = 2, b = 2, bl = 0, bob = -1 },
    { a = 2, al = 1, b = 0, bl = 0, bob = -1 },
  }

  local function leg(img, hx, hy, dx, lift, skin, shoe, sock)
    local fx, fy = hx + dx, 38 - lift
    L.line(img, hx, hy, fx, fy - 1, skin, 3)
    L.rect(img, fx, fy - 1, 3, 1, sock)
    L.rect(img, fx - 1, fy, 5, 2, shoe)
    L.px(img, fx + 3, fy, shoe)
  end

  local function arm(img, sx, sy, dx, sleeve, skin)
    local hx, hy = sx + dx, sy + 8
    L.line(img, sx, sy, sx + dx // 3, sy + 3, sleeve, 3)
    L.line(img, sx + dx // 3, sy + 3, hx, hy, skin, 2)
    L.rect(img, hx, hy, 2, 2, skin)
  end

  local function head(img, cx, cy, girl)
    if girl then
      -- long hair behind the head and a ponytail
      L.ellipse(img, cx - 2, cy + 3, 7, 8, P.hair)
      L.circle(img, cx - 8, cy + 1, 3, P.hair)
      L.circle(img, cx - 9, cy + 4, 2, P.hair)
      L.rect(img, cx - 6, cy - 1, 2, 3, P.red)
    end
    L.circle(img, cx, cy, 7, P.skin1)
    L.px(img, cx + 8, cy + 2, P.skin1)
    -- hair cap
    for y = cy - 8, cy + 6 do
      for x = cx - 8, cx + 8 do
        if L.get(img, x, y) == P.skin1 then
          local back = x < cx - 1 and y < cy + 4
          local top = y < cy - 2 and x < cx + 6
          if back or top then L.px(img, x, y, P.hair) end
        end
      end
    end
    -- fringe and shine
    L.px(img, cx + 4, cy - 2, P.hair); L.px(img, cx + 5, cy - 2, P.hair); L.px(img, cx + 2, cy - 1, P.hair)
    L.hline(img, cx - 3, cx + 1, cy - 6, P.hair2)
    -- face
    L.px(img, cx - 1, cy + 1, P.skin2) -- ear
    L.px(img, cx, cy + 1, P.skin2)
    L.vline(img, cx + 4, cy, cy + 1, P.ink)
    L.px(img, cx + 5, cy + 3, P.blush)
    L.px(img, cx + 6, cy + 4, P.skin3)
  end

  function M.student(frame, girl)
    local k = CYCLE[frame + 1]
    local img = L.newImage(28, 44)
    local by = k.bob
    local shirt, shirtShade = P.white, P.gray1
    local bottom, bottom2 = P.navy, P.navy2
    local bag, bag2 = girl and P.teal or P.red, girl and P.teal2 or P.red2

    -- far arm and far leg (darker)
    arm(img, 13, 19 + by, -k.b * 6 // 10, P.gray2, P.skin2)
    leg(img, 13, 31 + by, k.b, k.bl, P.skin2, P.ink2, P.gray2)
    -- backpack
    L.rect(img, 5, 18 + by, 6, 11, bag)
    L.rect(img, 5, 18 + by, 6, 2, bag2)
    L.vline(img, 5, 18 + by, 28 + by, bag2)
    L.rect(img, 6, 23 + by, 4, 3, bag2)
    -- torso
    L.rect(img, 9, 17 + by, 9, 13, shirt)
    L.vline(img, 9, 17 + by, 29 + by, shirtShade)
    L.hline(img, 9, 17, 29 + by, shirtShade)
    L.px(img, 14, 17 + by, P.gray2); L.px(img, 15, 18 + by, P.gray2); L.px(img, 16, 17 + by, P.gray2)
    L.rect(img, 15, 21 + by, 2, 2, P.blue) -- school patch
    L.line(img, 12, 17 + by, 11, 28 + by, bag2) -- strap
    -- near leg
    leg(img, 14, 31 + by, k.a, k.al, P.skin1, P.ink, P.white)
    -- shorts or skirt
    if girl then
      L.poly(img, { 9, 28 + by, 18, 28 + by, 20, 35 + by, 7, 35 + by }, bottom)
      for x = 9, 18, 3 do L.line(img, x, 29 + by, x + (x - 13) // 3, 34 + by, bottom2) end
    else
      L.rect(img, 9, 28 + by, 9, 5, bottom)
      L.hline(img, 9, 17, 28 + by, bottom2)
      L.vline(img, 13, 30 + by, 32 + by, bottom2)
    end
    -- head
    head(img, 13, 10 + by, girl)
    -- near arm
    arm(img, 14, 19 + by, -k.a * 6 // 10, shirt, P.skin1)
    L.outline(img, P.ink)
    return img
  end

  function M.studentFrames(girl)
    local frames = {}
    for f = 0, 5 do frames[#frames + 1] = M.student(f, girl) end
    return frames
  end

  -- The same Students seen from behind, walking up the driveway into the school (4 frames, 28x44):
  -- the backpack in the middle of the back, the arms swinging at the sides, one heel up.
  local BACK = {
    { up = 0, bob = 0 }, { up = 1, bob = -1 }, { up = 0, bob = 0 }, { up = 2, bob = -1 },
  }

  function M.studentBack(frame, girl)
    local k = BACK[frame + 1]
    local img = L.newImage(28, 44)
    local by = k.bob
    local bag, bag2 = girl and P.teal or P.red, girl and P.teal2 or P.red2
    -- legs, socks and shoes; the lifted foot shows its sole
    for i, lx in ipairs({ 10, 15 }) do
      local lift = (k.up == i) and 2 or 0
      local foot = 38 - lift
      L.rect(img, lx, 31 + by, 3, foot - 31 - by - 1, i == 1 and P.skin1 or P.skin2)
      L.rect(img, lx, foot - 1, 3, 1, P.white)
      L.rect(img, lx - 1, foot, 5, 2, lift > 0 and P.gray3 or P.ink)
    end
    -- arms at the sides, swinging against the legs
    for i, ax in ipairs({ 6, 20 }) do
      local swing = (k.up == 0) and 0 or ((k.up == i) and 1 or -1)
      L.rect(img, ax, 18 + by, 2, 4, i == 1 and P.white or P.gray1)
      L.rect(img, ax, 22 + by + swing, 2, 5, i == 1 and P.skin1 or P.skin2)
    end
    -- shirt, with the right side in shade
    L.rect(img, 8, 17 + by, 12, 13, P.white)
    L.vline(img, 18, 17 + by, 29 + by, P.gray1)
    L.vline(img, 19, 17 + by, 29 + by, P.gray1)
    -- skirt or shorts
    if girl then
      L.poly(img, { 8, 28 + by, 19, 28 + by, 21, 35 + by, 6, 35 + by }, P.navy)
      for x = 9, 18, 3 do L.line(img, x, 29 + by, x + (x - 13) // 3, 34 + by, P.navy2) end
    else
      L.rect(img, 8, 28 + by, 12, 5, P.navy)
      L.hline(img, 8, 19, 28 + by, P.navy2)
      L.vline(img, 13, 30 + by, 32 + by, P.navy2)
    end
    -- the backpack and its straps over the shoulders
    L.rect(img, 9, 18 + by, 10, 11, bag)
    L.rect(img, 9, 18 + by, 10, 2, bag2)
    L.vline(img, 9, 18 + by, 28 + by, bag2 == P.teal2 and P.teal3 or P.red3)
    L.vline(img, 18, 20 + by, 28 + by, bag2)
    L.rect(img, 11, 23 + by, 6, 4, bag2)
    L.hline(img, 11, 16, 23 + by, bag)
    L.px(img, 13, 25 + by, P.gray2)
    -- the back of the head: all hair, the ears showing at the sides
    L.circle(img, 13, 10 + by, 7, P.hair)
    L.px(img, 5, 11 + by, P.skin2); L.px(img, 21, 11 + by, P.skin2)
    L.hline(img, 10, 15, 4 + by, P.hair2); L.hline(img, 9, 12, 5 + by, P.hair2)
    if girl then
      -- the ponytail with its red tie, hanging down to the backpack
      L.rect(img, 12, 12 + by, 3, 2, P.red)
      L.rect(img, 12, 14 + by, 3, 5, P.hair)
      L.px(img, 13, 19 + by, P.hair)
    else
      L.hline(img, 11, 15, 17 + by, P.skin2)
    end
    L.outline(img, P.ink)
    return img
  end

  function M.studentBackFrames(girl)
    local frames = {}
    for f = 0, 3 do frames[#frames + 1] = M.studentBack(f, girl) end
    return frames
  end

  -- The school guard at the gate (30x48, front view): navy cap and trousers, a light blue shirt
  -- with a gold badge. Poses: "idle" (eyes open), "blink", "reach" (the left hand out to the gate)
  -- and "wave" 0 and 1 (the right hand up, waving the Students in).
  function M.guard(pose, frame)
    local img = L.newImage(30, 48)
    local shirt, shirt2, shirt3 = P.blue3, L.hex("78aee8"), P.blue
    -- trousers and shoes
    L.rect(img, 10, 33, 11, 11, P.navy)
    L.vline(img, 15, 35, 43, P.navy2)
    L.vline(img, 20, 34, 43, P.navy2)
    L.rect(img, 9, 44, 6, 2, P.ink); L.rect(img, 16, 44, 6, 2, P.ink)
    -- shirt, collar, badge, pocket and belt
    L.rect(img, 9, 19, 13, 14, shirt)
    L.vline(img, 9, 19, 32, P.white)
    L.vline(img, 20, 19, 32, shirt2); L.vline(img, 21, 19, 32, shirt2)
    L.px(img, 13, 19, P.white); L.px(img, 14, 20, P.white); L.px(img, 17, 19, P.white); L.px(img, 16, 20, P.white)
    L.vline(img, 15, 21, 31, shirt3)
    L.rect(img, 17, 23, 2, 2, P.yellow); L.px(img, 17, 23, P.yellow3)
    L.hline(img, 11, 13, 24, shirt2)
    L.rect(img, 9, 32, 13, 2, P.ink); L.rect(img, 14, 32, 3, 2, P.yellow2)
    -- arms: the near-side sleeves are lit, the far ones shaded
    local function hangingArm(x, light)
      L.rect(img, x, 19, 3, 6, light and shirt or shirt2)
      L.rect(img, x, 25, 3, 6, light and P.skin1 or P.skin2)
    end
    if pose == "reach" then
      L.line(img, 8, 21, 2, 23, shirt, 3)
      L.rect(img, 0, 22, 3, 3, P.skin1)
    else
      hangingArm(6, true)
    end
    if pose == "wave" then
      local hx = frame == 0 and 26 or 28
      L.line(img, 22, 21, 25, 14, shirt2, 3)
      L.line(img, 25, 14, hx, 6, P.skin2, 2)
      L.rect(img, hx - 1, 3, 3, 4, P.skin1)
    else
      hangingArm(22, false)
    end
    -- neck and head
    L.rect(img, 13, 17, 5, 2, P.skin2)
    L.circle(img, 15, 12, 6, P.skin1)
    L.px(img, 8, 12, P.skin2); L.px(img, 22, 12, P.skin2)
    L.hline(img, 10, 20, 8, P.hair); L.px(img, 9, 9, P.hair); L.px(img, 21, 9, P.hair)
    if pose == "blink" then
      L.hline(img, 12, 13, 12, P.ink); L.hline(img, 17, 18, 12, P.ink)
    else
      L.vline(img, 13, 11, 12, P.ink); L.vline(img, 17, 11, 12, P.ink)
    end
    L.hline(img, 14, 16, 15, P.skin3)
    -- the cap: navy crown, a gold badge, a dark visor
    L.ellipse(img, 15, 5, 7, 4, P.navy)
    L.rect(img, 8, 5, 15, 3, P.navy)
    L.hline(img, 10, 18, 2, L.hex("5a68b0"))
    L.rect(img, 14, 4, 2, 2, P.yellow)
    L.hline(img, 8, 23, 8, P.navy2)
    L.outline(img, P.ink)
    return img
  end

  return M
end
