-- Filipino kitchen. Actor feet use y=230. The stove is set against the back wall.
return function(L, kit)
  local M,P={},L.P
  local box=kit.box
  function M.background()
    local img=L.newImage(480,270)
    L.rect(img,0,0,480,270,P.paper)
    box(img,-2,-2,484,15,kit.WOOD)
    L.rect(img,0,141,480,65,P.teal3)
    for x=0,479,17 do L.vline(img,x,141,204,P.paper2) end
    for y=141,205,16 do L.hline(img,0,479,y,P.paper2) end
    L.rect(img,0,206,480,64,P.gray2)
    L.hline(img,0,479,206,P.gray4)
    for x=-40,520,64 do L.line(img,x,207,x+(x-240)//5,269,P.gray3) end
    L.hline(img,0,479,236,P.gray3)
    -- Screen door, trees visible through the mesh.
    box(img,24,92,62,117,kit.WOOD)
    box(img,29,96,52,106,kit.SAGE)
    L.rect(img,33,100,44,93,P.sky3)
    L.poly(img,{33,154,44,145,56,151,68,137,76,140,76,192,33,192},P.green3)
    for x=35,76,4 do L.vline(img,x,100,192,kit.SAGE[3]) end
    for y=103,192,5 do L.hline(img,33,76,y,kit.SAGE[3]) end
    box(img,30,151,50,5,kit.WOOD)
    box(img,71,155,7,4,kit.METAL)
    -- Slatted window above the sink.
    box(img,111,66,102,67,kit.WOOD)
    box(img,115,70,94,59,kit.GLASS)
    for y=77,127,9 do
      L.hline(img,117,206,y,kit.GLASS[3])
      L.hline(img,117,206,y-1,kit.WHITE[1])
    end
    box(img,160,70,4,59,kit.WOOD)
    -- Open shelves with enamel bowls, jars and stacked plates.
    for row=0,1 do
      local y=83+row*36
      box(img,281,y,161,5,kit.WOOD)
      for j=0,3 do
        local x=291+j*15
        box(img,x,y-14+j%2*3,10,14-j%2*3,j%2==0 and kit.GLASS or kit.POT)
        box(img,x-1,y-15+j%2*3,12,4,kit.METAL)
      end
      for k=0,2 do
        L.ellipse(img,403,y-3-k*3,17,3,P.ink)
        L.ellipse(img,403,y-4-k*3,15,2,k==2 and P.paper or P.gray2)
      end
      L.poly(img,{356,y-9,380,y-9,377,y-2,373,y-1,363,y-1},P.ink)
      L.poly(img,{358,y-8,378,y-8,375,y-3,363,y-3},P.teal)
    end
    -- Counter, sink and two-burner cooker. Open bay contains the LPG tank.
    box(img,108,166,338,42,kit.WOOD)
    for _,x in ipairs({112,159,206,253}) do
      box(img,x,171,44,33,kit.SAGE)
      box(img,x+30,176,9,3,kit.METAL)
    end
    L.rect(img,311,171,129,34,P.brown2)
    box(img,105,162,344,7,kit.METAL)
    L.ellipse(img,160,164,26,2,P.gray4)
    L.ellipse(img,160,163,22,1,P.gray2)
    L.line(img,141,160,141,148,P.ink,3)
    L.line(img,141,148,151,148,P.ink,3)
    L.line(img,151,148,151,153,P.ink,3)
    L.line(img,140,159,140,148,P.gray1)
    L.hline(img,140,150,147,P.gray1)
    box(img,309,161,63,47,kit.METAL)
    L.rect(img,311,163,59,13,kit.METAL[1])
    L.hline(img,310,370,176,kit.METAL[3])
    box(img,312,189,56,15,kit.NAVY)
    L.hline(img,318,361,192,P.gray2)
    for _,x in ipairs({325,355}) do
      L.ellipse(img,x,174,13,3,P.ink)
      L.hline(img,x-11,x+10,173,P.gray3)
    end
    -- End controls leave a clear standing spot to the left of the oven.
    for _,x in ipairs({310,365}) do
      L.circle(img,x,189,3,P.ink)
      L.circle(img,x,189,2,P.gray4)
      L.px(img,x-1,188,P.paper)
    end
    -- Leave open space behind the hose so its two fittings read at game scale.
    local hose={372,179,378,180,383,185,388,187,393,186,398,180,405,177}
    for i=1,#hose-2,2 do
      L.line(img,hose[i],hose[i+1],hose[i+2],hose[i+3],P.ink,4)
    end
    for i=1,#hose-2,2 do
      L.line(img,hose[i]+1,hose[i+1]+1,hose[i+2]+1,hose[i+3]+1,P.orange2,2)
    end
    box(img,370,177,7,6,kit.METAL)
    box(img,402,175,8,6,kit.METAL)
    L.ellipse(img,415,185,13,7,P.ink)
    L.rect(img,402,185,27,16,P.ink)
    L.ellipse(img,415,200,13,3,P.ink)
    L.ellipse(img,415,185,12,6,P.teal)
    L.rect(img,403,185,24,15,P.teal)
    L.vline(img,406,185,197,P.teal3)
    L.rect(img,422,186,5,14,P.teal2)
    box(img,405,201,21,4,kit.METAL)
    box(img,409,173,12,7,kit.METAL)
    box(img,412,170,6,4,kit.POT)
    return img
  end
  function M.pot(f)
    local img=L.newImage(28,20)
    box(img,4,6,20,13,kit.METAL)
    for _,x in ipairs({0,23}) do box(img,x,8,5,4,kit.METAL) end
    L.ellipse(img,14,6,10,3,P.ink)
    L.ellipse(img,14,6,8,2,P.paper2)
    for i=0,2 do
      local x=7+i*6
      L.circle(img,x,5-(i+f)%2*2,2,P.white)
    end
    L.line(img,21,6,22,10+f*3,P.white,2)
    return img
  end
  function M.burner(f)
    local img=L.newImage(22,6)
    for x=2,20,4 do
      L.poly(img,{x-1,4,x,1+(x//4+f)%2,x+1,3,x+1,4},P.blue)
      L.px(img,x,4,P.blue3)
    end
    L.outline(img,P.navy2)
    return img
  end
  function M.pan()
    local img=L.newImage(37,10)
    box(img,0,2,17,4,kit.WOOD)
    L.ellipse(img,24,5,11,4,P.ink)
    L.ellipse(img,24,4,10,3,P.gray3)
    L.ellipse(img,24,3,9,2,P.gray4)
    return img
  end
  function M.lid()
    local img=L.newImage(24,9)
    L.ellipse(img,12,6,11,2,P.ink)
    L.ellipse(img,12,5,9,2,P.gray2)
    L.hline(img,5,14,5,P.gray1)
    box(img,9,1,6,4,kit.WOOD)
    return img
  end
  function M.cup()
    local img=L.newImage(11,12)
    box(img,6,3,5,6,kit.GLASS)
    box(img,1,1,7,10,kit.GLASS)
    L.hline(img,2,6,4,P.blue2)
    return img
  end
  return M
end
