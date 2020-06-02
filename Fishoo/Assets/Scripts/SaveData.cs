using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO
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

    struct isCaughtFish
    {
        Fish.FishInfo fish;
        bool isCaught;
    }
    public Season season;
    public int week;
    public int money;
    public SaveData saveData;
    [SerializeField] bool Debug;
    /// <summary>
    /// 釣った魚の数を魚別にカウントする。
    /// </summary>
    List<isCaughtFish> EachFishCount = new List<isCaughtFish>();
    
    //public bool isCaught(Fish.FishInfo fish){ return CaughtFishDictionary.ContainsKey(fish); }

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

    [Tooltip("捕まえた魚の数")]
    int m_caughtFishCount;
    /// <summary>
    /// 捕まえた魚の合計数
    /// </summary>
    public int caughtFishCount { get { return m_caughtFishCount; } }

    /// <summary>
    /// 釣ったことのある魚のリスト
    /// </summary>
    [SerializeField,ReadOnly]
    List<Fish.FishInfo> m_fishes= new List<Fish.FishInfo>();
    /// <summary>
    /// デバッグ用の最初から追加される魚
    /// </summary>
    [SerializeField,Tooltip("デバッグ用")]
    List<Fish.FishInfo> firstAddedfishes= new List<Fish.FishInfo>();

    /// <summary>
    /// 今までに取ったことのある魚
    /// </summary>
    public List<Fish.FishInfo> fishes { get { return m_fishes; } }


    public void DebugAddFishes() { 
        if (Debug)
        {
            foreach(var fish in firstAddedfishes)
            {
                AddFish(fish);
            }
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fish"></param>
    public  void AddFish(Fish.FishInfo fish)
    {
        if (!m_fishes.Contains(fish))
        {
            m_fishes.Add(fish);
        }

        m_caughtFishCount++;
    }

}
