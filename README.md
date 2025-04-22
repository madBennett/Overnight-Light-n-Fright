# Final Project Midway Review 

## Game Name: Buigi’s Bansion

## Game Description: 

Buigi’s Bansion is a ghost-themed 2D pixel art top-down single-player ghost game with four main rooms/challenges. Each section will have a unique state machine ghost that behaves differently and will apply multiple kinds of shader effects to the player. The player can complete the rooms in any order they like and will be locked in each room until the challenge is completed. To win the game, the player must complete the challenges in all four rooms within the allotted time. If the player runs out of time before all challenges are completed, they lose the game. In all rooms, the player will have a flashlight to kill or scare ghosts away, depending on that ghost’s behavior. The player cannot die; however, the player will be stunned or have some kind of punishment from the ghost that will cause them to potentially run out of time and lose the game.

The Maze room:  Reach the end of the maze while various ghosts chase you.  Use the flashlight to scare them off and send them running.  Don't let the ghost get to you or they could distort your vision, reverse your controls, or stun you.  The ghost goes in between an idle, move, attack, and run state.  On an interaction with a flashlight the ghost will run away from the player.  However, if the ghost is able to get in range of the player it can attack.  When it attacks it can apply various effects to the player such as visual distortion via shaders, reversing the controls, preventing player movement, or damage the player.  When the player is not within a certain range it will idly wander the maze.  The player's goal is to reach the end of the maze as quickly as possible, but the ghost sneaking up on the player and attacking them will delay the player's progress..

The Shooter Room is a fast-paced, survival-style challenge where the player must fend off waves of Mob Ghosts using their flashlight. The objective is to survive multiple waves by strategically using the flashlight to destroy approaching ghosts. Mob Ghosts follow a unique state machine: they begin in a Wander state, moving randomly, and shift into a Chase state when the player enters their detection range, aggressively pursuing them. If a ghost collides with the player, the player is temporarily stunned, losing valuable time. (during which the screen and all ghosts are also frozen/stunned) Two custom shaders enhance the intensity of the room: Mob Ghost Damage, which flashes the screen red and distorts visuals when the player is hit, and Mob Ghost Chase, which adds visual distortion around the screen when ghosts are actively chasing. Each wave becomes progressively harder, introducing more ghosts, faster speeds, and tighter chase conditions. The player must clear all waves to unlock the exit and complete the room, relying on quick reflexes, spatial awareness, and flashlight control to avoid being overwhelmed and losing time.

The Dexterity room is a challenge of coordination. The player must avoid being hit by a series of ghost attacks for a period of time. This will involve simple ‘bullet’ ghost projectiles which will appear from offscreen and follow a set path, and potentially an additional player-chasing ghost to complicate the dodging process. There will be some timer counting down, making clear the amount of time which must be survived, and when hit, this could reset or it could simply add time to the timer.

The escape room is the last task given which adds extra thrill with the time left on the timer being limited. The player gets trapped in a room haunted by a ghost that keeps on shifting between different objects in the room and haunts them to affect the player’s performance. These effects are implemented through shaders effects. The objective is for the player to solve a mini puzzle game to open a chest in order to obtain an artifact that repulses the ghost before the timer runs out. As the timer decreases, the ghost becomes more aggressive in its attacks until it finally attacks the player for a game over when the attempt fails. The ghost behaves on state machine for idle, attacking, and haunting behavior. The ghost should traverse from one state to another. Further custom shaders will be applied to add distortions in the player’s visuals when the ghost is near. 

The elements from this class that we will be incorporating into our game are shaders and state machines. We plan to create more complex state machines with a high number of states and complex behavior.  The shaders we implement will also be more complex and visually appealing. We likely will need some help with implementing parallaxing, pathing mechanisms, and animations.

## Team Members:

Madison Bennett
mbennett4915@sdsu.edu

Adelina Martinez
amartinez8831@sdsu.edu 

Riley Potter
potter2911@sdsu.edu

Afnan Algharbi
aalgharbi5447@sdsu.edu 

## Role/Tasks: 
- **Madison Bennett:** Responsible for the Maze room, Effect controller, effect indicators, player controls, base ghost state machine, maze ghost state machine(with pathing), scene UI, created flashlight, a custom shader, audio implementation, will aid in the implementation of animations.

- **Adelina Martinez:** Responsible for the Shooter-based survival room. Creating a “Mob Ghost” State machine and a “Mob Spawner” script that will spawn waves/mobs of ghosts for the player to fight off with their flashlight. Will create a custom “Mob Ghost Damage” shader when the ghost hits the player, as well as a custom visual distortion shader “Mob Ghost Chase” when the player is being chased/pursued. Will work with Maddi to animate ghosts and players as well as create the environment and art. Generally responsible for tracking member tasks and taking notes at each meeting.

- **Riley Penguini:** Dexterity-based room and its own shaders. Also currently responsible for camera movement/control and flashlight behavior (and certain aspects of game design, particularly the basic overall structure of the game.). Further tasks will be decided on as they appear.

- **Afnan Algharbi:** Responsible for enabling transitions between the scenes from the lobby to the other rooms of each task which includes the locking/ unlocking of the entries to each scene as well as tracking the progress of the tasks completion. The game starts at the lobby then the player is able to choose any task to go to which are separate rooms. Upon the completion of each task, the player gets to return to the lobby and each completed task leads to locking the gate in question. This continues until all 4 Gates are locked which unlock the final gate to victory leading out of the lobby. Also, responsible for implementing the Escape room task which includes the full implementation and setting up of the ghost state machine, the custom distortion shader, and the hidden object mini game. 

## Post Midway Review Goals: 
- Environments and art 
- Player / Ghost animation
- Additional shader effects
- Additional particle effects 
- Sounds Design / Audio effects
- Parallax effect
- Refined State Machines
