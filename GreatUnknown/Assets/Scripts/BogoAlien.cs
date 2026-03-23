using UnityEngine;

public class BogoAlien : MonoBehaviour
{

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void OnClick()
    {
        audioSource.Play();
    }
}
