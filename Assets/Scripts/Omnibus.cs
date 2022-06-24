using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Omnibus : MonoBehaviour
{
    [Header("Data:")] [Space(12f)] [SerializeField]
    private AudioClip _thrusterSound;

    [SerializeField] private AudioClip _explosionSound;

    private InputHandler _inputHandler;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private CollisionHandler _collisionHandler;

    public Action<Collidable.CollisionType> OnColl;

    public float Thrust { get; private set; }
    public float Turn { get; private set; }

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _levelManager = GetComponent<LevelManager>();
        _audioManager = GetComponent<AudioManager>();
        _collisionHandler = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();

        _collisionHandler.OnCollide += Coll;
    }

    // private void Update() => _audioManager.PlayThruster?.Invoke(Thrust, _thrusterSound, true);

    private void Coll(Collidable.CollisionType type)
    {
        print($"touching {type}");

        switch (type)
        {
            case Collidable.CollisionType.Start:
                break;
            case Collidable.CollisionType.End:
                ResetThrust();
                //place particles
                //play audio jingle
                _levelManager.OnNextLevel?.Invoke(false);
                break;
            case Collidable.CollisionType.Obstacle:
                ResetThrust();
                //place particles
                //play explosion audio
                _levelManager.OnNextLevel?.Invoke(true);
                break;
            case Collidable.CollisionType.Bonus:
                //this shouldn't happen
                //and it never does =P
                break;
            default:
                print("the future is female!");
                break;
        }
    }

    private void ResetThrust()
    {
        _inputHandler.Thrust -= PopThrust;
        _inputHandler.Turn -= PopTurn;

        Thrust = 0f;
        Turn = 0f;
    }

    private void Start()
    {
        _inputHandler.Thrust += PopThrust;
        _inputHandler.Turn += PopTurn;

        _audioManager.PlayThruster?.Invoke(Thrust, _thrusterSound, false);
    }

    private void PopTurn(float value) => Turn = value;
    private void PopThrust(float value) => Thrust = value;
}