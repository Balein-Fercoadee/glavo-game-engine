using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal static class WordValidation
{
    public static string? FindDuplicateToken(IEnumerable<Word> words, Word candidate)
    {
        var existingTokens = words
            .SelectMany(word => new[] { word.Name }.Concat(word.Aliases))
            .Where(token => !string.IsNullOrWhiteSpace(token))
            .Select(token => token.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new[] { candidate.Name }.Concat(candidate.Aliases)
            .Where(token => !string.IsNullOrWhiteSpace(token))
            .Select(token => token.Trim())
            .FirstOrDefault(existingTokens.Contains);
    }
}
