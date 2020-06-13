using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    
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
        var path = Application.dataPath + "/" + "saveData.csv";
        //var path = Application.dataPath + "/" + "saveData2";

        //魚文の文字数(週や金の分70文字, 魚の分16文字*60種類, ランキングデータの分(30文字*15行*3ステージ) ~= 2500 < 4048
        StringBuilder sb = new StringBuilder(4048);  //※capacity は任意
        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            using (File.Create(path))
            {
                week = 1;
                money = 0;
                canSkipTutorial = false;
                m_caughtFishCount = 0;
                m_fishes.Clear();
                return;
            }
        }
        

        string[] line = File.ReadAllLines(path);
        foreach (var l in line)
        {
            var command = l.Replace(" ", "").Replace("　", "");
            var dat = command.Trim().Split(','); // , 区切りでリストに追加

            Debug.Log($"dat[0]='{dat[0]}'");
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
                case "Ranking":
                    InsertRecord(int.Parse(dat[1]), int.Parse(dat[2]), int.Parse(dat[3]));
                    break;
                default:
                    Debug.Log($"This propery is not exist '{dat[0]}'");
                    break;
            }
        }
    }

   public  void Save()
    {
        var path = Application.dataPath + "/" + "saveData.csv";
        //var path = Application.dataPath + "/" + "saveData2";
        FileInfo fi = new FileInfo(path);
        Debug.Log(path);
        using (StreamWriter sw = fi.CreateText())
        {
            sw.WriteLine($"week , {week}");
            sw.Flush();
            sw.WriteLine($"money , {money}");
            sw.Flush();
            sw.WriteLine($"canSkipTutorial , {canSkipTutorial}");
            sw.Flush();
            sw.WriteLine($"caughtFishCount , {m_caughtFishCount}");
            sw.Flush();
            foreach (var f in m_fishes)
            {
                sw.WriteLine($"Fish , {f.fishName} , {f.count}");
                sw.Flush();
            }
            foreach(var place in placeDatas)
            {
                if (!place.isFishingStage) continue;
                foreach(var rec in place.rankingSaveData.ranking)
                {
                    if (rec.name != playerName)
                        continue;
                    Debug.Log(place.placeName + " " + rec.fishCount + " " + rec.score);
                    sw.WriteLine($"Ranking , {place.id} , {rec.fishCount} , {rec.score}");
                    sw.Flush();
                }
            }
        }
      /*  Debug.Log(path);
        StreamWriter streamWriter = new StreamWriter(path, false);

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
        }*/
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
    [Tooltip("ランキングで表示されるプレイヤーのアイコン")]public Sprite playerIcon;
    [Tooltip("ランキングで表示されるプレイヤーの名前")] public string playerName;
    /// <summary>
    /// ランキングデータの保存用
    /// </summary>
    [SerializeField]List<Environment.PlaceData> placeDatas = new List<Environment.PlaceData>();


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
    /// 
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
