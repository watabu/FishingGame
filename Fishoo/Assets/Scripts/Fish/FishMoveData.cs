using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    [CreateAssetMenu(menuName = "Data/FishMovementData", order = 1)]
    //魚の動きのデータ
    public class FishMoveData : ScriptableObject
    {
        [Header("No meaning")]
        public int test;
       
    }
}