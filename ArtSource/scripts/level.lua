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

  -- A child crouched low and holding on, drawn on its own so it can be outlined.
  local function crouchingKid()
    local k = L.newImage(34, 22)
    -- legs and shoes
    L.rect(k, 3, 12, 9, 5, P.navy)
    L.rect(k, 2, 16, 7, 3, P.skin1)
    L.rect(k, 1, 19, 7, 2, P.ink)
    -- back and shirt
    L.ellipse(k, 14, 10, 8, 6, P.white)
    for y = 11, 16 do for x = 6, 22 do if L.get(k, x, y) == P.white then L.px(k, x, y, P.gray1) end end end
    L.rect(k, 8, 6, 6, 8, P.red)
    L.rect(k, 8, 6, 6, 2, P.red2)
    -- head tucked low, with hair
    L.circle(k, 24, 9, 6, P.skin1)
    for y = 2, 15 do
      for x = 17, 31 do
        if L.get(k, x, y) == P.skin1 and (y < 8 or x < 22) then L.px(k, x, y, P.hair) end
      end
    end
    L.px(k, 27, 9, P.ink); L.px(k, 27, 10, P.ink)
    L.px(k, 28, 12, P.blush)
    -- arm reaching to the desk leg, hand gripping it
    L.line(k, 18, 12, 30, 16, P.skin1, 2)
    L.rect(k, 30, 14, 3, 4, P.skin1)
    L.outline(k, P.ink)
    return k
  end

  function M.choicePictures()
    local pics = {}

    -- Duck under the desk and hold on
    local desk1 = pictureBackdrop()
    L.rect(desk1, 12, 20, 50, 4, P.brown)
    L.hline(desk1, 12, 61, 20, P.brown3)
    L.hline(desk1, 12, 61, 24, P.ink)
    L.rect(desk1, 14, 25, 3, 21, P.brown2)
    L.rect(desk1, 57, 25, 3, 21, P.brown2)
    L.blit(desk1, crouchingKid(), 25, 25)
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

  return M
end
