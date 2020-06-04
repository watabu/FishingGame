using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    /// <summary>
    /// 出てくる魚全てを含むリスト
    /// 主に図鑑用
    /// </summary>
    [CreateAssetMenu(menuName = "Data/FishInfoFolder", order = 1)]
    public class FishInfoFolder : ScriptableObject
    {
        public List<FishInfo> AvailableFishes = new List<FishInfo>();
        public void InitiateList()
        {
            var FishList = Resources.LoadAll<FishInfo>("FishInfo");
            foreach(var fish in FishList)
            {

            }
        }

    }
}