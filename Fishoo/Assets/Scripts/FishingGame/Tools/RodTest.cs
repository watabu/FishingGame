using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodTest : MonoBehaviour
{
    //釣り竿を回転する軸
    public Vector2 shaft;
    Rigidbody2D rg2d;
    //釣り竿の画像
    public SpriteRenderer sprite;
    float MaxAngle_Left = 55;
    float MaxAngle_Right = -35;

    public void PullToLeft()
    {
       // Debug.Log(Mathf.DeltaAngle(shaft.transform.eulerAngles.z, MaxAngle_Left));
        if (Mathf.DeltaAngle(transform.eulerAngles.z, MaxAngle_Left) > 0.1f)
        {
            rg2d.MoveRotation( Mathf.LerpAngle(transform.eulerAngles.z, MaxAngle_Left, Time.deltaTime * 6));
         //   shaft.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(shaft.transform.eulerAngles.z, MaxAngle_Left, Time.deltaTime * 2));
            //             shaft.transform.Rotate(new Vector3(0f, 0f, 2f));
        }
    }
    public void PullToRight()
    {
      //  Debug.Log(Mathf.DeltaAngle(shaft.transform.eulerAngles.z, MaxAngle_Right));
        if (Mathf.DeltaAngle(transform.eulerAngles.z, MaxAngle_Right) < -0.1f)
        {
            rg2d.MoveRotation(Mathf.LerpAngle(transform.eulerAngles.z, MaxAngle_Right, Time.deltaTime * 6));

//            shaft.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(shaft.transform.eulerAngles.z, MaxAngle_Right, Time.deltaTime * 2));
            //          shaft.transform.Rotate(new Vector3(0f,0f, -2f));
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.centerOfMass = shaft;
        shaft = rg2d.centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PullToLeft();
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            PullToRight();
        }
    }
}
