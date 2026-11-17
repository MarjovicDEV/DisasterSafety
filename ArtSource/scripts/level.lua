-- Level art: the classroom Scenario scene (480x270, shown at 4x = 1920x1080), the parts that
-- wobble while the ground shakes (ceiling fan, tall cabinet; 2 frames each), and one 72x52
-- picture per Choice, used on the Choice card and on the Feedback card.
-- Nothing falls: the research supports "watch out for falling objects" but the scene must stay
-- gentle for young Learners (docs/design/first-playable-level.md, Q11).

return function(L, CH)
  local P = L.P
  local M = {}

  local function framed(img, x, y, w, h, frame, fill)
    L.rect(img, x, y, w, h, P.ink)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, frame)
    L.rect(img, x + 4, y + 4, w - 8, h - 8, fill)
  end

  local function windowPane(img, x, y, w, h)
    framed(img, x, y, w, h, P.teal2, P.glass2)
    -- the view is drawn on its own image so hills and clouds stay inside the glass
    local gw, gh = w - 8, h - 8
    local view = L.newImage(gw, gh)
    L.vgrad(view, 0, 0, gw, gh, { P.sky3, P.sky4, P.sky5 })
    L.ellipse(view, gw * 0.3, gh, gw * 0.35, 9, P.hill)
    L.ellipse(view, gw * 0.75, gh, gw * 0.4, 7, P.hill2)
    L.rect(view, 0, gh - 3, gw, 3, P.hill2)
    L.ellipse(view, gw * 0.35, 10, 8, 3, P.white)
    L.ellipse(view, gw * 0.42, 8, 5, 3, P.white)
    L.blit(img, view, x + 4, y + 4)
    -- glare
    L.line(img, x + 8, y + 18, x + 16, y + 10, P.white)
    L.line(img, x + 10, y + 22, x + 20, y + 12, P.white)
    -- bars
    L.rect(img, x + w // 2 - 1, y + 4, 3, h - 8, P.teal2)
    L.rect(img, x + 4, y + h // 2 - 1, w - 8, 3, P.teal2)
    -- sill
    L.rect(img, x - 3, y + h, w + 6, 4, P.brown2)
    L.hline(img, x - 3, x + w + 2, y + h, P.brown3)
  end

  local function desk(img, x, y, w)
    -- top, apron and two legs; y is the desk top
    L.rect(img, x, y, w, 4, P.brown)
    L.hline(img, x, x + w - 1, y, P.brown3)
    L.hline(img, x, x + w - 1, y + 4, P.ink)
    L.rect(img, x + 2, y + 5, 3, 22, P.brown2)
    L.rect(img, x + w - 5, y + 5, 3, 22, P.brown2)
    L.rect(img, x + 2, y + 5, w - 4, 2, P.brown2)
  end

  local function chair(img, x, y)
    L.rect(img, x, y, 3, 26, P.blue2)
    L.rect(img, x, y + 12, 14, 3, P.blue)
    L.rect(img, x + 11, y + 15, 3, 11, P.blue2)
    L.rect(img, x + 1, y + 1, 2, 9, P.blue)
  end

  local function books(img, x, y, n, seed)
    L.seed(seed)
    local colors = { P.red, P.blue, P.green, P.yellow2, P.purple, P.orange }
    local cx = x
    for i = 1, n do
      local w = L.randi(2, 4)
      local h = L.randi(6, 9)
      L.rect(img, cx, y - h, w, h, colors[(i % #colors) + 1])
      L.hline(img, cx, cx + w - 1, y - h, P.ink2)
      cx = cx + w + 1
    end
  end

  -- Classroom background --------------------------------------------------
  function M.classroom()
    local W, H = 480, 270
    local img = L.newImage(W, H)
    -- wall and ceiling
    L.vgrad(img, 0, 0, W, 178, { P.wall2, P.wall, P.wall, P.cream })
    L.rect(img, 0, 0, W, 6, P.wall3)
    L.hline(img, 0, W - 1, 6, P.paper3)
    -- wainscot and baseboard
    L.rect(img, 0, 150, W, 26, P.paper2)
    L.hline(img, 0, W - 1, 150, P.wall3)
    for x = 6, W, 24 do L.vline(img, x, 152, 174, P.wall2) end
    L.rect(img, 0, 174, W, 4, P.brown2)
    -- floor planks
    L.vgrad(img, 0, 178, W, H - 178, { P.tan, P.brown3, P.brown3, P.brown })
    for y = 186, H, 12 do L.hline(img, 0, W - 1, y, P.tan2) end
    L.seed(77)
    for y = 178, H, 12 do
      local off = L.randi(0, 40)
      for x = off, W, 64 do L.vline(img, x, y + 1, y + 11, P.tan2) end
    end

    -- window (left)
    windowPane(img, 24, 34, 104, 92)

    -- blackboard (centre) with a friendly greeting
    framed(img, 160, 24, 160, 78, P.brown2, P.green3)
    L.vgrad(img, 164, 28, 152, 70, { P.green3, L.hex("245e2e") })
    local msg = "Good morning!"
    local tw = L.F.width(msg, 2)
    L.text(img, 160 + (160 - tw) // 2, 44, msg, P.white, 2)
    L.hline(img, 182, 296, 70, P.green2)
    L.text(img, 190, 78, "Class 4 - Sampaguita", P.gray1, 1)
    L.rect(img, 166, 102, 148, 3, P.brown)
    L.rect(img, 190, 100, 8, 2, P.white)
    L.rect(img, 280, 100, 6, 2, P.yellow3)

    -- wall clock, right of the hanging fan
    L.circle(img, 300, 14, 8, P.ink)
    L.circle(img, 300, 14, 7, P.white)
    L.vline(img, 300, 9, 14, P.ink)
    L.hline(img, 300, 304, 14, P.ink)


    -- Philippine flag poster to the left of the door
    L.rect(img, 334, 30, 34, 22, P.ink)
    L.rect(img, 335, 31, 32, 10, P.flagBlue)
    L.rect(img, 335, 41, 32, 10, P.flagRed)
    L.poly(img, { 335, 31, 351, 41, 335, 51 }, P.flagWhite)
    L.circle(img, 340, 41, 2, P.flagSun)

    -- door (right)
    framed(img, 408, 60, 52, 116, P.brown2, P.brown)
    L.rect(img, 418, 72, 32, 22, P.ink)
    L.rect(img, 419, 73, 30, 20, P.glass2)
    L.line(img, 422, 88, 432, 76, P.white)
    L.rect(img, 418, 102, 32, 30, P.brown2)
    L.rect(img, 420, 104, 28, 26, P.brown3)
    L.rect(img, 418, 140, 32, 28, P.brown2)
    L.rect(img, 420, 142, 28, 24, P.brown3)
    L.circle(img, 450, 124, 2, P.yellow)
    L.text(img, 414, 52, "EXIT", P.green3, 1)

    -- teacher's table with books and a plant
    L.rect(img, 186, 124, 108, 6, P.brown)
    L.hline(img, 186, 293, 124, P.brown3)
    L.hline(img, 186, 293, 130, P.ink)
    L.rect(img, 190, 131, 100, 20, P.brown2)
    L.rect(img, 194, 134, 92, 14, P.brown)
    L.rect(img, 190, 151, 4, 26, P.brown2)
    L.rect(img, 286, 151, 4, 26, P.brown2)
    books(img, 200, 124, 5, 11)
    L.rect(img, 266, 116, 10, 8, P.orange2)
    L.circle(img, 268, 112, 4, P.green)
    L.circle(img, 274, 111, 3, P.green2)

    -- student desks and chairs in front
    desk(img, 34, 150, 64)
    chair(img, 100, 152)
    desk(img, 334, 150, 64)
    chair(img, 316, 152)
    books(img, 42, 150, 3, 23)
    books(img, 344, 150, 2, 41)
    return img
  end

  -- Ceiling fan (hangs from the top centre), 2 wobble frames, 72x26 --------
  function M.fanFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(72, 26)
      local dx = (f == 1) and 1 or 0
      L.rect(img, 35, 0, 2, 10, P.gray3)
      L.rect(img, 35 + dx, 8, 2, 4, P.gray3)
      L.ellipse(img, 36 + dx, 14, 6, 3, P.metal2)
      L.ellipse(img, 36 + dx, 13, 5, 2, P.metal)
      local lw = (f == 1) and 26 or 30
      local rw = (f == 1) and 30 or 26
      L.rect(img, 36 + dx - 5 - lw, 13, lw, 3, P.brown3)
      L.hline(img, 36 + dx - 5 - lw, 36 + dx - 6, 13, P.tan)
      L.rect(img, 36 + dx + 6, 13, rw, 3, P.brown3)
      L.hline(img, 36 + dx + 6, 36 + dx + 5 + rw, 13, P.tan)
      L.ellipse(img, 36 + dx, 19, 3, 2, P.yellow3)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Tall cabinet (stands between the flag poster and the door), 2 wobble frames, 42x112
  function M.cabinetFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(42, 112)
      local lean = (f == 1) and 1 or 0
      L.rect(img, 1 + lean, 1, 38, 4, P.brown3)
      L.rect(img, 1, 4, 38, 107, P.brown2)
      L.rect(img, 4, 7, 32, 100, P.brown)
      for i, y in ipairs({ 32, 58, 84 }) do
        L.rect(img, 4, y, 32, 3, P.brown3)
        books(img, 6 + ((f == 1 and i == 2) and 2 or 0), y, 5, 90 + i)
      end
      -- a box on the top shelf
      L.rect(img, 8, 10, 14, 10, P.paper3)
      L.rect(img, 8, 10, 14, 2, P.paper2)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Choice pictures 72x52 -------------------------------------------------
  local PW, PH = 72, 52

  local function pictureBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 38, { P.wall, P.cream })
    L.rect(img, 0, 38, PW, PH - 38, P.brown3)
    L.hline(img, 0, PW - 1, 38, P.brown2)
    L.hline(img, 0, PW - 1, 45, P.tan2)
    return img
  end

  local function shakeLines(img, x, y)
    L.vline(img, x, y, y + 4, P.ink2)
    L.vline(img, x + 2, y + 2, y + 6, P.ink2)
  end

  -- Colours for the crouching-child pixel maps below (the same palette as characters.lua).
  local KID_KEY = { a = P.ink, b = P.hair, c = P.hair2, d = P.skin1, e = P.skin2, f = P.blush, g = P.skin3,
        h = P.gray1, i = P.white, j = P.gray2, k = P.red2, l = P.red, m = P.navy2, n = P.navy, o = P.ink2 }

  -- A child crouched low under the desk, holding on to the desk leg (Duck, Cover, and Hold).
  -- A hand-placed 30x30 pixel map in the style of the standing students (same head), facing right.
  local function crouchingKid()
    local k = L.newImage(30, 30)
    L.map(k, 0, 0, {
      "..........aaaaaaaaa...........",
      ".........aaabbbbbaaa..........",
      "........aabcccccbbbaa.........",
      ".......aabbbbbbbbbbbaa........",
      "......aabbbbbbbbbbbbdaa.......",
      "......aabbbbbbbbbbbbdaa.......",
      ".....aabbbbbbdddddbbddaa......",
      ".....aabbbbbbdddbdddddaa......",
      ".....aabbbbbbdddddadddaa......",
      ".....aabbbbbbeedddadddaa......",
      ".....aabbbbbbddddddddddaa.....",
      "......aabbbbbddddddfdaaa......",
      "......aaddddddddddddgaa.......",
      ".......aadddddddddddaa........",
      "....aaaaaadddddddddaa....aaa..",
      "...akkkkkkhiikiiiiia....addea.",
      "...akkkkkkhiikiiiiiiaaaaaddda.",
      "...akllllkhiiiiiiiiidddddddda.",
      "...akllllkhiiiiiiiiidddddddda.",
      "...akllllkhiiiiiiiiaeeeeeddda.",
      "...akkkkkkhiiiiiiiiaaaaaaddea.",
      "...akllllkhiiiiiiiiaaa...aaa..",
      "...akllllnnnnnnnnnnnnna.......",
      "...akkkkknnnnnnnnnnnnna.......",
      "....aaaaannnnnnnnnnnnna.......",
      "........ammmmmmmmmmmmma.......",
      ".........aeeeaaaaaddda........",
      ".........aeeea...adddaa.......",
      "........aoooooa.aooooooa......",
      "........aoooooa.aooooooa......",
    }, KID_KEY)
    return k
  end

  function M.choicePictures()
    local pics = {}

    -- Duck under the desk and hold on
    local desk1 = pictureBackdrop()
    L.rect(desk1, 12, 12, 50, 4, P.brown)
    L.hline(desk1, 12, 61, 12, P.brown3)
    L.hline(desk1, 12, 61, 16, P.ink)
    L.rect(desk1, 14, 17, 3, 30, P.brown2)
    L.rect(desk1, 57, 17, 3, 30, P.brown2)
    L.blit(desk1, crouchingKid(), 31, 18)
    shakeLines(desk1, 4, 8)
    shakeLines(desk1, 66, 8)
    pics.choice_desk = desk1

    -- Run outside
    local run = pictureBackdrop()
    L.rect(run, 52, 4, 18, 40, P.ink)
    L.rect(run, 53, 5, 16, 38, P.brown)
    L.rect(run, 56, 9, 10, 8, P.glass2)
    L.circle(run, 66, 26, 1, P.yellow)
    L.blit(run, CH.student(1, false), 22, 4)
    for _, s in ipairs({ { 6, 18 }, { 3, 25 }, { 7, 32 } }) do L.hline(run, s[1], s[1] + 9, s[2], P.ink2) end
    pics.choice_run = run

    -- Go to the window
    local win = pictureBackdrop()
    L.rect(win, 4, 4, 34, 30, P.ink)
    L.rect(win, 5, 5, 32, 28, P.teal2)
    L.vgrad(win, 7, 7, 28, 24, { P.sky3, P.sky5 })
    L.rect(win, 20, 7, 2, 24, P.teal2)
    L.rect(win, 7, 18, 28, 2, P.teal2)
    L.line(win, 9, 16, 14, 10, P.white)
    L.blit(win, CH.student(0, true), 36, 6, true)
    shakeLines(win, 40, 2)
    shakeLines(win, 1, 36)
    pics.choice_window = win

    -- Stand by the tall cabinet
    local cab = pictureBackdrop()
    L.rect(cab, 44, 1, 24, 44, P.ink)
    L.rect(cab, 45, 2, 22, 42, P.brown2)
    L.rect(cab, 47, 4, 18, 38, P.brown)
    for i, y in ipairs({ 16, 29, 41 }) do
      L.rect(cab, 47, y, 18, 2, P.brown3)
      books(cab, 48, y, 3, 60 + i)
    end
    L.blit(cab, CH.student(0, false), 16, 6)
    shakeLines(cab, 69, 4)
    shakeLines(cab, 40, 2)
    pics.choice_cabinet = cab

    return pics
  end

  -- Easy / 2: the corridor ===================================================
  -- No desk to hide under (EQ-47). Jalousie windows on the back wall, hanging flower pots
  -- (EQ-30), a bulletin board, a classroom door and the stairs down at the far end.

  -- A jalousie window: glass slats over a view of the courtyard.
  local function jalousie(img, x, y, w, h)
    framed(img, x, y, w, h, P.teal2, P.glass2)
    local gw, gh = w - 8, h - 8
    local view = L.newImage(gw, gh)
    L.vgrad(view, 0, 0, gw, gh, { P.sky3, P.sky4, P.sky5 })
    L.ellipse(view, gw * 0.25, gh + 2, gw * 0.4, 12, P.hill)
    L.ellipse(view, gw * 0.8, gh + 2, gw * 0.35, 9, P.hill2)
    L.rect(view, gw - 26, gh - 22, 3, 14, P.brown2)
    L.circle(view, gw - 25, gh - 26, 7, P.green2)
    L.circle(view, gw - 28, gh - 28, 4, P.green)
    L.ellipse(view, gw * 0.3, 9, 7, 3, P.white)
    L.blit(img, view, x + 4, y + 4)
    -- slats: a dark edge and a light glint every 7 px
    for sy = y + 9, y + h - 6, 7 do
      L.hline(img, x + 4, x + w - 5, sy, P.glass3)
      L.hline(img, x + 4, x + w - 5, sy + 1, P.white)
    end
    L.rect(img, x + w // 2 - 1, y + 4, 3, h - 8, P.teal2)
    L.rect(img, x - 3, y + h, w + 6, 4, P.concrete2)
    L.hline(img, x - 3, x + w + 2, y + h, P.white)
  end

  local function tileFloor(img, top, W, H, size)
    for ty = top, H - 1, size do
      for tx = 0, W - 1, size do
        local odd = ((tx // size) + ((ty - top) // size)) % 2 == 1
        L.rect(img, tx, ty, size, size, odd and P.paper2 or P.paper)
      end
    end
    for ty = top, H - 1, size do L.hline(img, 0, W - 1, ty, P.paper3) end
  end

  function M.corridor()
    local W, H = 480, 270
    local img = L.newImage(W, H)
    -- ceiling with a beam
    L.rect(img, 0, 0, W, 12, P.concrete2)
    L.hline(img, 0, W - 1, 12, P.concrete3)
    -- upper wall cream, lower wall painted green like many public schools
    L.vgrad(img, 0, 13, W, 117, { P.cream, P.wall, P.wall })
    L.rect(img, 0, 130, W, 46, P.teal)
    L.hline(img, 0, W - 1, 130, P.teal2)
    L.hline(img, 0, W - 1, 131, P.teal3)
    L.rect(img, 0, 172, W, 4, P.teal2)
    -- floor tiles
    tileFloor(img, 176, W, H, 16)

    jalousie(img, 18, 30, 94, 84)
    jalousie(img, 140, 30, 92, 84)
    -- ceiling hooks for the two hanging pots (sprites placed at x 116 and 236)
    L.rect(img, 125, 10, 2, 3, P.gray4)
    L.rect(img, 245, 10, 2, 3, P.gray4)

    -- bulletin board
    framed(img, 262, 34, 84, 66, P.brown2, P.tan2)
    L.rect(img, 270, 42, 22, 26, P.white)
    for ly = 48, 64, 4 do L.hline(img, 273, 288, ly, P.gray2) end
    L.rect(img, 298, 40, 20, 18, P.yellow3)
    L.poly(img, { 302, 54, 308, 46, 314, 54 }, P.roof)
    L.rect(img, 304, 54, 8, 2, P.brown)
    L.rect(img, 322, 44, 18, 24, P.blue3)
    L.circle(img, 331, 52, 4, P.yellow)
    L.rect(img, 298, 64, 20, 28, P.pink)
    L.rect(img, 270, 74, 22, 18, P.green4)
    for _, pin in ipairs({ { 280, 42 }, { 307, 40 }, { 330, 44 }, { 307, 64 }, { 280, 74 } }) do
      L.px(img, pin[1], pin[2], P.red)
    end

    -- classroom door with a room sign
    framed(img, 356, 60, 46, 116, P.brown2, P.brown)
    L.rect(img, 364, 72, 30, 22, P.ink)
    L.rect(img, 365, 73, 28, 20, P.glass2)
    L.line(img, 368, 88, 377, 77, P.white)
    L.rect(img, 364, 104, 30, 60, P.brown2)
    L.rect(img, 366, 106, 26, 56, P.brown3)
    L.circle(img, 394, 124, 2, P.yellow)
    L.rect(img, 360, 44, 38, 12, P.white)
    L.rect(img, 360, 44, 38, 1, P.gray2)
    L.text(img, 364, 46, "Gr. 4", P.blue2, 1)

    -- stairs going down at the far right, with a hand rail
    L.rect(img, 410, 120, 70, 56, P.wall2)
    L.rect(img, 410, 176, 70, H - 176, P.concrete3)
    for i = 0, 6 do
      local sx, sy = 412 + i * 10, 176 + i * 9
      L.rect(img, sx, sy, W - sx, 9, P.concrete)
      L.hline(img, sx, W - 1, sy, P.white)
      L.vline(img, sx, sy, sy + 8, P.concrete2)
    end
    L.line(img, 408, 138, 480, 196, P.brown2, 2)
    for i = 0, 3 do
      local px = 414 + i * 18
      local top = 140 + math.floor((px - 408) * 58 / 72)
      L.vline(img, px, top, top + 34, P.brown2)
    end
    -- EXIT sign above the stairs
    L.rect(img, 418, 60, 40, 14, P.green3)
    L.rect(img, 419, 61, 38, 12, P.green2)
    L.text(img, 425, 63, "EXIT", P.white, 1)
    return img
  end

  -- Hanging flower pot, 2 sway frames, 20x44. Nothing falls; it only sways (Q11).
  function M.potFrames()
    local frames = {}
    for f = 0, 1 do
      local img = L.newImage(20, 44)
      local dx = f
      L.line(img, 10, 0, 4 + dx, 22, P.gray3)
      L.line(img, 10, 0, 16 + dx, 22, P.gray3)
      L.line(img, 10, 0, 10 + dx, 22, P.gray4)
      -- leaves above the rim and vines trailing down
      L.circle(img, 7 + dx, 20, 3, P.green2)
      L.circle(img, 13 + dx, 19, 3, P.green)
      L.circle(img, 10 + dx, 18, 2, P.green4)
      L.line(img, 4 + dx, 24, 2 + dx, 38, P.green2)
      L.line(img, 16 + dx, 24, 17 + dx, 41, P.green2)
      for _, lf in ipairs({ { 2, 30 }, { 3, 36 }, { 17, 29 }, { 17, 36 }, { 18, 41 } }) do
        L.px(img, lf[1] + dx, lf[2], P.green)
        L.px(img, lf[1] + dx + 1, lf[2], P.green)
      end
      -- clay pot
      L.poly(img, { 4 + dx, 23, 16 + dx, 23, 14 + dx, 33, 6 + dx, 33 }, P.orange2)
      L.rect(img, 3 + dx, 21, 14, 3, P.orange)
      L.hline(img, 4 + dx, 15 + dx, 21, P.fire2)
      L.vline(img, 7 + dx, 25, 31, P.orange)
      L.outline(img, P.ink)
      frames[#frames + 1] = img
    end
    return frames
  end

  -- Easy / 3: the classroom after the shaking ================================
  -- Calm and still: the fan and cabinet are painted in, the door is open to the courtyard,
  -- and the teacher (a separate sprite at x 418, y 126) waves from the doorway.
  function M.classroomAfter()
    local img = M.classroom()
    L.blit(img, M.fanFrames()[1], 204, 0)
    L.blit(img, M.cabinetFrames()[1], 364, 64)
    -- open doorway with the courtyard outside
    local view = L.newImage(44, 108)
    L.vgrad(view, 0, 0, 44, 70, { P.sky3, P.sky4, P.sky5 })
    L.ellipse(view, 30, 70, 24, 10, P.hill)
    L.rect(view, 0, 70, 44, 38, P.grass)
    L.rect(view, 0, 82, 44, 26, P.concrete)
    L.hline(view, 0, 43, 82, P.concrete2)
    L.rect(view, 6, 46, 3, 26, P.brown2)
    L.circle(view, 7, 42, 8, P.green2)
    L.circle(view, 4, 39, 5, P.green)
    L.blit(img, view, 412, 64)
    -- the door leaf swung open against the wall
    L.rect(img, 460, 62, 12, 114, P.ink)
    L.rect(img, 461, 63, 10, 112, P.brown)
    L.rect(img, 462, 70, 8, 20, P.glass2)
    L.rect(img, 462, 100, 8, 70, P.brown3)
    -- warm light on the floor in front of the door
    for y = 178, 196 do
      for x = 404 - (y - 178), 468 do
        if ((x + y) % 4 == 0) then L.px(img, x, y, P.yellow3) end
      end
    end
    return img
  end

  -- The teacher, facing left toward the class, waving. 2 frames, 30x50.
  function M.teacher(f)
    local img = L.newImage(30, 50)
    local hx, hy = (f == 0) and 3 or 7, (f == 0) and 9 or 7
    -- far arm resting
    L.line(img, 20, 21, 21, 30, P.gray1, 2)
    L.rect(img, 20, 31, 2, 2, P.skin2)
    -- skirt, legs, shoes
    L.poly(img, { 9, 31, 21, 31, 23, 42, 7, 42 }, P.teal2)
    L.vline(img, 15, 33, 41, P.teal)
    L.rect(img, 11, 42, 3, 5, P.skin1)
    L.rect(img, 16, 42, 3, 5, P.skin2)
    L.rect(img, 9, 47, 5, 2, P.ink)
    L.rect(img, 15, 47, 5, 2, P.ink)
    -- blouse with a collar and an ID on a lanyard
    L.rect(img, 9, 19, 13, 13, P.white)
    L.vline(img, 21, 19, 31, P.gray1)
    L.poly(img, { 12, 19, 15, 23, 18, 19 }, P.teal3)
    L.line(img, 12, 20, 14, 26, P.red)
    L.line(img, 18, 20, 16, 26, P.red)
    L.rect(img, 13, 26, 4, 4, P.blue3)
    L.hline(img, 13, 16, 26, P.blue2)
    -- neck and head
    L.rect(img, 14, 17, 3, 3, P.skin1)
    L.circle(img, 15, 11, 6, P.skin1)
    for y = 4, 17 do
      for x = 8, 22 do
        if L.get(img, x, y) == P.skin1 and (y < 8 or x > 18) then L.px(img, x, y, P.hair) end
      end
    end
    L.circle(img, 20, 5, 3, P.hair)          -- bun
    L.px(img, 19, 4, P.hair2)
    L.vline(img, 11, 10, 11, P.ink)          -- eye
    L.px(img, 10, 13, P.blush)
    -- waving arm, up and to the front
    L.line(img, 10, 20, (10 + hx) // 2, 15, P.white, 2)
    L.line(img, (10 + hx) // 2, 15, hx, hy, P.skin1, 2)
    L.rect(img, hx - 1, hy - 2, 3, 3, P.skin1)
    L.outline(img, P.ink)
    return img
  end

  function M.teacherFrames() return { M.teacher(0), M.teacher(1) } end

  -- Choice pictures for Easy / 2 and Easy / 3, 72x52 ==========================
  local function corridorBackdrop()
    local img = L.newImage(PW, PH)
    L.vgrad(img, 0, 0, PW, 26, { P.cream, P.wall })
    L.rect(img, 0, 26, PW, 12, P.teal)
    L.hline(img, 0, PW - 1, 26, P.teal2)
    L.hline(img, 0, PW - 1, 27, P.teal3)
    tileFloor(img, 38, PW, PH, 7)
    return img
  end

  local function smallJalousie(img, x, y, w, h)
    L.rect(img, x, y, w, h, P.ink)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, P.teal2)
    L.vgrad(img, x + 3, y + 3, w - 6, h - 6, { P.sky3, P.sky5 })
    for sy = y + 6, y + h - 4, 4 do L.hline(img, x + 3, x + w - 4, sy, P.glass3) end
    L.rect(img, x + w // 2, y + 3, 1, h - 6, P.teal2)
  end

  local function smallPot(img, x, y)
    L.line(img, x + 4, y, x, y + 7, P.gray3)
    L.line(img, x + 4, y, x + 8, y + 7, P.gray3)
    L.circle(img, x + 2, y + 6, 2, P.green2)
    L.circle(img, x + 6, y + 6, 2, P.green)
    L.poly(img, { x, y + 8, x + 9, y + 8, x + 7, y + 13, x + 2, y + 13 }, P.orange2)
    L.rect(img, x - 1, y + 7, 11, 2, P.orange)
    L.vline(img, x - 1, y + 9, y + 15, P.green2)
    L.vline(img, x + 9, y + 9, y + 14, P.green2)
  end

  -- an open door at the right edge, showing the bright courtyard
  local function openDoor(img, x)
    L.rect(img, x, 3, PW - x, 36, P.ink)
    L.vgrad(img, x + 1, 4, PW - x - 1, 22, { P.sky3, P.sky5 })
    L.rect(img, x + 1, 26, PW - x - 1, 6, P.grass)
    L.rect(img, x + 1, 32, PW - x - 1, 7, P.concrete)
    L.rect(img, x - 4, 3, 4, 36, P.brown)
    L.vline(img, x - 4, 3, 38, P.ink)
  end

  -- A child crouched low with both arms over the head (Duck, Cover, and Hold with no desk).
  -- A 34x28 pixel map with folded knees, a tucked chin, and arms braced over the head.
  local function coverKid()
    local k = L.newImage(34, 28)
    L.map(k, 0, 0, {
      "..................................",
      "..................................",
      "....................aaaaa.........",
      "...................aedddda........",
      ".................aaeeddddea.......",
      "..............aaaeeeeeaaaaa.......",
      ".............aeeeeeeaadddda.......",
      "............aeeeeaaabadeddda......",
      "........aaaaeeeeabbbcaddeddda.....",
      "......aallaaeeeabcccccaadeddda....",
      ".....allkkkaeeeaccccccccadddda....",
      "....alkkkkaeeeeabccbbbbccadddda...",
      "...alkkkkkaeeeabbbbbbbbddadddda...",
      "...akkkkklaiiiabbbbbbddddadddda...",
      "..alkklllliiiiabbbbbdddadaddea....",
      "..allllllliiiiaaaaabddddaaddea....",
      "..allkklllaiiiiaiidaedfdaddeea....",
      "..akkkkkkaaiiiiiiiiiaaaaaddea.....",
      "..kkkkkkaahhhhhaiiiiidddddeaa.....",
      "..akkkkaahhhhhhhaiiiddddeeaa......",
      "...akkaanahhhhhhiaiadddeeaa.......",
      "....aaaannnaahhiaammaaaaa.........",
      "...aaaaaannnnnaannammaa...........",
      "..aooooaaannnnnnnaammma...........",
      ".aohhhhoaannnnnaaammma............",
      ".aohhhhooaaaaaammmmaaa............",
      ".aaaaaaaaaaaaaaaaaa...............",
      "..................................",
    }, KID_KEY)
    return k
  end

  local function speedLines(img, x, ys)
    for _, y in ipairs(ys) do L.hline(img, x, x + 8, y, P.ink2) end
  end

  function M.choicePictures2()
    local pics = {}

    -- Duck and cover your head
    local cover = corridorBackdrop()
    smallJalousie(cover, 4, 4, 26, 20)
    L.blit(cover, coverKid(), 22, 20)
    shakeLines(cover, 60, 6)
    shakeLines(cover, 14, 28)
    pics.choice_cover = cover

    -- Run down the stairs
    local stairs = corridorBackdrop()
    L.rect(stairs, 46, 16, PW - 46, 22, P.wall2)
    L.rect(stairs, 46, 38, PW - 46, PH - 38, P.concrete3)
    for i = 0, 3 do
      local sx, sy = 48 + i * 6, 38 + i * 4
      L.rect(stairs, sx, sy, PW - sx, 4, P.concrete)
      L.hline(stairs, sx, PW - 1, sy, P.white)
    end
    L.line(stairs, 46, 22, 71, 38, P.brown2, 2)
    L.blit(stairs, CH.student(1, false), 20, 4)
    speedLines(stairs, 4, { 16, 23, 30 })
    pics.choice_stairs = stairs

    -- Stand by the window
    local win = corridorBackdrop()
    smallJalousie(win, 4, 3, 34, 32)
    L.blit(win, CH.student(0, true), 36, 6, true)
    shakeLines(win, 40, 2)
    shakeLines(win, 1, 38)
    pics.choice_hall_window = win

    -- Stand under the hanging plants
    local plants = corridorBackdrop()
    L.blit(plants, CH.student(0, false), 22, 9)
    smallPot(plants, 31, -3)
    smallPot(plants, 56, -3)
    shakeLines(plants, 44, 2)
    shakeLines(plants, 68, 2)
    pics.choice_plants = plants

    return pics
  end

  function M.choicePictures3()
    local pics = {}

    -- Walk out with your teacher: the class follows the teacher in a line
    local walk = pictureBackdrop()
    openDoor(walk, 62)
    L.blit(walk, CH.student(3, false), -2, 6)
    L.blit(walk, CH.student(0, true), 16, 6)
    L.blit(walk, M.teacher(1), 34, 0, true)
    pics.choice_walk = walk

    -- Run and push to the door
    local push = pictureBackdrop()
    openDoor(push, 60)
    L.blit(push, CH.student(1, true), 24, 5)
    L.blit(push, CH.student(4, false), 36, 4)
    speedLines(push, 2, { 14, 21, 28 })
    L.px(push, 50, 8, P.yellow2); L.px(push, 52, 6, P.yellow2); L.px(push, 54, 9, P.yellow2)
    L.px(push, 51, 11, P.yellow2)
    pics.choice_push = push

    -- Stay under the desk, though the shaking has stopped and the door is open
    local stay = pictureBackdrop()
    openDoor(stay, 60)
    L.rect(stay, 4, 12, 50, 4, P.brown)
    L.hline(stay, 4, 53, 12, P.brown3)
    L.hline(stay, 4, 53, 16, P.ink)
    L.rect(stay, 6, 17, 3, 30, P.brown2)
    L.rect(stay, 49, 17, 3, 30, P.brown2)
    L.blit(stay, crouchingKid(), 23, 18)
    pics.choice_stay = stay

    return pics
  end

  -- Shared helpers for normal.lua (Earthquake / Normal art), so both files draw alike.
  M.helpers = {
    framed = framed, windowPane = windowPane, books = books, tileFloor = tileFloor, openDoor = openDoor,
    pictureBackdrop = pictureBackdrop, shakeLines = shakeLines, speedLines = speedLines,
    crouchingKid = crouchingKid, coverKid = coverKid, PW = PW, PH = PH,
  }

  return M
end
