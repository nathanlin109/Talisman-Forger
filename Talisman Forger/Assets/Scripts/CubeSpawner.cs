using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Fields
    public GameObject[] tiles;
    public GameObject tileNormal;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[49];
        GenerateTileGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Generates tiles
    void GenerateTileGrid()
    {
        for (int i = 0; i < tiles.Length / 7; i++)
        {
            for (int x = 0; x < tiles.Length / 7; x++)
            {
                // Generates tile and adds it to the array
                tiles[i * 7 + x] = Instantiate(tileNormal,
                    new Vector3(i - 3, x - 3, 0),
                    Quaternion.identity);
            }
        }
    }
}
