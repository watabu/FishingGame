using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

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

        [SerializeField, ReadOnly]
        List<Fish.Behavior.CommonFish> caughtFishList = new List<Fish.Behavior.CommonFish>();

        UnityEvent OnThrowRod=new UnityEvent();

        public void AddOnThrowRod(UnityAction func)
        {
            OnThrowRod.AddListener(func);
        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(coolerBox.gameObject);

            GameMgr.Instance.AddInputUpdatable(this);
            m_state = State.Normal;
            fishingToolMgr = FishingGame.FishingGameMgr.fishingToolMgr;
            GameMgr.Instance.AddOnGameStarted(() =>
            {
                canMove = true;
                ThrowRod();
            });
        }

        public void InputUpdate()
        {
          /*  if (!canMove) return;
            if (m_state == State.Fishing) return;
            if (m_state == State.Normal && InputSystem.GetKeyDown(throwRod))
            {
                ThrowRod();
            }
            else if (m_state == State.ThrowRod && InputSystem.GetKeyDown(retriveRod))
            {
                RetrieveRod();
            }*/
        }

        /// <summary>
        /// 釣り竿を振り海に糸を垂らす
        /// </summary>
        public async void ThrowRod()
        {
            m_state = State.ThrowRod;
            fishingToolMgr.ExpandTools();
            canMove = false;
            OnThrowRod.Invoke();
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
            canMove = true;
        }

        public async void CatchFish(Fish.Behavior.CommonFish fish)
        {
            m_state = State.Normal;
            coolerBox.Add(fish);
            canMove = false;
            await Task.Delay(1000);
            canMove = true;
            ThrowRod();
        }

        public void StartFishing()
        {
            m_state = State.Fishing;
        }
    }
}