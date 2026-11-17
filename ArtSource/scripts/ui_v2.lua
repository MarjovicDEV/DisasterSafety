-- v2 UI pieces in the Student palette (docs/adr/0001-v2-art-and-no-bantay.md):
-- four 16x16 Choice icons for the Action Bar and three 9-slices. The 9-slice borders below
-- must match PixelArtImporter.BorderFor.
--   caption_banner 32x24, border 8   (the Caption at the top of a Level)
--   action_button  32x26, border 9   (one Choice in the Action Bar; 3-pixel bottom lip)
--   speech_bubble  24x24, border 8   (the Student's Reactions)

return function(L, ST, CR)
  local M = {}
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end
  local WOOD = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) }
  local GLASS = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) }
  local GREEN = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) }
  local SUN = rgb(252, 209, 22)

  local function box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end
  -- Clear the 4 corner pixels of a box so it reads rounded.
  local function round(img, x, y, w, h)
    for _, p in ipairs({ { x, y }, { x + w - 1, y }, { x, y + h - 1 }, { x + w - 1, y + h - 1 } }) do
      img:drawPixel(p[1], p[2], L.CLEAR)
    end
  end

  function M.icons()
    local out = {}
    -- door: a wooden door, open a crack, with a green arrow going out
    local d = L.newImage(16, 16)
    box(d, 2, 0, 10, 16, WOOD)
    box(d, 4, 2, 6, 5, GLASS)
    L.px(d, 9, 9, SUN)
    local a = L.newImage(16, 16)
    L.rect(a, 10, 7, 3, 2, GREEN[2])
    L.poly(a, { 12, 4, 16, 8, 12, 12 }, GREEN[2])
    L.hline(a, 10, 12, 7, GREEN[1])
    L.outline(a, O)
    L.blit(d, a, -1, 0)
    out.door = d
    -- desk: a sturdy school desk with room underneath
    local k = L.newImage(16, 16)
    box(k, 0, 3, 16, 4, WOOD)
    box(k, 1, 6, 4, 10, WOOD)
    box(k, 11, 6, 4, 10, WOOD)
    L.rect(k, 4, 12, 8, 2, O)
    out.desk = k
    -- window: a jalousie window with a sill
    local w = L.newImage(16, 16)
    box(w, 1, 0, 14, 14, WOOD)
    L.rect(w, 3, 2, 10, 10, O)
    L.rect(w, 4, 3, 8, 8, GLASS[1])
    for y = 4, 10, 3 do L.hline(w, 4, 11, y, GLASS[3]) end
    L.vline(w, 7, 2, 11, O); L.vline(w, 8, 2, 11, O)
    box(w, 0, 13, 16, 3, WOOD)
    out.window = w
    -- cabinet: a tall cabinet with books on its shelves
    local c = L.newImage(16, 16)
    box(c, 2, 0, 12, 16, WOOD)
    L.rect(c, 4, 2, 8, 12, WOOD[3])
    L.hline(c, 4, 11, 7, O); L.hline(c, 4, 11, 12, O)
    local cols = { rgb(206, 17, 38), K.N, GREEN[2], SUN }
    for i = 0, 3 do
      L.rect(c, 4 + i * 2, 3, 1, 4, cols[i + 1]); L.rect(c, 5 + i * 2, 9, 1, 3, cols[4 - i])
    end
    out.cabinet = c
    return out
  end

  function M.captionBanner()
    local img = L.newImage(32, 24)
    L.rect(img, 0, 0, 32, 24, O)
    L.rect(img, 1, 1, 30, 22, K.N)
    L.hline(img, 1, 30, 1, K.m); L.vline(img, 1, 1, 22, K.m)
    L.rect(img, 3, 3, 26, 18, O)
    L.rect(img, 4, 4, 24, 16, K.W)
    L.rect(img, 4, 18, 24, 2, K.w)
    round(img, 0, 0, 32, 24)
    return img
  end

  function M.actionButton()
    local img = L.newImage(32, 26)
    L.rect(img, 0, 0, 32, 26, O)
    L.rect(img, 1, 1, 30, 24, K.v)             -- lip
    L.rect(img, 1, 1, 30, 21, K.w)             -- face
    L.rect(img, 1, 1, 30, 2, K.W)
    L.vline(img, 1, 1, 21, K.W)
    L.vline(img, 30, 3, 21, K.v)
    round(img, 0, 0, 32, 26)
    return img
  end

  function M.speechBubble()
    local img = L.newImage(24, 24)
    L.rect(img, 1, 0, 22, 24, O)
    L.rect(img, 0, 1, 24, 22, O)
    L.rect(img, 1, 1, 22, 22, K.W)
    L.rect(img, 2, 20, 20, 2, K.w)
    L.vline(img, 21, 3, 21, K.w)
    return img
  end

  return M
end
