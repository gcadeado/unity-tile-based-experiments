# Tilemap game test

#### ScriptableObject Based Archituecture

Singletons are bad. If you worked on any project longer than a few days, you might have started to use Singletons for global references to objects your prefab can't directly reference in the inspector. Like a GameManager, an AudioManager, an UIManager. If you did, you might have run into the probleme of Singletons initializing each other, turning it into a inter-dependant mess.

Turns out you can avoid most of this hassle by using ScriptableObjects for "global access", and keep the code modular. There can be also used for a lot of other useful purposes. I recommend to check that conference out, who explains it in great details  : https://www.youtube.com/watch?v=raQ3iHhE_Kk. 

 # Other Credits
 Monogram Font (https://datagoblin.itch.io/monogram), a free font used in the game.

