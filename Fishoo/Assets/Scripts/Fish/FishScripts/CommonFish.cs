using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fish.FishScripts
{
    /// <summary>
    /// 普通の魚
    /// </summary>
    /// 
    public enum FishState
    {
        Nomal,//適当に泳いでいる
        Approaching,//エサを狙っている
        Biting,//エサに食いついて釣りゲーム中
        Escaping,//釣りに失敗して逃げている
        Caught//釣りに成功して捕まった
    }

    [RequireComponent(typeof(Damageable))]
    public class CommonFish : MonoBehaviour
    {
        public FishData fishData;
        public FishMoveData fishMoveData;

        public float moveSpeed=0.05f;

        /// <summary>
        /// 魚が湧いてから経った時間
        /// </summary>
        [ReadOnly]
        public float m_livingTime;

        [SerializeField, Tooltip("魚がエサを狙っているかなどの状態"), ReadOnly]
        private FishState _state;
        /// <summary>
        /// 魚の状態(読み取り専用)
        /// </summary>
        public FishState state { get { return _state; } }

        [Header("Object References")]
        public FishMove fishMove;

        

        [Tooltip("魚の画像")]
        public SpriteRenderer sprite;

        [Tooltip("体力バー")]
        public Slider HPbar;


        /// <summary>
        /// 一時的に実行したい関数を入れる
        /// </summary>
        UnityEvent myUpdate = new UnityEvent();
        //public delegate void myUpdate();

        Damageable m_damageable;

        static private FishingGame.Tools.FishingHook m_fishingHook;

        private void Awake()
        {
            HPbar.maxValue = fishMoveData.status.hpMax;
            if (sprite == null)
            {
                //画像が指定されていなければ子供のオブジェクトから画像を得る
                sprite = transform.GetComponentInChildren<SpriteRenderer>();
            }
            SetAppear();
            m_damageable = GetComponent<Damageable>();
            m_damageable.Initialize(fishMoveData.status.hpMax, fishMoveData.status.hpMax, fishMoveData.status.hpRegene);
            m_damageable.AddHPChanged((hp) =>
            {
                HPbar.value = hp;
            });
            m_damageable.AddDead(() =>
            {
                //釣られたときの処理
                //SetDisAppear();
                SetCaught();
            });
        }
        private void Start()
        {
            m_fishingHook = FishingGame.FishingGameMgr.FishingHook;
        }

        // Update is called once per frame
        void Update()
        {
            //一時的に行う関数などを行う
            myUpdate.Invoke();

            switch (state)
            {
                //通常
                case FishState.Nomal:
                    //寿命が尽きたら消える            
                    if (m_livingTime > fishMoveData.lifeTime)
                        SetDisAppear();
                    fishMove.MoveFree();
                    break;
                //エサを狙っている
                case FishState.Approaching:
                    ApproachHook();
                    break;
                //釣りゲーム中
                case FishState.Biting:
                    Bite();
                    break;
                //釣りに失敗したら逃げる
                case FishState.Escaping:
                    fishMove.Escape();
                    break;
                //捕まった
                case FishState.Caught:
                    break;
                default:
                    break;
            }
            m_livingTime += Time.deltaTime;
        }

        //釣りゲーム外

        /// <summary>
        /// 魚が現れる時に最初に行う処理
        /// </summary>
        public void SetAppear()
        {
            Debug.Log("魚が現れた");
            HPbar.gameObject.SetActive(false);
            _state = FishState.Nomal;
            ColorFader.Instance.StartFade(sprite, true, 0.5f);
        }

        /// <summary>
        /// 魚が消える時に最初に行う処理
        /// </summary>
        public void SetDisAppear()
        {
            Debug.Log("魚は寿命をまっとうした。");
            m_fishingHook.FinishBite();
            ColorFader.Instance.StartFade(sprite, false, 0.5f, () => gameObject.SetActive(false));
        }



        /// <summary>
        /// 釣り針を狙うときに行う最初の処理
        /// </summary>
        public void SetApproaching()
        {
            if (m_fishingHook.CanBite())
            {
                Debug.Log("魚がエサを狙っている");
                _state = FishState.Approaching;
                m_fishingHook.SetTarget(this);
            }
            else
            {
                Debug.Log("すでに他の魚が狙っている");
            }
        }

        //非同期処理を一回だけ行うための変数
        bool isDone=false;

        /// <summary>
        /// 釣り針を狙う
        /// 釣りゲーム開始前のミニゲーム
        /// </summary>
        async void ApproachHook()
        {
            if (isDone) return;

            MoveToHook();
            //十分近づいたら針をつんつんする
            if (IsNearHook())
            {
                isDone = true;
                //座標を釣り針中心にする
                transform.parent = m_fishingHook.transform;

                //浮きを沈める力と時間のリスト
                List<Vector2> approachList = new List<Vector2> { new Vector2(4, 200), new Vector2(6, 150) /*,new Vector2(3, 300),new Vector2(5, 150),new Vector2(40, 80)*/ };

                float force;
                int time;
                //アプローチリストに従って浮きをつんつんする
                foreach (var a in approachList)
                {
                    force = a.x;
                    time = (int)a.y;
                    while (!IsNearHook())
                    {
                        MoveToHook();
                        await Task.Delay(10);
                    }
                    m_fishingHook.PullDown(force, time);
                    await Task.Delay(time);
                    LeaveFromHook();
                    await Task.Delay(Random.Range(time * 10, time * 30));
                }
                time = 50;
                while (!IsNearHook())
                {
                    MoveToHook();
                    await Task.Delay(10);
                }
                m_fishingHook.PullDown(40, time);

                //食いついたときになにかを入力して釣りゲームへ移行する
                SetBiting();
            }
        }

        /// <summary>
        /// 釣り針の方に近づいていく
        /// </summary>
        void MoveToHook()
        {
            transform.position = Vector3.MoveTowards(transform.position, m_fishingHook.transform.position, moveSpeed);
        }

        /// <summary>
        /// 針をつついた後に向かう針から離れた点
        /// </summary>
        Vector3 LeavePoint;
        void LeaveFromHook()
        {
            LeavePoint = m_fishingHook.transform.position;
            LeavePoint.x += 0.5f;
            myUpdate.AddListener(_LeaveFromHook);
        }

        void _LeaveFromHook()
        {
            transform.position = Vector3.MoveTowards(transform.position, LeavePoint, moveSpeed / 30);
            if ((LeavePoint - transform.position).sqrMagnitude < 0.01f)
                myUpdate.RemoveListener(_LeaveFromHook);
        }

        public bool IsNearHook()
        {
            float distance = 0.001f;
            return (m_fishingHook.transform.position - transform.position).sqrMagnitude < distance;
        }


        /// <summary>
        /// 針に食いついたときに最初に行う処理
        /// 釣りゲーム状態に移行する
        /// </summary>
        public void SetBiting()
        {
            Debug.Log("魚がくいついた!");
            _state = FishState.Biting;
            HPbar.gameObject.SetActive(true);

            ////釣りゲーム開始
            m_fishingHook.fishingGameMgr.StartFishing(this);
        }

        /// <summary>
        /// 釣りゲーム中の処理
        /// </summary>
        void Bite()
        {

        }
        
        public void SetCaught()
        {
            HPbar.gameObject.SetActive(false);
            Debug.Log("魚を捕まえた!");
            _state = FishState.Caught;
            m_fishingHook.fishingGameMgr.FishingSucceeded();
        }

        /// <summary>
        /// 釣りに失敗したときの最初に行う処理
        /// 逃走状態に移行する
        /// </summary>
        public void SetEscaping()
        {
            Debug.Log("魚は逃げ出した");
            transform.parent = null;
            _state = FishState.Escaping;
            m_fishingHook.fishingGameMgr.FishingFailed();
        }
        public void Damaged(float damage)
        {
            m_damageable.TakeDamage(damage);
        }

    }
}