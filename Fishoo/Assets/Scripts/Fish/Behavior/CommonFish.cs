using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fish.Behavior
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

    [RequireComponent(typeof(Damageable), typeof(CircleCollider2D))]
    public class CommonFish : MonoBehaviour
    {
        public FishInfo fishInfo;
        public FishMoveData fishMoveData;
        [Tooltip("針に気づく前の動き")] public NomalMove nomalMove;

        /// <summary>
        /// 針に気づいて近づく速さ
        /// </summary>
        [Tooltip("針に気づいて近づく速さ")]
        public float moveSpeed = 0.05f;

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

        [Tooltip("針に気づく範囲")]
        CircleCollider2D colliderToNoticeHook;

        /// <summary>
        /// 一時的に実行したい関数を入れる
        /// </summary>
        UnityEvent myUpdate = new UnityEvent();
        //public delegate void myUpdate();

        Damageable m_damageable;

        static private FishingGame.Tools.Hook m_fishingHook;
        public FishingGame.Tools.Hook GetHook{get { return m_fishingHook; }}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fishInfo"></param>
        /// <param name="fishMoveData"></param>
        public void InitData(FishInfo fishInfo,Sprite sprite)
        {
            this.fishInfo = fishInfo;
            fishMove.fish = this;
            fishMove.data = fishInfo.data;
            fishMoveData = fishInfo.data;

            var hpbar = HPbar.GetComponent<RectTransform>().sizeDelta;
            hpbar.x = 10 * fishMoveData.status.hpMax;
            HPbar.GetComponent<RectTransform>().sizeDelta = hpbar;
            this.sprite.sprite = sprite;
            HPbar.maxValue = fishMoveData.status.hpMax;
            if (this.sprite == null)
            {
                //画像が指定されていなければ子供のオブジェクトから画像を得る
                this.sprite = transform.GetComponentInChildren<SpriteRenderer>();
            }
            colliderToNoticeHook = GetComponent<CircleCollider2D>();
            colliderToNoticeHook.radius = fishMoveData.recognitionDistance;
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

        private void Awake()
        {



            
        }
        
        private void Start()
        {
            m_fishingHook = FishingGame.FishingGameMgr.Hook;
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
                    m_livingTime += Time.deltaTime;
                    if (fishMoveData == null)
                        return;
                    if (m_livingTime > fishMoveData.lifeTime)
                        SetDisAppear();
                    //                    fishMove.MoveFree();
                    //nomalMove.Move();
                    SetApproaching();
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

                    SetDisAppear();
                    break;
                //捕まった
                case FishState.Caught:
                    break;
                default:
                    break;
            }

        }

        //釣りゲーム外

        /// <summary>
        /// 魚が現れる時に最初に行う処理
        /// </summary>
        public void SetAppear()
        {
//            Debug.Log("魚が現れた");
            HPbar.gameObject.SetActive(false);
            _state = FishState.Nomal;
            ColorFader.Instance.StartFadeIn(sprite, 0.5f);
        }

        /// <summary>
        /// 魚が消える時に最初に行う処理
        /// </summary>
        public void SetDisAppear()
        {
         //   m_fishingHook.FinishBite();
            ColorFader.Instance.StartFadeOut(sprite, 0.5f, () => gameObject.SetActive(false));
        }

        /// <summary>
        /// 釣り針と接触したときに可能なら釣り針を狙う
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != "Hook") return;
//            Debug.Log("衝突！" + collision);
            SetApproaching();

        }

        /// <summary>
        /// 釣り針を狙うときに行う最初の処理
        /// </summary>
        public void SetApproaching()
        {
            if (m_fishingHook.CanBite())
            {
//                Debug.Log("魚がエサを狙っている");
                _state = FishState.Approaching;
                m_fishingHook.SetTarget(this);
            }
            else
            {
  //              Debug.Log("釣り針はなにか用事があるようだ。");
            }
        }

        //非同期処理を一回だけ行うための変数 もし釣りに失敗しても釣れるんだったら変える必要あり
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
                //リストにある要素の個数だけ針をつんつんする
                List<Vector2> approachList = new List<Vector2> { new Vector2(4, 200) };

                float force;
                int time;
                //アプローチリストに従って浮きをつんつんする
                //foreach (var a in approachList)
                //{
                //    force = a.x;
                //    time = (int)a.y;
                //    while (!IsNearHook())
                //    {
                //        MoveToHook();
                //        await Task.Delay(10);
                //    }
                //    m_fishingHook.PullDown(force, time);
                //    await Task.Delay(time);
                //    LeaveFromHook();
                //    await Task.Delay(Random.Range(time * 10, time * 30));
                //}

                force = 50;
                time = 50;
                while (!IsNearHook())
                {
                    MoveToHook();
                    await Task.Delay(10);
                }
                m_fishingHook.PullDown(force, time);

                //釣りゲームへ移行する
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
//            Debug.Log("魚がくいついた!");
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
            sprite.sprite = fishInfo.icon;
            HPbar.gameObject.SetActive(false);
//            Debug.Log("魚を捕まえた!");
            _state = FishState.Caught;
//            m_fishingHook.fishingGameMgr.FishingSucceeded();
        }

        /// <summary>
        /// 釣りに失敗したときの最初に行う処理
        /// 逃走状態に移行する
        /// </summary>
        public void SetEscaping()
        {
//            Debug.Log("魚は逃げ出した");
            transform.parent = null;
            _state = FishState.Escaping;
         //   m_fishingHook.fishingGameMgr.FishingFailed();
        }
        public void Damaged(float damage)
        {
            m_damageable.TakeDamage(damage);
        }

    }
}