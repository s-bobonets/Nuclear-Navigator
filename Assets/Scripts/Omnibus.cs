using UnityEngine;

public class Omnibus : MonoBehaviour
{
    private Data _data;
    private InputHandler _inputHandler;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private CollisionHandler _collisionHandler;
    private Navigator _navigator;

    private ParticleAccess _particles;
    private ParticleSystem _trail;

    private (float, float) _thrusts;

    private bool _playSoundOnce;
    private bool _playedSoundOnce;
    private bool _exploded;

    private void Awake()
    {
        _data = GetComponent<Data>();
        _inputHandler = GetComponent<InputHandler>();
        _levelManager = GetComponent<LevelManager>();
        _audioManager = GetComponent<AudioManager>();

        _navigator = GameObject.FindWithTag("Player").GetComponent<Navigator>();
        _collisionHandler = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();

        _particles = GameObject.FindWithTag("Player").GetComponent<ParticleAccess>();
        _trail = _particles.streamFlame.GetComponent<ParticleSystem>();
    }

    private void Start() => _collisionHandler.OnCollide += Coll;

    private void Update()
    {
        UpdateInput();
        PlaySoundContinuously(_data.thrusterSound);
        PlayTrail();
    }

    private void PlayTrail()
    {
        ActivateParticles(_particles.streamFlame, false);

        var emission = _trail.emission;
        var shape = _trail.shape;

        emission.rateOverTime = 32f * _thrusts.Item1;
        shape.scale = new Vector3(1f, 1f, _thrusts.Item1);
    }

    private void FixedUpdate() => _navigator.Move(_thrusts);
    private void UpdateInput() => _thrusts = _inputHandler.ReadThrusts();

    private void Coll(Collide.CollisionType type)
    {
        // print($"touching {type}");

        switch (type)
        {
            case Collide.CollisionType.Start:
                break;
            case Collide.CollisionType.Finish:
                ResetThrust();
                PlaySoundOnce(_data.finishSound);
                ActivateParticles(_particles.success);

                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collide.CollisionType.Obstacle:
                ResetThrust();
                PlaySoundOnce(_data.collisionSound);
                ActivateParticles(_particles.explosion);

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

    private void ActivateParticles(GameObject particleSpawner, bool resetParent = true)
    {
        if (_exploded) return;

        if (!particleSpawner.activeSelf)
            particleSpawner.SetActive(true);

        if (!resetParent) return;
        particleSpawner.transform.SetParent(null);
        _exploded = true;
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

    private void ResetThrust() => _thrusts = default;
}