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
        public FishStatus status;

        [Header("Object References")]
       public  FishingGame.Tools.FishingHook fishingHook;
        public FishMove fishMove;

        float m_HP;

        bool isEscaping=false;

        //hpが尽きたかどうか
        bool m_IsDead = false;
        public bool IsDead { get { return m_IsDead; } }

        Vector2 escapeDir;
        //釣りゲーム前(針に食いつく前)

        //針に食いついた
        public void Biting()
        {
            transform.parent = fishingHook.transform;
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
            m_HP += status.hpRegene;
            m_HP = Mathf.Clamp(m_HP, 0f, status.hpMax);
        }

        // Use this for initialization
        void Start()
        {
            //ステータスの初期化
            status = fishData.status;
            m_HP = status.hpMax;
            //釣り針の取得
            fishingHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<FishingGame.Tools.FishingHook>();
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

        }
    }
}