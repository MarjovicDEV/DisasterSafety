# ArtSource

Editable sources for every sprite and sound in the game. Nothing here is loaded by Unity directly; the build steps below write the game-ready files into `Assets/`.

## Art (Aseprite)

All sprites are drawn by Lua scripts that run in Aseprite 1.3:

| Script | Draws |
|---|---|
| `scripts/background.lua` | School courtyard (480x270), Philippine flag (4 frames), clouds, birds |
| `scripts/characters.lua` | Walking boy and girl students (6 frames each), Bantay the guide dog (idle, wag, blink, talk) |
| `scripts/ui.lua` | 9-slice buttons and panels, icons (including pause, hint, replay, levels), Episode emblems, title logo, particles, font atlas |
| `scripts/tutorial.lua` | How to Play Tutorial Page pictures (2 frames each) |
| `scripts/level.lua` | Level scene, one art set per Setting (480x270 backgrounds): `classroom` with the ceiling fan and tall cabinet (2 wobble frames each); `corridor` with jalousie windows, a bulletin board, stairs and a hanging flower pot (`pots`, 2 sway frames, placed twice); `classroom_after`, a still classroom with the door open and the teacher (`teacher`, 2 waving frames). Also one 72x52 picture per Choice |
| `scripts/normal.lua` | Earthquake / Normal Settings (480x270 backgrounds): `bedroom`; `home` with the hanging lamp and glass cabinet (2 wobble frames each); `schoolyard` with the big tree and electric post (2 wobble frames each); `beach` with a wave line (2 frames). Also one 72x52 picture per Normal Choice. It reuses the helpers `level.lua` exposes as `M.helpers` |
| `scripts/hard.lua` | Earthquake / Hard Settings (480x270 backgrounds): `mall_a`, a mall food court with a hanging shop sign (`mall_sign`) and a glass display case (`shop_display`), 2 wobble frames each; `mall_b`, outside a mall with the outdoor stair and the open parking area; `home_after`, the Normal living room after the shaking; `evacuation_area`, an open field with families. Also one 72x52 picture per Hard Choice. The malls carry made-up signs, no real logos or names. It reuses the helpers of `level.lua` and `normal.lua` |
| `scripts/student.lua` | The v2 Student, boy and girl, 32x50 frames in the style of `StyleSample/student2.py` (ADR 0001): mirrored half maps for the front views, the same 20-colour palette; walk, duck and wait are hand-placed maps, the raised arms of surprised and happy are shaded, outlined limbs. Poses: `idle` (2), `surprised` (2), `walk` (4, side view facing right; the game mirrors it for left), `duck` (2, Duck, Cover, and Hold; no longer used in the Level), `crouch` (2, side view facing right: knees bent, back hunched, shuffling in under the desk), `hold` (2, front view under the desk: the left hand holds the desk leg, the right arm covers the head; EQ-11), `wait` (2), `happy` (2), and `portrait` (1) |
| `scripts/classroom_v2.lua` | The v2 classroom (480x270) for the 50-pixel Student: door, bulletin board and flag, blackboard, desk with `desk_front`, jalousie windows, tall cabinet; the ceiling fan and the cabinet have 2 wobble frames each |
| `scripts/ui_v2.lua` | v2 UI: the Choice icons `icon_door`, `icon_desk`, `icon_window`, `icon_cabinet` (16x16) and the 9-slices `caption_banner` (32x24, border 8), `action_button` (32x26, border 9, with a 3-pixel bottom lip) and `speech_bubble` (24x24, border 8) |
| `scripts/font.lua` | 5x7 pixel font with lower case, digits, punctuation, ñ and Ñ |
| `scripts/lib.lua` | Palette and drawing helpers |
| `scripts/build.lua` | Runs everything and saves the results |

Rebuild from the project root:

```
"C:\Users\Marjovic\Desktop\aseprite\build\bin\aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script ArtSource/scripts/build.lua
```

To build only some parts, add `--script-param only=<parts>` (comma separated): `v1` (every script from before the v2 art), `student`, `classroom_v2`, `ui_v2`. The v2 art was made with `only=student,classroom_v2,ui_v2`, so the committed v1 files were not rewritten.

