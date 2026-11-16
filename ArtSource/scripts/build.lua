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
  L.save(UI.button(set, true), "UI/button_" .. name .. "_pressed", "UI/button_" .. name .. "_pressed")
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

print("Built " .. #L.made .. " images")
