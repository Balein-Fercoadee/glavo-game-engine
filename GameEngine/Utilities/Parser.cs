using GameEngine.GameData;

namespace GameEngine.Utilities;

public static class Parser
{
    public static (Word? verb, Word? noun) GetWordsFromInput(string playerInput, IEnumerable<Word> words)
    {
        Word? verb = null;
        Word? noun = null;

        var splitInput = playerInput.Split(" ");

        if (splitInput.Length == 1)
        {
            Word? placeholderWord = ObjectFinder.GetWord(words, splitInput[0]);
            // if it's a verb, that could be a full command so exit from parser.
            if (placeholderWord != null && placeholderWord.WordType == WordTypes.Verb)
            {
                verb = placeholderWord;
            }
            else
            {
                if (placeholderWord != null)
                {
                    noun = placeholderWord;
                    // looks we have have a directional command, add GO as a verb and exit.
                    if (Constants.STANDARD_DIRECTIONS.Contains(noun.Name))
                    {
                        verb = ObjectFinder.GetWord(words, "go");
                    }
                    else
                    {
                        if (noun.Name == "inventory")
                        {
                            verb = ObjectFinder.GetWord(words, "look");
                        }
                    }
                }
            }
        }
        else
        {
            foreach (string input in splitInput)
            {
                Word? placeholderWord = ObjectFinder.GetWord(words, input);

                if (placeholderWord != null && placeholderWord.WordType == WordTypes.Verb)
                {
                    verb = placeholderWord;
                }
                else if (placeholderWord != null && placeholderWord.WordType == WordTypes.Noun)
                {
                    noun = placeholderWord;
                }
            }
        }

        return (verb, noun);
    }
}