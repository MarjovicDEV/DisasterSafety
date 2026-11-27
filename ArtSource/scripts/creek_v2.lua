-- Hard 3: the foreground path leads home. The creek narrows toward a distant crossing.
return function(L,root,only)
  local P=L.P
  local water=dofile(root.."/ArtSource/scripts/flood_fx.lua")(L,root).WATER
  local function edges(x)
    local t=x/385
    local far=math.floor(125-18*t+4*math.sin(x/33))
    return far, far+math.max(1,math.floor(53*(1-t)+4*math.sin(x/30)*(1-t)))
  end
  -- Three-quarter cottages: a sunlit long facade and a shaded gable end.
  local function house(img,x,base,w,h,roof,wall,porch)
    local y=base-h
    local side=math.floor(w/3)
    local ridge=y-math.floor(h*0.48)
    local peak=x+w+side//2
    L.rect(img,x,y,w,h,P.ink)
    L.rect(img,x+1,y+1,w-1,h-2,wall[1])
    for row=y+4,base-3,4 do
      L.hline(img,x+1,x+w-1,row,wall[2])
    end
    L.poly(img,{x+w,y,peak,ridge,x+w+side,y-2,x+w+side,base-3,x+w,base},P.ink)
    L.poly(img,{x+w+1,y,peak,ridge+2,x+w+side-1,y-2,x+w+side-1,base-4,x+w+1,base-1},wall[2])
    L.rect(img,peak-2,y-2,5,2,P.brown2)
    L.rect(img,peak-2,y+3,5,6,P.ink2)
    L.vline(img,peak,y+4,y+7,P.tan)
    L.poly(img,{x-4,y+2,x+side//2-1,ridge,peak,ridge,x+w+2,y+2},P.ink)
    L.poly(img,{x-2,y,x+side//2,ridge+1,peak-2,ridge+1,x+w,y},roof[2])
    for rx=x+side//2+4,peak-2,5 do
      L.line(img,rx,ridge+2,rx-side//2-1,y-1,roof[3])
    end
    L.hline(img,x+side//2,peak-1,ridge+1,roof[1])
    L.line(img,peak,ridge+1,x+w+side+2,y-2,roof[3],2)
    local win=x+3
    local ww=math.max(6,w//3)
    L.rect(img,win,y+5,ww,8,P.ink2)
    L.rect(img,win+1,y+6,ww-2,5,P.glass2)
    L.hline(img,win+1,win+ww-2,y+10,P.glass3)
    L.vline(img,win+ww//2,y+6,y+11,P.brown2)
    L.hline(img,win-1,win+ww,y+13,P.brown)
    local door=x+w-9
    L.rect(img,door,base-h//2-3,7,h//2+2,P.ink)
    L.rect(img,door+1,base-h//2-2,5,h//2,P.brown)
    L.px(img,door+4,base-5,P.tan)
    L.hline(img,x,x+w,base,P.gray4)
    if porch then
      L.poly(img,{door-13,y+7,door+8,y+7,door+11,y+12,door-16,y+12},P.ink)
      L.poly(img,{door-12,y+8,door+7,y+8,door+9,y+11,door-14,y+11},roof[2])
      for _,px in ipairs({door-14,door+8}) do
        L.rect(img,px,y+13,2,base-y-12,P.brown2)
        L.vline(img,px,y+13,base,P.tan)
      end
      L.hline(img,door-14,door+9,base+1,P.concrete3)
      L.hline(img,door-5,door+8,base+2,P.concrete)
    end
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
  if only.creek then
    local bg=L.newImage(480,270)
    L.vgrad(bg,0,0,480,133,{P.mount,P.mount3,P.gray1})
    L.poly(bg,{0,108,52,85,85,93,144,69,207,95,263,80,303,88,357,68,414,85,479,68,479,137,0,137},P.hill2)
    L.poly(bg,{0,115,75,100,125,109,192,94,240,108,311,88,378,101,438,83,479,92,479,139,0,139},P.hill)
    for x=0,479 do
      local far,near=edges(math.min(x,385))
      L.vline(bg,x,far-12,269,P.grass2)
      L.vline(bg,x,far-5,far-1,P.grass3)
      if x<=385 then
        L.vline(bg,x,far,near,P.dirt2)
        L.vline(bg,x,near+1,near+5,P.brown)
        L.px(bg,x,near+6,P.grass)
      end
    end
    -- The dry path climbs toward the houses on the right.
    L.poly(bg,{0,219,90,216,175,208,249,201,323,181,393,150,447,132,479,135,479,145,404,166,344,197,263,220,171,240,74,255,0,260},P.tan2)
    L.poly(bg,{0,221,90,218,175,210,249,203,324,183,394,152,448,134,479,136,479,143,403,164,343,195,262,218,170,238,74,253,0,258},P.tan)
    L.seed(412)
    for i=1,1150 do
      local x,y=L.randi(0,478),L.randi(115,269)
      local c=bg:getPixel(x,y)
      if c==P.grass2 then
        L.px(bg,x,y,P.grass3)
        if i%4==0 then L.px(bg,x+1,y-1,P.grass) end
      elseif c==P.tan and i%3==0 then L.hline(bg,x,x+1,y,P.tan2) end
    end
    local red={P.roof3,P.roof,P.roof2}
    local tin={P.gray1,P.metal2,P.gray4}
    local green={P.teal3,P.teal,P.teal2}
    local timber={P.brown3,P.brown}
    local plaster={P.paper,P.paper2}
    local sage={P.hill,P.hill2}
    -- Far-bank lane, with rooflines at different heights and short garden fences.
    for _,site in ipairs({{32,108,21},{126,103,15},{235,106,17},{306,97,10}}) do
      local x,y,r=table.unpack(site)
      L.rect(bg,x-1,y,3,10,P.brown2)
      L.ellipse(bg,x,y-4,r,9,P.green3)
      L.ellipse(bg,x-3,y-7,r-3,7,P.grass3)
    end
    for _,span in ipairs({{8,116,119},{167,230,110},{249,303,107}}) do
      local lo,hi,y=table.unpack(span)
      L.hline(bg,lo,hi,y,P.brown)
      L.hline(bg,lo,hi,y+3,P.brown)
      for x=lo,hi,6 do L.vline(bg,x,y-2,y+5,P.tan2) end
    end
    house(bg,11,115,24,16,tin,plaster,false)
    house(bg,58,118,37,22,red,timber,true)
    house(bg,116,105,24,15,green,sage,false)
    house(bg,156,109,32,21,tin,timber,false)
    house(bg,220,105,29,19,red,plaster,false)
    house(bg,272,102,25,16,green,timber,false)
    -- Two larger homes beside the bend. The front steps face the dry path.
    house(bg,439,127,29,25,green,sage,false)
    house(bg,377,139,44,32,red,plaster,true)
    L.poly(bg,{403,142,413,142,415,148,402,154,397,154},P.tan2)
    banana(bg,-16,71); banana(bg,333,61)
    -- Reed fans on the far bank, entirely behind the current.
    for _,cx in ipairs({62,74,162,223,267}) do
      local far=edges(cx)-2
      for i=-3,3 do
        L.line(bg,cx+i,far,cx+3*i,far-7-(i+4)%3*2,P.green3)
      end
    end
    -- Bridge runs across the narrow upstream channel. Its opaque pixels mask the water.
    local bridge=L.newImage(31,29)
    L.poly(bridge,{1,21,18,3,29,5,11,24,1,24},P.gray3)
    L.poly(bridge,{1,20,18,2,29,4,11,22},P.concrete)
    for _,dx in ipairs({1,10}) do
      for k=0,2 do L.vline(bridge,dx+k*8,14-k*8+3,20-k*8+3,P.gray4) end
      L.line(bridge,dx,17,dx+16,1,P.metal2,2)
    end
    L.outline(bridge,P.ink)
    local bx,by=314,99
    L.blit(bg,bridge,bx,by)
    L.save(bg,"Level/v2/bg_creek","Level/v2/bg_creek")
    local frames={}
    local top=99
    for f=0,3 do
      local img=L.newImage(386,83)
      for x=0,385 do
        local far,near=edges(x)
        for y=far,near do
          if not L.opaque(bridge,x-bx,y-by) then
            local c=water.mid
            local band=y%9
            local phase=x+(y//9)*13
            if y==near or y==far then c=water.dark
            elseif band==1 and phase%39<19 then c=water.light
            elseif band==5 and (phase+6*f)%24<7 then c=water.sheen end
            L.px(img,x,y-top,c)
          end
        end
      end
      frames[#frames+1]=img
    end
    L.saveFrames(frames,"Level/v2/creek_water","Level/v2/creek_water",180)
    local stones=L.newImage(21,9)
    for _,v in ipairs({{5,5,4,2},{12,3,3,2},{17,6,2,1}}) do
      L.ellipse(stones,v[1],v[2],v[3],v[4],P.rock2)
      L.hline(stones,v[1]-v[3]+1,v[1],v[2]-1,P.rock3)
      L.px(stones,v[1]+1,v[2],P.rock)
    end
    L.outline(stones,P.ink)
    L.save(stones,"Level/v2/creek_stones","Level/v2/creek_stones")
  end
  if only.icons_flood_creek then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    local function icon(name,rows)
      if next(selected) and not selected["flood_"..name] then return end
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,{R=P.roof,B=P.blue2,b=P.blue3,W=P.white,T=P.tan,
        H=P.skin1,h=P.skin2,D=P.brown2,G=P.green2,S=P.rock3,s=P.rock2,Y=P.yellow})
      L.outline(img,P.ink)
      local path="UI/icon_flood_"..name
      L.save(img,path,path)
    end
    icon("walk_home",{".......RR","......RRRR",".....RRRRRR","....RRRRRRRR",".....TTTTTT",".....TWTDTT",".G...TTTDTT",".GG..TTTDTT","GGGGG","GGGGGG",".GGGG",".GG"})
    -- Goggles represent the swim Choice without depicting a swimmer in the creek.
    icon("swim",{"","...BBBBBBBB","..BB......BB",".BB........BB","BBBBBB..BBBBBB","BbbWWBBBBbbWWB","BbbWWB..BbbWWB",".BbbB....BbbB","..BB......BB"})
    icon("fish",{".....TTTTT","....TT...W","...TT....W","...T.....W","..TT.....W","..T......W",".TT......Y",".D......YYY",".D.......Y","DD.......B"})
    icon("throw_stones",{"........SS",".......SSss","........ss",".....W","...W",".......HHH","...HHHHHHh","..HHHHHHhh",".HHHHHHhh","HHHHhhh","hhh"})
  end
end
