using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Action<(float, float)> Move;
    private Rigidbody _rigidbody;

    //move to settings on bus later
    [SerializeField] private float _rotateThrustMult;
    [SerializeField] private float _forwardThrustMult;

    private float _thrustForward;
    private float _thrustRotate;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        Move += ControlShip;
    }

    private void ControlShip((float thrust, float rotation) value)
    {
        _rigidbody.AddForce(transform.up * (value.thrust * _forwardThrustMult));
        _rigidbody.AddTorque(Vector3.forward * (value.rotation * _rotateThrustMult));
    }
}