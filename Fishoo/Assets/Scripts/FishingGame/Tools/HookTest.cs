using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTest : MonoBehaviour
{
    public GameObject obj;
    Vector3 prev;
    void Update()
    {
        Vector3 p = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        Vector2 q;
        q.x = p.x - this.transform.position.x;
        q.y = p.y - this.transform.position.y;
        q *= 8;
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

