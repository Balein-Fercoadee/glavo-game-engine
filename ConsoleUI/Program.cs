using System.CommandLine;
using GameEngine;

namespace ConsoleUI;

class Program()
{
    static void Main(string[] args)
    {
        // Setup command line parameters
        Option<string> fileOption = new("--gamefile")
        {
            Description = "Game database file to load.",
            Required = false,
            DefaultValueFactory = parseResult => "sample_game/sample_game.gge"

        };
        Option<string> saveFileOption = new("--saveFile")
        {
            Description = "Save file to load.",
            Required = false,
            DefaultValueFactory = defaultValue => ""
        };
        RootCommand rootCommand = new("Glavo Game Engine");
        rootCommand.Options.Add(fileOption);
        rootCommand.Options.Add(saveFileOption);

        rootCommand.Parse(args).Invoke();
        
        Engine engine = new Engine();

        // Running the exe with no args loads the sample game.
        if (args.Length == 0)
        {
            args = new string[1];
            args[0] = "sample_game/sample_game.gge";
        }
        engine.GameLoop(args);
    }
}
