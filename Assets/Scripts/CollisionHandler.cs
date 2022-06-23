using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private Coroutine _changeLevelRoutine;

    private Movement _movement;
    private PlaySounds _playSounds;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _playSounds = GetComponent<PlaySounds>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var cl = other.gameObject.GetComponent<Collision>();

        if (cl.type != Collision.CollisionType.Bonus) return;
        other.gameObject.SetActive(false);
    }

    private IEnumerator ChangeLevel(bool restart = false)
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var sceneCount = SceneManager.sceneCountInBuildSettings;

        if (restart)
        {
            _movement.enabled = false;
            
            PlaySounds.AudioSource.Stop();
            _playSounds.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        if (restart)
        {
            SceneManager.LoadScene(currentIndex);
        }

        else
        {
            if (currentIndex == sceneCount - 1)
            {
                currentIndex = 0;
                SceneManager.LoadScene(currentIndex);
            }
            else
                SceneManager.LoadScene(currentIndex + 1);
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        var cl = collision.gameObject.GetComponent<Collision>();

        switch (cl.type)
        {
            case Collision.CollisionType.Start:
                print($"touching {cl.type}");
                break;
            case Collision.CollisionType.End:
                print($"touching {cl.type}");
                StartCoroutine(ChangeLevel());
                break;
            case Collision.CollisionType.Obstacle:
                // LevelManager.OnLevelReload?.Invoke();
                StartCoroutine(ChangeLevel(true));
                print($"touching {cl.type}");
                break;
            case Collision.CollisionType.Bonus:
                //this shouldn't happen
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}