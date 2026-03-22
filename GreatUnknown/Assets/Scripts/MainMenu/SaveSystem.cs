using UnityEngine;

public static class SaveSystem 
// static class don't need to attach to GameObject
{
    public static void SaveProgress(int daysCompleted)
    {
        PlayerPrefs.SetInt("masterDaysCompleted", daysCompleted);
        PlayerPrefs.Save();
    }
    public static int LoadProgress()
    {
        // returns 0 as default if crashes
        return PlayerPrefs.GetInt("masterDaysCompleted", 0);
    }
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey("masterDaysCompleted");
        PlayerPrefs.Save();
    }
        
}
