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
}
