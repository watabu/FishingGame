using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// HPを制御するコンポーネント
/// </summary>
public class Damageable : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField, Tooltip("最大体力"), ReadOnly]
    float m_MaxHP;
    [SerializeField, Tooltip("現在の体力"), ReadOnly]
    float m_HP;
    [SerializeField, Tooltip("秒間ごとの回復量"), ReadOnly]
    float m_HPRegene;

    bool isDead = false;

    [System.Serializable]
    public class DamageableEvent : UnityEvent<float> { }
    DamageableEvent m_OnHPChanged = new DamageableEvent();

    UnityEvent m_OnDead = new UnityEvent();

    public void Initialize(float MaxHP, float InitHP, float HPRegene)
    {
        m_MaxHP = MaxHP;
        m_HP = InitHP;
        m_HPRegene = HPRegene;
    }

    /// <summary>
    /// HPが変わったときに実行する関数を追加する
    /// 引数には現在のHPが入る
    /// </summary>
    /// <param name="func"></param>
    public void AddHPChanged(UnityAction<float> func) { m_OnHPChanged.AddListener(func); }

    /// <summary>
    /// 死んだとき(HP==0)に実行する関数を追加する
    /// </summary>
    /// <param name="func"></param>
    public void AddDead(UnityAction func) { m_OnDead.AddListener(func); }

    private void Update()
    {
        if (isDead) return;
        Regene();
    }

    void Regene()
    {
        if (isDead) return;
        m_HP += m_HPRegene * Time.deltaTime;
        m_HP = Mathf.Clamp(m_HP, 0f, m_MaxHP);
        m_OnHPChanged.Invoke(m_HP);
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        m_HP -= damage;
        if (m_HP < 0f)
        {
            m_HP = 0f;
            isDead = true;
            m_OnDead.Invoke();
        }
        m_OnHPChanged.Invoke(m_HP);
    }

}
