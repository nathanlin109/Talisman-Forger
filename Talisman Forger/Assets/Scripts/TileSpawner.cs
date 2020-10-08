using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    // Fields
    public GameObject[,] puzzle;
    public GameObject[,] finishedPuzzle;
    public GameObject tile;
    public int inserted;
    public Shape[] shapes;
    public Material whiteMat;
    public Material blackMat;
    public Material blackBorderMat;

    // Start is called before the first frame update
    void Start()
    {
        // Creates 2d arrays for puzzle and finished puzzle
        puzzle = new GameObject[7,7];
        finishedPuzzle = new GameObject[7, 7];

        // Creates shapes
        shapes = new Shape[5];
        shapes[0] = new Shape(2, 1, 0, 0);
        shapes[1] = new Shape(3, 2, 2, 0);
        shapes[2] = new Shape(3, 3, 1, -1);
        shapes[3] = new Shape(2, 2, 0, 0);
        shapes[4] = new Shape(1, 1, 0, 0);

        // Generates the puzzle and the completed puzzle
        GenerateTileGrid();
        GeneratePuzzle(shapes);
        for (int i = 0; i < puzzle.Length / 7; i++)
        {
            for (int x = 0; x < puzzle.Length / 7; x++)
            {
                switch (finishedPuzzle[i, x].GetComponent<Tile>().tileType)
                {
                    case TileType.White:
                        puzzle[i, x].GetComponent<Renderer>().material = whiteMat;
                        break;
                    case TileType.Black:
                        puzzle[i, x].GetComponent<Renderer>().material = blackMat;
                        break;
                    case TileType.BlackBorder:
                        puzzle[i, x].GetComponent<Renderer>().material = blackBorderMat;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Generates tiles
    void GenerateTileGrid()
    {
        for (int i = 0; i < puzzle.Length / 7; i++)
        {
            for (int x = 0; x < puzzle.Length / 7; x++)
            {
                // Generates tile and adds it to the array
                puzzle[i, x] = Instantiate(tile,
                    new Vector3(i - 3, x - 3, 0),
                    Quaternion.identity);
            }
        }
    }

    // Generates puzzle
    void GeneratePuzzle(Shape[] shapes)
    {
        // Resets the finished puzzle to be of all black tiles
        inserted = 0;
        for (int i = 0; i < puzzle.Length / 7; i++)
        {
            for (int x = 0; x < puzzle.Length / 7; x++)
            {
                // Generates tile and adds it to the array (array starts out as black)
                finishedPuzzle[i, x] = Instantiate(tile,
                    new Vector3(i - 100, x - 3, 0),
                    Quaternion.identity);
                finishedPuzzle[i, x].GetComponent<Tile>().tileType = TileType.Black;
            }
        }

        // Tries to insert shapes
        for (int y = 0; y < shapes.Length; y++)
        {
            bool didInsert = false;
            for (int i = 0; i < puzzle.Length / 7; i++)
            {
                for (int x = 0; x < puzzle.Length / 7; x++)
                {
                    didInsert = InsertShape(i, x, shapes[y]);
                    inserted++;

                    // Moves on to next shape if it inserted correctly
                    if (didInsert)
                    {
                        break;
                    }
                }
                // Moves on to next shape if it inserted correctly
                if (didInsert)
                {
                    break;
                }
            }
        }

        // Regenerates puzzle if all pieces didn't insert
        /*if (inserted != shapes.Length)
        {
            GeneratePuzzle(shapes);
        }*/
    }

    // Inserts shapes into puzzle
    bool InsertShape(int startX, int startY, Shape shape)
    {
        bool insertable = true;

        // Checks if shape can be inserted
        if (finishedPuzzle[startX, startY].GetComponent<Tile>().tileType == TileType.Black)
        {
            // Ensures shape will fit onto board
            if (startX + shape.width <= 6 && startY + shape.heightStartY + shape.height <= 6 &&
                startY + shape.heightStartY >= 0)
            {
                // Checks if it can fit horizontally
                for (int i = 0; i < shape.width; i++)
                {
                    // Checks left of shape
                    if (i == 0 && startX - 1 >= 0 &&
                        finishedPuzzle[startX - 1, startY].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks right of shape
                    if (i == shape.width - 1 && startX + shape.width <= 6 &&
                        finishedPuzzle[startX + shape.width, startY].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks top of horizontal line
                    if (startY - 1 >= 0 &&
                        finishedPuzzle[startX + i, startY - 1].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks bottom of horizontal line
                    if (startY + 1 >= 6 &&
                        finishedPuzzle[startX + i, startY + 1].GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }
                }

                // Checks if it can fit vertically
                for (int i = 0; i < shape.height; i++)
                {
                    // Don't looping if can't insert
                    if (insertable == false)
                    {
                        break;
                    }

                    // Checks top of shape
                    if (i == 0 && startY + shape.heightStartY - 1 >= 0 &&
                        finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks bottom of shape
                    if (i == shape.height - 1 && startY + shape.heightStartY + shape.height <= 6 &&
                        finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks left of vertical line
                    if (startX + shape.heightStartX - 1 >= 0 &&
                        startY != startY + shape.heightStartY + i &&
                        finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }

                    // Checks right of vertical line
                    if (startX + shape.heightStartX + 1 >= 6 &&
                        startY != startY + shape.heightStartY + i &&
                        finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.Black)
                    {
                        insertable = false;
                        break;
                    }
                }

                // Inserts the shape
                if (insertable)
                {
                    // Inserts shape horizontally
                    for (int i = 0; i < shape.width; i++)
                    {
                        // Inserts white part of shape
                        finishedPuzzle[startX + i, startY].GetComponent<Tile>().tileType = TileType.White;

                        // Inserts black borders of shape
                        if (i == 0 && startX - 1 >= 0)
                        {
                            finishedPuzzle[startX - 1, startY].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        if (i == shape.width - 1 && startX + shape.width <= 6)
                        {
                            finishedPuzzle[startX + shape.width, startY].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        if (i != shape.heightStartX)
                        {
                            if (startY - 1 >= 0)
                            {
                                finishedPuzzle[startX + i, startY - 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                            if (startY + 1 >= 6)
                            {
                                finishedPuzzle[startX + i, startY + 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                        }
                    }

                    // Inserts shape vertically
                    for (int i = 0; i < shape.height; i++)
                    {
                        // Inserts white part of shape
                        finishedPuzzle[shape.heightStartX, startY + shape.heightStartY + i]
                            .GetComponent<Tile>().tileType = TileType.White;

                        // Inserts black borders of shape
                        if (i == 0 && startY + shape.heightStartY - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY - 1]
                            .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        if (i == shape.height - 1 && startY + shape.heightStartY + shape.height <= 6)
                        {
                            finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY + shape.height]
                            .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        if (i != startY)
                        {
                            if (startX + shape.heightStartX - 1 >= 0)
                            {
                                finishedPuzzle[shape.heightStartX - 1, startY + shape.heightStartY + i]
                                    .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                            if (startX + shape.heightStartX + 1 >= 6)
                            {
                                finishedPuzzle[shape.heightStartX + 1, startY + shape.heightStartY + i]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    // Checks for double blacks
    bool checkDoubleBlacks()
    {
        return true;
    }
}
