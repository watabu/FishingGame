using UnityEngine;
using System.Collections;
using DG.Tweening;

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
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip rainySound;
        
        public PlaceData.Weather CurrentWeather { get; private set; } = PlaceData.Weather.Sunny;

        GameObject m_currentEffect;

        public void SwitchWeather(PlaceData.Weather weather)
        {
            CurrentWeather = weather;
            Destroy(m_currentEffect);
            switch (CurrentWeather)
            {
                case PlaceData.Weather.Sunny:
                    audioSource.StopWithFadeOut(2);
                    break;
                case PlaceData.Weather.Cloudy:
                    audioSource.StopWithFadeOut(2);
                    break;
                case PlaceData.Weather.Rainy:
                    m_currentEffect= Instantiate(rainEffect, effectParent);
                    audioSource.PlayWithFadeIn(rainySound,0.75f,  2);
                    //audioSource.Play();
                    break;
                case PlaceData.Weather.Snowy:
                    audioSource.StopWithFadeOut(2);
                    m_currentEffect = Instantiate(snowEffect, effectParent);
                    var pos = m_currentEffect.transform.localPosition;
                    pos.y += 189;
                    m_currentEffect.transform.localPosition = pos;
                    break;
                case PlaceData.Weather.Windy:
                    audioSource.StopWithFadeOut(2);
                    break;
                default:
                    audioSource.StopWithFadeOut(2);
                    break;
            }
        }





    }

}