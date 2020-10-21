using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour {
    public void RunStartScene() {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);    
    }
}
