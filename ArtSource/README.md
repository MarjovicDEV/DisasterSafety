# ArtSource

Editable sources for every sprite and sound in the game. Nothing here is loaded by Unity directly; the build steps below write the game-ready files into `Assets/`.

## Art (Aseprite)

All sprites are drawn by Lua scripts that run in Aseprite 1.3:

| Script | Draws |
|---|---|
| `scripts/background.lua` | School courtyard (480x270), Philippine flag (4 frames), clouds, birds |
| `scripts/characters.lua` | Walking boy and girl students for the menu courtyard (6 frames each) |
| `scripts/ui.lua` | 9-slice buttons and panels, icons (including pause, hint, replay, levels), Episode emblems, title logo, particles, font atlas |
| `scripts/tutorial.lua` | How to Play Tutorial Page pictures `topic`, `difficulty` and `stars` (2 frames each) |
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

To build only some parts, add `--script-param only=<parts>` (comma separated): `v1` (the menu background, the walking students, the UI kit, the font and the pilot Tutorial Pages), `student`, `classroom_v2`, `ui_v2`. The v2 art was made with `only=student,classroom_v2,ui_v2`, so the committed v1 files were not rewritten.

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

It writes `Assets/Audio/Music/bgm_menu.wav` (the menus), `Assets/Audio/Music/bgm_level.wav` (the Level: calmer, 88 BPM, no drums) and `Assets/Audio/Sfx/*.wav`. Add `--only-level-music` to write just `bgm_level.wav`. The Level sounds are `sfx_rumble` (a soft, low earthquake rumble, kept quiet on purpose), `sfx_safe`, `sfx_unsafe` (a kind two-note "hmm", never a buzzer), `sfx_star` (the game raises its pitch for each Star), `sfx_complete`, and `sfx_opened` (a soft rising jingle for "Normal is open!", quieter and shorter than `sfx_complete`).

## After rebuilding

In Unity, run **Disaster Safety > Build Everything**. It reimports the art and rebuilds the pixel fonts, the audio library, the Boot, MainMenu and Level scenes, and the Build Settings scene list. Rebuilding replaces every scene, so change layouts in `Assets/Scripts/Editor/ProjectBuilder.cs` and `ProjectBuilder.Level.cs` rather than by hand.

The Level art is gentle on purpose: a "during" scene shakes by at most 2 art pixels, and nothing falls (see `docs/design/first-playable-level.md` and `docs/design/earthquake-easy-2-3.md`). Every Level uses the v2 Settings under `Assets/Art/Level/v2/`. The v1 Setting backgrounds, their moving parts and the Choice pictures were removed in the Earthquake rollout cleanup (ticket 15).

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

