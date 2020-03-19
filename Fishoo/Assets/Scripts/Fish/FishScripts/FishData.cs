using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish.FishScripts
{
    [CreateAssetMenu(menuName = "Data/FishData", order = 1)]

    public class FishData : ScriptableObject
    {
        //魚の名前
        public string FishName;
        //アイテムとしての魚の説明欄（図鑑など）
        public string description;
        //釣りあげたときに表示される説明
        public string description_Caught;
        //売るときなどの価値
        public int sellingPrice ;
        //釣りあげたときに得られる経験値
        public int exp;
        //魚のアイコン
        public Sprite icon;
    }
}