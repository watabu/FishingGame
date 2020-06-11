using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// セーブデータ破壊時の音
/// </summary>
public class DestroySound : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;

    public void PlaySound()
    {
        audioSource = GetComponent<AudioSource>();
        foreach(var audio in audioClips)
        {
            audioSource.PlayOneShot(audio);

        }
    }


}
