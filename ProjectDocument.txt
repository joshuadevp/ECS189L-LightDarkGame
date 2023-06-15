# Game Basic Information #


## Summary ##


A malicious Darkness has covered the land, spreading inky blackness and horrific creatures. It is up to the Cleric of Light to defeat the enemies and dispel the Darkness. Using your light powers, fight off enemies, destroy the darkness, complete objectives, and upgrade your attacks. Can you fend off the Darkness?


## Gameplay Explanation ##


In this top down Vampire Survivor like shooter game you control your character with WASD and fire your attack with a left click. We recommend using a mouse for easier control.
Escape can be used to pause the game.


The main gimmick is the Darkness covering the map. Enemies can only spawn inside of the darkness, giving players the ability to influence where enemies can spawn by destroying the darkness with their attacks. But the Darkness also spreads and deals damage to the player; so be aware of your positioning and take some time to shoot through the Darkness instead of just enemies.


You progress by completing objectives. The yellow arrow points to your next objective and text at the top of the screen explains what you need to do.


After completing an objective you get a choice of three upgrades. The description explains how it influences your stats. Red text means it makes the stat worse, while all other text is beneficial. We’ve found that at least one speed upgrade is very important, as well as focusing on attack speed and damage. Knockback is very useful if enemies are getting too close.


As time goes by enemies get stronger and the Darkness spreads faster, so we recommend completing objectives quickly lest you be overrun.


Enemies drop yellow syringes that heal your character. 


**If you did work that should be factored into your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**


# Main Roles #


Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.


Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 


*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)


Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).


You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.


## Josh


### Producer


As producer I directed our vision for the game while letting team members include their own creative decisions. In order to be successful we needed to have easy ways to communicate and I needed to be available to my team. I organized roles and handed out tasks as appropriate. As producer, I took responsibility as the project wrapped up to merge our code and delegate tasks to team members. For example, Alex finished his enemy and player logic quickly, so I tasked him with applying sprites and including an audio manager. I quickly answered questions people had to help us finish.


Organization:


