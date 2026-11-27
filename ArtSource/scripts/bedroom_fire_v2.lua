-- Fire-only overlays on the existing bedroom. Coordinates in the review are top-left.
return function(L,kit)
  local M,P={},L.P
  local box=kit.box
  function M.door(f)
    local img=L.newImage(58,107)
    box(img,0,0,58,107,kit.WOOD)
    if f==0 then
      box(img,3,3,52,102,kit.WOOD)
      for _,y in ipairs({12,57}) do box(img,9,y,39,34,kit.WOOD) end
      box(img,44,48,6,5,kit.METAL)
    else
      L.rect(img,3,3,52,102,P.paper2)
      L.rect(img,3,68,52,16,P.teal3)
      L.rect(img,3,84,52,21,P.gray2)
      L.hline(img,3,54,84,P.gray3)
      L.poly(img,{3,3,19,13,19,96,3,104},P.ink)
      L.poly(img,{4,5,17,14,17,95,4,101},P.brown)
      L.line(img,6,13,6,94,P.brown3)
      L.rect(img,13,51,3,4,P.gray1)
    end
    return img
  end
  function M.closet()
    local img=L.newImage(46,92)
    box(img,0,0,46,92,kit.WOOD)
    for _,x in ipairs({2,23}) do
      box(img,x,2,21,86,kit.WOOD)
      for y=10,34,4 do
        L.hline(img,x+4,x+16,y,P.brown2)
        L.hline(img,x+4,x+16,y+1,P.brown3)
      end
    end
    box(img,17,42,4,7,kit.METAL)
    box(img,25,42,4,7,kit.METAL)
    return img
  end
  function M.toy()
    local img=L.newImage(16,18)
    for _,xy in ipairs({{3,3},{12,3},{4,14},{11,14}}) do
      L.circle(img,xy[1],xy[2],2,P.brown)
    end
    L.ellipse(img,8,12,4,4,P.tan)
    L.ellipse(img,8,7,6,5,P.brown3)
    L.ellipse(img,8,9,3,2,P.tan)
    L.px(img,5,6,P.ink)
    L.px(img,10,6,P.ink)
    L.hline(img,7,8,8,P.brown2)
    L.outline(img,P.ink)
    return img
  end
  function M.blanket(gap)
    local w,h=gap and 55 or 31,gap and 6 or 15
    local img=L.newImage(w,h)
    L.poly(img,{1,h-4,4,2,w-5,1,w-2,3,w-2,h-2,5,h-2},P.teal)
    L.hline(img,5,w-6,2,P.teal3)
    for x=6,w-5,8 do L.line(img,x,3,x-2,h-3,P.teal2) end
    if not gap then L.hline(img,4,w-4,10,P.teal3) end
    L.outline(img,P.ink)
    return img
  end
  function M.cloth()
    local img=L.newImage(17,13)
    L.poly(img,{1,4,7,1,15,3,14,11,8,9,3,11},P.red)
    L.line(img,2,4,7,2,P.red3)
    L.line(img,8,3,7,8,P.red2)
    L.outline(img,P.ink)
    return img
  end
  function M.window()
    local img=L.newImage(68,86)
    box(img,0,0,68,86,kit.WOOD)
    L.rect(img,2,2,64,82,P.sky3)
    L.circle(img,48,18,5,P.yellow3)
    L.poly(img,{2,62,12,52,25,59,44,46,65,57,65,83,2,83},P.grass2)
    L.rect(img,2,70,64,14,P.grass)
    for x=6,62,8 do
      L.rect(img,x,62,4,12,P.brown3)
      L.hline(img,x,x+3,62,P.tan)
    end
    L.hline(img,3,64,67,P.brown)
    for _,x in ipairs({2,57}) do
      box(img,x,3,9,80,kit.WOOD)
      box(img,x+2,6,5,73,kit.GLASS)
    end
    return img
  end
  return M
end
