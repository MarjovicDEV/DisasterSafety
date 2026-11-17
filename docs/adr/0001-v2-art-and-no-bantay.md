# 0001: v2 art style, an acting Student, and no Bantay

Date: 2026-11-17. Status: accepted.

## Context

The first Levels showed four Choice pictures (72x52) and had Bantay, a guide dog, read the intro, give the Hint and cheer. The user judged the characters and Choice pictures the weakest part of the game and the gameplay flat: picking a card does not show what the choice means. The art was drawn by Aseprite Lua scripts as ASCII pixel maps, which gave stiff results at the sizes a moving character needs.

## Decision

1. The Learner's own Student (boy or girl, picked once) acts out every Choice in the scene. Choices are short commands with a 16x16 icon in an Action Bar at the bottom. Choice pictures are retired.
2. Bantay is removed. A Caption banner tells the Scenario and shows the Hint; the Student's speech bubble carries reactions; the Student replaces Bantay on the Main Menu and in How to Play.
3. All art follows the v2 style: one dark outline around the outside only, three shades per colour with light from the left, symmetric front views built from mirrored half maps, a head about 1 to 2.7 of the body for the 50-pixel Student and about 1 to 2 for the 22-pixel chibi, and the school uniform (white polo or blouse, navy pants or skirt).
4. Claude drafts every sprite in code. The user may hand-polish any sprite in Aseprite; from then on that `.aseprite` file is the source and the build stops overwriting it.
5. The game is animated throughout: every Choice plays out, characters have frame sets per pose and a small animator.

## Consequences

- Every existing Setting background is redrawn in v2 style, with furniture sized for a 50-pixel Student.
- Each Choice needs an action animation, and walking needs side-view frames, which is much more art per Level than one picture. Mirroring and shared poses (walk, idle, reactions) keep it manageable.
- `DisasterSafety.Core` (Scenarios, Stars, Attempts) is unchanged. The Scenario JSON gains what the Action Bar and the action need (an icon and an action per Choice) and drops `picture` once every Level is moved over.
- The Bantay rule in AGENTS.md and the docs ("Bantay never judges or mentions Stars") now applies to the Student's reactions: they never judge and never mention Stars.
- Bantay's sprites and the Choice pictures stay until the rollout ends, then are deleted in one cleanup.

## Alternatives considered

- Tapping Hotspots in the scene, or dragging the Student: rejected for the Action Bar, which keeps the text to read and works on small phones.
- A licensed asset pack or a human artist for all art: kept open as a fallback if the scripted v2 art does not hold up in the pilot.
- Keeping Bantay on the menus only: rejected, so the game has one face, the Learner's Student.
