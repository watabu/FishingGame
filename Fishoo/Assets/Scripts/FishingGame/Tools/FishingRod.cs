using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{
    //釣り竿
    public class FishingRod : FishingTool
    {

        public GameObject rotator;

        public bool linkMouse;

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