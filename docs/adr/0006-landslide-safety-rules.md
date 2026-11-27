# 0006: Landslide safety rules and how a landslide is shown

Date: 2026-11-27. Status: accepted for the Landslide Episode.

## Context

The Landslide research (docs/research/landslide-safety.md) found "during" advice that the sources give in different ways, and a landslide is the most frightening picture in the game. The Learners are in Grades 3 to 6 at Baluan Elementary School, and some lived through the 8 June 2026 earthquake near General Santos. The Levels are in docs/design/landslide-episode.md.

## Decision

- **How a landslide is shown (L2):** the game shows Slide Signs and Fallen Soil at rest. A slide is drawn moving only once, in Hard 2: far away, on a bare hill across the valley, at most 48x40 art pixels, 6 frames and then still, with no house, tree, road or person on it or below it, no bouncing rocks, no dust cloud and no sound. Nobody is buried, hit, trapped, hurt or crying, nothing falls toward a person, and no house collapses. Every actor stays at least 24 art pixels from a Slope's foot, a crack, the spring, Muddy Runoff, Fallen Soil and the rope.
- **Earthquakes:** the protective action during shaking stays "Duck, Cover, and Hold" (CONTEXT.md). The one earthquake-triggered Level (Hard 7) starts after the shaking has stopped and does not shake; its lesson is moving away from the Slope (LS-29, EQ-82).
- **"During" advice not shown:** the Quezon City advice to get under a sturdy table when a slide comes and leaving is not possible, Ready.gov's "move uphill" when caught in the path, and the news-relayed "curl into a ball" are not taught. No Level puts a person where a slide could reach them, so the game teaches leaving early instead (LS-17, LS-20).
- **Night:** heavy rain at night (LS-22) is not shown.
- **Warnings:** the Barangay Tanod and the Teacher give them, in speech bubbles. Rainfall colour warnings stay with Flood (docs/design/flood-episode.md, F1); no Landslide Choice is about a TV or radio warning.
- **When to go home:** only when the Tanod says it is safe (LS-24, TY-55), even after the rain stops (LS-23).
- **Child actions with no Philippine source:** telling a Grown-up about a Slide Sign (LS-10), not digging into a Slope (LS-12), not playing on a wet Slope, and staying out of the cleanup (LS-28) are game adaptations of sourced rules, marked GAP in the Level table.
- **ADR 0005 holds:** a flashlight, never a candle; 911; the 3-day Go Bag.

## Consequences

No Level shows a slide near a person or a home, a person on or below a Slope, a rescue, or a Student holding a shovel in the mud. The far slide in Hard 2 is the only moving soil in the game. The art batches check the 24-pixel gap with each batch's `review.py`.

## Alternatives considered

Never drawing a slide moving was the first proposal; the user chose to allow one far slide so the Learners see what the signs warn about. Teaching the "get under a sturdy table" advice was rejected: it would need a slide reaching a house with the Student inside.
