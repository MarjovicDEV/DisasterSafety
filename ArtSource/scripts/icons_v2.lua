-- The 16x16 Choice icons for the rollout Levels, in the style of the pilot's four icons in
-- ui_v2.lua (one dark outline, three shades with light from the left, the Student palette).
-- Each Setting slice adds its icons to M.icons(); build.lua saves them as UI/icon_<name>
-- (run with --script-param only=icons_v2). The pilot's icon_door, icon_desk, icon_window and
-- icon_cabinet stay in ui_v2.lua and are reused where a Choice matches.

return function(L, ST)
  local M = {}
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- Colour ramps: light, base, shadow.
  M.WOOD = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) }
  M.GLASS = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) }
  M.GREEN = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) }
  M.RED = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) }
  M.POT = { rgb(232, 140, 96), rgb(200, 104, 70), rgb(156, 72, 52) }   -- the v2 plant pots
  M.NAVY = { K.m, K.N, K.n }
  M.WHITE = { K.W, K.w, K.v }
  M.SKIN = { K.t, K.S, K.s }
  M.SUN = rgb(252, 209, 22)
  M.O = O

  -- An outlined box filled with a ramp: light on the top and left, shadow on the right and bottom.
  function M.box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end

  -- Clears the 4 corner pixels of a box so it reads rounded.
  function M.round(img, x, y, w, h)
    for _, p in ipairs({ { x, y }, { x + w - 1, y }, { x, y + h - 1 }, { x + w - 1, y + h - 1 } }) do
      img:drawPixel(p[1], p[2], L.CLEAR)
    end
  end

  -- The icons by name (without the icon_ prefix). Each slice adds its own.
  function M.icons()
    local out = {}

    -- Easy 2, corridor ----------------------------------------------------------------------
    -- stairs: steps going down to the right, with a handrail
    local s = L.newImage(16, 16)
    -- four steps, each 4 wide and 3 lower than the last: a lit tread, the base, a shaded drop
    for i = 0, 3 do
      local x, top = i * 4, 5 + i * 3
      L.rect(s, x, top, 4, 16 - top, M.WHITE[2])
      L.hline(s, x, x + 3, top, M.WHITE[1])
      L.vline(s, x + 3, top + 1, 15, M.WHITE[3])
    end
    L.line(s, 1, 2, 14, 10, M.WOOD[2])             -- the handrail going down with the steps
    L.line(s, 1, 1, 14, 9, M.WOOD[1])
    L.vline(s, 1, 2, 4, M.WOOD[3])                 -- its post at the top step
    L.outline(s, O)
    out.stairs = s

    -- cover: a Student kneeling low and facing right, one arm over the head and neck
    -- (drawn after the Student's own cover pose, with the Student palette)
    local c = L.newImage(16, 16)
    L.map(c, 0, 3, {
      "..........ooo...",
      "........ootSSo..",
      "......oootSSSso.",
      ".....oWWoHHHHSo.",
      "....oWWwwoHHjSso",
      "...oWWwwvoHtSSo.",
      "...oWwwwvohSSso.",
      "..oWwwwwvvooooo.",
      "..oxxxxxxxo.....",
      "..omNNNNNNno....",
      ".obbBo.obbBo....",
      ".ooooo.ooooo....",
    }, K)
    out.cover = c

    -- plants: a pot hanging on its chains
    local p = L.newImage(16, 16)
    L.vline(p, 8, 0, 4, M.NAVY[3])
    L.line(p, 3, 8, 8, 4, M.NAVY[3]); L.line(p, 13, 8, 8, 4, M.NAVY[3])
    L.circle(p, 5, 7, 2, M.GREEN[2]); L.circle(p, 11, 7, 2, M.GREEN[2]); L.circle(p, 8, 6, 2, M.GREEN[1])
    M.box(p, 2, 9, 12, 3, M.POT)
    L.poly(p, { 3, 12, 12, 12, 11, 15, 4, 15 }, M.POT[2])
    L.vline(p, 4, 12, 14, M.POT[1]); L.vline(p, 11, 12, 14, M.POT[3])
    L.rect(p, 1, 9, 1, 4, M.GREEN[3])
    L.outline(p, O)
    out.plants = p

    -- Easy 3, classroom after the shaking --------------------------------------------------
    -- stay: the Student stays low behind the top and the front legs of the desk.
    local stay = L.newImage(16, 16)
    L.map(stay, 0, 0, {
      "................",
      "................",
      ".oooooooooooooo.",
      ".oAAAAAAAAAAAao.",
      ".oaaaaaaaaaaaao.",
      ".oooooooooooooo.",
      "..oAo..oooo.oao.",
      "..oAo.oHHHHooao.",
      "..oAo.oHjSSooao.",
      "..oAo.oHtSsooao.",
      "..oAo.oWwvo.oao.",
      "..oAooWWwvo.oao.",
      "..oAoomNnoo.oao.",
      "..oAoooooo..oao.",
      "..ooo.......ooo.",
      "................",
    }, setmetatable({ A = M.WOOD[1], a = M.WOOD[2] }, { __index = K }))
    out.stay = stay

    -- walk with the Teacher: the taller figure leads, the Student keeps a step back.
    local walk = L.newImage(16, 16)
    L.map(walk, 0, 0, {
      "..........ooo...",
      ".........oHHHo..",
      ".........oHjSSo.",
      ".........oHtSso.",
      "...ooo...oomno..",
      "..oHHHo..omNno..",
      "..oHtSsoomprno..",
      "..oWtsoo.omNnSo.",
      "..oWwvo..omNnno.",
      "..oWvwSo.omNno..",
      "..omNNno.omNno..",
      "..omNno..oSoo...",
      "...oSoo..oSoSo..",
      "..oSoSo.oBooSo..",
      "..oooBo.oo.oBBo.",
      "....ooo....oooo.",
    }, K)
    out.teacher_walk = walk

    -- push: the Student leans forward with both arms out behind the taller figure.
    local push = L.newImage(16, 16)
    L.map(push, 0, 0, {
      "...........ooo..",
      "..........oHHHo.",
      "..........oHjSSo",
      "..........oHtSso",
      "....ooo...oomno.",
      "...oHHHo..omNno.",
      "...oHtSsoomprno.",
      "....oWwwooNNno..",
      "...oWwwwtSmNno..",
      "...oWvvooomNno..",
      "..omNno..omNno..",
      "..omNno...oSoo..",
      ".omNoNno..oSoSo.",
      ".oBo.oNo.oBooSo.",
      ".oo..oBBo.ooBBo.",
      "......ooo...ooo.",
    }, K)
    out.push = push

    return out
  end

  return M
end
