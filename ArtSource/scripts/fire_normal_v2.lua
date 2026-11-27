-- Standalone props for the Fire Normal Levels. Actor and room sprites stay separate.
return function(L,kit)
  local M,P={},L.P
  local box=kit.box
  function M.bag()
    local img=L.newImage(18,23)
    box(img,5,1,9,7,kit.NAVY)
    L.rect(img,7,3,5,3,L.CLEAR)
    box(img,1,5,16,17,kit.SAGE)
    kit.round(img,1,5,16,17)
    box(img,3,13,12,7,kit.NAVY)
    L.hline(img,4,13,15,P.gray2)
    L.rect(img,12,15,2,3,P.yellow)
    L.hline(img,5,12,8,P.teal2)
    return img
  end
  function M.cloth()
    local img=L.newImage(8,7)
    L.poly(img,{0,2,5,0,7,2,6,6,3,5,1,6},P.ink)
    L.poly(img,{1,2,5,1,6,2,5,5,3,4,2,5},P.white)
    L.line(img,2,3,4,2,P.blue3)
    return img
  end
  function M.door()
    local img=L.newImage(50,94)
    box(img,0,0,50,94,kit.WOOD)
    box(img,3,3,44,90,kit.SAGE)
    box(img,11,14,28,17,kit.WHITE)
    L.text(img,18,19,"CR",P.navy2,1)
    for y=62,77,4 do
      L.hline(img,10,38,y,P.teal2)
      L.hline(img,10,38,y+1,P.teal3)
    end
    box(img,37,44,7,4,kit.METAL)
    return img
  end
  function M.outlet(f)
    local img=L.newImage(34,30)
    box(img,8,3,18,14,kit.WHITE)
    for i,lead in ipairs({{8,10,3,15,4,25,1,28},{16,13,16,20,10,24,11,29},{24,10,29,15,30,22,33,23}}) do
      for j=1,5,2 do
        L.line(img,lead[j],lead[j+1],lead[j+2],lead[j+3],P.ink,2)
      end
      box(img,lead[1]-3,lead[2]-3,8,7,i==2 and kit.NAVY or kit.METAL)
    end
    local x=({19,22,18})[f+1]
    L.line(img,x-2,1,x,3,P.orange2)
    L.line(img,x,3,x-1,5,P.yellow3)
    L.px(img,x+3,2,P.yellow)
    return img
  end
  function M.switchBox()
    local img=L.newImage(22,29)
    box(img,0,0,22,29,kit.METAL)
    box(img,3,3,16,19,kit.NAVY)
    L.rect(img,7,6,8,12,P.ink)
    box(img,6,8,10,6,kit.RED)
    L.hline(img,7,13,24,P.gray4)
    for _,x in ipairs({2,19}) do L.px(img,x,26,P.gray4) end
    return img
  end
  return M
end
