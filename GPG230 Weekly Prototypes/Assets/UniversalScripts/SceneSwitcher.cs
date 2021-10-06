using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
