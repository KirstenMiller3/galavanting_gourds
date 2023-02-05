using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{
    public Animator transition;

    public float transitionTime = 2f;

    // Update is called once per frame
    void Update()
    {
        // currently loading the new level via transition only when we click
        if (Input.GetKeyDown(KeyCode.Return)){
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Trying to load level:" + (SceneManager.GetActiveScene().buildIndex + 1));
        // check that we don't load beyond the last scene
        if (SceneManager.sceneCountInBuildSettings >= SceneManager.GetActiveScene().buildIndex){
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else{
            Debug.Log("No more levels");
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("LOAD");
        // trigger the transition trigger
        transition.SetTrigger("Start");
        // wait for the transtion time
        yield return new WaitForSeconds(transitionTime);
        // load the new scene
        SceneManager.LoadScene(levelIndex);
    }
}
