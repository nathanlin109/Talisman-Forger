using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance;
    public Text timerText;
    public float timePassed;
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
        currentScene = SceneManager.GetActiveScene().name;
        timerText = GameObject.Find("Canvas/TimerText").GetComponent<Text>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        // get the current scene
        currentScene = SceneManager.GetActiveScene().name;

        if (timerText != null)
        {
            // update the timer if the main game scene is active
            if (currentScene == "MainScene")
            {
                timePassed += Time.deltaTime;
                timerText.text = "Time: " + Math.Truncate(timePassed / 60).ToString("00") + ":" + Math.Truncate(timePassed % 60).ToString("00");
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // when the main game scene if loaded, find the timer text
        if (scene.name == "MainScene")
        {
            timerText = GameObject.Find("Canvas/TimerText").GetComponent<Text>();

            // reset the timer when loading the main game scene except when switching from the pause menu scene to the main game scene
            if (currentScene != "Menu")
            {
                timePassed = 0;
                timerText.text = "Time: " + Math.Truncate(timePassed / 60).ToString("00") + ":" + Math.Truncate(timePassed % 60).ToString("00");
            }
        }
    }
}