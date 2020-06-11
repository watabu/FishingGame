using UnityEngine;
using System.Collections;


namespace Environment
{

    //天候を管理するクラス
    //天候による演出(雨とか雪とか強風の効果)も担当する？

    public class WeatherManager : SingletonMonoBehaviour<WeatherManager>
    {

        //各場所についての天気の傾向はPlace.PlaceDataに含まれている
        [SerializeField]private Place place;
        [Header("Effects")]
        [SerializeField] private Transform effectParent;
        [SerializeField] private GameObject rainEffect;
        [SerializeField] private GameObject snowEffect;
        public PlaceData.Weather CurrentWeather { get; private set; } = PlaceData.Weather.Sunny;

        GameObject m_currentEffect;

        public void SwitchWeather(PlaceData.Weather weather)
        {
            CurrentWeather = weather;
            Destroy(m_currentEffect);
            switch (CurrentWeather)
            {
                case PlaceData.Weather.Sunny:
                    break;
                case PlaceData.Weather.Cloudy:
                    break;
                case PlaceData.Weather.Rainy:
                    m_currentEffect= Instantiate(rainEffect, effectParent);
                    break;
                case PlaceData.Weather.Snowy:
                    m_currentEffect = Instantiate(snowEffect, effectParent);
                    var pos = m_currentEffect.transform.localPosition;
                    pos.y += 189;
                    m_currentEffect.transform.localPosition = pos;
                    break;
                case PlaceData.Weather.Windy:
                    break;
                default:
                    break;
            }
        }

    }
}