using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Controls _controls;
    private Rigidbody _rigidbody;

    [SerializeField] private float _rotateThrustMult;
    [SerializeField] private float _forwardThrustMult;

    private float _thrustForward;
    private float _thrustRotate;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _thrustForward = _controls.GameMap.Thrust.ReadValue<float>();
        _thrustRotate = _controls.GameMap.Rotate.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(transform.up * (_thrustForward * _forwardThrustMult));
        _rigidbody.AddTorque(Vector3.forward * (_thrustRotate * _rotateThrustMult));
    }
}