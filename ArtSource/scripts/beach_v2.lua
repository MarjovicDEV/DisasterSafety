-- The v2 beach for Earthquake Normal 5 after the sea has pulled back.
return function(L, ST)
  local M = {}
  local P = L.P
  local O = ST.KEY.o
  local R = {
    dry = { P.wall, P.wall2, P.wall3 },
    wet = { P.wall2, P.wall3, P.rock },
    sea = { P.water3, P.water, P.water2 },
    grass = { P.green4, P.grass, P.grass3 },
    earth = { P.tan, P.brown3, P.brown },
    path = { P.concrete, P.concrete2, P.concrete3 },
    mat = { P.teal3, P.teal, P.teal2 },
    sign = { P.hill, P.green2, P.green3 },
  }

  -- Points are in art pixels from the top left. The walk points mark the Student's feet.
  M.LAYOUT = {
    floorY = 230, horizonY = 119,
    water = { 0, 136 }, waterMs = 420,
    mat = { 175, 209 }, sign = { 405, 103 },
    start = { 260, 230 },
    sandBend = { 201, 247 }, watchBend = { 113, 241 },
    fishStop = { 144, 225 }, watchStop = { 72, 216 }, waitStop = { 200, 219 },
    pathFoot = { 295, 230 }, pathTurn = { 330, 209 }, pathRise = { 342, 178 },
    pathCrest = { 367, 158 }, highGround = { 386, 151 },
  }
  local A = M.LAYOUT

  -- The Fish and Watch Choices pass in front of the mat, then approach the wet sand.
  -- The watch route stays on sand and stops short of the water.
  -- The Safe Act-out follows the path up to the clear space to the left of the sign.
  -- Direct Try Again walks from the wet sand pass just in front of the mat.

  local shore = { 0, 224, 30, 218, 58, 204, 91, 195, 120, 186, 151, 172,
    181, 162, 222, 150, 255, 144, 302, 139, 345, 137, 386, 136, 427, 138, 480, 138 }
  local wetEdge = { 0, 269, 26, 268, 47, 256, 75, 246, 105, 236, 142, 231,
    162, 218, 190, 203, 222, 185, 249, 168, 285, 154, 323, 146, 354, 141, 391, 143, 427, 145, 480, 147 }

  local function coastBand(img, edge, color)
    local shape = { 0, A.horizonY, edge[#edge - 1], A.horizonY }
    for i = #edge - 1, 1, -2 do
      shape[#shape + 1] = edge[i]
      shape[#shape + 1] = edge[i + 1]
    end
    L.poly(img, shape, color)
  end

  local function sky(img)
    L.rect(img, 0, 0, 480, A.horizonY, P.sky4)
    for y = 56, A.horizonY - 1 do
      local fade = math.min(1, (y - 56) / 48)
      for x = 0, 479 do
        if L.bayer(x, y) < fade then L.px(img, x, y, P.sky5) end
      end
    end
    L.poly(img, { 6, 80, 17, 80, 23, 75, 34, 75, 39, 69, 53, 68,
      62, 74, 74, 74, 83, 82, 99, 82, 108, 87, 6, 87 }, P.white)
    L.hline(img, 10, 84, 87, P.sky4)
    L.poly(img, { 351, 68, 364, 68, 371, 62, 384, 62, 392, 67, 410, 67,
      421, 73, 438, 73, 449, 78, 351, 78 }, P.white)
    L.hline(img, 356, 411, 78, P.sky4)
    L.poly(img, { 0, 119, 19, 113, 31, 109, 44, 110, 56, 116, 68, 119 }, P.mount)
    L.poly(img, { 42, 119, 63, 114, 72, 114, 91, 119 }, P.mount2)
  end

  local function sandAndSea(img)
    L.rect(img, 0, A.horizonY, 480, 270 - A.horizonY, R.dry[2])
    L.poly(img, { 117, 267, 156, 248, 211, 216, 247, 207, 282, 211,
      320, 229, 357, 247, 407, 254, 479, 250, 479, 270, 110, 270 }, R.dry[1])
    L.poly(img, { 87, 270, 145, 262, 185, 263, 234, 271, 341, 264,
      388, 263, 479, 258, 479, 270 }, R.dry[2])
    coastBand(img, wetEdge, R.wet[2])
    for i, limit in ipairs({ 7, 5 }) do
      local edge = {}
      for j = 1, #wetEdge, 2 do
        edge[j], edge[j + 1] = wetEdge[j], wetEdge[j + 1] - limit
      end
      coastBand(img, edge, i == 1 and R.wet[1] or R.wet[2])
    end

    coastBand(img, shore, R.sea[2])
    L.hline(img, 0, 479, A.horizonY, R.sea[3])
    for _, ripple in ipairs({
      { 12, 125, 47 }, { 93, 127, 125 }, { 148, 124, 176 },
      { 209, 130, 243 }, { 252, 133, 281 }, { 27, 140, 65 },
      { 87, 151, 135 }, { 4, 159, 40 }, { 49, 177, 79 },
      { 9, 188, 32 }, { 3, 209, 19 },
    }) do
      local x, y, last = table.unpack(ripple)
      L.hline(img, x, last, y, R.sea[1])
      L.hline(img, x + 3, last + 1, y + 1, P.sky1)
    end

    L.seed(909)
    for _ = 1, 570 do
      local x, y = L.randi(1, 478), L.randi(141, 269)
      local base = L.get(img, x, y)
      if base == R.dry[1] then
        L.px(img, x, y, R.dry[2])
      elseif base == R.dry[2] or base == R.wet[2] then
        L.px(img, x, y, base == R.dry[2] and R.dry[1] or R.wet[1])
      end
    end

    for _, pebble in ipairs({ { 32, 236 }, { 61, 228 }, { 86, 218 },
      { 126, 197 }, { 147, 197 }, { 163, 205 }, { 43, 247 }, { 101, 257 } }) do
      local x, y = table.unpack(pebble)
      L.ellipse(img, x, y, 3, 1, R.wet[3])
      L.hline(img, x - 2, x + 1, y - 1, P.rock3)
      L.px(img, x - 1, y - 1, P.cream)
    end
    for _, shell in ipairs({ { 55, 258 }, { 129, 248 }, { 154, 212 } }) do
      local x, y = table.unpack(shell)
      L.poly(img, { x, y, x + 1, y - 3, x + 4, y - 4, x + 6, y - 1, x + 4, y + 1 }, R.wet[3])
      L.hline(img, x + 1, x + 4, y - 1, P.cream)
      L.px(img, x + 2, y - 2, R.dry[2])
    end
  end

  local function hill(img)
    -- The lit grassy cap and exposed earth have the same contour along the crest.
    L.poly(img, { 258, 237, 278, 221, 304, 211, 314, 191, 327, 176, 341, 153,
      356, 143, 375, 136, 432, 136, 445, 138, 479, 141, 479, 270,
      439, 270, 402, 259, 366, 255, 339, 247, 303, 250, 279, 244 }, R.earth[3])
    L.poly(img, { 263, 237, 285, 224, 310, 214, 324, 194, 338, 178, 351, 157,
      369, 147, 430, 148, 455, 151, 479, 155, 479, 222, 444, 218,
      407, 223, 374, 233, 350, 233, 326, 243, 288, 242 }, R.earth[2])
    L.poly(img, { 306, 230, 323, 205, 344, 184, 365, 170, 392, 165, 422, 165,
      449, 171, 478, 169, 478, 181, 446, 187, 416, 183, 387, 193,
      361, 198, 342, 211, 324, 230 }, R.earth[1])
    L.poly(img, { 258, 237, 278, 221, 304, 211, 314, 191, 327, 176, 341, 153,
      356, 143, 375, 136, 432, 136, 445, 138, 479, 141, 479, 163,
      448, 160, 432, 162, 412, 157, 391, 163, 373, 158, 351, 172, 344, 188,
      330, 201, 316, 222, 291, 230, 275, 239 }, R.grass[3])
    L.poly(img, { 264, 235, 281, 222, 306, 212, 316, 191, 330, 176, 342, 155,
      357, 145, 376, 137, 432, 137, 450, 141, 479, 142, 479, 158,
      447, 154, 432, 156, 412, 153, 390, 158, 374, 154, 348, 170, 340, 185,
      327, 199, 314, 218, 291, 227, 273, 236 }, R.grass[2])
    L.line(img, 280, 224, 305, 213, R.grass[1])
    L.line(img, 307, 211, 318, 191, R.grass[1])
    L.line(img, 342, 156, 357, 146, R.grass[1])
    L.hline(img, 376, 429, 138, R.grass[1])

    for _, ledge in ipairs({
      { 348, 237, 371, 232 }, { 373, 232, 396, 226 }, { 412, 233, 442, 228 },
      { 449, 229, 479, 231 }, { 405, 251, 428, 249 }, { 442, 259, 478, 254 },
      { 374, 214, 400, 206 }, { 403, 205, 417, 204 }, { 446, 199, 478, 200 },
    }) do
      local x, y, right, below = table.unpack(ledge)
      L.line(img, x, y, right, below, R.earth[1])
      L.line(img, x + 3, y + 1, right, below + 1, R.earth[3])
    end
    for _, tuft in ipairs({ { 347, 167 }, { 319, 192 }, { 367, 146 },
      { 395, 143 }, { 438, 145 }, { 453, 149 }, { 470, 147 }, { 311, 219 } }) do
      local x, y = table.unpack(tuft)
      L.line(img, x, y, x - 2, y - 3, R.grass[3])
      L.line(img, x, y, x + 2, y - 2, R.grass[1])
    end
  end

  local function uphillPath(img)
    -- The Walk points follow the middle of this wide ribbon at each turn.
    L.poly(img, { 278, 234, 319, 203, 332, 173, 363, 152, 382, 143,
      398, 146, 400, 153, 388, 159, 370, 166, 352, 184, 341, 215,
      304, 238, 287, 239 }, R.path[3])
    L.poly(img, { 281, 234, 321, 205, 334, 175, 364, 154, 382, 145,
      398, 148, 398, 152, 387, 157, 369, 164, 350, 183, 339, 214,
      303, 236, 289, 237 }, R.path[2])
    L.line(img, 285, 233, 320, 207, R.path[1])
    L.line(img, 322, 205, 335, 177, R.path[1])
    L.line(img, 335, 177, 364, 155, R.path[1])
    L.line(img, 364, 155, 383, 146, R.path[1])
    L.seed(910)
    for _ = 1, 170 do
      local x, y = L.randi(283, 397), L.randi(146, 236)
      if L.get(img, x, y) == R.path[2] then L.px(img, x, y, R.path[1]) end
    end
  end

  local function beachMat(img)
    local x, y = table.unpack(A.mat)
    L.poly(img, { x + 8, y, x + 41, y, x + 48, y + 16, x, y + 16 }, O)
    L.poly(img, { x + 9, y + 1, x + 40, y + 1, x + 46, y + 15, x + 2, y + 15 }, R.mat[2])
    L.line(img, x + 9, y + 1, x + 2, y + 15, R.mat[1])
    L.line(img, x + 40, y + 2, x + 45, y + 14, R.mat[3])
    L.hline(img, x + 8, x + 41, y + 4, R.mat[1])
    L.hline(img, x + 5, x + 43, y + 11, R.mat[3])
    L.hline(img, x + 4, x + 44, y + 13, R.mat[1])
    for dx = 4, 44, 4 do
      L.px(img, x + dx, y + 17, R.mat[3])
    end
  end

  local function evacuationSign(img)
    local x, y = table.unpack(A.sign)
    L.ellipse(img, x + 16, y + 40, 17, 2, R.grass[3])
    for _, post in ipairs({ x + 4, x + 25 }) do
      L.rect(img, post, y + 23, 4, 17, O)
      L.vline(img, post + 1, y + 23, y + 38, R.earth[1])
      L.vline(img, post + 2, y + 23, y + 38, R.earth[2])
    end
    L.rect(img, x, y, 33, 26, O)
    L.rect(img, x + 1, y + 1, 31, 24, R.sign[1])
    L.rect(img, x + 2, y + 2, 29, 22, R.sign[2])
    L.hline(img, x + 2, x + 31, y + 24, R.sign[3])
    L.vline(img, x + 31, y + 2, y + 24, R.sign[3])

    -- The small board uses a wave and a figure running uphill.
    L.poly(img, { x + 4, y + 19, x + 4, y + 13, x + 7, y + 9, x + 11, y + 7,
      x + 15, y + 8, x + 16, y + 11, x + 13, y + 10, x + 10, y + 13,
      x + 10, y + 16, x + 14, y + 19 }, P.white)
    L.line(img, x + 15, y + 22, x + 29, y + 14, P.white)
    L.circle(img, x + 25, y + 6, 1, P.white)
    L.line(img, x + 23, y + 8, x + 21, y + 12, P.white, 2)
    L.line(img, x + 20, y + 9, x + 23, y + 9, P.white)
    L.line(img, x + 23, y + 9, x + 27, y + 10, P.white)
    L.line(img, x + 21, y + 12, x + 18, y + 16, P.white)
    L.line(img, x + 21, y + 12, x + 25, y + 12, P.white)
    L.px(img, x + 25, y + 14, P.white)
  end

  function M.background()
    local img = L.newImage(480, 270)
    sky(img)
    sandAndSea(img)
    hill(img)
    uphillPath(img)
    beachMat(img)
    evacuationSign(img)
    return img
  end

  local function waterFrame(shift)
    local img = L.newImage(306, 93)
    -- Thin foam traces the far edge. Both frames stay over the water.
    for i = 1, 18, 2 do
      local x1, y1 = shore[i], shore[i + 1] - A.water[2]
      local x2, y2 = shore[i + 2], shore[i + 3] - A.water[2]
      L.line(img, x1, y1 - 3 - shift, x2, y2 - 3 - shift, R.sea[1])
      L.line(img, x1, y1 - 2, x2, y2 - 2, P.sky1)
      local start = math.floor((x1 + x2) / 2) - 3
      local row = math.floor((y1 + y2) / 2) - 5 - shift
      L.hline(img, start + shift * 3, start + 5 + shift * 3, row, P.sky5)
    end
    return img
  end

  function M.waterFrames()
    return { waterFrame(0), waterFrame(1) }
  end

  return M
end
