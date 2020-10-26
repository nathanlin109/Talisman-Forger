using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneMan : MonoBehaviour
{
    public void RunGameScene()
    {
        FindObjectOfType<AudioMan>().Play("Click_2");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void RunStartScene()
    {
        FindObjectOfType<AudioMan>().Play("Click_2");
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }
}