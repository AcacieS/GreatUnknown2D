using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DisarmPasscode : MonoBehaviour
{
    [SerializeField] private Button button;
    [FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI lockedButtonLabel;
    [SerializeField] private GameObject passcodeObject;
    [FormerlySerializedAs("passcodeText")] [SerializeField] private TextMeshProUGUI passcodeTextInputField;
    [SerializeField] private string passcode;

    void OnEnable()
    {
        button.interactable = false;
        lockedButtonLabel.gameObject.SetActive(false);
        passcodeObject.SetActive(true);
    }

    void Update()
    {
        if (passcodeTextInputField.text.Contains(passcode))
        {
            button.interactable = true;
            lockedButtonLabel.gameObject.SetActive(true);
            passcodeObject.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(passcodeObject);
    }
}
