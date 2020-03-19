using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Environment
{
    [CreateAssetMenu(menuName = "Data/PlaceData", order = 1)]
    public class PlaceData : ScriptableObject
    {
        //場所の名前
        public string placeName;
        //場所の説明（地図での）
        public string description;
        //場所の詳細な説明（図鑑とか）
        public string description_detail;
        //行くときに使う金？
        public int cost;
        //行くのにかかる移動時間？
        public int TravelTime;
        //場所のアイコン
        public Sprite icon;

        [Header("Weather"), SerializeField]
        //天候に関するデータ
        //とりあえず降水確率だけ
        [Range(0,1.0f)]
        public float rainyPercent;

        [Header("Available fish list(Experimental)"), SerializeField]
        //その場所で釣れる魚たち
        //時間ごと、天候ごとに変更したい
        public List<Fish.FishScripts.CommonFish> availableFishList;
    }
}