using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FogTime : MonoBehaviour
{
    private Material fogMaterial;

    void Start()
    {
        RawImage rawImage = GetComponent<RawImage>();
        fogMaterial = new Material(rawImage.material);
        rawImage.material = fogMaterial;
    }

    void Update()
    {
        if (fogMaterial != null) fogMaterial.SetFloat("_FogTime", Time.unscaledTime);
    }
}
