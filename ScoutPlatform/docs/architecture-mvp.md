# ScoutPlatform MVP Design Notes

This file captures the design direction from your prior ChatGPT discussion and turns it into buildable scope.

## Positioning
- Product category: football scouting decision platform.
- Strategic focus: decision intelligence, not data-volume competition with incumbents.
- Core differentiator: team-specific suitability scoring using MCDA with explainability.

## Architecture target (Clean Architecture)
- `ScoutPlatform.Api`: HTTP endpoints, auth, request validation, API versioning.
- `ScoutPlatform.Application`: use-cases and orchestration.
- `ScoutPlatform.Domain`: entities, value objects, scoring interfaces/rules.
- `ScoutPlatform.Infrastructure`: EF Core, data providers, caching, job adapters.
- Optional: `ScoutPlatform.Worker` for ingestion/recalculation jobs.
- Optional: `ScoutPlatform.Tests` for scoring + use-case tests.

## Core bounded contexts
- Identity and Organizations
- Player Intelligence
- Team Profile
- Suitability and MCDA Engine
- Shortlists and Reports

## Scoring flow (deterministic)
1. Filter by hard constraints.
2. Normalize metrics to comparable range.
3. Compute weighted aggregate score.
4. Return explainable metric contribution breakdown.

## MVP order
1. Database + migrations + metric definition seed.
2. Players + player metrics ingestion APIs.
3. Team profile + weights + constraints APIs.
4. MCDA ranking endpoint (explainable response).
5. Persist suitability scores + background recalculation.
6. Shortlists, notes, tags.

## Why this can win
- Better team-fit decisions.
- Transparent scoring, easier scout trust.
- Faster shortlist creation with tactical context.
