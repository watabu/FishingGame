using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMoveScript : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float sequency=1f;
    [SerializeField] float phase = 0f;
    Vector3 first;
    private void Awake()
    {
        first = transform.position;
    }
    float time = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = first+new Vector3(0,amplitude*Mathf.Sin(Mathf.PI*2/sequency* (time+ phase)),0);
        time += Time.deltaTime;
    }
}
