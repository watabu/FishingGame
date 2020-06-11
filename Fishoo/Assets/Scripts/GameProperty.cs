using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Property")]
public class GameProperty : ScriptableObject
{
    public Color spring;
    public Color summer;
    public Color autumn;
    public Color winter;
    [Header("BGM")]
    public AudioClip springBGM;
    public AudioClip summerBGM;
    public AudioClip autumnBGM;
    public AudioClip winterBGM;
    public bool debug;

}
