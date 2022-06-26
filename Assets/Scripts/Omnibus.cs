using UnityEngine;

public class Omnibus : MonoBehaviour
{
    private AudioClipsAccess _gameAudioClipsAccess;
    private DataAccess _shipData;
    private Collider _shipCollider;

    private MeshRenderer[] _shipMRenderers;

    private ParticleSystem[] _psExhausts;
    private ParticleSystem[] _psRcsL;
    private ParticleSystem[] _psRcsR;

    private InputHandler _inputHandler;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private CollisionHandler _collisionHandler;
    private ShipController _shipController;

    private (float, float) _thrusts;

    private bool _playOnce;
    private bool _isPlaying;
    private bool _BlownUp;
    private bool _readInput = true;

    private void Awake()
    {
        _gameAudioClipsAccess = GetComponent<AudioClipsAccess>();

        _inputHandler = GetComponent<InputHandler>();
        _levelManager = GetComponent<LevelManager>();
        _audioManager = GetComponent<AudioManager>();

        _shipController = GameObject.FindWithTag("Player").GetComponent<ShipController>();
        _collisionHandler = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();

        _shipData = GameObject.FindWithTag("Player").GetComponent<DataAccess>();
        _shipCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();

        _psExhausts = _shipData.psExhausts;
        _psRcsL = _shipData.psStreamsL;
        _psRcsR = _shipData.psStreamsR;

        _shipMRenderers = _shipData.shipMRenderers;
    }

    private void Start()
    {
        _collisionHandler.OnCollide += Coll;
        _inputHandler.OnExitGame += Application.Quit;

        ProcessCheats();
    }

    private void ProcessCheats()
    {
        _inputHandler.OnNextLevel += () => { _levelManager.OnNextLevel?.Invoke(false); };
        _inputHandler.OnDisableCollisions += () => { _shipCollider.enabled = !_shipCollider.enabled; };
    }

    private void Update()
    {
        UpdateInput();
        PlaySounds();
        DrawExhaust();
        DrawRcs();
    }

    private void PlaySounds()
    {
        PlaySoundContinuously(_gameAudioClipsAccess.thrusterSound, _thrusts.Item1);
        PlaySoundContinuouslyExtra(_gameAudioClipsAccess.rcsSound, Mathf.Abs(_thrusts.Item2));
    }

    private void DrawRcs()
    {
        switch (_thrusts.Item2)
        {
            case > 0f:
                _shipData.goRcs[1].SetActive(true);

                foreach (var item in _psRcsR)
                {
                    var emission = item.emission;
                    var shape = item.shape;

                    emission.rateOverTime = 32f * _thrusts.Item2;
                    shape.scale = new Vector3(1f, 1f, _thrusts.Item2);
                }

                break;
            case < 0f:
                _shipData.goRcs[0].SetActive(true);


                foreach (var item in _psRcsL)
                {
                    var emission = item.emission;
                    var shape = item.shape;

                    emission.rateOverTime = 32f * Mathf.Abs(_thrusts.Item2);
                    shape.scale = new Vector3(1f, 1f, Mathf.Abs(_thrusts.Item2));
                }

                break;
            case 0f:
                foreach (var obj in _shipData.goRcs)
                    obj.SetActive(false);

                break;
        }
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
        _thrusts = _inputHandler.OnThrustersRead.Invoke();
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
                PlaySoundOnce(_gameAudioClipsAccess.finishSound);
                ActivateExplosion(_shipData.goWarp);

                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collide.CollisionType.Obstacle:
                ResetThrust();
                PlaySoundOnce(_gameAudioClipsAccess.collisionSound);
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
        if (_BlownUp) return;

        // if (!goParticle.activeSelf)
        goParticle.SetActive(true);

        goParticle.transform.SetParent(null);
        _BlownUp = true;
    }

    private void PlaySoundOnce(AudioClip finishSound)
    {
        _playOnce = true;
        if (_isPlaying) return;
        _audioManager.OnPlaySound?.Invoke(finishSound, 1f, true);
        _isPlaying = true;
    }

    private void PlaySoundContinuously(AudioClip clip, float value)
    {
        if (_playOnce) return;
        _audioManager.OnPlaySound?.Invoke(clip, value, false);
    }

    private void PlaySoundContinuouslyExtra(AudioClip clip, float value)
    {
        _audioManager.OnPlaySoundExtra?.Invoke(clip, value);
    }

    private void ResetThrust()
    {
        _readInput = false;
        _thrusts = default;
    }
}