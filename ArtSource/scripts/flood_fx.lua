-- Flood Episode art, Batch 1 (docs/design/flood-episode.md, Normal 2 and decision F2).
-- Floodwater seeping in under the closed home door in three steps, the stand fan and its low
-- outlet, and the home window with low floodwater in the yard. Indoors, Floodwater never
-- fills the room: it spreads only over the floor band in front of the door.
return function(L, root)
  local M, P = {}, L.P
  local pc = app.pixelColor

  -- Floodwater shades: muddy brown-grey with a pale grey-blue sheen, so it reads as water
  -- and not as a shadow, a rug or mud on the concrete floor.
  local WATER = {
    light = L.hex("a8916a"),
    mid = L.hex("8c7452"),
    dark = L.hex("66523c"),
    sheen = L.hex("b7c6cc"),
    glint = P.white,
  }
  M.WATER = WATER

  -- The seep layer is 120x30, placed with its top left at (0,208) on bg_home.
  -- The door sill covers rows 208 to 211 between x 16 and 72, so water there starts at row 212.
  M.SEEP_X, M.SEEP_Y = 0, 208
  local STEPS = {
    { rx = 30, ry = 5 },  -- a thin tongue of water under the door
    { rx = 44, ry = 13 }, -- spreading over the back of the floor
    { rx = 56, ry = 21 }, -- the furthest step: front edge about x 103 at the wall, dry beyond
  }
  M.SEEP_STEPS = STEPS
  local CX, CY = 44, 3 -- centre of the spread, under the middle of the door (art row 211)

  local function seepMask(step)
    local s = STEPS[step]
    local mask = {}
    for y = 0, 29 do
      mask[y] = {}
      local wob = 1 + 0.07 * math.sin(y * 1.3 + step * 0.9)
      for x = 0, 119 do
        local dx = (x - CX) / (s.rx * wob + 0.35)
        local dy = (math.max(y, CY) - CY) / (s.ry + 0.35)
        local inside = dx * dx + dy * dy <= 1
        if x >= 16 and x <= 72 then
          inside = inside and y >= 4 -- under the sill only from row 212
        else
          inside = inside and y >= 0
        end
        mask[y][x] = inside
      end
    end
    return mask
  end

  function M.seep(step, f)
    local img = L.newImage(120, 30)
    local mask = seepMask(step)
    local function at(x, y) return mask[y] ~= nil and mask[y][x] == true end
    for y = 0, 29 do
      for x = 0, 119 do
        if at(x, y) then
          -- Edge pixels get the dark rim. The rows against the wall and the sill are not edges.
          local edge = not at(x, y + 1) or (x > 0 and not at(x - 1, y)) or not at(x + 1, y)
          local c = WATER.mid
          if edge then
            c = WATER.dark
          else
            -- The back of the water mirrors the pale wall and fades to the mid shade by row 9,
            -- dithered, and a little further on the lit left side.
            local t = (y - 2) / 7 - (x < CX and 0.15 or 0)
            if t < L.bayer(x, y) then c = WATER.light end
          end
          L.px(img, x, y, c)
        end
      end
    end
    -- A dark line where the water comes out under the door.
    for x = 20, 68 do if at(x, 4) then L.px(img, x, 4, WATER.dark) end end
    -- Broken reflections below the sill. Keep each stroke inside the dark rim.
    local function reflection(x, y, length, bright)
      for i = 0, length - 1 do
        local xx = x + i
        if at(xx - 1, y) and at(xx + 1, y) and at(xx, y - 1) and at(xx, y + 1) then
          L.px(img, xx, y, bright and i < 2 and WATER.glint or WATER.sheen)
        end
      end
    end
    reflection(42 + f, 6, 17, true)
    if step >= 2 then
      reflection(18 - f, 10, 17, false)
      reflection(56 + f, 12, 12, false)
    end
    if step == 3 then
      reflection(73 - f, 8, 13, false)
      reflection(32 + f, 18, 18, true)
      reflection(12 - f, 16, 8, false)
    end
    -- Small ripple rings by the door gap.
    local rx = ({ 30, 40, 36 })[step]
    local ry = 6 + f
    if at(rx - 4, ry) and at(rx + 4, ry) and at(rx - 4, ry + 2) and at(rx + 4, ry + 2) then
      L.hline(img, rx - 2, rx + 2, ry, WATER.sheen)
      L.px(img, rx - 3, ry + 1, WATER.sheen)
      L.px(img, rx + 3, ry + 1, WATER.sheen)
      L.px(img, rx + 1 - f, ry - 1, WATER.glint)
    end
    return img
  end

  -- The stand fan (22x40). Its base sits on the back of the floor: top left (89,173),
  -- bottom row 212.
  M.FAN_X, M.FAN_Y = 89, 173
  function M.fan(f)
    local img = L.newImage(22, 40)
    -- base and pole
    L.ellipse(img, 11, 36, 7, 1.5, P.metal2)
    L.hline(img, 6, 15, 35, P.metal)
    L.rect(img, 10, 17, 3, 18, P.metal2)
    L.vline(img, 10, 17, 34, P.metal)
    -- head
    L.circle(img, 11, 9, 8, P.blue3)
    L.circle(img, 11, 9, 6, P.sky4)
    local a0 = f * math.pi / 3
    for b = 0, 2 do
      local a = a0 + b * 2 * math.pi / 3
      local x1, y1 = 11 + math.cos(a) * 6, 9 + math.sin(a) * 6
      local x2, y2 = 11 + math.cos(a + 0.6) * 5, 9 + math.sin(a + 0.6) * 5
      L.poly(img, { 11, 9, x1, y1, x2, y2 }, P.blue2)
      L.line(img, 11, 9, x1, y1, P.navy)
    end
    L.circle(img, 11, 9, 1.5, P.white)
    L.px(img, 11, 9, P.blue2)
    -- guard rings, lit from the left
    for _, r in ipairs({ 4, 7 }) do
      for t = 0, 35 do
        if t % 3 == 0 then
          local ang = t * math.pi / 18
          L.px(img, 11 + math.cos(ang) * r, 9 + math.sin(ang) * r, P.metal2)
        end
      end
    end
    L.px(img, 5, 5, P.white)
    L.px(img, 6, 4, P.white)
    L.outline(img, P.ink)
    return img
  end

  -- The low outlet on the wall between the door and the meeting board, with the fan's plug in
  -- it and the cord running down and along the back of the floor to the fan base
  -- (24x26, top left (72,188)).
  M.OUTLET_X, M.OUTLET_Y = 72, 188
  function M.outletLow()
    local img = L.newImage(24, 26)
    -- cord first, so the plate and plug sit on top
    L.vline(img, 7, 9, 20, P.ink2)
    L.line(img, 7, 20, 10, 23, P.ink2)
    L.hline(img, 10, 22, 23, P.ink2)
    L.hline(img, 11, 22, 24, P.ink)
    -- plate and plug, outlined on their own so the cord stays thin
    local part = L.newImage(24, 26)
    L.rect(part, 4, 2, 7, 10, P.white)
    L.vline(part, 10, 2, 11, P.gray2)
    L.hline(part, 4, 10, 11, P.gray2)
    L.px(part, 6, 4, P.ink2)
    L.px(part, 8, 4, P.ink2)
    L.rect(part, 5, 6, 5, 4, P.navy)
    L.hline(part, 5, 9, 6, P.blue2)
    L.vline(part, 9, 6, 9, P.navy2)
    L.outline(part, P.ink)
    L.blit(img, part, 0, 0)
    return img
  end

  -- The home window view (60x65, top left (304,72)) with low floodwater over the yard grass.
  -- It is the approved Typhoon storm view, frame for frame, with the lower grass under water.
  local GREENS = { "2a7034", "4e9c3a", "64b448", "3f9a3c", "62c44c", "9ee070", "7ccc5a" }
  local green = {}
  for _, h in ipairs(GREENS) do green[L.hex(h)] = true end
  M.WINDOW_X, M.WINDOW_Y, M.WATERLINE = 304, 72, 53
  function M.windowFlood(i, line)
    line = line or M.WATERLINE
    local src = Image { fromFile = root .. "/Assets/Art/Level/v2/home_window_storm_" .. i .. ".png" }
    local img = L.newImage(src.width, src.height)
    img:drawImage(src, Point(0, 0))
    for y = line, img.height - 1 do
      for x = 0, img.width - 1 do
        if green[img:getPixel(x, y)] then
          local w = WATER.mid
          if y == line then
            w = WATER.dark
          elseif (x + y * 3 + i * 2) % 11 == 0 then
            w = WATER.sheen
          elseif x < 26 then
            w = WATER.light
          end
          img:drawPixel(x, y, w)
        end
      end
    end
    return img
  end

  return M
end
