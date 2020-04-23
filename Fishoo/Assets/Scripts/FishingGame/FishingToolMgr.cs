using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame
{

    public class FishingToolMgr : SingletonMonoBehaviour<FishingToolMgr>
    {
        public Tools.FishingRod fishingRod;
        public Tools.FishingBobber fishingBobber;
        public Tools.FishingHook fishingHook;

        public GameObject toolsHolder;
        public List<Tools.FishingTool> tools;

        //釣り具を展開する
        public void ExpandTools()
        {
            foreach(var tool in tools)
            {
                tool.ExpandTools();
            }
            

        }
        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            foreach (var tool in tools)
            {
                tool.RetrieveTools();
            }

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
            tools = new List<Tools.FishingTool>();
            tools.Add(fishingRod);
            tools.Add(fishingHook);
            tools.Add(fishingBobber);
            foreach (var tool in tools)
            {
                tool.SetInvisible();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}