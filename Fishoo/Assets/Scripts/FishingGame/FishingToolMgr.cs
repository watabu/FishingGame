using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame
{

    public class FishingToolMgr : SingletonMonoBehaviour<FishingToolMgr>
    {
        [Header("Object References")]
        [SerializeField]Player.Player m_player;
        public Player.Player player { get { return m_player; } }


        public Tools.Rod fishingRod;
        public Tools.Bobber fishingBobber;
        public Tools.Hook fishingHook;
        public Tools.Bucket bucket;

        public GameObject toolsHolder;
        public List<Tools.FishingTool> tools = new List<Tools.FishingTool>();

        //釣り具を展開する
        public void ExpandTools()
        {
            foreach(var tool in tools)
                tool.ExpandTools();
        }
        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            foreach (var tool in tools)
                tool.RetrieveTools();
        }

        /// <summary>
        /// 釣りゲームに成功して魚を釣り上げる
        /// </summary>
        /// <param name="fish"></param>
        public void CatchFish(Fish.Behavior.CommonFish fish)
        {
            player.CatchFish(fish);
            bucket.SwallowFish(fish);
            RetrieveTools();
        }

        // Start is called before the first frame update
        void Start()
        {
            fishingBobber.SetInvisible();
            fishingRod.SetInvisible();
            fishingHook.SetInvisible();

            tools.Add(fishingRod);
            tools.Add(fishingHook);
            tools.Add(fishingBobber);
            //foreach (var tool in tools)
            //{
            //    tool.SetInvisible();
            //}
        }

    }
}