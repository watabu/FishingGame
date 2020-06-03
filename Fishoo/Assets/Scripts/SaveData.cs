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
            case Season.spring: return "春";
            case Season.summer: return "夏";
            case Season.autumn: return "秋";
            case Season.winter: return "冬";
            default: return "";
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
    [SerializeField]
    List<FishData> m_fishes = new List<FishData>();
    /// <summary>
    /// デバッグ用の最初から追加される魚
    /// </summary>
    [SerializeField, Tooltip("デバッグ用")]
    List<Fish.FishInfo> firstAddedfishes = new List<Fish.FishInfo>();

    [System.Serializable]
    public class FishData
    {
        public Fish.FishInfo fish;
        public int count;
    }

    Dictionary<Fish.FishInfo, FishData> fishData ;

    /// <summary>
    /// 今までに取ったことのある魚
    /// </summary>
    public Dictionary<Fish.FishInfo,FishData> fishes { 
        get {
            if (fishData == null) InitializeData();
            return fishData;
        }
    }

    public void InitializeData() { 
        fishData=new Dictionary<Fish.FishInfo, FishData>();
        foreach(var i in m_fishes)
        {
            if (i == null||i.fish==null) continue;
            fishData.Add(i.fish,i);
        }
    }

    public bool isCaught(Fish.FishInfo  fish)
    {
        return fishes.ContainsKey(fish);
    }

    public void DebugAddFishes()
    {
        if (Debug)
        {
            foreach (var fish in firstAddedfishes)
            {
                AddFish(fish);
            }
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(Fish.FishInfo fish_)
    {
        if (fish_ == null) return;
        foreach (var i in m_fishes)
        {
            if (isCaught(fish_))
            {
                i.count++;
                m_caughtFishCount++;
                return;
            }
        }
        var f = new FishData { fish = fish_, count = 1 };
        m_fishes.Add(f);
        fishes.Add(fish_, f);
    }

}
