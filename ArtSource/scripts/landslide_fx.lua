-- Indoor Landslide Settings keep the shared glass, desk and mat registrations.
return function(L,root,only)
  local P=L.P
  local function save(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local function box(im,x,y,w,h,light,mid,dark)
    L.rect(im,x,y,w,h,P.ink)
    L.rect(im,x+1,y+1,w-2,h-2,mid)
    L.hline(im,x+1,x+w-2,y+1,light)
    L.vline(im,x+1,y+2,y+h-2,light)
    L.hline(im,x+2,x+w-2,y+h-2,dark)
  end
  local function tile(im,start,light,mid,dark)
    L.rect(im,0,start,480,270-start,mid)
    local prev=start
    for _,y in ipairs({start+9,start+25,start+49,270}) do
      L.hline(im,0,479,prev,dark)
      L.hline(im,0,479,prev+1,light)
      for x=-40,520,52 do
        L.line(im,x+math.floor((x-240)*(prev-start)/320),prev,x+math.floor((x-240)*(y-start)/320),y,dark)
      end
      prev=y
    end
  end
  local function cupboard(im,x,y,w,h)
    box(im,x,y,w,h,P.tan,P.brown3,P.brown2)
    for j=0,1 do
      local dx=x+4+j*(w//2-1)
      box(im,dx,y+4,w//2-6,h-10,P.brown3,P.brown,P.brown2)
      for yy=y+10,y+h-11,4 do L.hline(im,dx+3,dx+w//2-11,yy,P.tan2) end
      L.px(im,dx+3,y+h//2,P.paper)
    end
  end
  if only.landslide_home then
    local home=L.newImage(480,270)
    L.rect(home,0,0,480,270,P.paper)
    L.rect(home,0,0,480,15,P.brown)
    for x=0,480,29 do L.line(home,x,0,x-9,14,P.brown2) end
    L.rect(home,0,15,480,4,P.brown2)
    L.rect(home,0,174,480,29,P.hill)
    L.rect(home,0,173,480,3,P.teal2)
    tile(home,202,P.concrete,P.concrete2,P.concrete3)
    box(home,22,104,59,100,P.brown3,P.brown,P.brown2)
    box(home,27,109,49,91,P.tan,P.brown3,P.brown2)
    for _,y in ipairs({114,157}) do box(home,32,y,39,33,P.brown3,P.tan2,P.brown) end
    L.rect(home,65,153,5,3,P.ink2)
    -- Side table with turned feet and a woven runner.
    for _,x in ipairs({201,253}) do box(home,x,184,6,32,P.tan,P.brown,P.brown2) end
    box(home,196,179,68,8,P.tan,P.brown3,P.brown2)
    L.rect(home,213,180,25,5,P.teal)
    for x=214,236,4 do L.line(home,x,180,x+3,183,P.teal3) end
    box(home,297,65,75,79,P.tan,P.brown3,P.brown2)
    L.rect(home,304,72,60,65,P.glass3)
    L.vline(home,333,72,136,P.brown2)
    box(home,293,140,83,4,P.tan,P.brown3,P.brown2)
    for _,x in ipairs({281,370}) do
      box(home,x,68,11,70,P.tan,P.brown3,P.brown2)
      for yy=74,132,6 do L.hline(home,x+2,x+8,yy,P.brown2) end
    end
    box(home,209,103,41,30,P.brown3,P.paper3,P.brown2)
    for x=213,244,8 do
      L.poly(home,{x,119,x+4,112,x+8,119,x+4,126},P.brown)
      L.line(home,x+2,119,x+4,116,P.paper)
    end
    cupboard(home,399,161,61,42)
    -- Earthenware plant pot with upright leaves at the far end of the room.
    L.poly(home,{422,149,438,149,435,161,425,161},P.brown)
    L.hline(home,422,438,149,P.tan2)
    for _,v in ipairs({{-13,-10},{-6,-17},{3,-19},{12,-11},{15,-5}}) do
      L.poly(home,{430,149,429+v[1],147+v[2],433+v[1],145+v[2],433,149},P.green3)
      L.line(home,431,147,431+v[1],146+v[2],P.grass3)
    end
    save("bg_home_landslide",home)
    local crack=L.newImage(12,38)
    local pts={6,1,8,9,5,15,8,19,5,24,6,29,3,36}
    for i=1,#pts-2,2 do
      L.line(crack,pts[i]+1,pts[i+1],pts[i+2]+1,pts[i+3],P.wall3,2)
      L.line(crack,pts[i],pts[i+1],pts[i+2],pts[i+3],P.ink2,i==5 and 2 or 1)
    end
    L.line(crack,6,23,1,18,P.brown2)
    save("home_wall_crack",crack)
    local poster=L.newImage(15,18)
    L.poly(poster,{1,12,9,2,13,2,14,5,5,16,2,16},P.brown2)
    L.poly(poster,{2,12,10,3,13,4,4,15},P.paper)
    L.line(poster,4,14,12,5,P.paper3)
    L.line(poster,6,9,8,11,P.teal)
    L.ellipse(poster,3,14,3,2,P.brown2)
    L.hline(poster,1,4,13,P.paper2)
    save("home_poster",poster)
    local window=L.newImage(60,65)
    L.vgrad(window,0,0,60,65,{P.mount,P.mount3,P.gray1})
    L.poly(window,{0,32,12,27,24,31,41,21,59,27,60,65,0,65},P.hill)
    L.poly(window,{0,40,13,31,25,35,35,43,47,47,60,45,60,65,0,65},P.green3)
    L.poly(window,{5,43,12,41,15,42,22,42,26,45,20,46,10,45},P.rock2)
    L.hline(window,11,15,42,P.rock)
    L.rect(window,0,52,60,13,P.grass3)
    L.rect(window,29,0,2,65,P.brown)
    for y=6,60,10 do L.hline(window,0,59,y,P.metal2) end
    save("home_window_hill",window)

    local room=L.newImage(480,270)
    L.rect(room,0,0,480,270,P.wall)
    L.rect(room,0,0,480,12,P.teal2)
    for x=0,480,64 do L.rect(room,x,0,3,12,P.teal) end
    L.rect(room,0,150,480,53,P.hill)
    L.rect(room,0,149,480,3,P.teal2)
    tile(room,204,P.paper2,P.concrete,P.concrete2)
    -- Bench behind the desk; the seat meets the seated Student's bottom row.
    box(room,226,207,34,4,P.tan,P.brown3,P.brown2)
    for _,leg in ipairs({228,255}) do box(room,leg,211,4,14,P.tan,P.brown,P.brown2) end
    box(room,15,98,63,106,P.tan,P.brown3,P.brown2)
    box(room,20,103,53,97,P.glass2,P.teal,P.teal2)
    box(room,26,110,41,29,P.metal,P.glass2,P.glass3)
    L.line(room,30,133,45,118,P.sky5,2)
    L.rect(room,61,152,8,3,P.ink2)
    -- A geography lesson: a chalk relief drawing, contour lines and a compass rose.
    box(room,116,63,180,75,P.tan,P.brown,P.brown2)
    L.rect(room,121,68,170,64,P.teal2)
    for _,v in ipairs({{131,120,151,103,160,112,179,88,201,118,215,103,238,122},
      {144,120,151,108,156,114},{167,113,179,93,187,107},{183,114,190,119,203,121}}) do
      for j=1,#v-2,2 do L.line(room,v[j],v[j+1],v[j+2],v[j+3],P.paper2) end
    end
    L.poly(room,{262,80,266,94,278,98,266,101,262,115,259,101,248,98,259,95},P.paper2)
    L.poly(room,{262,83,262,98,259,98},P.teal3)
    L.rect(room,126,132,157,3,P.brown3)
    L.hline(room,237,246,131,P.white)
    box(room,335,61,80,94,P.tan,P.brown3,P.brown2)
    L.rect(room,341,67,68,82,P.glass3)
    L.rect(room,374,67,3,82,P.brown)
    for y=72,148,10 do L.hline(room,341,408,y,P.glass2) end
    cupboard(room,429,149,44,54)
    for j=0,5 do
      local h=11+(j*7)%10
      box(room,434+j*5,149-h,5,h,P.paper2,({P.teal,P.brown3,P.roof})[j%3+1],P.brown)
    end
    L.circle(room,312,87,10,P.ink2)
    L.circle(room,312,87,8,P.paper)
    L.line(room,312,87,312,81,P.brown2)
    L.line(room,312,87,317,89,P.brown2)
    save("bg_classroom_landslide",room)
    local opening=L.newImage(54,98)
    L.rect(opening,0,0,54,98,P.mount3)
    L.poly(opening,{0,49,17,42,35,49,54,40,54,98,0,98},P.hill2)
    L.rect(opening,0,73,54,25,P.concrete2)
    L.poly(opening,{0,0,17,8,17,91,0,98},P.teal2)
    L.line(opening,16,8,16,91,P.ink)
    L.px(opening,12,54,P.paper2)
    save("classroom_landslide_door_open",opening)

    -- A covered barangay pavilion with masonry end bays and a woven floor mat.
    -- Its skyline and open truss bays retain the imported Typhoon sky-mask registration.
    local reference=Image{fromFile=root.."/Assets/Art/Level/v2/bg_evacuation_area.png"}
    local hall=Image(reference)
    L.rect(hall,0,0,439,62,P.teal2)
    for x=-10,448,10 do L.line(hall,x,0,x-12,61,P.teal,2) end
    for y=11,61,19 do L.hline(hall,0,438,y,P.ink2); L.hline(hall,0,438,y+1,P.metal2) end
    L.rect(hall,0,153,480,117,P.concrete2)
    tile(hall,157,P.concrete,P.concrete2,P.concrete3)
    -- Back wall sits below the open sky strip. Two bays frame the central aisle.
    L.rect(hall,0,128,480,30,P.hill)
    L.rect(hall,0,151,480,7,P.teal2)
    for x=4,480,22 do L.rect(hall,x,132,12,4,P.hill2) end
    for _,x in ipairs({14,109,205,301,397}) do
      box(hall,x,81,8,76,P.concrete,P.concrete2,P.concrete3)
      box(hall,x-2,148,12,10,P.metal2,P.gray3,P.gray4)
      L.line(hall,x+8,94,x+20,82,P.gray4,2)
    end
    local function tent(x,y,w,roof,light)
      local h=w//2
      L.poly(hall,{x,y+h,x+4,y+13,x+w//3,y,x+w*2//3,y,x+w-3,y+14,x+w,y+h},P.ink)
      L.poly(hall,{x+2,y+h-2,x+6,y+14,x+w//3,y+2,x+w*2//3-1,y+2,x+w-5,y+15,x+w-2,y+h-2},roof)
      L.poly(hall,{x+6,y+h-2,x+w//3,y+3,x+w//2,y+7,x+w//2-3,y+h-2},light)
      L.poly(hall,{x+w//4,y+h-1,x+w//2-3,y+9,x+w*2//3,y+h-1},P.ink2)
      L.line(hall,x+w//3,y+3,x+w//2-3,y+9,P.paper2)
      L.line(hall,x+w*2//3,y+3,x+w-6,y+h-4,P.brown2)
      L.line(hall,x+4,y+h-4,x-3,y+h+1,P.gray4)
    end
    tent(94,130,62,P.teal,P.teal3)
    tent(307,129,69,P.roof,P.roof3)
    L.rect(hall,84,80,4,77,P.gray4)
    local sign="EVACUATION CENTER"
    box(hall,8,84,L.F.width(sign)+10,30,P.paper,P.paper2,P.paper3)
    L.text(hall,13,95,sign,P.ink)
    -- Raised edge at the near left and patterned banig kept clear of the aisle.
    L.poly(hall,{94,224,203,224,212,248,83,248},P.brown2)
    L.poly(hall,{96,226,201,226,208,245,87,245},P.tan)
    for y=228,243,4 do
      local left=95-(y-226)//3
      L.hline(hall,left,201+(y-226)//3,y,P.brown)
      for x=left+4,199,8 do L.line(hall,x,y,x+3,y+2,P.paper2) end
    end
    save("bg_evacuation_area_landslide",hall)
    local tint=Image{fromFile=root.."/Assets/Art/Level/v2/typhoon_dark_sky_evacuation_area.png"}
    for y=0,269 do for x=0,479 do
      if hall:getPixel(x,y)~=reference:getPixel(x,y) then L.px(tint,x,y,L.CLEAR) end
    end end
    save("landslide_dark_sky_evacuation_area",tint)
  end
  if only.icons_landslide_indoor then
    local key={W=P.white,T=P.tan,S=P.rock,h=P.skin1,H=P.skin2,D=P.brown2,G=P.green2,B=P.glass2,b=P.blue2,Y=P.yellow,R=P.roof}
    local function icon(name,rows)
      local im=L.newImage(16,16)
      L.map(im,1,1,rows,key)
      L.outline(im,P.ink)
      local path="UI/icon_landslide_"..name
      L.save(im,path,path)
    end
    icon("tell_dad_wall",{"..WWWW...TTTTT",".WWWWWW..TTDTT",".WWWWWW..TDTTT","..WWWW...TTDTT","...W.....TDTTT","","..DDD","..DhhD","..hhh","...H","..bbb"})
    icon("poster",{".........WWW","........WTTWW",".......WWWTW","......WWWWW",".....WWWWW","....WGGWW","...WWWGG","..WWWWW",".WWWWW",".WTWW","..TT"})
    icon("pick_crack",{"TTTDTTTT","TTDTTTTT","TTTDTTTT","TTTTDTTT","TTTDTTTT","TTDTTThhhh","TTTDThhHHh","TTTTDTTThhh","TTTDTTTT.hh"})
    icon("push_door",{"TTTTTTTT","TDDDDDDT","TDDDDDDT","TDTTTTDT","TDTTTTDT","TDDDDDDT","TDDDYDDT","TDDDDDhT","TDDDDhhh","TTTTTThhhh"})
    icon("hill_road",{"........GG",".......Ggg","......Gggg",".....Ggggg","....Gggggg","...Ggggggg","..SSSSS",".....SSSSS","......SWWSS","...SSSWWSS",".SSSWSSS","SSWWSS"})
    icon("gate",{"..S........S","..S........S","..S..S..S..S","..SSSSSSSSSS","..S..S..S..S","..S..S..S..S","..SSSSSSSSSS","..S..S..S..S","..S........S"})
    icon("check_house",{".....RR","....RRRR","...RRRRRR","..RRRRRRRR","...TTTTTT","...TBBDTT","...TBBDTT","...TTTTTT","",".....WWWW","...WWBbbBWW",".....WWWW"})
  end
end
