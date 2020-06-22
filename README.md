# Tilemap game test

Unity mini experiments series: basic tile-based with custom 2D mechanics.

Live demo:
https://gcadeado.github.io/unity-tile-based-experiments/

#### ScriptableObject Based Archituecture

Singletons are bad. If you worked on any project longer than a few days, you might have started to use Singletons for global references to objects your prefab can't directly reference in the inspector. Like a GameManager, an AudioManager, an UIManager. If you did, you might have run into the probleme of Singletons initializing each other, turning it into a inter-dependant mess.

Turns out you can avoid most of this hassle by using ScriptableObjects for "global access", and keep the code modular. There can be also used for a lot of other useful purposes. I recommend to (check that conference out)(https://www.youtube.com/watch?v=raQ3iHhE_Kk), who explains it in great details.

## Experiments

- Custom player controller and movement mechanics
- Level designs using tilemaps
- Extended 2D tools with [2d-extras](https://github.com/Unity-Technologies/2d-extras)
- 2D animations and controllers
- TextMeshPro with custom script animations

# Run

- Download and install Unity 2020.1.0b12
- Rename the files `RoguePlayer_48x48-watermark.png` and `RogueEnvironment16x16-watermark.png` to `RoguePlayer_48x48.png` and `RogueEnvironment16x16.png`, respectively.
- Import project on your Unity Hub
- Play :)

# Credits
 - __**space_puzzle**__, music by Leon Menezes for the main song.
 - **[Rogue Dungeon Tileset 16x16](https://secrethideout.itch.io/rogue-dungeon-tileset-16x16)** by [Secret Hideout](https://secrethideout.itch.io/) for this awesome tileset
 - **[SmallWorld_WeeklyJam40](https://github.com/Cawotte/SmallWorld_WeeklyJam40)** by [Cawotte](https://github.com/Cawotte) for some core mechanics used in this game.
 - [Monogram Font](https://datagoblin.itch.io/monogram), a free font used in the game.
