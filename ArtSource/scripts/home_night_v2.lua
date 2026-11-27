-- Brownout in the established home. The moving curtain and candle are separate props.
return function(L, ST, home, kit)
  local P, box = L.P, kit.box
  local M = {}
  function M.background()
    local img = home.background()
    -- Low candle table just left of the window. The large table holds the plate.
    box(img,264,181,4,31,kit.WOOD)
    box(img,294,181,4,31,kit.WOOD)
    box(img,261,177,40,6,kit.WOOD)
    local c = app.pixelColor
    for y=0,269 do
      for x=0,479 do
        local old=img:getPixel(x,y)
        local light=0.29
        if x>=305 and x<=362 and y>=73 and y<=135 then light=0.54 end
        if y>=209 and y<=236 and x>=302-(y-209) and x<=363 then light=0.40 end
        local r=math.floor(c.rgbaR(old)*light+9)
        local g=math.floor(c.rgbaG(old)*light+15)
        local b=math.floor(c.rgbaB(old)*light+39)
        L.px(img,x,y,c.rgba(r,g,b,255))
      end
    end
    L.circle(img,347,87,5,P.paper2)
    L.circle(img,349,85,4,img:getPixel(353,85))
    return img
  end
  function M.curtain(f)
    local img=L.newImage(15,100)
    local d=f*2
    L.poly(img,{2,1,12,1,12,34,9,48,10+d,70,11+d,98,3+d,97,2+d,72,4,48},P.red2)
    L.line(img,4,2,5,47,P.red)
    L.line(img,5,50,5+d,95,P.red)
    L.line(img,9,50,9+d,96,P.brown2)
    L.hline(img,4,10,47,P.tan)
    L.outline(img,ST.KEY.o)
    return img
  end
  function M.candle(f)
    local img=L.newImage(6,12)
    L.rect(img,1,5,4,6,P.paper2)
    L.vline(img,1,5,10,P.white)
    L.vline(img,4,6,10,P.paper3)
    L.line(img,2,4,2+f,1,P.orange)
    L.px(img,2,3,P.yellow3)
    L.outline(img,ST.KEY.o)
    local halo=app.pixelColor.rgba(255,209,124,78)
    for y=0,5 do
      for x=0,5 do
        if not L.opaque(img,x,y) and (x-2-f)^2+(y-3)^2<10 then L.px(img,x,y,halo) end
      end
    end
    return img
  end
  function M.plate()
    local img=L.newImage(18,5)
    L.ellipse(img,8,2,8,2,P.ink)
    L.ellipse(img,8,2,7,1,P.paper)
    L.hline(img,5,12,2,P.paper2)
    return img
  end
  function M.lighter()
    local img=L.newImage(6,4)
    L.rect(img,0,0,6,4,P.ink)
    L.hline(img,1,3,1,P.red3)
    L.hline(img,1,3,2,P.red)
    L.vline(img,4,1,2,P.gray2)
    return img
  end
  M.flashlight=home.flashlight
  function M.typhoonShelf()
    local img=L.newImage(45,9)
    for _,x in ipairs({5,35}) do
      L.poly(img,{x,2,x+6,2,x,9},P.ink2)
      L.line(img,x+1,6,x+4,3,P.brown)
    end
    box(img,0,0,45,4,kit.WOOD)
    return img
  end
  function M.unlitCandle()
    local img=L.newImage(12,15)
    box(img,4,3,5,9,kit.WHITE)
    L.vline(img,6,1,3,P.ink)
    L.ellipse(img,6,12,5,2,P.ink)
    L.ellipse(img,6,12,4,1,P.glass3)
    L.hline(img,3,7,11,P.glass2)
    return img
  end
  function M.closedMatches()
    local img=L.newImage(10,6)
    box(img,0,0,10,6,kit.WOOD)
    L.rect(img,2,1,6,2,P.paper2)
    L.hline(img,2,7,4,P.brown2)
    for x=3,7,2 do L.px(img,x,4,P.tan2) end
    return img
  end
  return M
end
