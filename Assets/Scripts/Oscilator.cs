using System;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [Header("Movement")] [Space] [SerializeField]
    private Vector3 _direction;

    [SerializeField] [Range(0f, 10f)] private float _amount;
    [SerializeField] [Range(0f, 1f)] private float _factor;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        var tran = transform;
        var offset = _direction * (_amount * _factor);
        var v = tran.right * offset.x + tran.up * offset.y + tran.forward * offset.z;
        
        tran.position = _startPos + v;
    }
}