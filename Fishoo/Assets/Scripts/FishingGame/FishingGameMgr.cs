using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FishingGame
{
    public class FishingGameMgr : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FishingToolMgr fishingToolMgr;
        [SerializeField] private Fish.FishGenerator fishGenerator;
        [SerializeField] private CommandGenerator commandGenerator;
        [SerializeField] private Player.InputSystem input;

        [SerializeField, Tooltip("コマンドが生成されるまでの最低時間(tick)")] int attackTimeMin=150;



        [Header("When Fishing starts"), SerializeField, Tooltip("釣りゲームが始まったときに呼び出す関数")]
        UnityEvent WhenFishingStart;


        [Header("When Fishing is successful"), SerializeField,Tooltip("釣りが成功したときに呼び出す関数")]
        UnityEvent WhenFishingSucceeded;

        [Header("When Fishing fails "), SerializeField,Tooltip("釣りが失敗したときに呼び出す関数")]
        UnityEvent WhenFishingFailed;

        [Tooltip("今狙っている魚")]
        public Fish.FishScripts.CommonFish targetFish;

        bool isFishing = false;
        public bool canAttack = false;

        /// <summary>
        /// 攻撃の間隔をあけるためのタイマー
        /// </summary>
        int attackTimer=0;

        /// <summary>
        /// 釣りゲームが始まったときに呼び出す関数
        /// </summary>
        public void StartFishing()
        {
            WhenFishingStart.Invoke();
            commandGenerator.targetFish = targetFish;

            isFishing = true;
            canAttack = true;
            attackTimer = attackTimeMin *3 /4;

            List<List<KeyCode>> commandListTest = new List<List<KeyCode>>();
            commandListTest = targetFish.fishMoveData.GetCommandsList();
            Debug.Log("魚が持つコマンド:");
            foreach(var commands in commandListTest)
            {
                string S="";
                foreach(var command in commands)
                {
                    S += command.ToString();
                }
                Debug.Log(S);
            }

        }

        /// <summary>
        /// 釣りが成功したときに呼び出す
        /// </summary>
        public void FishingSucceeded()
        {
            WhenFishingSucceeded.Invoke();
            isFishing = false;

        }

        /// <summary>
        /// 釣りが失敗したときに呼び出す
        /// </summary>
        public void FishingFailed()
        {
            WhenFishingFailed.Invoke();
            isFishing = false;

        }

        private void Awake()
        {
            if (WhenFishingFailed == null)
                WhenFishingFailed = new UnityEvent();
            if (WhenFishingSucceeded == null)
                WhenFishingSucceeded = new UnityEvent();
            if (WhenFishingStart == null)
                WhenFishingStart = new UnityEvent();

        }

        // Update is called once per frame
        void Update()
        {
            if (isFishing)
            {
                Fishing();
            }





            ////釣り具の動作
            //if (input.RightClicked() )
            //{
            //   // fishingToolMgr.PullToRight();
            //}
            //else if (input.LeftClicked())
            //{
            //   // fishingToolMgr.PullToLeft();
            //}
        

        }

        /// <summary>
        /// 釣りゲーム中の処理
        /// </summary>
        void Fishing()
        {
            if(++attackTimer > attackTimeMin && canAttack)
            {
                canAttack = false;
                attackTimer = 0;
                commandGenerator.Generate();
                
            }


        }

        

    }
}