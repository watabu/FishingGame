using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame
{

    public class FishingToolMgr : MonoBehaviour
    {
        public Tools.FishingRod fishingRod;
        public Tools.FishingFloat fishingFloat;
        public Tools.FishingHook fishingHook;

        public GameObject toolsHolder;
        public List<Tools.FishingTool> tools;

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