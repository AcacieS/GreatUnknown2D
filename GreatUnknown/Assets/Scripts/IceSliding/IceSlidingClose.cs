using Unity.VisualScripting;
using UnityEngine;

public class IceSlidingClose : MonoBehaviour
{
    [SerializeField] private GameObject iceGameParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if(iceGameParent == null)
        {
            iceGameParent = Utilities.FindParentWithTag(this.gameObject, "IceSlidingGame");
            if(iceGameParent ==null) Debug.Log("Invalid Tag");
        }
    }


    public void CloseButton(){
        if (GameManagement.Instance ==null) iceGameParent.SetActive(false);
        GameManagement.Instance.ExitSlidingGame();
    }






    // Update is called once per frame
}