The build writes:

- `ArtSource/<group>/<name>.aseprite`: sources you can open and paint over in Aseprite.
- `Assets/Art/<group>/<name>.png`: the files Unity uses. Animations are saved as `<name>_0.png`, `<name>_1.png`, and so on.
- `Assets/Art/Font/pixel_font.png` and `pixel_font.txt`: the font atlas and glyph metrics.

Hand edits: paint over the `.aseprite` file, then export it to the matching PNG at the same size. Rerunning `build.lua` overwrites both files, so copy a hand-painted file elsewhere first or stop using the script for that asset.

Scale rule: one art pixel is four UI units at 1920x1080. Unity imports everything under `Assets/Art` at 25 pixels per unit with point filtering (`Assets/Scripts/Editor/PixelArtImporter.cs`). The 9-slice borders in that file must match the sizes drawn in `ui.lua`.

## Audio (Python)

`audio/make_audio.py` synthesises the menu music loop and the UI sound effects with the Python standard library only, so there are no third-party licences:

```
python -I ArtSource/audio/make_audio.py D:/GameDEV/DisasterSafety
```

It writes `Assets/Audio/Music/bgm_menu.wav` and `Assets/Audio/Sfx/*.wav`. The Level sounds are `sfx_rumble` (a soft, low earthquake rumble, kept quiet on purpose), `sfx_safe`, `sfx_unsafe` (a kind two-note "hmm", never a buzzer), `sfx_star` (the game raises its pitch for each Star), `sfx_complete`, and `sfx_opened` (a soft rising jingle for "Normal is open!", quieter and shorter than `sfx_complete`).

## After rebuilding

In Unity, run **Disaster Safety > Build Everything**. It reimports the art and rebuilds the pixel fonts, the audio library, the Boot, MainMenu and Level scenes, and the Build Settings scene list. Rebuilding replaces every scene, so change layouts in `Assets/Scripts/Editor/ProjectBuilder.cs` and `ProjectBuilder.Level.cs` rather than by hand.

The Level art is gentle on purpose: a "during" scene shakes by at most 2 art pixels, and nothing falls (see `docs/design/first-playable-level.md` and `docs/design/earthquake-easy-2-3.md`). Sprite placements in art pixels from the top left of the 480x270 scene: fan (204, 0), cabinet (364, 64), pots (116, 12) and (236, 12), teacher (418, 126), lamp (232, 0), glass cabinet (330, 64), tree (260, 34), post (360, 14), waves (140, 128), mall_sign (380, 0), shop_display (66, 112). Choice pictures are matched to Scenario files by name (`"picture": "choice_desk"` uses `Assets/Art/Level/choice_desk.png`).

## v2 classroom and the Student (ticket 01 of the gameplay redesign)

Files: `Assets/Art/Student/<kind>_<pose>_<n>.png` and `<kind>_portrait.png` (kind `boy` or `girl`), `Assets/Art/Level/v2/bg_classroom.png`, `desk_front.png`, `fan_0/1.png`, `cabinet_0/1.png`, and the `UI/` pieces above.

Placements in art pixels from the top left of the 480x270 v2 classroom: fan (204, 0), cabinet (424, 100), desk_front (198, 188).

The floor line is y = 230. Every Student frame is 32x50 and bottom aligned: frame row 49 is the dark outline under the shoes, so the feet stand on the floor when the frame's top-left corner is at (x - 16, 180), where x is the spot below. This holds for every pose, `duck` included: its figure fills only the lowest 25 (boy) or 26 (girl) rows, with the effort lines and sweat drop just above, so all of it fits under the desk top at y 188-194. The `walk` frames face right; flip them for walking left.

Spots (the Student's centre x, art pixels):

| Spot | x |
|---|---|
| start | 126 |
| door | 46 |
| desk | walk to 184, then `crouch` and shuffle to 222 and `hold` (the reaching hand meets the left front leg at x 200-206); draw `desk_front` over the Student from the crouch on |
| window | 372 |
| cabinet | 440 |

