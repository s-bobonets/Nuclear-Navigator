using System;
using UnityEngine;

public class Omnibus : MonoBehaviour
{
    private Data _data;
    private InputHandler _inputHandler;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private CollisionHandler _collisionHandler;
    private Navigator _navigator;

    private (float, float) _thrusts;

    private bool _playOnce;
    private bool _playedOnce;

    private void Awake()
    {
        _data = GetComponent<Data>();
        _inputHandler = GetComponent<InputHandler>();
        _levelManager = GetComponent<LevelManager>();
        _audioManager = GetComponent<AudioManager>();

        _navigator = GameObject.FindWithTag("Player").GetComponent<Navigator>();
        _collisionHandler = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();
    }

    private void Start() => _collisionHandler.OnCollide += Coll;

    private void Update()
    {
        UpdateInput();
        PlaySoundContinuously(_data.thrusterSound);
    }

    private void FixedUpdate() => _navigator.Move(_thrusts);
    private void UpdateInput() => _thrusts = _inputHandler.ReadThrusts();

    private void Coll(Collide.CollisionType type)
    {
        print($"touching {type}");

        switch (type)
        {
            case Collide.CollisionType.Start:
                break;
            case Collide.CollisionType.Finish:
                ResetThrust();
                PlaySoundOnce(_data.finishSound);

                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collide.CollisionType.Obstacle:
                ResetThrust();
                PlaySoundOnce(_data.collisionSound);

                _levelManager.OnNextLevel?.Invoke(true);
                break;
            case Collide.CollisionType.Bonus:
                //Legacy CollisionType from powerUp test
                //this shouldn't happen
                //and it never does =P
                break;
            default:
                //never ever would this get called
                print("the future is female!");
                break;
        }
    }

    private void PlaySoundOnce(AudioClip finishSound)
    {
        _playOnce = true;
        if (_playedOnce) return;
        _audioManager.OnPlaySound?.Invoke(finishSound, 1f, true);
        _playedOnce = true;
    }

    private void PlaySoundContinuously(AudioClip thrusterSound)
    {
        if (_playOnce) return;
        _audioManager.OnPlaySound?.Invoke(thrusterSound, _thrusts.Item1, false);
    }

    private void ResetThrust() => _thrusts = default;
}