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
    }


    /// <summary>
    /// 釣りゲームに関する魚のデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Data/FishMovementData", order = 1)]
    public class FishMoveData : ScriptableObject
    {
        [Header("Properties")]
        [Tooltip("ミスしたときに逃げるか")]
        public bool escapeOnMiss;
        /// <summary>
        /// 釣りゲーム外での速さ
        /// </summary>
        [Tooltip("通常時の速さ")]
        public float speed_nomal;
        /// <summary>
        /// 釣りゲーム中での速さ
        /// </summary>
        [Tooltip("釣り中に泳ぐ速度")]
        public float speed;
        [Tooltip("針を認識する距離")]
        public float recognitionDistance;
        [Tooltip("海にいる時間")]
        public float lifeTime;
        [Tooltip("ステータス")]
        public FishStatus status;
        [Tooltip("通常時の魚の動き方")]
        public Behavior.MoveType moveType = Behavior.MoveType.SwimHorizontaly;
        
        [Tooltip("コマンド 最大12文字")]
        public List<string> commands;





        /// <summary>
        /// 魚がもつコマンドのリストを返す
        /// </summary>
        /// <returns></returns>
        public List<List<KeyCode>> GetCommandsList()
        {
            List<List<KeyCode>> A = new List<List<KeyCode>>();
            foreach (string S in commands)
            {
                A.Add(S.ToKeyCodeList());
            }
            if(A.Count < 1)
            {
                Debug.LogError("魚のコマンドが設定されていません。");
            }
            return A;
        }

        public List<KeyCode> GetCommands()
        {
            int N = commands.Count;
            int selected = Random.Range(0, N);
            List<List<KeyCode>> A = GetCommandsList();
            return A[selected];
        }
    }
}
