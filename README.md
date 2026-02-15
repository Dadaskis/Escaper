# Escaper - Unity 5.6 Game Project

This is the complete Unity project repository for my game, **Escaper**. It's a first-person extraction shooter / survival game built from the ground up. This repo contains all the source code, assets, and project files. This project is a successor to **Escaper Abandoned Beta**. The main target during development of this project is keeping the codebase cleaner as well as changing the overall design focus on more "traditional" shooting experience. To make it easier, I didn't do any modular weaponry, because those were painful to develop.

## Overview

Escaper is a game focused on tense encounters, loot-driven progression, and tactical gameplay. Players must go through hostile zones, gather valuable items and weapons, fight or flee from AI-controlled enemies, and successfully extract to the next level. This repository holds the entire development history and all the moving parts of the project.

## Repository Structure

The project is organized into several key directories:

### `_Resources/`
This directory contains the raw, source art assets used to build the game. They are the originals, mostly in Blender format, before being imported into Unity.
-   **`Escaper v2.0 characters/`**: Contains the source files for the in-game character models, including the `Bandit0` and `Military0` NPCs. You'll find their high-resolution textures (diffuse, normal, specular, etc.) and their original `.blend` files.
-   **`Escaper v2.0 items/`**: The source files for interactive items like the `Medkit0`, its icons, UI elements, and other shared item graphics.
-   **`Escaper v2.0 locations/`**: All the building blocks for the game's maps. This includes `.blend` files for various props (buildings, fences, pipes), foliage (trees, bushes), vehicles, and many test scenes. There are also helpful Python scripts here, like `MaterialsObtainer.py`, which I used to manage textures during the level creation process.
-   **`Escaper v2.0 weapons/`**: The source files for the weapon systems, including the DI-SP-0 and DI-SP-1 firearms, ammunition models, and the first-person hand models.

### `Assets/`
This is the main Unity project folder, containing all imported assets, scripts, and configured game objects.
-   **`Resources/`**: The core of the game's runtime data. It's organized into subfolders for a clean workflow:
    -   **`Animators/`**: Mecanim animation controllers for characters and weapons.
    -   **`Models/`**: The final, imported 3D models for characters, items, maps, and weapons, ready for use in Unity.
    -   **`Prefabs/`**: Reusable game objects like the `Player`, `LootBox`, `Decals`, `Particles` (gunshots, explosions), and all `PhysicalItems` (the in-world versions of medkits, ammo, etc.).
    -   **`Scripts/`**: The bulk of the game's C# code. This is where the logic for the player controller (`PlayerFirstPersonController.cs`), AI (`HumanoidCharacter.cs`), inventory (`GUIInventory.cs`), weapons (`RaycastFirearm.cs`), and game management (`GameLogic.cs`) lives.
    -   **`Scenes/`**: All the game's scenes, including the main menu, tutorial, and all the playable locations like `StartingLocation` and `DevLocation0`.
    -   **`Shaders/`**: Custom shader files used to achieve specific visual effects, like the glass for collimator sights, water, and post-processing effects.
    -   **`Sounds/`**: All audio assets, from footsteps and weapon shots to ambient music and UI sounds.
-   **External Assets / Plugins:** The project also makes use of several high-quality Unity assets and plugins to speed up development, including:
    -   `PostProcessing` (Unity's official post-processing stack)
    -   `InkPainter` (for decal painting)
    -   `ProGrids` (for level editing)
    -   `MoonSharp` (for Lua scripting integration)
    -   `JsonDotNet` (for JSON serialization)
    -   `Graphy` (for performance monitoring)

### `GameData/`
This folder contains data intended to be driven by scripts, primarily Lua.
-   **`Maps/`**: Lua scripts that define map connections (e.g., `Map0.lua` points to `Map1`).
-   **`Materials/`**: Lua scripts that define material properties at a high level, likely for a custom material system.
-   **`Models/`**: Exported `.obj` files of the maps, which are referenced by the Lua map files.
*I'll be honest with you, I barely remember what I was doing 6 years ago, but all that Lua stuff seems to be obsolete. At least at first glance.*

### `ProjectSettings/`
The standard Unity project settings folder, containing configurations for input, graphics, physics, layers, and more.

### `Saves/`
A folder for local save data, including game saves and graphics settings.

## Key Features Implemented

-   **First-Person Controller:** A custom character controller with smooth movement, crouching, jumping, and procedural step sounds based on surface material.
-   **Inventory System:** A robust grid-based inventory allowing players to manage items, weapons, ammo, and armor.
-   **Item System:** Support for different item types, including weapons (with ammo mechanics), medical items, and armor. Items can exist in the world (`PhysicalItem`) and be represented in the UI (`GUIItem`).
-   **AI System:** Basic AI for humanoid NPCs (`HumanoidCharacter`) with patrolling, awaiting, and aggressive states, allowing for simple combat encounters.
-   **Weapon System:** A raycast-based firearm system (`RaycastFirearm`) with support for shooting, reloading, jamming, and different ammunition types.
-   **Dynamic Map Loading:** A system for transitioning between different map scenes.
-   **Post-Processing:** Extensive use of Unity's Post-Processing stack for color grading, ambient occlusion, bloom, and other cinematic effects.
-   **UI System:** A comprehensive UI built with Unity Canvas for the main menu, inventory, health display, quests, and settings.

## Development

This project represents a significant solo development effort (it is significant because I said so). It started as a prototype and grew into a near-complete game framework. The code and structure reflect a focus on creating a modular and expandable system where new items, weapons, and levels can be added with relative ease.

## License
MIT License, happiness to everyone!
