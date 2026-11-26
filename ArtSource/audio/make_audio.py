"""Generate the synthesized Level sound effects for Disaster Safety: the rumble, the Safe and Unsafe
Choice sounds and the Student's voice blip.

Run from the project root:
    python -I ArtSource/audio/make_audio.py D:/GameDEV/DisasterSafety
Add --only-blip to write just Assets/Audio/Sfx/sfx_blip.wav.

This script writes 16-bit mono WAV files to Assets/Audio/Sfx using the standard library.
The button, menu, Star and Level Completed sounds are recorded sounds, prepared by
prepare_recorded.py. The licensed soundtrack is prepared separately. See
Assets/Audio/Music/LICENSES.md.
"""
import math
import os
import random
import struct
import sys
import wave

RATE = 22050


def write_wav(path, samples):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    peak = max(1e-6, max(abs(s) for s in samples))
    gain = 0.89 / peak if peak > 0.89 else 1.0
    with wave.open(path, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        w.writeframes(b"".join(struct.pack("<h", int(max(-1, min(1, s * gain)) * 32767)) for s in samples))


def note_freq(name):
    """'C4', 'F#3', 'Bb4' -> Hz. 'R' is a rest."""
    if name == "R":
        return 0.0
    names = {"C": 0, "D": 2, "E": 4, "F": 5, "G": 7, "A": 9, "B": 11}
    n = names[name[0]]
    i = 1
    if name[i] == "#":
        n += 1
        i += 1
    elif name[i] == "b":
        n -= 1
        i += 1
    octave = int(name[i:])
    midi = 12 * (octave + 1) + n
    return 440.0 * 2 ** ((midi - 69) / 12)


def square(phase, duty=0.5):
    return 1.0 if (phase % 1.0) < duty else -1.0


def triangle(phase):
    p = phase % 1.0
    return 4 * p - 1 if p < 0.5 else 3 - 4 * p


def envelope(i, n, attack=0.005, release=0.06):
    t = i / RATE
    a = min(1.0, t / attack) if attack > 0 else 1.0
    tail = (n - i) / RATE
    r = min(1.0, tail / release) if release > 0 else 1.0
    return a * r


def mix(*tracks):
    n = max(len(t) for t in tracks)
    return [sum(t[i] if i < len(t) else 0.0 for t in tracks) for i in range(n)]


def sweep(f0, f1, dur, wave_fn=square, vol=0.5, duty=0.5, decay=None):
    n = int(dur * RATE)
    out = []
    phase = 0.0
    for i in range(n):
        t = i / n
        f = f0 * (f1 / f0) ** t
        phase += f / RATE
        v = wave_fn(phase, duty) if wave_fn is square else wave_fn(phase)
        e = math.exp(-i / (RATE * decay)) if decay else envelope(i, n, 0.002, dur * 0.4)
        out.append(v * vol * e)
    return out


def seq(*parts):
    out = []
    for p in parts:
        out += p
    return out


def silence(dur):
    return [0.0] * int(dur * RATE)


def make_sfx():
    s = {}
    # The button and menu sounds are recorded sounds now (prepare_recorded.py).

    # Level sounds -------------------------------------------------------
    s["sfx_rumble"] = make_rumble()
    # Safe Choice: a bright rising arpeggio
    s["sfx_safe"] = mix(
        seq(*[sweep(note_freq(n), note_freq(n), 0.08, square, 0.22, 0.25) for n in ["C5", "E5", "G5"]],
            sweep(note_freq("C6"), note_freq("C6"), 0.3, square, 0.22, 0.25, decay=0.12)),
        seq(silence(0.04), *[sweep(note_freq(n), note_freq(n), 0.08, triangle, 0.18) for n in ["E5", "G5", "C6"]],
            sweep(note_freq("E6"), note_freq("E6"), 0.3, triangle, 0.18, decay=0.12)))
    # Unsafe Choice: a soft, kind "hmm" (two low triangle notes, never a buzzer)
    s["sfx_unsafe"] = seq(sweep(note_freq("A4"), note_freq("A4"), 0.14, triangle, 0.32),
                          sweep(note_freq("F4"), note_freq("F4"), 0.24, triangle, 0.32, decay=0.1))
    # The Star, Level Completed and "Normal is open!" sounds are recorded sounds now (prepare_recorded.py).
    s["sfx_blip"] = make_blip()
    return s


def make_blip():
    # The Student's voice blip under each Reaction: one soft, very short "bup" syllable. The
    # game pitches it up a little for the girl. Never words.
    return seq(sweep(520, 700, 0.045, triangle, 0.26), sweep(700, 600, 0.035, triangle, 0.2, decay=0.02))


def make_rumble(dur=2.6):
    """A low, soft earthquake rumble: filtered noise with a slow swell, no sharp hits."""
    rnd = random.Random(11)
    n = int(dur * RATE)
    out = []
    lp1 = lp2 = 0.0
    k = 2 * math.pi * 70 / RATE  # one-pole low-pass near 70 Hz, twice
    for i in range(n):
        x = rnd.random() * 2 - 1
        lp1 += k * (x - lp1)
        lp2 += k * (lp1 - lp2)
        t = i / n
        swell = math.sin(math.pi * t) ** 0.8          # rises then fades away
        wobble = 0.75 + 0.25 * math.sin(2 * math.pi * 3.2 * i / RATE)
        out.append(lp2 * swell * wobble)
    peak = max(abs(v) for v in out)
    return [v / peak * 0.42 for v in out]               # quiet on purpose


def main():
    if "--only-level-music" in sys.argv[2:]:
        raise SystemExit("The --only-level-music option was retired. See Assets/Audio/Music/LICENSES.md.")

    root = sys.argv[1] if len(sys.argv) > 1 else "."
    out = os.path.join(root, "Assets", "Audio")
    if "--only-blip" in sys.argv[2:]:
        # Writes just the Student's voice blip, leaving every other audio file as it is.
        write_wav(os.path.join(out, "Sfx", "sfx_blip.wav"), make_blip())
        print("blip written to", out)
        return
    for name, data in make_sfx().items():
        write_wav(os.path.join(out, "Sfx", name + ".wav"), data)
    print("audio written to", out)


if __name__ == "__main__":
    main()
