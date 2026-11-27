-- Hard 2: Lola's house overlooks a sheltered landing. Animated water occupies
-- only the offshore strip. The road and the sandy landing remain dry.
return function(L,kit)
  local M,P={},L.P
  local box=kit.box
  local water={L.hex("b0cec7"),L.hex("789f9d"),L.hex("537e82")}
  local sky={L.hex("93a6b2"),L.hex("b1c0c5"),L.hex("d6deda")}
  local function waves(img,f)
    L.rect(img,0,85,205,39,water[2])
    for row=0,2 do
      local period=({40,52,68})[row+1]
      local base=({94,104,118})[row+1]
      for i=-2,8 do
        local x=i*period+(row*13)+(f*period//4)
        local h=5+row*2
        L.poly(img,{x,base,x+10,base-3,x+17,base-h,x+24,base-h-1,x+22,base-h+3,x+32,base-2,x+period,base+1,x+period,base+4,x,base+4},water[3])
        L.line(img,x+7,base-2,x+17,base-h,water[1],2)
        L.hline(img,x+17,x+24,base-h-1,P.white)
        L.line(img,x+24,base-h,x+22,base-h+2,P.gray1)
        L.line(img,x+26,base-3,x+32,base-1,water[1])
      end
    end
  end
  local function palm(img,f)
    local layer=L.newImage(77,103)
    local sway=({0,2,3,1})[f+1]
    for y=22,96 do
      local t=(96-y)/74
      local x=math.floor(24+17*t*t+sway*math.max(0,(46-y)/24))
      L.hline(layer,x-2,x+2,y,y%6==0 and P.brown2 or P.brown)
      L.px(layer,x-2,y,P.brown3)
    end
    local cx=41+sway
    for _,v in ipairs({{-32,9,-15,-17},{-28,22,-17,-6},{-14,28,-13,0},{8,24,5,-6},{28,17,19,-9},{30,4,18,-16},{18,-9,9,-15}}) do
      local ex,ey=cx+v[1],22+v[2]+sway//2
      local mx,my=cx+v[3],22+v[4]
      L.poly(layer,{cx,22,mx,my,ex,ey,mx,my+5},P.green3)
      L.line(layer,cx,21,mx,my+1,P.grass2)
      for k=1,4 do
        local x=mx+(ex-mx)*k//5
        local y=my+(ey-my)*k//5
        L.line(layer,x,y,x-2,y+5-k//2,P.grass3)
      end
    end
    L.outline(layer,P.ink)
    L.blit(img,layer,119,36)
  end
  local function banca(img)
    -- Twin bamboo outriggers tied to the hull; no one aboard.
    L.ellipse(img,60,140,42,3,P.tan2)
    L.line(img,19,140,86,142,P.brown2,2)
    L.line(img,20,139,85,141,P.tan)
    L.poly(img,{17,124,31,129,78,129,96,121,88,133,74,137,31,136,22,131},P.ink)
    L.poly(img,{20,126,33,130,78,130,92,125,86,132,74,135,33,134},P.teal2)
    L.line(img,27,130,77,132,P.teal3,2)
    L.line(img,36,129,30,140,P.brown2,2)
    L.line(img,73,129,80,141,P.brown2,2)
    L.line(img,36,128,30,139,P.tan)
    L.line(img,73,128,80,140,P.tan)
  end
  local function scene(f,p)
    local img=L.newImage(480,270)
    L.rect(img,0,0,480,270,P.hill2)
    L.vgrad(img,0,0,480,112,sky)
    L.poly(img,{0,85,28,82,47,77,67,77,95,84,122,80,143,75,166,78,198,85,210,90,0,91},P.hill2)
    local sea=L.newImage(205,124)
    waves(sea,f)
    L.blit(img,sea,0,0)
    L.poly(img,{0,122,52,120,114,122,169,115,205,111,219,153,0,160},P.tan)
    L.line(img,0,122,52,120,P.paper2)
    banca(img)
    palm(img,p)
    -- Near parapet ends at the stairs. A return wall separates the cove from the house.
    box(img,-3,147,129,22,kit.METAL)
    box(img,-3,144,131,5,kit.WHITE)
    for x=12,110,29 do L.vline(img,x,150,166,P.gray3) end
    L.poly(img,{177,145,202,96,209,96,187,166,176,169},P.ink)
    L.poly(img,{179,147,203,98,207,98,185,163,179,165},P.gray2)
    L.line(img,180,145,203,98,P.gray1,2)
    L.rect(img,0,169,480,101,P.concrete2)
    L.hline(img,0,479,169,P.concrete3)
    for x=-30,480,79 do L.line(img,x,171,x-16,269,P.concrete3) end
    L.hline(img,0,479,246,P.concrete3)
    -- Stairs widen toward the road-side landing at y=207.
    L.poly(img,{130,143,156,143,182,207,107,207},P.ink2)
    for j=0,6 do
      local y=144+8*j+j*j//3
      local left,right=130-3*j,156+4*j
      local nextY=144+8*(j+1)+(j+1)^2//3
      L.rect(img,left,y,right-left,nextY-y,P.concrete2)
      L.hline(img,left,right-1,y,P.concrete)
      L.hline(img,left,right-1,y+5+j,P.concrete3)
    end
    -- Road on the right climbs above the sea to the inland hill.
    L.poly(img,{248,172,304,124,340,111,383,80,422,72,480,57,480,143,361,187,250,202},P.grass3)
    L.poly(img,{253,172,308,128,345,115,386,85,429,77,479,62,479,151,320,181},P.grass2)
    L.poly(img,{218,245,279,201,341,169,377,139,407,127,448,123,483,115,483,139,425,146,398,156,365,190,318,212,269,251},P.concrete3)
    L.poly(img,{223,244,281,203,343,171,379,141,409,129,449,125,483,117,483,136,424,143,396,154,362,187,316,210,267,249},P.paper2)
    -- Lola's house: closed shutters, vent slats, corrugated roof and a sheltered doorstep.
    box(img,201,120,84,61,kit.WOOD)
    for x=204,282,7 do L.vline(img,x,124,177,P.brown) end
    L.poly(img,{193,123,208,98,272,98,291,122,291,126,193,126},P.ink)
    L.poly(img,{196,122,209,100,271,100,287,122},kit.SAGE[2])
    for x=211,269,6 do L.line(img,x,101,x-7,121,kit.SAGE[3]) end
    L.hline(img,196,287,123,kit.SAGE[1])
    box(img,209,133,27,23,kit.WOOD)
    for x=212,231,4 do L.vline(img,x,135,152,P.brown2) end
    box(img,248,131,22,49,kit.WOOD)
    box(img,251,135,16,18,kit.SAGE)
    L.px(img,265,162,P.yellow2)
    box(img,203,179,73,4,kit.METAL)
    box(img,199,183,80,4,kit.WHITE)
    box(img,401,92,5,60,kit.WOOD)
    box(img,365,68,78,25,kit.SAGE)
    L.rect(img,367,70,74,21,P.green3)
    for i,t in ipairs({"EVACUATION","CENTER"}) do
      L.text(img,404-L.F.width(t,1)//2,71+10*(i-1),t,P.white,1)
    end
    -- Fixed light rain follows the down-left slant of the outdoor Rain layers.
    L.seed(811)
    local pc=app.pixelColor
    for i=1,400 do
      local x,y=L.randi(1,479),L.randi(0,267)
      for k=0,3 do
        local xx,yy=x-k//3,y+k
        local c=img:getPixel(xx,yy)
        if not (xx>=365 and xx<=442 and yy>=68 and yy<=92) then
          local function mix(v,r) return (v*166+r*89)//255 end
          L.px(img,xx,yy,pc.rgba(mix(pc.rgbaR(c),153),mix(pc.rgbaG(c),183),mix(pc.rgbaB(c),204),255))
        end
      end
    end
    return img
  end
  function M.save()
    local base=scene(0,0)
    local path="Level/v2/bg_coast"
    L.save(base,path,path)
    -- Opaque crops replace the corresponding region of the still backdrop.
    -- The crown lies above the wave strip, so these two loops run independently.
    for _,s in ipairs({{"coast_waves",0,85,205,39,180},{"coast_palm_sway",119,36,77,49,200}}) do
      local frames={}
      for f=0,3 do
        local img=scene(s[1]=="coast_waves" and f or 0,s[1]=="coast_palm_sway" and f or 0)
        local region=L.newImage(s[4],s[5])
        for y=0,s[5]-1 do for x=0,s[4]-1 do
          L.px(region,x,y,img:getPixel(x+s[2],y+s[3]))
        end end
        frames[#frames+1]=region
      end
      path="Level/v2/"..s[1]
      L.saveFrames(frames,path,path,s[6])
    end
    local shells=L.newImage(19,7)
    for _,s in ipairs({{1,2},{8,0},{14,3}}) do
      local x,y=s[1],s[2]
      L.poly(shells,{x,y+2,x+1,y,x+3,y,x+5,y+2,x+3,y+4,x+2,y+4},P.brown2)
      L.hline(shells,x+1,x+3,y+1,P.paper)
      L.px(shells,x+2,y+2,P.tan)
    end
    path="Level/v2/coast_shells"
    L.save(shells,path,path)
  end
  return M
end

