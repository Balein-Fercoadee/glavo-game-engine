using GameEngine.GameData;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace GameDatabaseEditor.Utilities
{
    public static class GeneralUtilities
    {
        private static string sacredWordJson = "[\r\n{\r\n\"Aliases\": [\r\n\"enter\"\r\n],\r\n\"Id\": 0,\r\n\"Name\": \"go\",\r\n\"WordType\": 2\r\n},\r\n{\r\n\"Aliases\": [\r\n\"l\",\r\n\"examine\",\r\n\"read\"\r\n],\r\n\"Id\": 1,\r\n\"Name\": \"look\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"n\"\r\n    ],\r\n    \"Id\": 2,\r\n    \"Name\": \"north\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"s\"\r\n    ],\r\n    \"Id\": 3,\r\n    \"Name\": \"south\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"e\"\r\n    ],\r\n    \"Id\": 4,\r\n    \"Name\": \"east\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"w\"\r\n    ],\r\n    \"Id\": 5,\r\n    \"Name\": \"west\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"u\"\r\n    ],\r\n    \"Id\": 6,\r\n    \"Name\": \"up\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"d\"\r\n    ],\r\n    \"Id\": 7,\r\n    \"Name\": \"down\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"i\",\r\n      \"inv\"\r\n    ],\r\n    \"Id\": 8,\r\n    \"Name\": \"inventory\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"h\"\r\n    ],\r\n    \"Id\": 9,\r\n    \"Name\": \"help\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"q\"\r\n    ],\r\n    \"Id\": 10,\r\n    \"Name\": \"quit\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"get\"\r\n    ],\r\n    \"Id\": 11,\r\n    \"Name\": \"take\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"leave\"\r\n    ],\r\n    \"Id\": 12,\r\n    \"Name\": \"drop\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [\r\n      \"everything\"\r\n    ],\r\n    \"Id\": 13,\r\n    \"Name\": \"all\",\r\n    \"WordType\": 1\r\n  },\r\n  {\r\n    \"Aliases\": [],\r\n    \"Id\": 14,\r\n    \"Name\": \"score\",\r\n    \"WordType\": 2\r\n  },\r\n  {\r\n    \"Aliases\": [],\r\n    \"Id\": 15,\r\n    \"Name\": \"any\",\r\n    \"WordType\": 1\r\n  }\r\n]";

        /// <summary>
        /// Gets a list of the base 16 'sacred' words. These words are the only words that the GGE understands innately.
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Word> GetSacredWords()
        {
            return JsonSerializer.Deserialize<ObservableCollection<Word>>(sacredWordJson);
        }

        /// <summary>
        /// Given a list of <c>IIdentifiable</c> objects, this method returns the lowest integer available for an id.
        /// </summary>
        /// <param name="identifiableList"></param>
        /// <returns></returns>
        public static int GetIdNumber(List<IIdentifiable> identifiableList)
        {
            List<int> idList = identifiableList.Select(i => i.Id).ToList();

            return GameEngine.Utilities.GeneralUtilities.GetNextIdNumber(idList);
        }

        public static bool IsDatabaseEmpty(GameDatabase database)
        {
            bool isFilled = false;
            int countOfObjects = 0;

            if (database != null)
            {
                countOfObjects = database.Actions.Count + database.Items.Count + database.Messages.Count + database.Rooms.Count + database.Words.Count;
            }

            isFilled |= countOfObjects > 16;
            isFilled |= database?.Description.Length > 0;
            isFilled |= database?.Name.Length > 0;
            isFilled |= database?.Description.Length > 0;
            isFilled |= database?.StartingRoomId > -1;
            isFilled |= database?.TreasureRoomId > -1;

            return !isFilled;
        }

        /// <summary>
        /// Creates a new <c>GameDatabase</c> that contains only the 'sacred' <c>Words</c>.
        /// </summary>
        /// <returns></returns>
        public static GameDatabase GetFreshDatabase()
        {
            var db = new GameDatabase();
            db.Words = GetSacredWords();

            return db;
        }
    }
}
