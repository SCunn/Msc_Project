
## 🛠 Skills
C#, Unity, VR, Meta Quest


# Msc Project Games & Extended Reality


```bash
Project built using Unity version 2022.3.4f1
Due to use of LTS, Cloning the project from github to Visual Studio or VS Code is

recommended over downloading a zip of the project as doing the latter will cause problems, missing files and such.
```

A brief description of what this project is
```bash
This project is a 100% handtracking controlled room scale VR game developed on Unity for Meta Quest VR devices.  Using the Quest's 3D spatial mapping technology, The game generates the level at runtime and uses the players real environment (From Quest Space Setup) as a template for the game level.
```
What type of game is this?
```bash
This game is a survival horror themed, horde style shooter.
```
## Demo

link to video demo 

https://studentdkit-my.sharepoint.com/:v:/g/personal/d00262648_student_dkit_ie/EUGKRVBnnHpCqm2zuqaFfyMB8gd4te32phci3XL0tjVGkw?e=Cp5hIO&nav=eyJyZWZlcnJhbEluZm8iOnsicmVmZXJyYWxBcHAiOiJTdHJlYW1XZWJBcHAiLCJyZWZlcnJhbFZpZXciOiJTaGFyZURpYWxvZy1MaW5rIiwicmVmZXJyYWxBcHBQbGF0Zm9ybSI6IldlYiIsInJlZmVycmFsTW9kZSI6InZpZXcifX0%3D

## Screenshots

![App Screenshot](https://via.placeholder.com/468x300?text=App+Screenshot+Here)


## Download link for APK

Link to project application

```bash
  https://studentdkit-my.sharepoint.com/:u:/g/personal/d00262648_student_dkit_ie/EYeWYJqsyhJLt3smHlnxzQYBf1XHcBmyPOFAmDAp8SND5A?e=mKrRdc
```



## Run Beta

Run Beta:
```bash
To run the Beta game APK build:
-	Use SideQuest Advanced Installer App on PC: 
o	It is possible to transfer the file from a PC to a Quest us the SideQuest Advanced Installer App on the PC, but there are a few steps to follow before this can be achievable, such as setting up the Quest for development mode using a Meta Quest account and allowing USB debugging on the Quest.  If you have or haven't SideQuest setup already,  here is a link to a PDF walkthrough of how to Install a VR APK using SideQuest for Quest process:
o	https://hqsoftwarelab.com/wp-content/uploads/2023/01/How-to-install-APK-file-to-Oculus-Quest-2-1.pdf

The Beta has been built using and tested using a Quest 2, therefore I would recommend using a Quest 2. (The project should work with Quest pro and Quest 3, but has not been tested with either).

```
Space Setup on Quest 2
```bash
Before running the game insure that you have at least one mapped out room saved using the Quest's Space Setup  for the game to work. (Example video of Space Setup https://www.youtube.com/watch?v=4t1CdmDeBhA&t=4s)
To map a room with Space Setup on your Quest 2, Go to:
	Settings > Physical Space > Space Setup
      						-- Select Space Setup > Set up 
(Here you will use your controllers to map out your room.  First you will map out the walls, the  furniture (windows, doors, tables ....)).  For this project I recommend not creating doors, but windows the shape of a door and regular windows, these will serve as the entry points for the enemies.  I set this game to run in household sized rooms and a larger space in studio space, the nav mesh(The walkable area for the enemies) used in the game is dynamic and will generate to suit your mapped out play area, when mapping out your room consider the entry points and walkable floor space for the game’s enemies.  The game can be played either in a small or large space.

```

## Playing the Game

```bash
Playing the Game
The game will run on start up when the apk file is selected on Quest 2. 
Objectives
Kill oncoming hordes of enemy wolves using a shotgun and an axe to defend yourself with.  The enemies can kill you, when you die the game reloads back to the start state.
The game is a room scale VR experience 100% controlled using hand tracking (No Controllers).  When using hand tracking gestures, it is important to remember that hand gestures only work when they are visible to the headsets cameras.  The Quest 2 now uses wide motion which keeps track of the last known position of a held in game object, so actions such as strikes from objects from over-head are now possible.
Weapon Controls
Shotgun: This is a right-handed weapon operated using two hands.  There are two grab points on the gun.  The corresponding hands will snap into the correct positions when grabbed at either point (meaning, if the user grabs the barrel with the right hand, the right hand will automatically snap to the gunstock, and vice versa).
``` 
```bash
Right-Hand: Holds the gunstock, Fires gun 
Left-Hand: Holds the Barrel, Reloads gun
``` 
```bash
-	Fire shotgun (right hand only): While holding the gunstock with the right hand, make a thumbs up gesture with your right hand.  After firing the gun it must be reloaded.
```
```bash
-	Reload shotgun (Left hand only): While holding the underside of the gun barrel with your left hand, make a rock gesture (like in paper, scissors, rock).  This can be achieved more easily by extending then closing the thumb while holding the underbarrel, it can also be achieved by bringing your left fist towards your face or by making a pumping action to simulate a more realistic action, but this way is not always accurate.
``` 
      			 


```bash 
Axe: In this game the axe is a one handed weapon that can be held with either the left or right hand.  To fight with the axe you simply strike the enemies directly with it.  
(Alternatively, The left quest 2 touch controller also has an axe object attached to it, this was used during user testing where the controller was taped to a unused support handle from a drill set.  To activate this just pick up the left controller during the games runtime, this action will deactivate hand tracking.  )
```
