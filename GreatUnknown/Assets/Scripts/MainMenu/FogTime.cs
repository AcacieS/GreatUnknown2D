using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FogTime : MonoBehaviour
{
    private int fogTimeProperty;

    void Awake()
    {
        fogTimeProperty = Shader.PropertyToID("_FogTime");
    }

    void Update()
    {
        Shader.SetGlobalFloat(fogTimeProperty, Time.unscaledTime);
    }
}
