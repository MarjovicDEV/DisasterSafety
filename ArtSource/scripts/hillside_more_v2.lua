-- Batch 2 scene props. Draw the leaning palm on the original hillside underpainting.
return function(L,root,bg,sky,slope,palm)
  local P=L.P
  local function save(name,img)
    local path="Level/v2/hillside_"..name
    L.save(img,path,path)
  end
  local old=Image(slope)
  palm(old)
  local leaned=Image(slope)
  L.ellipse(leaned,84,150,9,3,P.dirt2)
  L.ellipse(leaned,82,149,7,2,P.dirt)
  palm(leaned,true)
  local patch=L.newImage(108,108)
  local tint=L.newImage(1,1)
  L.px(tint,0,0,L.hex("566677",134))
  for y=54,161 do for x=60,167 do
    if old:getPixel(x,y)~=leaned:getPixel(x,y) then
      local c=leaned:getPixel(x,y)
      if not L.opaque(leaned,x,y) then
        c=bg:getPixel(x,y)
        if c==sky:getPixel(x,y) then
          local sample=L.newImage(1,1)
          L.px(sample,0,0,c)
          sample:drawImage(tint)
          c=sample:getPixel(0,0)
        end
      end
      L.px(patch,x-60,y-54,c)
    end
  end end
  save("tree_lean",patch)
  local wet=L.newImage(209,154)
  local colors={[P.grass]=P.grass3,[P.grass2]=P.green2,[P.grass3]=P.green3,
    [P.tan2]=P.dirt,[P.dirt]=P.dirt2,[P.dirt2]=P.brown2}
  for y=62,215 do for x=0,208 do
    local c=old:getPixel(x,y)
    if colors[c] then L.px(wet,x,y-62,colors[c]) end
  end end
  for _,r in ipairs({{32,132,34,138},{61,154,65,164},{75,174,85,179},{110,194,118,201},
    {105,122,111,126},{153,174,160,179},{180,192,187,196}}) do
    L.line(wet,r[1],r[2]-62,r[3],r[4]-62,P.rock3)
  end
  save("wet_slope",wet)
  local box=L.newImage(44,20)
  L.poly(box,{1,6,8,1,42,1,42,14,35,19,1,19},P.brown2)
  L.poly(box,{3,6,9,2,40,2,34,7},P.tan)
  L.poly(box,{6,6,11,3,37,3,33,6},P.rock2)
  for x=10,32,5 do L.px(box,x,5,P.rock3) end
  L.rect(box,2,8,32,9,P.brown3)
  L.hline(box,2,33,12,P.brown)
  for _,x in ipairs({3,30}) do L.rect(box,x,8,3,10,P.tan); L.px(box,x+1,10,P.gray4) end
  L.outline(box,P.ink)
  save("garden_box",box)
  for _,full in ipairs({false,true}) do
    local pot=L.newImage(12,11)
    L.poly(pot,{2,3,10,3,9,10,4,10},P.brown)
    L.rect(pot,1,2,10,3,P.tan2)
    L.hline(pot,2,9,1,P.brown3)
    L.hline(pot,3,8,2,full and P.rock2 or P.brown2)
    L.vline(pot,4,5,8,P.tan)
    if full then L.hline(pot,4,8,1,P.rock2);L.px(pot,6,0,P.rock) end
    L.outline(pot,P.ink)
    save(full and "pot_full" or "pot_empty",pot)
  end
  local leaf=L.newImage(24,8)
  L.poly(leaf,{1,6,7,2,16,1,22,2,17,5,10,7},P.green2)
  L.line(leaf,2,6,20,2,P.grass)
  L.line(leaf,8,4,10,2,P.grass3)
  L.outline(leaf,P.ink)
  save("banana_leaf",leaf)
  local shovel=L.newImage(17,38)
  L.line(shovel,4,29,12,7,P.brown2,2)
  L.line(shovel,4,28,12,8,P.tan)
  L.poly(shovel,{1,26,7,28,8,31,5,36,2,35,0,31},P.metal2)
  L.line(shovel,1,28,3,32,P.metal)
  L.line(shovel,11,7,10,2,P.brown2,2)
  L.line(shovel,11,2,15,3,P.brown,2)
  L.line(shovel,15,4,12,8,P.brown2,2)
  L.outline(shovel,P.ink)
  save("shovel",shovel)
  local pail=L.newImage(17,17)
  L.ellipse(pail,8,7,6,6,P.metal2)
  L.ellipse(pail,8,7,5,5,L.CLEAR)
  L.poly(pail,{1,7,15,7,13,15,4,15},P.teal2)
  L.poly(pail,{2,8,12,8,11,14,5,14},P.teal)
  L.ellipse(pail,8,7,7,2,P.teal3)
  L.hline(pail,3,12,7,P.teal2)
  L.outline(pail,P.ink)
  save("pail",pail)
end
