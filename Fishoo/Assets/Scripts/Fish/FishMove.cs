using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    //釣りゲーム中の魚の動きを管理する
    public class FishMove : MonoBehaviour
    {
        [SerializeField] private Fish.FishMoveData data;

        //釣りゲーム中か
        public bool isFishing { get { return fish.state == FishScripts.FishState.Biting; } }
        [Tooltip("移動方向")]
        public Vector2 moveDirection;

        [Header("Object References")]
        [Tooltip("親として持つ魚本体")]
        public FishScripts.CommonFish fish;
        [SerializeField] Rigidbody2D m_rigidbody;

        //釣り竿の先端
        protected GameObject AheadofRod;


        void Awake()
        {
            fish = transform.parent.gameObject.GetComponent<FishScripts.CommonFish>();
           
            //AheadofRod = Fish.FishScripts.CommonFish.FishingHook.obj.transform.parent.gameObject;
            moveDirection = DecideDirection();
        }

        // Update is called once per frame
        void Update()
        {

        }


        //ランダムに単位ベクトルを返す
        Vector2 DecideDirection()
        {
            Vector2 a;
            a.x = Random.Range(0.6f, 1.0f);
            a.x *= (Random.value > 0.5f ? 1 : -1);
            a.y = Random.Range(-1.0f, -0.3f);
            return a;
        }

    }
}