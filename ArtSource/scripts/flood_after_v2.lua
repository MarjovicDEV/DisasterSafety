return function(L,root,only)
  local P=L.P
  local function save(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  if only.flood_after then
    local basin=L.newImage(36,16)
    L.poly(basin,{2,5,27,5,24,13,20,14,8,14,5,12},P.teal2)
    L.poly(basin,{3,5,23,5,20,12,8,12},P.teal)
    L.ellipse(basin,14,5,12,3,P.teal3)
    L.ellipse(basin,14,5,10,2,P.water2)
    L.hline(basin,8,17,5,P.water3)
    L.rect(basin,29,10,5,3,P.paper2)
    L.hline(basin,29,32,10,P.white)
    L.outline(basin,P.ink)
    save("evacuation_area_basin",basin)
    local dipper=L.newImage(12,8)
    L.line(dipper,4,4,10,1,P.teal3)
    L.ellipse(dipper,4,4,3,2,P.teal2)
    L.hline(dipper,2,5,3,P.teal3)
    L.outline(dipper,P.ink)
    save("evacuation_area_dipper",dipper)
    local mud=L.newImage(105,33)
    -- Interrupted silt line on the wall beside the open door.
    for x=71,103 do
      local y=7+(x//7)%2
      L.vline(mud,x,y,y+1+(x//4)%3,P.brown2)
    end
    for _,e in ipairs({{14,23,12,2},{29,25,9,2},{49,28,15,2},{66,23,11,2},{82,29,8,1}}) do
      L.ellipse(mud,e[1],e[2],e[3],e[4],P.dirt2)
      L.hline(mud,e[1]-e[3]+2,e[1]+2,e[2],P.brown2)
    end
    save("home_after_mud",mud)
    local loaf=L.newImage(20,13)
    L.poly(loaf,{1,3,5,4,8,2,16,3,18,6,17,10,6,11,3,9},P.gray2)
    L.ellipse(loaf,11,7,6,3,P.tan)
    L.ellipse(loaf,8,7,3,2,P.brown)
    L.px(loaf,14,8,P.brown2)
    L.line(loaf,5,4,8,3,P.paper)
    L.hline(loaf,6,15,10,P.metal)
    L.px(loaf,3,5,P.blue2)
    L.outline(loaf,P.ink)
    save("home_after_bread",loaf)
    local dirt=L.newImage(98,12)
    for _,e in ipairs({{16,7,15,2},{39,5,15,3},{62,8,11,2},{84,7,12,2}}) do
      L.ellipse(dirt,e[1],e[2],e[3],e[4],P.dirt2)
      L.hline(dirt,e[1]-5,e[1]+4,e[2]+1,P.brown2)
    end
    -- Leave the tree trunk visible through the road-edge layer at (279,177).
    for y=0,11 do for x=65,75 do L.px(dirt,x,y,0) end end
    save("street_mud",dirt)
    local water=dofile(root.."/ArtSource/scripts/flood_fx.lua")(L,root).WATER
    local street=Image{fromFile=root.."/Assets/Art/Level/v2/bg_street.png"}
    local frames={}
    for f=0,1 do
      local img=L.newImage(65,17)
      L.ellipse(img,32,8,31,7,water.dark)
      L.ellipse(img,32,8,29,6,water.mid)
      L.hline(img,12,24,6,water.light)
      L.hline(img,19+f,32+f,10,water.sheen)
      L.hline(img,44-f,52-f,6,water.sheen)
      -- One small floating slipper.
      L.line(img,38,8+f,42,7+f,P.teal2,2)
      L.px(img,40,7+f,P.teal3)
      for y=0,16 do for x=0,64 do
        if street:getPixel(281+x,149+y)~=P.gray3 then L.px(img,x,y,0) end
      end end
      frames[#frames+1]=img
    end
    L.saveFrames(frames,"Level/v2/flood_street_leftover","Level/v2/flood_street_leftover",400)
  end
  if only.icons_flood_after then
    local selected={}
    for n in (app.params.icons or ""):gmatch("[^,%s]+") do selected[n]=true end
    local colors={B=P.blue2,b=P.water3,W=P.white,G=P.teal,g=P.teal2,H=P.skin1,h=P.skin2,
      M=P.metal2,m=P.gray4,Y=P.tan,D=P.brown2,N=P.navy,R=P.red,T=P.brown3}
    local function icon(n,rows)
      if next(selected) and not selected["flood_"..n] then return end
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,colors)
      L.outline(img,P.ink)
      local path="UI/icon_flood_"..n
      L.save(img,path,path)
    end
    icon("wash_feet",{".GGG...GG",".ggGGGG","..ggg..b",".......bb",".......b","..HH....b","..Hh","..HHHHH","..hhhhhh"})
    icon("wipe_shirt",{"..WW...WW",".WWWNNNWWW","WWWWWWWWWWW","..WWWWWWW","..WWWWHHH","..WWWHHhh","..WWWWhhh","..WWWWWWW"})
    icon("let_dry",{"..b.....b","...b...b",".........",".....HH",".....Hh",".....Hh",".....HHHH",".....hhhhh"})
    icon("give_bread",{"....YYYY","...YYDYYT","...DYYTTT","...TTTTTT",".........HH","..HHHHHHHH","..HHHHhhh","HHHHhhh"})
    icon("eat_bread",{"....HHHHH","...HHHHHHh","...HHHNhHh","....HHHHHh",".....DDHh",".YYYY.hh","YYDYYT","DYYTTT","TTTTTT"})
    icon("wipe_bread",{"....YYYY","...YYDYYT","...DYYTTT","...TTTTTT","..bbWWW",".bbWWWb","bbWWWbb"})
    icon("feed_cat",{".......T....T",".......TT..TT",".......TTTTTT",".......TNTTNT","........TTTT",".YYYY....DD","YYDYYT","TTTTTT"})
    icon("wait_check",{"..MMMMMM","..MWBBWM","..MWBBWM","..MMMMMM",".......GG..G","........GGG",".........G"})
    icon("turn_on_tv",{".MMMMMMMMMMM",".MBBBBBBBBBM",".MBbbbbBBBBM",".MBbbBBBBBBM",".MBBBBBBBBBM",".MMMMMMMMMMM",".....mm","...mmmmmm"})
    icon("plug_fan",{"......bbb",".....bBNBb",".....bNBNb","......bbb",".......M","..MM...M","..BB...M","...B..MMM"})
    icon("switch_box",{"..MMMMMMMM","..MmmmmmmM","..MmRRRRmM","..MmRRRmmM","..MmmmmmmM","..MmmWWmmM","..MMMMMMMM"})
    icon("stay_mom",{"..DDD","..HHH","..HhH....NN","..RRR...HHH",".RRRRR..HHh",".RRRRR..WWW","..NNN...WWW","..N.N...NNN"})
    icon("see_street",{"....MMMMM","...MbbBBbM","...MbbBBbM","....MMMMM",".......MM","........MM","..........","DDWWDDWWDDD"})
    icon("play_mud",{"....HHHH","....HHHh","....HHHh",".....Hh","...TTDDTT","..DDTTDDTT","TTDDDDTTTDD"})
    icon("pick_water",{".....HHHH","....HHHHh","...HHHHh","...H.Hh",".........GG",".........gg","bbDDDDbbDDDD","DDDbbDDDDDbb"})
  end
end
