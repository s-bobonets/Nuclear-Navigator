using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public Action<AudioClip, float, bool> OnPlaySound;

    private void Awake()
    {
        _audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();

        OnPlaySound += PlaySound;
    }

    private void PlaySound(AudioClip clip, float value, bool once)
    {
        if (!once)
        {
            _audioSource.volume = value;

            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(clip);
            if (value == 0f)
                _audioSource.Stop();
        }
        else
        {
            _audioSource.volume = 5f;
            _audioSource.Stop();
            _audioSource.PlayOneShot(clip);
        }
    }
}