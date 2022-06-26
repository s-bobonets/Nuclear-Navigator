using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Controls _controls;

    public Action OnNextLevel;
    public Action OnDisableCollisions;

    public Func<(float, float)> OnThrustersRead;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();

        _controls.CheatMap.NextLevel.started += _ => { OnNextLevel?.Invoke(); };
        _controls.CheatMap.DisableCollisions.started += _ => { OnDisableCollisions?.Invoke(); };

        OnThrustersRead += ReadThrusts;
    }

    private (float, float) ReadThrusts()
    {
        return (_controls.GameMap.Thrust.ReadValue<float>(),
            _controls.GameMap.Rotate.ReadValue<float>());
    }

    private void OnDisable() => _controls.Disable();
    private void OnDestroy() => _controls.Disable();
}