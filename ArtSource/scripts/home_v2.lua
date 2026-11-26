-- The v2 living room for Earthquake Normal 2, Normal 3 and Hard 1.
-- Separate furniture and a shared layout let home_after reuse the intact room.
return function(L, ST)
  local M = {}
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- Light, base and shadow from the classroom and bedroom palettes.
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    lower = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    floor = { rgb(214, 198, 168), rgb(194, 176, 146), rgb(166, 148, 118) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    cork = { rgb(222, 178, 120), rgb(198, 150, 96), rgb(166, 120, 74) },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    red = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) },
    pot = { rgb(232, 140, 96), rgb(200, 104, 70), rgb(156, 72, 52) },
    navy = { K.m, K.N, K.n },
    paper = { K.W, K.w, K.v },
  }
  local CEIL = rgb(232, 216, 178)
  local SUN = rgb(252, 209, 22)

  -- Sprite and furniture positions are art pixels from the top left.
  M.LAYOUT = {
    floorY = 230, floorBackY = 208,
    door = { 18, 107 }, tv = { 87, 92 }, board = { 85, 149 },
    window = { 300, 68 }, lamp = { 384, 14 },
    tableFront = { 174, 188 }, sofa = { 308, 157 },
    cabinet = { 382, 105 }, boxes = { 426, 156 },
    bagOpen = { 302, 191 }, bagClosed = { 303, 191 },
    bagDoor = { 73, 191 }, bagHidden = { 437, 191 },
    toy = { 325, 203 }, water = { 341, 195 }, can = { 350, 202 },
    flashlight = { 360, 206 }, whistle = { 373, 207 }, snack = { 346, 170 },
    parkCard = { 213, 176 }, houseCard = { 232, 176 }, cardOnBoard = { 109, 172 },
    normal2 = { start = 281, mom = 158, pack = 308, drop = 108, tv = 190,
      toy = 322, boxes = 425 },
    normal3 = { start = 294, tableEdge = 282, hold = 240, run = 91, glass = 387, sofa = 307 },
    hard1 = { start = 154, mom = 290, park = 207, house = 226, board = 136, door = 91 },
    -- Coordinates for the same room after the earthquake.
    homeAfter = {
      innerDoor = { 429, 113 }, lamp = { 384, 14 },
      picture = { 208, 144 }, books = { 394, 217 }, chair = { 187, 248 },
      pen = { 245, 181 }, note = { 232, 180 },
      -- Actor points are the centre of the feet, measured from the top of the art.
      hard5 = {
        start = { 286, 230 }, mom = { 322, 233 }, crouch = { 284, 242 },
        pullUp = { 284, 234 }, aisle = { 269, 242 }, wait = { 284, 234 },
        doorStop = { 100, 232 }, call = { 56, 212 },
        dadOffscreen = { -24, 238 }, dadEntry = { 56, 249 }, dadAisle = { 265, 241 }, dadKneel = { 286, 244 },
      },
      hard6 = {
        start = { 286, 230 }, mom = { 38, 215 },
        aisle = { 282, 236 }, crossRoom = { 162, 236 }, bagPickUp = { 106, 225 },
        emptyStop = { 114, 232 }, carryingStop = { 91, 232 },
        runBend = { 363, 241 }, innerStop = { 441, 235 },
        studentThreshold = { 56, 212 }, momLead = { 32, 183 }, momThreshold = { 35, 178 },
        studentOutside = { 56, 206 }, momOutside = { 40, 171 },
      },
    },
  }
  local A = M.LAYOUT

  -- Act-out points are the centre of the Student's feet at y 230.
  -- Normal 2: Mom stands in the clear gap between the board and the table. The
  -- supplies end before the glass cabinet, and the snack rests on the sofa seat.
  -- Pack at 308. Carry left to 108, and place the closed bag at (73, 191)
  -- for Hard 6. Carry right to 425 for the boxes. Pick up the toy at 322 and turn
  -- left to return to 308. The walk toward the TV stops at 190, before Mom.
  -- Hard 1: Mom stands in the clear gap between the table and the sofa.
  -- Normal 3: approach at 282, crouch left to 240 and hold the right front leg.
  -- The run stops at 91, the cabinet walk at 387 and the sofa crouch at 307.
  -- Hard 1: pick up at 207 (park) or 226 (house), then reach left at 136.
  -- The Home reach-up pose pinches the park card at its lower-right edge.

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end

  local function floor(img)
    local w, h = img.width, img.height
    local light, base, shadow = table.unpack(R.floor)
    local y = A.floorBackY
    L.rect(img, 0, y, w, h - y, base)
    L.hline(img, 0, w - 1, y, shadow)
    L.poly(img, { 305, 209, 332, 209, 321, 237, 276, 237 }, light)
    L.poly(img, { 336, 209, 363, 209, 371, 237, 328, 237 }, light)
    for i = 1, 72 do
      local px = (97 * i + 19 * i * i) % (w - 8) + 4
      local py = (17 * i + 7 * i * i) % (h - 212) + 211
      L.px(img, px, py, img:getPixel(px, py) == light and base or (i % 3 == 0 and shadow or light))
    end
  end

  local function door(img, x, y)
    box(img, x, y, 52, 103, R.wood)
    box(img, x + 4, y + 4, 44, 97, R.cork)
    for _, inset in ipairs({ 9, 55 }) do
      L.rect(img, x + 10, y + inset, 32, 36, R.wood[3])
      L.rect(img, x + 11, y + inset + 1, 30, 34, R.wood[2])
      L.hline(img, x + 11, x + 40, y + inset + 34, R.cork[1])
      L.vline(img, x + 40, y + inset + 2, y + inset + 34, R.cork[1])
    end
    L.circle(img, x + 43, y + 52, 3, O)
    L.circle(img, x + 43, y + 52, 2, R.metal[2])
    L.px(img, x + 42, y + 51, R.metal[1])
    box(img, x - 2, y + 101, 56, 4, R.wood)
  end

  local function television(img, x, y)
    box(img, x, y, 56, 35, R.navy)
    L.rect(img, x + 3, y + 3, 50, 27, R.navy[3])
    L.poly(img, { x + 4, y + 4, x + 24, y + 4, x + 8, y + 28, x + 4, y + 28 }, R.navy[2])
    L.line(img, x + 6, y + 24, x + 19, y + 5, R.navy[1])
    L.px(img, x + 50, y + 32, R.red[2])
  end

  local function board(img, x, y)
    box(img, x, y, 58, 42, R.wood)
    L.rect(img, x + 3, y + 3, 52, 36, R.cork[2])
    L.hline(img, x + 4, x + 54, y + 3, R.cork[3])
    L.hline(img, x + 4, x + 54, y + 5, R.cork[1])
    local title = "Meet here"
    L.F.draw(title, x + (58 - L.F.width(title)) // 2, y + 8, 1,
      function(px, py) L.px(img, px, py, O) end)
    -- The board starts empty, so the Hard 1 card is the only thing pinned on it.
  end

  local function window(img, x, y)
    box(img, x, y, 68, 73, R.wood)
    L.rect(img, x + 4, y + 4, 60, 65, R.glass[1])
    L.poly(img, { x + 4, y + 57, x + 19, y + 47, x + 35, y + 57,
      x + 49, y + 50, x + 63, y + 55, x + 63, y + 68, x + 4, y + 68 }, R.leaf[2])
    for row = y + 10, y + 64, 9 do
      L.hline(img, x + 4, x + 63, row, R.glass[2])
      L.hline(img, x + 4, x + 63, row + 1, R.glass[3])
    end
    L.vline(img, x + 33, y + 4, y + 68, R.wood[3])
    L.vline(img, x + 34, y + 4, y + 68, R.wood[1])
    box(img, x - 3, y + 70, 74, 5, R.wood)
    for _, cx in ipairs({ x - 7, x + 61 }) do
      box(img, cx, y - 3, 14, 49, R.red)
      L.vline(img, cx + 5, y + 1, y + 43, R.red[3])
      L.vline(img, cx + 9, y + 1, y + 43, R.red[1])
      L.hline(img, cx + 2, cx + 11, y + 36, R.cork[1])
    end
    box(img, x - 9, y - 6, 86, 4, R.metal)
  end

  function M.tableFront()
    local img = L.newImage(96, 42)
    -- One continuous outline joins the front rail and both solid legs.
    L.rect(img, 1, 1, 94, 5, R.wood[2])
    L.rect(img, 6, 5, 7, 36, R.wood[2])
    L.rect(img, 83, 5, 7, 36, R.wood[2])
    L.hline(img, 1, 94, 1, R.wood[1])
    L.hline(img, 1, 94, 5, R.wood[3])
    for _, x in ipairs({ 6, 83 }) do
      L.vline(img, x, 6, 39, R.wood[1])
      L.vline(img, x + 6, 6, 40, R.wood[3])
      L.hline(img, x + 1, x + 6, 40, R.wood[3])
    end
    L.outline(img, O)
    return img
  end

  local function strongTable(img, x, y)
    local shade = { R.wood[2], R.wood[3], R.wood[3] }
    L.ellipse(img, x + 47, y + 38, 42, 4, R.floor[3])
    box(img, x + 15, y - 1, 7, 30, shade)
    box(img, x + 74, y - 1, 7, 30, shade)
    L.poly(img, { x, y, x + 11, y - 9, x + 84, y - 9, x + 95, y, x + 95, y + 1, x, y + 1 }, O)
    L.poly(img, { x + 2, y, x + 12, y - 8, x + 83, y - 8, x + 93, y, x + 93, y + 1, x + 2, y + 1 }, R.wood[1])
    L.hline(img, x + 14, x + 81, y - 6, R.cork[1])
    L.hline(img, x + 32, x + 54, y - 3, R.wood[2])
    L.blit(img, M.tableFront(), x, y)
  end

  local function sofa(img, x, y)
    L.ellipse(img, x + 31, y + 52, 35, 4, R.floor[3])
    box(img, x + 6, y + 43, 7, 10, R.wood)
    box(img, x + 54, y + 43, 7, 10, R.wood)
    L.poly(img, { x + 8, y + 28, x + 9, y + 6, x + 15, y + 1,
      x + 52, y + 1, x + 58, y + 6, x + 59, y + 28 }, O)
    L.poly(img, { x + 9, y + 28, x + 10, y + 7, x + 15, y + 2,
      x + 51, y + 2, x + 57, y + 7, x + 58, y + 28 }, R.lower[2])
    L.hline(img, x + 16, x + 50, y + 3, R.lower[1])
    L.line(img, x + 10, y + 10, x + 15, y + 3, R.lower[1])
    L.vline(img, x + 33, y + 7, y + 28, R.lower[3])
    box(img, x + 3, y + 27, 62, 21, R.lower)
    box(img, x + 10, y + 26, 24, 11, R.lower)
    box(img, x + 33, y + 26, 25, 11, R.lower)
    box(img, x, y + 23, 12, 21, R.lower)
    box(img, x + 55, y + 23, 12, 21, R.lower)
    -- A cushion on the left seat back.
    L.poly(img, { x + 14, y + 16, x + 24, y + 13, x + 29, y + 23, x + 18, y + 26 }, O)
    L.poly(img, { x + 15, y + 17, x + 24, y + 14, x + 28, y + 23, x + 18, y + 25 }, R.cork[2])
    L.line(img, x + 15, y + 17, x + 24, y + 14, R.cork[1])
    L.line(img, x + 18, y + 25, x + 28, y + 23, R.cork[3])
  end

  local function glassCabinet(img, x, y)
    box(img, x + 2, y + 2, 36, 107, R.wood)
    box(img, x, y, 40, 6, R.wood)
    L.rect(img, x + 5, y + 9, 30, 76, O)
    L.rect(img, x + 6, y + 10, 28, 74, R.glass[2])
    for shelf = 29, 77, 24 do
      L.hline(img, x + 6, x + 33, y + shelf + 5, R.glass[3])
      L.ellipse(img, x + 12, y + shelf - 3, 3, 6, R.paper[3])
      L.ellipse(img, x + 12, y + shelf - 4, 2, 4, R.paper[2])
      L.poly(img, { x + 24, y + shelf - 8, x + 31, y + shelf - 8,
        x + 29, y + shelf - 1, x + 26, y + shelf - 1 }, R.glass[1])
      L.vline(img, x + 28, y + shelf, y + shelf + 3, R.glass[3])
    end
    -- Clear diagonal reflections identify two intact glass doors.
    for _, dx in ipairs({ 0, 15 }) do
      L.line(img, x + 7 + dx, y + 28, x + 16 + dx, y + 15, R.paper[1])
      L.line(img, x + 8 + dx, y + 34, x + 14 + dx, y + 25, R.glass[1])
    end
    L.vline(img, x + 20, y + 9, y + 84, R.wood[3])
    L.vline(img, x + 18, y + 47, y + 52, O)
    L.vline(img, x + 23, y + 47, y + 52, O)
    box(img, x + 6, y + 88, 28, 14, R.cork)
    L.hline(img, x + 17, x + 23, y + 93, O)
  end

  local function storage(img, x, y)
    -- Low storage bench supports the boxes, with a full-height gap below.
    box(img, x + 1, y + 31, 5, 27, R.wood)
    box(img, x + 41, y + 31, 5, 27, R.wood)
    L.hline(img, x + 6, x + 40, y + 35, R.floor[3])
    box(img, x, y + 29, 47, 5, R.wood)
    box(img, x + 3, y + 10, 24, 19, R.cork)
    box(img, x + 27, y + 15, 17, 14, R.wood)
    box(img, x + 10, y, 15, 10, R.pot)
    L.vline(img, x + 15, y + 11, y + 18, R.paper[2])
    L.vline(img, x + 35, y + 16, y + 20, R.paper[2])
  end

  local function room(drawFrontDoor, drawRightSide, rightSide)
    local w, h = 480, 270
    local img = L.newImage(w, h)
    L.rect(img, 0, 0, w, 14, CEIL)
    L.hline(img, 0, w - 1, 14, O)
    L.rect(img, 0, 15, w, 188, R.wall[2])
    L.rect(img, 0, 15, w, 2, R.wall[1])
    L.rect(img, 0, 162, w, 41, R.wall[1])
    L.hline(img, 0, w - 1, 161, R.wall[3])
    L.rect(img, 0, 203, w, 4, R.wood[3])
    L.hline(img, 0, w - 1, 207, O)
    floor(img)
    drawFrontDoor(img, table.unpack(A.door))
    television(img, table.unpack(A.tv))
    board(img, table.unpack(A.board))
    window(img, table.unpack(A.window))
    strongTable(img, table.unpack(A.tableFront))
    sofa(img, table.unpack(A.sofa))
    glassCabinet(img, table.unpack(A.cabinet))
    drawRightSide(img, table.unpack(rightSide))
    return img
  end

  function M.background()
    return room(door, storage, A.boxes)
  end

  -- The open front door shows sunlit ground beyond the threshold.
  local function openFrontDoor(img, x, y)
    door(img, x, y)
    L.rect(img, x + 5, y + 6, 43, 94, R.glass[1])
    L.poly(img, { x + 5, y + 62, x + 21, y + 54, x + 34, y + 63,
      x + 47, y + 59, x + 47, y + 81, x + 5, y + 81 }, R.leaf[2])
    L.rect(img, x + 5, y + 76, 43, 24, R.leaf[1])
    L.poly(img, { x + 17, y + 62, x + 25, y + 62, x + 47, y + 100,
      x + 10, y + 100 }, R.floor[1])
    L.hline(img, x + 5, x + 47, y + 17, R.glass[2])
    L.vline(img, x + 47, y + 5, y + 100, R.wood[3])
    L.poly(img, { x + 4, y + 5, x + 16, y + 11, x + 16, y + 94,
      x + 4, y + 100 }, O)
    L.poly(img, { x + 5, y + 7, x + 15, y + 12, x + 15, y + 93,
      x + 5, y + 98 }, R.wood[2])
    L.line(img, x + 5, y + 7, x + 15, y + 12, R.wood[1])
    L.vline(img, x + 15, y + 13, y + 92, R.wood[3])
    L.px(img, x + 13, y + 54, R.metal[1])
  end

  -- The right opening leads to a dim interior with a toy on a low shelf.
  local function innerDoor(img, x, y)
    box(img, x, y, 46, 97, R.wood)
    L.rect(img, x + 5, y + 5, 36, 89, R.navy[3])
    L.rect(img, x + 11, y + 13, 24, 66, R.lower[3])
    L.poly(img, { x + 5, y + 94, x + 11, y + 78, x + 35, y + 78,
      x + 40, y + 94 }, R.floor[3])
    box(img, x + 18, y + 67, 16, 4, R.wood)
    L.poly(img, { x + 19, y + 64, x + 22, y + 64, x + 24, y + 61,
      x + 28, y + 61, x + 31, y + 64, x + 33, y + 64, x + 33, y + 66,
      x + 19, y + 66 }, R.red[2])
    L.px(img, x + 22, y + 67, O)
    L.px(img, x + 30, y + 67, O)
    box(img, x - 1, y + 95, 48, 4, R.wood)
  end

  local function tiltedPicture(img, x, y)
    L.poly(img, { x, y + 5, x + 29, y, x + 33, y + 20, x + 4, y + 25 }, O)
    L.poly(img, { x + 1, y + 6, x + 28, y + 1, x + 32, y + 19, x + 5, y + 24 }, R.wood[2])
    L.line(img, x + 1, y + 6, x + 28, y + 1, R.wood[1])
    L.line(img, x + 5, y + 24, x + 32, y + 19, R.wood[3])
    L.poly(img, { x + 5, y + 8, x + 25, y + 4, x + 28, y + 17, x + 8, y + 21 }, R.glass[1])
    L.poly(img, { x + 7, y + 17, x + 13, y + 12, x + 21, y + 15,
      x + 24, y + 11, x + 28, y + 17, x + 8, y + 21 }, R.leaf[2])
  end

  local function fallenBooks(img, x, y)
    box(img, x, y + 3, 21, 7, R.navy)
    L.hline(img, x + 4, x + 18, y + 7, R.paper[2])
    L.poly(img, { x + 5, y + 2, x + 23, y, x + 27, y + 4, x + 9, y + 7 }, O)
    L.poly(img, { x + 7, y + 2, x + 22, y + 1, x + 25, y + 4, x + 10, y + 6 }, R.red[2])
    L.line(img, x + 10, y + 6, x + 25, y + 4, R.paper[2])
  end

  -- The chair lies on its side, seen from the front: the slatted back at left,
  -- the seat on edge in the middle and two straight legs pointing right.
  -- Only straight bars are used, so the shape reads as furniture.
  local function toppledChair(img, x, y)
    L.hline(img, x + 1, x + 41, y + 17, R.floor[3])
    L.hline(img, x + 3, x + 39, y + 18, R.floor[3])
    -- The stretcher between the legs, drawn first so the legs cover its ends.
    L.vline(img, x + 34, y + 5, y + 13, O)
    L.vline(img, x + 35, y + 5, y + 13, R.wood[2])
    L.vline(img, x + 36, y + 5, y + 13, O)
    box(img, x + 24, y + 3, 19, 3, R.wood)
    box(img, x + 24, y + 13, 19, 3, R.wood)
    -- The ladder back: three slats between the two uprights and the top rail.
    for _, sx in ipairs({ 6, 11, 16 }) do
      box(img, x + sx, y + 5, 3, 9, R.wood)
    end
    box(img, x, y + 3, 22, 3, R.wood)
    box(img, x, y + 13, 22, 3, R.wood)
    box(img, x, y + 3, 3, 13, R.wood)
    -- The seat stands on edge between the back and the legs.
    box(img, x + 21, y + 1, 5, 17, R.wood)
  end

  function M.backgroundAfter()
    local B = A.homeAfter
    local img = room(openFrontDoor, innerDoor, B.innerDoor)
    -- The lamp is still after the earthquake.
    L.blit(img, M.lampFrames()[1], table.unpack(B.lamp))
    tiltedPicture(img, table.unpack(B.picture))
    fallenBooks(img, table.unpack(B.books))
    toppledChair(img, table.unpack(B.chair))
    return img
  end

  function M.note()
    local img = L.newImage(12, 8)
    L.poly(img, { 2, 0, 11, 0, 10, 7, 0, 7 }, O)
    L.poly(img, { 3, 1, 10, 1, 9, 6, 1, 6 }, R.paper[1])
    L.hline(img, 2, 9, 6, R.paper[3])
    L.px(img, 9, 5, R.paper[2])
    L.hline(img, 4, 8, 2, R.navy[2])
    L.hline(img, 3, 8, 4, R.navy[2])
    L.hline(img, 3, 6, 5, R.navy[1])
    return img
  end

  function M.pen()
    local img = L.newImage(9, 6)
    L.line(img, 1, 4, 7, 1, O, 2)
    L.line(img, 1, 4, 6, 2, R.navy[1])
    L.px(img, 7, 1, R.metal[1])
    L.px(img, 1, 4, R.metal[2])
    return img
  end

  function M.lampFrames()
    local frames = {}
    for _, dx in ipairs({ 0, 4 }) do
      local img = L.newImage(36, 65)
      local x = 17 + dx
      L.line(img, 17, 2, x, 45, O, 2)
      L.line(img, 17, 2, x, 45, R.metal[2])
      box(img, 12, 0, 11, 4, R.metal)
      L.ellipse(img, x, 59, 4, 4, O)
      L.ellipse(img, x, 59, 3, 3, R.paper[1])
      L.poly(img, { x - 4, 44, x + 4, 44, x + 13, 57, x + 13, 59,
        x - 13, 59, x - 13, 57 }, O)
      L.poly(img, { x - 4, 45, x + 4, 45, x + 12, 57, x + 12, 58,
        x - 12, 58, x - 12, 57 }, R.cork[2])
      L.line(img, x - 4, 45, x - 11, 56, R.cork[1])
      L.line(img, x + 4, 46, x + 11, 56, R.cork[3])
      L.hline(img, x - 11, x + 11, 58, R.cork[3])
      frames[#frames + 1] = img
    end
    return frames
  end

  local function bagBody(img, x, y)
    L.poly(img, { x + 2, y, x + 15, y, x + 18, y + 5, x + 18, y + 18,
      x + 16, y + 19, x + 1, y + 19, x, y + 17, x, y + 5 }, R.red[2])
    L.line(img, x, y + 5, x + 2, y, R.red[1])
    L.vline(img, x, y + 6, y + 16, R.red[1])
    L.vline(img, x + 18, y + 6, y + 17, R.red[3])
    L.hline(img, x + 2, x + 16, y + 19, R.red[3])
    L.rect(img, x + 4, y + 10, 11, 6, R.red[3])
    L.rect(img, x + 4, y + 11, 10, 4, R.red[2])
    L.hline(img, x + 5, x + 13, y + 10, R.red[1])
    L.px(img, x + 12, y + 11, R.metal[1])
  end

  function M.bagOpen()
    local img = L.newImage(22, 23)
    bagBody(img, 1, 2)
    L.ellipse(img, 10, 4, 8, 3, R.red[3])
    L.ellipse(img, 10, 4, 6, 2, O)
    L.hline(img, 4, 13, 6, R.red[1])
    L.px(img, 17, 6, R.metal[1])
    L.outline(img, O)
    return img
  end

  function M.bagClosed()
    local img = L.newImage(20, 23)
    bagBody(img, 1, 2)
    L.hline(img, 7, 12, 1, R.red[3])
    L.px(img, 7, 1, R.red[1])
    L.px(img, 7, 2, R.red[2])
    L.px(img, 12, 2, R.red[3])
    L.line(img, 3, 6, 15, 6, R.red[1])
    L.line(img, 15, 6, 17, 8, R.red[3])
    L.px(img, 16, 7, R.metal[1])
    L.outline(img, O)
    return img
  end

  function M.water()
    local img = L.newImage(8, 19)
    box(img, 0, 4, 8, 15, R.glass)
    box(img, 2, 0, 4, 5, R.navy)
    L.rect(img, 1, 10, 6, 4, R.paper[2])
    L.px(img, 3, 11, R.glass[3])
    L.px(img, 4, 12, R.glass[3])
    return img
  end

  function M.can()
    local img = L.newImage(9, 12)
    box(img, 0, 0, 9, 12, R.metal)
    L.rect(img, 1, 4, 7, 5, R.leaf[2])
    L.vline(img, 1, 4, 8, R.leaf[1])
    L.vline(img, 7, 4, 8, R.leaf[3])
    L.px(img, 4, 1, R.metal[3])
    return img
  end

  function M.flashlight()
    local img = L.newImage(12, 8)
    box(img, 0, 2, 8, 5, R.navy)
    box(img, 7, 0, 5, 8, R.metal)
    L.vline(img, 10, 2, 5, R.cork[1])
    L.px(img, 4, 3, R.red[1])
    return img
  end

  function M.whistle()
    local img = L.newImage(11, 7)
    L.rect(img, 1, 2, 6, 3, R.cork[1])
    L.circle(img, 7, 3, 2, SUN)
    L.hline(img, 1, 7, 5, R.cork[3])
    L.px(img, 7, 3, O)
    L.outline(img, O)
    return img
  end

  function M.snack()
    local img = L.newImage(9, 13)
    box(img, 0, 0, 9, 13, R.pot)
    L.hline(img, 1, 7, 3, R.pot[3])
    L.hline(img, 1, 7, 9, R.pot[3])
    L.rect(img, 2, 5, 5, 3, R.cork[1])
    return img
  end

  function M.toy()
    local img = L.newImage(15, 11)
    L.poly(img, { 1, 5, 4, 5, 6, 2, 10, 2, 12, 5, 13, 5,
      13, 8, 1, 8 }, R.red[2])
    L.hline(img, 2, 12, 5, R.red[1])
    L.hline(img, 2, 12, 8, R.red[3])
    L.rect(img, 7, 3, 3, 2, R.glass[1])
    L.circle(img, 4, 8, 1, R.navy[3])
    L.circle(img, 11, 8, 1, R.navy[3])
    L.outline(img, O)
    L.px(img, 4, 8, R.metal[2])
    L.px(img, 11, 8, R.metal[2])
    return img
  end

  local function card()
    local img = L.newImage(16, 13)
    box(img, 0, 0, 16, 13, R.paper)
    L.rect(img, 2, 2, 12, 8, R.glass[1])
    L.hline(img, 2, 13, 9, R.leaf[2])
    return img
  end

  function M.parkCard()
    local img = card()
    L.vline(img, 6, 5, 9, R.wood[3])
    L.circle(img, 6, 5, 2, R.leaf[3])
    L.circle(img, 5, 4, 1, R.leaf[1])
    L.hline(img, 9, 12, 8, R.wood[2])
    L.px(img, 11, 4, SUN)
    return img
  end

  function M.houseCard()
    local img = card()
    L.rect(img, 5, 6, 7, 4, R.cork[1])
    L.poly(img, { 3, 6, 8, 2, 13, 6 }, R.red[2])
    L.line(img, 3, 6, 8, 2, R.red[1])
    L.vline(img, 9, 7, 9, R.wood[3])
    return img
  end

  return M
end
