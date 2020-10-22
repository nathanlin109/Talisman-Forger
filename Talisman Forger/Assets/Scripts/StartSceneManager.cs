using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RunMenuScene()
    {
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
    }

    void RunGameScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
