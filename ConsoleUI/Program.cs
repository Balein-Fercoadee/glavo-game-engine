using System.CommandLine;
using GameEngine;

namespace ConsoleUI;

class Program()
{
    static int Main(string[] args)
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

        rootCommand.SetAction(parseResult =>
        {
            string gameFilePath = parseResult.GetValue(fileOption) ?? "sample_game/sample_game.gge";
            string saveFilePath = parseResult.GetValue(saveFileOption) ?? string.Empty;
            string[] engineArgs = string.IsNullOrWhiteSpace(saveFilePath)
                ? [gameFilePath]
                : [gameFilePath, saveFilePath];

            return new Engine().GameLoop(engineArgs);
        });

        return rootCommand.Parse(args).Invoke();
    }
}
