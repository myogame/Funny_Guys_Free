using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioFX(int soundNumber)
    {
       audioSource.clip = audioClips[soundNumber];
       audioSource.PlayOneShot(audioSource.clip);
       audioSource.Play();
    }
}
