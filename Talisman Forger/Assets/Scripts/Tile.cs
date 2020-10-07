using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { White, Black, Symbol, Dot }

public class Tile : MonoBehaviour
{
    // Fields
    public TileType tileType;
    public bool blackGenerated;
    public bool shouldRotate;
    public float initialYRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the default tile type to white
        tileType = TileType.White;
        blackGenerated = false;
        shouldRotate = false;
        initialYRotation = 0;
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
        int rotation = 180;
        if (initialYRotation == 180)
        {
            rotation = 0;
        }
        
        // Lerps a rotation for the tile
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                        new Vector3(0, rotation, 0),
                        Time.deltaTime * 5);

        // Stops rotating the tile once it's finished (within 1 degree)
        if (initialYRotation == 0 && 180.0f - transform.eulerAngles.y <= 1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            shouldRotate = false;

            // Toggles between white/black tile
            switch(tileType)
            {
                case TileType.White:
                    tileType = TileType.Black;
                        break;
                case TileType.Black:
                    tileType = TileType.White;
                        break;
                default:
                    break;
            }
        }
        else if (initialYRotation == 180 && transform.eulerAngles.y <= 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            shouldRotate = false;

            // Toggles between white/black tile
            switch (tileType)
            {
                case TileType.White:
                    tileType = TileType.Black;
                    break;
                case TileType.Black:
                    tileType = TileType.White;
                    break;
                default:
                    break;
            }
        }
    }
}
