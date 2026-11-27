-- Baluan backyard in late afternoon. Open foreground carries the Student's roll.
return function(L, kit)
  local M,P={},L.P
  local function mango(img,wind)
    local tree=L.newImage(158,176)
    L.poly(tree,{67,166,73,124,72,87,44,65,48,60,77,78,92,46,98,48,84,94,89,147,100,168,84,164,77,169},P.brown2)
    L.poly(tree,{71,164,77,121,76,88,51,66,78,82,84,68,81,96,83,146,85,163},P.brown3)
    local crowns={{33,65,29,19},{61,43,35,24},{92,34,31,22},{118,53,34,26},{86,65,41,27},{51,72,32,21}}
    for _,b in ipairs(crowns) do L.ellipse(tree,b[1],b[2],b[3],b[4],P.green3) end
    for _,b in ipairs(crowns) do
      L.ellipse(tree,b[1]-4,b[2]-5,b[3]-5,b[4]-6,P.green2)
      L.ellipse(tree,b[1]-9,b[2]-8,b[3]//2,b[4]//3,P.grass)
    end
    for _,v in ipairs({{31,76},{45,66},{75,86},{101,76},{128,66},{61,54}}) do
      L.line(tree,v[1]+1,v[2]-3,v[1],v[2],P.brown2)
      L.ellipse(tree,v[1],v[2]+2,2,4,P.yellow2)
      L.vline(tree,v[1]-1,v[2]+1,v[2]+3,P.yellow3)
    end
    L.outline(tree,P.ink)
    if wind and wind~=0 then
      for y=0,tree.height-1 do
        local shift=math.floor(wind*math.max(0,1-y/110)^2+0.5)
        for x=0,tree.width-1 do
          if L.opaque(tree,x,y) then L.px(img,8+x+shift,18+y,tree:getPixel(x,y)) end
        end
      end
    else
      L.blit(img,tree,8,18)
    end
  end
  function M.background(wind)
    local img=L.newImage(480,270)
    L.vgrad(img,0,0,480,175,{P.sky3,P.sky4,P.cream})
    L.circle(img,188,86,12,P.yellow3)
    L.poly(img,{0,128,39,109,71,119,110,99,164,126,211,107,246,120,297,97,359,123,405,107,479,126,479,175,0,175},P.hill2)
    L.rect(img,0,169,480,101,P.grass)
    L.rect(img,0,174,480,17,P.grass2)
    for _,y in ipairs({138,162}) do kit.box(img,-2,y,484,6,kit.WOOD) end
    for x=-4,477,12 do
      local y=127+(x+4)//12%3
      L.poly(img,{x,y+3,x+4,y,x+9,y+2,x+9,175,x,175},P.ink)
      L.rect(img,x+1,y+3,8,172-y,P.brown3)
      L.vline(img,x+7,y+4,173,P.brown)
      L.vline(img,x+2,y+4,172,P.tan)
    end
    L.ellipse(img,122,185,56,5,P.grass3)
    mango(img,wind)
    for _,x in ipairs({250,439}) do kit.box(img,x,81,6,99,kit.WOOD) end
    for x=252,442 do
      local t=(x-252)/190
      L.px(img,x,86+math.floor(8*4*t*(1-t)),P.ink2)
    end
    for i,hanger in ipairs({{275,90,21,26},{322,93,34,25},{387,91,18,21}}) do
      local x,y,w,h=table.unpack(hanger)
      local cloth=({kit.GLASS,kit.WHITE,kit.SAGE})[i]
      local fabric=L.newImage(w,h)
      kit.box(fabric,0,0,w,h,cloth)
      L.vline(fabric,6,3,h-3,cloth[3])
      L.hline(fabric,2,w-3,h-4,cloth[1])
      for row=0,h-1 do
        local shift=math.floor((wind or 0)*(row/(h-1))^2+0.5)
        for col=0,w-1 do L.px(img,x+col+shift,y+row,fabric:getPixel(col,row)) end
      end
      for _,pin in ipairs({x+2,x+w-5}) do L.rect(img,pin,y-2,2,5,P.brown2) end
    end
    L.ellipse(img,429,187,23,4,P.grass3)
    kit.box(img,408,148,32,39,kit.NAVY)
    L.rect(img,411,151,26,33,P.blue2)
    L.vline(img,414,151,183,P.blue3)
    for _,y in ipairs({158,176}) do kit.box(img,407,y,34,4,kit.METAL) end
    L.ellipse(img,424,149,17,4,P.ink)
    L.ellipse(img,424,148,15,3,P.gray2)
    L.hline(img,417,430,147,P.gray1)
    L.seed(810)
    for i=1,155 do
      local x,y=L.randi(0,479),L.randi(190,269)
      if x<180 or x>345 or y>250 then
        L.line(img,x,y,x+2,y-2,P.grass2)
        L.px(img,x-1,y-1,P.green4)
      end
    end
    return img
  end
  function M.typhoonProps()
    local pail=L.newImage(12,12)
    L.line(pail,2,6,3,1,P.ink)
    L.hline(pail,3,8,1,P.gray3)
    L.line(pail,8,1,10,5,P.ink)
    L.poly(pail,{0,4,12,4,10,12,2,12},P.ink)
    L.poly(pail,{1,5,11,5,9,11,3,11},P.teal)
    L.hline(pail,1,10,5,P.teal3)
    L.vline(pail,3,6,9,P.teal3)
    L.vline(pail,9,6,9,P.teal2)
    local ball=L.newImage(8,8)
    L.ellipse(ball,3.5,3.5,3.6,3.6,P.ink)
    L.ellipse(ball,3.5,3.5,2.8,2.8,P.orange)
    L.line(ball,2,5,5,2,P.yellow3)
    L.px(ball,5,5,P.orange2)
    local chair=L.newImage(18,22)
    L.poly(chair,{3,0,15,0,15,10,17,12,17,15,16,15,17,22,14,22,13,15,5,15,4,22,1,22,3,15,0,14,1,12,4,10},P.ink)
    L.poly(chair,{4,1,14,1,14,11,16,12,15,14,2,14,2,12,5,11},P.gray1)
    for x=6,12,3 do L.vline(chair,x,3,8,P.gray3) end
    L.hline(chair,3,15,12,P.white)
    L.line(chair,4,15,2,20,P.gray2)
    L.line(chair,14,15,15,20,P.gray2)
    local rope=L.newImage(36,12)
    for _,y in ipairs({2,6}) do
      L.line(rope,1,y,12,y+2,P.brown2,2)
      L.hline(rope,12,25,y+2,P.tan)
      L.line(rope,25,y+2,34,y,P.brown)
    end
    L.line(rope,9,3,15,10,P.brown2,2)
    L.line(rope,15,4,10,10,P.tan)
    return {yard_pail=pail,yard_ball=ball,yard_chair=chair,yard_rope=rope}
  end

  function M.swayFrames()
    local base=M.background()
    local tree,clothes={},{}
    -- Replace only displaced pixels. Revealed sky matches the Batch 2 Dark Sky.
    local function patch(scene,x0,y0,w,h)
      local img=L.newImage(w,h)
      for y=0,h-1 do for x=0,w-1 do
        local c=scene:getPixel(x+x0,y+y0)
        if c~=base:getPixel(x+x0,y+y0) then
          if c==P.sky3 or c==P.sky4 or c==P.cream then
            local pc=app.pixelColor
            local function shade(v,s) return (v*85+s*170+127)//255 end
            c=pc.rgba(shade(pc.rgbaR(c),110),shade(pc.rgbaG(c),124),shade(pc.rgbaB(c),140),255)
          end
          L.px(img,x,y,c)
        end
      end end
      return img
    end
    for _,wind in ipairs({1,4,2,-2}) do
      local scene=M.background(wind)
      tree[#tree+1]=patch(scene,8,18,160,112)
      clothes[#clothes+1]=patch(scene,252,86,191,38)
    end
    return tree,clothes
  end

  function M.damage()
    local shed=L.newImage(76,65)
    kit.box(shed,6,18,62,46,kit.WOOD)
    for x=10,64,7 do L.vline(shed,x,21,61,P.brown2) end
    L.poly(shed,{0,23,15,6,36,3,40,7,33,11,36,15,26,18,23,23},P.ink)
    L.poly(shed,{3,21,16,8,36,5,32,10,34,13,24,17,22,21},P.roof2)
    for x=10,25,6 do L.line(shed,x,18,x+9,8,P.roof3) end
    L.line(shed,33,17,46,4,P.brown2,3)
    L.line(shed,46,4,72,20,P.brown2,3)
    L.line(shed,48,8,64,19,P.tan2)
    kit.box(shed,23,28,26,36,kit.WOOD)
    L.rect(shed,26,30,20,32,P.ink2)
    L.poly(shed,{47,29,62,34,59,64,46,61},P.ink)
    L.poly(shed,{49,31,60,35,57,62,48,60},P.brown)
    L.line(shed,50,35,56,57,P.brown3,2)
    L.hline(shed,46,50,32,P.gray3)
    local sheet=L.newImage(43,15)
    L.poly(sheet,{0,10,13,1,42,3,31,14},P.gray4)
    L.poly(sheet,{3,10,14,2,39,4,30,13},P.gray2)
    for x=11,31,5 do L.line(sheet,x+4,4,x-4,10,P.gray3) end
    local glass=L.newImage(23,8)
    for _,s in ipairs({{0,5,6,2,4,6},{9,5,12,1,15,6},{17,6,22,4,21,7}}) do
      L.poly(glass,s,P.glass3);L.px(glass,s[1]+2,s[2],P.glass2)
    end
    local broom=L.newImage(17,37)
    L.line(broom,1,1,10,24,P.brown2,3)
    L.line(broom,1,1,9,22,P.brown3)
    L.poly(broom,{8,19,11,19,16,33,13,36,4,35},P.brown2)
    for dx=5,15,2 do L.line(broom,10,21,dx,34-dx%2,P.tan2) end
    L.hline(broom,8,12,23,P.brown)
    return {yard_shed_broken=shed,yard_roof_sheet=sheet,yard_glass=glass,yard_broom=broom}
  end

  function M.siga(f)
    local img=L.newImage(46,31)
    L.ellipse(img,23,23,21,6,P.brown2)
    L.ellipse(img,23,22,18,4,P.tan2)
    for _,s in ipairs({{4,23},{8,19},{15,17},{24,16},{33,17},{40,19},{42,23},{37,26},{29,28},{19,28},{11,27}}) do
      L.ellipse(img,s[1],s[2],4,2,P.ink)
      L.ellipse(img,s[1],s[2],3,1,P.gray3)
      L.hline(img,s[1]-2,s[1],s[2]-1,P.gray1)
    end
    for _,v in ipairs({{12,21,23,24},{19,19,31,23},{16,25,26,21},{25,23,33,20}}) do
      L.line(img,v[1],v[2],v[3],v[4],P.brown,2)
      L.px(img,v[1],v[2],P.tan)
    end
    local flame=L.newImage(20,22)
    local tip=({10,12,11,8})[f+1]
    L.poly(flame,{2,17,4,9,7,12,tip,3+f%2,12,11,16,7,17,15,15,19,6,19},P.red2)
    L.poly(flame,{4,16,6,12,8,14,tip,7,12,14,15,11,15,16,13,18,6,18},P.orange)
    L.poly(flame,{7,17,9,12+f%2,11,15,13,13,12,18},P.yellow)
    L.outline(flame,P.ink)
    L.blit(img,flame,13,3)
    return img
  end
  return M
end
