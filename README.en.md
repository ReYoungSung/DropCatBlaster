[한국어](README.md) | **English**

# DropCatBlaster

A vertically scrolling punch-action defense game. The project was released as an Android beta; distribution has since been discontinued.

## Background

The project began as a capstone collaboration with another member of the same laboratory, with the goal of creating a new approach to defense games. Although it did not reach a competition submission, it provided a foundation in Unity game development.

## Team

TeamGooCat — 이영성 (Youngsung Ree), 박정배

## Development environment

| Item | Value |
| --- | --- |
| Unity Editor | **2021.3.25f1** |
| Unity project folder | `DropCat/` |
| Default branch | `main` |

The Unity version is taken from [ProjectVersion.txt](DropCat/ProjectSettings/ProjectVersion.txt).

- Android device for testing mobile controls and builds
- Git for restoring the Spine and BetterStreamingAssets Git package dependencies
- Android Build Support for the specified Unity version when building for Android

## Getting started

```bash
git clone --branch main https://github.com/ReYoungSung/DropCatBlaster.git
```

1. Install the Editor version above through Unity Hub.
2. Add the repository's `DropCat` folder as a project. The repository root and the Unity project folder are different.
3. Complete the initial import and Package Manager dependency restoration.
4. Open `Assets/01_Scenes/2023-New version 0.6.0/InitLogo.unity` and enter Play mode.

The build configuration contains the logo, title menu, tutorial, and stage scenes. Start from the logo scene rather than an individual stage so that initialization can run.

See [EditorBuildSettings.asset](DropCat/ProjectSettings/EditorBuildSettings.asset) for the build configuration and [manifest.json](DropCat/Packages/manifest.json) for dependencies. The `Assets/` paths below are relative to the Unity project folder.

## Main folders

| Path | Contents |
| --- | --- |
| [Assets/01_Scenes/](DropCat/Assets/01_Scenes/) | Logo, menu, tutorial, and stage scenes |
| [Assets/02_Scripts/](DropCat/Assets/02_Scripts/) | Input, combat, levels, persistence, and UI logic |

## Packages

`Packages/manifest.json` defines dependencies including Spine 4.1, BetterStreamingAssets, Input System, and URP. If restoration fails, check the Unity Console and Package Manager for Git availability and download errors first.

## Usage

### Playing

1. Proceed through the logo and title screens and select the start button. Depending on saved progress, it appears as a new-game or continue option.
2. On the first playthrough, start with the tutorial and follow the movement, jump, and attack instructions.
3. Move the on-screen joystick left or right to adjust your position. Swipe it toward an enemy to detect a target in that direction and attempt a punch.
4. Jump between platforms and enemies. The number of jumps is limited and is restored on landing.
5. Review the result on the stage-clear or game-over screen. During play, use the pause menu to resume, restart, or return to the main menu.

| Control | Action |
| --- | --- |
| Move the virtual joystick left/right | Move the character horizontally |
| Swipe the joystick toward an enemy | Punch a detected enemy; disabled while stunned |
| Jump input | Jump while jumps remain |
| Pause input | Pause gameplay and display the menu |

Device-specific jump and pause buttons follow the `PlayerJump` and `PauseGame` bindings in the project's Input Actions. Do not assume the same keyboard key is used in every environment. If an attack does not trigger, check whether an enemy is within detection range and whether the character is stunned.

Behavior references: [PlayerManager.cs](DropCat/Assets/02_Scripts/%5BCharacters%5D/%5BPlayer%5D/Manager/PlayerManager.cs) · [PauseManager.cs](DropCat/Assets/02_Scripts/UIs/PauseManager.cs)
