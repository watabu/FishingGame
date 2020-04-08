using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish.FishScripts
{
    [System.Serializable]
    public struct FishStatus
    {
        [Tooltip("最大体力")]
        public float hpMax;
      //  [Tooltip("現在体力")]
     //   public float hp;//これはScene内のオブジェクトが保持すべき
        [Tooltip("体力回復(/1tick)")]
        public float hpRegene;
        [Tooltip("基礎的な速さ")]
        public float speed;//FishMoveDataの方に移動してもいい感じ？
    }

    [CreateAssetMenu(menuName = "Data/FishData", order = 1)]
    public class FishData : ScriptableObject
    {
        [Tooltip("魚の名前")]
        public string FishName;
        [Tooltip("アイテムとしての魚の説明欄（図鑑など）")]
        public string description;
        [Tooltip("釣りあげたときに表示される説明")]
        public string description_Caught;
        [Tooltip("売るときなどの価値")]
        public int sellingPrice;
        [Tooltip("釣りあげたときに得られる経験値")]
        public int exp;
        [Tooltip("魚のアイコン")]
        public Sprite icon;
        [Tooltip("動きのデータ")]
        public FishMoveData data;
        public FishStatus status;
    }
}