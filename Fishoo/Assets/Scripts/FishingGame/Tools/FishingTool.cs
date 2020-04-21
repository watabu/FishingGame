using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{

    //釣り道具の抽象クラス
    public interface FishingTool 
    {
        void PullToLeft();
        void PullToRight();
    }
}