-- Builds every sprite for Disaster Safety.
-- Run from the project root:
--   aseprite.exe -b --script-param root=D:/GameDEV/DisasterSafety --script ArtSource/scripts/build.lua
-- Writes editable sources to ArtSource/*.aseprite and PNGs to Assets/Art/.

local root = app.params.root
assert(root and root ~= "", "pass --script-param root=<project root>")
local here = root .. "/ArtSource/scripts/"

local L = dofile(here .. "lib.lua")
L.F = dofile(here .. "font.lua")
L.ROOT = app.params.out or root

-- Optional: --script-param only=student,classroom_v2,ui_v2 builds just those parts, so new art
-- can be drawn without rewriting the committed files. Parts: v1 (everything below up to the
-- v2 section), student, classroom_v2, ui_v2. Without `only`, every part is built.
local only = {}
for part in (app.params.only or ""):gmatch("[^,%s]+") do only[part] = true end
local function want(part) return next(only) == nil or only[part] == true end

if want("v1") then
local BG = dofile(here .. "background.lua")(L)
local CH = dofile(here .. "characters.lua")(L)
local UI = dofile(here .. "ui.lua")(L)
local TU = dofile(here .. "tutorial.lua")(L, UI)

-- How to Play Tutorial Pages: Tutorial/<name>_0.png and _1.png
for _, page in ipairs(TU.pages) do
  L.saveFrames({ page.draw(0), page.draw(1) }, "Tutorial/" .. page.name, "Tutorial/" .. page.name, 400)
end

-- Ambient menu animation (the menu backgrounds are the "menu" part below)
L.saveFrames(BG.flagFrames(), "Background/flag", "Background/flag", 140)
local clouds = BG.clouds()
L.save(clouds.big, "Background/cloud_big", "Background/cloud_big")
L.save(clouds.small, "Background/cloud_small", "Background/cloud_small")
L.saveFrames(BG.birdFrames(), "Background/bird", "Background/bird", 160)

-- Characters
L.saveFrames(CH.studentFrames(false), "Characters/student_boy", "Characters/student_boy", 110)
L.saveFrames(CH.studentFrames(true), "Characters/student_girl", "Characters/student_girl", 110)

-- UI kit
for name, set in pairs(UI.BUTTONS) do
  L.save(UI.button(set, false), "UI/button_" .. name, "UI/button_" .. name)
  -- Gray is only the disabled and Tried look, so it has no pressed state.
  if name ~= "gray" then
    L.save(UI.button(set, true), "UI/button_" .. name .. "_pressed", "UI/button_" .. name .. "_pressed")
  end
end
L.save(UI.panel(), "UI/panel", "UI/panel")
L.save(UI.bubble(), "UI/bubble", "UI/bubble")
L.save(UI.inputBox(), "UI/input_box", "UI/input_box")
L.save(UI.switch(true), "UI/switch_on", "UI/switch_on")
L.save(UI.switch(false), "UI/switch_off", "UI/switch_off")
L.save(UI.solid(), "UI/solid", "UI/solid")
L.save(UI.sparkle(), "UI/sparkle", "UI/sparkle")
L.save(UI.leaf(L.P.green, L.P.green3), "UI/leaf_0", "UI/leaf_0")
L.save(UI.leaf(L.P.yellow, L.P.yellow2), "UI/leaf_1", "UI/leaf_1")
L.save(UI.title(), "UI/title_logo", "UI/title_logo")
L.save(UI.subtitle(), "UI/subtitle_ribbon", "UI/subtitle_ribbon")

for name, img in pairs(UI.icons()) do
  L.save(img, "Icons/icon_" .. name, "Icons/icon_" .. name)
end
for name, img in pairs(UI.episodes()) do
  L.save(img, "Episodes/episode_" .. name, "Episodes/episode_" .. name)
end

-- Font atlas and metrics
local atlas, lines = UI.fontAtlas()
L.save(atlas, "Font/pixel_font", "Font/pixel_font")
local f = io.open(L.ROOT .. "/Assets/Art/Font/pixel_font.txt", "w")
f:write("# codepoint x y width (atlas top-left origin, cell height 11, baseline after row 9)\n")
for _, line in ipairs(lines) do f:write(line, "\n") end
f:close()
end -- v1

-- v2 art (docs/adr/0001-v2-art-and-no-bantay.md) ----------------------------
local ST = dofile(here .. "student.lua")(L)

-- The menu backgrounds (C3, C4): Background/bg_menu_<name>.png, one per MenuBackground.
if want("menu") then
  local MB = dofile(here .. "background.lua")(L, ST)
  for _, b in ipairs(MB.BACKGROUNDS) do
    local name = "Background/bg_menu_" .. b[1]
    L.save(b[2](), name, name)
  end
end

if want("fire_fx") then
  dofile(here .. "fire_fx.lua")(L)
end

if only.typhoon_fx then
  dofile(here .. "typhoon_fx.lua")(L, root)
end

if want("menu_typhoon") then
  local menu = dofile(here .. "background.lua")(L, ST)
  local home = dofile(here .. "home_v2.lua")(L, ST)
  local court = dofile(here .. "evacuation_area_v2.lua")(L, ST)
  for name, image in pairs({
    barangay_hall = menu.barangayHall(), typhoon_easy = menu.goBagDoorway(home, court),
    typhoon_normal = menu.rainyWindows(), typhoon_hard = menu.evacuationGym(),
  }) do
    local path = "Background/bg_menu_" .. name
    L.save(image, path, path)
  end
end

if want("menu_fire") then
  local menu = dofile(here .. "background.lua")(L, ST)
  local home = dofile(here .. "home_v2.lua")(L, ST)
  for _, entry in ipairs({
    { "easy", menu.fireEasy(home) }, { "normal", menu.fireNormal() }, { "hard", menu.fireHard() },
  }) do
    local name = "Background/bg_menu_fire_" .. entry[1]
    L.save(entry[2], name, name)
  end
end

-- The school gate scene on the Main Menu: the gate leaves (closed to open), the guard, and the
-- Students walking in seen from behind.
if want("menu_gate") then
  local MB = dofile(here .. "background.lua")(L, ST)
  local CH = dofile(here .. "characters.lua")(L)
  L.saveFrames(MB.gateFrames(), "Background/gate", "Background/gate", 120)
  L.saveFrames({ CH.guard("idle", 0), CH.guard("blink", 0) }, "Characters/guard_idle", "Characters/guard_idle", 400)
  L.save(CH.guard("reach", 0), "Characters/guard_reach", "Characters/guard_reach")
  L.saveFrames({ CH.guard("wave", 0), CH.guard("wave", 1) }, "Characters/guard_wave", "Characters/guard_wave", 250)
  L.saveFrames(CH.studentBackFrames(false), "Characters/student_boy_back", "Characters/student_boy_back", 140)
  L.saveFrames(CH.studentBackFrames(true), "Characters/student_girl_back", "Characters/student_girl_back", 140)
end

-- The Student, boy and girl: Student/<kind>_<pose>_<n>.png and Student/<kind>_portrait.png
-- Optional: --script-param poses=carry,reach writes only those poses (and not the portrait), so
-- new poses can be drawn without rewriting the committed ones.
local posesOnly = {}
for p in (app.params.poses or ""):gmatch("[^,%s]+") do posesOnly[p] = true end
local allPoses = next(posesOnly) == nil
if only.student_typhoon and (allPoses or posesOnly.flashlight) then
  local home = dofile(here .. "home_v2.lua")(L, ST)
  for _, kind in ipairs({"boy","girl"}) do
    local name = "Student/" .. kind .. "_flashlight"
    L.saveFrames(ST.flashlightFrames(kind,home.flashlight()),name,name,350)
  end
end
if want("student_fire") then
  for _, kind in ipairs({"boy","girl"}) do
    for _, pose in ipairs(ST.FIRE_POSES) do
      if allPoses or posesOnly[pose.name] then
        local name = "Student/" .. kind .. "_" .. pose.name
        L.saveFrames(ST.fireFrames(kind,pose),name,name,pose.ms)
      end
    end
  end
  if allPoses or posesOnly.sleeve_flame then
    local name = "Student/sleeve_flame"
    L.saveFrames(ST.sleeveFlames(),name,name,150)
  end
end
if want("student") then
  for _, kind in ipairs({ "boy", "girl" }) do
    for _, pose in ipairs(ST.POSES) do
      if allPoses or posesOnly[pose.name] then
        local name = "Student/" .. kind .. "_" .. pose.name
        L.saveFrames(ST.frames(kind, pose.name), name, name, pose.ms)
      end
    end
    if allPoses then
      L.save(ST.portraitImage(kind), "Student/" .. kind .. "_portrait", "Student/" .. kind .. "_portrait")
    end
  end
end

-- The v2 classroom: Level/v2/bg_classroom, desk_front, fan_0/1, cabinet_0/1
local CR = dofile(here .. "classroom_v2.lua")(L, ST)
if want("classroom_v2") then
  L.save(CR.background(), "Level/v2/bg_classroom", "Level/v2/bg_classroom")
  L.save(CR.deskFront(), "Level/v2/desk_front", "Level/v2/desk_front")
  L.saveFrames(CR.fanFrames(), "Level/v2/fan", "Level/v2/fan", 120)
  L.saveFrames(CR.cabinetFrames(), "Level/v2/cabinet", "Level/v2/cabinet", 120)
end

-- The v2 How to Play Tutorial Pages (ticket 13): Tutorial/tutorial_v2_<name>_0.png and _1.png.
-- tutorial.lua and its pilot pages are not changed.
if want("tutorial_v2") then
  local UV = dofile(here .. "ui_v2.lua")(L, ST, CR)
  local TV = dofile(here .. "tutorial_v2.lua")(L, ST, CR, UV)
  for _, page in ipairs(TV.pages) do
    local path = "Tutorial/tutorial_" .. page.name
    L.saveFrames({ page.draw(0), page.draw(1) }, path, path, 400)
  end
end

-- The v2 classroom after the shaking (Easy 3): its own background and desk-front overlay.
if want("classroom_after_v2") then
  local IC = dofile(here .. "icons_v2.lua")(L, ST)
  local CA = dofile(here .. "classroom_after_v2.lua")(L, ST, CR, IC)
  L.save(CA.background(), "Level/v2/bg_classroom_after", "Level/v2/bg_classroom_after")
  L.save(CA.deskFront(), "Level/v2/classroom_after_desk_front", "Level/v2/classroom_after_desk_front")
end

-- v2 UI: the door and desk Choice icons and the Caption, Action Bar button and speech bubble 9-slices
if want("ui_v2") then
  local UV = dofile(here .. "ui_v2.lua")(L, ST, CR)
  for name, img in pairs(UV.icons()) do
    L.save(img, "UI/icon_" .. name, "UI/icon_" .. name)
  end
  L.save(UV.captionBanner(), "UI/caption_banner", "UI/caption_banner")
  L.save(UV.actionButton(), "UI/action_button", "UI/action_button")
  L.save(UV.speechBubble(), "UI/speech_bubble", "UI/speech_bubble")
end

-- Rollout icons (16x16), added slice by slice: UI/icon_<name>
-- Optional: --script-param icons=stairs,cover writes only those icons, so a slice adds its own
-- icons without rewriting the ones earlier slices committed.
if want("icons_v2") then
  local IC = dofile(here .. "icons_v2.lua")(L, ST)
  local iconsOnly = {}
  for n in (app.params.icons or ""):gmatch("[^,%s]+") do iconsOnly[n] = true end
  for name, img in pairs(IC.icons()) do
    if next(iconsOnly) == nil or iconsOnly[name] then
      L.save(img, "UI/icon_" .. name, "UI/icon_" .. name)
    end
  end
end

-- The v2 corridor (Easy 2): Level/v2/bg_corridor
if want("corridor_v2") then
  local CO = dofile(here .. "corridor_v2.lua")(L, ST)
  L.save(CO.background(), "Level/v2/bg_corridor", "Level/v2/bg_corridor")
end

-- The v2 bedroom (Normal 1): Level/v2/bg_bedroom and its moving parts, bedroom_bed, bedroom_box,
-- bedroom_box_placed, bedroom_picture and bedroom_books
if want("bedroom_v2") then
  local BR = dofile(here .. "bedroom_v2.lua")(L, ST)
  L.save(BR.background(), "Level/v2/bg_bedroom", "Level/v2/bg_bedroom")
  L.save(BR.bed(), "Level/v2/bedroom_bed", "Level/v2/bedroom_bed")
  L.save(BR.box(), "Level/v2/bedroom_box", "Level/v2/bedroom_box")
  L.save(BR.boxPlaced(), "Level/v2/bedroom_box_placed", "Level/v2/bedroom_box_placed")
  L.save(BR.picture(), "Level/v2/bedroom_picture", "Level/v2/bedroom_picture")
  L.save(BR.books(), "Level/v2/bedroom_books", "Level/v2/bedroom_books")
end

-- The shared v2 living room, its Act-out props and its swaying lamp.
if want("fire_smoke_motion") then
  local smoke=dofile(here .. "fire_smoke_v2.lua")(L)
  for _,set in ipairs({{"smoke_drift",smoke.band},{"street_roof_smoke",smoke.plume}}) do
    local frames={}
    for f=0,23 do frames[#frames+1]=set[2](f,24) end
    local path="Level/v2/"..set[1]
    L.saveFrames(frames,path,path,80)
  end
end

if want("street") or want("street_after") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local menu=dofile(here .. "background.lua")(L,ST)
  local grownups=dofile(here .. "grownups_v2.lua")(L,ST)
  local street=dofile(here .. "street_v2.lua")(L,kit,menu,grownups)
  local function still(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  if want("street") then
    still("bg_street",street.background(false))
    still("street_cat",street.cat())
    still("street_phone",street.phone())
    still("street_hiding_box",street.crates())
  end
  if want("street_after") then still("bg_street_after",street.background(true)) end
end

if want("icons_fire_street") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local selected={}
  for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
  for name,img in pairs(kit.fireStreetIcons()) do
    if next(selected)==nil or selected[name] then
      local path="UI/icon_"..name
      L.save(img,path,path)
    end
  end
end

if want("mall_fire") then
  local mall=dofile(here .. "mall_v2.lua")(L,ST)
  local sign="Level/v2/mall_exit_sign"
  local toy="Level/v2/mall_toy_shop_toy"
  L.save(mall.fireExitSign(),sign,sign)
  L.save(mall.shopToy(),toy,toy)
end

if want("icons_fire_mall") then
  local selected={}
  for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
  if next(selected)==nil or selected.fire_exit_stairs then
    local mall=dofile(here .. "mall_v2.lua")(L,ST)
    local path="UI/icon_fire_exit_stairs"
    L.save(mall.exitStairsIcon(),path,path)
  end
end

if only.evacuation_area_typhoon or only.yard_damage or only.icons_typhoon_after then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  if only.evacuation_area_typhoon then
    local adults=dofile(here .. "grownups_v2.lua")(L,ST)
    local court=dofile(here .. "evacuation_area_v2.lua")(L,ST,adults)
    local path="Level/v2/evacuation_area_leaving"
    L.saveFrames(court.leavingFrames(),path,path,140)
  end
  if only.yard_damage then
    local yard=dofile(here .. "yard_v2.lua")(L,kit)
    for name,img in pairs(yard.damage()) do
      local path="Level/v2/"..name
      L.save(img,path,path)
    end
  end
  if only.icons_typhoon_after then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(kit.typhoonAfterIcons()) do
      if not next(selected) or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if only.home_night_typhoon or only.home_typhoon_hard or only.icons_typhoon_home_hard then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local home=dofile(here .. "home_v2.lua")(L,ST)
  local night=dofile(here .. "home_night_v2.lua")(L,ST,home,kit)
  if only.home_night_typhoon then
    for name,img in pairs({shelf=night.typhoonShelf(),candle_unlit=night.unlitCandle(),matches=night.closedMatches()}) do
      local path="Level/v2/home_night_"..name
      L.save(img,path,path)
    end
  end
  -- Hard 4's Setting reuses the approved home window, bicycle and radio.
  -- Its two home-specific icons can also be rendered by the home part alone.
  if only.home_typhoon_hard or only.icons_typhoon_home_hard then
    local icons=only.icons_typhoon_home_hard and kit.typhoonHomeHardIcons(home,night) or home.typhoonHardIcons()
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(icons) do
      if not next(selected) or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if only.coast or only.icons_typhoon_coast then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  if only.coast then dofile(here .. "coast_v2.lua")(L,kit).save() end
  if only.icons_typhoon_coast then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(kit.typhoonCoastIcons()) do
      if not next(selected) or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if only.street_storm or only.icons_typhoon_street then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  if only.street_storm then dofile(here .. "street_storm_v2.lua")(L,kit).save() end
  if only.icons_typhoon_street then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(kit.typhoonStreetIcons()) do
      if not next(selected) or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if only.home_typhoon_normal or only.icons_typhoon_normal then
  local home=dofile(here .. "home_v2.lua")(L,ST)
  if only.home_typhoon_normal then
    local path="Level/v2/home_module"
    L.save(home.schoolModule(),path,path)
  end
  if only.icons_typhoon_normal then
    local kit=dofile(here .. "icons_v2.lua")(L,ST)
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(kit.typhoonNormalIcons(home)) do
      if not next(selected) or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if only.home_typhoon or only.yard_typhoon or only.icons_typhoon_easy then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local home=dofile(here .. "home_v2.lua")(L,ST)
  local yard=dofile(here .. "yard_v2.lua")(L,kit)
  local function save(name,img)
    L.save(img,name,name)
  end
  if only.home_typhoon then
    for n=1,2 do save("Level/v2/home_tv_signal_"..n,home.signalScreen(n)) end
  end
  local props=yard.typhoonProps()
  if only.yard_typhoon then
    for name,img in pairs(props) do save("Level/v2/"..name,img) end
    local tree,clothes=yard.swayFrames()
    for name,frames in pairs({yard_tree_sway=tree,yard_clothes=clothes}) do
      local path="Level/v2/"..name
      L.saveFrames(frames,path,path,200)
    end
  end
  if only.icons_typhoon_easy then
    local selected={}
    for name in (app.params.icons or ""):gmatch("[^,%s]+") do selected[name]=true end
    for name,img in pairs(kit.typhoonEasyIcons(props,home)) do
      if not next(selected) or selected[name] then save("UI/icon_"..name,img) end
    end
  end
end

if want("yard") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local yard=dofile(here .. "yard_v2.lua")(L,kit)
  local bg="Level/v2/bg_yard"
  L.save(yard.background(),bg,bg)
  local path="Level/v2/yard_siga"
  L.saveFrames({yard.siga(0),yard.siga(1),yard.siga(2),yard.siga(3)},path,path,160)
end

if want("icons_fire_yard") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local selected={}
  for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
  for name,img in pairs(kit.fireYardIcons()) do
    if next(selected)==nil or selected[name] then
      local path="UI/icon_"..name
      L.save(img,path,path)
    end
  end
end

if want("bedroom_fire") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local bedroom=dofile(here .. "bedroom_fire_v2.lua")(L,kit)
  for name,img in pairs({
    bedroom_closet_door=bedroom.closet(), bedroom_toy=bedroom.toy(),
    bedroom_blanket=bedroom.blanket(false), bedroom_blanket_gap=bedroom.blanket(true),
    bedroom_cloth=bedroom.cloth(), bedroom_window_open=bedroom.window(),
  }) do
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local path="Level/v2/bedroom_door"
  L.saveFrames({bedroom.door(0),bedroom.door(1)},path,path,300)
end

if want("icons_fire_bedroom") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local selected={}
  for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
  for name,img in pairs(kit.fireBedroomIcons()) do
    if next(selected)==nil or selected[name] then
      local path="UI/icon_"..name
      L.save(img,path,path)
    end
  end
end

if want("fire_normal_props") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local props=dofile(here .. "fire_normal_v2.lua")(L,kit)
  for name,img in pairs({
    classroom_bag=props.bag(), corridor_handkerchief=props.cloth(),
    corridor_cr_door=props.door(), home_switch_box=props.switchBox(),
  }) do
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local path="Level/v2/home_outlet"
  L.saveFrames({props.outlet(0),props.outlet(1),props.outlet(2)},path,path,150)
end

if want("icons_fire_normal") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local selected={}
  for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
  for name,img in pairs(kit.fireNormalIcons()) do
    if next(selected)==nil or selected[name] then
      local path="UI/icon_"..name
      L.save(img,path,path)
    end
  end
end

-- Fire Easy Settings, props and Choice icons. Each part can render independently.
if want("home_night") or want("kitchen") or want("icons_fire_easy") then
  local kit=dofile(here .. "icons_v2.lua")(L,ST)
  local function still(name,img)
    local path="Level/v2/"..name
    L.save(img,path,path)
  end
  local function pair(name,draw,ms)
    local path="Level/v2/"..name
    L.saveFrames({draw(0),draw(1)},path,path,ms)
  end
  if want("home_night") then
    local home=dofile(here .. "home_v2.lua")(L,ST)
    local night=dofile(here .. "home_night_v2.lua")(L,ST,home,kit)
    still("bg_home_night",night.background())
    still("home_lighter",night.lighter())
    still("home_night_plate",night.plate())
    still("home_night_flashlight",night.flashlight())
    pair("home_night_candle",night.candle,240)
    pair("home_night_curtain",night.curtain,420)
  end
  if want("kitchen") then
    local kitchen=dofile(here .. "kitchen_v2.lua")(L,kit)
    still("bg_kitchen",kitchen.background())
    still("kitchen_pan",kitchen.pan())
    still("kitchen_pan_lid",kitchen.lid())
    still("kitchen_cup",kitchen.cup())
    pair("kitchen_pot",kitchen.pot,260)
    pair("kitchen_burner",kitchen.burner,180)
  end
  if want("icons_fire_easy") then
    local selected={}
    for key in (app.params.icons or ""):gmatch("[^,%s]+") do selected[key]=true end
    for name,img in pairs(kit.fireEasyIcons()) do
      if next(selected)==nil or selected[name] then
        local path="UI/icon_"..name
        L.save(img,path,path)
      end
    end
  end
end

if want("home_v2") then
  local HM = dofile(here .. "home_v2.lua")(L, ST)
  local function homeSprite(draw, name)
    local path = "Level/v2/" .. name
    L.save(draw(), path, path)
  end
  homeSprite(HM.background, "bg_home")
  homeSprite(HM.tableFront, "table_front")
  for _, prop in ipairs({
    { HM.bagOpen, "bag_open" }, { HM.bagClosed, "bag_closed" },
    { HM.water, "water" }, { HM.can, "can" }, { HM.flashlight, "flashlight" },
    { HM.whistle, "whistle" }, { HM.snack, "snack" }, { HM.toy, "toy" },
    { HM.parkCard, "park_card" }, { HM.houseCard, "house_card" },
  }) do
    homeSprite(prop[1], "home_" .. prop[2])
  end
  L.saveFrames(HM.lampFrames(), "Level/v2/home_lamp", "Level/v2/home_lamp", 160)
end

-- The same v2 living room after the earthquake, with the written family note and pen.
if want("home_after_v2") then
  local HM = dofile(here .. "home_v2.lua")(L, ST)
  local base = "Level/v2/"
  L.save(HM.backgroundAfter(), base .. "bg_home_after", base .. "bg_home_after")
  for _, prop in ipairs({
    { "pen", HM.pen() }, { "note", HM.note() },
  }) do
    local path = base .. "home_after_" .. prop[1]
    L.save(prop[2], path, path)
  end
end

-- The shared v2 schoolyard and its shaking-only canopy and post.
if want("schoolyard_v2") then
  local SY = dofile(here .. "schoolyard_v2.lua")(L, ST)
  local base = "Level/v2/"
  L.save(SY.background(), base .. "bg_schoolyard", base .. "bg_schoolyard")
  L.saveFrames(SY.canopyFrames(), base .. "schoolyard_canopy", base .. "schoolyard_canopy", SY.LAYOUT.swayMs)
  L.saveFrames(SY.postFrames(), base .. "schoolyard_post", base .. "schoolyard_post", SY.LAYOUT.swayMs)
end

-- The v2 beach, with a separate water line animation.
if want("beach_v2") then
  local BE = dofile(here .. "beach_v2.lua")(L, ST)
  local base = "Level/v2/"
  L.save(BE.background(), base .. "bg_beach", base .. "bg_beach")
  L.saveFrames(BE.waterFrames(), base .. "beach_water", base .. "beach_water", BE.LAYOUT.waterMs)
end

-- The two v2 mall Settings, with a table overlay and a shaking-only sign.
if want("mall_v2") then
  local MA = dofile(here .. "mall_v2.lua")(L, ST)
  local base = "Level/v2/"
  L.save(MA.backgroundA(), base .. "bg_mall_a", base .. "bg_mall_a")
  L.save(MA.backgroundB(), base .. "bg_mall_b", base .. "bg_mall_b")
  L.save(MA.foodtableFront(), base .. "foodtable_front", base .. "foodtable_front")
  L.saveFrames(MA.mallSignFrames(), base .. "mall_foodcourt_sign", base .. "mall_foodcourt_sign",
    MA.LAYOUT.mall_a.swayMs)
end

-- The v2 evacuation area (Hard 7): the covered court, the radio prop (off and two "on" frames with
-- sound waves) and the chatting neighbours (two frames).
if want("evacuation_area_v2") then
  local GU = dofile(here .. "grownups_v2.lua")(L, ST)
  local EA = dofile(here .. "evacuation_area_v2.lua")(L, ST, GU)
  local base = "Level/v2/"
  L.save(EA.background(), base .. "bg_evacuation_area", base .. "bg_evacuation_area")
  L.save(EA.radioOff(), base .. "evacuation_area_radio", base .. "evacuation_area_radio")
  L.saveFrames(EA.radioOnFrames(), base .. "evacuation_area_radio_on", base .. "evacuation_area_radio_on",
    EA.LAYOUT.radioMs)
  L.saveFrames(EA.neighboursFrames(), base .. "evacuation_area_neighbours", base .. "evacuation_area_neighbours",
    EA.LAYOUT.neighboursMs)
end

-- The Home-specific reach-up frames let the Student pinch a card at the board.
if want("home_reachup") then
  for _, kind in ipairs({ "boy", "girl" }) do
    local path = "Student/" .. kind .. "_home_reachup"
    L.saveFrames(ST.homeReachUpFrames(kind), path, path, 300)
  end
end

-- The Grown-ups (36x62): GrownUp/<who>_<pose>_<n>.png
if only.grownups_typhoon then
  local grownups = dofile(here .. "grownups_v2.lua")(L, ST)
  local home = dofile(here .. "home_v2.lua")(L, ST)
  grownups.saveTyphoonPoses(posesOnly, home.bagClosed())
end
if want("grownups_fire") then
  local grownups = dofile(here .. "grownups_v2.lua")(L, ST)
  grownups.saveFirePoses(posesOnly)
end
if want("grownups_v2") then
  local GU = dofile(here .. "grownups_v2.lua")(L, ST)
  for _, who in ipairs({ "teacher", "mom", "dad" }) do
    for _, pose in ipairs(GU.CHARACTERS[who]) do
      local ms = 300
      for _, p in ipairs(GU.POSES) do if p.name == pose then ms = p.ms end end
      local name = "GrownUp/" .. who .. "_" .. pose
      L.saveFrames(GU.frames(who, pose), name, name, ms)
    end
  end
end

print("Built " .. #L.made .. " images")
