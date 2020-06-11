using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountDownGenerator : SingletonMonoBehaviour<CountDownGenerator>
{
    [SerializeField] Sprite count1;
    [SerializeField] Sprite count2;
    [SerializeField] Sprite count3;
    [SerializeField] Sprite gameStart;
    [Header("References")]
    [SerializeField] SpriteRenderer count;
    [SerializeField] SpriteRenderer start;
    [SerializeField] AudioSource countDownBell;
    [SerializeField] AudioSource gameStartBell;

    private new void Awake()
    {
        base.Awake();
        count.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
    }

    /// <summary>
    /// カウントダウンを開始
    /// </summary>
    /// <param name="span">カウント１つについての秒(デフォルトは１秒)</param>
    public void CountStart(float span=1f,UnityAction Onfinished=null)
    {
        count.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
        start.sprite = null;
        StartCoroutine(CountDown(span, Onfinished));
    }

    IEnumerator CountDown(float span, UnityAction Onfinished)
    {
        count.sprite = count3;
        countDownBell.Play();
        yield return new WaitForSeconds(span);
        count.sprite = count2;
        countDownBell.Play();
        yield return new WaitForSeconds(span);
        count.sprite = count1;
        countDownBell.Play();
        yield return new WaitForSeconds(span);
        count.sprite = null;
        start.sprite = gameStart;
        gameStartBell.Play();
        yield return new WaitForSeconds(span);
        start.sprite = null;
        Onfinished.Invoke();
        yield return null;
    }
}
