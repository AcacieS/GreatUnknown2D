using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmergencyLight : MonoBehaviour
{
    public float maxIntensity = 2.0f;
    public float minIntensity = 0.0f;
    public float intervalTime = 0.1f;
    private Light2D emergencyLight;
    private float timer = 0f;
    bool isOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emergencyLight = GetComponent<Light2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (emergencyLight == null)
        {
            Debug.LogError("Emergency Light is not assigned!", this);
            return; 
        }
        timer += Time.deltaTime;
        if (timer > intervalTime)
        {
            isOn = !isOn;
            if (isOn)
            {
                emergencyLight.intensity = maxIntensity;
            }
            else
            {
                emergencyLight.intensity = minIntensity; 
            }
            timer = 0f;
        }
    }
}
