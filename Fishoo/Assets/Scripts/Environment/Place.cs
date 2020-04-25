using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Environment
{
    /// <summary>
    /// 場所
    /// </summary>
    public class Place : MonoBehaviour
    {
        [Header("Properties")]
        [Tooltip("何分おきに天気が変わるかの判定をするか")]
        [SerializeField] private int weatherChangeSpan = 30;

        [Header("Data")]
        //各場所の設定データ
        public PlaceData placeData;

        LightingMgr lighting;

        int weatherStartTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            var timeHolder = TimeHolder.Instance;
            timeHolder.AddOnTimeChanged((time) =>
            {
                if (time % weatherChangeSpan == 0)
                {
                    UpdateWeather();
                }
            });
            timeHolder.startTime = placeData.startTime;
            timeHolder.endTime = placeData.endTime;
            lighting = GetComponent<LightingMgr>();
            lighting.globalColor = placeData.globalColor;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateWeather()
        {
            if (WeatherManager.Instance.CurrentWeather != PlaceData.Weather.Rainy)
            {
                //データの降水確率がランダム値より大きければ雨を降らせる
                if (placeData.rainyPercent >= Random.value)
                {
                    WeatherManager.Instance.SwitchWeather(PlaceData.Weather.Rainy);
                    weatherStartTime = TimeHolder.Instance.CurrentTime;
                }
            }
            else
            {
                if (TimeHolder.Instance.CurrentTime > placeData.rainDuration + weatherStartTime)
                    WeatherManager.Instance.SwitchWeather(PlaceData.Weather.Sunny);
            }
        }

    }
}