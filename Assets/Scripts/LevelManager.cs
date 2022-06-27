using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Action<bool> OnNextLevel;

    private void Awake() => OnNextLevel += ChangeLevel;

    private void ChangeLevel(bool restart)
    {
        StartCoroutine(LoadNext(restart));
    }

    private IEnumerator LoadNext(bool restart)
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var sceneCount = SceneManager.sceneCountInBuildSettings;

        yield return new WaitForSeconds(1f); // delay hardcoded

        if (restart)
            SceneManager.LoadScene(currentIndex);

        else
        {
            if (currentIndex == sceneCount - 1) // this line runs by the last scene
            {
                currentIndex = 0;
                SceneManager.LoadScene(currentIndex);
            }
            else
                SceneManager.LoadScene(currentIndex + 1); // load next scene
        }
    }
}