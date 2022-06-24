using System;
using UnityEngine;

public class Omnibus : MonoBehaviour
{
    [Header("Data:")] [Space(12f)] [SerializeField]
    private AudioClip _thrusterSound;

    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _finishSound;

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
        PlaySound();
    }

    private void PlaySound()
    {
        if (_playOnce) return;
        _audioManager.OnPlaySound?.Invoke(_thrusterSound, _thrusts.Item1, false);
    }

    private void FixedUpdate() => _navigator.Move(_thrusts);
    private void UpdateInput() => _thrusts = _inputHandler.ReadThrusts();


    private void Coll(Collidable.CollisionType type)
    {
        print($"touching {type}");

        switch (type)
        {
            case Collidable.CollisionType.Start:
                break;
            case Collidable.CollisionType.End:
                ResetThrust();
                _playOnce = true;
                if (!_playedOnce)
                {
                    _audioManager.OnPlaySound?.Invoke(_finishSound, 1f, true);
                    _playedOnce = true;
                }

                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collidable.CollisionType.Obstacle:
                ResetThrust();
                _playOnce = true;
                if (!_playedOnce)
                {
                    _audioManager.OnPlaySound?.Invoke(_explosionSound, 1f, true);
                    _playedOnce = true;
                }

                _levelManager.OnNextLevel?.Invoke(true);
                break;
            case Collidable.CollisionType.Bonus:
                //Legacy CollisionType from powerup test
                //this shouldn't happen
                //and it never does =P
                break;
            default:
                //never ever this would get called
                print("the future is female!");
                break;
        }
    }

    private void ResetThrust() => _thrusts = default;
}