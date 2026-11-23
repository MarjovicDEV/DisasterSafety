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
local TU = dofile(here .. "tutorial.lua")(L, CH, UI)
local LV = dofile(here .. "level.lua")(L, CH)

-- Level scene: classroom, wobbling fan and cabinet, and one picture per Choice
L.save(LV.classroom(), "Level/bg_classroom", "Level/bg_classroom")
L.saveFrames(LV.fanFrames(), "Level/fan", "Level/fan", 120)
L.saveFrames(LV.cabinetFrames(), "Level/cabinet", "Level/cabinet", 120)
for name, img in pairs(LV.choicePictures()) do
  L.save(img, "Level/" .. name, "Level/" .. name)
end
-- Easy / 2 corridor (hanging pots sway) and Easy / 3 classroom after the shaking (teacher waves)
L.save(LV.corridor(), "Level/bg_corridor", "Level/bg_corridor")
L.saveFrames(LV.potFrames(), "Level/pots", "Level/pots", 160)
L.save(LV.classroomAfter(), "Level/bg_classroom_after", "Level/bg_classroom_after")
L.saveFrames(LV.teacherFrames(), "Level/teacher", "Level/teacher", 300)
for _, set in ipairs({ LV.choicePictures2(), LV.choicePictures3() }) do
  for name, img in pairs(set) do
    L.save(img, "Level/" .. name, "Level/" .. name)
  end
end

-- Earthquake / Normal: bedroom, home (hanging lamp, glass cabinet), schoolyard (tree, post),
-- beach (wave line), and one picture per Choice
local NM = dofile(here .. "normal.lua")(L, CH, LV)
L.save(NM.bedroom(), "Level/bg_bedroom", "Level/bg_bedroom")
L.save(NM.home(), "Level/bg_home", "Level/bg_home")
L.saveFrames(NM.lampFrames(), "Level/lamp", "Level/lamp", 160)
L.saveFrames(NM.glassCabinetFrames(), "Level/glass_cabinet", "Level/glass_cabinet", 120)
L.save(NM.schoolyard(), "Level/bg_schoolyard", "Level/bg_schoolyard")
L.saveFrames(NM.treeFrames(), "Level/tree", "Level/tree", 200)
L.saveFrames(NM.postFrames(), "Level/post", "Level/post", 160)
L.save(NM.beach(), "Level/bg_beach", "Level/bg_beach")
L.saveFrames(NM.waveFrames(), "Level/waves", "Level/waves", 400)
for name, img in pairs(NM.choicePicturesNormal()) do
  L.save(img, "Level/" .. name, "Level/" .. name)
end

-- Earthquake / Hard: mall_a (hanging sign, glass display), mall_b, home_after,
-- evacuation_area, and one picture per Choice
local HD = dofile(here .. "hard.lua")(L, CH, LV, NM)
L.save(HD.mallA(), "Level/bg_mall_a", "Level/bg_mall_a")
L.saveFrames(HD.mallSignFrames(), "Level/mall_sign", "Level/mall_sign", 160)
L.saveFrames(HD.shopDisplayFrames(), "Level/shop_display", "Level/shop_display", 120)
L.save(HD.mallB(), "Level/bg_mall_b", "Level/bg_mall_b")
L.save(HD.homeAfter(), "Level/bg_home_after", "Level/bg_home_after")
L.save(HD.evacuationArea(), "Level/bg_evacuation_area", "Level/bg_evacuation_area")
for name, img in pairs(HD.choicePicturesHard()) do
  L.save(img, "Level/" .. name, "Level/" .. name)
end

-- How to Play Tutorial Pages: Tutorial/<name>_0.png and _1.png
for _, page in ipairs(TU.pages) do
  L.saveFrames({ page.draw(0), page.draw(1) }, "Tutorial/" .. page.name, "Tutorial/" .. page.name, 400)
end

-- Background and ambient animation
L.save(BG.background(), "Background/bg_courtyard", "Background/bg_courtyard")
L.saveFrames(BG.flagFrames(), "Background/flag", "Background/flag", 140)
local clouds = BG.clouds()
L.save(clouds.big, "Background/cloud_big", "Background/cloud_big")
L.save(clouds.small, "Background/cloud_small", "Background/cloud_small")
L.saveFrames(BG.birdFrames(), "Background/bird", "Background/bird", 160)

-- Characters
L.saveFrames(CH.studentFrames(false), "Characters/student_boy", "Characters/student_boy", 110)
L.saveFrames(CH.studentFrames(true), "Characters/student_girl", "Characters/student_girl", 110)
-- Bantay frames: 0 idle, 1 wag, 2 blink, 3 talk
L.saveFrames({ CH.bantay("idle"), CH.bantay("wag"), CH.bantay("blink"), CH.bantay("talk") },
  "Characters/bantay", "Characters/bantay", 200)

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
L.save(UI.bubbleTail(), "UI/bubble_tail", "UI/bubble_tail")
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

-- The Student, boy and girl: Student/<kind>_<pose>_<n>.png and Student/<kind>_portrait.png
-- Optional: --script-param poses=carry,reach writes only those poses (and not the portrait), so
-- new poses can be drawn without rewriting the committed ones.
local posesOnly = {}
for p in (app.params.poses or ""):gmatch("[^,%s]+") do posesOnly[p] = true end
local allPoses = next(posesOnly) == nil
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

-- The v2 classroom after the shaking (Easy 3): its own background and desk-front overlay.
if want("classroom_after_v2") then
  local IC = dofile(here .. "icons_v2.lua")(L, ST)
  local CA = dofile(here .. "classroom_after_v2.lua")(L, ST, CR, IC)
  L.save(CA.background(), "Level/v2/bg_classroom_after", "Level/v2/bg_classroom_after")
  L.save(CA.deskFront(), "Level/v2/classroom_after_desk_front", "Level/v2/classroom_after_desk_front")
end

-- v2 UI: the 4 Choice icons and the Caption, Action Bar button and speech bubble 9-slices
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

-- The v2 corridor (Easy 2): Level/v2/bg_corridor and the hanging pots, corridor_pots_0/1
if want("corridor_v2") then
  local CO = dofile(here .. "corridor_v2.lua")(L, ST)
  L.save(CO.background(), "Level/v2/bg_corridor", "Level/v2/bg_corridor")
  L.saveFrames(CO.potFrames(), "Level/v2/corridor_pots", "Level/v2/corridor_pots", 160)
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

-- The Home-specific reach-up frames let the Student pinch a card at the board.
if want("home_reachup") then
  for _, kind in ipairs({ "boy", "girl" }) do
    local path = "Student/" .. kind .. "_home_reachup"
    L.saveFrames(ST.homeReachUpFrames(kind), path, path, 300)
  end
end

-- The Grown-ups (36x62): GrownUp/<who>_<pose>_<n>.png
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
