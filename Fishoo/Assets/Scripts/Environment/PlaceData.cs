using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Environment
{
    [CreateAssetMenu(menuName = "Data/PlaceData", order = 1)]
    public class PlaceData : ScriptableObject
    {
        [Tooltip("場所の名前")]
        public string placeName;
        [Tooltip("場所の説明（地図での）")]
        public string description;
        [Tooltip("場所の詳細な説明（図鑑とか）")]
        public string description_detail;
        [Tooltip("行くときに使う金？")]
        public int cost;
        [Tooltip("行くのにかかる移動時間？")]
        public int TravelTime;
        [Tooltip("場所のアイコン")]
        public Sprite icon;

        [Header("Weather")]
        //天候に関するデータ
        //とりあえず降水確率だけ
        [Range(0,1.0f)]
        [Tooltip("降水確率")]
        public float rainyPercent;

        [Header("Available fish list(Experimental)")]
        //その場所で釣れる魚たち
        //時間ごと、天候ごとに変更したい
        [Tooltip("その場所で釣れる魚たち")]
        public List<Fish.FishScripts.CommonFish> availableFishList;
    }
}