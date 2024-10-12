
namespace GameEngine.Utilities;

public static class InputFormatter
{
    public static string CleanPlayerInput(string rawUserInput)
    {
        string cleanedInput = string.Empty;

        cleanedInput = rawUserInput.ToLowerInvariant().Trim();

        return cleanedInput;
    }

}