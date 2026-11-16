-- School courtyard main menu background, flag, clouds and birds.
-- Canvas is 480x270 art pixels (1920x1080 at 4x).
-- Anchor constants here are mirrored in Assets/Scripts/UI/MenuArtLayout.cs.

return function(L)
  local P = L.P
  local M = {}

  M.W, M.H = 480, 270
  M.POLE_X = 200          -- flagpole left column
  M.FLAG_X, M.FLAG_Y = 202, 40 -- top-left of flag sprite

  local function window(img, x, y, w, h)
    L.rect(img, x - 1, y - 1, w + 2, h + 2, P.teal2)
    L.vgrad(img, x, y, w, h, { P.glass2, P.glass, P.glass, P.glass3 })
    -- diagonal shine
    for k = 0, math.min(w, h) - 4 do
      L.px(img, x + 2 + k, y + h - 3 - k, P.glass2)
      if k % 3 == 0 then L.px(img, x + 4 + k, y + h - 3 - k, P.glass2) end
    end
    L.vline(img, x + w // 2, y, y + h - 1, P.teal2)
    L.hline(img, x, x + w - 1, y + h // 2, P.teal2)
    L.hline(img, x - 2, x + w + 1, y + h + 1, P.wall3) -- sill
  end

  local function bush(img, cx, cy, rx, ry)
    L.ellipse(img, cx, cy + 1, rx, ry, P.green3)
    L.ellipse(img, cx, cy, rx, ry - 1, P.green2)
    L.ellipse(img, cx - 2, cy - 2, rx - 3, ry - 3, P.green)
    for _ = 1, rx do
      local fx = cx + L.randi(-rx + 2, rx - 2)
      local fy = cy + L.randi(-ry + 2, ry - 2)
      local col = ({ P.pink, P.yellow, P.white, P.red3 })[L.randi(1, 4)]
      L.px(img, fx, fy, col)
    end
  end

  local function treeCanopy(img, blobs)
    for _, b in ipairs(blobs) do L.circle(img, b[1] + 2, b[2] + 4, b[3], P.green3) end
    for _, b in ipairs(blobs) do L.circle(img, b[1], b[2], b[3], P.green2) end
    for _, b in ipairs(blobs) do L.circle(img, b[1] - 4, b[2] - 5, math.floor(b[3] * 0.6), P.green) end
    for _, b in ipairs(blobs) do
      for _ = 1, b[3] * 3 do
        local a = L.rand() * math.pi * 2
        local r = L.rand() * b[3]
        local x, y = b[1] + math.cos(a) * r, b[2] + math.sin(a) * r
        local under = L.get(img, math.floor(x), math.floor(y))
        if under == P.green then L.px(img, x, y, P.green4)
        elseif under == P.green2 then L.px(img, x, y, P.green3) end
      end
    end
  end

  function M.background()
    L.seed(42)
    local img = L.newImage(M.W, M.H)

    -- Sky
    L.vgrad(img, 0, 0, M.W, 176, { P.sky1, P.sky2, P.sky3, P.sky4, P.sky5 })

    -- Sun with soft rays
    local sx, sy = 440, 34
    for a = 0, 15 do
      local ang = a * math.pi / 8
      local len = (a % 2 == 0) and 27 or 23
      for r = 20, len do L.px(img, sx + math.cos(ang) * r, sy + math.sin(ang) * r, P.yellow3) end
    end
    L.circle(img, sx, sy, 17, P.yellow3)
    L.circle(img, sx, sy, 15, P.yellow)
    L.circle(img, sx - 4, sy - 4, 6, P.yellow3)

    -- Distant range
    for x = 0, M.W - 1 do
      local ridge = math.floor(126 + 7 * math.sin(x / 41) + 4 * math.sin(x / 13 + 1))
      L.vline(img, x, ridge, 176, P.mount3)
    end
    -- Mount Matutum style cone behind the school
    for x = 0, M.W - 1 do
      local ridge = math.floor(62 + math.abs(x - 372) * 0.44 + math.sin(x / 5) * 1.2)
      if ridge < 176 then
        L.vline(img, x, ridge, 176, x > 372 and P.mount2 or P.mount)
        if x <= 372 and (x % 7 == 0) then L.vline(img, x, ridge + 2, ridge + 6, P.mount3) end
      end
    end

    -- Rolling hills
    for x = 0, M.W - 1 do
      local h1 = math.floor(150 + 5 * math.sin(x / 33 + 0.5) + 3 * math.sin(x / 11))
      L.vline(img, x, h1, 190, P.hill)
      local h2 = math.floor(163 + 4 * math.sin(x / 27 + 2))
      L.vline(img, x, h2, 190, P.hill2)
    end
    for _ = 1, 26 do
      local tx = L.randi(0, M.W)
      local ty = math.floor(150 + 5 * math.sin(tx / 33 + 0.5) + 3 * math.sin(tx / 11))
      L.circle(img, tx, ty, L.randi(2, 4), P.hill2)
    end

    -- Perimeter wall on the left side of the courtyard
    L.rect(img, 0, 170, 240, 18, P.wall)
    L.rect(img, 0, 168, 240, 3, P.concrete)
    L.hline(img, 0, 239, 168, P.white)
    L.rect(img, 0, 181, 240, 7, P.teal)
    L.hline(img, 0, 239, 181, P.teal3)
    for x = 0, 239, 32 do
      L.rect(img, x, 164, 6, 24, P.concrete)
      L.vline(img, x + 5, 164, 187, P.concrete3)
      L.hline(img, x, x + 5, 164, P.white)
    end

    -- Ground
    L.vgrad(img, 0, 186, M.W, 84, { P.grass, P.grass2, P.grass3 })

    -- Courtyard concrete lane where the students walk
    L.rect(img, 0, 206, M.W, 46, P.concrete)
    L.vgrad(img, 0, 238, M.W, 14, { P.concrete, P.concrete2 })
    L.hline(img, 0, M.W - 1, 206, P.white)
    L.hline(img, 0, M.W - 1, 251, P.concrete3)
    for x = -40, M.W + 40, 32 do
      L.line(img, x, 207, x + (x - 240) * 0.18, 250, P.concrete2)
    end
    L.hline(img, 0, M.W - 1, 228, P.concrete2)

    -- Hopscotch painted on the lane
    local hx, hy = 112, 212
    local boxes = { { 0, 12 }, { 12, 12 }, { 24, 6 }, { 24, 18 }, { 36, 12 }, { 48, 6 }, { 48, 18 }, { 60, 12 } }
    for _, b in ipairs(boxes) do
      local x, y = hx + b[1], hy + b[2]
      L.hline(img, x, x + 11, y, P.white); L.hline(img, x, x + 11, y + 11, P.white)
      L.vline(img, x, y, y + 11, P.white); L.vline(img, x + 11, y, y + 11, P.white)
    end

    -- School building
    local bx0, bx1 = 236, 472
    L.rect(img, bx0, 104, bx1 - bx0 + 1, 95, P.wall)
    L.rect(img, bx1 - 5, 104, 6, 95, P.wall2)
    L.rect(img, bx0, 104, bx1 - bx0 + 1, 3, P.wall2) -- eave shadow
    -- second floor windows
    for x = bx0 + 10, bx1 - 30, 30 do window(img, x, 112, 20, 18) end
    -- railing
    L.rect(img, bx0 - 4, 132, bx1 - bx0 + 9, 2, P.teal)
    L.hline(img, bx0 - 4, bx1 + 4, 132, P.teal3)
    for x = bx0 - 2, bx1 + 2, 5 do L.vline(img, x, 134, 145, P.teal2) end
    -- corridor slab
    L.rect(img, bx0 - 4, 146, bx1 - bx0 + 9, 6, P.teal)
    L.hline(img, bx0 - 4, bx1 + 4, 146, P.teal3)
    L.hline(img, bx0 - 4, bx1 + 4, 151, P.teal2)
    L.rect(img, bx0, 152, bx1 - bx0 + 1, 3, P.wall2)
    -- ground floor windows and door
    local doorX = 340
    for x = bx0 + 10, bx1 - 30, 30 do
      if x + 20 < doorX - 4 or x > doorX + 34 then window(img, x, 162, 20, 20) end
    end
    L.rect(img, doorX - 2, 158, 34, 41, P.teal2)
    L.rect(img, doorX, 160, 14, 38, P.brown)
    L.rect(img, doorX + 16, 160, 14, 38, P.brown)
    L.vline(img, doorX + 15, 160, 197, P.brown2)
    for _, dx in ipairs({ doorX, doorX + 16 }) do
      L.rect(img, dx + 2, 163, 10, 14, P.brown3)
      L.rect(img, dx + 3, 164, 8, 12, P.brown)
      L.rect(img, dx + 2, 181, 10, 14, P.brown3)
      L.rect(img, dx + 3, 182, 8, 12, P.brown)
    end
    L.px(img, doorX + 12, 180, P.yellow); L.px(img, doorX + 18, 180, P.yellow)
    -- foundation and steps
    L.rect(img, bx0 - 2, 196, bx1 - bx0 + 5, 4, P.concrete3)
    L.rect(img, doorX - 6, 198, 42, 4, P.concrete)
    L.hline(img, doorX - 6, doorX + 35, 198, P.white)
    L.rect(img, doorX - 10, 202, 50, 4, P.concrete2)

    -- Roof
    local roof = { bx0 - 8, 105, bx1 + 8, 105, bx1 - 14, 82, bx0 + 14, 82 }
    L.poly(img, roof, P.roof)
    for x = bx0 - 8, bx1 + 8, 4 do
      for y = 82, 104 do
        if L.get(img, x, y) == P.roof then L.px(img, x, y, P.roof2) end
      end
    end
    L.line(img, bx0 + 14, 82, bx1 - 14, 82, P.roof3)
    L.line(img, bx0 - 8, 105, bx1 + 8, 105, P.roof2)
    L.line(img, bx0 - 8, 104, bx1 + 8, 104, P.roof2)
    L.line(img, bx0 + 14, 81, bx1 - 14, 81, P.ink2)
    L.line(img, bx0 - 9, 105, bx0 + 13, 81, P.ink2)
    L.line(img, bx1 + 9, 105, bx1 - 13, 81, P.ink2)

    -- School sign
    local sign = "BALUAN ELEMENTARY SCHOOL"
    local tw = L.F.width(sign, 1)
    local sx0 = math.floor((bx0 + bx1) / 2 - tw / 2) - 5
    L.rect(img, sx0, 86, tw + 10, 13, P.ink2)
    L.rect(img, sx0 + 1, 87, tw + 8, 11, P.white)
    L.rect(img, sx0 + 2, 88, tw + 6, 9, P.green3)
    L.text(img, sx0 + 5, 89, sign, P.white, 1)

    -- Bushes along the building
    for x = bx0 + 6, bx1 - 4, 16 do
      if x < doorX - 14 or x > doorX + 44 then bush(img, x, 196, 9, 6) end
    end

    -- Flagpole
    local px = M.POLE_X
    L.rect(img, px - 12, 200, 26, 6, P.concrete)
    L.hline(img, px - 12, px + 13, 200, P.white)
    L.rect(img, px - 12, 205, 26, 1, P.concrete3)
    L.rect(img, px - 8, 196, 18, 4, P.concrete)
    L.hline(img, px - 8, px + 9, 196, P.white)
    L.vline(img, px, 40, 199, P.metal)
    L.vline(img, px + 1, 40, 199, P.metal2)
    L.circle(img, px + 0.5, 37, 2, P.yellow)
    L.px(img, px, 36, P.yellow3)

    -- Tree on the left edge
    L.rect(img, 12, 120, 14, 86, P.brown)
    L.rect(img, 20, 120, 6, 86, P.brown2)
    L.vline(img, 14, 124, 200, P.brown3)
    L.poly(img, { 4, 206, 34, 206, 26, 196, 12, 196 }, P.brown)
    L.line(img, 22, 130, 46, 104, P.brown, 3)
    L.line(img, 16, 136, -2, 112, P.brown, 3)
    treeCanopy(img, { { -6, 92, 30 }, { 26, 74, 26 }, { 50, 96, 20 }, { 6, 118, 22 }, { 38, 116, 16 } })

    -- Evacuation area sign
    L.rect(img, 79, 182, 2, 24, P.brown2)
    L.rect(img, 68, 166, 26, 17, P.ink2)
    L.rect(img, 69, 167, 24, 15, P.green2)
    L.map(img, 71, 169, {
      "...##.......",
      "...##.......",
      ".#####......",
      "#.###.#..#..",
      "..##...######",
      ".#..#....#..",
      "#....#......",
      "......#.....",
    }, { ["#"] = P.white })

    -- Foreground grass tufts and flowers
    for _ = 1, 90 do
      local x, y = L.randi(0, M.W), L.randi(254, 268)
      L.px(img, x, y, P.grass3); L.px(img, x + 1, y - 1, P.grass3); L.px(img, x + 2, y, P.grass3)
      L.px(img, x + 1, y - 2, P.green4)
    end
    for _ = 1, 28 do
      local x, y = L.randi(0, M.W), L.randi(256, 268)
      local col = ({ P.pink, P.yellow, P.white })[L.randi(1, 3)]
      L.px(img, x, y - 1, col); L.px(img, x - 1, y, col); L.px(img, x + 1, y, col); L.px(img, x, y + 1, col)
      L.px(img, x, y, P.yellow2)
    end

    return img
  end

  -- Philippine flag, 4 waving frames, 44x30.
  function M.flagFrames()
    local frames = {}
    local FW, FH = 40, 20
    local function flat(u, v)
      -- white hoist triangle
      local half = FH / 2
      local tri = 17 * (1 - math.abs(v + 0.5 - half) / half)
      if u + 0.5 <= tri then
        local du, dv = u - 5, v - 9.5
        local d = math.sqrt(du * du + dv * dv)
        if d <= 2.4 then return P.flagSun end
        local ang = math.atan(dv, du)
        local k = ang / (math.pi / 4)
        if d <= 4.4 and math.abs(k - math.floor(k + 0.5)) < 0.18 then return P.flagSun end
        if (u == 1 and (v == 1 or v == 18)) or (u == 14 and (v == 9 or v == 10)) then return P.flagSun end
        return "w"
      end
      return v < half and "b" or "r"
    end
    for f = 0, 3 do
      local img = L.newImage(44, 30)
      local phase = f * math.pi / 2
      for u = 0, FW - 1 do
        local amp = 2.4 * (u / (FW - 1)) ^ 0.8
        local w = u / 6.5 - phase
        local dy = math.floor(amp * math.sin(w) + 0.5)
        local dark = math.cos(w) < -0.55 and u > 3
        local light = math.cos(w) > 0.75 and u > 3
        for v = 0, FH - 1 do
          local c = flat(u, v)
          if c == "w" then c = dark and P.flagWhite2 or P.flagWhite
          elseif c == "b" then c = dark and P.flagBlue2 or P.flagBlue
          elseif c == "r" then c = dark and P.flagRed2 or P.flagRed end
          if light and (c == P.flagBlue or c == P.flagRed) and v % 2 == 0 then
            c = (c == P.flagBlue) and L.hex("1a52c0") or L.hex("e02a3c")
          end
          L.px(img, 1 + u, 4 + v + dy, c)
        end
      end
      L.outline(img, P.ink2)
      frames[#frames + 1] = img
    end
    return frames
  end

  local function cloud(w, h, blobs)
    local img = L.newImage(w, h)
    local shade = L.hex("d8ecf8")
    for _, b in ipairs(blobs) do L.circle(img, b[1], b[2], b[3], P.white) end
    for y = 0, h - 1 do
      for x = 0, w - 1 do
        if L.opaque(img, x, y) and y > h * 0.62 then L.px(img, x, y, shade) end
      end
    end
    return img
  end

  function M.clouds()
    return {
      big = cloud(72, 28, { { 14, 18, 9 }, { 28, 12, 11 }, { 44, 14, 10 }, { 58, 19, 8 }, { 36, 20, 7 }, { 22, 21, 6 } }),
      small = cloud(44, 18, { { 10, 11, 6 }, { 21, 8, 7 }, { 32, 11, 6 }, { 20, 12, 5 } }),
    }
  end

  function M.birdFrames()
    local a = L.newImage(9, 6)
    L.map(a, 0, 0, { "#.......#", ".#.....#.", "..#...#..", "...#.#...", "....#...." }, { ["#"] = P.ink2 })
    local b = L.newImage(9, 6)
    L.map(b, 0, 0, { ".........", ".........", "###...###", "...#.#...", "....#...." }, { ["#"] = P.ink2 })
    return { a, b }
  end

  return M
end
