using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource[] _audioSource;

    public Action<AudioClip, float, bool> OnPlaySound;
    public Action<AudioClip, float> OnPlaySoundExtra;

    private void Awake()
    {
        _audioSource = GameObject.FindWithTag("Player").GetComponents<AudioSource>();

        OnPlaySound += PlaySound;
        OnPlaySoundExtra += PlaySoundExtra;
    }

    private void PlaySound(AudioClip clip, float value, bool once)
    {
        if (!once)
        {
            _audioSource[0].volume = value;

            if (!_audioSource[0].isPlaying)
                _audioSource[0].PlayOneShot(clip);
            if (value == 0f)
                _audioSource[0].Stop();
        }
        else
        {
            _audioSource[0].volume = 5f;
            _audioSource[0].Stop();
            _audioSource[0].PlayOneShot(clip);
        }
    }

    private void PlaySoundExtra(AudioClip clip, float value)
    {
        _audioSource[1].volume = value;

        if (!_audioSource[1].isPlaying)
            _audioSource[1].PlayOneShot(clip);
        if (value == 0f)
            _audioSource[1].Stop();
    }
}