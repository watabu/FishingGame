using UnityEngine;
using System.Collections;


namespace Environment
{

    //天候を管理するクラス
    //天候による演出(雨とか雪とか強風の効果)も担当する？

    public class Weather : MonoBehaviour
    {

        //各場所についての天気の傾向はPlace.PlaceDataに含まれている
        [SerializeField]private Place place;


        // Use this for initialization
        void Start()
        {
               
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}