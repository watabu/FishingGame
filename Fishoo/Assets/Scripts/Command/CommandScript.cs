using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour
{
    [SerializeField] GameObject effect;
    public Transform effectParent;
    Image m_image;
    bool m_LiveFlag = true;
    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void Update()
    {
//        if (!m_LiveFlag) Destroy(gameObject);
    }

    public void Kill()
    {
        var script=Instantiate(effect, effectParent).GetComponent<EffectScript>();
        script.SetOnDead(() => { m_LiveFlag = false; });
        m_image.color = m_image.color * new Color(1f,1f,1f,0.25f);
    }
    public void Kill(int combo, int Maxcombo, Vector3 Point)
    {
        var script = Instantiate(effect, effectParent).GetComponent<EffectScript>();
        script.SetOnDead(() => { m_LiveFlag = false; });
        m_image.color = m_image.color * new Color(1f, 1f, 1f, 0.25f);

        //コンボ数に応じてエフェクトの大きさを変化
        float Scale = 0.3f + 0.6f *combo / Maxcombo* combo / Maxcombo;
        Debug.Log(Scale);
        var main = script.GetComponent<ParticleSystem>().main;
        main.simulationSpeed /= Scale;
        script.lifetime *= Scale;
        script.transform.localScale *=Scale;

        //座標を魚の現在位置に合わせる
        script.transform.position = Point;
    }

}
