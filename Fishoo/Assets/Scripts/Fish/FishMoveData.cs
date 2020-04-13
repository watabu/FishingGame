using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    [System.Serializable]
    public struct FishStatus
    {
        [Tooltip("最大体力")]
        public float hpMax;
        [Tooltip("体力回復(/秒)")]
        public float hpRegene;
        [Tooltip("基礎的な速さ")]
        public float speed;//FishMoveDataの方に移動してもいい感じ？
    }
    /// <summary>
    /// 釣りゲームに関する魚の動きについてのデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Data/FishMovementData", order = 1)]
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
        [Tooltip("ステータス")]
        public FishStatus status;
    }
}