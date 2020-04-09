using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    [CreateAssetMenu(menuName = "Data/FishMovementData", order = 1)]
    //魚の動きのデータ
    public class FishMoveData : ScriptableObject
    {
        [Header("Properties")]
        [Tooltip("何回針に食いつこうとするか")]
        public int tryEatCount;
        [Tooltip("ミスしたときに逃げるか")]
        public bool escapeOnMiss;
        [Tooltip("泳ぐ速度")]
        public float speed;
        [Tooltip("針を認識する距離")]
        public float recognitionDistance;
        [Tooltip("海にいる時間")]
        public float lifeTime;
    }
}