using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    //釣りゲーム中の魚の動きを管理する
    public class FishMove : MonoBehaviour
    {
        [SerializeField]
        Fish.FishMoveData data;

        //釣りゲーム中か
        public bool isFishing = false;
        //親として持つ魚本体
        FishScripts.CommonFish fish;
        struct NewVector
        {
            public Vector3 vec;
            public float deltaTime;
        }
        //釣り竿に引っ張られて移動する先
        NewVector newVector;

        //vecの方向にdeltaTime時間を用いて魚が引っ張られる
        public void Pull(Vector3 vec,float deltaTime)
        {
            newVector.vec = transform.localPosition + vec;
            newVector.deltaTime = deltaTime;
        }

        // Start is called before the first frame update
        void Start()
        {
            fish = transform.parent.GetComponent<FishScripts.CommonFish>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}