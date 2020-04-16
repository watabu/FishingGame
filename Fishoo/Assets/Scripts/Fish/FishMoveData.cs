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
    /// 釣りゲームに関する魚のデータ
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
        [Tooltip("コマンド")]
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


/// <summary>
/// 文字コードの拡張メソッドなどを含むクラス
/// </summary>
public static class MyExtensions
{

    /// <summary>
    /// 文字をキーコードにする
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    static public KeyCode ToKeyCode(this char c)
    {
        if (c == 'A')
            return KeyCode.A;
        if (c == 'B')
            return KeyCode.B;
        if (c == 'X')
            return KeyCode.X;
        if (c == 'Y')
            return KeyCode.Y;
        if (c == 'R')
            return KeyCode.RightArrow;
        if (c == 'D')
            return KeyCode.DownArrow;
        if (c == 'L')
            return KeyCode.LeftArrow;
        if (c == 'U')
            return KeyCode.UpArrow;

        Debug.LogWarning("想定していないボタンを要求されました。");
        return KeyCode.Space;
        //コントローラーのR

        //コントローラーのL
    }

    /// <summary>
    /// 文字列をキーコードのリストにする
    /// </summary>
    /// <param name="S"></param>
    /// <returns></returns>
    static public List<KeyCode> ToKeyCodeList(this string S)
    {
        List<KeyCode> A = new List<KeyCode>();
        foreach (char c in S)
        {
            A.Add(c.ToKeyCode());
        }
        return A;

    }

}