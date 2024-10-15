using System;

namespace GameEngine;

public static class Constants
{
 
    public const string INPUT_PROMPT = "What should I do?> ";

    public const string HELP_STANDARD_VERBS = "drop, go, help (h), look (l), take, quit (q)";
    public const string HELP_STANDARD_DIRECTIONS = "north (n), south (s), east (e), west (w), up (u), down (d)";
    public const string HELP_STANDARD_MISC = "inventory (i, inv)";

    public const string LOADING_HELP_TEXT = "usage: glavo-game-engine <game_db_name> [save_game] [-d]\n" +
    "Arguments:\n" +
    "  <gamename>  Required. The full path and file name of the game to load.\n" +
    " [save_game]  The full path and file name of the saved game to load.\n" +
    "        [-d]  Outputs debug statements during game load and gameplay.";
    public const string LOADING_TEXT = "Glavo Game Engine\n" +
    "=================\n" +
    "A text-based game engine.\n" +
    "Release 1.0, (c)2024.";

    public const int WORD_ID_UNSET = -1;

    /// <summary>
    /// The default value for an unset <c>Room.Id</c>.
    /// </summary>
    public const int ROOM_ID_UNSET = -1;

    public const string SETUP_ERROR_HEADER_TEXT = "Error while loading...";

    public static readonly List<string> STANDARD_DIRECTIONS = ["north", "south", "east", "west", "up", "down"];

}
