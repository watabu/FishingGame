using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish.FishScripts
{
    public struct FishStatus
    {
        //最大体力
        public float hpMax;
        //現在体力
        public float hp;
        //体力回復(/1tick)
        public float hpRegene;
        //基礎的な速さ
        public float speed;
    }

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
        public int sellingPrice;
        //釣りあげたときに得られる経験値
        public int exp;
        //魚のアイコン
        public Sprite icon;

        public FishStatus status;
    }
}