using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();
    }

    public (float, float) ReadThrusts()
    {
        return (_controls.GameMap.Thrust.ReadValue<float>(),
            _controls.GameMap.Rotate.ReadValue<float>());
    }

    private void OnDisable() => _controls.Disable();
    private void OnDestroy() => _controls.Disable();
}