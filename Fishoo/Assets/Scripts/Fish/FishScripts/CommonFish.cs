using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading.Tasks;

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


    public class CommonFish : MonoBehaviour
    {
        public FishData fishData;
        public FishMoveData fishMoveData;

        public float moveSpeed=0.05f;

        [Header("Object References")]
        public FishMove fishMove;

        [Tooltip("現在体力")]
        public float hp;

        [SerializeField,　Tooltip("魚がエサを狙っているかなどの状態")]
        private FishState _state;

        /// <summary>
        /// 魚の状態(読み取り専用)
        /// </summary>
        public FishState state { get { return _state; } }

        [Tooltip("逃げる方向")]
        public Vector2 escapeDir;
        [Tooltip("体力が尽きたか")]
        public bool IsDead;

        [Tooltip("魚の画像")]
        public SpriteRenderer sprite;

        [Tooltip("体力バー")]
        public Slider HPbar;

        /// <summary>
        /// 魚が湧いてから経った時間
        /// </summary>
        public int LivingTime;
        /// <summary>
        /// 自由に動くときの中心座標
        /// </summary>
        Vector3 neutralPos;

        /// <summary>
        /// 一時的に実行したい関数を入れる
        /// </summary>
        UnityEvent myUpdate;
        //public delegate void myUpdate();


        static private FishingGame.Tools.FishingHook m_fishingHook;
        static public  FishingGame.Tools.FishingHook FishingHook
        {
            get
            {
                return m_fishingHook;
            }
        }

        private void Awake()
        {
            neutralPos = transform.position;
            hp = fishMoveData.status.hpMax;
            if (myUpdate== null)
                myUpdate = new UnityEvent();
            if(sprite == null)
            {
                //画像が指定されていなければ子供のオブジェクトから画像を得る
                sprite = transform.GetComponentInChildren<SpriteRenderer>();
            }
            HPbar.maxValue = hp;

            //釣り針の取得
            if (m_fishingHook == null)
                m_fishingHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<FishingGame.Tools.FishingHook>();

            //魚をフェードインする
            Color color = sprite.color;
            color.a = 0;
            sprite.color = color;
            SetAppear();
        }
        
        // Update is called once per frame
        void Update()
        {
            //一時的に行う関数などを行う
            myUpdate.Invoke();
            //回復する
            Regene();

            //寿命が尽きたら消える            
            if (　++LivingTime > fishMoveData.lifeTime && state == FishState.Nomal)
            {
                SetDisAppear();
            }

            //釣りに失敗したら逃げる
            if (state == FishState.Escaping)
            {
                Escape();
            }
            //エサを狙っている
            else if (state == FishState.Approaching)
            {
                ApproachHook();
            }
            //釣りゲーム中
            else if (state == FishState.Biting)
            {
                Bite();
            }
            //捕まった
            else if (state == FishState.Caught)
            {

            }
            //通常
            else if (state == FishState.Nomal)
            {
                MoveFree();

            }

        }

        //釣りゲーム外

        /// <summary>
        /// 魚が現れる時に最初に行う処理
        /// </summary>
        public void SetAppear()
        {
            Debug.Log("魚が現れた");
            myUpdate.AddListener(Appear);
            HPbar.gameObject.SetActive(false);
            _state = FishState.Nomal;
        }
        void Appear()
        {
            float speed = 0.004f;
            Color color = sprite.color;
            color.a += speed;
            sprite.color = color;
            if (sprite.color.a >= 0.99f)
            {
                color.a = 1.0f;
                sprite.color = color;
                myUpdate.RemoveListener(Appear);
            }
        }

        
        /// <summary>
        /// 魚が消える時に最初に行う処理
        /// </summary>
        public void SetDisAppear()
        {
            Debug.Log("魚は寿命をまっとうした。");
            myUpdate.AddListener(DisAppear);
        }
        void DisAppear()
        {
            float speed = 0.001f;
            Color color = sprite.color;
            color.a -= speed;
            sprite.color = color;
            if(sprite.color.a <= 0.01f)
            {
                myUpdate.RemoveListener(DisAppear);
                gameObject.SetActive(false);
            }
        }

       
        /// <summary>
        /// 自由に動く時の速さ(半径)
        /// </summary>
        float speed = 2;

        /// <summary>
        /// とくに目的を持たず自由に動く
        /// </summary>
        void MoveFree()
        {
            float x = neutralPos.x + speed * Mathf.Sin(Time.time);
            transform.position = new Vector3(x, neutralPos.y, neutralPos.z);

        }

        /// <summary>
        /// 釣り針を狙うときに行う最初の処理
        /// </summary>
        public void SetApproaching()
        {
            Debug.Log("魚がエサを狙っている");
            _state = FishState.Approaching;

        }

        //非同期処理を一回だけ行うための変数
        bool isDone=false;

        /// <summary>
        /// 釣り針を狙う
        /// 釣りゲーム開始前のミニゲーム
        /// </summary>
        async void ApproachHook()
        {
            if (!isDone)
            {
                MoveToHook();
            }
            //十分近づいたら針をつんつんする
            if (IsNearHook() && !isDone)
            {
                isDone = true;
                //座標を釣り針中心にする
                transform.parent = FishingHook.transform;


                //1回目
                float force = 4;
                int time = 200;
                while (!IsNearHook())
                {
                    MoveToHook();
                    await Task.Delay(10);
                }
                Debug.Log("1");
                FishingHook.PullDown(force, time);
                await Task.Delay(time);
                LeaveFromHook();
                await Task.Delay(Random.Range(time * 10, time * 30));
                
                //二回目
                Debug.Log("2");
                time = 150;
                while (!IsNearHook())
                {
                    MoveToHook();
                    await Task.Delay(10);
                }
                FishingHook.PullDown(8, time);
                await Task.Delay(time);
                LeaveFromHook();
                await Task.Delay(Random.Range(time * 10, time * 30));
                
                //Debug.Log("3");
                //time = 300;
                //FishingHook.PullDown(3, time);
                //await Task.Delay(Random.Range(time * 10, time * 30));

                //Debug.Log("4");
                //time = 150;
                //FishingHook.PullDown(5, time);
                //await Task.Delay(Random.Range(time * 10, time * 30));

                Debug.Log("5");
                time = 80;
                while (!IsNearHook())
                {
                    MoveToHook();
                    await Task.Delay(10);
                }
                FishingHook.PullDown(40, time);

                //食いついたときになにかを入力して釣りゲームへ移行する
                SetBiting();
            }
         
        }

        /// <summary>
        /// 釣り針の方に近づいていく
        /// </summary>
        void MoveToHook()
        {
            transform.position = Vector3.MoveTowards(transform.position, FishingHook.transform.position, moveSpeed);
        }

        /// <summary>
        /// 針をつついた後に向かう針から離れた点
        /// </summary>
        Vector3 LeavePoint;
        void LeaveFromHook()
        {
            LeavePoint = FishingHook.transform.position;
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
            return (FishingHook.transform.position - transform.position).sqrMagnitude < distance;
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
            m_fishingHook.fishingGameMgr.targetFish = this;
            m_fishingHook.fishingGameMgr.StartFishing();
        }

        /// <summary>
        /// 釣りゲーム中の処理
        /// </summary>
        void Bite()
        {
            if (IsDead)
            {
                SetCaught();

            }

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
            float[] dirXRnd = { 1f, -1f };
            float dirX = dirXRnd[Random.Range(0, 2)];
            dirX /= 8;
            escapeDir = new Vector2(dirX, 0);
        }

        /// <summary>
        /// 逃走中の処理
        /// </summary>
        void Escape()
        {
            transform.position += new Vector3(escapeDir.x, escapeDir.y, 0);
        }

        // 釣りゲーム中


        /// <summary>
        /// ダメージを食らう
        /// </summary>
        /// <param name="damage"></param>
        public void Damaged(float damage)
        {
            if (IsDead) return;
            hp -= damage;
            HPbar.value = hp;
            if (hp < 0)
            {
                hp = 0;
                IsDead = true;
            }
        }

        /// <summary>
        /// 体力を回復
        /// </summary>
        void Regene()
        {
            if (IsDead) return;
            hp += fishMoveData.status.hpRegene*Time.deltaTime;
            hp = Mathf.Clamp(hp, 0f, fishMoveData.status.hpMax);
            HPbar.value = hp;
        }



    }
}