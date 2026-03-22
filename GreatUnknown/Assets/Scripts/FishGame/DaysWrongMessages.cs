using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "DaysWrongMessages", menuName = "Wrong Message Scriptable/DaysWrongMessages")]
public class DaysWrongMessages: ScriptableObject
{
    public List<WrongMessages> listWrongMessages;
    public Sprite GetMessage(int day, int nbWrong)
    {
        WrongMessages currentWrongMessages = null;
        foreach(WrongMessages wrongMessages in listWrongMessages)
        {
            if (day >= wrongMessages.day)
            {
                currentWrongMessages = wrongMessages;
            }
        }
        if(currentWrongMessages == null) return null;

        switch (nbWrong)
        {
            case 1:
                return currentWrongMessages.message.wrong1;
            case 2:
                return currentWrongMessages.message.wrong2;
            case 3:
                return currentWrongMessages.message.wrong3;
        }

        return null;
    }
}