using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{

    //釣り道具の抽象クラス
    public interface FishingTool 
    {

        /// <summary>
        /// 釣り具を展開する
        /// </summary>
        void ExpandTools();
       
        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        void RetrieveTools();

        void SetInvisible();

    }
}