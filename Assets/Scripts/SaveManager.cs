using UnityEngine;

public static class SaveManager
{
    public static void SaveScore(string levelName, int score)
    {
        PlayerPrefs.SetInt($"score_{levelName}", score);
        PlayerPrefs.Save();
    }

    public static int LoadScore(string levelName)
    {
        return PlayerPrefs.GetInt($"score_{levelName}", 0);
    }

    public static void SaveStars(string levelName, int stars)
    {
        PlayerPrefs.SetInt($"stars_{levelName}", stars);
        PlayerPrefs.Save();
    }

    public static int LoadStars(string levelName)
    {
        return PlayerPrefs.GetInt($"stars_{levelName}", 0);
    }
}
