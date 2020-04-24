using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IInputUpdatable
    {
        public enum State
        {
            None,
            Normal,
            ThrowRod,

        }

        FishingGame.FishingToolMgr fishingToolMgr;

        [Header("Key config")]
        [SerializeField] KeyCode throwRod = KeyCode.A;
        [SerializeField] KeyCode retriveRod = KeyCode.B;


        [Header("Debug")]
        public float speed = 1;
        [SerializeField, ReadOnly] State m_state = State.None;

        /// <summary>
        /// 操作可能か
        /// </summary>
        [SerializeField, ReadOnly] bool canMove = true;
        /// <summary>
        /// 移動可能か
        /// </summary>
        [SerializeField, ReadOnly] bool canWalk = true;

        // Start is called before the first frame update
        void Start()
        {
            GameMgr.Instance.AddInputUpdatable(this);
            m_state = State.Normal;
            fishingToolMgr = FishingGame.FishingGameMgr.fishingToolMgr;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InputUpdate()
        {
            if (!canMove) return;

            if (Input.GetKeyDown(throwRod))
            {
                if (m_state == State.Normal)
                {
                    ThrowRod();
                }
            }
            else if (Input.GetKeyDown(retriveRod) &&  m_state == State.ThrowRod)
            {
                RetrieveRod();
            }

            if (!canWalk) return;
            Move(InputSystem.Instance.GetInputArrow());


        }
        public void Move(Vector2 velocity)
        {
            velocity.y = 0;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }

        /// <summary>
        /// 釣り竿を振り海に糸を垂らす
        /// </summary>
        public void ThrowRod()
        {
            m_state = State.ThrowRod;
            fishingToolMgr.ExpandTools();
            //            canMove = false;
            canWalk = false;
        }

        /// <summary>
        /// 釣り竿をもとに戻す
        /// </summary>
        public void RetrieveRod()
        {
            m_state = State.Normal;
            fishingToolMgr.RetrieveTools();
            //遅延を持たせたい
            //canMove = true;
            canWalk = true;
        }

        public void CatchFish(Fish.Behavior.CommonFish fish)
        {
            m_state = State.Normal;
            fishingToolMgr.CatchFish(fish);
            canWalk = true;
        }


    }
}