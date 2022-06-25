using System;
using UnityEngine;

public class Omnibus : MonoBehaviour
{
    private Data _gameData;

    private DataAccess _shipData;
    private ParticleSystem[] _psExhausts;
    private MeshRenderer[] _shipMRenderers;

    private InputHandler _inputHandler;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private CollisionHandler _collisionHandler;
    private ShipController _shipController;

    private (float, float) _thrusts;

    private bool _playSoundOnce; // remove one of these
    private bool _playedSoundOnce;
    private bool _itsBlown;
    private bool _readInput = true;

    private void Awake()
    {
        _gameData = GetComponent<Data>();
        _inputHandler = GetComponent<InputHandler>();
        _levelManager = GetComponent<LevelManager>();
        _audioManager = GetComponent<AudioManager>();

        _shipController = GameObject.FindWithTag("Player").GetComponent<ShipController>();
        _collisionHandler = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();

        _shipData = GameObject.FindWithTag("Player").GetComponent<DataAccess>();
        _psExhausts = _shipData.psExhausts;

        _shipMRenderers = _shipData.shipMRenderers;
    }

    private void Start() => _collisionHandler.OnCollide += Coll;

    private void Update()
    {
        UpdateInput();
        PlaySoundContinuously(_gameData.thrusterSound);
        DrawExhaust();
    }

    private void DrawExhaust()
    {
        foreach (var item in _psExhausts)
        {
            var emission = item.emission;
            var shape = item.shape;

            emission.rateOverTime = 32f * _thrusts.Item1;
            shape.scale = new Vector3(1f, 1f, _thrusts.Item1);
        }
    }

    private void FixedUpdate() => _shipController.Move(_thrusts);

    private void UpdateInput()
    {
        if (!_readInput) return;
        _thrusts = _inputHandler.ReadThrusts();
    }

    private void Coll(Collide.CollisionType type)
    {
        // print($"touching {type}");

        switch (type)
        {
            case Collide.CollisionType.Start:
                break;
            case Collide.CollisionType.Finish:
                ResetThrust();
                PlaySoundOnce(_gameData.finishSound);
                ActivateExplosion(_shipData.goWarp);

                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collide.CollisionType.Obstacle:
                ResetThrust();
                PlaySoundOnce(_gameData.collisionSound);
                ActivateExplosion(_shipData.goExplosion);
                HideShip();

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

    private void HideShip()
    {
        foreach (var item in _shipMRenderers)
        {
            item.enabled = false;
        }
    }

    private void ActivateExplosion(GameObject goParticle)
    {
        // we never need more than one type of ship explosion in a level
        if (_itsBlown) return;

        // if (!goParticle.activeSelf)
        goParticle.SetActive(true);

        goParticle.transform.SetParent(null);
        _itsBlown = true;
    }

    private void PlaySoundOnce(AudioClip finishSound)
    {
        _playSoundOnce = true;
        if (_playedSoundOnce) return;
        _audioManager.OnPlaySound?.Invoke(finishSound, 1f, true);
        _playedSoundOnce = true;
    }

    private void PlaySoundContinuously(AudioClip thrusterSound)
    {
        if (_playSoundOnce) return;
        _audioManager.OnPlaySound?.Invoke(thrusterSound, _thrusts.Item1, false);
    }

    private void ResetThrust()
    {
        _readInput = false;
        _thrusts = default;
    }
}