using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    // Fields
    public GameObject sceneMan;
    public GameObject[,] puzzle;
    public GameObject[,] finishedPuzzle;
    public GameObject tile;
    public int inserted;
    public int puzzleSize;
    public Shape[] shapesToInsert;
    public Shape[] allShapes;
    public List<Shape> addedShapes;
    public Material whiteMat;
    public Material blackMat;
    public Material blackBorderMat;
    public Material symbolTestMat;
    public List<Material> symbolMats;
    public int symbolCopies;
    public LevelInformation levelInfo;
    public Camera mainCamera;
    public bool isTutorial;
    public int tutorialLevel;

    // Start is called before the first frame update
    void Start()
    {
        tutorialLevel = 1;
        Init();
    }

    public void Init()
    {
        // set puzzle size
        levelInfo = GameObject.Find("LevelInformation").GetComponent<LevelInformation>();
        puzzleSize = 7 + levelInfo.level / 5;   // only change this 7 when changing the initial size of the puzzle grid

        if (puzzleSize > 15)
        {
            puzzleSize = 15;
        }

        // adjust camera zoom based on puzzle size
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.orthographicSize += (0.5f * (puzzleSize - 7));

        // Creates 2d arrays for puzzle and finished puzzle
        puzzle = new GameObject[puzzleSize, puzzleSize];
        finishedPuzzle = new GameObject[puzzleSize, puzzleSize];
        addedShapes = new List<Shape>();

        // Creates all possible shapes to insert into the puzzle
        allShapes = new Shape[18];
        allShapes[0] = new Shape(ShapeType.Vertical2);
        allShapes[1] = new Shape(ShapeType.Vertical3);
        allShapes[2] = new Shape(ShapeType.Horizontal2);
        allShapes[3] = new Shape(ShapeType.Horizontal3);
        allShapes[4] = new Shape(ShapeType.LongBL);
        allShapes[5] = new Shape(ShapeType.LongTL);
        allShapes[6] = new Shape(ShapeType.LongTR);
        allShapes[7] = new Shape(ShapeType.LongBR);
        allShapes[8] = new Shape(ShapeType.Cross);
        allShapes[9] = new Shape(ShapeType.ShortTL);
        allShapes[10] = new Shape(ShapeType.ShortTR);
        allShapes[11] = new Shape(ShapeType.ShortBR);
        allShapes[12] = new Shape(ShapeType.ShortBL);
        allShapes[13] = new Shape(ShapeType.TRight);
        allShapes[14] = new Shape(ShapeType.TDown);
        allShapes[15] = new Shape(ShapeType.TLeft);
        allShapes[16] = new Shape(ShapeType.TUp);
        allShapes[17] = new Shape(ShapeType.Circle);

        // Inserts shapes to be placed onto puzzle
        shapesToInsert = new Shape[puzzleSize * 3];
        // Normal randomized game
        if (!isTutorial)
        {
            for (int i = 0; i < shapesToInsert.Length / 2; i++)
            {
                int index = Random.Range(0, allShapes.Length - 1);
                shapesToInsert[i] = new Shape(allShapes[index].shapeType);
            }
            // Fills the list with circles
            for (int i = shapesToInsert.Length / 2; i < shapesToInsert.Length; i++)
            {
                shapesToInsert[i] = new Shape(ShapeType.Circle);
            }
        }
        // Tutorial
        else
        {
            if (tutorialLevel == 1)
            {
                shapesToInsert[0] = new Shape(ShapeType.Vertical2);
                shapesToInsert[1] = new Shape(ShapeType.Vertical3);
                shapesToInsert[2] = new Shape(ShapeType.LongBL);
                shapesToInsert[3] = new Shape(ShapeType.Cross);
                shapesToInsert[4] = new Shape(ShapeType.ShortTL);
                shapesToInsert[5] = new Shape(ShapeType.Circle);
                shapesToInsert[6] = new Shape(ShapeType.LongBL);
            }
            else if (tutorialLevel == 2)
            {
                shapesToInsert[5] = new Shape(ShapeType.Vertical2);
                shapesToInsert[2] = new Shape(ShapeType.Vertical3);
                shapesToInsert[6] = new Shape(ShapeType.LongBL);
                shapesToInsert[4] = new Shape(ShapeType.Cross);
                shapesToInsert[1] = new Shape(ShapeType.ShortTL);
                shapesToInsert[0] = new Shape(ShapeType.Circle);
                shapesToInsert[3] = new Shape(ShapeType.LongBL);
            }

            // Fills the list with circles
            for (int i = 7; i < shapesToInsert.Length; i++)
            {
                shapesToInsert[i] = new Shape(ShapeType.Circle);
            }
        }

        // Generates the puzzle and the completed puzzle
        GenerateTileGrid();
        GeneratePuzzle(shapesToInsert);

        // Sets the puzzle variables in scene manager
        sceneMan.GetComponent<SceneMan>().addedShapes = addedShapes;
        sceneMan.GetComponent<SceneMan>().puzzle = puzzle;
        sceneMan.GetComponent<SceneMan>().finishedPuzzle = finishedPuzzle;

        // Sets colors of tiles
        for (int i = 0; i < puzzle.Length / puzzleSize; i++)
        {
            for (int x = 0; x < puzzle.Length / puzzleSize; x++)
            {
                switch (finishedPuzzle[i, x].GetComponent<Tile>().tileType)
                {
                    case TileType.White:
                        if (!isTutorial || tutorialLevel == 2)
                        {
                            puzzle[i, x].GetComponent<Tile>().tileType = TileType.Black;
                            puzzle[i, x].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        }
                        else
                        {
                            puzzle[3, 5].GetComponent<Tile>().tileType = TileType.Black;
                            puzzle[3, 5].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        }
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
                        puzzle[i, x].GetComponent<Tile>().tileType = TileType.Symbol;
                        break;
                    default:
                        break;
                }
            }
        }

        // Sets symbols for tiles
        foreach (Shape shape in addedShapes)
        {
            int index = 0;
            switch (shape.shapeType)
            {
                case ShapeType.Circle:
                    index = Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.Cross:
                    index = symbolCopies + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.Vertical2:
                    index = symbolCopies * 2 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.Vertical3:
                    index = symbolCopies * 3 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.Horizontal2:
                    index = symbolCopies * 2 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    break;
                case ShapeType.Horizontal3:
                    index = symbolCopies * 3 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    break;
                case ShapeType.LongBL:
                    index = symbolCopies * 4 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.LongTL:
                    index = symbolCopies * 4 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    break;
                case ShapeType.LongTR:
                    index = symbolCopies * 4 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 180.0f);
                    break;
                case ShapeType.LongBR:
                    index = symbolCopies * 4 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 270.0f);
                    break;
                case ShapeType.ShortTL:
                    index = symbolCopies * 5 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.ShortTR:
                    index = symbolCopies * 5 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    break;
                case ShapeType.ShortBR:
                    index = symbolCopies * 5 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 180.0f);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.ShortBL:
                    index = symbolCopies * 5 + Random.Range(0, symbolCopies);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 270.0f);
                    break;
                case ShapeType.TRight:
                    index = symbolCopies * 6 + Random.Range(0, symbolCopies - 1);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    break;
                case ShapeType.TDown:
                    index = symbolCopies * 6 + Random.Range(0, symbolCopies - 1);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    break;
                case ShapeType.TLeft:
                    index = symbolCopies * 6 + Random.Range(0, symbolCopies - 1);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 180.0f);
                    break;
                case ShapeType.TUp:
                    index = symbolCopies * 6 + Random.Range(0, symbolCopies - 1);
                    puzzle[shape.xSymbol, shape.ySymbol].GetComponentInChildren<Renderer>().material = symbolMats[index];
                    puzzle[shape.xSymbol, shape.ySymbol].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 270.0f);
                    break;
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
        for (int i = 0; i < puzzle.Length / puzzleSize; i++)
        {
            for (int x = 0; x < puzzle.Length / puzzleSize; x++)
            {
                // Generates tile and adds it to the array
                puzzle[i, x] = Instantiate(tile,
                    new Vector3(i - 3 - (0.5f * (puzzleSize - 7)), 3 - x + (0.5f * (puzzleSize - 7)), 0),
                    Quaternion.Euler(0f, 180f, 0f));
            }
        }
    }

    // Generates puzzle
    void GeneratePuzzle(Shape[] shapes)
    {
        // Resets the finished puzzle to be of all black tiles
        inserted = 0;
        for (int i = 0; i < puzzle.Length / puzzleSize; i++)
        {
            for (int x = 0; x < puzzle.Length / puzzleSize; x++)
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
            for (int i = 0; i < puzzle.Length / puzzleSize; i++)
            {
                for (int x = 0; x < puzzle.Length / puzzleSize; x++)
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
            if (startX + shape.width - 1 <= puzzleSize - 1 && startY + shape.heightStartY + shape.height - 1 <= puzzleSize - 1 &&
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
                    if (i == shape.width - 1 && startX + shape.width <= puzzleSize - 1 &&
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
                    if (startY + 1 <= puzzleSize - 1 &&
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
                    if (startY + 1 <= puzzleSize - 1 &&
                        (finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }
                }
                if (startX + shape.width <= puzzleSize - 1)
                {
                    // Checks top of horizontal line
                    if (startY - 1 >= 0 &&
                        (finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }

                    // Checks bottom of horizontal line
                    if (startY + 1 <= puzzleSize - 1 &&
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
                    if (i == shape.height - 1 && startY + shape.heightStartY + shape.height <= puzzleSize - 1 &&
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
                    if (startX + shape.heightStartX + 1 <= puzzleSize - 1 &&
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
                    if (startX + shape.heightStartX + 1 <= puzzleSize - 1 &&
                        (finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.Black &&
                        finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                        .GetComponent<Tile>().tileType != TileType.BlackBorder))
                    {
                        insertable = false;
                    }
                }
                if (startY + shape.heightStartY + shape.height <= puzzleSize - 1)
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
                    if (startX + shape.heightStartX + 1 <= puzzleSize - 1 &&
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
                        if (i == shape.width - 1 && startX + shape.width <= puzzleSize - 1)
                        {
                            finishedPuzzle[startX + shape.width, startY].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        if (i != shape.heightStartX)
                        {
                            if (startY - 1 >= 0)
                            {
                                finishedPuzzle[startX + i, startY - 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                            }
                            if (startY + 1 <= puzzleSize - 1)
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
                        if (startY + 1 <= puzzleSize - 1)
                        {
                            finishedPuzzle[startX - 1, startY + 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }
                    if (startX + shape.width <= puzzleSize - 1)
                    {
                        // Right top
                        if (startY - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.width, startY - 1].GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                        // Right bottom
                        if (startY + 1 <= puzzleSize - 1)
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
                        if (i == shape.height - 1 && startY + shape.heightStartY + shape.height <= puzzleSize - 1)
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
                            if (startX + shape.heightStartX + 1 <= puzzleSize - 1)
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
                        if (startX + shape.heightStartX + 1 <= puzzleSize - 1)
                        {
                            finishedPuzzle[startX + shape.heightStartX + 1, startY + shape.heightStartY - 1]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                        }
                    }
                    if (startY + shape.heightStartY + shape.height <= puzzleSize - 1)
                    {
                        // Bottom left
                        if (startX + shape.heightStartX - 1 >= 0)
                        {
                            finishedPuzzle[startX + shape.heightStartX - 1, startY + shape.heightStartY + shape.height]
                                .GetComponent<Tile>().tileType = TileType.BlackBorder;
                            
                        }
                        // Bottom right
                        if (startX + shape.heightStartX + 1 <= puzzleSize - 1)
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
                    shape.InsertSymbol(finishedPuzzle, isTutorial);

                    return true;
                }
                return false;
            }
        }
        return false;
    }
}