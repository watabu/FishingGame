using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    /// <summary>
    /// 釣りゲーム中の魚の動きを管理する
    /// </summary>
    public class FishMove : MonoBehaviour
    {
        [SerializeField] private Fish.FishMoveData data;

        [Tooltip("移動方向"),ReadOnly]
        public Vector2 moveDirection;

        [Header("Object References")]
        //釣り竿の先端
        protected GameObject AheadofRod;

        Vector2 escapeVelocity;
        void Awake()
        {
            //AheadofRod = Fish.FishScripts.CommonFish.FishingHook.obj.transform.parent.gameObject;
            moveDirection = DecideDirection();

            float[] dirXRnd = { 1f, -1f };
            float dirX = dirXRnd[Random.Range(0, 2)];
            escapeVelocity = new Vector2(dirX, 0) / 8f;
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

        /// <summary>
        /// 逃走中の処理
        /// </summary>
        public void Escape()
        {
            transform.position += (Vector3)escapeVelocity;
        }
    }
}