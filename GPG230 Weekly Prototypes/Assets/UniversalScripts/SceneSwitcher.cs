using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string scene)
    {
        Time.timeScale = 1;

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.LeaveLobby();

        SceneManager.LoadScene(scene);
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
