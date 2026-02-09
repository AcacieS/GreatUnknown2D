using System;
using UnityEngine;
[Serializable]
public class Restrictions
{
    [SerializeField] private bool day1;
    [SerializeField] private bool day2;
    [SerializeField] private bool day3;
    [SerializeField] private bool day4;
    [SerializeField] private bool day5;
    public bool getDay(int day)
    {
        switch (day)
        {
            case 1:
                return day1;
            case 2:
                return day2;
            case 3:
                return day3;
            case 4:
                return day4;
            case 5:
                return day5;
            default:
                Debug.LogError("not available day");
                return false;
        }
    }

}
