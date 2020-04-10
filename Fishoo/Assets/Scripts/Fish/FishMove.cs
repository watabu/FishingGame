using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    //釣りゲーム中の魚の動きを管理する
    public class FishMove : MonoBehaviour
    {
        [SerializeField] private Fish.FishMoveData data;

        //isEscapingと紛らわしい
        [Tooltip("釣りゲーム中か")]
        public bool isFishing = false;
        [Tooltip("逃げる方向")]
        public Vector2 escapeDirection;

        [Header("Object References")]
        [Tooltip("親として持つ魚本体")]
        public FishScripts.CommonFish fish;
        [SerializeField] Rigidbody2D m_rigidbody;

        //釣り竿の先端
        protected GameObject AheadofRod;


        void Awake()
        {
            fish = transform.parent.gameObject.GetComponent<FishScripts.CommonFish>();
           
            AheadofRod = Fish.FishScripts.CommonFish.FishingHook.obj.transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        } 



    }
}