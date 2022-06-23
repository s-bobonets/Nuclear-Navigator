using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static Action OnLevelReload;

    private int _i;

    private int Index
    {
        get
        {
            _i++;
            return _i % 2;
        }
    }

    private void Awake()
    {
        OnLevelReload += LoadNextLevel;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(Index);
    }
}