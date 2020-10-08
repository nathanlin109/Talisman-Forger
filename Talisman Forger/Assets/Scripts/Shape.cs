using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    // Fields (vertical part of shape will go from top down)
    public int width;
    public int height;
    public int heightStartX;
    public int heightStartY;

    public Shape(int width, int height, int heightStartX, int heightStartY)
    {
        this.width = width;
        this.height = height;
        this.heightStartX = heightStartX;
        this.heightStartY = heightStartY;
    }
}
