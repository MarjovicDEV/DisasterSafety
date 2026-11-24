-- The 16x16 Choice icons for the rollout Levels, in the style of the pilot's four icons in
-- ui_v2.lua (one dark outline, three shades with light from the left, the Student palette).
-- Each Setting slice adds its icons to M.icons(); build.lua saves them as UI/icon_<name>
-- (run with --script-param only=icons_v2). The pilot's icon_door, icon_desk, icon_window and
-- icon_cabinet stay in ui_v2.lua and are reused where a Choice matches.

return function(L, ST)
  local M = {}
  local K = ST.KEY
  local O = K.o
  local function rgb(r, g, b) return app.pixelColor.rgba(r, g, b, 255) end

  -- Colour ramps: light, base, shadow.
  M.WOOD = { rgb(218, 154, 98), rgb(184, 118, 72), rgb(138, 84, 54) }
  M.GLASS = { rgb(196, 232, 248), rgb(150, 204, 232), rgb(108, 164, 204) }
  M.GREEN = { rgb(120, 196, 104), rgb(84, 160, 80), rgb(56, 118, 62) }
  M.RED = { rgb(240, 130, 126), rgb(214, 64, 64), rgb(128, 40, 52) }
  M.POT = { rgb(232, 140, 96), rgb(200, 104, 70), rgb(156, 72, 52) }   -- the v2 plant pots
  M.METAL = { rgb(214, 218, 228), rgb(170, 176, 192), rgb(124, 128, 146) }
  M.SAGE = { rgb(156, 204, 176), rgb(124, 180, 150), rgb(96, 152, 126) }
  M.NAVY = { K.m, K.N, K.n }
  M.WHITE = { K.W, K.w, K.v }
  M.SKIN = { K.t, K.S, K.s }
  M.SUN = rgb(252, 209, 22)
  M.O = O

  -- An outlined box filled with a ramp: light on the top and left, shadow on the right and bottom.
  function M.box(img, x, y, w, h, ramp)
    L.rect(img, x, y, w, h, O)
    L.rect(img, x + 1, y + 1, w - 2, h - 2, ramp[2])
    L.vline(img, x + 1, y + 1, y + h - 2, ramp[1])
    L.hline(img, x + 1, x + w - 2, y + 1, ramp[1])
    L.vline(img, x + w - 2, y + 2, y + h - 2, ramp[3])
    L.hline(img, x + 2, x + w - 2, y + h - 2, ramp[3])
  end

  -- Clears the 4 corner pixels of a box so it reads rounded.
  function M.round(img, x, y, w, h)
    for _, p in ipairs({ { x, y }, { x + w - 1, y }, { x, y + h - 1 }, { x + w - 1, y + h - 1 } }) do
      img:drawPixel(p[1], p[2], L.CLEAR)
    end
  end

  local function schoolyardIcons(out)
    -- The direct Choices show the landmark at full size.
    local tree = L.newImage(16, 16)
    L.rect(tree, 6, 7, 4, 7, M.WOOD[2])
    L.vline(tree, 6, 9, 13, M.WOOD[1])
    L.vline(tree, 9, 9, 13, M.WOOD[3])
    L.poly(tree, { 1, 6, 3, 3, 6, 3, 8, 1, 11, 2, 13, 4, 14, 7, 13, 9,
      10, 9, 8, 10, 5, 9, 2, 9 }, M.GREEN[3])
    L.poly(tree, { 2, 6, 5, 4, 7, 4, 8, 2, 11, 3, 13, 5, 13, 7, 10, 8, 5, 7 }, M.GREEN[2])
    L.hline(tree, 4, 7, 5, M.GREEN[1])
    L.hline(tree, 8, 10, 3, M.GREEN[1])
    L.outline(tree, O)
    out.tree = tree

    local field = L.newImage(16, 16)
    L.poly(field, { 1, 7, 5, 5, 10, 5, 14, 7, 14, 12, 11, 14, 4, 14, 1, 12 }, M.GREEN[3])
    L.poly(field, { 2, 7, 5, 6, 10, 6, 13, 7, 13, 11, 10, 13, 4, 13, 2, 11 }, M.GREEN[2])
    L.hline(field, 5, 10, 6, M.GREEN[1])
    L.hline(field, 3, 6, 9, M.GREEN[1])
    L.px(field, 10, 10, M.GREEN[1])
    L.outline(field, O)
    L.px(field, 3, 2, M.SUN)
    L.hline(field, 2, 4, 3, M.SUN)
    out.field = field

    local building = L.newImage(16, 16)
    M.box(building, 2, 6, 12, 9, M.WHITE)
    L.poly(building, { 0, 7, 5, 2, 10, 2, 15, 7, 15, 8, 0, 8 }, O)
    L.poly(building, { 2, 6, 5, 3, 10, 3, 13, 6, 13, 7, 2, 7 }, M.RED[2])
    L.line(building, 3, 6, 5, 4, M.RED[1])
    L.hline(building, 5, 13, 7, M.RED[3])
    L.rect(building, 6, 9, 4, 6, O)
    L.rect(building, 7, 10, 2, 5, M.WOOD[3])
    L.px(building, 7, 10, M.WOOD[1])
    L.rect(building, 3, 10, 2, 2, M.GLASS[2])
    L.rect(building, 11, 10, 2, 2, M.GLASS[3])
    out.building = building

    local post = L.newImage(16, 16)
    M.box(post, 5, 1, 5, 14, M.METAL)
    L.line(post, 8, 4, 14, 6, O)
    L.line(post, 8, 7, 15, 9, O)
    M.box(post, 2, 4, 11, 4, M.METAL)
    L.px(post, 3, 3, M.GLASS[2])
    L.px(post, 11, 3, M.GLASS[2])
    out.post = post

    -- The route Choices pair an arrow in the foreground with a smaller landmark.
    local function route(ramp)
      local img = L.newImage(16, 16)
      L.poly(img, { 1, 14, 3, 10, 4, 8, 6, 6, 4, 6, 8, 3,
        9, 7, 7, 7, 5, 11, 5, 14 }, ramp[2])
      L.line(img, 2, 13, 5, 8, ramp[1])
      L.outline(img, O)
      return img
    end

    local routePost = route(M.METAL)
    M.box(routePost, 11, 2, 4, 10, M.METAL)
    L.hline(routePost, 10, 15, 4, O)
    L.px(routePost, 15, 6, O)
    out.route_post = routePost

    local routeTree = route(M.WOOD)
    L.vline(routeTree, 12, 6, 11, O)
    L.line(routeTree, 12, 8, 12, 10, M.WOOD[1])
    L.poly(routeTree, { 9, 4, 12, 1, 14, 2, 15, 5, 14, 7, 10, 7, 9, 6 }, O)
    L.poly(routeTree, { 10, 4, 12, 2, 14, 3, 14, 5, 13, 6, 10, 6 }, M.GREEN[2])
    L.px(routeTree, 11, 4, M.GREEN[1])
    out.route_tree = routeTree

    local routeField = route(M.GREEN)
    L.vline(routeField, 12, 6, 11, O)
    M.box(routeField, 9, 2, 7, 6, M.GREEN)
    L.hline(routeField, 11, 13, 4, M.WHITE[1])
    L.hline(routeField, 11, 12, 5, M.WHITE[1])
    out.route_field = routeField

    local later = L.newImage(16, 16)
    L.circle(later, 9, 6, 5, O)
    L.circle(later, 9, 6, 4, M.WHITE[2])
    L.circle(later, 8, 5, 2, M.WHITE[1])
    L.vline(later, 9, 3, 6, M.NAVY[3])
    L.hline(later, 9, 11, 6, M.NAVY[3])
    L.poly(later, { 1, 12, 5, 9, 5, 11, 12, 11, 14, 9, 14, 12,
      12, 14, 5, 14, 5, 15 }, M.WOOD[2])
    L.hline(later, 5, 11, 12, M.WOOD[1])
    L.outline(later, O)
    out.route_later = later
  end

  local function beachIcons(out)
    local fish = L.newImage(16, 16)
    L.poly(fish, { 3, 8, 1, 5, 4, 6, 6, 7, 8, 4, 10, 5, 12, 5,
      14, 7, 14, 9, 12, 11, 9, 11, 5, 9, 1, 11 }, M.GLASS[3])
    L.poly(fish, { 5, 8, 8, 6, 12, 6, 13, 8, 12, 10, 8, 10 }, M.GLASS[2])
    L.line(fish, 7, 8, 10, 6, M.GLASS[1])
    L.px(fish, 12, 7, O)
    L.outline(fish, O)
    out.fish = fish

    local watch = L.newImage(16, 16)
    L.poly(watch, { 2, 5, 5, 2, 10, 2, 13, 5, 10, 8, 5, 8 }, M.WHITE[3])
    L.poly(watch, { 3, 5, 5, 3, 10, 3, 12, 5, 10, 7, 5, 7 }, M.WHITE[1])
    L.circle(watch, 7, 5, 2, M.NAVY[2])
    L.px(watch, 7, 5, M.GLASS[1])
    L.poly(watch, { 1, 12, 4, 10, 6, 10, 8, 12, 10, 12, 12, 10, 14, 10,
      14, 13, 12, 14, 9, 14, 6, 12, 4, 12, 1, 14 }, M.GLASS[3])
    L.line(watch, 1, 12, 4, 10, M.GLASS[1])
    L.line(watch, 10, 12, 13, 10, M.GLASS[2])
    L.outline(watch, O)
    out.watch_waves = watch

    local high = L.newImage(16, 16)
    L.poly(high, { 1, 13, 4, 12, 7, 8, 10, 8, 12, 6, 14, 6, 14, 14, 1, 14 }, M.GREEN[3])
    L.poly(high, { 3, 13, 6, 11, 8, 9, 11, 9, 13, 7, 14, 7, 14, 12, 11, 12, 8, 13 }, M.GREEN[2])
    L.line(high, 4, 12, 7, 9, M.GREEN[1])
    L.poly(high, { 2, 7, 7, 2, 5, 2, 5, 1, 10, 1, 10, 6, 9, 6, 9, 3, 4, 8 }, M.WHITE[2])
    L.line(high, 3, 7, 8, 2, M.WHITE[1])
    L.outline(high, O)
    out.high_ground = high

    local wait = L.newImage(16, 16)
    L.poly(wait, { 3, 10, 12, 10, 14, 14, 1, 14 }, M.SAGE[3])
    L.hline(wait, 3, 12, 11, M.SAGE[1])
    L.hline(wait, 3, 12, 12, M.SAGE[2])
    L.hline(wait, 2, 13, 13, M.SAGE[1])
    L.circle(wait, 7, 5, 4, M.WHITE[3])
    L.circle(wait, 7, 5, 3, M.WHITE[1])
    L.line(wait, 7, 3, 7, 5, M.NAVY[3])
    L.line(wait, 7, 5, 9, 6, M.NAVY[3])
    L.px(wait, 5, 3, M.GLASS[1])
    L.outline(wait, O)
    out.wait_beach = wait
  end

  -- The icons by name (without the icon_ prefix). Each slice adds its own.
  local function mallIcons(out)
    local elevator = L.newImage(16, 16)
    M.box(elevator, 1, 3, 12, 13, M.METAL)
    L.rect(elevator, 3, 5, 8, 9, M.METAL[2])
    L.vline(elevator, 3, 5, 13, M.METAL[1])
    L.vline(elevator, 9, 5, 13, M.METAL[3])
    L.vline(elevator, 7, 5, 13, O)
    L.poly(elevator, { 4, 2, 6, 0, 8, 2 }, M.NAVY[2])
    L.rect(elevator, 13, 8, 3, 5, O)
    L.px(elevator, 14, 9, M.GREEN[1])
    out.elevator = elevator

    local shop = L.newImage(16, 16)
    M.box(shop, 0, 3, 16, 12, M.METAL)
    L.rect(shop, 2, 5, 12, 8, M.GLASS[2])
    L.rect(shop, 3, 8, 3, 4, M.RED[2])
    L.rect(shop, 7, 9, 3, 3, M.WOOD[2])
    L.rect(shop, 11, 7, 2, 5, M.GREEN[3])
    L.hline(shop, 2, 13, 12, M.GLASS[3])
    L.line(shop, 3, 8, 6, 5, M.GLASS[1])
    L.line(shop, 9, 9, 12, 6, M.GLASS[1])
    M.box(shop, 1, 1, 14, 4, M.SAGE)
    out.shop_window = shop

    local rush = L.newImage(16, 16)
    L.circle(rush, 4, 4, 2, K.S)
    L.poly(rush, { 3, 6, 6, 6, 8, 7, 10, 7, 10, 9, 7, 9, 5, 8,
      6, 10, 9, 14, 7, 14, 4, 11, 2, 14, 0, 14, 2, 10 }, M.RED[2])
    L.line(rush, 4, 7, 3, 10, M.RED[1])
    L.outline(rush, O)
    L.circle(rush, 13, 4, 2, O)
    L.px(rush, 13, 4, K.t)
    L.rect(rush, 11, 7, 5, 5, O)
    L.rect(rush, 12, 8, 3, 3, M.SAGE[2])
    L.vline(rush, 12, 11, 14, O)
    L.vline(rush, 14, 11, 14, O)
    out.run_push = rush

    local wall = L.newImage(16, 16)
    L.poly(wall, { 2, 3, 10, 1, 14, 3, 14, 13, 10, 15, 2, 13 }, O)
    L.poly(wall, { 3, 4, 10, 2, 10, 14, 3, 12 }, M.WOOD[1])
    L.poly(wall, { 11, 3, 13, 4, 13, 12, 11, 13 }, M.WOOD[3])
    L.rect(wall, 4, 5, 6, 5, O)
    L.rect(wall, 5, 6, 4, 3, M.GLASS[2])
    L.hline(wall, 5, 8, 6, M.GLASS[1])
    L.line(wall, 3, 11, 9, 13, M.WOOD[2])
    out.mall_wall = wall
  end

  local function homeAfterIcons(out)
    local pull = L.newImage(16, 16)
    -- Two angled arms end in separate hands.
    L.poly(pull, { 1, 3, 4, 2, 9, 6, 9, 8, 6, 9, 1, 6 }, M.SKIN[2])
    L.line(pull, 2, 3, 7, 6, M.SKIN[1])
    L.line(pull, 1, 6, 6, 9, M.SKIN[3])
    L.poly(pull, { 6, 12, 10, 9, 12, 10, 14, 12, 14, 14, 11, 14,
      9, 12, 7, 14 }, M.SKIN[2])
    L.line(pull, 7, 12, 10, 10, M.SKIN[1])
    L.rect(pull, 1, 3, 2, 4, M.WHITE[2])
    L.rect(pull, 13, 11, 2, 4, M.RED[2])
    L.outline(pull, O)
    out.pull_up = pull

    local call = L.newImage(16, 16)
    L.poly(call, { 5, 2, 8, 2, 10, 4, 10, 7, 12, 8, 10, 9,
      10, 11, 8, 12, 10, 14, 2, 14, 2, 6 }, M.SKIN[2])
    L.poly(call, { 4, 2, 8, 2, 10, 4, 7, 4, 6, 6, 3, 7 }, M.NAVY[3])
    L.line(call, 3, 8, 3, 12, M.SKIN[1])
    L.px(call, 9, 6, O)
    L.px(call, 9, 10, M.SKIN[3])
    L.hline(call, 3, 9, 14, M.WHITE[2])
    L.outline(call, O)
    L.line(call, 12, 5, 14, 3, M.NAVY[3])
    L.hline(call, 13, 15, 8, M.NAVY[3])
    L.line(call, 12, 11, 14, 13, M.NAVY[3])
    out.call_adult = call

    local pipe = L.newImage(16, 16)
    M.box(pipe, 1, 3, 14, 6, M.METAL)
    M.box(pipe, 1, 2, 4, 8, M.METAL)
    M.box(pipe, 12, 2, 3, 8, M.METAL)
    L.line(pipe, 9, 4, 7, 6, O)
    L.line(pipe, 7, 6, 9, 7, O)
    L.px(pipe, 8, 8, O)
    L.poly(pipe, { 8, 10, 10, 13, 8, 15, 6, 13 }, M.GLASS[3])
    L.px(pipe, 8, 12, M.GLASS[1])
    L.px(pipe, 8, 13, M.GLASS[2])
    out.fix_pipe = pipe

    local empty = L.newImage(16, 16)
    -- Keep the crossed-out bag separate from the red go bag used for the Safe Choice.
    L.poly(empty, { 5, 4, 11, 4, 13, 7, 13, 13, 11, 14, 4, 14,
      2, 12, 2, 7 }, M.METAL[2])
    L.hline(empty, 6, 10, 2, M.METAL[3])
    L.vline(empty, 6, 2, 4, M.METAL[2])
    L.vline(empty, 10, 2, 4, M.METAL[3])
    L.line(empty, 3, 7, 5, 5, M.METAL[1])
    L.hline(empty, 5, 11, 12, M.METAL[3])
    L.outline(empty, O)
    L.line(empty, 2, 13, 13, 2, M.RED[3], 3)
    L.line(empty, 2, 13, 13, 2, M.RED[1])
    out.no_go_bag = empty

    local note = L.newImage(16, 16)
    M.box(note, 2, 1, 12, 14, M.WHITE)
    L.hline(note, 5, 10, 4, M.NAVY[2])
    L.hline(note, 5, 10, 6, M.NAVY[2])
    L.hline(note, 5, 8, 8, M.NAVY[1])
    L.line(note, 2, 13, 13, 2, M.RED[3], 3)
    L.line(note, 2, 13, 13, 2, M.RED[1])
    out.no_note = note

    local toy = L.newImage(16, 16)
    L.poly(toy, { 1, 8, 4, 8, 6, 4, 10, 4, 13, 8, 14, 8,
      14, 12, 1, 12 }, M.RED[2])
    L.hline(toy, 2, 13, 9, M.RED[1])
    L.hline(toy, 2, 13, 12, M.RED[3])
    L.poly(toy, { 6, 7, 7, 5, 10, 5, 11, 7 }, M.GLASS[1])
    L.circle(toy, 4, 12, 2, M.NAVY[3])
    L.circle(toy, 11, 12, 2, M.NAVY[3])
    L.px(toy, 4, 12, M.METAL[1])
    L.px(toy, 11, 12, M.METAL[1])
    L.outline(toy, O)
    out.toy_car = toy
  end

  -- Hard 7, evacuation area: going home, the radio, a rumor and the phone all day.
  local function evacuationIcons(out)
    -- go_home: a small house on the right with an arrow pointing toward it.
    local home = L.newImage(16, 16)
    M.box(home, 8, 8, 7, 7, M.WOOD)
    L.poly(home, { 6, 9, 11, 3, 12, 3, 15, 7, 15, 9 }, O)
    L.poly(home, { 8, 8, 11, 4, 12, 4, 14, 7, 14, 8 }, M.RED[2])
    L.line(home, 9, 7, 11, 5, M.RED[1])
    L.rect(home, 11, 11, 2, 3, M.WOOD[3])
    L.hline(home, 0, 5, 11, M.NAVY[2])
    L.hline(home, 0, 5, 12, M.NAVY[2])
    L.poly(home, { 4, 8, 7, 11, 7, 12, 4, 15 }, M.NAVY[2])
    L.hline(home, 1, 4, 11, M.NAVY[1])
    L.outline(home, O)
    out.go_home = home

    -- radio: the red radio with a speaker grille and a dial, and sound arcs above it.
    local radio = L.newImage(16, 16)
    M.box(radio, 0, 7, 16, 9, M.RED)
    L.rect(radio, 2, 9, 6, 5, M.METAL[3])
    L.hline(radio, 2, 7, 10, M.METAL[1])
    L.hline(radio, 2, 7, 12, M.METAL[1])
    L.rect(radio, 10, 9, 3, 3, M.METAL[2])
    L.px(radio, 10, 9, M.METAL[1])
    L.line(radio, 4, 6, 2, 1, O)
    L.px(radio, 9, 4, M.SUN); L.px(radio, 10, 3, M.SUN); L.px(radio, 11, 3, M.SUN); L.px(radio, 12, 4, M.SUN)
    L.px(radio, 8, 2, M.SUN); L.px(radio, 9, 1, M.SUN); L.hline(radio, 10, 11, 0, M.SUN)
    L.px(radio, 12, 1, M.SUN); L.px(radio, 13, 2, M.SUN)
    out.radio = radio

    -- rumor: two speech bubbles, the front one with a question mark.
    local rumor = L.newImage(16, 16)
    M.box(rumor, 0, 0, 10, 8, M.WHITE)
    M.round(rumor, 0, 0, 10, 8)
    L.px(rumor, 2, 8, O); L.px(rumor, 3, 8, O); L.px(rumor, 2, 9, O)
    L.hline(rumor, 2, 6, 3, M.WHITE[3])
    L.hline(rumor, 2, 5, 5, M.WHITE[3])
    M.box(rumor, 5, 5, 11, 9, M.SAGE)
    M.round(rumor, 5, 5, 11, 9)
    L.px(rumor, 12, 14, O); L.px(rumor, 13, 14, O); L.px(rumor, 13, 15, O)
    L.hline(rumor, 9, 11, 7, O)
    L.px(rumor, 12, 8, O)
    L.px(rumor, 11, 9, O)
    L.px(rumor, 10, 10, O)
    L.px(rumor, 10, 12, O)
    out.rumor = rumor

    -- phone_all_day: a phone with a small clock at its corner.
    local phone = L.newImage(16, 16)
    M.box(phone, 1, 0, 9, 16, M.NAVY)
    M.round(phone, 1, 0, 9, 16)
    L.rect(phone, 3, 3, 5, 9, M.GLASS[2])
    L.hline(phone, 3, 7, 3, M.GLASS[1])
    L.hline(phone, 4, 6, 13, M.METAL[1])
    L.circle(phone, 11, 11, 4, O)
    L.circle(phone, 11, 11, 3, M.WHITE[1])
    L.vline(phone, 11, 9, 11, O)
    L.hline(phone, 11, 13, 11, O)
    out.phone_all_day = phone
  end

  function M.icons()
    local out = {}

    -- Easy 2, corridor ----------------------------------------------------------------------
    -- stairs: steps going down to the right, with a handrail
    local s = L.newImage(16, 16)
    -- four steps, each 4 wide and 3 lower than the last: a lit tread, the base, a shaded drop
    for i = 0, 3 do
      local x, top = i * 4, 5 + i * 3
      L.rect(s, x, top, 4, 16 - top, M.WHITE[2])
      L.hline(s, x, x + 3, top, M.WHITE[1])
      L.vline(s, x + 3, top + 1, 15, M.WHITE[3])
    end
    L.line(s, 1, 2, 14, 10, M.WOOD[2])             -- the handrail going down with the steps
    L.line(s, 1, 1, 14, 9, M.WOOD[1])
    L.vline(s, 1, 2, 4, M.WOOD[3])                 -- its post at the top step
    L.outline(s, O)
    out.stairs = s

    -- cover: a Student kneeling low and facing right, one arm over the head and neck
    -- (drawn after the Student's own cover pose, with the Student palette)
    local c = L.newImage(16, 16)
    L.map(c, 0, 3, {
      "..........ooo...",
      "........ootSSo..",
      "......oootSSSso.",
      ".....oWWoHHHHSo.",
      "....oWWwwoHHjSso",
      "...oWWwwvoHtSSo.",
      "...oWwwwvohSSso.",
      "..oWwwwwvvooooo.",
      "..oxxxxxxxo.....",
      "..omNNNNNNno....",
      ".obbBo.obbBo....",
      ".ooooo.ooooo....",
    }, K)
    out.cover = c

    -- plants: a pot hanging on its chains
    local p = L.newImage(16, 16)
    L.vline(p, 8, 0, 4, M.NAVY[3])
    L.line(p, 3, 8, 8, 4, M.NAVY[3]); L.line(p, 13, 8, 8, 4, M.NAVY[3])
    L.circle(p, 5, 7, 2, M.GREEN[2]); L.circle(p, 11, 7, 2, M.GREEN[2]); L.circle(p, 8, 6, 2, M.GREEN[1])
    M.box(p, 2, 9, 12, 3, M.POT)
    L.poly(p, { 3, 12, 12, 12, 11, 15, 4, 15 }, M.POT[2])
    L.vline(p, 4, 12, 14, M.POT[1]); L.vline(p, 11, 12, 14, M.POT[3])
    L.rect(p, 1, 9, 1, 4, M.GREEN[3])
    L.outline(p, O)
    out.plants = p

    -- Easy 3, classroom after the shaking --------------------------------------------------
    -- stay: the Student stays low behind the top and the front legs of the desk.
    local stay = L.newImage(16, 16)
    L.map(stay, 0, 0, {
      "................",
      "................",
      ".oooooooooooooo.",
      ".oAAAAAAAAAAAao.",
      ".oaaaaaaaaaaaao.",
      ".oooooooooooooo.",
      "..oAo..oooo.oao.",
      "..oAo.oHHHHooao.",
      "..oAo.oHjSSooao.",
      "..oAo.oHtSsooao.",
      "..oAo.oWwvo.oao.",
      "..oAooWWwvo.oao.",
      "..oAoomNnoo.oao.",
      "..oAoooooo..oao.",
      "..ooo.......ooo.",
      "................",
    }, setmetatable({ A = M.WOOD[1], a = M.WOOD[2] }, { __index = K }))
    out.stay = stay

    -- walk with the Teacher: the taller figure leads, the Student keeps a step back.
    local walk = L.newImage(16, 16)
    L.map(walk, 0, 0, {
      "..........ooo...",
      ".........oHHHo..",
      ".........oHjSSo.",
      ".........oHtSso.",
      "...ooo...oomno..",
      "..oHHHo..omNno..",
      "..oHtSsoomprno..",
      "..oWtsoo.omNnSo.",
      "..oWwvo..omNnno.",
      "..oWvwSo.omNno..",
      "..omNNno.omNno..",
      "..omNno..oSoo...",
      "...oSoo..oSoSo..",
      "..oSoSo.oBooSo..",
      "..oooBo.oo.oBBo.",
      "....ooo....oooo.",
    }, K)
    out.teacher_walk = walk

    -- push: the Student leans forward with both arms out behind the taller figure.
    local push = L.newImage(16, 16)
    L.map(push, 0, 0, {
      "...........ooo..",
      "..........oHHHo.",
      "..........oHjSSo",
      "..........oHtSso",
      "....ooo...oomno.",
      "...oHHHo..omNno.",
      "...oHtSsoomprno.",
      "....oWwwooNNno..",
      "...oWwwwtSmNno..",
      "...oWvvooomNno..",
      "..omNno..omNno..",
      "..omNno...oSoo..",
      ".omNoNno..oSoSo.",
      ".oBo.oNo.oBooSo.",
      ".oo..oBBo.ooBBo.",
      "......ooo...ooo.",
    }, K)
    out.push = push

    -- Normal 1, bedroom ----------------------------------------------------------------------
    -- picture: a framed hill and sun hanging from a nail on its string
    local pic = L.newImage(16, 16)
    L.line(pic, 3, 5, 7, 1, M.NAVY[3]); L.line(pic, 12, 5, 8, 1, M.NAVY[3])
    L.px(pic, 7, 0, M.NAVY[2]); L.px(pic, 8, 0, M.NAVY[2])
    M.box(pic, 0, 4, 16, 12, M.WOOD)
    L.rect(pic, 2, 6, 12, 8, O)
    L.rect(pic, 3, 7, 10, 6, M.GLASS[1])
    L.px(pic, 10, 8, M.SUN); L.px(pic, 11, 8, M.SUN)
    L.poly(pic, { 3, 11, 6, 9, 9, 11, 12, 10, 12, 12, 3, 12 }, M.GREEN[2])
    L.hline(pic, 3, 12, 12, M.GREEN[3])
    out.picture = pic

    -- Low shelf with an empty upper tier and a taped box on the lower tier.
    local sh = L.newImage(16, 16)
    M.box(sh, 0, 3, 16, 12, M.WOOD)
    L.rect(sh, 2, 5, 12, 3, M.WOOD[3])
    L.rect(sh, 2, 9, 12, 4, M.WOOD[3])
    L.hline(sh, 2, 13, 8, M.WOOD[1])
    L.rect(sh, 5, 9, 7, 4, M.POT[2])
    L.vline(sh, 5, 9, 12, M.POT[1])
    L.vline(sh, 11, 9, 12, M.POT[3])
    L.vline(sh, 8, 9, 10, M.WHITE[1])
    out.low_shelf = sh

    -- Side-view bed with a raised headboard, pillow, blanket and two legs.
    local bd = L.newImage(16, 16)
    L.rect(bd, 1, 4, 2, 10, M.WOOD[2])
    L.rect(bd, 13, 9, 2, 5, M.WOOD[2])
    L.rect(bd, 3, 10, 10, 2, M.WOOD[2])
    L.rect(bd, 3, 8, 10, 2, M.WHITE[2])
    L.hline(bd, 3, 5, 7, M.WHITE[1])
    L.rect(bd, 7, 7, 6, 3, M.NAVY[2])
    L.hline(bd, 7, 12, 7, M.NAVY[1])
    L.hline(bd, 8, 12, 9, M.NAVY[3])
    L.vline(bd, 1, 4, 12, M.WOOD[1])
    L.hline(bd, 3, 12, 11, M.WOOD[3])
    L.vline(bd, 14, 10, 13, M.WOOD[3])
    L.outline(bd, O)
    out.bed = bd

    -- books: a stack of three books, pages on the right
    local bk = L.newImage(16, 16)
    local function book(x, y, w, ramp)
      M.box(bk, x, y, w, 4, ramp)
      L.hline(bk, x + 2, x + w - 3, y + 2, M.WHITE[1])
    end
    book(0, 11, 16, M.NAVY)
    book(2, 7, 13, M.RED)
    book(1, 3, 14, M.GREEN)
    out.books = bk

    -- Normal 2, living room: a clock, storage boxes, the go bag, and snacks with a toy.
    local later = L.newImage(16, 16)
    L.circle(later, 8, 8, 7, O)
    L.circle(later, 8, 8, 6, M.METAL[3])
    L.circle(later, 8, 8, 5, M.WHITE[2])
    L.line(later, 5, 5, 7, 4, M.WHITE[1])
    L.vline(later, 8, 4, 8, O)
    L.hline(later, 8, 11, 8, O)
    L.px(later, 8, 12, M.METAL[3])
    out.pack_later = later

    local hidden = L.newImage(16, 16)
    M.box(hidden, 1, 2, 14, 7, M.WOOD)
    L.vline(hidden, 7, 3, 6, M.WHITE[2])
    L.rect(hidden, 1, 8, 2, 6, M.WOOD[3])
    L.rect(hidden, 13, 8, 2, 6, M.WOOD[3])
    M.box(hidden, 5, 10, 6, 5, M.RED)
    L.px(hidden, 8, 12, M.WHITE[1])
    out.bag_hidden = hidden

    local bag = L.newImage(16, 16)
    L.rect(bag, 6, 1, 5, 1, M.RED[2])
    L.px(bag, 6, 2, M.RED[2]); L.px(bag, 10, 2, M.RED[3])
    L.poly(bag, { 4, 3, 11, 3, 13, 6, 13, 13, 11, 14, 4, 14, 2, 13, 2, 6 }, M.RED[2])
    L.line(bag, 2, 6, 4, 3, M.RED[1])
    L.vline(bag, 2, 7, 12, M.RED[1])
    L.vline(bag, 13, 6, 12, M.RED[3])
    L.hline(bag, 4, 11, 14, M.RED[3])
    L.rect(bag, 5, 7, 6, 4, M.RED[3])
    L.hline(bag, 5, 10, 7, M.RED[1])
    L.px(bag, 9, 8, M.WHITE[1])
    L.outline(bag, O)
    out.go_bag = bag

    local toys = L.newImage(16, 16)
    M.box(toys, 1, 2, 6, 12, M.POT)
    L.hline(toys, 2, 5, 7, M.SUN)
    L.poly(toys, { 6, 9, 8, 9, 10, 6, 13, 6, 15, 9, 15, 12, 6, 12 }, M.RED[2])
    L.hline(toys, 10, 12, 7, M.GLASS[1])
    L.hline(toys, 7, 14, 10, M.RED[1])
    L.circle(toys, 8, 12, 1, O); L.circle(toys, 13, 12, 1, O)
    L.outline(toys, O)
    out.snack_toy = toys

    -- Normal 3 reuses the pilot door. Glass panes distinguish this cabinet from the pilot bookcase.
    local glassCabinet = L.newImage(16, 16)
    M.box(glassCabinet, 2, 0, 12, 16, M.WOOD)
    L.rect(glassCabinet, 4, 2, 8, 10, M.GLASS[2])
    L.hline(glassCabinet, 4, 11, 7, M.GLASS[3])
    L.line(glassCabinet, 4, 6, 7, 2, M.WHITE[1])
    L.line(glassCabinet, 8, 10, 11, 6, M.WHITE[1])
    L.vline(glassCabinet, 8, 2, 11, M.WOOD[3])
    L.px(glassCabinet, 7, 7, O)
    L.px(glassCabinet, 9, 7, O)
    L.hline(glassCabinet, 6, 9, 13, M.WOOD[3])
    out.glass_cabinet = glassCabinet

    local sofa = L.newImage(16, 16)
    M.box(sofa, 2, 3, 12, 8, M.SAGE)
    L.vline(sofa, 8, 5, 8, M.SAGE[3])
    M.box(sofa, 1, 8, 14, 5, M.SAGE)
    M.box(sofa, 0, 7, 4, 6, M.SAGE)
    M.box(sofa, 12, 7, 4, 6, M.SAGE)
    L.rect(sofa, 2, 13, 2, 2, O); L.rect(sofa, 12, 13, 2, 2, O)
    out.sofa = sofa

    local tbl = L.newImage(16, 16)
    L.map(tbl, 0, 0, {
      "................",
      "................",
      "...oooooooooo...",
      "..oAAAAAAAAAAo..",
      ".oooooooooooooo.",
      ".oaaaaaaaaaaaao.",
      ".oooooooooooooo.",
      "..oAo..ooo.oao..",
      "..oAo.oHHHooao..",
      "..oAo.oHtSooao..",
      "..oAooWWwvooao..",
      "..oAooWWvvooao..",
      "..oAoomNnnooao..",
      "..oooooooooooo..",
      "................",
      "................",
    }, setmetatable({ A = M.WOOD[1], a = M.WOOD[2] }, { __index = K }))
    out.strong_table = tbl

    -- Hard 1, living room: separate paths, a park pin, a house, and a phone.
    local paths = L.newImage(16, 16)
    -- A common path ends at the fork. The two arrows point away from each other.
    L.rect(paths, 7, 8, 3, 7, M.NAVY[3])
    L.poly(paths, { 1, 2, 7, 2, 6, 4, 9, 8, 7, 10, 4, 6, 2, 7 }, M.NAVY[2])
    L.poly(paths, { 14, 2, 14, 7, 12, 6, 9, 10, 7, 8, 10, 4, 9, 2 }, M.RED[2])
    L.line(paths, 2, 3, 5, 3, M.NAVY[1])
    L.line(paths, 2, 4, 7, 9, M.NAVY[1])
    L.line(paths, 10, 3, 13, 3, M.RED[1])
    L.line(paths, 8, 8, 13, 3, M.RED[1])
    L.outline(paths, O)
    out.own_way = paths

    local meet = L.newImage(16, 16)
    M.box(meet, 0, 3, 16, 13, M.WOOD)
    L.rect(meet, 2, 5, 12, 9, M.GLASS[1])
    L.hline(meet, 2, 13, 13, M.GREEN[2])
    L.vline(meet, 5, 10, 13, M.WOOD[3])
    L.circle(meet, 5, 9, 2, M.GREEN[3])
    L.px(meet, 4, 8, M.GREEN[1])
    L.vline(meet, 11, 4, 10, O)
    L.circle(meet, 11, 4, 3, O)
    L.circle(meet, 11, 4, 2, M.RED[2])
    L.px(meet, 10, 3, M.RED[1])
    out.meet_park = meet

    local house = L.newImage(16, 16)
    M.box(house, 3, 7, 10, 8, M.WOOD)
    L.poly(house, { 0, 8, 7, 1, 9, 1, 15, 8, 15, 9, 0, 9 }, O)
    L.poly(house, { 2, 7, 7, 2, 9, 2, 13, 7, 13, 8, 2, 8 }, M.RED[2])
    L.line(house, 3, 7, 7, 3, M.RED[1])
    L.hline(house, 3, 12, 8, M.RED[3])
    L.rect(house, 7, 10, 3, 4, M.WOOD[3])
    L.px(house, 6, 11, M.GLASS[1])
    out.meet_house = house

    local phone = L.newImage(16, 16)
    M.box(phone, 3, 0, 10, 16, M.NAVY)
    M.round(phone, 3, 0, 10, 16)
    L.rect(phone, 5, 3, 6, 9, M.GLASS[2])
    L.hline(phone, 5, 10, 3, M.GLASS[1])
    L.vline(phone, 10, 4, 11, M.GLASS[3])
    L.hline(phone, 6, 8, 6, M.WHITE[1])
    L.hline(phone, 6, 9, 8, M.WHITE[2])
    L.hline(phone, 7, 8, 13, M.METAL[1])
    out.phone_numbers = phone

    schoolyardIcons(out)
    beachIcons(out)

    mallIcons(out)
    homeAfterIcons(out)
    evacuationIcons(out)

    return out
  end

  return M
end
