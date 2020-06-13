using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Environment
{
    [CreateAssetMenu(menuName = "Data/PlaceData", order = 1)]
    public class PlaceData : ScriptableObject
    {
        [System.Serializable]
        public class FishGenerateData
        {
            public Fish.FishInfo fishInfo;
            [Tooltip("unused")]
            public Weather weather;
            //釣れる時刻
            [Tooltip("unused")]
            public int beginTime;
            [Tooltip("unused")]
            public int endTime;
        }

        public enum Weather
        {
            Sunny,
            Cloudy,
            Rainy,
            Snowy,
            Windy,
        }
        [Tooltip("釣りを行う場所か")]
        public bool isFishingStage;
        [Tooltip("セーブで使われるID")]
        public int id;
        [Tooltip("場所の名前"), TextArea]
        public string placeName;
        [Tooltip("場所の説明（地図での）"), TextArea]
        public string description;
        [Tooltip("場所の詳細な説明（図鑑とか）")]
        public string description_detail;
        [Tooltip("何円以上持ってれば移動可能か")]
        public int moneyCondition;
        [Tooltip("何週目から移動可能か")]
        public int weekCondition;
        [Tooltip("場所のアイコン")]
        public Sprite icon;
        [Tooltip("開始時刻(分)")]
        public int startTime;
        [Tooltip("終了時刻(分)")]
        public int endTime;
        [Tooltip("時間ごとの環境光")]
        public Gradient globalColor = new Gradient();

        [Header("Weather")]
        //天候に関するデータ
        //とりあえず降水確率だけ
        [Range(0,1.0f)]
        [Tooltip("降水確率")]
        public float rainyPercent;

        [Tooltip("雨が何分続くか(分)")]
        public int rainDuration=120;
        [Tooltip("その場所の背景")]
        public GameObject backGroundPrefab;
        [SerializeField,Tooltip("ランキングデータの原型")]
        private RankingSaveData rankingSaveData_Origin;
        [SerializeField,Tooltip("ランキングデータ")]
        public RankingSaveData rankingSaveData;

        [SerializeField, ReadOnly] int fishCount_R;
        [SerializeField, ReadOnly] int fishCount_SR;
        [SerializeField, ReadOnly] int fishCount_SSR;

        [Header("Available fish list")]
        //その場所で釣れる魚たち
        //時間ごと、天候ごとに変更したい
        [Tooltip("その場所で釣れる魚たち")]
        public List<FishGenerateData> availableFishList;

        private void OnEnable()
        {
            Dictionary<Fish.FishInfo.Rank, int> fishCount_dic = new Dictionary<Fish.FishInfo.Rank, int>();
            fishCount_dic.Add(Fish.FishInfo.Rank.R, 0);
            fishCount_dic.Add(Fish.FishInfo.Rank.SR, 0);
            fishCount_dic.Add(Fish.FishInfo.Rank.SSR, 0);

            foreach(var fish in availableFishList)
            {
                if (fish == null) continue;
                fish.fishInfo.usedCount++;
                fishCount_dic[fish.fishInfo.rank] += 1;
            }
            fishCount_R = fishCount_dic[Fish.FishInfo.Rank.R];
            fishCount_SR = fishCount_dic[Fish.FishInfo.Rank.SR];
            fishCount_SSR = fishCount_dic[Fish.FishInfo.Rank.SSR];
        }


        public int AchievementRate()
        {
            var info = AchievementInfo();
            if (info.Item2 == 0)
                return 100;
            return info.Item1 * 100 / info.Item2;
        }

        public (int,int) AchievementInfo()
        {
            var data = SaveManager.Instance;
            int caughtFishCount = 0;
            foreach (var fish in availableFishList)
            {
                caughtFishCount += (data.isCaught(fish.fishInfo) ? 1 : 0);
            }
            return (caughtFishCount, availableFishList.Count);
        }
        public void ResetRanking()
        {
            rankingSaveData.ranking = new List<RankingSaveData.Record>(rankingSaveData_Origin.ranking);
            rankingSaveData.maxRanking = rankingSaveData_Origin.maxRanking;
        }
    }
}