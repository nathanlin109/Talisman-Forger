using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    // Fields
    public GameObject sceneMan;
    public GameObject[,] puzzle;
    public GameObject[,] finishedPuzzle;
    public GameObject tile;
    public int inserted;
    public Shape[] shapesToInsert;
    public Shape[] allShapes;
    public List<Shape> addedShapes;
    public Material whiteMat;
    public Material blackMat;
    public Material blackBorderMat;
    public Material symbolMat;

    // Start is called before the first frame update
    void Start()
    {
        // Creates 2d arrays for puzzle and finished puzzle
        puzzle = new GameObject[7,7];
        finishedPuzzle = new GameObject[7, 7];
        addedShapes = new List<Shape>();

        // Creates all possible shapes to insert into the puzzle
        allShapes = new Shape[5];
        allShapes[0] = new Shape(2, 1, 0, 0, ShapeType.HorizontalLine);
        allShapes[1] = new Shape(3, 2, 2, 0, ShapeType.LongLShape);
        allShapes[2] = new Shape(3, 3, 1, -1, ShapeType.Cross);
        allShapes[3] = new Shape(2, 2, 0, 0, ShapeType.SmallLShape);
        allShapes[4] = new Shape(1, 1, 0, 0, ShapeType.Dot);

        // Inserts shapes to be placed onto puzzle
        shapesToInsert = new Shape[20];
        for (int i = 0; i < shapesToInsert.Length; i++)
        {
            int index = Random.Range(0, allShapes.Length);
            shapesToInsert[i] = new Shape(allShapes[index].width, allShapes[index].height,
                allShapes[index].heightStartX, allShapes[index].heightStartY, allShapes[index].shapeType);
        }

        // Generates the puzzle and the completed puzzle
        GenerateTileGrid();
        GeneratePuzzle(shapesToInsert);

        // Sets the puzzle variables in scene manager
        sceneMan.GetComponent<SceneMan>().addedShapes = addedShapes;
        sceneMan.GetComponent<SceneMan>().puzzle = puzzle;
        sceneMan.GetComponent<SceneMan>().finishedPuzzle = finishedPuzzle;

        // ------------------------TESTING--------------------------
        for (int i = 0; i < puzzle.Length / 7; i++)
        {
            for (int x = 0; x < puzzle.Length / 7; x++)
            {
                switch (finishedPuzzle[i, x].GetComponent<Tile>().tileType)
                {
                    case TileType.White:
                        break;
                    case TileType.Black:
                        puzzle[i, x].GetComponent<Tile>().tileType = TileType.Black;
                        puzzle[i, x].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case TileType.BlackBorder:
                        puzzle[i, x].GetComponent<Tile>().tileType = TileType.Black;
                        puzzle[i, x].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case TileType.Symbol:
                        puzzle[i, x].GetComponentInChildren<Renderer>().material = symbolMat;
                        puzzle[i, x].GetComponent<Tile>().tileType = TileType.Symbol;
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
                    new Vector3(i - 3, 3 - x, 0),
                    Quaternion.Euler(0f, 180f, 0f));
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
                    new Vector3(i - 100, 3 - x, 0),
                    Quaternion.Euler(0f, 180f, 0f));
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

                    // Moves on to next shape if it inserted correctly
                    if (didInsert)
                    {
                        inserted++;
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
    }

    // Inserts shapes into puzzle
    bool InsertShape(int startX, int startY, Shape shape)
    {
        bool insertable = true;

        // Checks if shape can be inserted
        if (finishedPuzzle[startX, startY].GetComponent<Tile>().tileType == TileType.Black)
        {
            // Ensures shape will fit onto board
            if (startX + shape.width - 1 <= 6 && startY + shape.heightStartY + shape.height - 1 <= 6 &&
                startY + shape.heightStartY >= 0)
            {
                // Checks 4 corners to make sure there aren't black tiles that are cut off
                // Top left
                /*if (startX == 0 && shape.heightStartX > 0 && shape.heightStartY < 0 && startY + shape.heightStartY == 0)
                {
                    insertable = false;
                }
                // Bottom left
                if (startX == 0 && shape.heightStartX > 0 && startY + shape.heightStartY + shape.height - 1 == 6)
                {
                    insertable = false;
                }
                // Top right
                if (startX + shape.width - 1 == 6 && startX + shape.heightStartX < 6 && shape.heightStartY < 0 && startY + shape.heightStartY == 0)
                {
                    insertable = false;
                }
                // Bottom right
                if (startX + shape.width - 1 == 6 && startX + shape.heightStartX < 6 && startY + shape.heightStartY + shape.height - 1 == 6)
                {
                    insertable = false;
                }*/

                // -----------------------Checks if it can fit horizontally-----------------------
                for (int i = 0; i < shape.width; i++)
                {
                    if (insertable == false)
                    {
                        break;
                    }

                    // Checks left of shape
                    if (i == 0 && startX - 1 >= 0 &&
                        (finishedPuzzle[startX - 1, startY].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX - 1, startY].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks right of shape
                    if (i == shape.width - 1 && startX + shape.width <= 6 &&
                        (finishedPuzzle[startX + shape.width, startY].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.width, startY].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks top of horizontal line
                    if (startY - 1 >= 0 &&
                        (finishedPuzzle[startX + i, startY - 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + i, startY - 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks bottom of horizontal line
                    if (startY + 1 <= 6 &&
                        (finishedPuzzle[startX + i, startY + 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + i, startY + 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }
                }

                // ----------------------CHECKS TOP LEFT/RIGHT OF HORIZONTAL ASPECT-------------------------
                if (startX - 1 >= 0)
                {
                    // Checks top of horizontal line
                    if (startY - 1 >= 0 &&
                        (finishedPuzzle[startX - 1, startY - 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX - 1, startY - 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }

                    // Checks bottom of horizontal line
                    if (startY + 1 <= 6 &&
                        (finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }
                }
                if (startX + shape.width <= 6)
                {
                    // Checks top of horizontal line
                    if (startY - 1 >= 0 &&
                        (finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }

                    // Checks bottom of horizontal line
                    if (startY + 1 <= 6 &&
                        (finishedPuzzle[startX + shape.width, startY + 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.width, startY + 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }
                }

                // -----------------------Checks if it can fit vertically-----------------------
                for (int i = 0; i < shape.height; i++)
                {
                    // Don't looping if can't insert
                    if (insertable == false)
                    {
                        break;
                    }

                    // Checks top of shape
                    if (i == 0 && startY + shape.heightStartY - 1 >= 0 &&
                        (finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks bottom of shape
                    if (i == shape.height - 1 && startY + shape.heightStartY + shape.height <= 6 &&
                        (finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks left of vertical line
                    if (startX + shape.heightStartX - 1 >= 0 &&
                        startY != startY + shape.heightStartY + i &&
                        (finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }

                    // Checks right of vertical line
                    if (startX + shape.heightStartX + 1 <= 6 &&
                        startY != startY + shape.heightStartY + i &&
                        (finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + i]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                        break;
                    }
                }

                // ----------------------CHECKS TOP LEFT/RIGHT OF VERTICAL ASPECT-------------------------
                if (startY + shape.heightStartY - 1 >= 0)
                {
                    // Checks top of vertical line
                    if (startX + shape.heightStartX - 1 >= 0 &&
                        (finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }

                    // Checks bottom of vertical line
                    if (startX + shape.heightStartX + 1 <= 6 &&
                        (finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }
                }
                if (startY + shape.heightStartY + shape.height <= 6)
                {
                    // Checks top of vertical line
                    if (startX + shape.heightStartX - 1 >= 0 &&
                        (finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }

                    // Checks bottom of vertical line
                    if (startX + shape.heightStartX + 1 <= 6 &&
                        (finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + shape.height]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
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
                            if (startY + 1 <= 6)
                            {
                                finishedPuzzle[startX + i, startY + 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                        }
                    }

                    // ----------------------HORIZONTAL LEFT/RIGHT IS BLACKBORDER-------------------------
                    if (startX - 1 >= 0)
                    {
                        // Left top
                        if (startY - 1 >= 0)
                        {
                            finishedPuzzle[startX - 1, startY - 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        // Left bottom
                        if (startY + 1 <= 6)
                        {
                            finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }
                    if (startX + shape.width <= 6)
                    {
                        // Right top
                        if (startY - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        // Right bottom
                        if (startY + 1 <= 6)
                        {
                            finishedPuzzle[startX + shape.width, startY + 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }

                    // Inserts shape vertically
                    for (int i = 0; i < shape.height; i++)
                    {
                        // Inserts white part of shape
                        finishedPuzzle[startX + shape.heightStartX, startY + shape.heightStartY + i]
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
                        if (startY + shape.heightStartY + i != startY)
                        {
                            if (startX + shape.heightStartX - 1 >= 0)
                            {
                                finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + i]
                                    .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                            if (startX + shape.heightStartX + 1 <= 6)
                            {
                                finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + i]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                        }
                    }
                    // ----------------------VERTICAL LEFT/RIGHT IS BLACKBORDER-------------------------
                    if (startY + shape.heightStartY - 1 >= 0)
                    {
                        // Top left
                        if (startX + shape.heightStartX - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY - 1]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        // Top right
                        if (startX + shape.heightStartX + 1 <= 6)
                        {
                            finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }
                    if (startY + shape.heightStartY + shape.height <= 6)
                    {
                        // Bottom left
                        if (startX + shape.heightStartX - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + shape.height]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            
                        }
                        // Bottom right
                        if (startX + shape.heightStartX + 1 <= 6)
                        {
                            finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY + shape.height]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }

                    // Sets the starting position of the shape in the array and inserts symbol in random spot
                    shape.startingXPos = startX;
                    shape.startingYPos = startY;
                    shape.didInsert = true;
                    addedShapes.Add(shape);
                    shape.InsertSymbol(finishedPuzzle);

                    return true;
                }
                return false;
            }
        }
        return false;
    }
}
