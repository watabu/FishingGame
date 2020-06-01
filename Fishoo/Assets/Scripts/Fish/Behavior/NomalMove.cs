using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使わない
/* 魚の動き方のメモ
 * 上層を浮かびながら下流へ流れていく
 * 表面をはねる
 * 表面を泳ぐ
 * 中層を左右にゆっくり
 * ぷかぷか漂う（くらげ）
 * 底を歩く
 * 底を転がる
 * 底の岩などにささっている
 * 
 */

namespace Fish.Behavior
{
    /// <summary>
    /// 釣り針に気づくまでの魚の動き方のタイプ
    /// </summary>
   public enum MoveType
    {
        /// <summary>
        /// 水面を飛ぶ
        /// </summary>
        Jump,
        /// <summary>
        /// 上層を浮かびながら下流へ流れていく
        /// </summary>
        DriftOnSurface,
        /// <summary>
        /// 水面をはねる
        /// </summary>
        JumpOnSurface,
        /// <summary>
        /// 左右におよぐ
        /// </summary>
        SwimHorizontaly,
        /// <summary>
        /// 上下に動く
        /// </summary>
        SwimVerticaly,
        /// <summary>
        /// ある点を中心に動く
        /// </summary>
        SwimAround,
        /// <summary>
        /// ある点らをランダムに回る
        /// </summary>
        SwimPointToPoint,
        /// <summary>
        /// 浮いたり沈んだりしながら泳ぐ（くらげ）
        /// </summary>
        FloatingAndSinking,
        /// <summary>
        /// 水底を歩く
        /// </summary>
        WalkingAtBottom,
        /// <summary>
        /// 水底を転がる
        /// </summary>
        RollingAtBottom,
        /// <summary>
        /// 不動
        /// </summary>
        Fixed
    }

    /// <summary>
    /// 湧いてから針を見つけるまでの魚の自由な動き
    /// </summary>
    public class NomalMove : MonoBehaviour
    {
        [ReadOnly,SerializeField]CommonFish fish;
        [ReadOnly,SerializeField]FishMoveData fishMoveData;
        /// <summary>
        ///Awakeで初期化
        /// </summary>
        [SerializeField]MoveType m_MoveType;

        float m_speed;

        /// <summary>
        /// RollingAtBottomの回転半径
        /// </summary>
        public float RollingRadius = 1;

        private void Awake()
        {
            fish = transform.parent.GetComponent<CommonFish>();
            if (fish == null) Debug.LogError("親オブジェクトがCommonFishを持っていません。");
            fishMoveData = fish.fishMoveData;
            m_speed = fishMoveData.speed_nomal;
            m_MoveType = fish.fishMoveData.moveType;
        }
        public void Move()
        {
            Move(m_MoveType);
        }

        void Move(MoveType type)
        {
            Debug.Log(type);
            switch (type)
            {
                case MoveType.DriftOnSurface:

                    break;
                case MoveType.Fixed:


                    break;
                case MoveType.FloatingAndSinking:
                    


                    break;
                case MoveType.Jump:

                    break;
                case MoveType.JumpOnSurface:

                    break;
                case MoveType.RollingAtBottom:
                    //角速度(度/s)
                    float angularVelocity = -m_speed * 180 / Mathf.PI / RollingRadius;
                    //回転と移動
                    fish.sprite.transform.Rotate(0, 0, angularVelocity * Time.deltaTime);
                    fish.transform.position +=  new Vector3(m_speed * Time.deltaTime, 0, 0);
                    break;
                case MoveType.SwimAround:

                    break;
                case MoveType.SwimHorizontaly:

                    break;
                case MoveType.SwimVerticaly:
                    float deltaY = Mathf.Cos(Time.time) * Time.deltaTime;
                    fish.transform.position += new Vector3(0, deltaY, 0);
                    break;
                case MoveType.SwimPointToPoint:

                    break;
                case MoveType.WalkingAtBottom:

                    break;
                default:
                    Debug.LogError("存在しないタイプです。");
                    break;
            }
            


        }


        

    }
}


