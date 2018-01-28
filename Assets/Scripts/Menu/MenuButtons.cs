using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public string StartScene;
    public GameObject instructionsScreen;
    public GameObject loadingScreen;
    public AudioSource introSound;
    public GameObject skipButton;
    private AsyncOperation asyncLoader;

    public void StartGame()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);
        if (introSound != null)
            introSound.Play();
        StartCoroutine(LoadGameSceneAsync());
        
    }

    IEnumerator LoadGameSceneAsync()
    {
        asyncLoader = SceneManager.LoadSceneAsync(StartScene);
        asyncLoader.allowSceneActivation = false;
        StartCoroutine(StartGameWhenLouisIsDoneTalking());
        while (asyncLoader.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1F);
        }
        GameSceneDoneLoading();
    }

    private void GameSceneDoneLoading()
    {
        if (skipButton != null)
            skipButton.SetActive(true);
    }

    IEnumerator StartGameWhenLouisIsDoneTalking()
    {
        if (introSound != null)
        {
            while (introSound.isPlaying)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (asyncLoader != null)
                asyncLoader.allowSceneActivation = true;
        }
    }

    public void SkipLouis()
    {
        if (asyncLoader != null)
            asyncLoader.allowSceneActivation = true;
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
