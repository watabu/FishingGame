using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputSystem : MonoBehaviour
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


    }
}