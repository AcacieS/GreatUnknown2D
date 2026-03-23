using UnityEngine;

public static class Ext
{
    public static void WarnRefAndDisable(string field, MonoBehaviour script)
    {
        Debug.LogError("[" + script.name + "] Missing " + field + " Reference");
        script.gameObject.SetActive(false);
    }

    public static void WarnRef(string field, MonoBehaviour script)
    {
        Debug.LogWarning("[" + script.name + "] Missing " + field + " Reference (optional)");
    }

    public static void WarnAndDisable(string what, MonoBehaviour script)
    {
        Debug.LogError("[" + script.name + "] " + what);
        script.gameObject.SetActive(false);
    }

    public static void Warn(string what, MonoBehaviour script)
    {
        Debug.LogWarning("[" + script.name + "] " + what);
    }
}
