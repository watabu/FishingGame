using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FishingGame.Tools
{
    public class Bucket : MonoBehaviour, FishingTool
    {

        [SerializeField, Tooltip("画像")] SpriteRenderer sprite;
        [SerializeField, Tooltip("釣った魚のリスト")] List<Fish.Behavior.CommonFish> CaughtFishList;

        /// <summary>
        /// 釣った魚をバケツに入れる
        /// </summary>
        /// <param name="fish"></param>
        public void SwallowFish(Fish.Behavior.CommonFish fish)
        {
            CaughtFishList.Add(fish);
            _SwallowFish(fish);
        }

        /// <summary>
        /// 釣られた魚がバケツに入るまでの動き
        /// </summary>
        async　void _SwallowFish(Fish.Behavior.CommonFish fish)
        {
            await Task.Delay(800);
            fish.transform.parent = gameObject.transform;

            GameObject obj = fish.gameObject;
            //バケツの上
            Vector3 AheadOfBucket = transform.position +  new Vector3(0, 4, 0);
            while((AheadOfBucket - obj.transform.position).sqrMagnitude > 0.01f)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, AheadOfBucket, 0.3f);
                await Task.Delay(20);
            }
            fish.SetDisAppear();
            while ((transform.position - obj.transform.position).sqrMagnitude > 0.01f)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, 0.3f);
                await Task.Delay(20);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            CaughtFishList = new List<Fish.Behavior.CommonFish>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 釣り具を展開する
        /// </summary>
        public void ExpandTools()
        {

        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {

        }
        public void PullToLeft()
        {

        }
        public void PullToRight()
        {

        }

        public void SetInvisible()
        {
            var color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }
    }
}