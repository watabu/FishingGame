using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある時間帯にのみ出現するものを管理する
/// </summary>
public class FireFly : MonoBehaviour
{
    [SerializeField, Tooltip("季節で出現するか")]
    public bool inSpring;
    public bool inSummer;
    public bool inAutumn;
    public bool inWinter;

    List<bool> isAppeared = new List<bool>(); 

    [Tooltip("hhmmで設定")] public int AppearTime;
    [Tooltip("hhmmで設定")] public int DisappearTime;

    [ReadOnly, SerializeField] int m_AppearTime;
    [ReadOnly, SerializeField] int m_DisappearTime;

    [SerializeField,Tooltip("")] bool debug;

    /// <summary>
    /// 時間を比較して条件を満たせば現れる
    /// </summary>
    /// <param name="time"></param>
    void Appear(int time)
    {
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

    private void Awake()
    {
        isAppeared.Add(inSpring);
        isAppeared.Add(inSummer);
        isAppeared.Add(inAutumn);
        isAppeared.Add(inWinter);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_AppearTime = AppearTime / 100 * 60 + AppearTime % 100;
        m_DisappearTime = DisappearTime / 100 * 60 + DisappearTime % 100;
        Initialize();
    }

    public void Initialize()
    {
        int SeasonID = (int)SaveManager.Instance.GetSeasonID();
        Debug.Log(SeasonID);
        Debug.Log(isAppeared[SeasonID]);

        if (!isAppeared[SeasonID])
        {
            gameObject.SetActive(false);
            return;
        }

        if(Environment.TimeHolder.Instance == null)
        {
            return;
        }
        Debug.Log(SeasonID);
        Debug.Log(isAppeared[SeasonID]);
        m_AppearTime = AppearTime / 100 * 60 + AppearTime % 100;
        m_DisappearTime = DisappearTime / 100 * 60 + DisappearTime % 100;

        var timeHolder = Environment.TimeHolder.Instance;

        timeHolder.AddOnTimeChanged(Appear);
        timeHolder.AddOnTimeChanged(DisAppear);
        gameObject.SetActive(false);

    }

}
