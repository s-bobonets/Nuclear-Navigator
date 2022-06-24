using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public Action<float, AudioClip, bool> PlayThruster;

    private void Awake()
    {
        _audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();

        PlayThruster += Play;
    }

    private void Play(float volume, AudioClip clip, bool controlVolume)
    {
        if (controlVolume)
            _audioSource.volume = volume * .2f;

        if (_audioSource.isPlaying) return;


        _audioSource.PlayOneShot(clip);
    }
}