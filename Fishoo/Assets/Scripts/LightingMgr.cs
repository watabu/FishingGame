using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// 時間によって環境光を変えるスクリプト
/// </summary>
public class LightingMgr : MonoBehaviour
{

    public Gradient globalColor = new Gradient();
    [Header("References")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D seaLight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
