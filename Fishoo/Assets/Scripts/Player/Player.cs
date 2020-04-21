using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {

        public enum State
        {
             None,
             Normal,
             ThrowRod,

        }
        State m_state = State.None;
        // Start is called before the first frame update
        void Start()
        {
            m_state = State.Normal;
        }

        // Update is called once per frame
        void Update()
        {
            InputUpdate();
        }

        void InputUpdate()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (m_state == State.Normal)
                {
                    ThrowRod();
                }else if (m_state == State.ThrowRod)
                {
                    RetrieveRod();
                }
            }
            
        }

        /// <summary>
        /// 釣り竿を振り海に糸を垂らす
        /// </summary>
        void ThrowRod()
        {

        }

        /// <summary>
        /// 釣り竿をもとに戻す
        /// </summary>
        void RetrieveRod()
        {

        }

    }
}