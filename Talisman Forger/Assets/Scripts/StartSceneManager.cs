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

    public void RunGameScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void RunInstructionsScene()
    {
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RunTutorialMessage()
    {
        GameObject.Find("SceneMan").GetComponent<SceneMan>().tileSpawner.SetActive(true);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().StartingInstructionsCanvas.SetActive(false);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().WelcomeCanvas.SetActive(true);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().BackgroundCanvas.SetActive(true);
    }

    public void RunTutorial()
    {
        if (GameObject.Find("SceneMan").GetComponent<SceneMan>().tileSpawner.GetComponent<TileSpawner>().
            tutorialLevel < 3)
        {
            GameObject.Find("SceneMan").GetComponent<SceneMan>().WelcomeCanvas.SetActive(false);
            GameObject.Find("SceneMan").GetComponent<SceneMan>().UICanvas.SetActive(true);
            GameObject.Find("SceneMan").GetComponent<SceneMan>().paused = false;
        }
        else
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
    }
}
