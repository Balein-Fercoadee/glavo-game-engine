namespace GameEngine;

public class EngineSetup
{
    public EngineSetup()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static EngineSetupReturn StartSetup(string[] args)
    {
        EngineSetupReturn setupReturn = new EngineSetupReturn();
        setupReturn.SetupSuccessful = true;

        // Check that args contain at least one value.
        // I.e. Has at least a path to a game database.
        if (args.Length == 0)
        {
            setupReturn.ErrorMessages.Add("<game_db_name> is required!");
        }
        else
        {
            setupReturn.GameDatabaseFilePath = args[0].Trim();
            if (!File.Exists(setupReturn.GameDatabaseFilePath))
            {
                setupReturn.ErrorMessages.Add($"Invalid <game_db_name> file/path: {setupReturn.GameDatabaseFilePath}");
            }
            else
            {
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i].Trim();

                    if (string.IsNullOrWhiteSpace(arg))
                    {
                        continue;
                    }

                    if (arg.StartsWith('-'))
                    {
                        string cleanArg = arg.TrimStart('-');

                        switch (cleanArg)
                        {
                            case "d":
                                setupReturn.OutputDebugStatements = true;
                                break;
                            default:
                                setupReturn.ErrorMessages.Add($"Invalid option parameter: {cleanArg}");
                                break;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(setupReturn.SaveFilePath))
                    {
                        setupReturn.SaveFilePath = arg;

                        if (!File.Exists(setupReturn.SaveFilePath))
                        {
                            setupReturn.ErrorMessages.Add($"Invalid [save_name] file/path: {setupReturn.SaveFilePath}");
                        }
                    }
                }
            }
        }

        if (setupReturn.ErrorMessages.Any())
        {
            setupReturn.SetupSuccessful = false;
        }

        return setupReturn;
    }
}

public class EngineSetupReturn
{
    public List<string> ErrorMessages { get; set; }
    public string GameDatabaseFilePath { get; set; }
    public bool OutputDebugStatements { get; set; }
    public bool SetupSuccessful { get; set; }
    public string SaveFilePath { get; set; }

    public EngineSetupReturn()
    {
        ErrorMessages = new List<string>();
        GameDatabaseFilePath = string.Empty;
        SaveFilePath = string.Empty;
    }
}