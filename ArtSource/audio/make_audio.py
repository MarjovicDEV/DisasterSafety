"""Generate the menu and Level music loops and UI sound effects for Disaster Safety.

Run from the project root:
    python -I ArtSource/audio/make_audio.py D:/GameDEV/DisasterSafety
Add --only-level-music to write just Assets/Audio/Music/bgm_level.wav.
Add --only-blip to write just Assets/Audio/Sfx/sfx_blip.wav.

Writes 16-bit mono WAV files to Assets/Audio/. Uses only the standard library.
Everything is synthesised, so there are no third-party licences to track.
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


def render_voice(notes, beat, wave_fn, volume, duty=0.5, staccato=0.85):
    out = []
    for name, beats in notes:
        n = int(beats * beat * RATE)
        f = note_freq(name)
        sounding = int(n * staccato)
        phase = 0.0
        for i in range(n):
            if f > 0 and i < sounding:
                phase += f / RATE
                v = wave_fn(phase, duty) if wave_fn is square else wave_fn(phase)
                out.append(v * volume * envelope(i, sounding, 0.004, 0.03))
            else:
                out.append(0.0)
    return out


def mix(*tracks):
    n = max(len(t) for t in tracks)
    return [sum(t[i] if i < len(t) else 0.0 for t in tracks) for i in range(n)]


def make_music():
    bpm = 116
    beat = 60.0 / bpm
    # 8 bars of 4/4, chords: C G Am F | C G F G
    lead = [
        ("E5", 1), ("G5", 0.5), ("E5", 0.5), ("C5", 1), ("D5", 1),
        ("D5", 1), ("B4", 0.5), ("D5", 0.5), ("G5", 1.5), ("R", 0.5),
        ("C5", 1), ("E5", 0.5), ("C5", 0.5), ("A4", 1), ("C5", 1),
        ("F5", 1), ("E5", 0.5), ("D5", 0.5), ("C5", 1), ("A4", 1),
        ("E5", 1), ("G5", 0.5), ("E5", 0.5), ("C6", 1), ("G5", 1),
        ("B4", 1), ("D5", 0.5), ("G5", 0.5), ("F5", 1), ("D5", 1),
        ("A4", 0.5), ("C5", 0.5), ("F5", 1), ("E5", 0.5), ("D5", 0.5), ("C5", 1),
        ("D5", 1), ("E5", 0.5), ("F#5", 0.5), ("G5", 1), ("R", 1),
    ]
    roots = ["C3", "G2", "A2", "F2", "C3", "G2", "F2", "G2"]
    bass = []
    for r in roots:
        f = r[0] + str(int(r[-1]) + 1)
        bass += [(r, 1), (f, 0.5), (r, 0.5), (r, 1), (f, 1)]
    chords = {
        "C3": ["C4", "E4", "G4"], "G2": ["B3", "D4", "G4"], "A2": ["A3", "C4", "E4"], "F2": ["A3", "C4", "F4"],
    }
    arp = []
    for r in roots:
        tones = chords[r]
        for k in range(8):
            arp.append((tones[k % 3], 0.5))
    t_lead = render_voice(lead, beat, square, 0.22, duty=0.25, staccato=0.9)
    t_bass = render_voice(bass, beat, triangle, 0.38, staccato=0.8)
    t_arp = render_voice(arp, beat, square, 0.07, duty=0.125, staccato=0.6)
    # soft hi-hat on off-beats
    rnd = random.Random(7)
    hat = []
    step = int(beat * 0.5 * RATE)
    total = len(t_lead)
    for s in range(0, total, step):
        on = (s // step) % 2 == 1
        for i in range(step):
            v = (rnd.random() * 2 - 1) * 0.06 * math.exp(-i / (RATE * 0.02)) if on else 0.0
            hat.append(v)
    return mix(t_lead, t_bass, t_arp, hat[:total])


def make_level_music():
    """The Level loop: calm and focused, slower and softer than the menu song, so the
    earthquake rumble and the Student's Choices stand out. No drums; soft triangle lead."""
    bpm = 88
    beat = 60.0 / bpm
    # 8 bars of 4/4, chords: F C G Am | F C G G
    lead = [
        ("A4", 2), ("C5", 1), ("A4", 1),
        ("G4", 3), ("E4", 1),
        ("D5", 2), ("B4", 1), ("G4", 1),
        ("C5", 3), ("R", 1),
        ("A4", 1), ("C5", 1), ("F5", 2),
        ("E5", 2), ("D5", 1), ("C5", 1),
        ("B4", 2), ("D5", 2),
        ("G4", 3), ("R", 1),
    ]
    roots = ["F2", "C3", "G2", "A2", "F2", "C3", "G2", "G2"]
    bass = []
    for r in roots:
        bass += [(r, 2), (r, 2)]
    chords = {
        "F2": ["A3", "C4", "F4"], "C3": ["G3", "C4", "E4"], "G2": ["G3", "B3", "D4"], "A2": ["A3", "C4", "E4"],
    }
    arp = []
    for r in roots:
        tones = chords[r]
        for k in range(8):
            arp.append((tones[[0, 1, 2, 1][k % 4]], 0.5))
    t_lead = render_voice(lead, beat, triangle, 0.24, staccato=0.95)
    t_bass = render_voice(bass, beat, triangle, 0.30, staccato=0.92)
    t_arp = render_voice(arp, beat, square, 0.045, duty=0.125, staccato=0.55)
    return mix(t_lead, t_bass, t_arp)


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
    s["sfx_click"] = sweep(660, 990, 0.06, square, 0.35, 0.25)
    s["sfx_back"] = sweep(700, 420, 0.08, square, 0.35, 0.25)
    s["sfx_open"] = seq(sweep(523, 523, 0.05, square, 0.3, 0.25), sweep(784, 784, 0.07, square, 0.3, 0.25))
    s["sfx_confirm"] = seq(*[sweep(note_freq(n), note_freq(n), 0.07, square, 0.3, 0.25) for n in ["C5", "E5", "G5"]],
                           sweep(note_freq("C6"), note_freq("C6"), 0.16, square, 0.3, 0.25))
    s["sfx_locked"] = seq(sweep(180, 160, 0.09, square, 0.35, 0.5), silence(0.03), sweep(150, 130, 0.12, square, 0.35, 0.5))
    s["sfx_type"] = sweep(1400, 1200, 0.025, square, 0.2, 0.5)
    s["sfx_toggle"] = seq(sweep(880, 880, 0.04, square, 0.3, 0.25), sweep(1320, 1320, 0.05, square, 0.3, 0.25))
    s["sfx_sparkle"] = mix(*[[0.0] * int(k * 0.05 * RATE) + sweep(note_freq(n), note_freq(n), 0.25, triangle, 0.25, decay=0.08)
                             for k, n in enumerate(["E6", "G6", "C7"])])
    s["sfx_soon"] = seq(sweep(note_freq("G5"), note_freq("G5"), 0.08, triangle, 0.4), sweep(note_freq("E5"), note_freq("E5"), 0.14, triangle, 0.4))

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
    # One Star landing: a bell chime (the game raises its pitch for each Star)
    s["sfx_star"] = mix(sweep(note_freq("E6"), note_freq("E6"), 0.5, triangle, 0.3, decay=0.12),
                        sweep(note_freq("B6"), note_freq("B6"), 0.4, triangle, 0.12, decay=0.08))
    # Level Completed: a short fanfare
    lead = [("G4", 0.5), ("C5", 0.5), ("E5", 0.5), ("G5", 1), ("E5", 0.5), ("G5", 2)]
    bass = [("C3", 1), ("G3", 1), ("C3", 1.5), ("C4", 1.5)]
    beat = 0.15
    s["sfx_complete"] = mix(render_voice(lead, beat, square, 0.24, duty=0.25, staccato=0.92),
                            render_voice(bass, beat, triangle, 0.34, staccato=0.9))
    # A Difficulty or Episode is now Open ("Normal is open!"): a soft rising 4-note jingle,
    # shorter and quieter than sfx_complete, with a triangle echo for sparkle.
    opened = [("C5", 0.1), ("E5", 0.1), ("G5", 0.1), ("C6", 0.35)]
    s["sfx_opened"] = mix(
        seq(*[sweep(note_freq(n), note_freq(n), d, triangle, 0.3, decay=0.1 if n == "C6" else None) for n, d in opened]),
        seq(silence(0.05), *[sweep(note_freq(n) * 2, note_freq(n) * 2, d, triangle, 0.08, decay=0.08) for n, d in opened]))
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
    root = sys.argv[1] if len(sys.argv) > 1 else "."
    out = os.path.join(root, "Assets", "Audio")
    if "--only-level-music" in sys.argv[2:]:
        # Writes just the Level loop, leaving every other audio file as it is.
        write_wav(os.path.join(out, "Music", "bgm_level.wav"), make_level_music())
        print("level music written to", out)
        return
    if "--only-blip" in sys.argv[2:]:
        # Writes just the Student's voice blip, leaving every other audio file as it is.
        write_wav(os.path.join(out, "Sfx", "sfx_blip.wav"), make_blip())
        print("blip written to", out)
        return
    write_wav(os.path.join(out, "Music", "bgm_level.wav"), make_level_music())
    write_wav(os.path.join(out, "Music", "bgm_menu.wav"), make_music())
    for name, data in make_sfx().items():
        write_wav(os.path.join(out, "Sfx", name + ".wav"), data)
    print("audio written to", out)


if __name__ == "__main__":
    main()
