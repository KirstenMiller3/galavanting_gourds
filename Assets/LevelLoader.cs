using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        // currently loading the new level via transition only when we click
        if (Input.GetMouseButtonDown(0)){
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        // check that we don't load beyond the last scene
        if (SceneManager.sceneCount > SceneManager.GetActiveScene().buildIndex){
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else{
            Debug.Log("No more levels");
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // trigger the transition trigger
        transition.SetTrigger("Start");
        // wait for the transtion time
        yield return new WaitForSeconds(transitionTime);
        // load the new scene
        SceneManager.LoadScene(levelIndex);
    }



}
