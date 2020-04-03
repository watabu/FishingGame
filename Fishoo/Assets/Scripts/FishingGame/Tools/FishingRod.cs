using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{
    //釣り竿
    public class FishingRod : FishingTool
    {
    //釣り竿を回転する軸
        public GameObject shaft;
        //釣り竿の画像
        public SpriteRenderer sprite;
        float MaxAngle_Left = 55;
        float MaxAngle_Right = -35;

        new public void PullToLeft()
        {
            Debug.Log(Mathf.DeltaAngle(shaft.transform.eulerAngles.z, MaxAngle_Left));
            if (Mathf.DeltaAngle(shaft.transform.eulerAngles.z,MaxAngle_Left ) > 0.1f)
            {
                shaft.transform.eulerAngles = new Vector3(0f,0f, Mathf.LerpAngle(shaft.transform.eulerAngles.z, MaxAngle_Left, Time.deltaTime*2));
   //             shaft.transform.Rotate(new Vector3(0f, 0f, 2f));
            }
        }
        new public void PullToRight()
        {
            Debug.Log(Mathf.DeltaAngle(shaft.transform.eulerAngles.z, MaxAngle_Right));
            if (Mathf.DeltaAngle(shaft.transform.eulerAngles.z, MaxAngle_Right) < -0.1f)
            {
                shaft.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(shaft.transform.eulerAngles.z, MaxAngle_Right, Time.deltaTime*2));
    //          shaft.transform.Rotate(new Vector3(0f,0f, -2f));
            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!linkMouse) return;
            //マウスに棒が追従するようにしている。画面をMaximizeしないとずれるので注意
            // Vector3でマウス位置座標を取得する
            var position = Input.mousePosition;
            // Z軸修正
            position.z = 10f;
            // マウス位置座標をスクリーン座標からワールド座標に変換する
           var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
            // ワールド座標に変換されたマウス座標を代入
            rotator.transform.position = screenToWorldPointPosition;
        }
    }
}