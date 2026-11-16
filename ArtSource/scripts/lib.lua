-- Drawing helpers for the Disaster Safety art build.
-- All art is drawn at 1 art pixel = 4 screen pixels at 1920x1080.

local L = {}
local pc = app.pixelColor

L.CLEAR = pc.rgba(0, 0, 0, 0)

function L.hex(h, a)
  h = h:gsub("#", "")
  return pc.rgba(tonumber(h:sub(1, 2), 16), tonumber(h:sub(3, 4), 16), tonumber(h:sub(5, 6), 16), a or 255)
end

-- Shared palette. Names describe use, not hue, where possible.
L.P = {
  ink = L.hex("2b2135"),
  ink2 = L.hex("4a3b52"),
  white = L.hex("fffaf0"),
  paper = L.hex("fff3d6"),
  paper2 = L.hex("f0dcae"),
  paper3 = L.hex("c9a774"),
  gray1 = L.hex("e4e0d8"),
  gray2 = L.hex("b8b2a8"),
  gray3 = L.hex("8a8478"),
  gray4 = L.hex("5e5a54"),
  skin1 = L.hex("f6c9a0"),
  skin2 = L.hex("dc9a6c"),
  skin3 = L.hex("b0704c"),
  blush = L.hex("f08a8a"),
  hair = L.hex("3a2626"),
  hair2 = L.hex("5a3a32"),
  navy = L.hex("34428a"),
  navy2 = L.hex("232c5e"),
  red = L.hex("e04848"),
  red2 = L.hex("a8303a"),
  red3 = L.hex("ff8a7a"),
  orange = L.hex("f5902e"),
  orange2 = L.hex("c8641e"),
  yellow = L.hex("ffd23f"),
  yellow2 = L.hex("e8a317"),
  yellow3 = L.hex("fff0a0"),
  green = L.hex("62c44c"),
  green2 = L.hex("3f9a3c"),
  green3 = L.hex("2a7034"),
  green4 = L.hex("9ee070"),
  teal = L.hex("2f9e95"),
  teal2 = L.hex("1f6f6c"),
  teal3 = L.hex("7fd8cc"),
  blue = L.hex("3f8fe0"),
  blue2 = L.hex("2a62b0"),
  blue3 = L.hex("9cd0ff"),
  purple = L.hex("8a5cc8"),
  purple2 = L.hex("5e3a94"),
  pink = L.hex("ff7aa8"),
  brown = L.hex("9a6440"),
  brown2 = L.hex("6e4228"),
  brown3 = L.hex("c88a5a"),
  tan = L.hex("e2a868"),
  tan2 = L.hex("c08048"),
  cream = L.hex("fff0d4"),
  sky1 = L.hex("4aa6e8"),
  sky2 = L.hex("68bcf0"),
  sky3 = L.hex("8fd2f6"),
  sky4 = L.hex("b8e6fb"),
  sky5 = L.hex("dcf4ff"),
  wall = L.hex("f6e6b8"),
  wall2 = L.hex("e0c890"),
  wall3 = L.hex("c4a86c"),
  roof = L.hex("cc4a3c"),
  roof2 = L.hex("a3382e"),
  roof3 = L.hex("e8705c"),
  glass = L.hex("5ea4cc"),
  glass2 = L.hex("a8dcf2"),
  glass3 = L.hex("3e7aa4"),
  concrete = L.hex("dcd4c4"),
  concrete2 = L.hex("c4baa6"),
  concrete3 = L.hex("a89c86"),
  grass = L.hex("7ccc5a"),
  grass2 = L.hex("64b448"),
  grass3 = L.hex("4e9c3a"),
  mount = L.hex("8fb4cc"),
  mount2 = L.hex("7a9ebc"),
  mount3 = L.hex("b4cfe0"),
  hill = L.hex("8ec89a"),
  hill2 = L.hex("72b480"),
  flagBlue = L.hex("0038a8"),
  flagBlue2 = L.hex("002878"),
  flagRed = L.hex("ce1126"),
  flagRed2 = L.hex("980c1c"),
  flagWhite = L.hex("ffffff"),
  flagWhite2 = L.hex("d8dce8"),
  flagSun = L.hex("fcd116"),
  metal = L.hex("d8dce4"),
  metal2 = L.hex("9aa2b4"),
  water = L.hex("4ab0e8"),
  water2 = L.hex("2a78c0"),
  water3 = L.hex("b0e4ff"),
  fire = L.hex("ff6a2a"),
  fire2 = L.hex("ffb02a"),
  fire3 = L.hex("fff27a"),
  rock = L.hex("9a8a7a"),
  rock2 = L.hex("6e6058"),
  rock3 = L.hex("c4b4a0"),
  dirt = L.hex("b07a4a"),
  dirt2 = L.hex("84562e"),
}

