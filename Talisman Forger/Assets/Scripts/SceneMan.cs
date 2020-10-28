using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { MainMenu, Game, Pause, Win}

public class SceneMan : MonoBehaviour
{
    // Fields
    public GameObject tileSpawner;
    public List<Shape> addedShapes;
    public GameObject[,] puzzle;
    public GameObject[,] finishedPuzzle;
    public bool didWin;
    public GameObject UICanvas;
    public GameObject pauseCanvas;
    public GameObject instructionsCanvas;
    public GameObject StartingInstructionsCanvas;
    public GameObject BackgroundCanvas;
    public GameObject WelcomeCanvas;
    public bool paused;
    private bool playedParticles;
    private float particleTime;
    public int puzzleSize;
    public bool isTutorial;

    // Start is called before the first frame update
    void Start()
    {
        didWin = false;
        playedParticles = false;
        particleTime = 0;
        if (!isTutorial)
        {
            puzzleSize = tileSpawner.GetComponent<TileSpawner>().puzzleSize;
        }
        else
        {
            puzzleSize = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (didWin)
        {
            if (playedParticles == false)
            {
                // Plays particles on all white/symbol tiles
                for (int i = 0; i < puzzle.Length / puzzleSize; i++)
                {
                    for (int x = 0; x < puzzle.Length / puzzleSize; x++)
                    {
                        if (puzzle[i, x].GetComponent<Tile>().tileType == TileType.White ||
                            puzzle[i, x].GetComponent<Tile>().tileType == TileType.Symbol)
                        {
                            puzzle[i, x].GetComponentInChildren<ParticleSystem>().Play();
                        }
                    }
                }
                playedParticles = true;
            }
            if (playedParticles)
            {
                particleTime += Time.deltaTime;
                if (particleTime >= puzzle[0, 0].GetComponentInChildren<ParticleSystem>().main.duration + .5f)
                {
                    if (!isTutorial)
                    {
                        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                    }
                    else
                    {
                        // Destroys all previous tiles
                        for (int i = 0; i < puzzleSize; i++)
                        {
                            for (int x = 0; x < puzzleSize; x++)
                            {
                                Destroy(puzzle[i, x]);
                                Destroy(finishedPuzzle[i, x]);
                            }
                        }

                        // Create new ppuzzle
                        tileSpawner.GetComponent<TileSpawner>().tutorialLevel++;
                        if (tileSpawner.GetComponent<TileSpawner>().tutorialLevel < 3)
                        {
                            tileSpawner.GetComponent<TileSpawner>().Init();
                        }
                        didWin = false;

                        // Sets appropriate UI
                        WelcomeCanvas.SetActive(true);
                        if (tileSpawner.GetComponent<TileSpawner>().tutorialLevel == 2)
                        {
                            GameObject.Find("Welcome Canvas/WelcomeTextTitle").GetComponent<Text>().text =
                                "Congratulations!";
                            GameObject.Find("Welcome Canvas/WelcomeTextBody").GetComponent<Text>().text =
                                "You Completed your first puzzle and forged your first talisman! Not let's try something a little bit harder. Try completing " +
                                "this next puzzle from the beginning.";
                        }
                        else if (tileSpawner.GetComponent<TileSpawner>().tutorialLevel == 3)
                        {
                            GameObject.Find("Welcome Canvas/WelcomeTextTitle").GetComponent<Text>().text =
                                "You Have Completed the Tutorial!";
                            GameObject.Find("Welcome Canvas/WelcomeTextBody").GetComponent<Text>().text =
                                "Good luck with your sorcery exams and enjoy your journey in completing puzzles and creating some of the finest talismans in Ethshar!";
                            GameObject.Find("Welcome Canvas/OkButton/Text").GetComponent<Text>().text =
                                "Return to Menu";
                        }
                        UICanvas.SetActive(false);
                        paused = true;
                    }

                    playedParticles = false;
                    particleTime = 0;
                }
            }
        }
    }

    public void Pause()
    {
        PlayClickSound();
        GameObject.Find("UI Canvas/MenuButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("UI Canvas/MenuButton").GetComponent<Image>().sprite = GameObject.Find("UI Canvas/MenuButton").GetComponent<ButtonHover>().buttonSprites[0];
        UICanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        paused = true;
    }

    public void UnPause()
    {
        PlayClickSound();
        GameObject.Find("Pause Canvas/ResumeButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("Pause Canvas/ResumeButton").GetComponent<Image>().sprite = GameObject.Find("Pause Canvas/ResumeButton").GetComponent<ButtonHover>().buttonSprites[0];
        UICanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        paused = false;
    }

    public void RunStartScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }

    public void OpenInstructions()
    {
        PlayClickSound();
        GameObject.Find("Pause Canvas/InstructionsButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("Pause Canvas/InstructionsButton").GetComponent<Image>().sprite = GameObject.Find("Pause Canvas/InstructionsButton").GetComponent<ButtonHover>().buttonSprites[0];
        pauseCanvas.SetActive(false);
        instructionsCanvas.SetActive(true);
    }

    public void CloseInstructions()
    {
        PlayClickSound();
        GameObject.Find("Instructions Canvas/CloseButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("Instructions Canvas/CloseButton").GetComponent<Image>().sprite = GameObject.Find("Instructions Canvas/CloseButton").GetComponent<ButtonHover>().buttonSprites[0];
        pauseCanvas.SetActive(true);
        instructionsCanvas.SetActive(false);
    }

    public void RunGameScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    private void PlayClickSound()
    {
        FindObjectOfType<AudioMan>().Play("Click_2");
    }

    // Checks for win every time a tile is flipped
    public bool CheckWin()
    {
        didWin = true;

        // Loops through the shapes
        foreach (Shape shape in addedShapes)
        {
            // Checks if the shape started in correct position
            if (puzzle[shape.startingXPos, shape.startingYPos]
                .GetComponent<Tile>().tileType == TileType.White ||
                puzzle[shape.startingXPos, shape.startingYPos]
                .GetComponent<Tile>().tileType == TileType.Symbol ||
                puzzle[shape.startingXPos, shape.startingYPos]
                .GetComponent<Tile>().tileType == TileType.Dot)
            {
                for (int i = 0; i < shape.width; i++)
                {
                    // Checks horizontal to see if it's white, symbol, or dot
                    if (puzzle[shape.startingXPos + i, shape.startingYPos]
                            .GetComponent<Tile>().tileType != TileType.White &&
                        puzzle[shape.startingXPos + i, shape.startingYPos]
                            .GetComponent<Tile>().tileType != TileType.Symbol &&
                        puzzle[shape.startingXPos + i, shape.startingYPos]
                            .GetComponent<Tile>().tileType != TileType.Dot)
                    {
                        didWin = false;
                    }

                    // Checks left and right of shape to make sure it's black
                    if (i == 0 && shape.startingXPos - 1 >= 0 &&
                        puzzle[shape.startingXPos - 1, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        didWin = false;
                    }
                    if (i == shape.width - 1 && shape.startingXPos + shape.width <= puzzleSize - 1 &&
                        puzzle[shape.startingXPos + shape.width, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        didWin = false;
                    }

                    // Checks above vertical and below horizontal line to make sure it's black
                    if (shape.startingYPos - 1 >= 0 &&
                        i != shape.heightStartX &&
                        puzzle[shape.startingXPos + i, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        didWin = false;
                    }
                    if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                        i != shape.heightStartX &&
                        puzzle[shape.startingXPos + i, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        didWin = false;
                    }

                    // ----------------------CHECKS TOP LEFT/RIGHT OF HORIZONTAL ASPECT-------------------------
                    if (shape.startingXPos - 1 >= 0)
                    {
                        // Checks top of horizontal line
                        if (shape.startingYPos - 1 >= 0 &&
                            puzzle[shape.startingXPos - 1, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }

                        // Checks bottom of horizontal line
                        if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                            puzzle[shape.startingXPos - 1, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }
                    }
                    if (shape.startingXPos + shape.width <= puzzleSize - 1)
                    {
                        // Checks top of horizontal line
                        if (shape.startingYPos - 1 >= 0 &&
                            puzzle[shape.startingXPos + shape.width, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }

                        // Checks bottom of horizontal line
                        if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                            puzzle[shape.startingXPos + shape.width, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }
                    }

                    // Checks vertical to see if it's white, symbol, or dot
                    if (i == shape.heightStartX && didWin)
                    {
                        for (int x = 0; x < shape.height; x++)
                        {
                            // Checks shape itself to see if it's white, symbol, or dot
                            if (puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x]
                                    .GetComponent<Tile>().tileType != TileType.White &&
                                puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x]
                                    .GetComponent<Tile>().tileType != TileType.Symbol &&
                                puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x]
                                    .GetComponent<Tile>().tileType != TileType.Dot)
                            {
                                didWin = false;
                            }

                            // Checks top and bottom of shape to make sure it's black
                            if (x == 0 && shape.startingYPos + shape.heightStartY - 1 >= 0 &&
                                puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY - 1]
                                .GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                            if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height <= puzzleSize - 1 &&
                                puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY + shape.height]
                                .GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // Checks left and right of vertical line to make sure it's black
                            if (shape.startingXPos + shape.heightStartX - 1 >= 0 && 
                                shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + x]
                                .GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                            if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY + x]
                                .GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // ----------------------CHECKS TOP LEFT/RIGHT OF VERTICAL ASPECT-------------------------
                            if (shape.startingYPos + shape.heightStartY - 1 >= 0)
                            {
                                // Checks top of vertical line
                                if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                    puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY - 1]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of vertical line
                                if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY - 1]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }
                            if (shape.startingYPos + shape.heightStartY + shape.height <= puzzleSize - 1)
                            {
                                // Checks top of vertical line
                                if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                    puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + shape.height]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of vertical line
                                if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY + shape.height]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }

                            if (didWin == false)
                            {
                                break;
                            }
                        }
                    }

                    if (didWin == false)
                    {
                        break;
                    }
                }
            }

            // Checks the shape one to the right and down
            else
            {
                for (int i = 0; i < shape.width; i++)
                {
                    // Checks horizontal to see if it's white, symbol, or dot
                    if (shape.startingXPos + i + 1 <= puzzleSize - 1)
                    {
                        if (puzzle[shape.startingXPos + i + 1, shape.startingYPos]
                                .GetComponent<Tile>().tileType != TileType.White &&
                            puzzle[shape.startingXPos + i + 1, shape.startingYPos]
                                .GetComponent<Tile>().tileType != TileType.Symbol &&
                            puzzle[shape.startingXPos + i + 1, shape.startingYPos]
                                .GetComponent<Tile>().tileType != TileType.Dot)
                        {
                            didWin = false;
                        }

                        // Checks left and right of shape to make sure it's black
                        if (i == 0 && shape.startingXPos >= 0 &&
                            puzzle[shape.startingXPos, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }
                        if (i == shape.width - 1 && shape.startingXPos + shape.width + 1 <= puzzleSize - 1 &&
                            puzzle[shape.startingXPos + shape.width + 1, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }

                        // Checks above vertical and below horizontal line to make sure it's black
                        if (shape.startingYPos - 1 >= 0 &&
                            i != shape.heightStartX &&
                            puzzle[shape.startingXPos + i + 1, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }
                        if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                            i != shape.heightStartX &&
                            puzzle[shape.startingXPos + i + 1, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }

                        // ----------------------CHECKS TOP LEFT/RIGHT OF HORIZONTAL ASPECT-------------------------
                        if (shape.startingXPos >= 0)
                        {
                            // Checks top of horizontal line
                            if (shape.startingYPos - 1 >= 0 &&
                                puzzle[shape.startingXPos, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // Checks bottom of horizontal line
                            if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                                puzzle[shape.startingXPos, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                        }
                        if (shape.startingXPos + shape.width + 1 <= puzzleSize - 1)
                        {
                            // Checks top of horizontal line
                            if (shape.startingYPos - 1 >= 0 &&
                                puzzle[shape.startingXPos + shape.width + 1, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // Checks bottom of horizontal line
                            if (shape.startingYPos + 1 <= puzzleSize - 1 &&
                                puzzle[shape.startingXPos + shape.width + 1, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                        }

                        // Checks vertical to see if it's white, symbol, or dot
                        if (i == shape.heightStartX)
                        {
                            for (int x = 0; x < shape.height; x++)
                            {
                                if (puzzle[shape.startingXPos + i + 1, shape.startingYPos + shape.heightStartY + x]
                                        .GetComponent<Tile>().tileType != TileType.White &&
                                    puzzle[shape.startingXPos + i + 1, shape.startingYPos + shape.heightStartY + x]
                                        .GetComponent<Tile>().tileType != TileType.Symbol &&
                                    puzzle[shape.startingXPos + i + 1, shape.startingYPos + shape.heightStartY + x]
                                        .GetComponent<Tile>().tileType != TileType.Dot)
                                {
                                    didWin = false;
                                }

                                // Checks top and bottom of shape to make sure it's black
                                if (x == 0 && shape.startingYPos + shape.heightStartY - 1 >= 0 &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY - 1]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                                if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height <= puzzleSize - 1 &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY + shape.height]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks left and right of vertical line to make sure it's black
                                if (shape.startingXPos + shape.heightStartX >= 0 &&
                                    shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                    puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY + x]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                                if (shape.startingXPos + shape.heightStartX + 2 <= puzzleSize - 1 &&
                                    shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 2, shape.startingYPos + shape.heightStartY + x]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // ----------------------CHECKS TOP LEFT/RIGHT OF VERTICAL ASPECT-------------------------
                                if (shape.startingYPos + shape.heightStartY - 1 >= 0)
                                {
                                    // Checks top of vertical line
                                    if (shape.startingXPos + shape.heightStartX >= 0 &&
                                        puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY - 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }

                                    // Checks bottom of vertical line
                                    if (shape.startingXPos + shape.heightStartX + 2 <= puzzleSize - 1 &&
                                        puzzle[shape.startingXPos + shape.heightStartX + 2, shape.startingYPos + shape.heightStartY - 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }
                                }
                                if (shape.startingYPos + shape.heightStartY + shape.height <= puzzleSize - 1)
                                {
                                    // Checks top of vertical line
                                    if (shape.startingXPos + shape.heightStartX >= 0 &&
                                        puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY + shape.height]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }

                                    // Checks bottom of vertical line
                                    if (shape.startingXPos + shape.heightStartX + 2 <= puzzleSize - 1 &&
                                        puzzle[shape.startingXPos + shape.heightStartX + 2, shape.startingYPos + shape.heightStartY + shape.height]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }
                                }

                                if (didWin == false)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        didWin = false;
                        break;
                    }
                }

                // Checks the right corners to make sure black tiles aren't cut off
                /*if (shape.startingXPos + shape.width <= 6)
                {
                    // Top right
                    if (shape.startingXPos + shape.width == 6 && shape.startingXPos + shape.heightStartX + 1 < 6 && shape.heightStartY < 0 && shape.startingYPos + shape.heightStartY == 0)
                    {
                        didWin = false;
                    }
                    // Bottom right
                    if (shape.startingXPos + shape.width == 6 && shape.startingXPos + shape.heightStartX + 1 < 6 && shape.startingYPos + shape.heightStartY + shape.height - 1 == 6)
                    {
                        didWin = false;
                    }
                }*/

                // Checks one down
                for (int i = 0; i < shape.width; i++)
                {
                    // Don't bother checking one down because checking one right was correct
                    if (didWin)
                    {
                        break;
                    }
                    else
                    {
                        didWin = true;
                        if (shape.startingYPos + shape.heightStartY + shape.height <= puzzleSize - 1)
                        {
                            // Checks horizontal to see if it's white, symbol, or dot
                            if (puzzle[shape.startingXPos + i, shape.startingYPos + 1]
                                    .GetComponent<Tile>().tileType != TileType.White &&
                                puzzle[shape.startingXPos + i, shape.startingYPos + 1]
                                    .GetComponent<Tile>().tileType != TileType.Symbol &&
                                puzzle[shape.startingXPos + i, shape.startingYPos + 1]
                                    .GetComponent<Tile>().tileType != TileType.Dot)
                            {
                                didWin = false;
                            }

                            // Checks left and right of shape to make sure it's black
                            if (i == 0 && shape.startingXPos - 1 >= 0 &&
                                puzzle[shape.startingXPos - 1, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                            if (i == shape.width - 1 && shape.startingXPos + shape.width <= puzzleSize - 1 &&
                                puzzle[shape.startingXPos + shape.width, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // Checks above vertical and below horizontal line to make sure it's black
                            if (shape.startingYPos >= 0 &&
                                i != shape.heightStartX &&
                                puzzle[shape.startingXPos + i, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                            if (shape.startingYPos + 2 <= puzzleSize - 1 &&
                                i != shape.heightStartX &&
                                puzzle[shape.startingXPos + i, shape.startingYPos + 2].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // ----------------------CHECKS TOP LEFT/RIGHT OF HORIZONTAL ASPECT-------------------------
                            if (shape.startingXPos - 1 >= 0)
                            {
                                // Checks top of horizontal line
                                if (shape.startingYPos >= 0 &&
                                    puzzle[shape.startingXPos - 1, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of horizontal line
                                if (shape.startingYPos + 2 <= puzzleSize - 1 &&
                                    puzzle[shape.startingXPos - 1, shape.startingYPos + 2].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }
                            if (shape.startingXPos + shape.width <= puzzleSize - 1)
                            {
                                // Checks top of horizontal line
                                if (shape.startingYPos >= 0 &&
                                    puzzle[shape.startingXPos + shape.width, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of horizontal line
                                if (shape.startingYPos + 2 <= puzzleSize - 1 &&
                                    puzzle[shape.startingXPos + shape.width, shape.startingYPos + 2].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }

                            // Checks vertical to see if it's white, symbol, or dot
                            if (i == shape.heightStartX && didWin)
                            {
                                for (int x = 0; x < shape.height; x++)
                                {
                                    // Checks shape itself to see if it's white, symbol, or dot
                                    if (puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x + 1]
                                            .GetComponent<Tile>().tileType != TileType.White &&
                                        puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x + 1]
                                            .GetComponent<Tile>().tileType != TileType.Symbol &&
                                        puzzle[shape.startingXPos + i, shape.startingYPos + shape.heightStartY + x + 1]
                                            .GetComponent<Tile>().tileType != TileType.Dot)
                                    {
                                        didWin = false;
                                    }

                                    // Checks top and bottom of shape to make sure it's black
                                    if (x == 0 && shape.startingYPos + shape.heightStartY >= 0 &&
                                        puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }
                                    if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height + 1 <= puzzleSize - 1 &&
                                        puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY + shape.height + 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }

                                    // Checks left and right of vertical line to make sure it's black
                                    if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                        shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                        puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + x + 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }
                                    if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                        shape.startingYPos != shape.startingYPos + shape.heightStartY + x &&
                                        puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY + x + 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }

                                    // ----------------------CHECKS TOP LEFT/RIGHT OF VERTICAL ASPECT-------------------------
                                    if (shape.startingYPos + shape.heightStartY >= 0)
                                    {
                                        // Checks top of vertical line
                                        if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                            puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }

                                        // Checks bottom of vertical line
                                        if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                            puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }
                                    }
                                    if (shape.startingYPos + shape.heightStartY + shape.height + 1 <= puzzleSize - 1)
                                    {
                                        // Checks top of vertical line
                                        if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                            puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + shape.height + 1]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }

                                        // Checks bottom of vertical line
                                        if (shape.startingXPos + shape.heightStartX + 1 <= puzzleSize - 1 &&
                                            puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY + shape.height + 1]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }
                                    }

                                    if (didWin == false)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (didWin == false)
                            {
                                break;
                            }
                        }
                        else
                        {
                            didWin = false;
                            break;
                        }
                    }
                }

                // Checks the bottom corners to make sure black tiles aren't cut off
                /*if (shape.startingYPos + shape.height <= 6)
                {
                    // Top right
                    // Bottom left
                    if (shape.startingXPos == 0 && shape.heightStartX > 0 && shape.startingYPos + shape.heightStartY + shape.height == 6)
                    {
                        didWin = false;
                    }
                    // Bottom right
                    if (shape.startingXPos + shape.width - 1 == 6 && shape.startingXPos + shape.heightStartX < 6 && shape.startingYPos + shape.heightStartY + shape.height == 6)
                    {
                        didWin = false;
                    }
                }*/
            }

            if (didWin == false)
            {
                break;
            }
        }

        // Checks if finished and current puzzle have same number of black tiles
        if (didWin)
        {
            int finishedPuzzleBlackCount = 0;
            int puzzleBlackCount = 0;
            for (int i = 0; i < puzzle.Length / puzzleSize; i++)
            {
                for (int x = 0; x < puzzle.Length / puzzleSize; x++)
                {
                    if (puzzle[i, x].GetComponent<Tile>().tileType == TileType.Black)
                    {
                        puzzleBlackCount++;
                    }
                    if (finishedPuzzle[i, x].GetComponent<Tile>().tileType == TileType.Black ||
                        finishedPuzzle[i, x].GetComponent<Tile>().tileType == TileType.BlackBorder)
                    {
                        finishedPuzzleBlackCount++;
                    }
                }
            }

            if (puzzleBlackCount != finishedPuzzleBlackCount)
            {
                didWin = false;
            }
        }

        Debug.Log(didWin);
        if (didWin)
        {
            FindObjectOfType<AudioMan>().Play("Puzzle_win");
        }
        return didWin;
    }
}