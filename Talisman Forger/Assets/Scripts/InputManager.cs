using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlipTile();
        DotTile();
    }

    // Flips tiles
    void FlipTile()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            // Detects which tile the player clicked on
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Flips all tiles but smybol
                if (hit.transform.gameObject.GetComponent<Tile>().tileType != TileType.Symbol)
                {
                    // Tells tile to rotate
                    if (hit.transform.gameObject.GetComponent<Tile>().shouldRotate == false)
                    {
                        // Sets the initial y rotation before telling it to rotate
                        hit.transform.gameObject.GetComponent<Tile>().initialYRotation =
                            hit.transform.eulerAngles.y;
                        hit.transform.gameObject.GetComponent<Tile>().shouldRotate = true;
                    }
                }
            }
        }
    }

    // Dots the tile
    void DotTile()
    {
        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            // Detects which tile the player clicked on
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Can only dot a white tile
                if (hit.transform.gameObject.GetComponent<Tile>().tileType == TileType.White)
                {
                    hit.transform.gameObject.GetComponent<Tile>().tileType = TileType.Symbol;
                }
            }
        }
    }
}
