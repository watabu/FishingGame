using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{
    public class HookTest : FishingHook
    {
        public float force = 8;
        Vector3 prev;

        void Update()
        {
            Vector3 p = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            Vector2 q;

            q.x = p.x - this.transform.position.x;
            q.y = p.y - this.transform.position.y;
            q /= Mathf.Sqrt(q.x * q.x + q.y * q.y);
            q *= force;

            if (Input.GetMouseButton(0))
            {
                if (obj.GetComponent<Rigidbody2D>() != null)
                    obj.GetComponent<Rigidbody2D>().AddForce(q);
                else
                    obj.GetComponent<Rigidbody>().AddForce(q);

            }
            if (Input.GetMouseButton(1))
            {
                q *= 10;
                if (obj.GetComponent<Rigidbody2D>() != null)
                    obj.GetComponent<Rigidbody2D>().AddForce(q);
                else
                    obj.GetComponent<Rigidbody>().AddForce(q);
            }
        }
    }
}

