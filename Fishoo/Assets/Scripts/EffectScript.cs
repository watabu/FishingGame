using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectScript : MonoBehaviour
{

    public float lifetime;

    float m_time=0f;
    UnityAction onDead;
    ParticleSystem particle;

    public void SetOnDead(UnityAction func) { onDead += func; }
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0f && m_time >= lifetime)
        {
            Stop();
        }
        m_time += Time.deltaTime;
    }

    public void Stop()
    {
        particle.Stop(true);
        onDead?.Invoke();
        this.Delay(particle.main.startLifetimeMultiplier,()=> {
            KillImmidiate();
        });
    }
    public void KillImmidiate()
    {
        Destroy(gameObject);
    }
}
