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
        [SerializeField] private Behavior.CommonFish fish;
        [SerializeField] private Fish.FishMoveData data;

        [Tooltip("移動方向"),ReadOnly]
        public Vector2 moveDirection;

        [Header("Object References")]
        //釣り竿の先端
        protected GameObject AheadofRod;

        /// <summary>
        /// 自由に動くときの中心座標
        /// </summary>
        Vector3 neutralPos;
        /// <summary>
        /// テスト用
        /// 自由に動く時の速さ(半径)
        /// </summary>
        float speed = 2;

        Vector2 escapeVelocity;
        void Awake()
        {
            //AheadofRod = Fish.Behavior.CommonFish.Hook.obj.transform.parent.gameObject;
            moveDirection = DecideDirection();

            float[] dirXRnd = { 1f, -1f };
            float dirX = dirXRnd[Random.Range(0, 2)];
            escapeVelocity = new Vector2(dirX, 0) / 8f;
            neutralPos = transform.position;
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
        /// とくに目的を持たず自由に動く
        /// </summary>
        public void MoveFree()
        {
            float x = speed * Mathf.Sin(Time.time);
            fish.gameObject.transform.position = neutralPos + new Vector3(x, 0f, 0f);
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