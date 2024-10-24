# Glavo Game Engine

![Repository Automated Testing](https://github.com/Balein-Fercoadee/glavo-game-engine/actions/workflows/dotnet.yml/badge.svg)

The goal of this project is to create a text-based game engine in the spirit of the classic text RPG of the 80s.

## Requirements

- .NET 8.0 or greater
- VSCode with the 'C# Dev Kit' extension installed, or Visual Studio 2022 or greater

## Design

The engine is organized into a few projects:

| Project | Description |
|---------|-------------|
| Console UI | The text UI that runs Game Engine |
| Game Engine | The code that drives the game. All game logic is contained here. |
| Game Engine Tests | Unit tests that run against Game Engine in the CI/CD pipeline. |
| Game Database Editor | A MAUI application that allows for the creation and/or editing of game databases. |

## Running a game

If you want to a small sample of what the engine is capable of, from the command line:

```bash
glavo-game-engine.exe
```

This loads the file located in `ConsoleUI/sample_game`

If you have a game database you would like to load from the command line:

```bash
glavo-game-engine.exe <game_db_name> [save_game] [-d]
```

- `game_db_name` (required): The relative or full path and file name of the game to load
- `save_game` (optional): The relative or full path and file name of the game to load
- `-d` (optional): Output debug statements during game load and gameplay

## Creating or editing a game database

Creating a game database by hand (using a text editor) is possible but becomes cumbersome as the database becomes larger. This led to the creation of `GameDatabaseEditor`.

`GameDatabaseEditor` is a MAUI (Multi-platform App UI) application that allows the creation of a new game database or the editing of an existing database.

As of Oct 2024, `GameDatabaseEditor` can only run on Windows and MacOS. That's why the project isn't included in the solution; Actions won't run successfully if the MAUI project is in the solution and the runner OS is set as Ubuntu.
