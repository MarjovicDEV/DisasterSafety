-- Flood Batch 3. All three overlays register to the top left of their Setting.
return function(L, root, only)
  local P = L.P
  local water = dofile(root .. "/ArtSource/scripts/flood_fx.lua")(L, root).WATER

  local function yard(x,y)
    local shore = 240 + math.floor(8*math.sin(y/12) + 3*math.sin(y/4))
    if x < shore or y < 175 then return false end
    -- Preserve the drum and the posts above the surface.
    if y <= 182 and x >= 407 and x <= 440 then return false end
    if y < 180 and ((x>=249 and x<=255) or (x>=438 and x<=444)) then return false end
    return true
  end
  local function school(x,y)
    local shore = 68 + math.floor((y-163)*0.43 + 4*math.sin(y/8))
    return y >= 166 and y <= 242 + math.floor(3*math.sin(x/11))
        and x <= shore and y >= 166 + math.floor(x/26)
  end
  local function street(x,y)
    local shore = 228 + math.floor(7*math.sin(y/7)+2*math.sin(y/3))
    return x>=90 and x<=shore and y >= 149 and y <= 187
        and not (x>=150 and x<=155 and y<=153)
  end
  local function surface(mask,f,moving)
    local img=L.newImage(480,270)
    for y=0,269 do
      for x=0,479 do
        if mask(x,y) then
          local c=water.mid
          if not mask(x-1,y) or not mask(x+1,y) or not mask(x,y+1) or not mask(x,y-1) then
            c=water.dark
          else
            local band=y%11
            local offset=(y//11)*17
            if (band==1 or band==2) and (x+offset)%51<28 then c=water.light end
            if moving then
              -- One 32-pixel period in four frames, travelling left through the loop.
              if band==5 and (x+offset+8*f)%32<8 then c=water.sheen end
              if band==6 and (x+offset+8*f)%32==0 then c=water.light end
            else
              if band==6 and (x+offset*3+f*2)%67<12 then c=water.sheen end
              if band==7 and (x+offset*3+f*2)%67==66 then c=water.sheen end
            end
          end
          L.px(img,x,y,c)
        end
      end
    end
    return img
  end

  if only.flood_outdoor then
    local function loop(name,count,ms,draw)
      local frames={}
      for f=0,count-1 do frames[#frames+1]=draw(f) end
      local path="Level/v2/"..name
      L.saveFrames(frames,path,path,ms)
    end
    loop("flood_yard",2,400,function(f) return surface(yard,f,false) end)
    loop("flood_schoolyard",4,180,function(f) return surface(school,f,true) end)
    loop("flood_street_storm",4,180,function(f) return surface(street,f,true) end)
    local ball=Image{fromFile=root.."/Assets/Art/Level/v2/yard_ball.png"}
    loop("flood_ball",2,400,function(f)
      local img=L.newImage(16,11)
      L.ellipse(img,8,7+f,7-f,2,water.sheen)
      L.ellipse(img,8,7+f,5-f,1,water.mid)
      L.blit(img,ball,4,f)
      L.hline(img,6,10,7+f,water.light)
      return img
    end)
    -- The deck opening is transparent: the animated road Floodwater passes through it.
    local bridge=L.newImage(60,24)
    for _,offset in ipairs({0,11}) do
      L.poly(bridge,{1,5+offset,12,1+offset,47,1+offset,58,5+offset,58,8+offset,47,5+offset,12,5+offset,1,9+offset},P.gray3)
      L.hline(bridge,12,47,1+offset,P.concrete)
      for _,x in ipairs({4,15,43,55}) do
        local y=(x==4 or x==55) and 7 or 4
        L.rect(bridge,x,y+offset,3,4,P.gray4)
        L.vline(bridge,x,y+offset,y+3+offset,P.gray2)
      end
    end
    L.outline(bridge,P.ink)
    L.save(bridge,"Level/v2/street_storm_bridge","Level/v2/street_storm_bridge")
  end

  if only.icons_flood_outdoor then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    local colors={S=water.sheen,M=water.mid,L=water.light,D=water.dark,T=P.tan,
      O=P.orange,Y=P.yellow,N=P.navy,B=P.blue2,W=P.white,G=P.green2,H=P.skin1,F=P.skin2,C=P.concrete}
    local function icon(name,rows)
      if next(selected) and not selected["flood_"..name] then return end
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,colors)
      L.outline(img,P.ink)
      local path="UI/icon_flood_"..name
      L.save(img,path,path)
    end
    icon("wade_ball",{"........YYY",".......YYOOO",".......YOOYO","........OOO","........SSSS","..BB","..HH","..HHH","..FFHH...SS","SSSSSMMMMMMM","MMSSMMMMSSMM"})
    icon("splash",{".....S","..S..S....S","...S.S...S","....S...S","SS......S..SS","..SS...S..S","....SSSSSS","...MMLSSMMM","SSMMMMMMSSMM","MMSSMMMMMMMM"})
    icon("stay_teacher",{"..NNN","..HHH","..HHF....NNN",".GGGGG...HHH",".GGGGGH..HHF",".GGGGG..WWWW","..BBB...WWWW","..BBB....BBB","..N.N....N.N"})
    icon("cross_water",{".....G",".....GG","..GGGGGG","..GGGGGGG",".....GGG",".....GG","SSMMMMSSMMMM","MMMSSMMMMSSM","SSMMMMSSMMMM"})
    icon("shoes_off",{".......YY","......YYYY",".......YY","...BBB.YY","...BWWB","...BBBBBB","...NNNNNNN","","WWWWW","NNNNNNNN"})
    icon("high_road",{".........GG","........GGGG",".........GG",".........GG",".......CCCCCC","......CCDDDDD",".....CCDD","....CCDD","...CCDD...SSS","CCCCDD..MMSSM"})
    icon("flooded_bridge",{"..CC.....CC","CCCCCCCCCCCCC","..CC.....CC","..CC.....CC","MSSMMMSSMMMMM","MMMLMMMMSSMMM","CCCCCCCCCCCCC","..CC.....CC","MSSMMMMMSSMMM"})
    icon("wade_barefoot",{".....HH","....HHHF","....HHHF",".....HHF",".....HHF","...HHHHF","..HHHFFF","..FFFFF","..SSSS...SS","MMMMMMSSMMMM","SSMMMMMMSSMM"})
  end
end
