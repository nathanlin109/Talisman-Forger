using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    // Fields
    public GameObject mainCanvas;
    public GameObject loreCanvas;
    public GameObject creditsCanvas;
    public LevelInformation levelInformation;
    public Sprite menuButtonNorm;
    public Sprite menuButtonHover;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Instructions")
        {
            levelInformation = GameObject.Find("LevelInformation").GetComponent<LevelInformation>();
        }
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
            GameObject.Find("Welcome Canvas/OkButton").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("Welcome Canvas/OkButton").GetComponent<Image>().sprite = GameObject.Find("Welcome Canvas/OkButton").GetComponent<ButtonHover>().buttonSprites[0];

            if (GameObject.Find("SceneMan").GetComponent<SceneMan>().tileSpawner.GetComponent<TileSpawner>().tutorialLevel == 2)
            {
                GameObject.Find("Welcome Canvas/OkButton").GetComponent<Image>().sprite = menuButtonNorm;
                GameObject.Find("Welcome Canvas/OkButton").GetComponent<ButtonHover>().buttonSprites[0] = menuButtonNorm;
                GameObject.Find("Welcome Canvas/OkButton").GetComponent<ButtonHover>().buttonSprites[1] = menuButtonHover;
            }

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
        GameObject.Find("Canvas/LoreButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("Canvas/LoreButton").GetComponent<Image>().sprite = GameObject.Find("Canvas/LoreButton").GetComponent<ButtonHover>().buttonSprites[0];
        mainCanvas.SetActive(false);
        loreCanvas.SetActive(true);
    }

    public void RunCredits()
    {
        PlayClickSound();
        GameObject.Find("Canvas/CreditsButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("Canvas/CreditsButton").GetComponent<Image>().sprite = GameObject.Find("Canvas/CreditsButton").GetComponent<ButtonHover>().buttonSprites[0];
        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void CloseCreditsLore()
    {
        PlayClickSound();
        if (creditsCanvas.activeSelf)
        {
            GameObject.Find("CreditsCanvas/Button").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("CreditsCanvas/Button").GetComponent<Image>().sprite = GameObject.Find("CreditsCanvas/Button").GetComponent<ButtonHover>().buttonSprites[0];
            creditsCanvas.SetActive(false);
        }
        else if (loreCanvas.activeSelf)
        {
            GameObject.Find("LoreCanvas/Button").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("LoreCanvas/Button").GetComponent<Image>().sprite = GameObject.Find("LoreCanvas/Button").GetComponent<ButtonHover>().buttonSprites[0];
            loreCanvas.SetActive(false);
        }
        mainCanvas.SetActive(true);
    }

    private void PlayClickSound()
    {
        FindObjectOfType<AudioMan>().Play("Click_2");
    }
}
