using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish {
    
    public class FishGenerator : MonoBehaviour
    {
        public Environment.Place place;
        
        //とりあえず生成される魚
        public GameObject testFish;

        // Start is called before the first frame update
        void Start()
        {

        }

        //場所の設定などから魚を一体生成し、オブジェクトを返す
        public GameObject GenerateFish()
        {
            //とりあえず生成される魚は確定
            GameObject fish = Instantiate(testFish) as GameObject;

            return fish;
        }
        

    }
}