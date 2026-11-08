# NRCU Kart

A kart racer built on the [Nous Research Cinematic Universe](https://github.com/123mikeyd/Nous_Research_Cinematic_Universe)
cast. Arcade handling in the Mario Kart 8 / Crash Team Racing register: grip-based
turning, a hop-drift with three mini-turbo tiers, and item counterplay — never
Nintendo assets, names, or music. The reference is feel.

Unity **6000.0.32f1** (6 LTS), URP, new Input System.

## Open it

1. Unity Hub → Open → Add project from disk → this folder.
2. Open `Assets/Scenes/TestTrack` once it exists (the first milestone scene).
3. Press Play. Arrow keys / WASD to drive, Space to hop-drift.

## What is here

| Area | File | What it does |
|---|---|---|
| Driving | `Assets/Scripts/Kart/KartController.cs` | throttle, speed-scaled steer, lateral grip, hop-drift, blue/orange/purple boosts |
| Input | `KartInputFrame` (same file) | one struct, deadzoned at the source — never `Input.GetAxis` ad hoc |
| Track | `Assets/Scripts/Track/Checkpoint.cs` | ordered gates; anti-shortcut |
| Recovery | `Assets/Scripts/Track/Respawn.cs` | returns a fallen kart to its last gate |
| Race | `Assets/Scripts/Race/RaceManager.cs` | countdown, laps, finish, cup points 15/12/10/8/6/4/2/1 |
| Standing | `Assets/Scripts/Race/PositionTracker.cs` | rank by lap, gate, then distance |
| Items | `Assets/Scripts/Items/ItemBox.cs` | rows with respawn; items are data |
| Cast | `Assets/Data/Roster/roster.json` | **35 characters** derived from the NRCU bible |

The roster is generated, not hand-written — `bots/kart-racer/scripts/roster.py` in
[bot-wiki](https://github.com/SouthpawIN/bot-wiki) rebuilds it from the NRCU
character pages. Every entry carries the reason it got its weight class
(`source_note`) and a credit line, because uncredited art does not ship.

## Milestones

1. **One kart, one flat track, drift that feels right.** Nothing else until this is true.
2. Track kit: spline → road mesh → walls → gates → item rows → respawn.
3. Race loop: 3 laps, rubber-band AI with per-character skill, results, cup points.
4. Items: three to start, none undodgeable.
5. Polish: FOV kick, drift sparks, hit shake, engine pitch, minimap, pause menu.

## Credits

Characters and art belong to their NRCU creators — see `CREDITS.md` upstream and
each roster entry's `credit`. Turbo Fit / Sovthpaw is Chris (sovthpaw).
