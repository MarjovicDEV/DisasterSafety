-- The four v2 How to Play Tutorial Page pictures that show the Action Bar flow (ticket 13). Each is
-- 136x76 art pixels with two frames, shown at 4x like the pilot pages drawn by tutorial.lua, which
-- this script does not change. The pictures use the v2 classroom, the v2 Student frames and the v2
-- UI pieces, so they match what the Learner sees in a Level:
--   v2_actionbar: the boy in the classroom above an Action Bar of three Choice buttons; frame 1
--                 presses the middle button.
--   v2_actout:    the girl under the desk in the Hold pose (both Hold frames).
--   v2_feedback:  the Feedback panel with the "Safe!" heading and a green check, and the boy Happy
--                 beside it; frame 1 adds a sparkle.
--   v2_hint:      the Caption bar with Easy 1's Hint, the HINT button below it, and the girl; frame 1
--                 presses the HINT button.
return function(L, ST, CR, UV)
  local M = {}
  local P = L.P
  local K = ST.KEY
  local O = K.o
  local W, H = 136, 76
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end
  local YELLOW = { rgb(255, 232, 120), rgb(252, 209, 22), rgb(214, 160, 20) }
  local GREEN = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) }

  local room = CR.background()
  local icons = UV.icons()

  -- A 136x76 window of the classroom whose top-left corner is the art point (x, y).
  local function crop(x, y)
    local img = L.newImage(W, H)
    for j = 0, H - 1 do
      for i = 0, W - 1 do img:drawPixel(i, j, room:getPixel(x + i, y + j)) end
    end
    return img
  end

  -- Places a 32x50 Student frame with the centre of its feet at (x, feetRow).
  local function student(img, kind, pose, f, x, feetRow)
    L.blit(img, ST.frames(kind, pose)[f + 1], x - 16, feetRow - 49)
  end

  local function box(img, x, y, w, h, face, lip)
    L.rect(img, x + 1, y, w - 2, h, O)
    L.rect(img, x, y + 1, w, h - 2, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, lip)
    L.rect(img, x + 1, y + 1, w - 2, h - 4, face)
  end

  -- One Action Bar button. A pressed button sits 2 pixels lower on a shorter lip, with a yellow ring.
  local function actionButton(img, x, y, icon, pressed)
    local d = pressed and 2 or 0
    box(img, x, y + d, 40, 19 - d, pressed and K.w or K.W, K.v)
    if pressed then
      L.hline(img, x + 1, x + 38, y + d, YELLOW[2])
      L.hline(img, x + 1, x + 38, y + 16, YELLOW[2])
      L.vline(img, x, y + d + 1, y + 16, YELLOW[2])
      L.vline(img, x + 39, y + d + 1, y + 16, YELLOW[2])
    end
    L.blit(img, icon, x + 12, y + 1 + d)
  end

  local function sparkle(img, cx, cy)
    L.vline(img, cx, cy - 4, cy + 4, YELLOW[2])
    L.hline(img, cx - 4, cx + 4, cy, YELLOW[2])
    L.px(img, cx, cy, P.white)
    L.px(img, cx - 1, cy - 1, YELLOW[1]); L.px(img, cx + 1, cy + 1, YELLOW[1])
    L.px(img, cx + 1, cy - 1, YELLOW[1]); L.px(img, cx - 1, cy + 1, YELLOW[1])
  end

  local function actionBar(f)
    local img = crop(60, 178)
    -- The floor line (art row 230) is row 52 here; the Student stands at the start spot.
    student(img, "boy", "idle", f, 66, 52)
    L.rect(img, 0, 54, W, H - 54, K.n)
    L.hline(img, 0, W - 1, 54, O)
    actionButton(img, 4, 56, icons.door, false)
    actionButton(img, 48, 56, icons.desk, f == 1)
    actionButton(img, 92, 56, icons.window, false)
    return img
  end

  local function actOut(f)
    -- The floor line is row 74. The girl holds the desk leg at x 222, and the desk front is drawn over her.
    local img = crop(154, 156)
    student(img, "girl", "hold", f, 222 - 154, 74)
    L.blit(img, CR.deskFront(), 198 - 154, 188 - 156)
    return img
  end

  local function feedback(f)
    -- A plain light backdrop, like the dimmed room behind the Feedback panel.
    local img = L.newImage(W, H)
    L.rect(img, 0, 0, W, H, K.v)
    L.rect(img, 0, 56, W, H - 56, K.m)
    box(img, 4, 6, 88, 64, K.W, K.v)
    L.circle(img, 17, 20, 7, O)
    L.circle(img, 17, 20, 6, GREEN[2])
    L.line(img, 14, 20, 16, 22, P.white, 2)
    L.line(img, 16, 22, 20, 17, P.white, 2)
    L.text(img, 29, 12, "Safe!", GREEN[3], 2)
    for row, w in ipairs({ 76, 68, 72, 44 }) do
      L.hline(img, 10, 10 + w - 1, 32 + row * 6, K.v)
    end
    student(img, "boy", "happy", f, 114, 74)
    if f == 1 then sparkle(img, 101, 26); sparkle(img, 129, 38) end
    return img
  end

  local function hint(f)
    -- The plain wall and floor beside the classroom door, as in the Action Bar picture.
    local img = crop(60, 178)
    -- The Caption bar across the top, with Easy 1's Hint on two lines.
    L.rect(img, 1, 1, W - 2, 30, O)
    L.rect(img, 2, 2, W - 4, 28, K.N)
    L.rect(img, 4, 4, W - 8, 24, O)
    L.rect(img, 5, 5, W - 10, 22, K.W)
    for i, line in ipairs({ "Find something strong", "to hide under." }) do
      L.text(img, math.floor((W - L.F.width(line, 1)) / 2), 5 + (i - 1) * 10, line, K.N)
    end
    -- The HINT button with its bulb; frame 1 presses it.
    local d = f == 1 and 2 or 0
    local x, y = 26, 38
    L.rect(img, x + 1, y + d, 52, 18 - d, O)
    L.rect(img, x, y + 1 + d, 54, 16 - d, O)
    L.rect(img, x + 1, y + 1 + d, 52, 16 - d, YELLOW[3])
    L.rect(img, x + 1, y + 1 + d, 52, 13 - d, f == 1 and YELLOW[2] or YELLOW[1])
    L.circle(img, x + 9, y + 7 + d, 4, O)
    L.circle(img, x + 9, y + 7 + d, 3, P.white)
    L.rect(img, x + 8, y + 11 + d, 3, 2, K.v)
    L.text(img, x + 17, y + 3 + d, "HINT", O)
    student(img, "girl", "idle", f, 110, 74)
    return img
  end

  M.pages = {
    { name = "v2_actionbar", draw = actionBar },
    { name = "v2_actout", draw = actOut },
    { name = "v2_feedback", draw = feedback },
    { name = "v2_hint", draw = hint },
  }

  return M
end
