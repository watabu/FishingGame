using UnityEngine;
using System.Collections;

namespace Fish.FishScripts
{
    /// <summary>
    /// 普通の魚
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CommonFish : MonoBehaviour
    {
        public FishData fishData;
        public FishMoveData fishMoveData;

        public float moveSpeed=0.1f;

        [Header("Object References")]
        public FishMove fishMove;

        float m_HP;

        bool isEscaping=false;

        //hpが尽きたかどうか
        bool m_IsDead = false;
        public bool IsDead { get { return m_IsDead; } }

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


     Vector2 escapeDir;
        //釣りゲーム前(針に食いつく前)

        //針に食いついた
        public void Biting()
        {
            //座標を釣り針に固定する
            transform.parent = FishingHook.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            isEscaping = false;
            fishMove.isFishing = true;
        }
        //釣りに失敗して逃げる
        public void Escape()
        {
            transform.parent = null;
            isEscaping = true;
            float[] dirXRnd = { 1f, -1f };
            float dirX = dirXRnd[Random.Range(0, 2)];
            dirX /= 8;
            escapeDir = new Vector2(dirX, 0);
        }



        //釣りゲーム中

        //ダメージを食らう
        public void Damaged(float damage)
        {
            if (IsDead) return;
            m_HP -= damage;
            if (m_HP < 0)
            {
                m_HP = 0;
                m_IsDead = true;
            }
        }

        //体力回復
        void Regene()
        {
            if (IsDead) return;
            m_HP += fishData.status.hpRegene;
            m_HP = Mathf.Clamp(m_HP, 0f, fishData.status.hpMax);
        }
        private void Awake()
        {
            m_HP = fishData.status.hpMax;
        }

        // Update is called once per frame
        void Update()
        {
            //毎フレーム回復する
            Regene();

            //テスト　食いつく
            if (Input.GetKeyDown(KeyCode.Space))
                Biting();
            //テスト 逃げる
            else if(Input.GetKeyDown(KeyCode.UpArrow)){
                Escape();
            }
            //釣りに失敗したら逃げる
            if (isEscaping)
            {
                transform.position += new Vector3(escapeDir.x, escapeDir.y, 0);
            }
            else
            {
                MoveToBobber();
            }

        }

        /// <summary>
        /// 浮きの方に近づいていく
        /// </summary>
        void MoveToBobber()
        {
            transform.position= Vector3.MoveTowards(transform.position, FishingHook.transform.position, moveSpeed);
        }

        public bool IsNearBobber()
        {
            return (FishingHook.transform.position - transform.position).sqrMagnitude < 5f;
        }
    }
}