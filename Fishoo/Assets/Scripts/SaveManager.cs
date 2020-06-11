using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{

    [SerializeField] TextAsset data;


    public new void Awake()
    {
        base.Awake();
        Load();
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        Save();
    }
    void Load()
    {
        StringBuilder sb = new StringBuilder(1024);  //※capacity は任意
        var line = data.text.Replace(" ", "").Split('\n');

        foreach(var l in line)
        {
            var dat = l.Trim().Split(','); // , 区切りでリストに追加

            if (dat.Length == 1)
            {
                Debug.LogError($"SaveData has error   {l}");
                continue;
            }
            Debug.Log($"dat[0]={dat[0]}");
            switch (dat[0])
            {
                case "week":
                    week = int.Parse(dat[1]);
                    Debug.Log($"week={dat[1]}");
                    break;
                case "money":
                    money = int.Parse(dat[1]);
                    Debug.Log($"money={dat[1]}");
                    break;
                case "canSkipTutorial":
                    canSkipTutorial = bool.Parse(dat[1]);
                    break;
                case "caughtFishCount":
                    m_caughtFishCount = int.Parse(dat[1]);
                    break;
                case "Fish":
                    m_fishes.Add(new FishData() { fishName = dat[1], count = int.Parse(dat[2]) });
                    break;
                default:
                    Debug.Log($"This propery is not exist '{dat[0]}'");
                    break;
            }
        }
    }

    void Save()
    {

        StreamWriter streamWriter = new StreamWriter("Assets/Data/saveData.csv", false);

        streamWriter.WriteLine($"week , {week}");
            streamWriter.Flush();
        streamWriter.WriteLine($"money , {money}");
            streamWriter.Flush();
        streamWriter.WriteLine($"canSkipTutorial , {canSkipTutorial}");
            streamWriter.Flush();
        streamWriter.WriteLine($"caughtFishCount , {m_caughtFishCount}");
        streamWriter.Flush();
        foreach(var f in m_fishes)
        {
            streamWriter.WriteLine($"Fish , {f.fishName} , {f.count}");
            streamWriter.Flush();
        }
    }


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
        public string fishName;
        public int count;
    }

    public GameProperty property;
    public int week;
    public int money;
    public bool canSkipTutorial = false;

    [SerializeField, Tooltip("捕まえた魚の数")]
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

    public bool isCaught(Fish.FishInfo fish) {
        for (int i = 0; i < m_fishes.Count; i++)
            if (m_fishes[i].fishName == fish.FishName) return true;
        return false;
    }
    public int GetFishCount(Fish.FishInfo fish)
    {
        for (int i = 0; i < m_fishes.Count; i++)
            if (m_fishes[i].fishName == fish.FishName) return m_fishes[i].count;
        return 0;
    }
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(Fish.FishInfo fish_)
    {
        if (fish_ == null) return;
        m_caughtFishCount++;

        if (!isCaught(fish_))
        {
            var f = new FishData { fishName = fish_.FishName, count = 1 };
            m_fishes.Add(f);
            return;
        }

        foreach (var i in m_fishes)
        {
            if(i.fishName == fish_.FishName)
            {
                i.count++;
                return;
            }
        }
        

    }
    public string GetSeasonKanji()
    {
        switch (GetSeason())
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
        switch (GetSeason())
        {
            case Season.spring: return property.spring;
            case Season.summer: return property.summer;
            case Season.autumn: return property.autumn;
            case Season.winter: return property.winter;
        }
        return property.spring;
    }

    public void AddWeek() { week++; }

    /// <summary>
    /// 現在の年数
    /// </summary>
    public int Year { get { return (week + 3) / 4; } }
    public Season GetSeason()
    {
        switch (week % 4)
        {
            case 1: return Season.spring;
            case 2: return Season.summer;
            case 3: return Season.autumn;
            case 0: return Season.winter;
        }
        return Season.spring;
    }
    public AudioClip GetSeasonBGM()
    {
        switch (week % 4)
        {
            case 1: return property.springBGM;
            case 2: return property.summerBGM;
            case 3: return property.autumnBGM;
            case 0: return property.winterBGM;
        }
        return property.springBGM;
    }

    public int GetSeasonID()
    {
        return (week+3) % 4;
    }
    public void DeleteAll()
    {
        week = 1;
        money = 0;
        m_caughtFishCount = 0;
        m_fishes.Clear();
    }
}
