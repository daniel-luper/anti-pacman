# Anti Pacman
Proof of concept for a Pacman game "gone wrong". Written in C# using the XNA-inspired [MonoGame](http://www.monogame.net/) framework.

## The game
#### Concept
You start up an old Pacman game in the arcade. It looks and sounds a bit strange, a bit glitchy.
You press start and discover that Pacman and the Ghosts appear to have switched rolls!
**_Pacman chases the ghosts, the ghosts eat the dots._**


<img src="https://media.giphy.com/media/XaFiVCmcgvIkowGbMV/giphy.gif" height=324 width=250> <img src="https://i.imgur.com/EqPjm5r.png" height=324 width=576>
<img src="https://media.giphy.com/media/Y0nXu8Eb6R2Za9ATXD/giphy.gif" height=232 width=413> <img src="https://media.giphy.com/media/RKMmTLvGyO8KuuqZF3/giphy.gif" height=232 width=413>

#### Controls
- Arrow keys to move and navigate menus
- Escape to go back
- Space to select

#### Problems
The original Pacman is a cleverly designed game whose simple mechanics are naturally fun to the player.
However, while working on this project, I realized that *just* flipping the roles would not be very entertaining gameplay...

There needs to be advanced AI, flashy visual effects, additional game mechanics, and ideally a robust animation system.
This beginner, solo-developed project suddenly increased in scope, making this a 'failed' proof of concept, in the sense that my original idea was not sufficient. Maybe a bigger team could create a full game.

## Installation
#### Download (Windows 64-bit)
1. Download zip from [here](https://www.dropbox.com/s/hg77yhmzwlkt6hq/anti-pacman_v1.0.zip?dl=0)
2. Extract zip
3. Run setup.exe

Game should automatically launch after installation. There is also a desktop shortcut that gets added.
Use 'Programs and Features' in the Control Panel to uninstall.

#### Build from source
1. Install MonoGame 3.6
2. Launch Visual Studio
3. Open the project (File->Open->Project/Solution->Browse to the 'AntiPacman.sln' file)
4. Build and run the project (it might be necessary to clean it first)
