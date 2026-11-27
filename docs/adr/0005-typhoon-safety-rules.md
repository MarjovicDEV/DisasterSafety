# 0005: Typhoon safety rules where sources disagree

Date: 2026-11-27. Status: accepted for the Typhoon Episode.

## Context

The Typhoon research (docs/research/typhoon-safety.md, "Source conflicts") found ten points where the primary sources disagree, and a Level must show each one way. PAGASA, DepEd, DILG and UNICEF Philippines are the Philippine sources; NOAA, Ready.gov and the American Red Cross are the international ones. The Levels are in docs/design/typhoon-episode.md.

## Decision

- **Class suspension:** follow DepEd Order No. 22, s. 2024 and EO 66, s. 2012 (TY-12). Grades 3 to 6 have no face-to-face classes at Wind Signal No. 2 or higher; at Signal No. 1 their classes go on. DO 37, s. 2022 (every signal suspends Kindergarten to Grade 12) is the older rule and is not used. If a signal is raised during class, the school sends learners home only if it is safe, and otherwise keeps them (TY-13).
- **Rainfall warnings:** a yellow warning is left to the mayor, as both DepEd orders say (TY-14). Rainfall colour warnings belong to the Flood Episode and do not appear in Typhoon Levels.
- **Light in a power outage:** a flashlight, never a candle or a kerosene lamp (TY-39, with FI-60 and FI-61). The Student never holds a candle or matches, and no Go Bag in the game holds them. The DepEd Grade 4 module, the UNICEF Philippines list and the OCD list that include candles are not followed on this point.
- **Windows:** no Level shows taping or covering a window as a Safe Choice (TY-38). Every source agrees the Student stays away from windows (TY-33). The DepEd Grade 4 advice to cover windows with cloth is not shown either way.
- **Open or closed windows:** windows stay closed during the storm (TY-38). No source says to open them during a typhoon; opening them is an Unsafe Choice.
- **Which signal system:** only the current PAGASA 5-level Wind Signals in use since 23 March 2022 (TY-01 to TY-06). Only Signals 1, 2 and 3 appear in the game. The older 4-level system in the DepEd Grade 4 module and the older PAGASA poster are not used.
- **When to evacuate:** the Student goes with the Grown-ups when the barangay or officials say to go (TY-51, TY-53). UNICEF's "at any warning" is not shown as a separate rule.
- **When it is safe to go out or go home:** only when officials say it is safe (TY-55). In the game the Barangay Tanod or the radio says so. The calm in the eye of the storm is never safe (TY-37).
- **Go Bag:** supplies for 3 days, as in CONTEXT.md and the Red Cross and OCD lists (TY-23, TY-24). NOAA's 7 days of water is not used.
- **Drinking water:** "boiled water" with no time (TY-58). Drinking water is a Flood topic (design T2).
- **Hotline:** always 911 (TY-65, FI-77). No General Santos CDRRMO number was found.

## Consequences

No Typhoon Level has a Choice where the Student holds a candle or matches, tapes or covers a window, opens a window as a Safe Choice, or goes outside before officials or the radio say it is safe. Signal 4 and Signal 5 never appear. The researcher reviews the Level list against these rules before the spec.

## Alternatives considered

Following the DepEd Grade 4 module everywhere, as the learners' own school material, was rejected: it predates the 2022 signal system, and its candle, kerosene lamp and window-cloth advice contradicts NOAA, the American Red Cross, BFP and NFPA. Following DO 37, s. 2022 for class suspension was rejected because DO 22, s. 2024 is newer and repeals inconsistent provisions.
