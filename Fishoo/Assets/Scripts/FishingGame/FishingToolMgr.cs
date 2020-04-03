using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame
{

    public class FishingToolMgr : MonoBehaviour
    {
        public Tools.FishingRod fishingRod;
        public Tools.FishingBobber fishingFloat;
        public Tools.FishingHook fishingHook;

        public GameObject toolsHolder;
        public List<Tools.FishingTool> tools;

        //釣り具を展開する
        public void ExpandTools()
        {



        }

        //釣りが終わり釣り具を収納する
        public void PutAwayTools()
        {


        }
        public void PullToLeft()
        {
            fishingRod.PullToLeft();
        }
        public void PullToRight()
        {
            fishingRod.PullToRight();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}