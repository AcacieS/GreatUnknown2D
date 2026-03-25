using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FogTime : MonoBehaviour
{
    private Material fogMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fogMaterial = GetComponent<RawImage>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (fogMaterial != null) fogMaterial.SetFloat("_FogTime", Time.unscaledTime);
    }
}
