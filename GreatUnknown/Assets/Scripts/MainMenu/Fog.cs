using UnityEngine;
using UnityEngine.UI;

public class FogEffect : MonoBehaviour
{
    public float speedX = 0.02f;
    public float speedY = 0.01f;
    public int textureSize = 256;
    public float noiseScale = 5f;
    public Color fogColor = new Color(1f, 1f, 1f, 0.3f);

    private RawImage fogImage;

    void Start()
    {
        fogImage = GetComponent<RawImage>();
        fogImage.texture = GenerateNoiseTexture();
        fogImage.color = fogColor;
    }

    void Update()
    {
        fogImage.uvRect = new Rect(
            fogImage.uvRect.x + speedX * Time.deltaTime,
            fogImage.uvRect.y + speedY * Time.deltaTime,
            fogImage.uvRect.width,
            fogImage.uvRect.height
        );
    }

    Texture2D GenerateNoiseTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);
        texture.wrapMode = TextureWrapMode.Repeat; // makes it tile seamlessly

        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                float noiseValue = Mathf.PerlinNoise(
                    x / (float)textureSize * noiseScale,
                    y / (float)textureSize * noiseScale
                );
                texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue, noiseValue));
            }
        }

        texture.Apply();
        return texture;
    }
}
