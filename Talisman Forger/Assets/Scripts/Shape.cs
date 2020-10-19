using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType {Vertical2, Vertical3, LongBL, Cross , ShortTL, Circle}

public class Shape
{
    // Fields (vertical part of shape will go from top down)
    public int width; // Min is 1
    public int height; // Min is 1
    public int heightStartX; 
    public int heightStartY; // Vertical part of shape ALWAYS goes from top down (always <= 0)
    public int startingXPos;
    public int startingYPos;
    public int xSymbol;
    public int ySymbol;
    public bool didInsert;
    public ShapeType shapeType;
               
    /*        |  
     *       -+-  width: 3, height: 3, heightStartX: 1, heightStartY: -1
     *        |      
               
             --+  width: 3, height: 2, heightStartX: 2, heightStartY: 0
               |

               + widht: 1, height: 1, heightStartX: 0, heightStartY: 0
     */

    // Ctor
    public Shape(int width, int height, int heightStartX, int heightStartY, ShapeType shapeType)
    {
        this.width = width;
        this.height = height;
        this.heightStartX = heightStartX;
        this.heightStartY = heightStartY;
        this.shapeType = shapeType;
        startingXPos = 0;
        startingYPos = 0;
        didInsert = false;
    }

    // Inserts the symbol in a random place in shape
    public void InsertSymbol(GameObject[,] finishedPuzzle)
    {
        if (didInsert)
        {
            // Gets a random x position to insert symbol
            xSymbol = Random.Range(startingXPos, startingXPos + width);
            ySymbol = startingYPos;

            // If the x position is where the vertical part starts, get a random y pos
            if (xSymbol == startingXPos + heightStartX)
            {
                ySymbol = Random.Range(startingYPos + heightStartY, startingYPos + heightStartY + height);
            }

            // Changes the tile type to symbol
            finishedPuzzle[xSymbol, ySymbol].GetComponent<Tile>().tileType = TileType.Symbol;
        }
    }
}
