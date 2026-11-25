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

  return M
end
