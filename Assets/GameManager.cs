using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject NextGamePanel;

    public void NextGmaePanelShow()
    {
        NextGamePanel.SetActive(true);
    }

    public void GameOverPanelShow()
    {
        GameOverPanel.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void GoNextScene(int sceneNumber)
    {
        SceneManager.LoadSceneAsync(sceneNumber);
    }

}
