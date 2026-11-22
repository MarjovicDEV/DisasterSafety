-- The pilot classroom after the shaking (480x270, 4x at 1920x1080).
-- Keep the desk and floor line in place so the Student can start where Easy 1 ended.

return function(L, ST, CR, IC)
  local M = {}
  local O = ST.KEY.o
  local wood, navy, leaf, glass, paper = IC.WOOD, IC.NAVY, IC.GREEN, IC.GLASS, IC.WHITE

  local function drawChairs(img)
    -- An upright chair slid back. The second has tipped over, clear of the route.
    local x, y = 302, 187
    local floorShadow = CR.FLOOR[3]
    L.ellipse(img, x + 12, y + 42, 17, 2, floorShadow)
    IC.box(img, x, y, 5, 42, navy)
    IC.box(img, x, y + 4, 22, 5, navy)
    IC.box(img, x, y + 22, 24, 5, wood)
    IC.box(img, x + 19, y + 26, 5, 16, navy)

    L.ellipse(img, 370, 233, 25, 3, floorShadow)
    local fallen = L.newImage(54, 24)
    L.line(fallen, 6, 5, 39, 15, navy[3], 3)
    L.line(fallen, 6, 5, 39, 15, navy[1])
    L.line(fallen, 14, 14, 48, 19, navy[2], 2)
    L.line(fallen, 38, 13, 46, 8, navy[3], 2)
    L.line(fallen, 37, 13, 45, 8, navy[1])
    L.line(fallen, 44, 19, 51, 14, navy[2], 2)
    L.poly(fallen, { 5, 4, 12, 2, 21, 11, 14, 15 }, navy[2])
    L.line(fallen, 5, 4, 14, 14, navy[1])
    L.line(fallen, 12, 2, 21, 11, navy[3])
    L.poly(fallen, { 22, 10, 35, 8, 44, 15, 28, 17 }, wood[2])
    L.line(fallen, 22, 10, 35, 8, wood[1])
    L.line(fallen, 28, 17, 43, 15, wood[3])
    L.outline(fallen, O)
    L.blit(img, fallen, 340, 212)
  end

  local function doorLight(img)
    -- Daylight from the open door brightens the concrete and its flecks.
    local light, base, shadow = table.unpack(CR.FLOOR)
    local wallBase = img:getPixel(1, 32)
    local ramp = { [light] = wallBase, [base] = light, [shadow] = base }
    for y = 207, 238 do
      local depth = y - 207
      local startX = 25 + depth // 3
      local endX = 72 + depth + depth // 3
      for x = startX, endX do
        local lit = ramp[img:getPixel(x, y)]
        if lit then img:drawPixel(x, y, lit) end
      end
    end
  end

  local function openDoor(img)
    IC.box(img, 16, 106, 62, 98, wood)
    L.rect(img, 21, 111, 52, 92, O)
    L.rect(img, 23, 112, 48, 91, glass[1])
    L.poly(img, { 23, 159, 36, 151, 51, 155, 70, 150, 70, 181, 23, 181 }, leaf[3])
    L.poly(img, { 23, 167, 41, 160, 56, 163, 70, 157, 70, 181, 23, 181 }, leaf[2])
    L.hline(img, 23, 70, 181, leaf[1])

    -- Daylight on the path and the bevel down from the threshold.
    L.rect(img, 23, 182, 48, 21, paper[2])
    L.poly(img, { 27, 182, 54, 182, 70, 202, 25, 202 }, paper[1])
    L.poly(img, { 23, 203, 70, 203, 76, 211, 19, 211 }, paper[3])
    L.hline(img, 20, 75, 210, paper[2])
    L.vline(img, 71, 112, 202, wood[3])

    -- The door leaf swings left into the room. The opening stays wide enough to read
    -- behind the Teacher, with the EXIT sign in the same spot as the pilot.
    L.poly(img, { 7, 104, 26, 113, 26, 202, 7, 216 }, O)
    L.poly(img, { 8, 106, 25, 114, 25, 201, 8, 214 }, wood[2])
    L.line(img, 8, 106, 8, 214, wood[1])
    L.line(img, 24, 114, 24, 201, wood[3])
    L.poly(img, { 11, 121, 22, 125, 22, 143, 11, 143 }, wood[3])
    L.poly(img, { 12, 122, 21, 126, 21, 142, 12, 142 }, glass[2])
    L.line(img, 12, 140, 19, 128, glass[1])
    L.poly(img, { 12, 166, 22, 166, 22, 191, 12, 202 }, wood[3])
    L.poly(img, { 13, 167, 21, 167, 21, 190, 13, 199 }, wood[2])
    L.line(img, 13, 167, 13, 199, wood[1])
    IC.box(img, 10, 158, 5, 5, { IC.SUN, IC.SUN, wood[3] })
  end

  local function sheet(img, x, y, tilt)
    L.poly(img, { x + 1, y + 1, x + 12, y + tilt + 1, x + 15, y + tilt + 6, x + 2, y + 6 }, paper[3])
    L.poly(img, { x, y, x + 11, y + tilt, x + 14, y + tilt + 5, x + 1, y + 5 }, paper[1])
    L.line(img, x + 2, y + 4, x + 12, y + tilt + 4, paper[2])
    L.line(img, x + 3, y + 1, x + 9, y + tilt + 1, paper[3])
  end

  function M.background()
    local img = CR.background(true)
    doorLight(img)
    openDoor(img)
    drawChairs(img)
    L.blit(img, CR.fanFrames()[1], CR.LAYOUT.fan[1], CR.LAYOUT.fan[2])
    L.blit(img, CR.cabinetFrames()[1], CR.LAYOUT.cabinet[1], CR.LAYOUT.cabinet[2])
    sheet(img, 122, 216, 2)
    sheet(img, 154, 230, -2)
    sheet(img, 325, 224, 2)
    sheet(img, 398, 232, -2)
    return img
  end

  function M.deskFront()
    return CR.deskFront()
  end

  return M
end
