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
    return out
  end

  return M
end
