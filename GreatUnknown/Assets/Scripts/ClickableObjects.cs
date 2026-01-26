using UnityEngine;

public class Codex : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject bookViewerUI;

    public void ToggleBook()
    {
    bookViewerUI.SetActive(!bookViewerUI.activeSelf);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
