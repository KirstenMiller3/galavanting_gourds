using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int gameStartScene;

    public LevelLoader loader;


    public void Start()
    {
        AudioManager.instance.Play("title");
    }

    public void Nextlevel()
    {
        Debug.Log("Start game");
        loader.LoadNextLevel();
        AudioManager.instance.Stop("title");

    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
