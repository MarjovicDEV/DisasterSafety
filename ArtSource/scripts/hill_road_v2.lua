-- Two barangay roads: the residential bend and the exposed ridge turnout.
return function(L,root,only)
  local P=L.P
  local function still(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local function sequence(name,frames,ms)
    local path="Level/v2/"..name
    L.saveFrames(frames,path,path,ms)
  end
  local function route(img,x,y)
    local w=L.F.width("EVACUATION")+20
    L.rect(img,x+6,y+17,4,38,P.ink2)
    L.rect(img,x+7,y+17,2,37,P.metal2)
    L.rect(img,x+w-10,y+17,4,38,P.ink2)
    L.rect(img,x+w-9,y+17,2,36,P.metal2)
    L.rect(img,x,y,w,21,P.ink)
    L.rect(img,x+1,y+1,w-2,19,P.paper2)
    L.rect(img,x+2,y+2,w-4,17,P.teal2)
    L.text(img,x+4,y+3,"EVACUATION",P.white)
    L.text(img,x+4,y+11,"ROUTE",P.white)
    L.poly(img,{x+w-16,y+9,x+w-10,y+9,x+w-10,y+6,x+w-4,y+11,x+w-10,y+16,x+w-10,y+13,x+w-16,y+13},P.white)
  end
  local function houses(im)
    -- A raised amakan house on the level bench beyond the bend.
    for _,x in ipairs({261,277,311}) do
      L.rect(im,x,139,4,24,P.brown2)
      L.vline(im,x,140,162,P.tan)
    end
    L.rect(im,256,115,61,32,P.ink)
    L.rect(im,258,117,57,28,P.tan)
    for y=117,142,4 do for x=258,310,5 do
      L.line(im,x,y,x+3,y+2,P.brown3)
      L.line(im,x+2,y+3,x+4,y+1,P.paper3)
    end end
    L.poly(im,{248,118,262,96,297,91,325,113,316,119},P.ink)
    L.poly(im,{251,115,263,98,296,93,314,115},P.teal)
    for x=264,294,5 do L.line(im,x,99-(x-264)//7,x-9,114,P.teal2) end
    L.poly(im,{297,94,323,113,316,115},P.teal2)
    L.rect(im,268,122,20,14,P.brown2)
    L.rect(im,270,124,16,9,P.glass3)
    for x=271,282,4 do L.vline(im,x,124,133,P.glass2) end
    L.rect(im,297,121,10,23,P.brown2)
    L.rect(im,299,122,6,21,P.brown)
    for j=0,4 do L.rect(im,295+j,146+j*3,14,2,P.concrete3) end
    -- Low plaster bungalow with a hipped tile roof and a shaded veranda.
    L.rect(im,335,123,72,30,P.ink)
    L.rect(im,336,124,70,26,P.wall)
    L.poly(im,{326,124,343,107,386,105,413,120,409,125},P.ink)
    L.poly(im,{329,122,344,109,385,107,393,121},P.roof3)
    L.poly(im,{385,107,410,120,393,121},P.roof2)
    for y=114,120,3 do L.hline(im,341-(y-114),391,y,P.roof) end
    for _,x in ipairs({340,365}) do
      L.rect(im,x,130,16,13,P.brown2)
      L.rect(im,x+1,131,14,10,P.glass2)
      L.vline(im,x+8,131,140,P.brown)
      L.hline(im,x-1,x+16,143,P.brown)
    end
    L.rect(im,389,130,11,19,P.teal2)
    L.rect(im,385,125,21,3,P.brown3)
    L.vline(im,404,128,151,P.brown2)
  end
  local function hillside(im,ridge)
    local shape=L.newImage(480,270)
    local contour=ridge and {0,91,18,75,37,69,61,83,75,106,87,126,110,135,129,153,151,164,165,178,157,183,102,185,43,182,0,185}
      or {0,77,23,64,49,71,61,86,83,93,94,108,116,119,126,143,152,162,162,175,176,180,175,188,111,188,48,185,0,189}
    L.poly(shape,contour,P.grass3)
    local face=ridge and {0,107,30,83,55,92,78,121,105,144,140,162,156,177,125,176,102,171,65,166,33,169,0,175}
      or {0,98,23,76,48,83,64,98,90,115,109,123,118,145,148,169,162,181,134,183,88,176,41,181,0,183}
    L.poly(shape,face,ridge and P.rock or P.dirt)
    L.seed(ridge and 343 or 304)
    for i=1,480 do
      local x,y=L.randi(1,156),L.randi(76,184)
      if shape:getPixel(x,y)==(ridge and P.rock or P.dirt) then
        L.line(shape,x,y,x+3,y-1,ridge and P.rock2 or P.brown)
      end
    end
    L.poly(shape,ridge and {25,87,48,100,63,118,58,125,87,142,102,150,94,155,136,166,161,179,145,175,87,158,94,151,51,126,56,119,45,102}
      or {26,77,54,100,75,112,64,120,92,137,112,145,104,153,140,168,165,181,145,175,97,155,104,146,58,121,68,113},P.paper3)
    L.outline(shape,P.ink)
    L.blit(im,shape,0,0)
    return shape
  end
  if only.hill_road then
    for _,ridge in ipairs({false,true}) do
      local im=L.newImage(480,270)
      L.vgrad(im,0,0,480,170,{P.sky3,P.sky4,P.sky5})
      local sky=Image(im)
      if ridge then
        -- An uninhabited, bare ochre hill across a deep valley.
        L.poly(im,{0,123,92,96,164,114,218,77,272,100,313,81,346,96,395,71,480,99,480,176,0,176},P.mount)
        L.poly(im,{0,133,73,122,140,136,227,103,274,122,319,102,359,85,387,90,424,110,480,121,480,176,0,176},P.hill2)
        L.poly(im,{304,130,324,113,346,95,361,87,377,91,399,106,426,119,449,132,429,140,392,145,344,141},P.paper3)
        L.poly(im,{361,88,376,93,397,107,425,119,446,132,405,130,382,120},P.tan2)
        L.poly(im,{0,148,83,134,154,148,224,124,268,137,315,129,352,147,411,141,480,151,480,183,0,183},P.hill)
        L.poly(im,{0,155,95,145,151,158,235,142,285,151,331,142,403,156,457,146,480,151,480,185,0,185},P.green3)
        -- Contour strips and small groves on the green side of the valley.
        for _,v in ipairs({{172,143,194,135,213,135},{187,145,207,139,226,139},{207,146,227,142,247,143},{241,146,258,144,275,147}}) do
          L.line(im,v[1],v[2],v[3],v[4],P.grass3)
          L.line(im,v[3],v[4],v[5],v[6],P.grass3)
        end
        for _,x in ipairs({151,158,165,266,271,280,457,465,469}) do
          local y=150+x%4
          L.vline(im,x,y-5,y+1,P.green3)
          L.ellipse(im,x,y-5,3,3,P.hill2)
        end
        L.rect(im,0,168,480,102,P.concrete2)
        -- Concrete ridge road climbs toward the lookout, with a fan of joint lines.
        L.poly(im,{0,177,230,174,480,155,480,181,230,204,0,216},P.gray3)
        for x=12,471,48 do
          local y=190-math.floor(x*.052)
          L.line(im,x,y,x+21,y-1,P.paper2,2)
        end
        L.poly(im,{0,214,230,202,480,179,480,182,230,206,0,218},P.gray4)
        for x=9,445,79 do L.line(im,x,269,x+42,216-math.floor(x*.068),P.concrete3) end
        for x=163,470,29 do
          local y=169-math.floor((x-163)*.042)
          L.rect(im,x,y-12,4,15,P.gray4)
          L.vline(im,x+1,y-11,y,P.metal2)
        end
        L.line(im,163,161,479,147,P.ink2,3)
        L.line(im,164,161,479,147,P.metal,2)
      else
        L.poly(im,{0,137,102,108,155,122,207,95,251,109,294,91,335,110,397,82,480,105,480,179,0,179},P.hill)
        L.poly(im,{0,150,134,132,169,138,238,115,298,133,354,119,395,129,448,111,480,117,480,179,0,179},P.hill2)
        L.rect(im,0,156,480,114,P.grass2)
        -- Banana grove behind the houses, with broad split leaves.
        for _,v in ipairs({{307,139,16},{320,146,20},{431,143,17},{449,148,22}}) do
          local x,y,h=table.unpack(v)
          L.line(im,x,y,x+2,y-h,P.brown,2)
          for _,d in ipairs({-1,1}) do
            L.poly(im,{x+2,y-h,x+d*7,y-h-7,x+d*14,y-h-5,x+d*17,y-h+2,x+d*10,y-h-2},P.green3)
            L.line(im,x+2,y-h,x+d*9,y-h-5,P.grass3)
          end
        end
        houses(im)
        -- A two-lane asphalt bend, with a culvert and a gravel shoulder.
        L.poly(im,{0,184,146,177,233,172,310,158,425,151,480,152,480,159,401,160,323,170,245,186,170,199,0,213},P.concrete3)
        L.poly(im,{0,189,150,181,237,175,311,160,425,153,480,154,480,157,401,159,322,168,245,184,170,196,0,209},P.gray4)
        for _,v in ipairs({{15,199,42,197},{70,195,98,193},{133,189,153,188},{193,183,213,181},{247,175,263,172},{300,165,310,163},{355,159,367,158},{423,155,433,155}}) do L.line(im,v[1],v[2],v[3],v[4],P.paper2) end
        L.poly(im,{0,215,175,200,271,185,370,169,480,165,480,270,0,270},P.paper3)
        L.seed(753)
        for i=1,780 do local x,y=L.randi(0,479),L.randi(169,269)
          if im:getPixel(x,y)==P.paper3 then L.hline(im,x,x+1,y,P.concrete3) end
        end
        L.poly(im,{163,182,184,181,194,186,165,190},P.concrete2)
        L.rect(im,165,187,25,4,P.gray4)
        for x=167,186,4 do L.line(im,x,187,x-1,190,P.gray2) end
      end
      local mask=hillside(im,ridge)
      route(im,ridge and 402 or 184,ridge and 113 or 121)
      local suffix=ridge and "_ridge" or ""
      still("bg_hill_road"..suffix,im)
      local tint=L.newImage(480,270)
      for y=0,155 do for x=0,479 do
        if im:getPixel(x,y)==sky:getPixel(x,y) then L.px(tint,x,y,L.hex("596c80",126)) end
      end end
      still("landslide_dark_sky_hill_road"..suffix,tint)
      if app.params.out then mask:saveAs(app.params.out.."/"..(ridge and "hard-2" or "normal-4").."-slope.png") end
    end
    local water=dofile(root.."/ArtSource/scripts/flood_fx.lua")(L,root).WATER
    local frames={}
    for f=0,3 do
      local im=L.newImage(120,47)
      L.poly(im,{2,1,11,2,20,8,32,10,48,20,63,22,76,27,84,27,99,33,118,36,115,40,95,39,78,35,59,33,46,26,28,23,18,16,10,12},water.dark)
      for y=1,44 do for x=1,118 do
        if L.opaque(im,x,y) and L.opaque(im,x,y+1) and L.opaque(im,x,y-1) then
          local c=water.mid
          if (x-3*f+y*2)%24<9 and y%5==2 then c=water.sheen
          elseif (x+y)%18<11 and y%4==0 then c=water.light end
          L.px(im,x,y,c)
        end
      end end
      frames[#frames+1]=im
    end
    sequence("hill_road_runoff",frames,180)
    local soil=L.newImage(58,18)
    L.poly(soil,{1,12,7,9,14,8,18,4,23,3,30,5,34,4,40,7,47,8,53,11,56,13,52,15,42,16,30,15,23,17,9,16,2,15},P.rock2)
    L.poly(soil,{4,12,15,9,19,5,23,4,32,7,39,6,47,10,37,9,27,10,20,8,13,12},P.rock)
    for _,v in ipairs({{9,13},{24,11},{39,12},{48,13}}) do L.hline(soil,v[1],v[1]+2,v[2],P.gray3) end
    L.line(soil,14,9,11,6,P.green3)
    L.line(soil,14,9,16,5,P.grass3)
    L.outline(soil,P.ink)
    still("hill_road_soil",soil)
    frames={}
    for f=0,5 do
      local im=L.newImage(30,33)
      local y=5+4*f
      L.poly(im,{4,2,9,3,11+f,y-1,13+f,y,10+f,y+2,5+f,y-2},P.dirt)
      L.poly(im,{6,3,8,3,9+f,y-1,8+f,y},P.tan2)
      L.poly(im,{6+f,y-2,11+f,y-3,15+f,y,16+f,y+2,12+f,y+3,7+f,y+1},P.rock2)
      L.hline(im,8+f,12+f,y-1,P.rock)
      frames[#frames+1]=im
    end
    sequence("hill_road_far_slide",frames,150)
  end
  if only.icons_landslide_road then
    local colors={W=P.white,B=P.glass2,b=P.blue2,G=P.grass,g=P.green3,T=P.tan,D=P.dirt2,M=P.rock,S=P.rock2,Y=P.yellow}
    local function icon(name,rows)
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,colors)
      L.outline(img,P.ink)
      local path="UI/icon_landslide_"..name
      L.save(img,path,path)
    end
    icon("long_way",{"","...YYYYYYYY","..YY......YY",".YY........Y",".Y.........Y",".Y..Y......Y","YYYYY......Y",".YYY","..Y",".....MMMM","....MMSSSM"})
    icon("cross_mud",{"....bb","....bb","....bbb","....bbbbb","....WWWWW","","..DDTTDD","DDTTDDDDTTDDD",".DDDDTTDDDD"})
    icon("shortcut",{"..........G",".........GG","........GGG",".......GTGG","......GTGGG",".....GGTTGG","....GGGTGGG","...GGTTGGGG","..GGTTGGGGG",".GTTGGGGGGG"})
    icon("watch_hill",{"....WWW","..WWbbWWW",".WWbbbbWWW","..WWbbWWW","....WWW","","........GG","......GGgg","....GGgggg","..GGgggggg"})
    icon("run_ahead",{".......Y",".......YY",".....YYYYY",".......YY",".......Y",".bb","..bb","...bbb","...WWWW","......bb",".......bbb",".......WWWW"})
  end
end
