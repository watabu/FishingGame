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

        //釣りゲーム中か isEscapingと紛らわしい
        public bool isFishing = false;
        //親として持つ魚本体
        public FishScripts.CommonFish fish;
        //釣り竿の先端
        protected GameObject AheadofRod;

        //逃げる方向
        public Vector2 movedirection;


        void Awake()
        {
            fish = transform.parent.gameObject.GetComponent<FishScripts.CommonFish>();
           
            AheadofRod = fish.fishingHook.obj.transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        } 



    }
}