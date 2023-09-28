using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);

    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(2f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(2f));
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        SceneManager.LoadScene(nextScene);
    }
}
