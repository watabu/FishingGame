using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputSystem : SingletonMonoBehaviour<InputSystem>
    {
        /// <summary>
        /// Axisesの過去の値を持つ 
        /// </summary>
        static Dictionary<KeyCode,bool> AxisValues = new Dictionary<KeyCode, bool>();
        /// <summary>
        /// InputからGetButtonDownで入力を得るボタンたち
        /// </summary>
        static List<KeyCode> Buttons = new List<KeyCode>() { KeyCode.A, KeyCode.B, KeyCode.X, KeyCode.Y ,KeyCode.Escape, KeyCode.L, KeyCode.R};
        /// <summary>
        /// InputからAxis経由で入力を得るボタンたち
        /// </summary>
        static List<KeyCode> Axises = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        private void Awake()
        {
            if (AxisValues.Count == 0)
            {
                //Axisesを値管理リストに入れる
                foreach (var key in Axises)
                {
                    AxisValues.Add(key, false);
                }
            }
        }

        private void LateUpdate()
        {
            UpdateInput();
        }
        private void Update()
        {
          //  InputTest();
            //   Debug.Log(Input.GetAxisRaw("RightArrow"));
        }
        void UpdateInput()
        {
            foreach(var Axis in Axises)
            {
                AxisValues[Axis] = GetKey(Axis);
            }
        }
        static public bool GetKeyDown(KeyCode key)
        {

            //キーボードで入力されていれば
            if (Input.GetKeyDown(key))
                return true;
            if (Buttons.Contains(key))
                return Input.GetButtonDown(key.ToString());
            if (Axises.Contains(key))
                return (AxisValues[key] ==false && GetKey(key));
            else
            {
                Debug.LogWarning("想定していないキー入力です。");
            }
            return false;
        }
        static public bool GetKey(KeyCode key)
        {
            //キーボードで入力されていれば
            if (Input.GetKey(key))
                return true;
            if (Buttons.Contains(key))
                return Input.GetButton(key.ToString());
            if (Axises.Contains(key))
                if(key == KeyCode.RightArrow || key == KeyCode.LeftArrow)
                    return (Input.GetAxisRaw(key.ToString()) > 0.99);
                else
                {
//                    Debug.Log(Input.GetAxisRaw(key.ToString()) > 0.99);
//                  Debug.Log(Mathf.Abs(Input.GetAxisRaw("UpArrow")) < 0.2);
                    return Input.GetAxisRaw(key.ToString()) > 0.99 && Mathf.Abs(Input.GetAxisRaw("RightArrow")) < 0.2;
                }
                    
            else
            {
                Debug.LogWarning("想定していないキー入力です。");
            }
            return false;
        }
        /// <summary>
        /// アナログスティックを含めて、何らかのボタンが押されているか。
        /// アナログスティックの押された判定が鋭敏なので注意。
        /// </summary>
        static public bool anyKeyDown
        {
            get
            {
                if (Input.anyKeyDown)
                    return true;
                foreach (var key in Axises)
                {
                    if (GetKeyDown(key))
                        return true;
                }
                return false;

            }
        }

        /// <summary>
        /// 正解のキーが押されていないかつ、不正解のキーが押されたか
        /// </summary>
        /// <param name="trueKey"></param>
        /// <returns></returns>
        static public bool FalseKeyDown(KeyCode trueKey)
        {
            return !GetKeyDown(trueKey) && GetOppositeKeyDown(trueKey);
        }

        /// <summary>
        /// （コントローラ上で)反対側にあるキーが押されたか
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool GetOppositeKeyDown(KeyCode key)
        {
            return GetKeyDown(OppositeKey(key));
        }


        /// <summary>
        /// （コントローラ上で)反対側にあるキー
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static KeyCode OppositeKey(KeyCode key)
        {
            if (key == KeyCode.A)
                return KeyCode.B;
            if (key == KeyCode.B)
                return KeyCode.A;
            if (key == KeyCode.RightArrow)
                return KeyCode.LeftArrow;
            if (key == KeyCode.LeftArrow)
                return KeyCode.RightArrow;
            if (key == KeyCode.UpArrow)
                return KeyCode.DownArrow;
            if (key == KeyCode.DownArrow)
                return KeyCode.UpArrow;
                
            Debug.LogWarning("設定していないキーです。");
            return KeyCode.A;
        }
        public Vector2 GetInputArrow()
        {
            Vector2 vec=new Vector2();
            if (GetKey(KeyCode.LeftArrow))vec.x = -1;
           else if (GetKey(KeyCode.RightArrow))vec.x = 1;
            if (GetKey(KeyCode.UpArrow)) vec.x = 1;
            else if (GetKey(KeyCode.DownArrow)) vec.x = -1;
            return vec.normalized;
        }

        void InputTest()
        {
            foreach (var key in Buttons)
            {
                if (GetKeyDown(key))
                    Debug.Log(key);
            }
            foreach(var axis in Axises)
            {
                if (GetKeyDown(axis))
                    Debug.Log(axis);
            }

        }
        void TestGamepad()
        {
            if (Input.GetButtonDown("A"))
                Debug.Log("Pushed A!");
            if (Input.GetButtonDown("B"))
                Debug.Log("Pushed B!");
            if (Input.GetButtonDown("X"))
                Debug.Log("Pushed X!");
            if (Input.GetButtonDown("Y"))
                Debug.Log("Pushed Y!");
            if (Input.GetAxis("RightArrow") > 0)
                Debug.Log("RightArrow");
            if (Input.GetAxis("LeftArrow") > 0)
                Debug.Log("LeftArrow");
            if (Input.GetAxis("UpArrow") > 0)
                Debug.Log("UpArrow");
            if (Input.GetButtonDown("DownArrow"))
                Debug.Log("DownArrow");
            if (Input.GetButtonDown("L"))
                Debug.Log("L");
            if (Input.GetButtonDown("R"))
                Debug.Log("R");

            /*
            if (Input.GetKeyDown("joystick button 0"))
                Debug.Log("0");
            if (Input.GetKeyDown("joystick button 1"))
                Debug.Log("1");
            if (Input.GetKeyDown("joystick button 2"))
                Debug.Log("2");
            if (Input.GetKeyDown("joystick button 3"))
                Debug.Log("3");
                */

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
    /// ABXYの4種類が難しいのでX->(A,B)からランダム,Y->(RLUD)からランダムに変換する。
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
            return new List<KeyCode>() { KeyCode.A, KeyCode.B }[Random.Range(0, 2)];
        if (c == 'Y')
            return new List<KeyCode>() { KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.DownArrow }[Random.Range(0, 4)];
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

    static public string ToString(this char c)
    {
        char[] C = { c };
        return new string(C);
    }

}