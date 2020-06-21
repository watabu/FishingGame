using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FishingGame
{

    public class FishingGameMgr : SingletonMonoBehaviour<FishingGameMgr>
    {
        static private FishingGame.Tools.Hook m_fishingHook;
        static public FishingGame.Tools.Hook Hook
        {
            get
            {
                //釣り針の取得
                if (m_fishingHook == null) m_fishingHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<FishingGame.Tools.Hook>();
                return m_fishingHook;
            }
        }

        static private FishingToolMgr m_fishingToolMgr;
        static public FishingToolMgr fishingToolMgr
        {
            get
            {
                //釣り針の取得
                if (m_fishingToolMgr == null)
                    m_fishingToolMgr = Transform.FindObjectOfType<FishingToolMgr>();
                return m_fishingToolMgr;
            }
        }

        private Player.Player player { get { return fishingToolMgr.player; } }//釣りゲームが開始したときのみ他の操作の禁止をする

        [Header("References")]
        [SerializeField] private Fish.FishGenerator generator;
        [SerializeField] private CommandGenerator commandGenerator;
        [SerializeField] private AudioSource fishCaughtSE;
        

        [Tooltip("今狙っている魚"),ReadOnly]
        [SerializeField]
        private Fish.Behavior.CommonFish m_targetFish;
        public Fish.Behavior.CommonFish TargetFish
        {
            get { return m_targetFish; }
        }
        [ReadOnly]
        public bool canAttack = false;
        


        [Header("When Fishing starts"), SerializeField, Tooltip("釣りゲームが始まったときに呼び出す関数")]
        UnityEvent WhenFishingStart;


        [Header("When Fishing is successful"), SerializeField,Tooltip("釣りが成功したときに呼び出す関数")]
        UnityEvent WhenFishingSucceeded;

        [Header("When Fishing fails "), SerializeField,Tooltip("釣りが失敗したときに呼び出す関数")]
        UnityEvent WhenFishingFailed;

        
        /// <summary>
        /// 釣りゲーム中(コマンドバトル)しているか
        /// </summary>
        public bool isFishing { get { return m_isFishing; } }
        bool m_isFishing = false;
        bool isFishCaught { get { return TargetFish.state == Fish.Behavior.FishState.Caught; } }


        private void Start()
        {
            FindObjectOfType<Player.Player>().AddOnThrowRod(() => PrepareFish());
        }

        /// <summary>
        /// 釣り竿が投げられたときに魚を用意する
        /// </summary>
        public void PrepareFish()
        {
            m_targetFish = generator.GenerateFish().GetComponent<Fish.Behavior.CommonFish>();
        }

        /// <summary>
        /// 魚が針に引っ掛かり釣りゲームを開始する
        /// </summary>
        public void StartFishing()
        {
            if (isFishing)
            {
                Debug.LogError("Fishing is already started");
                return;
            }
            Hook.OnBiteHook();
            WhenFishingStart.Invoke();

            m_isFishing = true;
            canAttack = true;


            player.StartFishing();
            commandGenerator.ResetComboCount();
        }

        /// <summary>
        /// 釣りが成功したときに呼び出す
        /// </summary>
        public void FishingSucceeded()
        {
            WhenFishingSucceeded.Invoke();
            m_isFishing = false;
            fishingToolMgr.CatchFish(TargetFish);
            FIshCatchEffect.Instance.Initialize(TargetFish.fishInfo);
            Hook.FinishBite();
            fishCaughtSE.Play();
            commandGenerator.ResetComboCount();
        }

        /// <summary>
        /// 釣りが失敗したときに呼び出す
        /// </summary>
        public void FishingFailed()
        {
            Hook.FinishBite();
            WhenFishingFailed.Invoke();
            m_isFishing = false;
            fishingToolMgr.RetrieveTools();
            m_targetFish.SwitchState(Fish.Behavior.FishState.Escaping);
        }

        new private void Awake()
        {
            base.Awake();
            if (WhenFishingFailed == null) WhenFishingFailed = new UnityEvent();
            if (WhenFishingSucceeded == null) WhenFishingSucceeded = new UnityEvent();
            if (WhenFishingStart == null) WhenFishingStart = new UnityEvent();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isFishing)
            {
                Fishing();
            }
        }

        /// <summary>
        /// 釣りゲーム中の処理
        /// </summary>
        void Fishing()
        {
            if(isFishCaught && canAttack)
            {
                FishingSucceeded();
                return;
            }
            if( canAttack)
            {
                canAttack = false;
                commandGenerator.Generate();
            }
        }


    }
}