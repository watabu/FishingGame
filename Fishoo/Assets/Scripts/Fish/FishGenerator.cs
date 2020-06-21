using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    /// <summary>
    /// Playerで釣り竿が投げられるーFishingGameMgrで魚が生成される
    /// 魚は一直線に針へ向かう
    /// CommonFishが針に十分近づいたことを検出したら釣り開始
    /// </summary>
    public class FishGenerator : MonoBehaviour
    {
        [SerializeField] private Environment.Place place;
        [Tooltip("魚を生成する座標オブジェクトをまとめている親オブジェクト")]
        [SerializeField] private Transform generatorsParent;
        [SerializeField] Fish.Behavior.CommonFish FishPrefab;
        [SerializeField] Sprite fishR;
        [SerializeField] Sprite fishSR;
        [SerializeField] Sprite fishSSR;

        [SerializeField] bool debug;

        List<Environment.PlaceData.FishGenerateData> availableFishList;

        [SerializeField,ReadOnly]List<Environment.PlaceData.FishGenerateData> fishList_R = new List<Environment.PlaceData.FishGenerateData>();
        [SerializeField, ReadOnly] List<Environment.PlaceData.FishGenerateData> fishList_SR = new List<Environment.PlaceData.FishGenerateData>();
        [SerializeField, ReadOnly] List<Environment.PlaceData.FishGenerateData> fishList_SSR = new List<Environment.PlaceData.FishGenerateData>();

        //魚を生成する座標
        List<Transform> generators = new List<Transform>();

        [SerializeField]GameObject testfish;
        
        private int bottomScore;
        // Start is called before the first frame update
        void Start()
        {
            bottomScore = 0;
            availableFishList = place.placeData.availableFishList;
            foreach (var fish in availableFishList)
            {
                switch (fish.fishInfo.rank)
                {
                    case FishInfo.Rank.R:
                        fishList_R.Add(fish);
                        break;
                    case FishInfo.Rank.SR:
                        fishList_SR.Add(fish);
                        break;
                    case FishInfo.Rank.SSR:
                        fishList_SSR.Add(fish);
                        break;
                    default:
                        Debug.LogError("Error: fishInfo is strange.");
                        break;
                }
            }


            foreach (Transform child in generatorsParent)
            {
                generators.Add(child);
            }
        }

        //場所の設定などから魚を一体生成し、オブジェクトを返す
        public GameObject GenerateFish()
        {
            //生成する座標をリストからランダムで選択
            var pos = generators[Random.Range(0, generators.Count)].position;
            FishInfo selectedFish;
            if (debug)
            {
                GameObject fishObj = Instantiate(testfish, pos, Quaternion.identity);
                fishObj.GetComponent<Fish.Behavior.CommonFish>().InitData_Debug();
                return fishObj;
            }
            else {
                var selectedFishList = SelectFishList();
                if (selectedFishList.Count == 0)
                {
                    Debug.LogError("PlaceDataに設定されている魚のランクのうちどれかが0です。PlaceDataの魚のレア度の変更を検討してください");
                }

                selectedFish = selectedFishList[Random.Range(0, selectedFishList.Count)].fishInfo;

                GameObject fishObj = Instantiate(FishPrefab.gameObject, pos, Quaternion.identity);
                fishObj.GetComponent<Behavior.CommonFish>().InitData(selectedFish, GetShadowSprite(selectedFish.rank));


                return fishObj;
            }
        }

        /// <summary>
        /// 年ごとに魚のレア度の排出率を変える。（変わってない)
        /// </summary>
        /// <returns></returns>
        List<Environment.PlaceData.FishGenerateData> SelectFishList()
        {
            bottomScore++;
            //int score = Random.Range(0, 100) + bottomScore; //だんだんSレア排出率が上がる計算式
            int score = Random.Range(0, 100);

            var rank = GetRank(score, 3);
            Debug.Log(rank);
            while (true)
            {
                switch (rank)
                {
                    case FishInfo.Rank.R:
                        if (fishList_R.Count == 0)
                        {
                            rank = FishInfo.Rank.SR;
                            continue;
                        }
                        return fishList_R;
                    case FishInfo.Rank.SR:
                        if (fishList_SR.Count == 0)
                        {
                            rank = FishInfo.Rank.SSR;
                            continue;
                        }
                        bottomScore = 0;
                        return fishList_SR;
                    case FishInfo.Rank.SSR:
                        if (fishList_SSR.Count == 0)
                        {
                            return null;
                        }
                        bottomScore = 0;
                        return fishList_SSR;
                }
            }
        }

        /// <summary>
        /// 0~99のスコアと年数からレア度を決めて魚のリストを返す
        /// </summary>
        /// <param name="score"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        FishInfo.Rank GetRank(int score, int year)
        {
            if (score < 70) return FishInfo.Rank.R;
            if (score < 92)  return FishInfo.Rank.SR;
            return FishInfo.Rank.SSR;
            //if (year < 2)
            //{
            //    if (score < 90)
            //        return FishInfo.Rank.R;
            //    return FishInfo.Rank.SR;
            //}
            //if (year < 3)
            //{
            //    if (score < 75)
            //        return FishInfo.Rank.R;
            //    if (score < 95)
            //        return FishInfo.Rank.SR;
            //    return FishInfo.Rank.SSR;
            //}
            //if (score < 60)
            //    return FishInfo.Rank.R;
            //if (score < 85)
            //    return FishInfo.Rank.SR;
            //return FishInfo.Rank.SSR;
        }
    


        /// <summary>
        /// レア度に応じた魚影を返す
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public Sprite GetShadowSprite(FishInfo.Rank rank)
        {
            switch (rank)
            {
                case FishInfo.Rank.R:
                    return fishR;
                case FishInfo.Rank.SR:
                    return fishSR;
                case FishInfo.Rank.SSR:
                    return fishSSR;
            }
            return fishR;
        }
    }
}