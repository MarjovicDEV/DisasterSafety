-- The food court during the shaking and the stairway out after it.
return function(L, ST)
  local M = {}
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- The v2 classroom and living room palettes keep the mall in the same world.
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    floor = { rgb(232, 216, 178), rgb(214, 198, 168), rgb(194, 176, 146) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    green = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    red = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) },
    navy = { K.m, K.N, K.n },
    paper = { K.W, K.w, K.v },
  }
  local TILE = rgb(224, 206, 170)
  local SIGN = rgb(52, 108, 82)
  local TEAL = rgb(64, 108, 116)
  local ROAD = rgb(152, 156, 168)
  local STONE = rgb(206, 185, 146)

  -- Positions are measured from the top left in art pixels.
  -- Act-out points locate the centre of the Student's feet.
  -- Hard 3: Run past the table in the foreground. Approach from the right
  -- for the Safe Choice, so the Student can hold the right front leg.
  -- Hard 4: The landing is at row 178. The stairs are the shared route for
  -- the Safe Choice, the crowd stop and the wall stop, including Try Again.
  M.LAYOUT = {
    mall_a = {
      floorY = 230, floorBackY = 208,
      start = { 230, 230 }, exit = { 29, 126 },
      tableFront = { 106, 192 }, tableEdge = { 204, 230 }, table = { 168, 230 },
      runBend = { 214, 247 }, runExit = { 66, 247 },
      shop = { 274, 121 }, shopWindow = { 307, 230 },
      elevator = { 387, 121 }, callButton = { 374, 206 }, elevatorStop = { 363, 230 },
      sign = { 118, 119 }, swayMs = 160,
    },
    mall_b = {
      floorY = 230, landingY = 178,
      start = { 129, 178 }, elevator = { 21, 99 },
      callButton = { 84, 154 }, elevatorStop = { 96, 178 },
      stairs = { { 155, 178 }, { 167, 184 }, { 179, 190 }, { 191, 196 },
        { 203, 203 }, { 215, 210 }, { 227, 217 }, { 239, 224 }, { 251, 230 } },
      exit = { 261, 144 }, crowd = { { 296, 221 }, { 317, 218 } },
      crowdStop = { 270, 221 }, exitNear = { 279, 235 }, exitFar = { 329, 235 },
      outside = { 352, 230 }, wall = { 333, 131 }, wallStop = { 357, 230 },
      parkingSign = { 452, 173 }, parking = { 430, 230 }, curbY = 195,
    },
  }
  local A, B = M.LAYOUT.mall_a, M.LAYOUT.mall_b

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
  end

  local function pane(img, x, y, w, h)
    box(img, x, y, w, h, R.metal)
    L.rect(img, x + 3, y + 3, w - 6, h - 6, R.glass[2])
    L.poly(img, { x + 3, y + 3, x + w - 4, y + 3, x + w - 4, y + 9,
      x + 3, y + h - 4 }, R.glass[1])
    L.hline(img, x + 3, x + w - 4, y + h - 4, R.glass[3])
  end

  local function wall(img, right, back)
    L.rect(img, 0, 0, right, back, R.wall[2])
    L.rect(img, 0, 32, right, 40, R.wall[1])
    L.rect(img, 0, 70, right, 6, R.wall[3])
    L.rect(img, 0, back - 12, right, 12, R.green[3])
    L.hline(img, 0, right - 1, back - 12, R.green[1])
    L.hline(img, 0, right - 1, back - 1, O)
  end

  local function tiledFloor(img, y, right)
    L.rect(img, 0, y, right, 270 - y, R.floor[2])
    L.poly(img, { 0, y, 117, y, 212, 270, 2, 270 }, R.floor[1])
    L.poly(img, { 125, y, 142, y, 267, 270, 223, 270 }, TILE)
    for _, row in ipairs({ y + 12, y + 31, y + 54 }) do
      if row < 270 then L.hline(img, 0, right - 1, row, R.floor[3]) end
    end
    for x = 20, right - 1, 57 do
      local endX = math.floor(x + (x - right / 2) * 0.46)
      local endY = 269
      if endX >= right then
        endY = y + math.floor((endY - y) * (right - 1 - x) / (endX - x))
        endX = right - 1
      end
      L.line(img, x, y, endX, endY, R.floor[3])
    end
  end

  local function planter(img, x, y)
    L.ellipse(img, x + 12, y + 4, 15, 3, R.floor[3])
    L.poly(img, { x + 3, y - 9, x + 20, y - 9, x + 17, y + 4, x + 7, y + 4 }, O)
    L.poly(img, { x + 4, y - 8, x + 18, y - 8, x + 16, y + 3, x + 8, y + 3 }, R.wood[2])
    L.line(img, x + 6, y - 7, x + 9, y + 3, R.wood[1])
    L.ellipse(img, x + 12, y - 19, 11, 10, O)
    L.ellipse(img, x + 12, y - 19, 10, 9, R.leaf[2])
    L.ellipse(img, x + 8, y - 22, 4, 4, R.leaf[1])
    L.ellipse(img, x + 16, y - 15, 5, 3, R.leaf[3])
  end

  local function elevator(img, point, button, floor)
    local x, y = table.unpack(point)
    local bottom = floor - 2
    box(img, x - 4, y - 4, 56, bottom - y + 7, R.wall)
    box(img, x, y, 48, bottom - y + 1, R.metal)
    L.rect(img, x + 3, y + 5, 42, bottom - y - 6, R.metal[3])
    for _, panel in ipairs({ x + 4, x + 25 }) do
      L.rect(img, panel, y + 6, 19, bottom - y - 7, R.metal[2])
      L.vline(img, panel, y + 6, bottom - 2, R.metal[1])
      L.vline(img, panel + 17, y + 7, bottom - 2, R.metal[3])
    end
    L.vline(img, x + 24, y + 5, bottom - 1, O)
    L.hline(img, x + 2, x + 45, bottom - 1, R.metal[1])
    box(img, x + 14, y - 16, 20, 10, R.navy)
    L.poly(img, { x + 19, y - 10, x + 21, y - 13, x + 23, y - 10 }, R.green[1])
    L.poly(img, { x + 25, y - 13, x + 29, y - 13, x + 27, y - 10 }, R.metal[1])
    local bx, by = table.unpack(button)
    box(img, bx - 4, by - 6, 9, 13, R.metal)
    L.circle(img, bx, by, 2, O)
    L.px(img, bx, by, R.green[1])
  end

  local function exitSymbol(img, x, y)
    box(img, x, y, 30, 13, { R.green[1], SIGN, R.green[3] })
    L.rect(img, x + 6, y + 3, 6, 7, K.W)
    L.rect(img, x + 7, y + 4, 3, 6, SIGN)
    L.hline(img, x + 14, x + 24, y + 6, K.W)
    L.line(img, x + 22, y + 4, x + 24, y + 6, K.W)
    L.line(img, x + 24, y + 6, x + 22, y + 8, K.W)
  end

  local function shopfront(img, x, y)
    box(img, x, y, 81, 86, R.metal)
    L.rect(img, x + 4, y + 15, 73, 63, R.glass[3])
    L.rect(img, x + 7, y + 18, 67, 58, R.glass[2])
    for _, row in ipairs({ y + 40, y + 61, y + 75 }) do
      L.rect(img, x + 7, row, 67, 2, TEAL)
    end
    for i = 0, 4 do
      local column = x + 10 + i * 13
      box(img, column, y + 29 + i % 2, 8, 11 - i % 2, i % 2 == 0 and R.red or R.wood)
      box(img, column + 2, y + 49, 7, 12, i % 2 == 0 and R.green or R.paper)
      L.rect(img, column + 1, y + 69, 9, 6, i % 2 == 0 and R.navy[1] or R.wood[3])
    end
    L.line(img, x + 7, y + 46, x + 30, y + 19, R.glass[1], 2)
    L.line(img, x + 44, y + 64, x + 73, y + 30, R.glass[1], 2)
    L.vline(img, x + 39, y + 15, y + 77, O)
    L.vline(img, x + 40, y + 16, y + 77, R.metal[1])
    box(img, x - 2, y - 3, 85, 15, { R.green[1], TEAL, R.green[3] })
    L.ellipse(img, x + 40, y + 4, 10, 3, R.glass[1])
    L.px(img, x + 37, y + 3, K.W)
    L.px(img, x + 43, y + 3, K.W)
    L.hline(img, x + 39, x + 41, y + 5, O)
    L.rect(img, x + 2, y + 80, 77, 3, R.metal[1])
  end

  function M.foodtableFront()
    local img = L.newImage(88, 39)
    L.rect(img, 1, 1, 86, 9, R.wood[2])
    L.hline(img, 1, 86, 1, R.wood[1])
    L.hline(img, 1, 86, 9, R.wood[3])
    for _, x in ipairs({ 6, 76 }) do
      L.rect(img, x, 9, 6, 29, R.metal[2])
      L.vline(img, x, 10, 37, R.metal[1])
      L.vline(img, x + 5, 10, 37, R.metal[3])
    end
    L.outline(img, O)
    return img
  end

  local function stool(img, x, y)
    L.ellipse(img, x, y + 25, 11, 3, R.floor[3])
    for _, dx in ipairs({ -7, 5 }) do
      box(img, x + dx, y + 3, 4, 22, R.metal)
    end
    L.ellipse(img, x, y, 11, 5, O)
    L.ellipse(img, x, y - 1, 10, 3, R.green[2])
    L.hline(img, x - 6, x + 5, y - 2, R.green[1])
    L.hline(img, x - 7, x + 7, y + 2, R.green[3])
  end

  local function foodtable(img)
    local x, y = table.unpack(A.tableFront)
    stool(img, x - 12, y - 4)
    stool(img, x + 94, y - 9)
    L.ellipse(img, x + 44, y + 36, 44, 5, R.floor[3])
    local rearLeg = { R.metal[2], R.metal[3], R.metal[3] }
    box(img, x + 19, y - 7, 7, 30, rearLeg)
    box(img, x + 63, y - 7, 7, 30, rearLeg)
    L.poly(img, { x, y, x + 12, y - 13, x + 75, y - 13, x + 87, y, x + 87, y + 1, x, y + 1 }, O)
    L.poly(img, { x + 2, y, x + 13, y - 12, x + 74, y - 12, x + 85, y, x + 85, y + 1, x + 2, y + 1 }, R.wood[1])
    L.hline(img, x + 14, x + 72, y - 10, R.wall[1])
    L.hline(img, x + 21, x + 76, y - 6, R.wood[2])
    L.blit(img, M.foodtableFront(), x, y)
  end

  function M.mallSignFrames()
    local frames = {}
    for _, shift in ipairs({ 0, 2, 0, -2 }) do
      local img = L.newImage(55, 29)
      local x = 3 + shift
      L.line(img, 13, 0, x + 10, 12, O)
      L.line(img, 42, 0, x + 39, 12, O)
      box(img, x, 10, 49, 18, { R.green[1], TEAL, R.green[3] })
      -- A fork and a cup identify the food court without adding a line of text.
      for dx = 12, 18, 3 do L.vline(img, x + dx, 14, 18, K.W) end
      L.hline(img, x + 12, x + 18, 18, K.W)
      L.vline(img, x + 15, 19, 24, K.W)
      L.poly(img, { x + 27, 16, x + 36, 16, x + 34, 24, x + 29, 24 }, K.W)
      L.hline(img, x + 26, x + 37, 16, R.glass[1])
      L.line(img, x + 32, 16, x + 34, 13, R.glass[1])
      frames[#frames + 1] = img
    end
    return frames
  end

  function M.backgroundA()
    local img = L.newImage(480, 270)
    wall(img, 480, A.floorBackY)
    for _, x in ipairs({ 0, 109, 231, 355 }) do
      L.rect(img, x, 0, 9, A.floorBackY, R.wall[3])
      L.vline(img, x, 0, A.floorBackY - 1, R.wall[1])
    end
    for _, x in ipairs({ 21, 131, 251, 372 }) do
      pane(img, x, 79, 90, 31)
      L.vline(img, x + 45, 82, 106, R.metal[3])
    end
    L.rect(img, 111, 114, 66, 5, R.wall[3])
    L.hline(img, 111, 176, 114, R.wall[1])
    tiledFloor(img, A.floorBackY, 480)
    local x, y = table.unpack(A.exit)
    box(img, x, y, 51, 84, R.metal)
    L.rect(img, x + 4, y + 4, 43, 78, TEAL)
    L.poly(img, { x + 4, y + 5, x + 39, y + 8, x + 39, y + 73, x + 4, y + 81 }, R.green[2])
    L.poly(img, { x + 8, y + 11, x + 35, y + 14, x + 35, y + 59, x + 8, y + 62 }, R.glass[2])
    L.line(img, x + 8, y + 30, x + 25, y + 13, R.glass[1])
    L.hline(img, x + 29, x + 37, y + 68, R.paper[1])
    exitSymbol(img, x + 10, y - 15)
    planter(img, 4, 209)
    shopfront(img, table.unpack(A.shop))
    elevator(img, A.elevator, A.callButton, 226)
    planter(img, 449, 206)
    foodtable(img)
    return img
  end

  -- C11: a low curb closes the far edge of the lot, and the painted parking lines are two art
  -- pixels thick, so the Safe ending at (430, 230) reads as open parking. The Student stands in an
  -- empty stall of the front row, beside the assembly sign.
  local function parkingLot(img)
    L.rect(img, 0, 0, 480, 270, R.glass[1])
    L.poly(img, { 310, 177, 344, 160, 382, 164, 408, 151, 435, 154,
      460, 143, 479, 146, 479, 205, 310, 205 }, R.glass[2])
    L.poly(img, { 343, 187, 363, 180, 387, 183, 404, 173, 431, 177,
      447, 172, 479, 178, 479, 197, 343, 197 }, R.green[2])
    L.rect(img, 0, 194, 480, 76, ROAD)
    -- The curb: an outline, a lit top face and a front face, 7 art pixels high in all.
    L.hline(img, 374, 479, B.curbY - 6, O)
    L.rect(img, 374, B.curbY - 5, 106, 2, R.floor[1])
    L.rect(img, 374, B.curbY - 3, 106, 3, STONE)
    L.hline(img, 374, 479, B.curbY, O)
    -- The dividers lean away from a vanishing point at (240, 0), like the pavement edge.
    local function divider(x, top, bottom)
      L.line(img, x, top, x + math.floor((x - 240) * (bottom - top) / top + 0.5), bottom, K.W, 2)
    end
    for _, x in ipairs({ 384, 412, 440 }) do divider(x, B.curbY + 2, 211) end
    L.rect(img, 380, 213, 100, 2, K.W)
    -- The Safe ending stands in the empty front stall between these two dividers.
    for _, x in ipairs({ 398, 452 }) do divider(x, 217, 244) end
    L.rect(img, 404, 246, 76, 2, K.W)
    -- A green painted box inside that stall marks where people gather under the assembly-area sign.
    L.rect(img, 409, 220, 45, 2, SIGN)
    L.rect(img, 412, 241, 56, 2, SIGN)
    L.line(img, 409, 220, 412, 241, SIGN, 2)
    L.line(img, 452, 220, 466, 241, SIGN, 2)
    L.poly(img, { 329, 218, 374, 216, 407, 269, 329, 269 }, R.floor[2])
    L.line(img, 374, 216, 407, 269, R.floor[3])
    L.line(img, 376, 216, 409, 269, R.paper[2])
  end

  -- C11: the outer wall from (333, 0) is the mall's exterior. Its corner at x 373 runs from the
  -- cornice down to a stone plinth, so the wall stop at (357, 230) reads as beside the building.
  local function exteriorCorner(img)
    L.rect(img, 333, 0, 41, 216, R.wall[3])
    L.vline(img, 332, 0, 143, O)
    L.vline(img, 372, 9, 207, R.wall[1])
    L.vline(img, 373, 9, 208, O)
    L.rect(img, 333, 0, 44, 7, R.wall[1])
    L.hline(img, 333, 376, 7, R.wall[2])
    L.hline(img, 333, 377, 8, O)
    L.vline(img, 377, 0, 8, O)
    L.hline(img, 333, 375, 208, O)
    L.rect(img, 333, 209, 42, 7, STONE)
    L.hline(img, 333, 374, 209, R.floor[1])
    L.vline(img, 375, 208, 215, O)
  end

  local function stairs(img)
    local treads = B.stairs
    local topX, topY = table.unpack(treads[1])
    L.poly(img, { 0, topY - 19, topX, topY - 19, 251, 211,
      251, 239, 0, 239 }, STONE)
    L.poly(img, { 0, 188, topX, 188, 248, 237, 0, 199 }, R.floor[3])
    L.hline(img, 0, 249, 238, R.floor[3])
    L.line(img, 0, topY - 51, topX, topY - 51, TEAL, 3)
    L.line(img, topX, topY - 51, 251, 179, TEAL, 3)
    for index, point in ipairs(treads) do
      local x, y = table.unpack(point)
      L.vline(img, x, y - 50, y - 16, TEAL)
      if index < #treads then
        local nextX, nextY = table.unpack(treads[index + 1])
        L.poly(img, { x, y - 17, nextX, y - 17, nextX, y + 7, x, y + 7 }, R.floor[1])
        L.hline(img, x, nextX - 1, y - 16, R.wall[1])
        L.hline(img, x, nextX - 1, y + 7, R.floor[3])
        L.vline(img, nextX - 1, y - 17, y + 7, R.floor[2])
        L.rect(img, x, y + 8, nextX - x, nextY - y, STONE)
        L.hline(img, x, nextX - 1, y + 8, O)
        L.hline(img, x, nextX - 1, y + 9, R.floor[3])
      end
    end
    L.rect(img, 0, topY - 18, topX, 26, R.floor[1])
    L.hline(img, 0, topX - 1, topY + 7, R.floor[3])
    L.rect(img, 0, topY + 8, topX, 11, STONE)
    L.hline(img, 0, topX - 1, topY + 8, O)
    -- The rail is at the back of the landing, behind the walking lane.
    for _, x in ipairs({ 4, 91, 122, 154 }) do
      L.vline(img, x, topY - 50, topY - 18, TEAL)
      L.vline(img, x + 1, topY - 50, topY - 18, R.green[1])
    end
    L.hline(img, 91, 155, topY - 52, O)
    L.hline(img, 91, 155, topY - 51, R.metal[1])
    L.line(img, 155, topY - 51, 251, 179, R.metal[1])
  end

  local function person(img, x, feet, shirt, skin)
    L.ellipse(img, x, feet, 8, 2, R.floor[3])
    L.rect(img, x - 4, feet - 14, 8, 13, O)
    L.rect(img, x - 3, feet - 14, 3, 12, R.navy[1])
    L.rect(img, x + 1, feet - 14, 2, 12, R.navy[2])
    L.hline(img, x - 5, x - 1, feet - 1, O)
    L.hline(img, x + 1, x + 6, feet - 1, O)
    box(img, x - 6, feet - 30, 13, 17, shirt)
    L.line(img, x - 5, feet - 26, x - 6, feet - 16, skin)
    L.line(img, x + 6, feet - 26, x + 7, feet - 16, skin)
    L.ellipse(img, x, feet - 36, 6, 8, O)
    L.ellipse(img, x + 1, feet - 35, 4, 6, skin)
    L.poly(img, { x - 5, feet - 38, x - 2, feet - 43, x + 3, feet - 42,
      x + 5, feet - 38, x + 1, feet - 38, x - 2, feet - 34 }, O)
    L.px(img, x + 3, feet - 36, O)
  end

  local function parkingSign(img, x, y)
    L.ellipse(img, x + 12, y + 50, 8, 2, R.metal[3])
    box(img, x + 10, y + 19, 5, 31, R.metal)
    box(img, x, y, 25, 24, { R.green[1], SIGN, R.green[3] })
    L.rect(img, x + 2, y + 2, 21, 20, K.W)
    L.rect(img, x + 3, y + 3, 19, 18, SIGN)
    -- Inward arrows surround three people to mark the assembly area.
    for _, dx in ipairs({ 8, 12, 16 }) do
      L.rect(img, x + dx - 1, y + 10, 2, 2, K.W)
      L.rect(img, x + dx - 1, y + 13, 2, 3, K.W)
    end
    for _, arrow in ipairs({ { 5, 5, 8, 8 }, { 19, 5, 16, 8 },
      { 5, 19, 8, 16 }, { 19, 19, 16, 16 } }) do
      local ax, ay, bx, by = table.unpack(arrow)
      L.line(img, x + ax, y + ay, x + bx, y + by, K.W)
      L.hline(img, x + math.min(bx, (ax + 2 * bx) / 3), x + math.max(bx, (ax + 2 * bx) / 3), y + by, K.W)
      L.vline(img, x + bx, y + math.min(by, (ay + 2 * by) / 3), y + math.max(by, (ay + 2 * by) / 3), K.W)
    end
  end

  function M.backgroundB()
    local img = L.newImage(480, 270)
    parkingLot(img)
    wall(img, 374, 215)
    pane(img, 93, 82, 60, 36)
    pane(img, 157, 82, 69, 36)
    pane(img, 230, 82, 72, 36)
    tiledFloor(img, 215, 374)
    L.poly(img, { 328, 216, 374, 216, 407, 269, 328, 269 }, R.floor[2])
    L.hline(img, 329, 385, 229, R.floor[3])
    L.hline(img, 329, 392, 247, R.floor[3])
    L.vline(img, 328, 219, 269, R.floor[3])
    exteriorCorner(img)
    local x, y = table.unpack(B.exit)
    box(img, x, y, 70, 77, R.metal)
    -- The right side is open to the daylight and the exterior pavement.
    L.rect(img, x + 4, y + 5, 62, 71, R.glass[1])
    L.poly(img, { x + 4, y + 54, x + 43, y + 48, x + 65, y + 50,
      x + 65, y + 74, x + 4, y + 74 }, R.green[2])
    L.poly(img, { x + 4, y + 60, x + 29, y + 57, x + 65, y + 57,
      x + 65, y + 76, x + 4, y + 76 }, R.floor[1])
    L.rect(img, x + 4, y + 5, 23, 70, R.glass[2])
    L.line(img, x + 6, y + 36, x + 25, y + 13, R.glass[1])
    L.vline(img, x + 27, y + 5, y + 75, O)
    L.vline(img, x + 28, y + 5, y + 75, R.metal[1])
    L.hline(img, x + 7, x + 23, y + 53, R.metal[1])
    L.hline(img, x + 29, x + 65, y + 76, R.floor[1])
    L.hline(img, x + 29, x + 65, y + 77, R.floor[3])
    exitSymbol(img, x + 20, y - 15)
    stairs(img)
    elevator(img, B.elevator, B.callButton, B.landingY - 2)
    local wx, wy = table.unpack(B.wall)
    pane(img, wx + 6, wy + 27, 28, 38)
    box(img, wx + 4, wy, 32, 19, R.metal)
    L.rect(img, wx + 7, wy + 3, 25, 11, TEAL)
    L.hline(img, wx + 11, wx + 27, wy + 7, R.glass[1])
    L.hline(img, wx + 13, wx + 25, wy + 10, R.glass[2])
    person(img, B.crowd[1][1], B.crowd[1][2], R.green, K.S)
    person(img, B.crowd[2][1], B.crowd[2][2], R.red, K.t)
    parkingSign(img, table.unpack(B.parkingSign))
    return img
  end

  return M
end
