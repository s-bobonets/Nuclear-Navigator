using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Controls _controls;

    public Action<float> Thrust;
    public Action<float> Turn;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }

    private void Update()
    {
        Thrust?.Invoke(_controls.GameMap.Thrust.ReadValue<float>());
        Turn?.Invoke(_controls.GameMap.Rotate.ReadValue<float>());
    }
}