-- Deterministic random numbers so every build draws the same art.
local seed = 1234567
function L.seed(s) seed = s end
function L.rand()
  seed = (seed * 1103515245 + 12345) % 2147483648
  return seed / 2147483648
end
function L.randi(a, b) return a + math.floor(L.rand() * (b - a + 1)) end

function L.newImage(w, h) return Image(w, h, ColorMode.RGB) end

function L.inside(img, x, y) return x >= 0 and y >= 0 and x < img.width and y < img.height end

function L.px(img, x, y, c)
  x = math.floor(x); y = math.floor(y)
  if L.inside(img, x, y) then img:drawPixel(x, y, c) end
end

function L.get(img, x, y)
  if not L.inside(img, x, y) then return L.CLEAR end
  return img:getPixel(x, y)
end

function L.opaque(img, x, y) return pc.rgbaA(L.get(img, x, y)) > 0 end

function L.rect(img, x, y, w, h, c)
  for j = y, y + h - 1 do for i = x, x + w - 1 do L.px(img, i, j, c) end end
end

function L.hline(img, x0, x1, y, c) for i = x0, x1 do L.px(img, i, y, c) end end
function L.vline(img, x, y0, y1, c) for j = y0, y1 do L.px(img, x, j, c) end end

function L.ellipse(img, cx, cy, rx, ry, c)
  for j = math.floor(cy - ry), math.ceil(cy + ry) do
    for i = math.floor(cx - rx), math.ceil(cx + rx) do
      local dx = (i - cx) / (rx + 0.35)
      local dy = (j - cy) / (ry + 0.35)
      if dx * dx + dy * dy <= 1 then L.px(img, i, j, c) end
    end
  end
end

function L.circle(img, cx, cy, r, c) L.ellipse(img, cx, cy, r, r, c) end

function L.line(img, x0, y0, x1, y1, c, thick)
  x0, y0, x1, y1 = math.floor(x0), math.floor(y0), math.floor(x1), math.floor(y1)
  local dx, dy = math.abs(x1 - x0), -math.abs(y1 - y0)
  local sx = x0 < x1 and 1 or -1
  local sy = y0 < y1 and 1 or -1
  local err = dx + dy
  while true do
    if thick and thick > 1 then L.rect(img, x0, y0, thick, thick, c) else L.px(img, x0, y0, c) end
    if x0 == x1 and y0 == y1 then break end
    local e2 = 2 * err
    if e2 >= dy then err = err + dy; x0 = x0 + sx end
    if e2 <= dx then err = err + dx; y0 = y0 + sy end
  end
end

