# RPG-Test

<b>Unity version used</b>: 2017.4.11f1

Android Build - https://github.com/dimaswift/RPG-Test/blob/master/dimaswift-rpg-test.apk

For this test I used a custom implementation of the MVC pattern, observer pattern, factory pattern, etc.

I separated the logic from the Unity engine completely by abstracting the View,
so it can be run on the console application, for example.

Average time spent on project: about 14 hours.

<b>Basic project structure:</b>
____
    |
    |___Model
    |___View
    |___Controller
    |___UnityImplementation
    |
____
<b>Relationships:</b>



![alt text](https://github.com/dimaswift/RPG-Test/blob/master/Untitled%20presentation.png)

<b>Usage:</b>

Main class of the game is GameController that depends on the GameConfig class and few interfaces.

Since the logic is completely separated from Unity, it's easily tested. The example of such unit tests is
inside <b>UnitTests</b> namespace.

Inside the Unity inplementation - entry point is the Game class that holds references to the Views.

Game class initializes GameController, Unity Views implementations, hooks them all together, holds all the references needed and is responsible for visualization of the units.
It also has GameConfig as an exposed field in the inspector.

In order to unit test the game inside Unity, just enable TestRunner object in the Main scene, and read the logs.
