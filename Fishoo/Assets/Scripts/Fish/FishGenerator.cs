using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{

    public class FishGenerator : MonoBehaviour
    {
        [SerializeField] private Environment.Place place;
        [Tooltip("魚を生成する座標オブジェクトをまとめている親オブジェクト")]
        [SerializeField] private Transform generatorsParent;
        [SerializeField] Fish.Behavior.CommonFish FishPrefab;
        [SerializeField] Sprite Rfish;
        [SerializeField] Sprite SRfish;
        [SerializeField] Sprite SSRfish;

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
            FindObjectOfType<Player.Player>().AddOnThrowRod(() => GenerateFish());
        }

        //場所の設定などから魚を一体生成し、オブジェクトを返す
        public GameObject GenerateFish()
        {
            //生成する座標をランダムで選択
            var pos = generators[Random.Range(0, generators.Count)].position;
            FishInfo selectedFish;
            if (!debug)
            {
                var selectedFishList = SelectFishList();
                int index = Random.Range(0, selectedFishList.Count);
                //            Debug.Log(index);
                selectedFish = selectedFishList[Random.Range(0, selectedFishList.Count)].fishInfo;
                //場所データから一つランダムに選択
                //            FishInfo selectedFish = availableFishList[Random.Range(0, availableFishList.Count)].fishInfo;

                //Debug.Log(selectedFish);
                GameObject fishObj = Instantiate(FishPrefab.gameObject, pos, Quaternion.identity);
                fishObj.GetComponent<Behavior.CommonFish>().InitData(selectedFish, GetShadowSprite(selectedFish.rank));
                //            commonFish.sprite.sprite = GetShadowSprite(commonFish.fishInfo.rank);

                //とりあえず生成される魚は確定
                //GameObject fish = Instantiate(testFish[0], pos, Quaternion.identity);

                return fishObj;

            }
            else
            {
                GameObject fishObj = Instantiate(testfish, pos, Quaternion.identity);
                fishObj.GetComponent<Fish.Behavior.CommonFish>().InitData();
                return fishObj;
            }
        }

        /// <summary>
        /// 年ごとに魚のレア度の排出率を変える。
        /// </summary>
        /// <returns></returns>
        List<Environment.PlaceData.FishGenerateData> SelectFishList()
        {
            bottomScore++;
            int score = Random.Range(0, 100) + bottomScore;
            score = Mathf.Min(score, 99);
            //            var rank = GetRank(score, SaveManager.Instance.Year);
            var rank = GetRank(score, 3);
            switch (rank)
            {
                case FishInfo.Rank.R:
                    return fishList_R;
                case FishInfo.Rank.SR:
                    bottomScore = 0;
                    return fishList_SR;
                case FishInfo.Rank.SSR:
                    bottomScore = 0;
                    return fishList_SSR;
            }
            return fishList_R;
        }

        /// <summary>
        /// 0~99のスコアと年数からレア度を決めて魚のリストを返す
        /// </summary>
        /// <param name="score"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        FishInfo.Rank GetRank(int score, int year)
        {
            if (score < 60)
                return FishInfo.Rank.R;
            if (score < 85)
                return FishInfo.Rank.SR;
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
                    return Rfish;
                case FishInfo.Rank.SR:
                    return SRfish;
                case FishInfo.Rank.SSR:
                    return SSRfish;
            }
            return Rfish;
        }
    }
}