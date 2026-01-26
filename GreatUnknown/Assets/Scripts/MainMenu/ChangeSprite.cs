using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    public Sprite pressedButton;
    public Button button;
    public void ChangeButtonImage()
    {
        button.image.sprite = pressedButton;
    }
    
}
