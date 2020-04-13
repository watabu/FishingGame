using UnityEngine;
using System.Collections;
using UnityEngine.Events;
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

        public float moveSpeed=0.1f;

        [Header("Object References")]
        public FishMove fishMove;

        [Tooltip("現在体力")]
        public float hp;

        [Tooltip("魚がエサを狙っているかなどの状態")]
        public FishState state = FishState.Nomal;

        [Tooltip("移動方向")]
        public Vector2 escapeDir;
        [Tooltip("体力が尽きたか")]
        public bool IsDead;

        [Tooltip("魚の画像")]
        public SpriteRenderer sprite;

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
            //釣り針の取得
                if(m_fishingHook==null)
                    m_fishingHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<FishingGame.Tools.FishingHook>();
                return m_fishingHook;
            }
        }

        private void Awake()
        {
            hp = fishData.status.hpMax;
            if (myUpdate== null)
                myUpdate = new UnityEvent();
            if(sprite == null)
            {
                //画像が指定されていなければ子供のオブジェクトから画像を得る
                sprite = transform.GetComponentInChildren<SpriteRenderer>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            myUpdate.Invoke();
            //毎フレーム回復する
            Regene();

            //関数ポインタ的なの使いてえ！
            //釣りに失敗したら逃げる
            if (state == FishState.Escaping)
            {
                transform.position += new Vector3(escapeDir.x, escapeDir.y, 0);
            }
            //エサを狙っている
            else if (state == FishState.Approaching)
            {
                MoveToHook();
            }
            //釣りゲーム中
            else if (state == FishState.Biting)
            {

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
        /// 魚が現れる
        /// </summary>
        public void SetAppear()
        {
            myUpdate.AddListener(Appear);
        }
        void Appear()
        {
            float speed = 0.01f;
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
        /// 魚が消える
        /// </summary>
        public void SetDisAppear()
        {
            myUpdate.AddListener(DisAppear);
        }
        void DisAppear()
        {
            float speed = 0.01f;
            Color color = sprite.color;
            color.a -= speed;
            sprite.color = color;
            if(sprite.color.a <= 0.01f)
            {
                myUpdate.RemoveListener(DisAppear);

            }
        }

        /// <summary>
        /// とくに目的を持たず自由に動く
        /// </summary>
        void MoveFree()
        {

        }


        /// <summary>
        /// 釣り針の方に近づいていく
        /// </summary>
        void MoveToHook()
        {
            transform.position = Vector3.MoveTowards(transform.position, FishingHook.transform.position, moveSpeed);
        }

        public bool IsNearHook()
        {
            return (FishingHook.transform.position - transform.position).sqrMagnitude < 5f;
        }
        /// <summary>
        /// 針に食いついた
        /// </summary>
        public void Biting()
        {
            //座標を釣り針に固定する
            transform.parent = FishingHook.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            state = FishState.Biting;
        }

        /// <summary>
        /// 釣りに失敗して逃げる
        /// </summary>
        public void Escape()
        {
            transform.parent = null;
            state = FishState.Escaping;
            float[] dirXRnd = { 1f, -1f };
            float dirX = dirXRnd[Random.Range(0, 2)];
            dirX /= 8;
            escapeDir = new Vector2(dirX, 0);
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
            hp += fishData.status.hpRegene;
            hp = Mathf.Clamp(hp, 0f, fishData.status.hpMax);
        }



    }
}