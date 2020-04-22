using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{

    /// <summary>
    /// 釣浮き
    /// </summary>
    public class FishingBobber :  MonoBehaviour,FishingTool
    {

        public SpriteRenderer sprite;
        /// <summary>
        /// 釣り具を展開する
        /// </summary>
        public void ExpandTools()
        {


        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {



        }
        public void SetInvisible()
        {
            var color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }

        public void PullToLeft()
        {
        }

        public void PullToRight()
        {
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