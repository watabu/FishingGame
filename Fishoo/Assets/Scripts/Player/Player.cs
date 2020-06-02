using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Player
{
    /// <summary>
    /// Player - FishingToolMgr - FishingGameMgr
    /// 釣り竿を下す・回収する のみPlayerが指示する
    /// 他はFishingToolMgr・FishingGameMgrの間で処理をする
    /// </summary>
    public class Player : MonoBehaviour, IInputUpdatable
    {
        public enum State
        {
            None,
            Normal,
            ThrowRod,
            Fishing
        }

        FishingGame.FishingToolMgr fishingToolMgr;

        [SerializeField]CoolerBox coolerBox;
        //[Header("Key config")]
        KeyCode throwRod = KeyCode.A;
        KeyCode retriveRod = KeyCode.B;


        [Header("Debug")]
        public float speed = 5;
        [SerializeField, ReadOnly] State m_state = State.None;

        /// <summary>
        /// 操作可能か
        /// </summary>
        [SerializeField, ReadOnly] bool canMove = true;
        /// <summary>
        /// 移動可能か
        /// </summary>
        [SerializeField, ReadOnly] bool canWalk = true;

        [SerializeField, ReadOnly]
        List<Fish.Behavior.CommonFish> caughtFishList = new List<Fish.Behavior.CommonFish>();

        public List<Fish.Behavior.CommonFish> GetFishList() { return caughtFishList; }

        // Start is called before the first frame update
        void Start()
        {
            GameMgr.Instance.AddInputUpdatable(this);
            m_state = State.Normal;
            fishingToolMgr = FishingGame.FishingGameMgr.fishingToolMgr;
        }



        public void InputUpdate()
        {
            if (!canMove) return;
            if (m_state == State.Fishing) return;
            if (InputSystem.GetKeyDown(throwRod) && m_state == State.Normal) 
            {
                if (m_state == State.Normal)
                {
                    ThrowRod();
                }
            }
            else if (InputSystem.GetKeyDown(retriveRod) &&  m_state == State.ThrowRod)
            {
                RetrieveRod();
            }

            if (!canWalk) return;
            Move(InputSystem.Instance.GetInputArrow());


        }
        public void Move(Vector2 velocity)
        {
            velocity.y = 0;
//            transform.position += (Vector3)velocity * Time.deltaTime * speed;
        }

        /// <summary>
        /// 釣り竿を振り海に糸を垂らす
        /// </summary>
        public async void ThrowRod()
        {
            m_state = State.ThrowRod;
            fishingToolMgr.ExpandTools();
            canMove = false;
            canWalk = false;
            await Task.Delay(1000);
            canMove = true;
        }

        /// <summary>
        /// 釣り竿をもとに戻す(魚を釣らなかった)
        /// </summary>
        public async void RetrieveRod()
        {
            m_state = State.Normal;
            fishingToolMgr.RetrieveTools();
            canMove = false;
            await Task.Delay(1000);
            canWalk = true;
            canMove = true;
        }

        public async void CatchFish(Fish.Behavior.CommonFish fish)
        {
            m_state = State.Normal;
            caughtFishList.Add(fish);
            coolerBox.Add(fish);
            canMove = false;
            await Task.Delay(1000);
            canWalk = true;
            canMove = true;
        }

        public void StartFishing()
        {
            m_state = State.Fishing;

        }
    }
}