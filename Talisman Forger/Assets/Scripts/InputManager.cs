using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Fields
    public Material tileNormalMat;
    public Material tileDotMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("SceneMan").GetComponent<SceneMan>().paused == false)
        {
            FlipTile();
            DotTile();
        }
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
                        hit.transform.gameObject.GetComponent<Tile>().shouldRotate = true;
                        FindObjectOfType<AudioMan>().Play("Click_1");
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
                // Can only dot/undot a white/symbol tiles
                switch (hit.collider.gameObject.GetComponent<Tile>().tileType)
                {
                    case TileType.White:
                        hit.collider.gameObject.GetComponent<Tile>().tileType = TileType.Dot;
                        hit.collider.gameObject.GetComponentInChildren<Renderer>().material = tileDotMat;
                        break;
                    case TileType.Dot:
                        hit.collider.gameObject.GetComponent<Tile>().tileType = TileType.White;
                        hit.collider.gameObject.GetComponentInChildren<Renderer>().material = tileNormalMat;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
