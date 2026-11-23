# Bob Is Angry — Combat System

A high-school project: a 2D Unity/C# game prototype focused on building a player combat system. This README describes the project as it exists in the repository; it is an older school project, not a claim about current work or production-ready software.

## Systems explored

- Player movement and state handling, including grounded movement, jumping, sliding, and attacks.
- 2D attack hit detection with configurable ranges, damage, and knockback.
- Entity health and stun behavior, plus combo tracking and gameplay effects.
- Integration with Unity animation, physics, and scene components.

## Where to look

- `GymnasieArbete (tempName)/Assets/Scripts/PlayerController.cs` — player movement and state transitions.
- `GymnasieArbete (tempName)/Assets/Scripts/PlayerCombat.cs` — attacks, hit detection, and combo-related logic.
- `GymnasieArbete (tempName)/Assets/Scripts/Entity.cs` — shared health, movement, and hit behavior.
- `GymnasieArbete (tempName)/Assets/Scenes/SampleScene.unity` — included Unity scene.

## Opening the project

Open `GymnasieArbete (tempName)` with Unity 2019.3.0f6, the editor version recorded in `ProjectSettings/ProjectVersion.txt`. The project manifest records its Unity package dependencies. Newer Unity versions may require migration or compatibility adjustments.

## Skills demonstrated

C# scripting in Unity, 2D gameplay programming, state-driven player behavior, collision-based combat, and connecting gameplay logic to animation and effects.
