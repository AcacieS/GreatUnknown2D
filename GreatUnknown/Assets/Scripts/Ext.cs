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
}