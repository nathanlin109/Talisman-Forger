using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { White, Black, Symbol, Dot, BlackBorder }

public class Tile : MonoBehaviour
{
    // Fields
    public TileType tileType;
    public bool shouldRotate;

    // Start is called before the first frame update
    void Start()
    {
        shouldRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate)
        {
            Rotate();
        }
    }

    // Rotates tile
    void Rotate()
    {
        // Gets the target rotation (180 or 0 depending on initial orientation)
        int rotation = 0;
        if (tileType == TileType.Black)
        {
            rotation = 180;
        }
        
        // Lerps a rotation for the tile
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                        new Vector3(0, rotation, 0),
                        Time.deltaTime * 5);

        // Stops rotating the tile once it's finished (within 1 degree)
        if (tileType == TileType.White && transform.eulerAngles.y <= 1)
        {
            // Resets tile
            transform.eulerAngles = new Vector3(0, 0, 0);
            shouldRotate = false;

            // Switches it to a black tile
            tileType = TileType.Black;
        }
        else if (tileType == TileType.Black && 180 - transform.eulerAngles.y <= 1)
        {
            // Resets tile
            transform.eulerAngles = new Vector3(0, 180, 0);
            shouldRotate = false;

            // Switches it to a white tile
            tileType = TileType.White;
        }
    }
}
