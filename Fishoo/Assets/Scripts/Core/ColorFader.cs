using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorFader : SingletonMonoBehaviour<ColorFader>
{

    public void StartFadeIn(SpriteRenderer sprite, float duration, UnityAction OnFadeFinished = null)
    {
        Color c = sprite.color;
        c.a = 0f;
        sprite.color = c;
        StartCoroutine(Fade(sprite, 1f, duration, OnFadeFinished));
    }
    public void StartFadeOut(SpriteRenderer sprite, float duration, UnityAction OnFadeFinished = null)
    {
        Color c = sprite.color;
        c.a = 1f;
        sprite.color = c;
        StartCoroutine(Fade(sprite, 0f, duration, OnFadeFinished));
    }

    IEnumerator Fade(SpriteRenderer sprite, float target, float duration, UnityAction OnFinished)
    {
        float t = 0f;
        float a = sprite.color.a;
        while (true)
        {
            if (t >= duration) break;
            Color color = sprite.color;
            color.a = Mathf.Lerp(a, target, t / duration);
            sprite.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
