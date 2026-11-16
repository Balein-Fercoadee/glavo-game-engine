# Glavo Game Engine

![Repository Automated Testing](https://github.com/Balein-Fercoadee/glavo-game-engine/actions/workflows/dotnet.yml/badge.svg)
[![Code Coverage](https://codecov.io/gh/Balein-Fercoadee/glavo-game-engine/branch/main/graph/badge.svg)](https://codecov.io/gh/Balein-Fercoadee/glavo-game-engine)

API documentation is published to [GitHub Pages](https://balein-fercoadee.github.io/glavo-game-engine/).

The goal of this project is to create a text-based game engine in the spirit of the classic text RPG of the 80s.

## Requirements

- .NET 10.0 or greater
- VSCode with the 'C# Dev Kit' extension installed, or Visual Studio 2022 or greater

If you want to play with the Database Editor in VS Code:

- .NET MAUI extension

## Design

The engine is organized into a few projects:

| Project | Description |
| --------- | ------------- |
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

Creating a game database by hand (using a text editor) is possible but becomes cumbersome as the database becomes larger (it doesn't have to be that large). This led to the creation of `GameDatabaseEditor`.

`GameDatabaseEditor` is a MAUI (Multi-platform App UI) application that allows the creation of a new game database and the editing of an existing database. By default, the Editor complies for Windows.

## Game Database details

A game based on the GGE is comprised of a few simple pieces:

- Room - A location within the game that the player can visit.
- Item - A thing that the player can interact with. Sometimes they can take them with.
- Word - Verbs and Nouns that the game understands. There are certain Words that are common within every GGE game.
- Action - A combination of Verb/Noun that triggers something to happen. An Action really can do anything the game designer wants.
- Message - A string that will be displayed to the player. Usually triggered as part of an Action.
- Player - Keeps track of where the player is, where the player was, and what the player is carrying (inventory).
- GameDatabase - Keeps all of the above in-memory. Also keeps track of the starting Room and the treasure Room.

### Room description

Rooms are the basis of a GGE game. They are the locations that the player visits. Room has the following properties:

| Name | Description |
| - | - |
| Id | The internal ID for the Room. Players never know about these. |
| Name | The name of the Room. When a player revists a Room, they only see the name unless they `look`. |
| Description | The description of the Room the players gets when they first enter a Room. They need to `look` again to get the description. |
| ExitIdEast | The Room Id that is east of this Room. Value of -1 means the player cannot go this direction. |
| ExitIdNorth | The Room Id that is north of this Room. Value of -1 means the player cannot go this direction. |
| ExitIdSouth | The Room Id that is south of this Room. Value of -1 means the player cannot go this direction. |
| ExitIdWest | The Room Id that is west of this Room. Value of -1 means the player cannot go this direction. |
| ExitIdUp | The Room Id that is up of this Room. Value of -1 means the player cannot go this direction. |
| ExitIdDown | The Room Id that is down of this Room. Value of -1 means the player cannot go this direction. |
| ContainedItemIds | A collection of all Items that are in this Room. |

### Item description

Items are the things in a game that the player can interact with. Some can be picked up and carried; others are features of a Room. Item has the following properties:

| Name | Description |
| - | - |
| Id | The internal ID for the Item. Players never know about these. |
| Name | The name of the Item. This is what the player sees. |
| ShortName | The name the Parser and Processor understand. This is what the player types to refer to the Item. |
| Description | The description of the Item. |
| CanTake | Whether the player can pick up the Item. Items that can't be taken are typically features of the Room. |
| IsTreasure | Whether the Item is a treasure. Treasures are meant to be collected and placed in the treasure Room to score or complete the game. |

### Word description

Words are the Verbs and Nouns that the game understands. Word has the following properties:

| Name | Description |
| - | - |
| Id | The internal ID for the Word. Players never know about these. |
| Name | The Word as it is known by the game. |
| Aliases | Alternate spellings or abbreviations the player can type for the Word (for example, `n` for `north`). |
| WordType | Whether the Word is a `Noun` (1) or a `Verb` (2). |

### Action description

Actions define interactions beyond the built-in words. When the player enters the trigger Verb and Noun and all of the Action's Conditions are true, its Commands are executed. Action has the following properties:

| Name | Description |
| - | - |
| Id | The internal ID for the Action. Players never know about these. |
| Description | A description of the Action. It is for documentation only and is never displayed in-game. |
| TriggerVerbId | The Id of the Verb Word that triggers the Action. Value of -1 means unset. |
| TriggerNounId | The Id of the Noun Word that triggers the Action. Value of -1 means unset. |
| Conditions | A collection of Conditions that must all be true for the Action to execute its Commands. |
| Commands | A collection of Commands that are executed when all Conditions are true. |

Each Condition and Command has an object Id (`ObjectId`) whose meaning depends on its type.

| Condition | Description |
| - | - |
| PlayerHasItem | The player has the Item, identified by `ObjectId`, in inventory. |
| PlayerInRoomWithItem | The player is in the same Room as the Item identified by `ObjectId`. |

| Command | Description |
| - | - |
| DisplayMessage | Displays the Message identified by `ObjectId`. |
| PutItemInRoom | Adds the Item identified by `ObjectId` to the current Room. |
| RemoveItemFromRoom | Removes the Item identified by `ObjectId` from the current Room. |
| MoveToRoom | Moves the player to the Room identified by `ObjectId`. |

### Message description

Messages are strings that are displayed to the player, usually as the result of an Action. Message has the following properties:

| Name | Description |
| - | - |
| Id | The internal ID for the Message. Players never know about these. |
| Text | The text of the Message. |

### Player description

Player keeps track of where the player is, where the player has been, and what the player is carrying. Player is part of the engine's runtime state and is not defined in the game database. Player has the following properties:

| Name | Description |
| - | - |
| CurrentRoomId | The Id of the Room where the player currently is. Value of -1 means unset. Setting it also records the Room as visited. |
| VisitedRoomIds | A collection of the Ids of the Rooms the player has already visited. |
| ItemInventory | A collection of the Ids of the Items the player is carrying. |

### GameDatabase description

GameDatabase holds all of the game's data in memory and is what is saved to and loaded from a `.gge` file. GameDatabase has the following properties:

| Name | Description |
| - | - |
| Name | The name of the game. |
| Description | The description of the game. It is displayed once the game is loaded and before the first Room is shown. |
| StartingRoomId | The Id of the Room where the player starts the game. |
| TreasureRoomId | The Id of the Room where treasure Items must be placed to be scored. |
| Rooms | A collection of all Rooms in the game. |
| Items | A collection of all Items in the game. |
| Words | A collection of all Words in the game. |
| Actions | A collection of all Actions in the game. |
| Messages | A collection of all Messages in the game. |

## Publishing the Windows editor

The [Windows release workflow](https://github.com/Balein-Fercoadee/glavo-game-engine/actions/workflows/game-database-editor-release.yml) publishes a self-contained, unpackaged `win-x64` build. To create a GitHub release, push a tag named `gamedatabaseeditor-v<version>` (for example, `gamedatabaseeditor-v1.0.0`). The workflow attaches `GameDatabaseEditor-win-x64.zip` to the release. You can also run the workflow manually to download the ZIP from its workflow artifacts.

Extract the ZIP, or publish from Solution Explorer on Windows. Run `GameDatabaseEditor.exe` from the extracted or publish output folder and keep the accompanying files and folders together. This distribution does not require a signing certificate or the Microsoft Store. Note: It is for 64-bit Windows only.
