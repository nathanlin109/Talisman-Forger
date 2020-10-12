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
               
    /*        |  
     *       -+-  width: 3, height: 3, heightStartX: 1, heightStartY: -1
     *        |      
               
             --+  width: 3, height: 2, heightStartX: 2, heightStartY: 0
               |

               + widht: 1, height: 1, heightStartX: 0, heightStartY: 0
     */

    public Shape(int width, int height, int heightStartX, int heightStartY)
    {
        this.width = width;
        this.height = height;
        this.heightStartX = heightStartX;
        this.heightStartY = heightStartY;
        startingXPos = 0;
        startingYPos = 0;
    }
}
