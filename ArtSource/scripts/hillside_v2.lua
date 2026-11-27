-- Landslide Batch 1. The flat family yard lies beyond the toe of the slope.
return function(L,root,only)
  local P=L.P
  local water=dofile(root.."/ArtSource/scripts/flood_fx.lua")(L,root).WATER
  local function save(img,name)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local function palm(img,lean)
    local layer=L.newImage(lean and 104 or 77,103)
    local sway=lean and 22 or 0
    local drop=lean and 43 or 0
    for y=22,96 do
      local t=(96-y)/74
      local x=math.floor(24+(17+sway)*t*t)
      local row=y+math.floor(t*drop)
      L.hline(layer,x-2,x+2,row,y%6==0 and P.brown2 or P.brown)
      L.px(layer,x-2,row,P.brown3)
    end
    local cx=41+sway
    local cy=22+drop
    for _,v in ipairs({{-32,9,-15,-17},{-28,22,-17,-6},{-14,28,-13,0},{8,24,5,-6},{28,17,19,-9},{30,4,18,-16},{18,-9,9,-15}}) do
      local ex,ey=cx+v[1],cy+v[2]
      local mx,my=cx+v[3],cy+v[4]
      L.poly(layer,{cx,cy,mx,my,ex,ey,mx,my+5},P.green3)
      L.line(layer,cx,cy-1,mx,my+1,P.grass2)
      for k=1,4 do
        local x=mx+(ex-mx)*k//5
        local y=my+(ey-my)*k//5
        L.line(layer,x,y,x-2,y+5-k//2,P.grass3)
      end
    end
    L.circle(layer,cx-2,cy+2,3,P.brown2)
    L.circle(layer,cx+3,cy+3,2,P.brown)
    L.outline(layer,P.ink)
    L.blit(img,layer,60,55)
  end

  local function banana(img,x,y)
    local plant=L.newImage(52,53)
    L.poly(plant,{23,50,25,32,25,16,29,16,30,51},P.green3)
    L.line(plant,26,48,26,20,P.tan,2)
    for _,leaf in ipairs({
      {26,19,17,7,6,5,1,12,10,9,17,13},
      {26,20,13,14,3,18,0,32,6,25,9,25,12,19},
      {27,18,22,4,25,1,29,9,29,16},
      {27,19,35,7,45,6,50,13,41,10,34,14},
      {28,19,42,16,48,20,51,29,42,24,38,25,36,21}}) do
      L.poly(plant,leaf,P.green2)
      L.line(plant,leaf[1],leaf[2],leaf[3],leaf[4],P.grass)
    end
    L.outline(plant,P.ink)
    L.blit(img,plant,x,y)
  end

  local function house(bg)
    local function rect(x,y,w,h,c) L.rect(bg,x,y,w,h,c) end
    local function poly(points,c) L.poly(bg,points,c) end
    -- Timber siding rests on a concrete skirt, with a shaded gable end.
    rect(321,133,112,72,P.ink)
    rect(322,135,110,54,P.brown3)
    for y=139,185,6 do
      L.hline(bg,322,431,y,P.brown)
      L.hline(bg,322,431,y+1,P.tan)
    end
    rect(322,189,110,14,P.concrete)
    rect(322,189,110,2,P.concrete2)
    poly({433,130,453,100,473,129,473,199,433,205},P.ink)
    poly({434,131,453,103,472,130,472,197,434,203},P.brown)
    for y=137,183,6 do L.line(bg,434,y,471,y-5,P.brown2) end
    poly({434,189,472,184,472,197,434,203},P.concrete3)
    rect(449,122,8,4,P.ink2)
    rect(444,146,15,23,P.brown2)
    for x=446,456,3 do rect(x,148,2,18,P.tan2) end
    poly({310,137,332,97,454,97,432,137},P.ink)
    poly({313,134,333,99,451,99,431,134},P.roof)
    for x=336,449,6 do
      L.line(bg,x,100,x-18,132,P.roof2)
      L.line(bg,x+1,100,x-17,132,P.roof3)
    end
    L.line(bg,454,97,478,129,P.ink,3)
    L.line(bg,454,99,477,130,P.roof3,2)
    rect(311,134,121,4,P.brown2)
    rect(313,134,117,1,P.tan)
    rect(335,150,30,27,P.ink)
    rect(337,152,26,23,P.glass3)
    rect(338,153,24,15,P.glass2)
    L.line(bg,339,165,348,154,P.sky5,2)
    rect(349,152,2,23,P.brown)
    rect(337,163,26,2,P.brown)
    rect(333,176,34,3,P.brown2)
    rect(377,141,32,63,P.brown2)
    rect(379,143,28,59,P.ink2)
    rect(380,144,10,56,P.brown)
    rect(380,145,2,54,P.brown3)
    rect(387,174,2,2,P.yellow3)
    rect(373,202,40,4,P.ink)
    rect(374,202,38,2,P.concrete)
    rect(370,206,46,4,P.concrete3)
    rect(371,206,44,2,P.concrete)
  end

  if only.landslide_hillside or only.landslide_hillside_more then
    local bg=L.newImage(480,270)
    L.vgrad(bg,0,0,480,168,{P.sky2,P.sky4,P.sky5})
    local sky=Image(bg)
    L.poly(bg,{0,135,66,112,110,120,173,92,204,103,255,78,296,98,327,84,371,106,427,87,480,102,480,166,0,166},P.hill)
    L.poly(bg,{0,142,80,124,143,134,193,117,229,124,281,108,330,125,394,107,443,116,480,102,480,171,0,171},P.hill2)
    L.rect(bg,0,156,480,114,P.grass2)
    -- Garden hedge and a low bamboo fence behind the house.
    for x=5,480,11 do
      local y=158+math.floor(3*math.sin(x))
      L.ellipse(bg,x,y,10,7,P.green3)
      L.ellipse(bg,x-2,y-2,8,5,P.grass3)
    end
    for y=169,177,8 do L.rect(bg,199,y,119,2,P.brown) end
    for x=201,318,12 do
      L.rect(bg,x,164,3,20,P.brown2)
      L.vline(bg,x,164,182,P.tan)
    end
    L.poly(bg,{0,209,142,207,202,195,269,188,340,184,480,185,480,270,0,270},P.paper3)
    L.seed(518)
    for i=1,530 do
      local x,y=L.randi(0,479),L.randi(184,269)
      if bg:getPixel(x,y)==P.paper3 then L.hline(bg,x,x+i%3,y,i%4==0 and P.paper2 or P.concrete3) end
    end
    local slope=L.newImage(480,270)
    L.poly(slope,{0,75,17,67,32,64,48,66,66,78,89,94,110,102,138,128,157,158,178,178,193,189,206,201,198,207,162,212,107,215,48,212,0,214},P.grass2)
    L.poly(slope,{0,78,31,65,47,68,88,98,65,89,44,87,19,92,0,103},P.grass)
    L.poly(slope,{0,134,27,113,56,98,88,100,114,109,139,134,117,127,92,115,70,117,35,130,0,159},P.grass3)
    L.poly(slope,{0,188,32,158,60,150,97,153,127,143,146,145,157,161,130,157,102,171,66,166,23,188,0,202},P.grass3)
    -- Incised dry gully and a separate switchback footpath.
    L.poly(slope,{6,103,28,119,36,135,62,147,70,166,104,180,119,200,139,210,116,205,97,188,63,172,56,153,30,140,24,124},P.dirt2)
    L.poly(slope,{11,109,28,121,34,138,59,150,66,168,102,185,118,202,101,191,63,171,56,153,30,140,25,124},P.dirt)
    L.poly(slope,{65,83,97,105,118,115,103,123,139,144,152,158,140,165,177,186,199,199,192,204,164,184,132,166,145,157,135,147,97,124,112,115,93,106},P.tan2)
    L.poly(slope,{0,205,43,203,99,207,157,202,192,198,205,201,197,206,160,211,104,215,44,211,0,214},P.dirt)
    for i=1,840 do
      local x,y=L.randi(0,203),L.randi(65,212)
      local c=slope:getPixel(x,y)
      if c==P.grass2 or c==P.grass3 then
        L.line(slope,x,y,x+2,y-1,c==P.grass2 and P.grass or P.grass2)
      end
    end
    L.outline(slope,P.ink)
    banana(slope,15,91)
    if only.landslide_hillside_more then
      dofile(root.."/ArtSource/scripts/hillside_more_v2.lua")(L,root,bg,sky,slope,palm)
    end
    if only.landslide_hillside then
    palm(slope)
    L.blit(bg,slope,0,0)
    house(bg)
    save(bg,"bg_hillside")
    if app.params.out then slope:saveAs(app.params.out.."/slope-mask-source.png") end
    local tint=L.newImage(480,270)
    for y=0,155 do for x=0,479 do
      if bg:getPixel(x,y)==sky:getPixel(x,y) then L.px(tint,x,y,L.hex("566677",134)) end
    end end
    save(tint,"landslide_dark_sky_hillside")

    local crack=L.newImage(36,9)
    L.poly(crack,{0,6,8,3,13,4,20,1,24,3,35,1,27,5,21,4,14,8,9,6},P.paper2)
    L.poly(crack,{0,6,8,3,13,4,20,1,24,3,35,1,26,4,20,3,14,7,9,5},P.ink)
    L.line(crack,14,5,12,1,P.brown2)
    save(crack,"hillside_crack")
    local frames={}
    for f=0,3 do
      local im=L.newImage(56,24)
      L.ellipse(im,7,4,5,2,P.rock2)
      L.hline(im,4,8,2,P.rock3)
      L.poly(im,{5,5,9,5,14,10,14,12,23,16,21,18,10,13,10,10},f==0 and P.glass3 or water.dark)
      L.line(im,7,5,12,10,water.sheen)
      L.line(im,12,11,21,16,f==0 and P.glass2 or water.light,2)
      local w=5+4*f
      L.ellipse(im,20+4*f,17,w,2+f,water.dark)
      L.ellipse(im,20+4*f,16,w-1,1+f,f==0 and P.glass2 or water.mid)
      L.hline(im,18+f,23+2*f,16,water.sheen)
      if f>0 then L.hline(im,23+2*f,26+3*f,18,water.light) end
      frames[#frames+1]=im
    end
    L.saveFrames(frames,"Level/v2/hillside_spring","Level/v2/hillside_spring",220)
    local soil=L.newImage(200,90)
    local mass={0,26,18,29,32,35,49,32,65,37,76,46,95,51,106,58,130,62,144,66,155,66,172,72,185,74,190,79,184,82,161,84,148,83,129,87,99,85,80,88,52,84,24,87,0,83}
    L.poly(soil,mass,P.rock2)
    L.poly(soil,{0,26,18,29,32,35,49,32,65,37,76,46,95,51,106,58,130,62,145,69,119,66,103,68,81,60,64,58,47,49,29,48,15,42,0,44},P.rock)
    -- Overlapping lobes give the settled deposit volume at phone size.
    for _,v in ipairs({{13,60,16,5},{41,67,17,6},{72,74,16,5},{105,75,12,4},{137,77,9,3}}) do
      L.ellipse(soil,v[1],v[2],v[3],v[4],P.gray4)
      L.ellipse(soil,v[1]-2,v[2]-2,v[3]-2,v[4]-1,P.rock2)
      L.hline(soil,v[1]-v[3]+3,v[1]-3,v[2]-v[4],P.rock)
    end
    for i=1,270 do
      local x,y=L.randi(1,185),L.randi(32,85)
      if soil:getPixel(x,y)==P.rock2 then
        L.hline(soil,x,math.min(x+1,185),y,P.gray4)
      end
    end
    for _,v in ipairs({{11,52,4},{55,65,3},{83,75,4},{120,70,2},{150,77,3},{35,78,2}}) do
      L.ellipse(soil,v[1],v[2],v[3],2,P.gray4)
      L.hline(soil,v[1]-1,v[1]+1,v[2]-1,P.rock3)
    end
    for _,v in ipairs({{19,53},{71,57},{102,78},{138,72}}) do
      L.line(soil,v[1]-3,v[2]-3,v[1],v[2],P.green3,2)
      L.line(soil,v[1],v[2],v[1]+4,v[2]-3,P.grass3)
    end
    L.outline(soil,P.ink)
    save(soil,"hillside_soil")
    local rope=L.newImage(25,29)
    for _,x in ipairs({3,21}) do
      L.rect(rope,x,4,3,23,P.brown2)
      L.vline(rope,x,4,24,P.tan)
    end
    for x=3,22 do
      local y=9+math.floor(3*math.sin((x-3)*math.pi/19))
      L.vline(rope,x,y,y+2,P.brown2)
      L.px(rope,x,y,x%4==0 and P.tan2 or P.paper2)
    end
    L.outline(rope,P.ink)
    save(rope,"hillside_rope")
    local w=L.F.width("LANDSLIDE")+6
    local board=L.newImage(w,36)
    L.rect(board,w//2-2,18,5,17,P.brown2)
    L.vline(board,w//2-1,19,34,P.brown3)
    L.rect(board,0,0,w,21,P.ink)
    L.rect(board,1,1,w-2,19,P.paper3)
    L.rect(board,2,2,w-4,17,P.paper)
    for i,word in ipairs({"LANDSLIDE","ZONE"}) do L.text(board,(w-L.F.width(word))//2,3+(i-1)*8,word,P.ink) end
    save(board,"hillside_sign")
    local toy=L.newImage(15,9)
    local original=Image{fromFile=root.."/Assets/Art/Level/v2/home_toy.png"}
    L.blit(toy,original,0,0)
    for x=0,14 do
      local lip=6+(x//3)%2
      for y=lip,8 do L.px(toy,x,y,y==lip and P.rock or P.rock2) end
    end
    save(toy,"hillside_toy_soil")
  end
    end
  if only.icons_landslide_hillside then
    dofile(root.."/ArtSource/scripts/hillside_choices_v2.lua")(L)
  end
  if only.icons_landslide_signs then
    local key={W=P.white,B=P.glass2,S=P.rock,h=P.skin1,H=P.skin2,T=P.tan,D=P.brown2,Y=P.yellow,R=P.red,G=P.green2,M=water.mid,L=water.sheen}
    local function icon(name,rows)
      local im=L.newImage(16,16)
      L.map(im,1,1,rows,key)
      L.outline(im,P.ink)
      local path="UI/icon_landslide_"..name
      L.save(im,path,path)
    end
    icon("jump_crack",{"",".....YYY","....YY.YY","...YY...YY.Y","..YY.....YYY","..........YY","","","TTTTT..TTTTTT","TTTT..TTTTTTT","TTTTT..TTTTTT","TTT...TTTTTTT"})
    icon("splash_mud",{"","......BB",".....BBBB",".....BLLB","......BB","..B.......B","...B.....B",".....B.B","..MMMMMMMMM",".MMLLLMMMMLM","..MMMMMMMMM"})
    icon("look_spring",{"..WWWWW",".WWhDhWW","WWWhDhWWW",".WWhhhWW","..WWWWW","","...SS","....BB",".....LL","......BMMMM","....MMMLLLMMM",".....MMMMMMM"})
    icon("stay_back",{"..........TT","..........TT",".....G....TT","....GG....DD","...GGGGGGGTT","..GGGGGGGGTT","...GGGGGGGTT","....GG....TT",".....G....TT","..........TT"})
    icon("climb_soil",{".......YY","......YYY",".....YYYY","....YY..Y","...YY",".......SS","......SSSS","....SSSSSSS","...SSSDSSSSS",".SSSSSSSSSDSS","SSSDSSSSSSSSSS"})
    icon("toy_soil",{"",".....RRRR","....RBBRRR","...RRBBRRRR",".RRRRRRRRRRRR",".RRRRRRRRRRRR","..DDSDSSDDSS","SSSSSSSSSSSSS",".SSSDSSSSSSSS"})
    icon("look_close",{"...YYYY","..YWWWWY",".YWWBBWWY",".YWWBBWWY",".YWWWWWWY","..YWWWWY","...YYYYD",".......DD","........TT",".........TT","..........TT"})
  end
end
