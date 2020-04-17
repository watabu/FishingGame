using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectScript : MonoBehaviour
{

    public float lifetime;

    float m_time=0f;
    UnityAction onDead;

    public void SetOnDead(UnityAction func) { onDead += func; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time >= lifetime)
        {
            Destroy(gameObject);
            onDead?.Invoke();
        }
            m_time += Time.deltaTime;
    }
}
