-- Typhoon Batch 2. Window origins are the first glass pixel, not the outer frame.
return function(L, root)
  local P, pc = L.P, app.pixelColor
  local function background(name)
    return Image{fromFile = root .. "/Assets/Art/Level/v2/bg_" .. name .. ".png"}
  end
  local function still(name, image)
    local path = "Level/v2/" .. name
    L.save(image, path, path)
  end
  local function loop(name, count, ms, draw)
    local images = {}
    for f=0,count-1 do images[#images+1] = draw(f) end
    local path = "Level/v2/" .. name
    L.saveFrames(images,path,path,ms)
  end

  -- Stagger each drop's three exposure phases and wrap both tile edges.
  for _,weight in ipairs({"light","heavy"}) do
    local heavy = weight == "heavy"
    loop("typhoon_rain_" .. weight,3,120,function(f)
      local img=L.newImage(480,270)
      local color=pc.rgba(153,183,204,heavy and 140 or 89)
      L.seed(heavy and 993 or 731)
      for i=1,(heavy and 780 or 320) do
        local x,y=L.randi(0,479),L.randi(0,269)
          local step=(i+f)%3
          for k=0,(heavy and 8 or 4) do
            L.px(img,(x-step*2-k//3)%480,(y+step*6+k)%270,color)
          end
      end
      return img
    end)
  end
  local leafRows={{".ll.","lls.","s..."},{"..l.",".ls.","s..."},{".s..",".ls.","..l."},{"....","sll.",".lls"}}
  local function leaf(f)
    local img=L.newImage(4,3)
    L.map(img,0,0,leafRows[f+1],{l=P.grass3,s=P.brown2})
    return img
  end
  loop("typhoon_leaf",4,180,leaf)

  for _,name in ipairs({"yard","evacuation_area"}) do
    local base=background(name)
    local img=L.newImage(480,270)
    for y=0,139 do for x=0,479 do
      local c=base:getPixel(x,y)
      local yard=name=="yard"
      local sky=c==P.sky3 or c==P.sky4 or (not yard and c==P.sky5)
      local sun=yard and c==P.yellow3 and math.abs(x-188)<=12 and math.abs(y-86)<=12
      if yard then sky=sky or c==P.cream or sun end
      if sky then
        -- Neutralize the yellow sun to match the adjacent shaded sky.
        local shade=pc.rgba(110,124,140,170)
        if sun then
          -- The backdrop's four-pixel dither repeats outside the sun.
          local neighbor=base:getPixel(x+28,y)
          local function channel(value,grey)
            return math.floor((value(neighbor)*85+grey*170-value(c)*8)/247+0.5)
          end
          shade=pc.rgba(channel(pc.rgbaR,110),channel(pc.rgbaG,124),channel(pc.rgbaB,140),247)
        end
        L.px(img,x,y,shade)
      end
    end end
    still("typhoon_dark_sky_" .. name,img)
  end

  local home={x=304,y=72,w=60,h=65}
  local classroom={x=341,y=67,w=68,h=82}
  local bedroom={x=53,y=59,w=62,h=82}
  local function glass(a,x,y)
    if a==home then
      return x~=29 and x~=30 and (y>=42 or (x>=3 and x<=56))
    elseif a==bedroom then
      return (x<29 or x>32) and (y<39 or y>41) and (y>=61 or (x>=3 and x<=58))
    end
    local gx,gy=x+a.x,y+a.y
    return (x<32 or x>35) and (gx-346)^2+(gy-143)^2>5.35^2
      and (gx-354)^2+(gy-141)^2>5.35^2 and not (gy>=148 and gx>=344 and gx<=355)
  end
  local function clip(img,a)
    for y=0,a.h-1 do for x=0,a.w-1 do
      if not glass(a,x,y) then L.px(img,x,y,L.CLEAR) end
    end end
    return img
  end
  local sway={0,2,3,1}
  local function view(a,f,state)
    local night=state=="night"
    local storm=night or state=="storm"
    local img=L.newImage(a.w,a.h)
    local sky=night and {P.navy2,P.navy} or
      (state=="calm" and {L.hex("abbac1"),P.gray1} or {L.hex("899aa7"),L.hex("b3c0c7"),P.gray1})
    L.vgrad(img,0,0,a.w,a.h,sky)
    local ground=a.h-14
    local dx=storm and sway[f+1]+2 or 0
    local bark=night and P.ink2 or P.brown2
    local dark=night and P.navy2 or P.green3
    local mid=night and P.teal2 or P.grass3
    local light=night and P.ink2 or P.grass2
    L.poly(img,{0,ground-5,14,ground-9,34,ground-5,a.w-1,ground-12,a.w-1,a.h,0,a.h},dark)
    L.rect(img,0,ground,a.w,14,mid)
    L.poly(img,{13,ground+3,17,ground-16,16+dx,ground-30,19+dx,ground-30,21,ground-12,20,ground+3},bark)
    for _,b in ipairs({{9,-28,10,8},{18,-34,10,9},{27,-28,10,8},{18,-25,12,7}}) do
      local x,y=b[1]+dx,ground+b[2]
      L.ellipse(img,x,y,b[3],b[4],dark)
      L.ellipse(img,x-1,y-2,b[3]-2,b[4]-2,mid)
      L.ellipse(img,x-3,y-3,3,2,light)
    end
    if state=="calm" then
      for _,x in ipairs({6,15,24,42,53}) do L.line(img,x,ground+5+x%5,x+2,ground+4+x%5,P.green3) end
    end
    if storm then
      for i=0,15 do
        local x=(i*19+7-f*2)%a.w
        local y=(i*23+i*i+f*6)%24
        for row=y-24,a.h,24 do
          L.line(img,x,row,x-2,row+5,night and P.navy or P.gray1)
        end
      end
      if a==classroom then L.blit(img,leaf(f),7+f*14,4) end
    end
    if a~=bedroom then
      local pitch=a==home and 9 or 7
      local first=a==home and 6 or 2
      for y=first,a.h-3,pitch do
        L.hline(img,0,a.w-1,y,night and P.navy or P.gray2)
        L.hline(img,0,a.w-1,y+1,night and P.navy2 or P.gray3)
      end
    end
    return clip(img,a)
  end
  for _,entry in ipairs({{"classroom",classroom},{"home",home},{"bedroom",bedroom},{"home_night",home}}) do
    loop(entry[1].."_window_storm",4,180,function(f)
      return view(entry[2],f,entry[1]=="home_night" and "night" or "storm")
    end)
  end
  still("home_window_grey",view(home,0,"grey"))
  still("home_window_calm",view(home,0,"calm"))
  local mask=L.newImage(home.w,home.h)
  L.rect(mask,0,0,home.w,home.h,P.white)
  still("home_window_mask",clip(mask,home))

  -- Registered to (48,54). Displace the closed sash against its dark rebate;
  -- cover the vacated edge with adjacent storm glass. Curtains stay fixed.
  local bed=background("bedroom")
  loop("bedroom_window_rattle",2,120,function(f)
    local img=L.newImage(72,92)
    for y=3,88 do for x=3,68 do
      local sx=x-f
      if (y>=66 or (x>=8 and x<64)) and
          (x<=5 or x>=66 or y<=5 or y>=87 or (x>=34 and x<=38) or (y>=44 and y<=46)) then
        if sx<5 or sx>=67 or y<5 or y>=87 or (sx>=34 and sx<38) or (y>=44 and y<=46) then
          L.px(img,x,y,bed:getPixel(48+sx,54+y))
        elseif f==1 and x==34 then
          L.px(img,x,y,P.gray3)
        end
      end
    end end
    return img
  end)

  local fruit=L.newImage(home.w,home.h)
  for _,p in ipairs({{9,56},{19,60},{24,55},{38,59}}) do
    L.ellipse(fruit,p[1],p[2],2,1,P.yellow2)
    L.px(fruit,p[1]-1,p[2]-1,P.yellow3)
    L.px(fruit,p[1]+2,p[2]-1,P.brown2)
  end
  still("home_window_mangoes",clip(fruit,home))
  loop("home_window_clothes",4,180,function(f)
    local img=L.newImage(home.w,home.h)
    L.line(img,33,34,58,35,P.brown2)
    L.vline(img,56,33,52,P.brown2)
    for i,x in ipairs({35,46}) do
      local dx=sway[f+1]
      L.poly(img,{x,35,x+7,35,x+8+dx,45,x+dx,46},P.ink2)
      L.poly(img,{x+1,36,x+6,36,x+7+dx,44,x+1+dx,44},i==1 and P.teal3 or P.paper2)
      L.px(img,x+1,34,P.brown)
      L.px(img,x+6,34,P.brown)
    end
    return clip(img,home)
  end)
  local bike=L.newImage(home.w,home.h)
  for _,x in ipairs({38,52}) do
    L.circle(bike,x,57,5,P.ink)
    L.circle(bike,x,57,3,L.CLEAR)
  end
  for _,e in ipairs({{38,57,42,50},{42,50,45,57},{45,57,38,57},{45,57,49,49},{49,49,42,50},{49,48,52,57}}) do
    L.line(bike,e[1],e[2],e[3],e[4],P.red2)
  end
  L.hline(bike,39,43,49,P.ink)
  L.hline(bike,47,51,47,P.ink)
  still("home_window_bike",clip(bike,home))
end
