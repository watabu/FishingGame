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
    [System.Serializable]
    public class FishData
    {
        public Fish.FishInfo fish;
        public int count;
    }


    public Season season;
    public int week;
    public int money;
    [SerializeField] bool Debug;
    public bool canSkipTutorial=false;

    [SerializeField]Color springColor;
    [SerializeField] Color summerColor;
    [SerializeField] Color autumnColor;
    [SerializeField] Color winterColor;

    [SerializeField,Tooltip("捕まえた魚の数")]
    int m_caughtFishCount;

    /// <summary>
    /// 捕まえた魚の合計数
    /// </summary>
    public int caughtFishCount { get { return m_caughtFishCount; } }

    /// <summary>
    /// 釣ったことのある魚のリスト
    /// </summary>
    [SerializeField]
    [Header("Fish Data")]
    List<FishData> m_fishes = new List<FishData>();




    /// <summary>
    /// デバッグ用の最初から追加される魚
    /// </summary>
    [SerializeField, Tooltip("デバッグ用")]
    List<Fish.FishInfo> firstAddedfishes = new List<Fish.FishInfo>();
    
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
        //if (fishData.Count == 0)
        //    AddFish(firstAddedFish);
    }

    public bool isCaught(Fish.FishInfo fish) { return fishes.ContainsKey(fish); }

    public void DebugAddFishes()
    {
        if (!Debug) return;
        foreach (var fish in firstAddedfishes)
            AddFish(fish);
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(Fish.FishInfo fish_)
    {
        if (fish_ == null) return;
        m_caughtFishCount++;
        
        foreach (var i in m_fishes)
        {
            if (isCaught(fish_))
            {
                i.count++;
                return;
            }
        }
        var f = new FishData { fish = fish_, count = 1 };
        m_fishes.Add(f);
        fishes[fish_]= f;
        //fishes.Add(fish_, f);

    }
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
    public Color GetSeasonColor()
    {
        switch (season)
        {
            case Season.spring: return springColor;
            case Season.summer: return summerColor;
            case Season.autumn: return autumnColor;
            case Season.winter: return winterColor;
        }
        return springColor;
    }

    public void AddWeek()
    {
        week++;
        switch (week % 4)
        {
            case 1: season = Season.spring;
                break;
            case 2: season = Season.summer;
                break;
            case 3: season = Season.autumn;
                break;
            case 0: season = Season.winter;
                break;
        }
    }

    /// <summary>
    /// 現在の年数
    /// </summary>
    public int Year { get { return (week+3) / 4 ; }}
    
    public void DeleteAll()
    {
        season = Season.spring;
        week = 0;
        money = 0;
        m_caughtFishCount = 0;
        m_fishes.Clear();
        fishData.Clear();

    }
}
