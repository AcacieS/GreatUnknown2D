using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FogTime : MonoBehaviour
{
    private Material fogMaterial;

    void Start()
    {
        fogMaterial = GetComponent<RawImage>().material;
    }

    void Update()
    {
        if (fogMaterial != null) fogMaterial.SetFloat("_FogTime", Time.unscaledTime);
    }
}
