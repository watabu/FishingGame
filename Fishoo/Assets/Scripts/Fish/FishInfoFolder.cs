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
        public List<FishInfo> AvailableGomi = new List<FishInfo>();

        public void InitiateList()
        {
            AvailableFishes.Clear();
            AvailableGomi.Clear();
            var FishList = Resources.LoadAll<FishInfo>("FishInfo");
            foreach (var fish in FishList)
            {
                AvailableFishes.Add(fish);
            }
            var GomiList = Resources.LoadAll<FishInfo>("GomiInfo");
            foreach (var gomi in GomiList)
            {
                AvailableGomi.Add(gomi);
            }
        }

    }
}