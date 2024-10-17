
namespace GameEngine.Utilities;

public static class InputFormatter
{
    /// <summary>
    /// Takes a user's input and ensures it's 'clean'. (i.e. not null, no leading/trailing spaces, all lowercase)
    /// </summary>
    /// <param name="rawUserInput"></param>
    /// <returns></returns>
    public static string CleanPlayerInput(string rawUserInput)
    {
        if(string.IsNullOrWhiteSpace(rawUserInput))
            rawUserInput=string.Empty;

        string cleanedInput = rawUserInput.ToLowerInvariant().Trim();

        return cleanedInput;
    }

}