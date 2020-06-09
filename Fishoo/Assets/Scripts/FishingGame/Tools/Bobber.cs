using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Tools
{

    /// <summary>
    /// 釣浮き
    /// </summary>
    public class Bobber :  MonoBehaviour,FishingTool
    {

        public SpriteRenderer sprite;
        [SerializeField] AudioClip waterSE;
        bool isThrown;

        AudioSource audio;
        /// <summary>
        /// 釣り具を展開する
        /// </summary>
        public void ExpandTools()
        {
            isThrown = true;
        }

        /// <summary>
        /// 釣りが終わり釣り具を収納する
        /// </summary>
        public void RetrieveTools()
        {
            isThrown = false;


        }
        public void SetInvisible()
        {
            var color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }
        public void SetVisible()
        {
            var color = sprite.color;
            color.a = 1;
            sprite.color = color;
        }

        // Start is called before the first frame update
        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ocean") && isThrown)
            {
                audio.Play();
            }
        }
    }
}