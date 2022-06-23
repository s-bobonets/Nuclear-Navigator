using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Controls ControlsInstance;
    private Rigidbody _rigidbody;

    [SerializeField] private float _rotateThrustMult;
    [SerializeField] private float _forwardThrustMult;

    private float _thrustForward;
    private float _thrustRotate;

    private void Awake()
    {
        ControlsInstance = new Controls();
        ControlsInstance.Enable();
    }

    private void OnDestroy()
    {
        ControlsInstance.Disable();
    }

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _thrustForward = ControlsInstance.GameMap.Thrust.ReadValue<float>();
        _thrustRotate = ControlsInstance.GameMap.Rotate.ReadValue<float>();
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