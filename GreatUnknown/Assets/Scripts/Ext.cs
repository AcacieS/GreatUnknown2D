using UnityEngine;

public static class Ext
{
    public static void Fatal(string reason, MonoBehaviour script)
    {
        Debug.LogError(reason);
        script.gameObject.SetActive(false);
    }

    public static void Warning(string reason, MonoBehaviour script)
    {
        Debug.LogWarning(reason);
    }
}