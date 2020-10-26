using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType {Vertical2, Vertical3, Horizontal2, Horizontal3,
    LongBL, LongTL, LongTR, LongBR,
    Cross ,
    ShortTL, ShortTR, ShortBR, ShortBL,
    Circle}

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
    public Shape(ShapeType shapeType)
    {
        this.shapeType = shapeType;

        switch(shapeType)
        {
            case ShapeType.Circle:
                this.width = 1;
                this.height = 1;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.Cross:
                this.width = 3;
                this.height = 3;
                this.heightStartX = 1;
                this.heightStartY = -1;
                break;
            case ShapeType.Vertical2:
                this.width = 1;
                this.height = 2;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.Vertical3:
                this.width = 1;
                this.height = 3;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.Horizontal2:
                this.width = 2;
                this.height = 1;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.Horizontal3:
                this.width = 3;
                this.height = 1;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.LongBL:
                this.width = 2;
                this.height = 3;
                this.heightStartX = 0;
                this.heightStartY = -2;
                break;
            case ShapeType.LongTL:
                this.width = 3;
                this.height = 2;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.LongTR:
                this.width = 2;
                this.height = 3;
                this.heightStartX = 1;
                this.heightStartY = 0;
                break;
            case ShapeType.LongBR:
                this.width = 3;
                this.height = 2;
                this.heightStartX = 2;
                this.heightStartY = -1;
                break;
            case ShapeType.ShortTL:
                this.width = 2;
                this.height = 2;
                this.heightStartX = 0;
                this.heightStartY = 0;
                break;
            case ShapeType.ShortTR:
                this.width = 2;
                this.height = 2;
                this.heightStartX = 1;
                this.heightStartY = 0;
                break;
            case ShapeType.ShortBR:
                this.width = 2;
                this.height = 2;
                this.heightStartX = 1;
                this.heightStartY = -1;
                break;
            case ShapeType.ShortBL:
                this.width = 2;
                this.height = 2;
                this.heightStartX = 0;
                this.heightStartY = -1;
                break;
        }
        startingXPos = 0;
        startingYPos = 0;
        didInsert = false;
    }

    // Inserts the symbol in a random place in shape
    public void InsertSymbol(GameObject[,] finishedPuzzle, bool isTutorial)
    {
        if (didInsert)
        {
            if (!isTutorial)
            {
                // Gets a random x position to insert symbol
                xSymbol = Random.Range(startingXPos, startingXPos + width);
                ySymbol = startingYPos;

                // If the x position is where the vertical part starts, get a random y pos
                if (xSymbol == startingXPos + heightStartX)
                {
                    ySymbol = Random.Range(startingYPos + heightStartY, startingYPos + heightStartY + height);
                }
            }
            else
            {
                xSymbol = startingXPos;
                ySymbol = startingYPos;
            }

            // Changes the tile type to symbol
            finishedPuzzle[xSymbol, ySymbol].GetComponent<Tile>().tileType = TileType.Symbol;
        }
    }
}
