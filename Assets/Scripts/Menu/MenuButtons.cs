using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public string StartScene;
    public GameObject instructionsScreen;

    public void StartGame()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(StartScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OpenInstructions()
    {
        if (instructionsScreen != null)
        {
            instructionsScreen.SetActive(true);
        }
    }

    public void CloseInstructions()
    {
        if (instructionsScreen != null)
        {
            instructionsScreen.SetActive(false);
        }
    }
}
