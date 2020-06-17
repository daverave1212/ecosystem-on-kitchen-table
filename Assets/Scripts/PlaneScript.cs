using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneScript : MonoBehaviour
{

    public float leavesRotation = -90;
    public List<GameObject> allLeaves;
    public GameObject wind;

    public static PlaneScript Instance;

    const int nTilesRows = 32;
    const int nTilesCols = 32;
    public int nTrees = 20;
    public int nInitialPlants = 16;
    public int nInitialRabbits = 10;
    public int nInitialFoxes = 2;

    public static TileScript.TileType[,] terrainMatrix;
    public static GameObject[,] tiles;

    public static List<RabbitScript> rabbits = new List<RabbitScript>();
    public static List<FoxScript> foxes = new List<FoxScript>();

    public /*debug*/ int _nRabbits = 0;
    public /*debug*/ int _nFoxes = 0;

    public static bool tileExists(int i, int j) {
        return i >= 0 && i < nTilesRows && j >= 0 && j < nTilesCols;
    }
    public static GameObject GetTile(int i, int j) {
        if (tileExists(i, j)) return tiles[i, j];
        else return null;
    }
    public static TileScript GetTileScript(int i, int j) {
        if (tileExists(i, j)) return tiles[i, j].GetComponent<TileScript>();
        else return null;
    }

    void AdjustWind() {
        leavesRotation = UnityEngine.Random.Range(0, 360);
        wind.transform.eulerAngles = new Vector3(0, leavesRotation, 0);
        foreach (var leaf in allLeaves) {
            leaf.transform.eulerAngles = new Vector3(0, leavesRotation, 0);
        }
    }
    void SpawnTiles() {        
        allLeaves = new List<GameObject>();
        leavesRotation = UnityEngine.Random.Range(0, 360);
        wind = Instantiate(Prefabs.Instance.windPrefab);
        wind.tag = "Particle";
        wind.transform.position = new Vector3(transform.position.x, K.LEAVES_HEIGHT, transform.position.z);
        wind.transform.eulerAngles = new Vector3(0, leavesRotation, 0);
        InvokeRepeating("AdjustWind", 20f, 20f);
        MapGeneration mapGenerator = new MapGeneration();
        terrainMatrix = mapGenerator.Generate(nTilesRows, 3, 3, 50, nTrees);
        float totalWidth = nTilesRows * K.TILE_SIZE_X;
        float totalHeight = nTilesCols * K.TILE_SIZE_Z;
        float topMostPoint = 0 - totalHeight / 2;
        float leftMostPoint = 0 - totalWidth / 2;
        tiles = new GameObject[nTilesRows, nTilesCols];
        var rand = new System.Random();
        for (int row = 0; row<nTilesRows; row++) {
            for (int col = 0; col<nTilesCols; col++) {
                var newTile = Instantiate(Prefabs.Instance.tilePrefab);
                var x = leftMostPoint + col * K.TILE_SIZE_X;
                var y = K.TILE_SIZE_Y / 2;
                var z = topMostPoint + row * K.TILE_SIZE_Z;
                var type = terrainMatrix[row, col];
                newTile.tag = "Tile";
                newTile.GetComponent<TileScript>().Initialize(x, y, z, row, col, type);
                tiles[row,col] = newTile;
            }
        }
    }

    void SpawnStartingPlants() {
        var k = new K();
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
                    return;
                }
            }
            // Failed to spawn one plant
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
            Spawner.SpawnRabbit(theTile);
            return true;
        }
        void trySpawnRabbitNTimes(int times) {
            var spawnedRandomRabbit= false;
            for (int i = 1; i<=times; i++) {
                spawnedRandomRabbit = trySpawnOneRabbit();
                if (spawnedRandomRabbit) {
                    return;
                }
            }
            print("Failed to spawn a rabbit");
        }
        for (int i = 1; i<=nInitialRabbits; i++) {
            trySpawnRabbitNTimes(5);
        }
    }

    void SpawnStartingFoxes() {
        bool trySpawnOneFox() {
            var rRow = Random.Range(0, nTilesRows);
            var rCol = Random.Range(0, nTilesCols);
            var theTile = tiles[rRow, rCol].GetComponent<TileScript>();
            if (theTile.IsOccupied()) return false;
            Spawner.SpawnFox(theTile);
            return true;
        }
        void trySpawnFoxNTimes(int times) {
            var spawnedRandomFox= false;
            for (int i = 1; i<=times; i++) {
                spawnedRandomFox = trySpawnOneFox();
                if (spawnedRandomFox) {
                    return;
                }
            }
            print("Failed to spawn a fox");
        }
        for (int i = 1; i<=nInitialFoxes; i++) {
            trySpawnFoxNTimes(5);
        }
    }

    void DropHunger() {
        foreach (var r in rabbits) { r.AddHunger(-1); }
        foreach (var f in foxes) { f.AddHunger(-2); }
    }

    // Start is called before the first frame update
    void Start() {
        Instance = this;
        SpawnTiles();
        SpawnStartingPlants();
        SpawnStartingRabbits();
        SpawnStartingFoxes();
        InvokeRepeating("DropHunger", 1.0f, 1.0f);
    }

    
}
