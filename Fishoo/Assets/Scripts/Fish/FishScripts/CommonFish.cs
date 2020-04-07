using UnityEngine;
using System.Collections;

namespace Fish.FishScripts
{
    //普通の魚
    public class CommonFish : MonoBehaviour
    {
        public FishData fishData;
        public FishMoveData fishMoveData;
        public FishMove fishMove;
        public FishStatus status;

       public  FishingGame.Tools.FishingHook fishingHook;
        bool isEscaping=false;
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
            float dirX = (int)Random.value % 2 == 0 ? 1 : -1;
            dirX /= 8;
            escapeDir = new  Vector2(dirX, 0);
        }



        //釣りゲーム中

        //ダメージを食らう
        public void Damaged(float damage)
        {
            status.hp -= damage;
            if (status.hp < 0)
                status.hp = 0;
        }

        //体力回復
        void Regene()
        {
            if (status.hp <= 0)
            {
                status.hp = 0;
                return;
            }
            if (status.hp < status.hpMax)
            {
                status.hp += status.hpRegene;
            }
            if (status.hp > status.hpMax)
                status.hp = status.hpMax;
        }

        //hpが尽きたかどうか
        public bool isDead()
        {
            return (status.hp <= 0);
        }

        // Use this for initialization
        void Start()
        {
            //ステータスの初期化
            status = fishData.status;
            status.hp = status.hpMax;
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