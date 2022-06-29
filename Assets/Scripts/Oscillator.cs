using System;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Right,
        Forward
    }

    private enum Mode
    {
        Local,
        Global
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    [Header("Movement")] [Space] [SerializeField]
    private Mode _mode;

    [SerializeField] private Direction _direction;

    [SerializeField] private bool _inverted;

    [Header("Custom Vector")] [Space] [SerializeField]
    private bool _use;

    [SerializeField] private Vector3 _customDirection;

    [Header("Oscillation Settings")] [Space] [SerializeField] [Range(0f, 20f)]
    private float _amplitude;

    [SerializeField] [Range(.1f, 10f)] private float _period;

    private float _factor;
    private Vector3 _dir;
    private Vector3 _startPos;
    private AudioSource _audioSource;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void SetDirection()
    {
        if (_use)
        {
            _dir = _customDirection;
        }
        else
        {
            _dir = _direction switch
            {
                Direction.Up => _inverted ? Vector3.down : Vector3.up,
                Direction.Right => _inverted ? Vector3.left : Vector3.right,
                Direction.Forward => _inverted ? Vector3.back : Vector3.forward,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private void GenerateSin()
    {
        var cycles = Time.time / _period;
        const float tau = Mathf.PI * 2f;
        var sinWave = Mathf.Sin(cycles * tau);
        _factor = (sinWave + 1f) * .5f;

        PlayNoise(_factor);
    }

    private void PlayNoise(float value)
    {
        _audioSource.volume = value * .05f;

        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    private void Update()
    {
        SetDirection();
        GenerateSin();

        var tran = transform;
        var gOffset = _dir * (_amplitude * _factor);
        var lOffset = tran.right * gOffset.x + tran.up * gOffset.y + tran.forward * gOffset.z;

        tran.position = _mode switch
        {
            Mode.Local => _startPos + lOffset,
            Mode.Global => _startPos + gOffset,
            _ => tran.position
        };
    }
}