We used Github to manage our code. I asked people to create new branches to create and test their changes before merging with the rest of our code. I set up a prototype branch where we could merge our code before pulling into the main branch. To reduce merge conflicts, I made it clear which parts people were working on so scripts or art didn’t conflict and had people work in different scenes when testing changes. We also started with a large [game document](https://docs.google.com/document/d/1ByETL1qGBzoG69ypi7tCN__MSBwDZnII1hAxYNHsbbs/edit#heading=h.bkb9krhee1cw) to get our initial ideas down. But as we progressed we leaned into smaller bits of information during communication as a large document gets difficult to work with.


Communication:


I created a Discord server to facilitate communication. We used this server to ask each other questions, post our work so far, post ideas and reference art, and hold meetings. I used when2meet to decide the best times for us to meet, with links like this, https://www.when2meet.com/?20201593-FmQrd. We met almost every week to discuss our progress and plan our next steps. I also set up Github and Discord webhooks to get automatic updates when people make changes to the repository in our server using this [tutorial].
(https://gist.github.com/jagrosh/5b1761213e33fc5b54ec7f6379034a22) Discord was really great because I could send announcements to everyone easily and we could interact with files like images and music quickly before uploading to the repository.


### Game Logic: Darkness System


I implemented the Darkness system that our game uses, all related scripts are found in this [folder](https://github.com/joshuadevp/ECS189L-LightDarkGame/tree/main/Assets/Scripts/FogOfWarDarkness).


Backend Logic:


Behind the scenes, the Darkness is controlled by a monolithic script [FogOfDarknessManager.cs](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/FogOfDarknessManager.cs). It consists of an array of [DarknessPoints](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/DarknessPoint.cs) that hold unique data per point of Darkness in the game. My first approach involved creating GameObjects but Unity could not efficiently handle almost 100k objects without switching to an Entity Component System, which would have been a challenging change to make part way through development. Instead, I managed my own array of points and ran large loops over them to translate their data into the game. But optimizing was still a problem. Online forums and posts on [Reddit](https://www.reddit.com/r/howdidtheycodeit/comments/y757yg/how_does_vampire_survivors_handle_so_many_enemies/), [Unity](https://forum.unity.com/threads/how-to-optimize-hundreds-of-bullets.463257/), and [other](https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity8.html), helped me come up with these ideas and optimize them. I pooled a set of colliders and moved them into position every time I updated a point so Unity is handling less game objects. I followed Unity’s [special optimizations](https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity8.html) guide to optimize my C# code, such as switching everything to [jagged arrays](https://github.com/joshuadevp/ECS189L-LightDarkGame/commit/8bfaf188329c322351d4463565444e47e83d6981).


Generation:


I initialized all the Darkness with an [interface](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/IDarknessSpecGenerator.cs) to support different scripts for generation. I implemented a simple [perlin noise generator](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/PerlinDarknessGenerator.cs), which you can see as different chunks of Darkness look darker in game.


Control:


I used a [scriptable object](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/DarknessSettings.cs) to control the Darkness spreading and enemy spawning. By having a single reference used everywhere, we can easily edit spawn or spread rates and influence the entire game.


Frontend:


Visually, the player sees the Darkness through a particle system I also made. A [script](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/FogOfWarDarkness/DarknessParticleEmitter.cs) gets a list of active points and using their data, spawns custom particles. The particle emitter then also applies some randomness to give the look a more natural feeling. Despite the Darkness being represented as a large 2D grid of squares, the particles make it look natural and smooth.


## User Interface - Dylan


UI inspirations and gameplay interactions can be found here: Art, UI, Audio - Google Docs


## Movement/Physics/Player Input/Miscellaneous - Alex Chiu


**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**


**Describe the default input configuration.**


**Add an entry for each platform or input style your project supports.**


*Note* - Just wanted to note that my sub-role: Player and Enemy programming was arguably a more important part of the game and it’s a little hard to draw the line between the two. I have also spent more time on that role than physics and movement.


*Movement scheme* - The only input scheme is WASD and mouse left on PC. Preferably the game is played with a mouse as hold left click while moving on trackpad may be difficult.


*3D rigidbodies with 2D sprites* - The physics scheme that we chose to use is using 3D rigidbodies and colliders while keeping 2D sprites for rendering. There was no downside for using 2D collision, but 3D collision is much more efficient for the darkness, so we wanted to use the same physics scheme across the board. I think this is pretty common for 2D games.


*Script-controlled movement* - The movement of characters are fully controlled by controller scripts, namely [PlayerController](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/PlayerController.cs#LL6C9-L6C9) and [EnemyController](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Enemy/MeleeEnemy.cs#LL6C13-L6C13). The speed that they move at and damage they deal are based on their stat scripts (Player and Enemy), and different enemies inherit from others to give new functionalities. I wanted to give the player movement a crisp, precise feel, so we are using raw input axis to control movement. Enemies generally just move towards player, but different enemies may act differently. For example, a range enemy shoots projectiles while moving, whereas the smiler starts asleep but will be awaken and charge towards the player once it is damaged or approached.


*Ability cast scheme* - The original plan was for player to [execute commands on left and right click](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/PlayerController.cs#LL31C35-L31C35) and allow the executed [PlayerCommand](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/70bb3f8b42f150cce73ef1b6c0d348f3a92b8110/Assets/Scripts/Player/PlayerCommand.cs#LL5C7-L5C7) script to handle all remaining calculations and decisions using the player game object that was passed in. This includes timer and projectile spawning, etc. I also wrote an [example command](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/TestCommand.cs#LL5C7-L5C7) to demonstrate it. Later, Nam moved it into a [separate script](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/AimController.cs#LL5C18-L5C18) and disabled this function in Player Controller (the script still uses PlayerCommands, though). There was also setback when initially PlayerCommand was .implemented as an interface, which cannot be displayed in the unity inspector GUI, so we could not manually drag in preset commands. I had to change it into an abstract class inheriting from MonoBehaviour.
 
*On trigger collisions for most physics interactions* - There are no physical collision other than the Smiler enemy, which pushes the player around using its rigidbody. Enemy projectiles deals a set amount of damage while melee elements deals damage every frame while colliding with the player. Originally there was physical collisions, but got changed to on trigger by Josh when we merged all our progress together.


*Game Manager and Enemy Scaling* - Implemented a singleton [game manager](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/GameManager.cs#LL102C19-L102C19) that retains some key information and scales the enemy’s stats based on the time and levels cleared in the game. Also implemented a simple pausing scheme and holds a reference to audio manager to minimize number of singletons and coupling.


*Audio Manager* - Implemented an [audio manager](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Audio/AudioManager.cs#L5) that allows developers to play sfx easily using the name of the file. It loads in the audio files from a scriptable object on awake and holds reference to music and sfx players. Was planning to expand on it given more time but others had implemented workaround for music for the demo.




## Animation and Visuals - Dylan and JingAng Xu


All game assets except our font were hand drawn and created from scratch.
Inspirations, thought processes, and designs can be found here: Art, UI, Audio - Google Docs




## Game Logic (Primary Fire/Upgrades) - Nam Nguyen


Originally created a span of abilities for the player to use (a primary and secondary), with passive abilities in mind that would allow the player to create a variety of builds for each run. The original documentation can be found here: https://docs.google.com/document/d/1q69GPi5flTMQwvtRjdbSWXX8bVX44xsPGswkNd65YuY/edit?usp=sharing However, due to time constraints and consideration, it was cut for the base game and an upgradable basic shot command was used instead. 


###Aiming and Shooting:
I created the AimController script which checks the mouse position and player position and finds an angle at which to fire towards the mouse. This makes it possible to fire in any direction.


Initially, there was a PrimaryFlickerCommand and PrimaryCandleCommand, which had different behavior for how the projectile was going to be fired. Due to complications with implementation and planning, these two were scrapped. Instead, a PrimaryShotCommand script is used instead. It's attached to the fire1 input in theAimController, and utilizes the command pattern. This command takes care of all the behavioral needs for firing a projectile, such as checking to see whether or not enough time has passed to fire another projectile. It takes into account the player’s stats via a Player reference. The player can manipulate these stats through upgrades after completing an objective.


###Upgrades:
Upgrades are made as an interface as IUpgrade. It contains three abstract functions that all upgrades will need to use: ApplyUpgrade(), GetSprite(), and GetDetails(). 
- ApplyUpgrade utilizes the ModStat class which modifies a given stat on the player
- GetSprite simply returns the Sprite icon that will be shown to the player upon upgrading
- GetDetails returns a string that describes the held information in the upgrade script in a way that is easy to digest.
Using IUpgrade as a foundation, I created two Scriptable Object scripts: FlickerGenericUpgrade and PlayerStatUpgrade. FlickerGenericUpgrade handles and gives the player stat changes that correspond to the player’s projectile attack, while PlayerStatUpgrade typically only changes the player’s health and movement speed. These two scripts were separated as they handled applying the upgrade and what kind of detailed description is presented differently, and it helps streamline the process of easily making more upgrades. Due to the system created with these two scripts, I was easily able to end up creating and tweaking 17 total upgrades that the player can pick from in-game. A lot of edits and polishing was done for GetDetails to be sure that it is easy to understand what the upgrades do at a glance. I put in a lot of checks for whether or not a stat is changed, and if it was not changed, then it does not show that stat on the upgrade details. I also used rich text via colored text for when an upgrade has a downside to it (like when an upgrade lowers damage instead of increases). I had to put consideration into the shotInterval as well, as the opposite value (lower shot interval = faster firerate, which is good) was better in that specific instance.


The upgrade UI was created by Josh initially, and then I cleaned it up as things like the text scaling were off. I edited Josh’s ObjectiveManager script to incorporate the upgrade system and its UI elements. After an objective is completed by the player, three upgrades are selected randomly to pick from. 






###


# Sub-Roles


## Game Feel: Map and Objective Design


Map Design:


Using the tiles our artist drew and a tilemap, I designed the map generation you see in the background of the game. I used an [interface](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/MapBackground/ITileMapGenerator.cs) to support many different generators but only implemented a [noise generator](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/MapBackground/NoiseTileMapGenerator.cs). This script randomly selects from finely tuned noise functions that are more advanced than the simple perlin noise Unity provides. I used this [article](https://medium.com/@5argon/various-noise-functions-76327e056450) to learn about these undocumented functions and apply them to map generation. I particularly liked using the F1 and F2 values from the cellular noise function.


Some visuals:
Simplex Noise
  

































Cellular F1 Noise
  







































Cellular F2 Noise
  



Objectives:


Unlike typical Vampire Survivor like games, I didn’t want players to get upgrades by just killing enemies because they have no reason to move and I wanted players to use more of their controls, not less. Inspired by [Swarm Grinder](https://store.steampowered.com/app/1375900/Swarm_Grinder/), I made objectives and gave players a choice of upgrade once they complete an objective. My goal was to keep players moving, so each objective spawns a set distance from the player and forces them to move towards, fighting off the Darkness that’s in their way. Once there, the objectives differ. [One](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/Objectives/SurviveObjective.cs) requires them to stay in the area for a set amount of time while [another](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/main/Assets/Scripts/Objectives/BossObjective.cs) has them fight an extra hard enemy but gives them freedom of movement to move around it.


Letting players choose their upgrade is important because it lets players focus on a build and gives some control over what they get when the upgrades are randomly chosen.


The objectives were programmed as scriptable objects, which lets us define more objectives with different settings from a single base script.


## Audio JingAng Xu


Sound effect and Music can be found here: Art, UI, Audio - Google Docs




All of the game audios are original, created specifically for this game, especially the music. As corresponding to our 2D hand drawing art style, I chose to use 8-bit sound effects to keep the interaction sounds consistent to the visual and create the feeling of fighting in an underground dungeon filled with deadly darkness.


For the Music, I composed 2 loop musics for the title scene (Title theme) and the in-game combat, and two phrases for victory and defeat.


The title music aimed to create tense and horror feelings , while the combat music consists of two parts (A Beginning part and the following Loop part), gradually building the intense feelings of the game world with all the darkness and monsters sieging the player character.


When the player HP bar hit zero, the YOU DIED theme fit perfectly for the sad feelings, and correspondingly a Stage finishing theme to be played cheering the player when the player wins the game. 


## Gameplay Testing/Balance/Game Feel - Nam Nguyen
###Playtesting/Balance
To understand a general sense of how people felt about the game, I created a playtest form that helped get feedback from players. Form here: https://docs.google.com/forms/d/e/1FAIpQLSc8lDZ4qjl39D0qPBurGcPjvFcZIc6alwE1vtmx1_VDqizMOA/viewform


We received a total of 5 responses from this form. Responses can be find through this sheet:
https://docs.google.com/spreadsheets/d/1cTiSIB0d_ygR9U4uwLQREkW1qIVwIzl1o2g9LS6uQ_s/edit?usp=sharing 


I took note from this and my own experience playing was that the game was very unbalanced; many players found the game to generally be too difficult and confusing. Specifically, the darkness spread too quickly and was hard to combat. The difficulty scaling of the game was way off, as the beginning was near impossible as the base projectile was too weak, while the later-half was too easy due to some upgrades being too strong. 


In doing so, I made the following changes:
- Reworked all pre-existing upgrades and added many more to improve balance and diversity
- Duplicate upgrades are no longer possible as one of the three pickable upgrades
- - This was more of an oversight, and was intended to be fixed later.
- Lowered base darkness spread speed by a little bit
- Lowered base enemy spawn by a little bit
- Increased the player’s base projectile size by almost 2 times its original size
- - This change is mainly directed for making easier to clear the darkness
- Increased base projectile lifetime by 1 more second
- - Same reasoning as above
- Base projectile speed increased by about 3
- Decreased base shot interval to be 0.15 sec faster
- Doubled base knockback on enemies
- Player speed reduced by 2
- - Since the overall projectile was stronger and enemies spawning less earlier, reducing the speed by a little felt right with the difficulty decrease


These changes made the game a lot more doable at the beginning, and the difficulty curve isn’t as steep as it was originally.


###Game Feel
In tandem with the balancing, I also went through the game multiple times editing other team member’s pre-existing scripts while adjusting the game feel to create better sensory responses, such as updated visuals on the menu buttons, faded background to the menus, and more sounds implemented into the game (like sound on button hover/click).


## Player and Enemy programming - Alex Chiu


**Player and enemy programming is mainly concerned with the structure, movement, interaction, and animation of player and enemies.**


*More complex stat system* - Allows [stats](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/ModifiableStats/ModStat.cs#LL10C6-L10C6) to be changed based on [modifiers](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/ModifiableStats/StatModifier.cs#LL8C18-L8C18), which has types flat, multAdd, and multiplicative, allowing level ups to give more complex buffs to the player (this is later also used in the projectile system).


*The ability to add temporary modifiers* - [Temporary Modifiers](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/Player.cs#LL73C26-L73C26) was implemented using coroutines, which was originally ideated for giving a temporary speed or damage boost for the secondary ability.


*Player and Enemy stat designs* - [Player](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Player/Player.cs#L6) and [Enemy](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Enemy/Enemy.cs#LL5C5-L5C5) contains the general stat designs of the two entities. While player is more complex and subject to change (thus the mod stats), enemies are much more simple and has their stats modified on spawn based on the [enemy modifier](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/GameManager.cs#LL102C19-L102C19), which scales with time and progress in the game (the modifier scaling can be tweaked in the unity inspector for game balance).


*Player and Enemy movement* - Player and enemy controllers use the stats in the stat scripts to determine [how fast they move](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Enemy/MeleeEnemy.cs#L33) and how much damage they deal when [colliding with enemies](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2940a67a943460a6ce926c8d564f98e0c9c8f65c/Assets/Scripts/Enemy/MeleeEnemy.cs#LL63C20-L63C20). The enemy controller scripts all inherit from EnemyController, then based on their movement breaks into melee enemy, ranged enemy, and smiler (a special type of melee enemy that starts asleep).


*Player/enemy/projectile animation* - I was also in charge of importing and setting up the animations for players, enemies, and projectiles. They are generally very simple, and smiler is the most complex with three animation states.


*Player and Enemy HP Bar and damage stats* - I implemented the [player](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2b235827e54ebc25cd2335a84d081dc7ba4a75aa/Assets/Scripts/Player/PlayerHPBar.cs#LL41C15-L41C15) and [enemy](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/2b235827e54ebc25cd2335a84d081dc7ba4a75aa/Assets/Scripts/Enemy/EnemyHPBar.cs#LL27C29-L27C29) hp bars using a simple lerp animation with 2 bars. I also added the function to print your damage on enemies on projectile hit.


*Character base stats* - These are some [artifacts](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/70bb3f8b42f150cce73ef1b6c0d348f3a92b8110/Assets/Scripts/Player/CharacterBase.cs#LL10C9-L10C9) from when we thought about having different starting characters with different stats, but eventually we focused on getting other things done first. Can easily be expanded with more time and playtesting.


*Drag-and-drop enemy prefabs* - Pre-implemented [prefabs of enemies](https://github.com/joshuadevp/ECS189L-LightDarkGame/blob/70bb3f8b42f150cce73ef1b6c0d348f3a92b8110/Assets/Prefabs/Enemy/Smiler.prefab#L1). They can easily be expanded to introduce new kinds of enemies by just tweaking their base stats like speed and attack (e.g. the smiler was originally just a faster version of melee enemy).


## Press Kit and Trailer


**Include links to your presskit materials and trailer.**


Trailer - Dylan: https://drive.google.com/file/d/1CjC772ZNsmP6pffEzeTseHdcwOKWITOm/view?usp=sharing


Itch Page - Nam Nguyen: 
https://xerial.itch.io/light-in-the-dark 
**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**


In our trailer, we showed off our main gameplay loop. You start off weak, build up to be something powerful, and attempt to stave off the darkness. The last bit of the trailer is modified to have an increased spawn rate of enemies, but it’s to show our game is challenging and that you WILL get overrun.


For the screenshots (you can find them on the itch page), I tried to pick ones that showcased very varied situations that you as the player could get yourself in. From fighting a powerful version of the smiler, a sneak peak at the possible upgrades you can receive, darkness overcoming the player, all the way to an instance where the player is extraordinarily powerful.


**Document what you added to and how you tweaked your game to improve its game feel.**