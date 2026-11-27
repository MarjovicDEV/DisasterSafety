# 0002: Choice counts by Difficulty across all Episodes

Date: 2026-11-25. Status: accepted for all Episodes.

## Context

Fourteen Earthquake Levels currently offer four Choices. Easy 3 offers three. The researcher requested fewer Choices in Easy and Normal and reviewed every Level's Safe Choice, Unsafe Choices, Feedback findings, Act-outs and Hint.

## Decision

In every Episode, including future Episodes, each Easy Level will have two Choices, each Normal Level three, and each Hard Level four. ScenarioRules must check the exact count for the Level’s Difficulty. Exactly one Choice in each Scenario is the Safe Choice. For Earthquake, the researcher approved the retained Choices and their order in the [rollout design](../design/gameplay-redesign.md#c5-choice-counts-and-retained-choices).

## Consequences

The approved list reduces the total from 59 to 49 Choices across 15 Levels. Five Choices leave Easy and five leave Normal. Hard 5 replaces the pipe Choice with waiting for Mom. Hard 6 replaces leaving without a note with pushing past Mom at the door. Each Level keeps four Choices. The researcher also approved centering the Easy and Normal Action Bar buttons at their current size. C5 follows rollout ticket 15. Tutorial Page 3 will show the two retained Easy 1 Choices. C5 removes a retired Choice’s v2 art and its source only after confirming that no other Level, Tutorial Page, or screen uses them.

## Alternatives considered

The rollout allowed three or four Choices per Scenario. The researcher selected a fixed count at each Difficulty to reduce the number of actions that Learners compare in Easy and Normal.
