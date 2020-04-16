using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneScript : MonoBehaviour
{

    public Material _magentaMaterial;
    public Material _blueMaterial;

    // Drag and drop the prefabs / materials here in Unity
    public GameObject tilePrefab;
    public GameObject plantPrefab;
    public GameObject grassPrefab;
    public GameObject flowersPrefab;

    public GameObject rabbitPrefab;

    public static float TILE_SIZE_X = 1.0f;
    public static float TILE_SIZE_Y = 0.25f;
    public static float TILE_SIZE_Z = 1.0f;
    public static float ANIMAL_FEET_HEIGHT = 0.25f;

    public static PlaneScript self;

    const int nTilesRows = 32;
    const int nTilesCols = 32;
    const int nTrees = 20;
    const int nInitialPlants = 10;
    const int nInitialRabbits = 1;

    public static string[,] terrainMatrix;
    public static GameObject[,] tiles;

    public static bool tileExists(int i, int j) {
        return i >= 0 && i < nTilesRows && j >= 0 && j < nTilesCols;
    }
    public static GameObject GetTile(int i, int j) {
        if (tileExists(i, j)) return tiles[i, j];
        else return null;
    }

    void SpawnTiles() {
        MapGeneration mapGenerator = new MapGeneration();
        terrainMatrix = mapGenerator.Generate(nTilesRows, 3, 3, 50, nTrees);
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

    void SpawnStartingPlants() {
        print("Spawning plants");
        bool trySpawnOneRandomPlant() {
            var rRow = Random.Range(0, nTilesRows);
            var rCol = Random.Range(0, nTilesCols);
            var theTile = tiles[rRow, rCol].GetComponent<TileScript>();
            if (theTile.IsOccupied()) return false;
            theTile.SpawnPlant();
            return true;
        }
        void trySpawnNTimes(int times) {
            var spawnedRandomPlant = false;
            for (int i = 1; i<=times; i++) {
                spawnedRandomPlant = trySpawnOneRandomPlant();
                if (spawnedRandomPlant) {
                    print("Spawned one plant successfully!");
                    return;
                }
            }
            print("Failed to spawn a plant");
        }
        for (var i = 1; i<nInitialPlants; i++) {
            trySpawnNTimes(5);
        }
    }

    void SpawnStartingRabbits() {
        bool trySpawnOneRabbit() {
            var rRow = Random.Range(0, nTilesRows);
            var rCol = Random.Range(0, nTilesCols);
            var theTile = tiles[rRow, rCol].GetComponent<TileScript>();
            if (theTile.IsOccupied()) return false;
            var theRabbit = Instantiate(rabbitPrefab);
            theRabbit.GetComponent<RabbitScript>().PutOnTile(theTile);
            return true;
        }
        void trySpawnRabbitNTimes(int times) {
            var spawnedRandomRabbit= false;
            for (int i = 1; i<=times; i++) {
                spawnedRandomRabbit = trySpawnOneRabbit();
                if (spawnedRandomRabbit) {
                    print("Spawned one rabbit successfully!");
                    return;
                }
            }
            print("Failed to spawn a rabbit");
        }
        for (int i = 1; i<=nInitialRabbits; i++) {
            trySpawnRabbitNTimes(5);
        }
    }

    // Start is called before the first frame update
    void Start() {
        self = this;
        SpawnTiles();
        SpawnStartingPlants();
        SpawnStartingRabbits();
    }

    
}
