-- Shared ceiling Smoke and the localized roof plume use loopable phases.
return function(L)
  local M={}
  local tau=2*math.pi
  local function grey(r,g,b,a) return app.pixelColor.rgba(r,g,b,a) end
  local pale=grey(217,218,215,150)
  local mid=grey(184,186,185,163)
  local deep=grey(157,161,162,178)
  function M.band(f,n)
    local img=L.newImage(480,110)
    local phase=tau*f/n
    for x=0,479 do
      local t=tau*x/480
      local lower=94+6*math.sin(3*t)+4*math.sin(7*t+phase)+2*math.sin(13*t+phase)
      local middle=62+8*math.sin(4*t+0.4)+4*math.sin(8*t+phase)+3*math.sin(6*t-phase)
      local upper=29+5*math.sin(3*t-0.8)+4*math.sin(7*t-phase)
      L.vline(img,x,0,math.floor(lower),pale)
      L.vline(img,x,0,math.floor(middle),mid)
      L.vline(img,x,0,math.floor(upper),deep)
    end
    return img
  end
  function M.plume(f,n)
    local img=L.newImage(67,73)
    -- Evenly stagger the puffs. Each fades at both ends of its rising path.
    for i=0,9 do
      local t=((i+f/n)%10)/10
      local x=13+38*t*t+2*math.sin(2*tau*t)
      local y=67-55*t
      local radius=3+8*t
      local alpha=math.floor(165*math.min(1,t*16,(1-t)*5))
      local cx,cy,r=math.floor(x),math.floor(y),math.floor(radius)
      L.ellipse(img,cx,cy,r,r-1,grey(165,169,168,alpha))
      L.ellipse(img,cx-2,cy-1,math.max(1,r-2),math.max(1,r-3),grey(198,200,197,alpha))
    end
    return img
  end
  return M
end
