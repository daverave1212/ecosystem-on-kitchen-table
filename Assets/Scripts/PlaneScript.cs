using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{

    // Drag and drop the prefabs / materials here in Unity
    public GameObject tilePrefab;
    public Material grassMaterial;
    public Material waterMaterial;
    public Material sandMaterial;

    const float TILE_SIZE_X = 1.0f;
    const float TILE_SIZE_Y = 0.25f;
    const float TILE_SIZE_Z = 1.0f;

    const int nTilesRows = 16;
    const int nTilesCols = 16;

    GameObject[,] tiles;

    void SpawnTiles() {
        float totalWidth = nTilesRows * TILE_SIZE_X;
        float totalHeight = nTilesCols * TILE_SIZE_Z;
        float topMostPoint = 0 - totalHeight / 2;
        float leftMostPoint = 0 - totalWidth / 2;
        tiles = new GameObject[nTilesRows, nTilesCols];
        for (int row = 0; row<nTilesRows; row++) {
            for (int col = 0; col<nTilesCols; col++) {
                var newTile = Instantiate(tilePrefab);
                newTile.gameObject.transform.position = new Vector3(
                    leftMostPoint + col * TILE_SIZE_X,
                    TILE_SIZE_Y / 2,
                    topMostPoint + row * TILE_SIZE_Z
                );
                newTile.GetComponent<Renderer>().material = grassMaterial;
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
