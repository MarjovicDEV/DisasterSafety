-- Flood Batch 2. All scenery uses top-left registration. See the batch placement sheet.
return function(L, root, only)
  local P = L.P
  local function still(name, img)
    local path = "Level/v2/" .. name
    L.save(img, path, path)
  end
  local function loop(name, draw)
    local path = "Level/v2/" .. name
    L.saveFrames({draw(0), draw(1), draw(2), draw(3)}, path, path, 180)
  end
  if only.flood_home_more then
    local shelf = L.newImage(40, 8)
    L.rect(shelf, 2, 1, 36, 3, P.brown)
    L.hline(shelf, 1, 38, 1, P.tan)
    for _, x in ipairs({5, 31}) do
      L.poly(shelf, {x,4, x+4,4, x,6}, P.brown2)
    end
    L.outline(shelf, P.ink)
    still("home_shelf_high", shelf)

    -- A projecting workbook cover can be layered with the existing classroom bag.
    local book = L.newImage(12, 7)
    L.rect(book, 1, 1, 10, 5, P.teal2)
    L.rect(book, 2, 1, 8, 3, P.teal)
    L.hline(book, 3, 10, 1, P.paper)
    L.vline(book, 2, 2, 5, P.teal3)
    L.outline(book, P.ink)
    still("home_workbook", book)

    local tv = L.newImage(50, 27)
    L.rect(tv, 0, 0, 50, 27, P.red2)
    L.rect(tv, 0, 0, 50, 12, P.red)
    for i, word in ipairs({"RAINFALL", "WARNING"}) do
      local w = L.F.width(word)
      assert(w <= 48, word)
      L.text(tv, (50-w)//2, 3+(i-1)*13, word, P.white, 1)
    end
    still("home_tv_rainfall_red", tv)
    local fx = dofile(root .. "/ArtSource/scripts/flood_fx.lua")(L, root)
    loop("home_window_flood_high", function(f)
      local view = fx.windowFlood(f, 44)
      local mask = Image{fromFile=root .. "/Assets/Art/Level/v2/home_window_mask.png"}
      for y=44,64 do
        for x=0,59 do
          if L.opaque(mask,x,y) then
            local water = x < 25 and fx.WATER.light or fx.WATER.mid
            if y == 44 then water = fx.WATER.dark
            elseif (y%6==1 and (x+f+(y//6)*7)%22<6) then water=fx.WATER.sheen end
            L.px(view,x,y,water)
          end
        end
      end
      return view
    end)
  end
  if only.flood_kitchen then
    local jug = L.newImage(15, 18)
    L.rect(jug, 8, 5, 5, 7, P.teal2)
    L.rect(jug, 9, 6, 3, 4, 0)
    L.poly(jug,{2,5, 4,4, 8,4, 10,7, 10,15, 8,16, 2,16, 1,14},P.teal)
    L.vline(jug,2,7,14,P.teal3)
    L.rect(jug,7,8,3,7,P.teal2)
    L.rect(jug,2,2,8,3,P.paper2)
    L.hline(jug,3,8,2,P.white)
    L.outline(jug,P.ink)
    still("kitchen_jug",jug)
    local drum = L.newImage(24,32)
    L.ellipse(drum,11,27,9,3,P.blue2)
    L.rect(drum,2,7,19,21,P.blue)
    L.rect(drum,17,8,4,19,P.blue2)
    L.vline(drum,4,10,25,P.blue3)
    for _,y in ipairs({12,23}) do L.hline(drum,3,19,y,P.blue2) end
    L.ellipse(drum,11,7,9,3,P.navy)
    L.ellipse(drum,10,5,8,2,P.blue2)
    L.hline(drum,4,15,4,P.blue3)
    L.line(drum,15,8,21,2,P.tan,2)
    L.ellipse(drum,15,8,3,1,P.paper2)
    L.outline(drum,P.ink)
    still("kitchen_rain_drum",drum)
    loop("kitchen_window_rain", function(f)
      local view=L.newImage(90,55)
      for y=0,54 do
        for x=0,89 do
          local gy=72+y
          if (x<43 or x>46) and gy%9~=5 and gy%9~=4 then
            local c=y<19 and P.mount3 or (y<39 and P.mount or P.mount2)
            if (x+(y//2)+3*f)%19<2 and (y+7*x-4*f)%17<5 then c=P.sky5 end
            L.px(view,x,y,c)
          end
        end
      end
      return view
    end)
  end

  if only.icons_flood_home then
    -- Each glyph has a distinct silhouette; B/b/c are the three material shades.
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    local function icon(name, shades, rows)
      if next(selected) and not selected["flood_"..name] then return end
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,{B=shades[1], b=shades[2], c=shades[3], W=P.white,
        T=P.tan, D=P.brown2, S=P.metal, N=P.navy, Y=P.yellow})
      L.outline(img,P.ink)
      local path="UI/icon_flood_"..name
      L.save(img,path,path)
    end
    local blue={P.blue3,P.blue,P.blue2}
    local wood={P.tan,P.brown,P.brown2}
    icon("bag_up",blue,{"...SS....Y","..S..S..YYY",".BbbbbcYYYYY",".Bbbbbc..Y",".BSSbbc..Y",".Bbbbbb",".cccccc","TTTTTTTTTTTT","DDDDDDDDDDDD",".D........D"})
    icon("bag_floor",blue,{"..........Y","...SS.....Y","..S..S..YYYYY",".Bbbbbc..YYY",".Bbbbbc...Y",".BSSbbc",".Bbbbbc",".cccccc","","TTTTTTTTTTTT"})
    icon("boiled_water",{P.teal3,P.teal,P.teal2},{"...WWWW","..SSSSSS","..BbbbbcSS",".Bbbbbbc.S",".Bbbbbbc.S",".BbbbbbcSS",".BBbbbbc",".BBbbbc...SSS",".BBbbbc...WWS",".cccccc...SSS"})
    icon("rain_drum",blue,{"..........TT","...BBBB..TT",".BbbbbbccT",".cccccccTcc",".BBBBbbbbbc",".Bbbbbbbbbc",".Bccccccccc",".Bbbbbbbbbc",".Bbbbbbbbbc",".Bccccccccc","..cccccccc"})
    icon("tap",blue,{"....SSSSS","......S",".....SSS","..SSSSSSSS","..SS....SS","..SS....SS",".........B","........Bb","........bc"})
    icon("riverbank",wood,{"..........BB",".........BbB","......BBBbbB",".....BbbbbbB","....Bbbccccc","...Bbbcc....","..BbBc..SSS.",".Bbbc.......","BbBc...SSS..","bbc.........","cc..SSSS...."})
    icon("bridge",wood,{"...T.....T","TTTTTTTTTTTT","...T.....T","..TTTTTTTTT","TTbbbbbbbbbTT","bbcc.....ccbb","..cc.....cc","..cc.....cc","SSS..SSS..SSS"})
    icon("leave_now",{P.green4,P.green2,P.green3},{"TTTTTTT","TccccbT","TccbbbT","TcbbYbT....B","TccbbbT....BB","TccbbbT.BBBBBB","TccbbbT.BBBBBBB","TccbbbT....BBB","TccbbbT....BB","TTTTTTT....B"})
    icon("wait_water",wood,{".TTTTTTT","..WYYYW","..cYYWc","...cYc","....c","...cWc","..cWYWc","..WYYYW",".TTTTTTT","..........SS","...SSS......","bbbbbSSSbbbb"})
    icon("unplug",blue,{"..SS..SS","..SS..SS",".Bbbbbbbc",".Bbbbbbbc","..bbbbbc","...cccc","....cc","....cc",".....cc...cc","......ccccc"})
    icon("wipe",{P.teal3,P.teal,P.teal2},{".....TTTT","....TTTTTT","...TTTTTTT","..BBBBBTT",".BBbbbbbbB","BbbbbbbbbbB","Bbbbccccbbbc",".ccccccccccc",".......SSS"})
  end
end
