-- The v2 classroom (480x270, shown at 4x = 1920x1080) for the 50-pixel Student, in the v2 style
-- of docs/adr/0001-v2-art-and-no-bantay.md: one dark outline around each object, three shades
-- per colour with light from the left. The Student's feet stand on y = 230.
-- Left to right: the door, the start spot, the desk (middle), the windows, the tall cabinet.
-- Moving parts are separate sprites: the ceiling fan and the cabinet (2 wobble frames each),
-- and desk_front, the part of the desk drawn over the ducking Student.
-- Placements (top-left, art pixels) are in M.LAYOUT and ArtSource/README.md.

return function(L, ST)
  local M = {}
  local O = ST.KEY.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end
  -- Ramps: light, base, dark
  local R = {
    wall = { rgb(252, 240, 208), rgb(240, 224, 184), rgb(218, 196, 152) },
    lower = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) },
    floor = { rgb(214, 198, 168), rgb(194, 176, 146), rgb(166, 148, 118) },
    wood = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) },
    board = { rgb(88, 138, 104), rgb(64, 112, 84), rgb(48, 90, 68) },
    glass = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) },
    metal = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) },
    paper = ST.KEY.W and { ST.KEY.W, ST.KEY.w, ST.KEY.v },
    navy = { ST.KEY.m, ST.KEY.N, ST.KEY.n },
    leaf = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) },
    pot = { rgb(232, 140, 96), rgb(200, 104, 70), rgb(156, 72, 52) },
    cork = { rgb(222, 178, 120), rgb(198, 150, 96), rgb(166, 120, 74) },
  }
  local FLAG = { blue = rgb(0, 56, 168), red = rgb(206, 17, 38), sun = rgb(252, 209, 22), white = ST.KEY.W }
  local CEIL = rgb(232, 216, 178)

  M.LAYOUT = {
    floorY = 230,
    fan = { 204, 0 },
    cabinet = { 424, 100 },
    deskFront = { 198, 188 },
    start = 126,
    spots = { door = 46, desk = 240, window = 372, cabinet = 440 },
  }

  -- A box with an outside outline, light left column, dark right column.
  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end
  -- Fill without the outline, light left / dark right.
  local function slab(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, ramp[2])
    L.vline(img, x, y, y + h - 1, ramp[1])
    L.vline(img, x + w - 1, y, y + h - 1, ramp[3])
  end

  -- The desk (top 84x7 at y 188, legs down to the floor line) -----------------
  local function deskFront()
    local img = L.newImage(84, 42)
    box(img, 0, 0, 84, 7, R.wood)              -- the top
    L.hline(img, 1, 82, 1, R.wood[1])
    box(img, 2, 6, 7, 36, R.wood)              -- front legs
    box(img, 75, 6, 7, 36, R.wood)
    box(img, 8, 32, 68, 4, R.wood)             -- the low front bar
    return img
  end
  M.deskFront = deskFront

  local function deskBack(img, x, y)
    -- back legs and the back bar sit higher (further away) and are a shade darker
    box(img, x + 12, y + 6, 5, 26, { R.wood[2], R.wood[3], R.wood[3] })
    box(img, x + 67, y + 6, 5, 26, { R.wood[2], R.wood[3], R.wood[3] })
    box(img, x + 14, y + 14, 56, 3, { R.wood[3], R.wood[3], R.wood[3] })
    L.blit(img, deskFront(), x, y)
  end

  local function chair(img, x, y)
    box(img, x, y, 5, 42, R.navy)              -- back post
    box(img, x, y + 4, 22, 5, R.navy)          -- backrest
    box(img, x, y + 22, 24, 5, R.wood)         -- seat
    box(img, x + 19, y + 26, 5, 16, R.navy)    -- front leg
  end

  local function jalousie(img, x, y, w, h)
    box(img, x, y, w, h, R.wood)
    L.rect(img, x + 4, y + 4, w - 8, h - 8, O)
    -- sky and a hill line seen through the glass
    L.rect(img, x + 5, y + 5, w - 10, h - 10, R.glass[1])
    L.rect(img, x + 5, y + h - 20, w - 10, 15, R.leaf[2])
    L.rect(img, x + 5, y + h - 20, w - 10, 2, R.leaf[1])
    -- slats every 7 rows
    for sy = y + 8, y + h - 8, 7 do
      L.hline(img, x + 5, x + w - 6, sy, R.glass[3])
      L.hline(img, x + 5, x + w - 6, sy - 1, R.glass[2])
    end
    -- middle post
    L.rect(img, x + w // 2 - 2, y + 4, 4, h - 8, O)
    L.rect(img, x + w // 2 - 1, y + 4, 2, h - 8, R.wood[2])
    -- sill
    box(img, x - 4, y + h - 2, w + 8, 6, R.wood)
  end

  local function drawFloor(img, w, h)
    local light, base, shadow = table.unpack(R.floor)
    L.rect(img, 0, 208, w, h - 208, base)
    L.hline(img, 0, w - 1, 208, shadow)
    -- Jalousie light fans across the floor. The gap is the window's middle post.
    L.poly(img, { 337, 209, 371, 209, 362, 236, 305, 236 }, light)
    L.poly(img, { 375, 209, 410, 209, 427, 236, 369, 236 }, light)

    -- Sparse flecks give the concrete texture without making a repeating grid.
    for i = 1, 72 do
      local x = (97 * i + 19 * i * i) % (w - 8) + 4
      local y = (17 * i + 7 * i * i) % (h - 212) + 211
      local pixel = img:getPixel(x, y)
      L.px(img, x, y, pixel == light and base or (i % 3 == 0 and shadow or light))
    end
  end

  M.FLOOR = R.floor
  function M.background(withoutChair)
    local W, H = 480, 270
    local img = L.newImage(W, H)
    -- ceiling, wall, lower wall, baseboard, floor
    L.rect(img, 0, 0, W, 14, CEIL)
    L.hline(img, 0, W - 1, 14, O)
    L.rect(img, 0, 15, W, 150, R.wall[2])
    L.rect(img, 0, 15, W, 2, R.wall[1])
    L.rect(img, 0, 165, W, 38, R.lower[2])
    L.hline(img, 0, W - 1, 165, R.lower[1])
    L.hline(img, 0, W - 1, 164, R.wall[3])
    L.rect(img, 0, 203, W, 4, R.wood[3])
    L.hline(img, 0, W - 1, 207, O)
    drawFloor(img, W, H)

    -- door (left) with its frame and a small window
    box(img, 16, 106, 62, 98, R.wood)
    box(img, 22, 112, 50, 92, R.wood)
    box(img, 30, 120, 34, 22, R.glass)
    L.line(img, 34, 136, 44, 124, R.glass[1])
    box(img, 30, 150, 34, 18, R.wood)
    box(img, 30, 174, 34, 22, R.wood)
    box(img, 62, 158, 6, 6, { FLAG.sun, FLAG.sun, R.wood[3] })
    box(img, 24, 92, 46, 11, { R.leaf[1], R.leaf[2], R.leaf[3] })
    L.text(img, 35, 94, "EXIT", ST.KEY.W, 1)

    -- Philippine flag above the bulletin board
    local fx, fy = 92, 30
    L.rect(img, fx, fy, 40, 24, O)
    L.rect(img, fx + 1, fy + 1, 38, 11, FLAG.blue)
    L.rect(img, fx + 1, fy + 12, 38, 11, FLAG.red)
    L.poly(img, { fx + 1, fy + 1, fx + 20, fy + 12, fx + 1, fy + 23 }, FLAG.white)
    L.circle(img, fx + 7, fy + 12, 2, FLAG.sun)
    for _, s in ipairs({ { 3, 4 }, { 3, 20 }, { 16, 12 } }) do L.px(img, fx + s[1], fy + s[2], FLAG.sun) end

    -- bulletin board with papers
    box(img, 86, 64, 52, 50, R.wood)
    L.rect(img, 89, 67, 46, 44, R.cork[2])
    L.vline(img, 89, 67, 110, R.cork[1])
    box(img, 93, 71, 16, 20, R.paper)
    box(img, 112, 74, 18, 14, { FLAG.sun, FLAG.sun, R.wood[1] })
    box(img, 96, 94, 20, 13, R.paper)
    for _, p in ipairs({ { 100, 71 }, { 120, 74 }, { 105, 94 } }) do L.px(img, p[1], p[2], FLAG.red) end
    for yy = 76, 86, 3 do L.hline(img, 96, 105, yy, R.metal[3]) end

    -- alphabet strip and the blackboard
    box(img, 150, 30, 184, 14, R.paper)
    L.text(img, 156, 33, "Aa Bb Cc Dd Ee Ff Gg Hh", R.navy[2], 1)
    box(img, 150, 50, 184, 82, R.wood)
    L.rect(img, 155, 55, 174, 72, O)
    L.rect(img, 156, 56, 172, 70, R.board[2])
    L.vline(img, 156, 56, 125, R.board[1])
    L.hline(img, 156, 327, 56, R.board[1])
    local msg = "Magandang umaga!"
    L.text(img, 156 + (172 - L.F.width(msg, 1)) // 2, 70, msg, ST.KEY.W, 1)
    L.text(img, 176, 90, "Duck, Cover, and Hold", ST.KEY.w, 1)
    L.hline(img, 176, 304, 100, R.board[1])
    box(img, 152, 130, 180, 5, R.wood)          -- chalk ledge
    L.rect(img, 180, 128, 8, 2, ST.KEY.W)
    L.rect(img, 300, 128, 6, 2, FLAG.sun)

    -- clock
    L.circle(img, 352, 34, 9, O)
    L.circle(img, 352, 34, 8, R.paper[1])
    L.circle(img, 352, 34, 7, R.paper[2])
    L.vline(img, 352, 29, 34, O)
    L.hline(img, 352, 356, 34, O)

    -- windows with a plant on the sill
    jalousie(img, 336, 62, 78, 92)
    L.rect(img, 344, 148, 12, 9, O)
    L.rect(img, 345, 149, 10, 7, R.pot[2])
    L.vline(img, 345, 149, 155, R.pot[1])
    L.circle(img, 346, 143, 5, O); L.circle(img, 354, 141, 5, O)
    L.circle(img, 346, 143, 4, R.leaf[2]); L.circle(img, 354, 141, 4, R.leaf[2])
    L.circle(img, 345, 142, 2, R.leaf[1]); L.circle(img, 353, 140, 2, R.leaf[1])

    -- a chair tucked behind the desk, then the desk itself
    if not withoutChair then chair(img, 280, 188) end
    deskBack(img, M.LAYOUT.deskFront[1], M.LAYOUT.deskFront[2])
    return img
  end

  -- Ceiling fan, 72x30, 2 wobble frames -------------------------------------
  function M.fanFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(72, 30)
      local dx = f
      L.rect(img, 35, 0, 2, 14, R.metal[3])
      L.rect(img, 35 + dx, 12, 2, 4, R.metal[3])
      local lw, rw = (f == 1) and 25 or 29, (f == 1) and 29 or 25
      local cx = 36 + dx
      L.rect(img, cx - 5 - lw, 18, lw, 3, R.wood[2]); L.hline(img, cx - 5 - lw, cx - 6, 18, R.wood[1])
      L.rect(img, cx + 6, 18, rw, 3, R.wood[2]); L.hline(img, cx + 6, cx + 5 + rw, 18, R.wood[1])
      L.ellipse(img, cx, 19, 6, 3, R.metal[2])
      L.ellipse(img, cx - 1, 18, 4, 2, R.metal[1])
      L.ellipse(img, cx, 24, 3, 2, FLAG.sun)
      L.outline(img, O)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Tall cabinet, 48x112, 2 wobble frames (the top leans 1 pixel) -------------
  function M.cabinetFrames()
    local frames = {}
    local books = { FLAG.red, R.navy[2], R.leaf[2], FLAG.sun, R.pot[2], R.glass[3] }
    for f = 0, 1 do
      local img = L.newImage(48, 112)
      local lean = f
      box(img, 1 + lean, 0, 44, 8, R.wood)          -- top board
      box(img, 1, 6, 44, 106, R.wood)
      L.rect(img, 5, 10, 36, 98, O)
      L.rect(img, 6, 11, 34, 96, R.wood[3])
      for i, sy in ipairs({ 36, 62, 88 }) do
        box(img, 4, sy, 38, 4, R.wood)
        local bx = 8 + ((f == 1 and i == 2) and 1 or 0)
        for b = 1, 6 do
          local bh = 14 + (b * 5 + i * 3) % 6
          box(img, bx, sy - bh, 5, bh, { books[b], books[b], books[(b % 6) + 1] })
          bx = bx + 5
        end
      end
      box(img, 9, 15, 16, 12, R.cork)                -- a box on the top shelf
      frames[#frames + 1] = img
    end
    return frames
  end

  return M
end
