using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneManager : MonoBehaviour
{
    // Fields
    public GameObject mainCanvas;
    public GameObject loreCanvas;
    public GameObject creditsCanvas;
    public LevelInformation levelInformation;

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
        PlayClickSound();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void RunInstructionsScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void RunTutorialMessage()
    {
        PlayClickSound();
        GameObject.Find("SceneMan").GetComponent<SceneMan>().tileSpawner.SetActive(true);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().StartingInstructionsCanvas.SetActive(false);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().WelcomeCanvas.SetActive(true);
        GameObject.Find("SceneMan").GetComponent<SceneMan>().BackgroundCanvas.SetActive(true);
    }

    public void RunTutorial()
    {
        PlayClickSound();
        if (GameObject.Find("SceneMan").GetComponent<SceneMan>().tileSpawner.GetComponent<TileSpawner>().
            tutorialLevel < 3)
        {
            GameObject.Find("SceneMan").GetComponent<SceneMan>().WelcomeCanvas.SetActive(false);
            GameObject.Find("SceneMan").GetComponent<SceneMan>().UICanvas.SetActive(true);
            GameObject.Find("SceneMan").GetComponent<SceneMan>().paused = false;
            levelInformation.SetLevelInfo();
            levelInformation.SetupTutorial();
        }
        else
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
    }

    public void RunLore()
    {
        PlayClickSound();
        mainCanvas.SetActive(false);
        loreCanvas.SetActive(true);
    }

    public void RunCredits()
    {
        PlayClickSound();
        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void CloseCreditsLore()
    {
        PlayClickSound();
        mainCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
        loreCanvas.SetActive(false);
    }

    private void PlayClickSound()
    {
        FindObjectOfType<AudioMan>().Play("Click_2");
    }
}
