using System;

namespace GameEngine.Utilities;

public static class GeneralUtilities
{

    /// <summary>
    /// Given a list of integers, this method returns the lowest integer available for an id.
    /// </summary>
    /// <param name="listOfInt"></param>
    /// <returns></returns>
    public static int GetNextIdNumber(List<int> listOfInt)
    {
        int count = listOfInt.Count();
        int idNumber = count;

        if (count == 0)
        {
            idNumber = 0;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int currentId = listOfInt[i];

                // if i is less than currentId, that's the id to use
                if (currentId > i)
                {
                    idNumber = i;
                    break;
                }
            }
        }

        return idNumber;
    }
}
