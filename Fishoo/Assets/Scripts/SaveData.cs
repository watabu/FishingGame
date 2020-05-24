using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SaveData", order = 1)]
public class SaveData : ScriptableObject
{
    public enum Season
    {
        spring,
        summer,
        autumn,
        winter
    }

    public Season season;
    public int week;
    public int money;
    public string GetSeasonKanji()
    {
        switch (season)
        {
            case Season.spring:return "春";
            case Season.summer: return "夏";
            case Season.autumn: return "秋";
            case Season.winter: return "冬";
            default:return "";
        }
    }
}
