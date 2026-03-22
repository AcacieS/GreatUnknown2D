using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "WrongMessages", menuName = "Wrong Message Scriptable/WrongMessages")]
public class WrongMessages: ScriptableObject
{
    public int day;
    public WrongMessage message;
    public Sprite GetMessage(int nbWrong)
    {
        switch (nbWrong)
        {
            case 1:
                return message.wrong1;
            case 2:
                return message.wrong1;
            case 3:
                return message.wrong1;
        }
        return null;
    }
}