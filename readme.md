# Glavo Game Engine

![Repository Automated Testing](https://github.com/Balein-Fercoadee/glavo-game-engine/actions/workflows/dotnet.yml/badge.svg)

A text-based game engine built from the ground up on C#/.NET.

## Requirements

- .NET 8.0 or greater
- VSCode with the 'C# Dev Kit' extension installed, or Visual Studio 2022 or greater

## Design

The engine is organized into a few projects:

- Console UI
- Game Engine
- Game Engine Tests

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

- game_db_name (required): The relative or full path and file name of the game to load
- save_game (optional): The relative or full path and file name of the game to load
- -d (optional): Output debug statements during game load and gameplay