-- Filled polygon, points as {x1,y1,x2,y2,...}
function L.poly(img, pts, c)
  local minY, maxY = math.huge, -math.huge
  for i = 2, #pts, 2 do minY = math.min(minY, pts[i]); maxY = math.max(maxY, pts[i]) end
  local n = #pts // 2
  for y = math.floor(minY), math.ceil(maxY) do
    local yc = y + 0.5
    local xs = {}
    for i = 0, n - 1 do
      local x1, y1 = pts[i * 2 + 1], pts[i * 2 + 2]
      local j = (i + 1) % n
      local x2, y2 = pts[j * 2 + 1], pts[j * 2 + 2]
      if (y1 <= yc and y2 > yc) or (y2 <= yc and y1 > yc) then
        xs[#xs + 1] = x1 + (yc - y1) / (y2 - y1) * (x2 - x1)
      end
    end
    table.sort(xs)
    for k = 1, #xs - 1, 2 do
      for x = math.floor(xs[k] + 0.5), math.floor(xs[k + 1] - 0.5) do L.px(img, x, y, c) end
    end
  end
end

local BAYER = { 0, 8, 2, 10, 12, 4, 14, 6, 3, 11, 1, 9, 15, 7, 13, 5 }
function L.bayer(x, y) return (BAYER[(y % 4) * 4 + (x % 4) + 1] + 0.5) / 16 end

-- Vertical dithered gradient across a list of colours.
function L.vgrad(img, x, y, w, h, colors)
  local n = #colors
  for j = 0, h - 1 do
    local t = (h > 1) and (j / (h - 1)) * (n - 1) or 0
    local idx = math.floor(t)
    local frac = t - idx
    if idx >= n - 1 then idx = n - 2; frac = 1 end
    for i = 0, w - 1 do
      local c = (frac > L.bayer(x + i, y + j)) and colors[idx + 2] or colors[idx + 1]
      L.px(img, x + i, y + j, c)
    end
  end
end

-- Replace colour a with b inside a rectangle (used for recolours).
function L.recolor(img, map)
  for y = 0, img.height - 1 do
    for x = 0, img.width - 1 do
      local c = img:getPixel(x, y)
      if map[c] then img:drawPixel(x, y, map[c]) end
    end
  end
end

-- Add a 1 px outline around every opaque pixel (4-neighbour).
function L.outline(img, c, diagonal)
  local marks = {}
  for y = 0, img.height - 1 do
    for x = 0, img.width - 1 do
      if not L.opaque(img, x, y) then
        local hit = L.opaque(img, x - 1, y) or L.opaque(img, x + 1, y) or L.opaque(img, x, y - 1) or L.opaque(img, x, y + 1)
        if not hit and diagonal then
          hit = L.opaque(img, x - 1, y - 1) or L.opaque(img, x + 1, y - 1) or L.opaque(img, x - 1, y + 1) or L.opaque(img, x + 1, y + 1)
        end
        if hit then marks[#marks + 1] = { x, y } end
      end
    end
  end
  for _, m in ipairs(marks) do img:drawPixel(m[1], m[2], c) end
end

-- Draw an ASCII map. key maps characters to colours; others are skipped.
function L.map(img, x, y, rows, key)
  for j, row in ipairs(rows) do
    for i = 1, #row do
      local c = key[row:sub(i, i)]
      if c then L.px(img, x + i - 1, y + j - 1, c) end
    end
  end
end

-- Copy src onto dst at (x, y), skipping transparent pixels.
function L.blit(dst, src, x, y, flipX)
  for j = 0, src.height - 1 do
    for i = 0, src.width - 1 do
      local c = src:getPixel(i, j)
      if pc.rgbaA(c) > 0 then
        local dx = flipX and (src.width - 1 - i) or i
        L.px(dst, x + dx, y + j, c)
      end
    end
  end
end

-- Text -----------------------------------------------------------------
L.F = nil -- set by build.lua after loading font.lua

function L.text(img, x, y, s, c, scale)
  L.F.draw(s, x, y, scale or 1, function(px, py) L.px(img, px, py, c) end)
end

-- Text with a two-colour vertical fill, outline and drop shadow.
function L.fancyText(img, x, y, s, scale, top, bottom, outline, shadow)
  local tmp = L.newImage(img.width, img.height)
  L.F.draw(s, x, y, scale, function(px, py, row)
    L.px(tmp, px, py, row <= 4 and top or bottom)
  end)
  -- highlight on the top row of every stroke
  L.F.draw(s, x, y, scale, function(px, py, row)
    if row == 1 and (py - y) % scale == 0 then L.px(tmp, px, py, L.P.white) end
  end)
  L.outline(tmp, outline, true)
  L.outline(tmp, outline, false)
  if shadow then
    for j = img.height - 1, 0, -1 do
      for i = 0, img.width - 1 do
        if L.opaque(tmp, i, j) then L.px(img, i, j + shadow, outline) end
      end
    end
  end
  L.blit(img, tmp, 0, 0)
end

-- Export helpers -------------------------------------------------------
L.ROOT = nil   -- project root, set by build.lua
L.made = {}

local function ensureDir(path)
  local dir = app.fs.filePath(path)
  if not app.fs.isDirectory(dir) then app.fs.makeAllDirectories(dir) end
end

-- Save a single image as a one-frame sprite source and as a PNG.
function L.save(img, name, pngRel)
  local spr = Sprite(img.width, img.height, ColorMode.RGB)
  spr.cels[1].image = img
  local src = L.ROOT .. "/ArtSource/" .. name .. ".aseprite"
  ensureDir(src)
  spr:saveAs(src)
  local png = L.ROOT .. "/Assets/Art/" .. pngRel .. ".png"
  ensureDir(png)
  img:saveAs(png)
  spr:close()
  L.made[#L.made + 1] = pngRel
end

-- Save frames as one animated source and one PNG per frame (name_0.png ...).
function L.saveFrames(frames, name, pngRelBase, durationMs)
  local w, h = frames[1].width, frames[1].height
  local spr = Sprite(w, h, ColorMode.RGB)
  spr.cels[1].image = frames[1]
  spr.frames[1].duration = (durationMs or 120) / 1000
  for i = 2, #frames do
    local fr = spr:newEmptyFrame()
    fr.duration = (durationMs or 120) / 1000
    spr:newCel(spr.layers[1], i, frames[i], Point(0, 0))
  end
  local src = L.ROOT .. "/ArtSource/" .. name .. ".aseprite"
  ensureDir(src)
  spr:saveAs(src)
  for i, f in ipairs(frames) do
    local png = L.ROOT .. "/Assets/Art/" .. pngRelBase .. "_" .. (i - 1) .. ".png"
    ensureDir(png)
    f:saveAs(png)
    L.made[#L.made + 1] = pngRelBase .. "_" .. (i - 1)
  end
  spr:close()
end

return L
