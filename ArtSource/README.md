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
| `scripts/font.lua` | 5x7 pixel font with lower case, digits, punctuation, ñ and Ñ |
| `scripts/lib.lua` | Palette and drawing helpers |
| `scripts/build.lua` | Runs everything and saves the results |

Rebuild from the project root:

```
"C:\Users\Marjovic\Desktop\aseprite\build\bin\aseprite.exe" -b --script-param root=D:/GameDEV/DisasterSafety --script ArtSource/scripts/build.lua
```

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

The Level art is gentle on purpose: a "during" scene shakes by at most 2 art pixels, and nothing falls (see `docs/design/first-playable-level.md` and `docs/design/earthquake-easy-2-3.md`). Sprite placements in art pixels from the top left of the 480x270 scene: fan (204, 0), cabinet (364, 64), pots (116, 12) and (236, 12), teacher (418, 126). Choice pictures are matched to Scenario files by name (`"picture": "choice_desk"` uses `Assets/Art/Level/choice_desk.png`).
