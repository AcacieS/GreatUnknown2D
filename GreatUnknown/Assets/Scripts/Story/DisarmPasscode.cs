using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisarmPasscode : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject passcodeObject;
    [SerializeField] private TextMeshProUGUI passcodeText;
    [SerializeField] private string passcode;

    void OnEnable()
    {
        button.interactable = false;
        text.gameObject.SetActive(false);
        passcodeObject.SetActive(true);
    }

    void Update()
    {
        if (passcodeText.text.Contains(passcode))
        {
            button.interactable = true;
            text.gameObject.SetActive(true);
            passcodeObject.SetActive(false);
        }
    }
}
