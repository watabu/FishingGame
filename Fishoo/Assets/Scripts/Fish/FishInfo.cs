using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{


    /// <summary>
    /// 釣りゲーム外の、図鑑などでの魚のデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Data/FishInfo", order = 1)]
    public class FishInfo : ScriptableObject
    {
        [Tooltip("魚の名前")]
        public string FishName;
        [Tooltip("アイテムとしての魚の説明欄（図鑑など）"),TextArea]
        public string description;
        [Tooltip("釣りあげたときに表示される説明 未使用")]
        public string description_Caught;
        [Tooltip("売るときなどの価値")]
        public int sellingPrice;
        [Tooltip("釣りあげたときに得られる経験値")]
        public int exp;
        [Tooltip("魚のアイコン")]
        public Sprite icon;
        [Tooltip("動きのデータ")]
        public FishMoveData data;
        public bool isGomi=false;

    }
}