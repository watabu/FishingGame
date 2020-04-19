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

    Environment.TimeHolder m_timeHolder;
    private void Awake()
    {
        m_timeHolder = FindObjectOfType<Environment.TimeHolder>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_timeHolder.AddOnTimeChanged((time)=> { SetLight(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetLight()
    {
       var col= globalColor.Evaluate(m_timeHolder.NormalizedTime);
        Debug.Log("time"+m_timeHolder.NormalizedTime);
        globalLight.color = col;
    }

}
