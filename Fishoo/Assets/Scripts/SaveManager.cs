using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    [Header("save file")]
    [SerializeField] string directory = "Data";
    [SerializeField] string filename = "saveData.csv";

    public new void Awake()
    {
        base.Awake();
        //場所の初期化
        var availablePlaces = Resources.LoadAll<Environment.PlaceData>("Places");

        placeDatas.Clear();
        foreach (var place in availablePlaces)
        {
            //釣りステージならランキングデータを初期化
            placeDatas.Add(place);
            if (place.isFishingStage)
            {
                Debug.Log(place.placeName);
                place.ResetRanking();
            }
        }
        Load();
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        Save();
    }
    void Load()
    {

      if(!CSVReader.Exists(directory, filename))
        {
            Debug.Log("File does not exist");
            week = 1;
            money = 0;
            canSkipTutorial = false;
            m_caughtFishCount = 0;
            m_fishes.Clear();
            return;
        }
        var dat = CSVReader.Read(directory, filename);

        foreach (var l in dat)
        {
            Debug.Log($"dat[0]='{l[0]}'");
            switch (l[0])
            {
                case "week":
                    week = int.Parse(l[1]);
                    Debug.Log($"week={l[1]}");
                    break;
                case "money":
                    money = int.Parse(l[1]);
                    Debug.Log($"money={l[1]}");
                    break;
                case "canSkipTutorial":
                    canSkipTutorial = bool.Parse(l[1]);
                    break;
                case "caughtFishCount":
                    m_caughtFishCount = int.Parse(l[1]);
                    break;
                case "Fish":
                    m_fishes.Add(new FishData() { fishName = l[1], count = int.Parse(l[2]) });
                    break;
                case "Ranking":
                    InsertRecord(int.Parse(l[1]), int.Parse(l[2]), int.Parse(l[3]));
                    break;
                default:
                    Debug.Log($"This propery is not exist '{l[0]}'");
                    break;
            }
        }
    }

    public void Save()
    {
        if (!CSVReader.Exists(directory, filename))
        {
            CSVReader.CreateFile(directory, filename);
        }

        var dat = new List<List<string>>();
        dat.Add(new List<string>() { "week", week.ToString() });
        dat.Add(new List<string>() { "money", money.ToString() });
        dat.Add(new List<string>() { "canSkipTutorial", canSkipTutorial.ToString() });
        dat.Add(new List<string>() { "caughtFishCount", m_caughtFishCount.ToString() });
        foreach (var f in m_fishes)
        {
            dat.Add(new List<string>() { "Fish", f.fishName, f.count.ToString() });
        }
        foreach (var place in placeDatas)
        {
            if (!place.isFishingStage) continue;
            foreach (var rec in place.rankingSaveData.ranking)
            {
                if (rec.name != playerName) continue;
                Debug.Log(place.placeName + " " + rec.fishCount + " " + rec.score);
                dat.Add(new List<string>() { "Ranking", place.id.ToString(), rec.fishCount.ToString(), rec.score.ToString() });
            }
        }
        CSVReader.Write(directory, filename, dat);
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

    [Header("property")]
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
    [Tooltip("ランキングで表示されるプレイヤーのアイコン")]public Sprite playerIcon;
    [Tooltip("ランキングで表示されるプレイヤーの名前")] public string playerName;
    /// <summary>
    /// ランキングデータの保存用
    /// </summary>
    [SerializeField]List<Environment.PlaceData> placeDatas = new List<Environment.PlaceData>();


    public bool isCaught(Fish.FishInfo fish) {
        for (int i = 0; i < m_fishes.Count; i++)
            if (m_fishes[i].fishName == fish.fishName) return true;
        return false;
    }
    public int GetFishCount(Fish.FishInfo fish)
    {
        for (int i = 0; i < m_fishes.Count; i++)
            if (m_fishes[i].fishName == fish.fishName) return m_fishes[i].count;
        return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fish"></param>
    public void AddFish(Fish.FishInfo fish_)
    {
        if (fish_ == null) return;
        m_caughtFishCount++;

        if (!isCaught(fish_))
        {
            var f = new FishData { fishName = fish_.fishName, count = 1 };
            m_fishes.Add(f);
            return;
        }

        foreach (var i in m_fishes)
        {
            if(i.fishName == fish_.fishName)
            {
                i.count++;
                return;
            }
        }
        

    }

    private void InsertRecord(int placeID, int fishCount, int score)
    {
        if(placeID == 0)
        {
            Debug.LogError("placeIDの0は非使用です。設定しなおしてください。");
        }
        RankingSaveData.Record record;
        record.icon = playerIcon;
        record.name = playerName;
        record.fishCount = fishCount;
        record.score = score;
        foreach (var place in placeDatas)
        {
            if(place.id == placeID)
            {
                place.rankingSaveData.InsertRecord(record);
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
        canSkipTutorial = false;
        foreach (var place in placeDatas)
        {
            //釣りステージならランキングデータを初期化
            if (place.isFishingStage)
            {
                Debug.Log(place.placeName);
                place.ResetRanking();
            }
        }
        Save();
    }
}
