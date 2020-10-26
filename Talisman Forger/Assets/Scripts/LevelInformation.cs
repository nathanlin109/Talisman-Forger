using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInformation : MonoBehaviour
{
    public static LevelInformation instance;
    public GameObject UICanvas;
    public SceneMan sceneMan;
    public Text timerText;
    public Text levelText;
    public float timePassed;
    public int level;
    private string currentScene;

    void Awake()
    {
        // Ensures only 1 instance of game timer
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        level = 1;
        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        // get the current scene
        currentScene = SceneManager.GetActiveScene().name;

        // update the timer if the main game scene is active, not paused, and the win has not been set
        if (timerText != null && UICanvas != null && currentScene == "MainScene" && UICanvas.activeSelf && !sceneMan.didWin)
        {
            timePassed += Time.deltaTime;
            timerText.text = "Time: " + Math.Truncate(timePassed / 60).ToString("00") + ":" + Math.Truncate(timePassed % 60).ToString("00");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // when the main game scene is loaded, find the objects and reset the timer
        if (scene.name == "MainScene")
        {
            UICanvas = GameObject.Find("UI Canvas");
            sceneMan = GameObject.Find("SceneMan").GetComponent<SceneMan>();
            timerText = GameObject.Find("UI Canvas/TimerText").GetComponent<Text>();
            levelText = GameObject.Find("UI Canvas/LevelText").GetComponent<Text>();
            timePassed = 0;
            timerText.text = "Time: " + Math.Truncate(timePassed / 60).ToString("00") + ":" + Math.Truncate(timePassed % 60).ToString("00");
            levelText.text = "Level: " + level;
        }
        // show the final score on the win scene
        else if (scene.name == "WinScene")
        {
            timerText = GameObject.Find("Canvas/FinalTimeText").GetComponent<Text>();
            timerText.text = "You completed level " + level + " with a time of " + Math.Truncate(timePassed / 60).ToString("00") + ":" + Math.Truncate(timePassed % 60).ToString("00");
            level++;
        }
    }
}