﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Fish.Behavior;

namespace FishingGame.Tools
{
    /// <summary>
    /// 釣り針
    /// どの魚が狙っているかを保管し二重に魚が引っ掛からないようにする
    /// </summary>
    public class Hook : MonoBehaviour, FishingTool
    {
        [SerializeField]
        public FishingGame.FishingGameMgr fishingGameMgr;

        [SerializeField] GameObject biteHookEffect;
        [SerializeField] Transform effectParent;

        [SerializeField]Bobber bobber;
        public SpriteRenderer sprite;

        [Tooltip("釣り針を引っ張る力の大きさ")]
        float force = 1;//魚の種類で釣り針の下がり具合を変えたければpublic
        [Tooltip("ニュートラルポジション")]
        Vector3 neutralPos;


        /// <summary>
        /// 一時的に行いたい関数
        /// </summary>
        UnityEvent myUpdate;

        /// <summary>
        /// 力を及ぼす対象
        /// </summary>
//        Rigidbody2D rg2d;


        [ReadOnly,SerializeField]CommonFish m_currentFish;
        public CommonFish currentFish
        {
            get { return m_currentFish; }
        }

        private void Awake()
        {
            if (myUpdate == null)  myUpdate = new UnityEvent();
            neutralPos = transform.localPosition;
            //            rg2d = obj.GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            myUpdate.Invoke();
        }

        public void SetTarget(CommonFish fish) { m_currentFish = fish; }
        public bool CanBite() { return m_currentFish == null && isInWater && isInWater_mask; }
        public void FinishBite() {
            m_currentFish = null;
//            rg2d.simulated = true;
        }
        [ReadOnly,SerializeField]bool isInWater = false;
        [ReadOnly,SerializeField]bool isInWater_mask = false;
        
        /// <summary>
        /// 釣り具を展開する 2秒後に釣り可能にする
        /// </summary>
        public async void ExpandTools()
        {
            //         rg2d.simulated = true;
            transform.localPosition = neutralPos;

            isInWater_mask = true;
            isInWater = false;
            m_currentFish = null;
            await Task.Delay(1700);
            isInWater = true;
        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            isInWater_mask = false;
            isInWater = false;

            m_currentFish = null;
        }

        public void SetInvisible()
        {
            var color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }

        /// <summary>
        /// 下向きの力をtimeミリ秒与える
        /// </summary>
        /// <param name="power"></param>
        /// <param name="time"></param>
        public async void PullDown(float power, int time)
        {
            force = power;
            myUpdate.AddListener(_PullDown);
            await Task.Delay(time);
            myUpdate.RemoveListener(_PullDown);
        }

        /// <summary>
        /// 下向きの力を与える
        /// </summary>
        void _PullDown()
        {
            Vector2 q;
            q.x = 0;
            q.y = -1;
            q *= force;
//            rg2d.AddForce(q);
        }

        /// <summary>
        /// 食いついた
        /// </summary>
        public void OnBiteHook()
        {
            Instantiate(biteHookEffect,transform.position,Quaternion.identity, effectParent);
            
            bobber.SetInvisible();
        }

        public void PulledToPos(Vector3 pos)
        {
            transform.localPosition = neutralPos + pos;
        }



    }
}
