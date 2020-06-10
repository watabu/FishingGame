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
       

        //とりあえず生成される魚
        public List<GameObject> testFish;

        List<Environment.PlaceData.FishGenerateData> availableFishList;

        [Tooltip("魚を生成するスパン(秒)")]
        public float generateSpan;

        //魚を生成する座標
        List<Transform> generators = new List<Transform>();

        // Start is called before the first frame update
        void Start()
        {
            availableFishList = place.placeData.availableFishList;
            foreach (Transform child in generatorsParent)
            {
                generators.Add(child);
            }
            StartCoroutine(Generate());
        }

        //場所の設定などから魚を一体生成し、オブジェクトを返す
        public GameObject GenerateFish()
        {
            //生成する座標をランダムで選択
            var pos = generators[Random.Range(0, generators.Count)].position;

            //場所データから一つランダムに選択
            FishInfo selectedFish = availableFishList[Random.Range(0, availableFishList.Count)].fishInfo;
            //Debug.Log(selectedFish);
            GameObject fishObj = Instantiate(FishPrefab.gameObject, pos, Quaternion.identity);
            var commonFish = fishObj.GetComponent<Behavior.CommonFish>();
            commonFish.InitData(selectedFish, GetShadowSprite(commonFish.fishInfo.rank));
//            commonFish.sprite.sprite = GetShadowSprite(commonFish.fishInfo.rank);
            
            //とりあえず生成される魚は確定
            //GameObject fish = Instantiate(testFish[0], pos, Quaternion.identity);

            return fishObj;
        }
        IEnumerator Generate()
        {
            while (true)
            {
                GenerateFish();
                yield return new WaitForSeconds(generateSpan);
            }
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