# Snap! Simulation

This is a console-based simulation of the card game "Snap!" between two computer players.

## How to Run

1. Ensure you have .NET 8 SDK installed.
2. Build and run the project using Visual Studio 2022 or via command line:
3. Follow the prompts:
- Enter the number of packs (each pack is 52 cards).
- Choose the matching condition (face value, suit, or both).

The game will simulate playing through the shuffled pile, announcing each "Snap!" and the winner at the end.

## Notes

- All cards from all packs are shuffled into a single pile.
- When two consecutive cards match by the chosen condition, a random player "snaps" and wins all cards on the table.
- Any cards left on the table at the end (not won by a snap) are discarded.
- The player with the most cards at the end wins. If tied, it's a draw.