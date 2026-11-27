-- Two visits to the same street. All active Smoke and Flames are separate layers.
return function(L,kit,menu,grownups)
  local M,P={},L.P
  local box=kit.box
  local function house(img,x,y,w,h,roof,wall)
    box(img,x,y,w,h,wall)
    L.poly(img,{x-5,y+2,x+7,y-14,x+w-10,y-14,x+w+5,y+2},P.ink)
    L.poly(img,{x-3,y,x+8,y-12,x+w-11,y-12,x+w+2,y},roof[2])
    for i=x+12,x+w-10,7 do L.line(img,i,y-11,i-4,y-1,roof[3]) end
    L.hline(img,x+8,x+w-11,y-12,roof[1])
    L.hline(img,x-2,x+w+1,y+1,roof[3])
    box(img,x+7,y+9,24,16,kit.WOOD)
    box(img,x+9,y+11,20,12,kit.GLASS)
    L.vline(img,x+19,y+11,y+22,P.brown2)
    box(img,x+w-23,y+h-33,16,33,kit.WOOD)
    L.px(img,x+w-11,y+h-15,P.yellow)
  end
  local function foodVendor(img)
    local cart=L.newImage(84,84)
    -- Isawan vendor holding grilled isaw on a bamboo skewer.
    L.ellipse(img,250,212,39,3,P.paper3)
    L.blit(img,grownups.isawanVendor(),213+46,133+17)
    L.line(cart,48,42,46,24,P.tan)
    for k=0,2 do
      local x,y=45+k,26+4*k
      L.line(cart,x,y,x+3,y,P.brown2,2)
      L.line(cart,x+3,y+1,x,y+3,P.orange2,2)
      L.px(cart,x+1,y,P.tan)
    end
    -- Canopy, charcoal grill, tusok-tusok tray, sauces and push handle.
    L.rect(cart,8,14,2,42,P.gray3)
    L.rect(cart,43,14,2,40,P.gray3)
    L.poly(cart,{1,13,9,5,38,3,49,12,48,15,2,16},P.teal2)
    L.poly(cart,{4,12,11,6,25,5,19,13},P.teal3)
    L.poly(cart,{20,13,26,5,36,5,40,12},P.paper)
    L.rect(cart,7,53,42,16,P.teal2)
    L.rect(cart,9,55,37,11,P.teal)
    L.hline(cart,11,43,56,P.teal3)
    L.rect(cart,5,50,47,4,P.gray2)
    L.hline(cart,6,50,50,P.gray1)
    L.poly(cart,{6,47,10,39,28,39,25,47},P.gray4)
    L.rect(cart,6,47,20,3,P.gray3)
    for k=0,2 do
      local x=11+k*6
      L.line(cart,x-3,48,x+2,36,P.tan)
      for j=0,2 do
        L.hline(cart,x-1+j,x+1+j,44-j*3,P.orange2)
        L.px(cart,x-1+j,43-j*3,P.tan2)
      end
    end
    L.rect(cart,28,47,12,3,P.gray2)
    for k=0,2 do
      local x=29+k*4
      L.line(cart,x,47,x+1,40-k%2,P.tan)
      L.rect(cart,x,43,3,3,k==1 and P.red2 or P.tan2)
      L.px(cart,x,43,k==1 and P.red3 or P.tan)
    end
    for _,v in ipairs({{42,P.red2},{47,P.brown}}) do
      L.rect(cart,v[1],44,3,6,v[2])
      L.px(cart,v[1]+1,43,P.paper)
    end
    L.line(cart,48,54,53,49,P.gray3,2)
    L.hline(cart,53,58,49,P.brown2)
    L.rect(cart,42,67,2,10,P.gray4)
    L.circle(cart,16,71,7,P.gray4)
    L.circle(cart,16,71,4,P.gray2)
    L.circle(cart,16,71,2,P.gray4)
    L.outline(cart,P.ink)
    L.blit(img,cart,213,133)
  end
  function M.background(after)
    local img=L.newImage(480,270)
    L.vgrad(img,0,0,480,174,{P.sky2,P.sky3,P.sky4})
    L.poly(img,{0,116,50,97,87,108,176,82,243,111,317,93,383,107,446,91,479,101,479,153,0,153},P.hill2)
    L.rect(img,0,139,480,30,P.grass2)
    house(img,164,105,60,39,kit.SAGE,kit.WOOD)
    house(img,280,97,92,47,kit.RED,kit.WHITE)
    house(img,395,114,69,30,kit.WOOD,kit.SAGE)
    L.rect(img,0,147,480,43,P.gray3)
    L.hline(img,0,479,147,P.gray2)
    for x=16,479,61 do L.rect(img,x,166,27,2,P.paper2) end
    L.rect(img,0,188,480,82,P.paper2)
    L.hline(img,0,479,188,P.ink2)
    L.hline(img,0,479,191,P.tan)
    for x=15,479,62 do L.line(img,x,193,x-13,269,P.paper3) end
    L.hline(img,0,479,242,P.paper3)
    box(img,238,57,7,91,kit.WOOD)
    box(img,228,63,27,4,kit.WOOD)
    for k=0,1 do
      for x=0,479 do
        local d=(x-241)/241
        L.px(img,x,60+k*5+math.floor(13*math.sin(math.abs(d)*math.pi*0.7)),P.ink2)
      end
    end
    house(img,1,125,104,72,kit.SAGE,kit.WHITE)
    if after then
      for i=0,12 do L.vline(img,13+i,127+i%4,135,P.gray3) end
    end
    for x=91,142,7 do box(img,x,162,5,36,kit.WOOD) end
    box(img,90,171,57,4,kit.WOOD)
    box(img,90,184,57,4,kit.WOOD)
    -- Small neighbourhood shop with stocked shelves beneath its striped awning.
    house(img,387,132,95,64,kit.RED,kit.WOOD)
    box(img,392,148,83,36,kit.NAVY)
    for row=0,1 do
      for k=0,11 do box(img,395+k*6,155+14*row-k%3,4,8+k%3,({kit.GLASS,kit.RED,kit.SAGE})[(k+row)%3+1]) end
      L.hline(img,395,471,164+14*row,P.brown3)
    end
    box(img,385,142,94,8,kit.WHITE)
    for x=386,470,12 do L.rect(img,x,143,6,6,P.red) end
    box(img,389,184,88,6,kit.WOOD)
    -- Meeting Place under the tree on the near side of the street.
    L.ellipse(img,346,201,27,4,P.paper3)
    box(img,344,133,9,66,kit.WOOD)
    for _,b in ipairs({{326,134,17},{347,119,20},{365,134,17},{343,138,21}}) do
      L.circle(img,b[1],b[2],b[3],P.green3)
      L.circle(img,b[1]-2,b[2]-3,b[3]-3,P.green2)
      L.ellipse(img,b[1]-5,b[2]-7,8,4,P.grass)
    end
    if after then
      L.line(img,184,188,206,198,P.ink,3)
      L.line(img,206,198,236,196,P.ink,3)
      L.line(img,236,196,249,200,P.ink,3)
      L.line(img,185,189,206,199,P.tan,1)
      L.line(img,206,199,236,197,P.tan,1)
      menu.parkedEngine(img,109,137)
      box(img,337,166,24,18,kit.SAGE)
      for _,pt in ipairs({{340,169,346,173},{357,169,352,173}}) do L.line(img,pt[1],pt[2],pt[3],pt[4],P.white) end
      L.circle(img,349,174,2,P.white)
      L.rect(img,346,177,7,3,P.white)
    else
      foodVendor(img)
    end
    return img
  end
  function M.cat()
    local img=L.newImage(21,17)
    L.ellipse(img,10,11,5,4,P.orange2)
    L.line(img,14,12,18,8,P.orange,2)
    L.line(img,18,8,17,5,P.orange,2)
    L.poly(img,{2,3,5,5,9,5,12,2,12,10,8,12,3,10},P.tan)
    L.px(img,5,8,P.ink)
    L.px(img,9,8,P.ink)
    L.rect(img,6,13,3,2,P.tan)
    L.outline(img,P.ink)
    return img
  end
  function M.phone()
    local img=L.newImage(7,11)
    box(img,0,0,7,11,kit.NAVY)
    L.rect(img,2,2,3,5,P.glass2)
    L.px(img,3,8,P.paper)
    return img
  end
  function M.crates()
    local img=L.newImage(43,34)
    for _,b in ipairs({{2,17,22,16},{24,18,18,15},{9,2,24,15}}) do
      local x,y,w,h=table.unpack(b)
      box(img,x,y,w,h,kit.WOOD)
      L.rect(img,x+4,y+3,w-8,2,P.brown2)
      L.line(img,x+2,y+h-3,x+w-3,y+5,P.tan)
      L.hline(img,x+1,x+w-2,y+h-3,P.brown2)
    end
    return img
  end
  return M
end
