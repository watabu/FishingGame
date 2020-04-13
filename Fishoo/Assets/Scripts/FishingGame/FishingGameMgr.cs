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
        [SerializeField] private Player.InputSystem input;

        [Header("When Fishing starts"), SerializeField]
        UnityEvent WhenFishingStart;//釣りゲームが始まったときに呼び出す関数


        [Header("When Fishing is successful"), SerializeField]
        UnityEvent WhenFishingSucceeded;//釣りが成功したときに呼び出す関数

        [Header("When Fishing fails "), SerializeField]
        UnityEvent WhenFishingFailed;//釣りが失敗したときに呼び出す関数

        [Tooltip("今狙っている魚")]
        public Fish.FishScripts.CommonFish targetFish;

        /// <summary>
        /// 釣りゲームが始まったときに呼び出す関数
        /// </summary>
        public void StartFishing()
        {
            WhenFishingStart.Invoke();




        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
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