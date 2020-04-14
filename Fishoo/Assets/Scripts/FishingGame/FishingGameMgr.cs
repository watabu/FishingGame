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

        [Header("When Fishing starts"), SerializeField, Tooltip("釣りゲームが始まったときに呼び出す関数")]
        UnityEvent WhenFishingStart;


        [Header("When Fishing is successful"), SerializeField,Tooltip("釣りが成功したときに呼び出す関数")]
        UnityEvent WhenFishingSucceeded;

        [Header("When Fishing fails "), SerializeField,Tooltip("釣りが失敗したときに呼び出す関数")]
        UnityEvent WhenFishingFailed;

        [Tooltip("今狙っている魚")]
        public Fish.FishScripts.CommonFish targetFish;

        /// <summary>
        /// 釣りゲームが始まったときに呼び出す関数
        /// </summary>
        public void StartFishing()
        {
            WhenFishingStart.Invoke();
            commandGenerator.targetFish = targetFish;
            targetFish.SetBiting();


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
            //釣り具の動作
            if (input.RightClicked() )
            {
               // fishingToolMgr.PullToRight();
            }
            else if (input.LeftClicked())
            {
               // fishingToolMgr.PullToLeft();
            }
        

        }




    }
}