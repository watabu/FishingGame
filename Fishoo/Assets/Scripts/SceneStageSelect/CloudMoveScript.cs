using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoveScript : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed*Time.deltaTime,0,0);
        if (speed < 0)
        {
            if (transform.position.x < minX)
            {
                transform.position = new Vector3(maxX, transform.position.y, 0);
            }
        }
        else
        {
            if (transform.position.x > maxX)
            {
                transform.position = new Vector3(minX, transform.position.y, 0);
            }
        }
    }
}
