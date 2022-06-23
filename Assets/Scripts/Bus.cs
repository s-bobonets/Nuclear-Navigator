using UnityEngine;

public class Bus : MonoBehaviour
{
    private InputHandler _inputHandler;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Start()
    {
        _inputHandler.Thrust += (message) => { print($"thrust: {message}"); };
        _inputHandler.Turn += (message) => { print($"turn: {message}"); };
    }
}