using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisplayText();
    }

    // Update is called once per frame
    void Update()
    {
        RunMenuScene();
        RunGameScene();
    }
    void DisplayText()
    {
        TextMesh title = GetComponent<TextMesh>();
        title.text = "Talisman Forger";
        title.color = Color.green;
        title.alignment = TextAlignment.Center;
        title.characterSize = 40;
    }

    void RunMenuScene()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    void RunGameScene()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
    }
}
