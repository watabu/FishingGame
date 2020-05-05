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
            GameObject selectedFish = availableFishList[Random.Range(0, availableFishList.Count)].fish.gameObject;            
            
            GameObject fish = Instantiate(selectedFish, pos, Quaternion.identity);


            //とりあえず生成される魚は確定
            //GameObject fish = Instantiate(testFish[0], pos, Quaternion.identity);

            return fish;
        }
        IEnumerator Generate()
        {
            while (true)
            {
                GenerateFish();
                yield return new WaitForSeconds(generateSpan);
            }
        }

    }
}