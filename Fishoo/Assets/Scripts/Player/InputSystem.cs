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

        //試験的な入力　右クリックと左クリック
        public bool Right()
        {
            return UnityEngine.Input.GetMouseButton(1);
        }
        public bool Left()
        {
            return UnityEngine.Input.GetMouseButton(0);
        }


    }
}