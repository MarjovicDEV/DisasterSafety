# 0003: Licensed music for the Earthquake release

Date: 2026-11-25. Status: accepted.

## Context

The researcher wants more natural-sounding background music and has selected three musical roles for this release. The game must be able to include each recording in its offline Android package.

## Decision

The researcher chooses and downloads recordings whose source pages identify the recording and state either CC0 or CC BY 4.0. The three tracks serve the menus, Earthquake gameplay and Level Completed.

During implementation, record each track's title, creator, source page and licence in `Assets/Audio/Music/LICENSES.md`. For a CC BY 4.0 recording, include the required credit, links to the source and licence, and a description of any edits in the in-game credits. For a CC0 recording, credit is optional. See the [CC BY 4.0 terms](https://creativecommons.org/licenses/by/4.0/) and the [CC0 dedication](https://creativecommons.org/publicdomain/zero/1.0/).

## Revision, 2026-11-26

The researcher replaced the three musical roles with two music sets and two menu tracks. Each set has an Easy, a Normal and a Hard gameplay track and a Level Completed cue. Set A plays in Earthquake, Typhoon and Landslide: Cherry Blossom on Easy, Happy Alley on Normal, Prelude and Action on Hard and Light Sting on Level Completed. Set B plays in Fire and Flood: Cheery Monday on Easy, B-Roll on Normal, Machinations on Hard and Piano Between on Level Completed. The menus play Somewhere Sunny and Easy Lemon (60 second) in turn, one after the other. All ten recordings are by Kevin MacLeod and are licensed under CC BY 4.0, so the game must show credits. incompetech offers them only as MP3, so no lossless source exists.

## Consequences

The music ticket requires the selected recordings and their source pages. The implementation must preserve the attribution information supplied with each CC BY recording when trimming, looping or converting it.

## Alternatives considered

Limiting the selection to CC0 would simplify the credits. The researcher approved both licences to allow a wider choice of recordings.
