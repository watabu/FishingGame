using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish.Behavior
{
    /// <summary>
    /// 釣りゲーム中の魚の動きを管理する
    /// </summary>
    public class FishMove : MonoBehaviour
    {
        [SerializeField] private Behavior.CommonFish fish;
        [SerializeField] private Fish.FishMoveData data;

        [Tooltip("移動方向"), ReadOnly]
        public Vector2 moveDirection;

        [Header("Object References")]
        [ReadOnly, SerializeField] KeyCode CurrentKey= KeyCode.None;

        private void Update()
        {
            if (fish.state == Behavior.FishState.Biting && CurrentKey!= KeyCode.None)
            {
                fish.transform.localPosition = KeyToVec3(CurrentKey) *2;
            }
        }



        public void ReceiveNextKey(KeyCode keyCode)
        {
            CurrentKey = keyCode;
        }

        /// <summary>
        ///     Y
        ///  X      B
        ///     A
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        Vector3 KeyToVec3(KeyCode keyCode)
        {
            Vector3 dir = new Vector3(0, 0, 0);
            switch (keyCode)
            {
                case KeyCode.A:
                    dir.y = 1;
                    dir.x = -1;
                    break;
                case KeyCode.B:
                    dir.y = 1;
                    dir.x = 2;
                    break;
                case KeyCode.X:
                    dir.x = -1;
                    break;
                case KeyCode.Y:
                    dir.y = 1;
                    break;
                case KeyCode.RightArrow:
                    dir.x = 2;
                    break;
                case KeyCode.LeftArrow:
                    dir.x = -1;
                    break;
                case KeyCode.UpArrow:
                    dir.y = 1;
                    break;
                case KeyCode.DownArrow:
                    dir.y = -1;
                    break;
                default:
                    Debug.LogError("想定されていないキーです。");
                    break;
            }
            return -dir;
        }

    }
}