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
                if (args.Length > 1 && args[1].Length > 2)
                {
                    setupReturn.SaveFilePath = args[1].Trim();

                    if (!File.Exists(setupReturn.SaveFilePath))
                    {
                        setupReturn.ErrorMessages.Add($"Invalid [save_name] file/path: {setupReturn.SaveFilePath}");
                    }
                    else
                    {
                        // Loop through the args for any optional paramters
                        foreach (string arg in args.Where(a => a.StartsWith('-')))
                        {
                            string cleanArg = arg.Replace("-", string.Empty);

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