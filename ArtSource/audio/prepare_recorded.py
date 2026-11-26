"""Prepares the recorded (human-made) sounds for the game from ArtSource/audio/recorded/.

The menu, button, Star and "Normal is open!" sounds are Kenney's recorded "UI Audio" clicks
and switches, and the Level Completed music is a real ukulele from Freesound. Every
source is CC0; see ArtSource/audio/recorded/SOURCES.md and Assets/Audio/Music/LICENSES.md.

Needs ffmpeg on PATH. Run from anywhere:
    python -I ArtSource/audio/prepare_recorded.py D:/GameDEV/DisasterSafety

It overwrites the listed Assets/Audio/Sfx/sfx_*.wav files and writes
Assets/Audio/Music/Ukulele Short Theme.wav. make_audio.py no longer writes these sounds.
"""
import os
import re
import subprocess
import sys

KENNEY = "kenney-ui-audio"
FREESOUND = "freesound"
UKULELE = os.path.join(FREESOUND, "537253_Tetoszka_ukulele-short-theme-hq.mp3")

# Game sound: (source, start seconds or None, end seconds or None, peak dBFS).
# Soft, low clicks for the common taps; the brighter ones only where a sound should stand out.
SFX = {
    "sfx_click": (os.path.join(KENNEY, "click2.ogg"), None, None, -6.0),
    "sfx_type": (os.path.join(KENNEY, "click5.ogg"), None, None, -9.0),
    "sfx_back": (os.path.join(KENNEY, "rollover2.ogg"), None, None, -6.0),
    "sfx_open": (os.path.join(KENNEY, "rollover5.ogg"), None, None, -6.0),
    "sfx_confirm": (os.path.join(KENNEY, "rollover6.ogg"), None, None, -5.0),
    "sfx_locked": (os.path.join(KENNEY, "switch21.ogg"), None, None, -6.0),
    "sfx_toggle": (os.path.join(KENNEY, "switch28.ogg"), None, None, -6.0),
    "sfx_sparkle": (os.path.join(KENNEY, "rollover3.ogg"), None, None, -8.0),
    "sfx_soon": (os.path.join(KENNEY, "rollover1.ogg"), None, None, -7.0),
    # One tick per Star; the game raises its pitch for each Star.
    "sfx_star": (os.path.join(KENNEY, "click1.ogg"), None, None, -6.0),
    # "Normal is open!": a soft recorded switch. Level Completed itself is only the ukulele music.
    "sfx_opened": (os.path.join(KENNEY, "switch22.ogg"), None, None, -6.0),
}

# The Level Completed music: both ukulele phrases, with a soft ending, matched to the other music.
UKULELE_CUT = (0.40, 6.00)
MUSIC_LUFS = -20.0


def run(args):
    return subprocess.run(args, check=True, capture_output=True, text=True)


def peak_db(path, start, end):
    args = ["ffmpeg", "-hide_banner", "-i", path]
    if start is not None:
        args += ["-ss", str(start), "-to", str(end)]
    args += ["-af", "astats=metadata=0", "-f", "null", "-"]
    out = subprocess.run(args, capture_output=True, text=True).stderr
    overall = out[out.rfind("Overall"):]
    return float(re.search(r"Peak level dB:\s*(-?[\d.]+)", overall).group(1))


def loudness(path, filters):
    out = subprocess.run(["ffmpeg", "-hide_banner", "-i", path, "-af", filters + ",ebur128", "-f", "null", "-"],
                         capture_output=True, text=True).stderr
    return float(re.findall(r"I:\s*(-?[\d.]+) LUFS", out)[-1])


def prepare_sfx(src_root, out_dir):
    for name, (rel, start, end, target) in SFX.items():
        src = os.path.join(src_root, rel)
        gain = target - peak_db(src, start, end)
        chain = []
        if start is not None:
            length = end - start
            chain.append("afade=t=in:d=0.01,afade=t=out:st={:.3f}:d=0.08".format(length - 0.08))
        chain.append("volume={:.2f}dB".format(gain))
        args = ["ffmpeg", "-hide_banner", "-y"]
        if start is not None:
            args += ["-ss", str(start), "-to", str(end)]
        args += ["-i", src, "-af", ",".join(chain), "-ac", "1", "-ar", "44100", "-c:a", "pcm_s16le",
                 os.path.join(out_dir, name + ".wav")]
        run(args)
        print("{:<14} from {} ({:+.1f} dB)".format(name, rel, gain))


def prepare_music(src_root, out_dir):
    src = os.path.join(src_root, UKULELE)
    start, end = UKULELE_CUT
    length = end - start
    shape = "atrim={}:{},asetpts=PTS-STARTPTS,afade=t=in:d=0.012,afade=t=out:st={:.3f}:d=0.6".format(
        start, end, length - 0.6)
    gain = MUSIC_LUFS - loudness(src, shape)
    out = os.path.join(out_dir, "Ukulele Short Theme.wav")
    run(["ffmpeg", "-hide_banner", "-y", "-i", src, "-af", shape + ",volume={:.2f}dB,alimiter=limit=0.95".format(gain),
         "-ac", "2", "-ar", "44100", "-c:a", "pcm_s16le", out])
    print("Ukulele Short Theme.wav, {:.2f} s ({:+.1f} dB)".format(length, gain))


def main():
    root = sys.argv[1] if len(sys.argv) > 1 else "."
    src_root = os.path.join(root, "ArtSource", "audio", "recorded")
    prepare_sfx(src_root, os.path.join(root, "Assets", "Audio", "Sfx"))
    prepare_music(src_root, os.path.join(root, "Assets", "Audio", "Music"))


if __name__ == "__main__":
    main()
