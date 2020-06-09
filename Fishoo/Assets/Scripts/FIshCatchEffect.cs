using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class FIshCatchEffect : SingletonMonoBehaviour<FIshCatchEffect>
{
    [SerializeField] GameObject Effect_R;
    [SerializeField] GameObject Effect_SR;
    [SerializeField] GameObject Effect_SSR;
    [SerializeField] GameObject Effect_Text;

    [Header("Object References")]
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Transform effectParent;

    [SerializeField] Transform effectCanvas;

    EffectScript currentEffect;

    private void Start()
    {
        renderer.sprite = null;
    }
    public void Initialize(Fish.FishInfo info)
    {
        renderer.sprite = info.icon;
        switch (info.rank)
        {
            case Fish.FishInfo.Rank.R:
                currentEffect = Instantiate(Effect_R, effectParent).GetComponent<EffectScript>();
                break;
            case Fish.FishInfo.Rank.SR:
                currentEffect = Instantiate(Effect_SR, effectParent).GetComponent<EffectScript>();
                break;
            case Fish.FishInfo.Rank.SSR:
                currentEffect = Instantiate(Effect_SSR, effectParent).GetComponent<EffectScript>();
                break;
            default:
                break;
        }
        var obj = Instantiate(Effect_Text, effectCanvas);
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = info.FishName;
        currentEffect.SetOnDead(() =>
        {
            renderer.DOFade(0f, 0.5f).OnComplete(() =>
            {
                renderer.sprite = null;
                renderer.color = Color.white;
                Destroy(obj);
            });
        });
    }
}
