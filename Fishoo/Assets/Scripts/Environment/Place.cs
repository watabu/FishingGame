using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    /// <summary>
    /// 場所
    /// </summary>
    public class Place : MonoBehaviour
    {
        [Header("Data")]
        //各場所の設定データ
        public PlaceData placeData;
        //
        [Header("References")]
        [SerializeField]private Weather weather;
        //
        [SerializeField] private TimeHolder timeHolder;
        
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}