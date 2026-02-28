using UnityEngine;

[CreateAssetMenu(menuName = "Fax/Fax Print Job")]
public class FaxPrintJob : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] private string body;

    public Sprite Sprite => sprite;
    public string Header => header;
    public string Body => body;
}