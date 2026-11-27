-- Batch 2 Choice pictures, composed at the shared 16-pixel icon scale.
return function(L)
  local P=L.P
  local colors={G=P.grass,g=P.green3,B=P.blue3,b=P.blue2,T=P.tan,t=P.brown2,
    S=P.rock,s=P.rock2,W=P.paper,R=P.roof,Y=P.yellow,M=P.metal,H=P.skin1}
  local shapes={
    slope={".......GG","......Ggg",".....Gggg","...GGgggg","..GGggggg","GGggggggg"},
    tree={"...ggg","GGGgGGGG","G..t...G","...t","..Tt","..Tt",".Tt"},
    arrow={"...G","...GG","GGGGGG","GGGGGG","...GG","...G"},
    house={"...RR","..RRRR",".RRRRRR","RRRRRRRR",".TTTTTT",".TBBttT",".TBBttT",".TTTttT"},
    ball={"..YY",".YWWY","YWbWWY","YWWbbY",".YbbY","..YY"},
    shovel={"......tt",".....t.t",".....tt","....T","...T","..T","MMM","MMM",".M"},
    eye={"..WWWW",".WWbbWW","WWbbbbWW",".WWbbWW","..WWWW"},
    leaf={".....GGGG","..GGGGgg",".GGGggg","GGggg","gg"},
    foot={"bbb","bbb","bbb","bbbbb","bbWWWW","WWWWWW"},
    rope={"T....T","Tt..tT","T.tt.T","T....T","T....T"},
    up={"...YY","..YYYY",".YY.YY","YY..YY"},
  }
  local function icon(name,parts)
    local img=L.newImage(16,16)
    for _,p in ipairs(parts) do L.map(img,p[1],p[2],shapes[p[3]] or p[3],colors) end
    L.outline(img,P.ink)
    local path="UI/icon_landslide_"..name
    L.save(img,path,path)
  end
  icon("garden_box",{{2,3,{".....G","...GgG","....g","..SSsSSSS","TTTTTTTTTTTT","TtTTTTTTTTtT","TtTTTTTTTTtT","tttttttttttt"}}})
  icon("dig_slope",{{1,8,"slope"},{5,1,"shovel"}})
  icon("leave_ball",{{7,2,"ball"},{7,9,"rope"},{1,4,{"..G",".GG","GGGGGG",".GG","..G"}}})
  icon("under_rope",{{2,4,"rope"},{8,3,"rope"},{5,10,"arrow"}})
  icon("walk_house",{{6,1,"house"},{1,8,"arrow"}})
  icon("under_tree",{{3,1,"tree"},{5,10,{"HHH","HtH",".H.","bbb","b.b"}}})
  icon("coconut",{{2,2,{".....Ggg","...GGgg","...Tt","..tttt",".tTTttt","tTTttttt","tttttttt",".tttttt","..tttt"}}})
  icon("flat_yard",{{4,2,"ball"},{2,10,{"GGGGGGGGGGGG","gggggggggggg","TTTTTTTTTTTT"}}})
  icon("slide_leaf",{{4,8,"slope"},{2,4,"leaf"}})
  icon("climb_slope",{{4,9,"slope"},{2,2,"up"}})
  icon("wait_rain",{{1,1,{"BB.BB.BB","B..B..B"}},{4,5,{"TTTTTTT",".TWWWT","..TWt","...T","..TWT",".TWWWT","TTTTTTT"}}})
  icon("look_slope",{{2,2,"eye"},{5,9,"slope"}})
  icon("shovel",{{3,2,"shovel"}})
  icon("pail",{{2,2,{"...MMMM","..M....M",".M......M","TBBBBBBBBT",".BbbbbbbB",".BBbbbbBB","..BBbbBB","..BBBBBB"}}})
  icon("open_yard",{{1,1,"house"},{8,6,"arrow"},{1,12,{"GGGGGGGGGGGGG","ggggggggggggg"}}})
  icon("by_slope",{{6,8,"slope"},{1,4,"foot"}})
  icon("path_up",{{3,7,"slope"},{2,2,"up"},{6,9,{".....T","...TT","..T","...T",".TT"}}})
end
