using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// ある時間帯にのみ出現するものを管理する
/// </summary>
public class ParticleMgr : MonoBehaviour
{
    [SerializeField, Tooltip("季節で出現するか")]
    public bool inSpring;
    public bool inSummer;
    public bool inAutumn;
    public bool inWinter;

    List<bool> isAppeared = new List<bool>(); 

    [Tooltip("hhmmで設定")] public int appearTime;
    [Tooltip("hhmmで設定")] public int disappearTime;


    [SerializeField,Tooltip("ステージセレクトでパーティクルが表示される確率")] public float probability_StageSelect;
    [SerializeField,Tooltip("RateOverTimeの最大")] float rateOverTime_Max;
    [ReadOnly, SerializeField] int m_AppearTime;
    [ReadOnly, SerializeField] int m_DisappearTime;

    [SerializeField,Tooltip("")] bool debug;

    ParticleSystem particle;

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
        particle = GetComponent<ParticleSystem>();
        if (rateOverTime_Max <= 0)
        {
            return;
        }
        var emission = particle.emission;
        if (Random.Range(0, 4) != 0)
        {
            float currentValue = emission.rateOverTime.constant;
            emission.rateOverTime = Random.Range(currentValue, (currentValue * 5 + rateOverTime_Max) / 6);
        }
        else
        {
            float currentValue = emission.rateOverTime.constant;
            emission.rateOverTime = Random.Range(currentValue, (currentValue + rateOverTime_Max*2) / 3);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_AppearTime = appearTime / 100 * 60 + appearTime % 100;
        m_DisappearTime = disappearTime / 100 * 60 + disappearTime % 100;
        Initialize();
    }

    public void Initialize()
    {
        int SeasonID = (int)SaveManager.Instance.GetSeasonID();
     
        if (!isAppeared[SeasonID])
        {
            gameObject.SetActive(false);
            return;
        }

        if(Environment.TimeHolder.Instance == null)
        {
            Debug.Log("TimeHolderがないのでタイトル用として起動");
            if (probability_StageSelect < Random.Range(0, 1.0f))
                gameObject.SetActive(false);
            return;
        }
//        Debug.Log(SeasonID);
//        Debug.Log(isAppeared[SeasonID]);
        m_AppearTime = appearTime / 100 * 60 + appearTime % 100;
        m_DisappearTime = disappearTime / 100 * 60 + disappearTime % 100;

        var timeHolder = Environment.TimeHolder.Instance;

        timeHolder.AddOnTimeChanged(Appear);
        timeHolder.AddOnTimeChanged(DisAppear);
        gameObject.SetActive(false);

    }



}
