using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneMan : MonoBehaviour
{
    public void RunGameScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void RunStartScene()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }
}