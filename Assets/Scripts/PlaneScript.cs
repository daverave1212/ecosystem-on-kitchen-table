using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneScript : MonoBehaviour
{

    // Drag and drop the prefabs / materials here in Unity
    public GameObject tilePrefab;

    public static float TILE_SIZE_X = 1.0f;
    public static float TILE_SIZE_Y = 0.25f;
    public static float TILE_SIZE_Z = 1.0f;
    public static float ANIMAL_FEET_HEIGHT = 0.125f;

    const int nTilesRows = 16;
    const int nTilesCols = 16;

    public static string[,] terrainMatrix;
    public static GameObject[,] tiles;

    void SpawnTiles() {
        MapGeneration mapGenerator = new MapGeneration();
        terrainMatrix = mapGenerator.Generate(16, 3, 3, 50);
        float totalWidth = nTilesRows * TILE_SIZE_X;
        float totalHeight = nTilesCols * TILE_SIZE_Z;
        float topMostPoint = 0 - totalHeight / 2;
        float leftMostPoint = 0 - totalWidth / 2;
        tiles = new GameObject[nTilesRows, nTilesCols];
        var rand = new System.Random();
        for (int row = 0; row<nTilesRows; row++) {
            for (int col = 0; col<nTilesCols; col++) {
                var newTile = Instantiate(tilePrefab);
                var x = leftMostPoint + col * TILE_SIZE_X;
                var y = TILE_SIZE_Y / 2;
                var z = topMostPoint + row * TILE_SIZE_Z;
                var type = terrainMatrix[row, col];
                newTile.GetComponent<TileScript>().Initialize(x, y, z, row, col, type);
                tiles[row,col] = newTile;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        SpawnTiles();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
