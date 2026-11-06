# Glavo Game Engine

![Repository Automated Testing](https://github.com/Balein-Fercoadee/glavo-game-engine/actions/workflows/dotnet.yml/badge.svg)

The goal of this project is to create a text-based game engine in the spirit of the classic text RPG of the 80s.

## Requirements

- .NET 10.0 or greater
- VSCode with the 'C# Dev Kit' extension installed, or Visual Studio 2022 or greater

If you want to play with the Database Editor in VS Code:

- .NET MAUI extension

## Design

The engine is organized into a few projects:

| Project | Description |
|---------|-------------|
| Console UI | The text UI that runs Game Engine |
| Game Engine | The code that drives the game. All game logic is contained here. |
| Game Engine Tests | Unit tests that run against Game Engine in the CI/CD pipeline. |
| Game Database Editor | A MAUI application that allows for the creation and/or editing of game databases. |

## Running a game

To run the sample game from the command line:

```bash
glavo-game-engine.exe
```

With no options, the Console UI loads `sample_game/sample_game.gge`.

To load a different game database, use `--gamefile`:

```bash
glavo-game-engine.exe --gamefile <game_db_path>
```

To load a saved game, also provide `--saveFile`:

```bash
glavo-game-engine.exe --gamefile <game_db_path> --saveFile <save_file_path>
```

- `--gamefile` (optional): The relative or full path to the game database. Defaults to `sample_game/sample_game.gge`.
- `--saveFile` (optional): The relative or full path to the save file to load.
- `--help` (or `-h`): Display command-line usage information.

## Creating or editing a game database

Creating a game database by hand (using a text editor) is possible but becomes cumbersome as the database becomes larger. This led to the creation of `GameDatabaseEditor`.

`GameDatabaseEditor` is a MAUI (Multi-platform App UI) application that allows the creation of a new game database or the editing of an existing database.

As of Oct 2024, `GameDatabaseEditor` can only run on Windows and MacOS. That's why the project isn't included in the solution; Actions won't run successfully if the MAUI project is in the solution and the runner OS is set as Ubuntu.

### Publishing the Windows editor

The Windows release workflow publishes a self-contained, unpackaged `win-x64` build. To create a GitHub release, push a tag named `gamedatabaseeditor-v<version>` (for example, `gamedatabaseeditor-v1.0.0`). The workflow attaches `GameDatabaseEditor-win-x64.zip` to the release. You can also run the workflow manually to download the ZIP from its workflow artifacts.

Extract the ZIP, or publish from Solution Explorer on Windows. Run `GameDatabaseEditor.exe` from the extracted or publish output folder and keep the accompanying files and folders together. This distribution does not require a signing certificate or the Microsoft Store; it is for 64-bit Windows only.
