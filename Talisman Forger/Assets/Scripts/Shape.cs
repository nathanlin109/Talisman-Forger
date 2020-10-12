using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    // Fields (vertical part of shape will go from top down)
    public int width; // Min is 1
    public int height; // Min is 1
    public int heightStartX; 
    public int heightStartY; // Vertical part of shape ALWAYS goes from top down (always <= 0)
    public int startingXPos;
    public int startingYPos;
    public bool didInsert;
               
    /*        |  
     *       -+-  width: 3, height: 3, heightStartX: 1, heightStartY: -1
     *        |      
               
             --+  width: 3, height: 2, heightStartX: 2, heightStartY: 0
               |

               + widht: 1, height: 1, heightStartX: 0, heightStartY: 0
     */

    // Ctor
    public Shape(int width, int height, int heightStartX, int heightStartY)
    {
        this.width = width;
        this.height = height;
        this.heightStartX = heightStartX;
        this.heightStartY = heightStartY;
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
            int xInsert = Random.Range(startingXPos, startingXPos + width);
            int yInsert = startingYPos;

            // If the x position is where the vertical part starts, get a random y pos
            if (xInsert == startingXPos + heightStartX)
            {
                yInsert = Random.Range(startingYPos + heightStartY, startingYPos + heightStartY + height);
            }

            // Changes the tile type to symbol
            finishedPuzzle[xInsert, yInsert].GetComponent<Tile>().tileType = TileType.Symbol;
        }
    }

    // Randomly chooses a rotation and flipped version of the shape
    public void RandomizeOrientation()
    {
        // randomize width and height - if it chooses 1, swap width and height, otherwise leave them as is
        if (Random.Range(0, 2) == 1)
        {
            width ^= height;
            height = width ^ height;
            width ^= height;
        }

        // randomly choose height start X and height start Y
    }
}
