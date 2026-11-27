-- Fire Episode effects. Each image uses art pixels at the same scale as the Student.
return function(L)
  local P = L.P
  local function tint(color, alpha)
    local c = app.pixelColor
    return c.rgba(c.rgbaR(color), c.rgbaG(color), c.rgbaB(color), alpha)
  end

  local function bell(frame)
    local img = L.newImage(14, 14)
    L.rect(img, 2, 1, 10, 12, P.red2)
    L.circle(img, 6, 6, 5, P.red2)
    L.circle(img, 6, 5, 4, P.red)
    L.line(img, 3, 5, 5, 3, P.red3)
    L.px(img, 6, 5, P.red2)
    local x = frame == 0 and 9 or 12
    L.line(img, 8, 10, x, 11, P.gray3)
    L.rect(img, x-1, 11, 2, 2, P.gray2)
    L.outline(img, P.ink)
    return img
  end

  local function smoke(frame)
    local img = L.newImage(480, 110)
    local tau = 2 * math.pi
    for x = 0, 479 do
      local t = (x + frame * 8) * tau / 480
      local ripple = 4*math.sin(t*5) + 2*math.cos(t*13)
      local edge = math.floor(95 + 8*math.sin(t*3) + ripple)
      L.vline(img, x, 0, edge, tint(P.gray2, 144))
      L.vline(img, x, 0, math.floor(54 + 8*math.sin(t*4) + ripple), tint(P.gray3, 176))
      L.vline(img, x, 0, math.floor(24 + 6*math.cos(t*3) + ripple), tint(P.gray1, 191))
    end
    return img
  end

  local function flames(frame, pan)
    local img = L.newImage(pan and 16 or 20, pan and 10 or 24)
    local w, h = img.width, img.height
    local dx = ({0, 2, -1, -2})[frame+1]
    local tip = pan and 9 or 12
    L.poly(img, {2,h-4, 3,h//2, 5,h//2+1, tip+dx,1+frame%2,
      tip+dx, h//2, w-4, h//2-2, w-3,h//2+1, w-2,h-4, w-4,h-2, 5,h-2}, P.red2)
    L.poly(img, {4,h-4, 6,h//2+2, tip+dx-1,h//3, tip+dx,h//2+2,
      w-5,h//2, w-4,h-4, w-6,h-2, 6,h-2}, P.orange)
    L.poly(img, {6,h-3, 8,h-5, 9+dx,h//2+1, 11,h-4, 12,h-3, 10,h-2, 7,h-2}, P.yellow)
    L.outline(img, P.ink)
    return img
  end

  local function sparks(frame)
    local img = L.newImage(14,12)
    local x = 3 + frame
    L.poly(img, {x+3,1, x,6, x+3,6, x+1,10, x+7,4, x+4,4, x+6,1}, P.yellow)
    L.line(img, x+3,2, x+1,5, P.yellow3)
    L.px(img, 1, 2+frame, P.orange)
    L.px(img, 12, 8-frame, P.yellow3)
    L.outline(img, P.ink)
    return img
  end

  local function night()
    local img = L.newImage(480,270)
    L.rect(img,0,0,480,270,tint(P.navy2,169))
    for _, x in ipairs({56,86}) do
      for _, pane in ipairs({{59,39},{101,40}}) do
        L.rect(img,x,pane[1],26,pane[2],tint(P.navy,109))
      end
    end
    -- Existing light pools below the bedroom window and its ceiling fixture.
    L.poly(img,{54,209,81,209,85,236,41,236},tint(P.navy,119))
    L.poly(img,{85,209,114,209,138,236,92,236},tint(P.navy,119))
    L.ellipse(img,240,27,24,13,tint(P.navy2,144))
    L.ellipse(img,240,24,14,6,tint(P.navy2,116))
    L.ellipse(img,240,22,8,2,tint(P.yellow3,72))
    return img
  end

  for _, set in ipairs({
    {"fire_bell",2,140,bell}, {"smoke_band",3,220,smoke},
    {"flames_small",4,150,function(i) return flames(i,false) end},
    {"flames_pan",3,150,function(i) return flames(i,true) end},
    {"sparks",3,110,sparks},
  }) do
    local frames = {}
    for i=0,set[2]-1 do frames[#frames+1] = set[4](i) end
    local name = "Level/v2/" .. set[1]
    L.saveFrames(frames,name,name,set[3])
  end
  L.save(night(),"Level/v2/night_overlay","Level/v2/night_overlay")
end
