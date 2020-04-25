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

        AudioSource audio;
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
        public void SetInvisible()
        {
            var color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }

        public void PullToLeft()
        {
        }

        public void PullToRight()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            audio = GetComponent<AudioSource>();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ocean"))
            {
                audio.Play();

            }
        }
    }
}