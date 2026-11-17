# 01 v2 art: the Student, the v2 classroom, icons and UI pieces

Blocked by: none. Blocks: 02, 03.

Spec: `docs/specs/gameplay-redesign-pilot.md` (the Art section). Design: `docs/design/gameplay-redesign.md`. Style: `docs/adr/0001-v2-art-and-no-bantay.md`. Rules: `AGENTS.md`.

## Build

- `ArtSource/scripts/student.lua`: port the v2 half-map method from `D:\GameDEV\DisasterSafety - Source\StyleSample\student2.py` (mirrored half maps, the same 20-colour palette, light from the left, the right outer edge in shadow). Draw every Student pose in the spec's art table for the boy and the girl: idle, surprised, walk (side view, 4 frames), duck (Duck, Cover, and Hold, front), wait ("Hmm, wait..."), happy and portrait. The side view and the duck pose cannot be mirrored halves; draw them as full maps in the same style.
- `ArtSource/scripts/classroom_v2.lua`: the classroom in v2 style at 480x270 for a 50-pixel Student. Floor line (feet) at y = 230. Door on one side, the desk near the middle (about 40 pixels tall, with a separate `desk_front` overlay for the part drawn over the ducking Student), windows and the tall cabinet on the other side, the ceiling fan. The fan and cabinet get 2 wobble frames each.
- The 4 icons (16x16) and 3 UI 9-slices (`caption_banner`, `action_button`, `speech_bubble`) in the v2 palette; add the 9-slice borders to `PixelArtImporter.cs`.
- Wire both scripts into `build.lua`, saving `.aseprite` sources under `ArtSource/Student/`, `ArtSource/Level/v2/`, `ArtSource/UI/` and PNGs under the paths in the spec.
- `ArtSource/README.md`: add both scripts to the table and record every placement, the start spot and the 4 action spots (door, desk, window, cabinet) with their x in art pixels.

## Check

- Render a preview sheet of all poses (both Students) and the classroom with the Student composited at the start spot and ducking at the desk, enlarged; look at it and fix what reads badly. Send it to the user.
- No file under `Assets/Art` that exists on `main` changes (`git status` shows only new files and the importer).

## Done when

All files in the spec's art table exist, the preview reads clearly (a Grade 3 learner can tell idle, walk, duck, wait and happy apart), the README lists the spots, and the art is committed in one commit with a one-sentence message and no co-author line.
