using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputSystem : SingletonMonoBehaviour<InputSystem>
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //試験的な入力　右クリック・左クリックされているかどうか
        public bool RightClicked()
        {
            return Input.GetMouseButton(1);
        }
        public bool LeftClicked()
        {
            return Input.GetMouseButton(0);
        }

        public Vector2 GetInputArrow()
        {
            Vector2 vec=new Vector2();
            if (Input.GetKey(KeyCode.LeftArrow))vec.x = -1;
           else if (Input.GetKey(KeyCode.RightArrow))vec.x = 1;
            if (Input.GetKey(KeyCode.UpArrow)) vec.x = 1;
            else if (Input.GetKey(KeyCode.DownArrow)) vec.x = -1;
            return vec.normalized;
        }


    }
}