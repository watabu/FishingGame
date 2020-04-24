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
        public FishingGame.FishingGameMgr fishingGameMgr;//publicこわい

        [SerializeField] GameObject biteHookEffect;
        [SerializeField] Transform effectParent;

        public SpriteRenderer sprite;

        [Tooltip("釣り針を引っ張る力の大きさ")]
        public float force = 1;

        public int _time = 10;
        Vector3 prev;

        /// <summary>
        ///     力の作用点
        /// </summary>
        public GameObject obj;

        /// <summary>
        /// 一時的に行いたい関数
        /// </summary>
        UnityEvent myUpdate;

        /// <summary>
        /// 力を及ぼす対象
        /// </summary>
        Rigidbody2D rg2d;

        CommonFish m_currentFish;

        private void Awake()
        {
            if (myUpdate == null)
                myUpdate = new UnityEvent();
            rg2d = obj.GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            myUpdate.Invoke();
        }


        public void SetTarget(CommonFish fish) { m_currentFish = fish; }
        public bool CanBite() { return m_currentFish == null && isInWater; }
        public void FinishBite() { m_currentFish = null; }
        bool isInWater = false;
        /// <summary>
        /// 釣り具を展開する
        /// </summary>
        public async void ExpandTools()
        {
            await Task.Delay(2000);
            isInWater = true;
        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            isInWater = false;
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
