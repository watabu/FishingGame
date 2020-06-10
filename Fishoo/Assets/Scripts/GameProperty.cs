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
    public bool debug;

}
