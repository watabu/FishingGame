using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class InputSystem : SingletonMonoBehaviour<InputSystem>
    {
        /// <summary>
        /// Axisesの過去の値を持つ 
        /// </summary>
        static Dictionary<KeyCode,float> AxisValues = new Dictionary<KeyCode, float>();
        /// <summary>
        /// InputからGetButtonDownで入力を得るボタンたち
        /// </summary>
        static List<KeyCode> Buttons = new List<KeyCode>() { KeyCode.A, KeyCode.B, KeyCode.X, KeyCode.Y };
        /// <summary>
        /// InputからAxis経由で入力を得るボタンたち
        /// </summary>
        static List<KeyCode> Axises = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };

        private void Awake()
        {
            //Axisesを値管理リストに入れる
            foreach( var key in Axises)
            {
                AxisValues.Add(key, 0);
            }
        }

        private void LateUpdate()
        {
            UpdateInput();
        }
        private void Update()
        {
            InputTest();
        }
        void UpdateInput()
        {
            foreach(var Axis in Axises)
            {
                AxisValues[Axis] = Input.GetAxis(Axis.ToString());
            }
        }
        static public bool GetKeyDown(KeyCode key)
        {

            if (Buttons.Contains(key))
                return Input.GetButtonDown(key.ToString());
            if (Axises.Contains(key))
                return (AxisValues[key] ==0 && Input.GetAxis(key.ToString()) > 0);
            else
            {
                Debug.LogWarning("想定していないキー入力です。");
            }
            return false;
        }
        static public bool GetKey(KeyCode key)
        {

            if (Buttons.Contains(key))
                return Input.GetButton(key.ToString());
            if (Axises.Contains(key))
                return (Input.GetAxis(key.ToString()) > 0);
            else
            {
                Debug.LogWarning("想定していないキー入力です。");
            }
            return false;
        }
        /// <summary>
        /// 十字キーを含めて、何らかのボタンが押されているか
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
            foreach(var key in Buttons)
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

    static public string ToString(this char c)
    {
        char[] C = { c };
        return new string(C);
    }

}