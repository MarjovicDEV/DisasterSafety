-- Flood Easy 2 and 3. The canal is dry; the yard props hold ordinary rainwater.
return function(L,only)
  local P=L.P
  local function save(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  if only.flood_street then
    local grate=L.newImage(51,13)
    L.poly(grate,{1,8,7,1,49,1,44,10,1,10},P.concrete3)
    L.poly(grate,{4,8,9,3,46,3,42,8},P.ink2)
    for x=13,41,5 do
      L.line(grate,x,3,x-3,8,P.metal2,2)
      L.line(grate,x,3,x-3,7,P.metal)
    end
    L.hline(grate,7,49,1,P.concrete)
    L.hline(grate,2,43,10,P.gray4)
    L.outline(grate,P.ink)
    save("street_canal_grate",grate)
    local bin=L.newImage(38,35)
    L.poly(bin,{3,7,34,7,32,32,6,32},P.teal2)
    L.poly(bin,{4,7,30,7,28,31,7,31},P.teal)
    L.vline(bin,7,13,27,P.teal3)
    L.rect(bin,2,4,34,5,P.teal2)
    L.hline(bin,3,34,4,P.teal3)
    L.rect(bin,6,6,17,2,P.ink)
    L.rect(bin,3,13,32,11,P.paper)
    L.text(bin,(38-L.F.width("TRASH"))//2,15,"TRASH",P.ink,1)
    L.outline(bin,P.ink)
    save("street_trash_bin",bin)
    local wrap=L.newImage(8,6)
    L.map(wrap,1,1,{"YrrrrY","rYYYrr","rWWYYr","YrrrrY"},{Y=P.yellow,r=P.red,W=P.white})
    L.outline(wrap,P.ink)
    save("street_wrapper",wrap)
  end
  if only.flood_yard_easy then
    local can=L.newImage(12,14)
    L.rect(can,2,3,8,9,P.metal2)
    L.vline(can,3,4,11,P.white)
    L.hline(can,3,8,10,P.metal)
    L.ellipse(can,5,3,4,2,P.gray4)
    L.hline(can,3,7,3,P.water3)
    L.outline(can,P.ink)
    save("yard_can_water",can)
    local empty=L.newImage(25,13)
    L.ellipse(empty,17,9,6,2,P.water2)
    L.hline(empty,14,19,9,P.water3)
    local body=L.newImage(14,10)
    L.rect(body,2,2,9,6,P.metal2)
    L.hline(body,2,10,2,P.metal)
    L.ellipse(body,10,5,2,3,P.metal)
    L.vline(body,11,4,6,P.gray4)
    L.outline(body,P.ink)
    L.blit(empty,body,0,2)
    save("yard_can_tipped",empty)
    local flat=L.newImage(31,17)
    L.ellipse(flat,15,10,14,5,P.ink2)
    L.ellipse(flat,15,7,14,5,P.gray4)
    L.ellipse(flat,15,7,8,3,P.ink)
    L.ellipse(flat,15,8,6,1,P.water2)
    L.hline(flat,11,16,8,P.water3)
    for x=4,25,5 do L.line(flat,x,11,x+1,13,P.gray3) end
    L.outline(flat,P.ink)
    save("yard_tire_water",flat)
    local upright=L.newImage(41,30)
    L.ellipse(upright,30,26,9,2,P.water2)
    L.hline(upright,27,33,26,P.water3)
    local ring=L.newImage(24,29)
    L.ellipse(ring,12,14,10,13,P.ink2)
    L.ellipse(ring,9,14,8,12,P.gray4)
    L.ellipse(ring,10,14,4,7,0)
    for y=6,21,5 do L.line(ring,19,y,21,y+1,P.gray3) end
    L.line(ring,4,9,6,5,P.gray3)
    L.outline(ring,P.ink)
    L.blit(upright,ring,0,0)
    save("yard_tire_tipped",upright)
  end
  if only.icons_flood_easy then
    local selected={}
    for n in (app.params.icons or ""):gmatch("[^,%s]+") do selected[n]=true end
    local function icon(n,rows)
      if next(selected) and not selected["flood_"..n] then return end
      local img=L.newImage(16,16)
      L.map(img,1,1,rows,{G=P.teal,g=P.teal2,H=P.teal3,M=P.metal2,m=P.gray4,S=P.metal,
        B=P.water2,b=P.water3,W=P.white,Y=P.yellow,R=P.red})
      L.outline(img,P.ink)
      local path="UI/icon_flood_"..n
      L.save(img,path,path)
    end
    icon("trash_bin",{"....YYR","....RYY","..HHHHHHHHH",".Gmmmmmmmmg","..GGGGGGGg","..GWWWWWGg","..GGGGGGGg","..GHGHGHGg","..GHGHGHGg","...gggggg"})
    icon("canal",{"",".....SSSSSSSS","....SmmSmmSmS","...SmmSmmSmmS","..SmmSmmSmmS",".SSSSSSSSSS","mmmmmmmmmmm"})
    icon("tip_water",{"....SS","...SMMS","..SMMMMS",".SMMMMSm","..SMMSm.b","...SSm..bb",".........B",".........b",".......bbbbBB"})
    icon("leave_water",{"","..SSSSSSS",".MbbbbbBMS",".MSSSSSSMm",".MMSMMMMMm",".MMSMMMMMm",".MMSMMMMMm",".MMMMMMMMm","..mmmmmmm"})
  end
end

