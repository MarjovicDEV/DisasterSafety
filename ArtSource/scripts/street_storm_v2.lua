-- Typhoon street. The ground sprites use top-left anchors; actors register at their feet.
return function(L,kit)
  local M,P={},L.P
  local sky={L.hex("95a9b7"),L.hex("b6c6cc"),L.hex("d5ddda")}
  local wood,box=kit.WOOD,kit.box
  local function house(img,x,y,w,h,roof,wall)
    box(img,x,y,w,h,wall)
    L.poly(img,{x-5,y+2,x+7,y-14,x+w-10,y-14,x+w+5,y+2},P.ink)
    L.poly(img,{x-3,y,x+8,y-12,x+w-11,y-12,x+w+2,y},roof[2])
    for rx=x+13,x+w-11,7 do L.line(img,rx,y-11,rx-4,y-1,roof[3]) end
    L.hline(img,x+8,x+w-11,y-12,roof[1])
    local wy=y+7
    box(img,x+5,wy,24,17,wood)
    for sx=x+7,x+25,4 do L.vline(img,sx,wy+2,wy+14,wood[3]) end
    L.vline(img,x+17,wy+1,wy+15,P.ink)
    box(img,x+w-23,y+h-32,16,32,wood)
    L.rect(img,x+w-19,y+h-28,8,12,wood[3])
    L.px(img,x+w-11,y+h-13,P.yellow2)
  end
  local function tree(img,wind)
    local layer=L.newImage(78,91)
    L.poly(layer,{32,89,36,63,34,49,20,38,22,36,38,46,50,29,53,31,42,52,44,88},P.brown2)
    L.line(layer,36,85,39,52,P.brown3,2)
    for _,b in ipairs({{20,34,15,12},{32,22,19,16},{51,22,17,13},{60,36,15,12},{38,39,23,14}}) do
      local x,y=b[1]+wind,b[2]
      L.ellipse(layer,x,y,b[3],b[4],P.green3)
      L.ellipse(layer,x-2,y-2,b[3]-3,b[4]-3,P.grass3)
      L.ellipse(layer,x-5,y-4,b[3]//2,b[4]//3,P.grass2)
    end
    L.outline(layer,P.ink)
    L.blit(img,layer,234,94)
  end
  local function street(wind)
    local img=L.newImage(480,270)
    L.vgrad(img,0,0,480,149,sky)
    L.poly(img,{0,118,50,98,88,108,172,85,240,112,321,94,379,109,449,92,479,103,479,155,0,155},P.hill2)
    L.rect(img,0,141,480,10,P.grass3)
    house(img,162,106,60,39,kit.SAGE,wood)
    house(img,287,101,72,45,kit.RED,kit.WHITE)
    house(img,409,110,66,37,wood,kit.SAGE)
    L.rect(img,0,148,480,45,P.gray3)
    L.hline(img,0,479,148,P.gray2)
    for x=12,479,62 do L.rect(img,x,167,24,2,P.paper2) end
    L.rect(img,0,188,480,82,P.concrete2)
    L.hline(img,0,479,188,P.ink2)
    L.hline(img,0,479,191,P.concrete)
    for x=15,479,62 do L.line(img,x,192,x-13,269,P.concrete3) end
    L.hline(img,0,479,246,P.concrete3)
    -- The path bends out of sight in the reeds. Its entrance faces the near lane.
    L.poly(img,{102,137,114,137,113,152,127,176,151,198,101,198,101,175,94,155},P.brown3)
    L.poly(img,{105,138,111,138,108,154,119,177,139,196,108,196,106,173,99,154},P.tan)
    for x=86,113,3 do
      L.line(img,x,145,x-3,132-x%7,P.green3)
      L.line(img,x+1,144,x+3,134-x%4,P.grass3)
    end
    house(img,0,122,90,73,kit.SAGE,kit.WHITE)
    house(img,388,133,94,62,kit.RED,wood)
    box(img,393,149,78,33,kit.METAL)
    for y=152,178,4 do L.hline(img,395,468,y,P.gray3) end
    L.vline(img,432,150,180,P.gray4)
    box(img,385,141,94,8,kit.WHITE)
    for x=386,469,12 do L.rect(img,x,142,6,5,kit.RED[2]) end
    box(img,390,182,84,5,wood)
    for _,x in ipairs({150,369}) do
      box(img,x,62,6,92,wood)
      box(img,x-8,67,23,4,wood)
      for _,dx in ipairs({-5,9}) do box(img,x+dx,63,4,5,kit.METAL) end
    end
    for _,dx in ipairs({-4,10}) do
      for x=0,479 do
        local t=((x-150-dx)%219)/219
        L.px(img,x,63+math.floor(13*4*t*(1-t)),P.ink2)
      end
    end
    tree(img,wind)
    box(img,422,113,5,77,wood)
    box(img,379,93,91,28,kit.SAGE)
    L.rect(img,381,95,87,24,P.green3)
    L.text(img,383,98,"EVACUATION",P.white,1)
    L.text(img,383,108,"ROUTE",P.white,1)
    L.rect(img,446,104,13,4,P.white)
    L.poly(img,{458,99,466,106,458,113},P.white)
    return img
  end
  function M.save()
    local function save(name,img)
      local path="Level/v2/"..name
      L.save(img,path,path)
    end
    local base=street(0)
    save("bg_street_storm",base)
    local grey=app.pixelColor.rgba(103,117,132,132)
    local mask=L.newImage(480,270)
    local isSky={[sky[1]]=true,[sky[2]]=true,[sky[3]]=true}
    for y=0,147 do for x=0,479 do
      if isSky[base:getPixel(x,y)] then L.px(mask,x,y,grey) end
    end end
    save("typhoon_dark_sky_street_storm",mask)
    local frames={}
    for _,wind in ipairs({1,3,2,-1}) do
      local changed=street(wind)
      local img=L.newImage(78,55)
      for y=0,54 do for x=0,77 do
        local c=changed:getPixel(x+234,y+94)
        if c~=base:getPixel(x+234,y+94) then
          if isSky[c] then
            local pc=app.pixelColor
            local function mix(v,k) return (123*v+132*k+127)//255 end
            c=pc.rgba(mix(pc.rgbaR(c),103),mix(pc.rgbaG(c),117),mix(pc.rgbaB(c),132),255)
          end
          L.px(img,x,y,c)
        end
      end end
      frames[#frames+1]=img
    end
    local path="Level/v2/street_storm_tree_sway"
    L.saveFrames(frames,path,path,200)
    local wire=L.newImage(85,163)
    local knots={{80,0},{78,29},{74,61},{66,92},{57,119},{48,143},{42,150},{28,154},{17,152},{9,155},{2,160}}
    for i=2,#knots do
      local a,b=knots[i-1],knots[i]
      L.line(wire,a[1],a[2],b[1],b[2],P.ink,2)
    end
    save("street_storm_wire",wire)
    local branch=L.newImage(42,19)
    L.line(branch,2,16,37,5,P.brown2,3)
    L.line(branch,4,15,36,5,P.brown3)
    for _,v in ipairs({{13,12,10,5},{24,9,27,2},{31,7,38,11}}) do
      L.line(branch,v[1],v[2],v[3],v[4],P.brown2)
      L.ellipse(branch,v[3],v[4],3,2,P.green3)
      L.hline(branch,v[3]-2,v[3],v[4]-1,P.grass2)
    end
    L.outline(branch,P.ink)
    save("street_storm_branch",branch)
    local stick=L.newImage(20,8)
    L.line(stick,1,6,18,2,P.brown2,2)
    L.line(stick,9,4,12,1,P.brown)
    L.line(stick,3,5,8,4,P.brown3)
    save("street_storm_stick",stick)
    frames={}
    for f=0,1 do
      local puddle=L.newImage(32,8)
      L.ellipse(puddle,16,4,15,3,P.gray3)
      L.ellipse(puddle,16,4,14,2,P.glass)
      L.hline(puddle,6-f,13+f,3,P.glass2)
      L.hline(puddle,20-f,23+2*f,5,P.glass2)
      frames[#frames+1]=puddle
    end
    path="Level/v2/street_storm_puddle"
    L.saveFrames(frames,path,path,400)
  end
  return M
end
