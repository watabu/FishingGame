using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish.Behavior
{

    public class SwimPointToPoint : NomalMove
    {
        List<Vector3> PointList;
        //        List<GameObject> PointList;

        /// <summary>
        /// 一か所にとどまる時間(s)
        /// </summary>
        float MaxStayTime=5;

        /// <summary>
        /// ある場所についた時間
        /// </summary>
        float StayTime;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        new public void Move() 
        {
            //時間がたったら次の場所へ
            if(Time.time - StayTime > MaxStayTime)
            {



            }

        }

    }
}