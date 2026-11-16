-- Walking students (boy and girl in school uniform) and Bantay, the guide dog.

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

  -- Bantay: sitting dog with a yellow hard hat. 44x44.
  -- pose: "idle", "wag", "blink", "talk"
  function M.bantay(pose)
    local img = L.newImage(44, 44)
    local cx = 21
    -- tail
    if pose == "wag" then
      L.line(img, 30, 34, 38, 31, P.tan2, 3)
      L.circle(img, 38, 31, 2, P.tan)
    else
      L.line(img, 30, 34, 36, 25, P.tan2, 3)
      L.circle(img, 36, 25, 2, P.tan)
    end
    -- body
    L.ellipse(img, cx, 31, 11, 10, P.tan)
    L.ellipse(img, cx + 4, 33, 6, 7, P.tan2)
    L.ellipse(img, cx, 32, 5, 7, P.cream)
    -- paws
    L.ellipse(img, cx - 5, 40, 3, 2, P.cream)
    L.ellipse(img, cx + 5, 40, 3, 2, P.cream)
    L.px(img, cx - 5, 40, P.paper3); L.px(img, cx + 5, 40, P.paper3)
    -- collar with tag
    L.rect(img, cx - 7, 23, 15, 2, P.red)
    L.circle(img, cx, 26, 1, P.yellow)
    -- ears
    L.poly(img, { cx - 9, 9, cx - 5, 9, cx - 6, 22, cx - 11, 19 }, P.brown)
    L.poly(img, { cx + 5, 9, cx + 9, 9, cx + 11, 19, cx + 6, 22 }, P.brown)
    -- head
    L.circle(img, cx, 15, 9, P.tan)
    L.ellipse(img, cx, 19, 5, 3, P.cream)
    -- eyes
    if pose == "blink" then
      L.hline(img, cx - 5, cx - 3, 14, P.ink)
      L.hline(img, cx + 3, cx + 5, 14, P.ink)
    else
      L.rect(img, cx - 5, 13, 2, 3, P.ink)
      L.rect(img, cx + 4, 13, 2, 3, P.ink)
      L.px(img, cx - 5, 13, P.white)
      L.px(img, cx + 4, 13, P.white)
    end
    L.px(img, cx - 7, 17, P.blush); L.px(img, cx + 7, 17, P.blush)
    -- nose and mouth
    L.rect(img, cx - 1, 17, 3, 2, P.ink)
    L.px(img, cx, 17, P.gray3)
    if pose == "talk" then
      L.rect(img, cx - 2, 20, 5, 3, P.ink)
      L.rect(img, cx - 1, 21, 3, 2, P.red3)
    else
      L.px(img, cx - 1, 20, P.ink); L.px(img, cx + 1, 20, P.ink)
      L.px(img, cx - 2, 19, P.ink); L.px(img, cx + 2, 19, P.ink)
    end
    -- hard hat
    for y = 0, 7 do
      for x = cx - 10, cx + 10 do
        local dx = (x - cx) / 9.5
        local dy = (y - 7) / 7.5
        if dx * dx + dy * dy <= 1 then L.px(img, x, y + 1, P.yellow) end
      end
    end
    L.hline(img, cx - 12, cx + 12, 8, P.yellow2)
    L.hline(img, cx - 12, cx + 12, 9, P.yellow2)
    L.rect(img, cx - 1, 1, 3, 7, P.yellow2)
    L.line(img, cx - 6, 4, cx - 4, 2, P.yellow3)
    L.px(img, cx - 7, 6, P.yellow3)
    L.outline(img, P.ink)
    return img
  end

  return M
end
