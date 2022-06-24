using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Omnibus _bus;
    private Rigidbody _rigidbody;

    [SerializeField] private float _rotateThrustMult;
    [SerializeField] private float _forwardThrustMult;

    private float _thrustForward;
    private float _thrustRotate;

    private void Start()
    {
        _bus = GameObject.FindWithTag("GameController").GetComponent<Omnibus>();

        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _thrustForward = _bus.Thrust;
        _thrustRotate = _bus.Turn;
    }

    private void FixedUpdate()
    {
        ControlShip();
    }

    private void ControlShip()
    {
        _rigidbody.AddForce(transform.up * (_thrustForward * _forwardThrustMult));
        _rigidbody.AddTorque(Vector3.forward * (_thrustRotate * _rotateThrustMult));
    }
}