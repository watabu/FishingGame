using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Fish.Behavior;

namespace FishingGame.Tools
{
    //釣り針
    public class Hook : MonoBehaviour, FishingTool
    {
        [SerializeField]
        public FishingGame.FishingGameMgr fishingGameMgr;

        [SerializeField] GameObject biteHookEffect;
        [SerializeField] Transform effectParent;

        public SpriteRenderer sprite;

        [Tooltip("釣り針を引っ張る力の大きさ")]
        float force = 1;//魚の種類で釣り針の下がり具合を変えたければpublic

        Vector3 prev;

        /// <summary>
        ///     力の作用するゲームオブジェクト(rg2dを持つ)
        /// </summary>
        GameObject obj;

        /// <summary>
        /// 一時的に行いたい関数
        /// </summary>
        UnityEvent myUpdate;

        /// <summary>
        /// 力を及ぼす対象
        /// </summary>
        Rigidbody2D rg2d;


        [ReadOnly,SerializeField]CommonFish m_currentFish;
        public CommonFish currentFish
        {
            get { return m_currentFish; }
        }

        private void Awake()
        {
            if (myUpdate == null)
                myUpdate = new UnityEvent();
            obj = gameObject;
            rg2d = obj.GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            myUpdate.Invoke();
        }


        public void SetTarget(CommonFish fish) { m_currentFish = fish; }
        public bool CanBite() { return m_currentFish == null && isInWater && isInWater_mask; }
        public void FinishBite() { m_currentFish = null; }
        bool isInWater = false;
        bool isInWater_mask = false;
        
        /// <summary>
        /// 釣り具を展開する 2秒後に釣り可能にする
        /// </summary>
        public async void ExpandTools()
        {
            isInWater_mask = true;
            isInWater = false;
            m_currentFish = null;
            await Task.Delay(2000);
            isInWater = true;
        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            isInWater_mask = false;
            isInWater = false;
            if (currentFish != null && currentFish.state != Fish.Behavior.FishState.Caught)
            {
                currentFish.SetEscaping();
            }
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
            rg2d.AddForce(q);
        }

        /// <summary>
        /// 食いついた
        /// </summary>
        public void OnBiteHook()
        {
            Instantiate(biteHookEffect,transform.position,Quaternion.identity, effectParent);
        }

        public async void testPullDown()
        {
            int _time = 10;

            myUpdate.AddListener(_PullDown);
            await Task.Delay(_time);
            myUpdate.RemoveListener(_PullDown);
        }
        /// <summary>
        /// マウスの方向に引っ張る
        /// </summary>
        void testPull()
        {
            Vector3 p = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            Vector2 q;

            q.x = p.x - this.transform.position.x;
            q.y = p.y - this.transform.position.y;
            q /= Mathf.Sqrt(q.x * q.x + q.y * q.y);
            q *= force;

            if (Input.GetMouseButton(0))
            {
                if (obj.GetComponent<Rigidbody2D>() != null)
                    obj.GetComponent<Rigidbody2D>().AddForce(q);
                else
                    obj.GetComponent<Rigidbody>().AddForce(q);

            }
            if (Input.GetMouseButton(1))
            {
                q *= 10;
                if (obj.GetComponent<Rigidbody2D>() != null)
                    obj.GetComponent<Rigidbody2D>().AddForce(q);
                else
                    obj.GetComponent<Rigidbody>().AddForce(q);
            }

        }

        public void PullToLeft()
        {
        }

        public void PullToRight()
        {
        }
    }
}
