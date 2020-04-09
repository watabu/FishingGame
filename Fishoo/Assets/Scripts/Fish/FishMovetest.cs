using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    public class FishMovetest : FishMove
    {
        int timer = 0;
        //魚が逃げる方向を変えるまでの時間
        public int maxtime = 100;
        public float power = 400;
        
        //ランダムに単位ベクトルを返す
        Vector2 DecideDirection()
        {
            Vector2 a;
            a.x = Random.Range(0.6f, 1.0f);
            a.x *= (Random.value > 0.5f ? 1 : -1);
            a.y= Random.Range(-1.0f, -0.3f);
            return a;
        }
        // Start is called before the first frame update
        void Start()
        {
            escapeDirection = DecideDirection();
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            timer--;
            if(timer <0)
            {
                timer = Random.Range(20, maxtime);
                escapeDirection = DecideDirection();
            }
            if (isFishing)
            {
                //竿から逃げようとする、釣り糸をたるませないための補正項
                Vector2 C = transform.position - AheadofRod.transform.position;
                C /= Mathf.Sqrt(C.x * C.x + C.y * C.y);

                Vector2 force = (escapeDirection + C) * power;
                fish.fishingHook.obj.GetComponent<Rigidbody2D>().AddForce(force);


            }
        }
    }
}