using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある時間帯にのみ出現するものを管理する
/// 2dLight付けられなくて断念、3dLightは光源として機能しなくて断念
/// 点滅させようと思ったけどParticleわからん
/// </summary>
public class FireFly : MonoBehaviour
{
    [Tooltip("hhmmで設定")]public int AppearTime;
    [Tooltip("hhmmで設定")] public int DisappearTime;
    [ReadOnly,SerializeField]int m_AppearTime;
    [ReadOnly, SerializeField]int m_DisappearTime;


    /// <summary>
    /// 時間を比較して条件を満たせば現れる
    /// </summary>
    /// <param name="time"></param>
    void Appear(int time) {
        if (time >= m_AppearTime)
        {
            gameObject.SetActive(true);
            Environment.TimeHolder.Instance.RemoveOnTimeChanged(Appear);
        }
    }

    /// <summary>
    /// 時間を比較して条件を満たせば消える
    /// </summary>
    /// <param name="time"></param>
    void DisAppear(int time)
    {
        if (time >= m_DisappearTime)
        {
            gameObject.SetActive(false);
            Environment.TimeHolder.Instance.RemoveOnTimeChanged(DisAppear);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        m_AppearTime = AppearTime / 100 * 60 + AppearTime % 100;
        m_DisappearTime = DisappearTime / 100 * 60 + DisappearTime % 100;

        var timeHolder = Environment.TimeHolder.Instance;

        timeHolder.AddOnTimeChanged(Appear);
        timeHolder.AddOnTimeChanged(DisAppear);
        gameObject.SetActive(false);

    }

  
}
