using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEffect : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

}
