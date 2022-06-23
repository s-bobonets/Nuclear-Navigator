using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public static AudioSource AudioSource;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var thrust = Movement.ControlsInstance.GameMap.Thrust.ReadValue<float>();
        var thrusting = thrust != 0f;

        if (thrusting)
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();
        }
        else
            AudioSource.Stop();

        AudioSource.volume = thrust * .2f;
    }
}