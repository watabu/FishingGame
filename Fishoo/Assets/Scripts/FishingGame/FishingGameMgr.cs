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
        UnityEvent FishingStart;//釣りが成功したときに呼び出す関数


        [Header("When Fishing is successful"), SerializeField]
        UnityEvent FishingSucceeded;//釣りが成功したときに呼び出す関数

        [Header("When Fishing fails "), SerializeField]
        UnityEvent FishingFailed;//釣りが失敗したときに呼び出す関数





        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (input.RightClicked() )
            {
                fishingToolMgr.PullToRight();
            }
            else if (input.LeftClicked())
            {
                fishingToolMgr.PullToLeft();

            }
        

        }


    }
}