using UnityEngine;

public class ControlsHandler : MonoBehaviour
{
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();
    }
}