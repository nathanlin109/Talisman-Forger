using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Game, Pause, Win}

public class SceneMan : MonoBehaviour
{
    // Fields
    public List<Shape> addedShapes;
    public GameObject[,] puzzle;
    public GameObject[,] finishedPuzzle;
    public bool didWin;

    // Start is called before the first frame update
    void Start()
    {
        didWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        RunMenuScene();
        RunGameScene();
    }

    void RunStartScene()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
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
                    if (i == shape.width - 1 && shape.startingXPos + shape.width <= 6 &&
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
                    if (shape.startingYPos + 1 <= 6 &&
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
                        if (shape.startingYPos + 1 <= 6 &&
                            puzzle[shape.startingXPos - 1, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }
                    }
                    if (shape.startingXPos + shape.width <= 6)
                    {
                        // Checks top of horizontal line
                        if (shape.startingYPos - 1 >= 0 &&
                            puzzle[shape.startingXPos + shape.width, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                        {
                            didWin = false;
                        }

                        // Checks bottom of horizontal line
                        if (shape.startingYPos + 1 <= 6 &&
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
                            if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height <= 6 &&
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
                            if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
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
                                if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
                                    puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY - 1]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }
                            if (shape.startingYPos + shape.heightStartY + shape.height <= 6)
                            {
                                // Checks top of vertical line
                                if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                    puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + shape.height]
                                    .GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of vertical line
                                if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
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
                    if (shape.startingXPos + i + 1 <= 6)
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
                        if (i == shape.width - 1 && shape.startingXPos + shape.width + 1 <= 6 &&
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
                        if (shape.startingYPos + 1 <= 6 &&
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
                            if (shape.startingYPos + 1 <= 6 &&
                                puzzle[shape.startingXPos, shape.startingYPos + 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }
                        }
                        if (shape.startingXPos + shape.width + 1 <= 6)
                        {
                            // Checks top of horizontal line
                            if (shape.startingYPos - 1 >= 0 &&
                                puzzle[shape.startingXPos + shape.width + 1, shape.startingYPos - 1].GetComponent<Tile>().tileType != TileType.Black)
                            {
                                didWin = false;
                            }

                            // Checks bottom of horizontal line
                            if (shape.startingYPos + 1 <= 6 &&
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
                                if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height <= 6 &&
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
                                if (shape.startingXPos + shape.heightStartX + 2 <= 6 &&
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
                                    if (shape.startingXPos + shape.heightStartX + 2 <= 6 &&
                                        puzzle[shape.startingXPos + shape.heightStartX + 2, shape.startingYPos + shape.heightStartY - 1]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }
                                }
                                if (shape.startingYPos + shape.heightStartY + shape.height <= 6)
                                {
                                    // Checks top of vertical line
                                    if (shape.startingXPos + shape.heightStartX >= 0 &&
                                        puzzle[shape.startingXPos + shape.heightStartX, shape.startingYPos + shape.heightStartY + shape.height]
                                        .GetComponent<Tile>().tileType != TileType.Black)
                                    {
                                        didWin = false;
                                    }

                                    // Checks bottom of vertical line
                                    if (shape.startingXPos + shape.heightStartX + 2 <= 6 &&
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
                        if (shape.startingYPos + shape.heightStartY + shape.height <= 6)
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
                            if (i == shape.width - 1 && shape.startingXPos + shape.width <= 6 &&
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
                            if (shape.startingYPos + 2 <= 6 &&
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
                                if (shape.startingYPos + 2 <= 6 &&
                                    puzzle[shape.startingXPos - 1, shape.startingYPos + 2].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }
                            }
                            if (shape.startingXPos + shape.width <= 6)
                            {
                                // Checks top of horizontal line
                                if (shape.startingYPos >= 0 &&
                                    puzzle[shape.startingXPos + shape.width, shape.startingYPos].GetComponent<Tile>().tileType != TileType.Black)
                                {
                                    didWin = false;
                                }

                                // Checks bottom of horizontal line
                                if (shape.startingYPos + 2 <= 6 &&
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
                                    if (x == shape.height - 1 && shape.startingYPos + shape.heightStartY + shape.height + 1 <= 6 &&
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
                                    if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
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
                                        if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
                                            puzzle[shape.startingXPos + shape.heightStartX + 1, shape.startingYPos + shape.heightStartY]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }
                                    }
                                    if (shape.startingYPos + shape.heightStartY + shape.height + 1 <= 6)
                                    {
                                        // Checks top of vertical line
                                        if (shape.startingXPos + shape.heightStartX - 1 >= 0 &&
                                            puzzle[shape.startingXPos + shape.heightStartX - 1, shape.startingYPos + shape.heightStartY + shape.height + 1]
                                            .GetComponent<Tile>().tileType != TileType.Black)
                                        {
                                            didWin = false;
                                        }

                                        // Checks bottom of vertical line
                                        if (shape.startingXPos + shape.heightStartX + 1 <= 6 &&
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
            for (int i = 0; i < puzzle.Length / 7; i++)
            {
                for (int x = 0; x < puzzle.Length / 7; x++)
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
