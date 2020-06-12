using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public enum TileType {
        Grass,
        Water,
        Sand,
        Tree,
        None
    }

    /*public static string GRASS = "grass";
    public static string WATER = "water";
    public static string SAND  = "sand";*/
    public static bool isWaterAnimated = true;

    //public string type = "grass";
    public TileType type = TileType.Grass;
    public int row = 0;
    public int col = 0;
    public AnimalScript animalOn = null;
    public GameObject tree = null;
    public GameObject plantOn = null;
    public GameObject grassOn = null;   // only visual, has no effect
    public GameObject flowersOn = null; // only visual, has no effect

    static float floatSpeed = 0.05f;
    static float floatLimit = 0.06f;
    bool isWater = false;
    float floatAmount = 0;
    bool goingDown = true;

    public bool HasAnimal() { return animalOn != null; }
    public bool HasAnimal(string animalName) { return animalOn != null && animalOn.name == animalName; }
    public bool HasPlant() { return plantOn != null; }
    public bool HasTree() { return tree != null; }
    public bool IsOccupied() { return type == TileType.Water || HasAnimal() || HasPlant() || HasTree(); }
    public bool IsFree() { return !IsOccupied(); }
    public void SpawnPlant() { plantOn = Spawner.SpawnPlant(transform.position); }

    public TileScript GetLeftTile()  { return PlaneScript.GetTileScript(row, col - 1); }
    public TileScript GetDownTile()  { return PlaneScript.GetTileScript(row, col + 1); }
    public TileScript GetRightTile() { return PlaneScript.GetTileScript(row + 1, col); }
    public TileScript GetUpTile()    { return PlaneScript.GetTileScript(row - 1, col); }
    public TileScript GetUpLeftTile()  { return PlaneScript.GetTileScript(row - 1, col - 1); }
    public TileScript GetUpRightTile()  { return PlaneScript.GetTileScript(row - 1, col + 1); }
    public TileScript GetDownLeftTile()  { return PlaneScript.GetTileScript(row + 1, col - 1); }
    public TileScript GetDownRightTile()  { return PlaneScript.GetTileScript(row + 1, col + 1); }

    public TileScript GetAdjacentTile(Direction direction) {  // direction = AnimalScript.UP or DOWN or etc
        if (direction == Direction.Up)      return GetUpTile();
        if (direction == Direction.Right)   return GetRightTile();
        if (direction == Direction.Down)    return GetDownTile();
        if (direction == Direction.Left)    return GetLeftTile();
        return this;
    }

    public TileScript[] GetAdjacentTiles() {
        var up    = GetUpTile();
        var right = GetRightTile();
        var down  = GetDownTile();
        var left  = GetLeftTile();
        List<TileScript> adjacentTiles = new List<TileScript>();
        if (up != null)    adjacentTiles.Add(up);
        if (right != null) adjacentTiles.Add(right);
        if (down != null)  adjacentTiles.Add(down);
        if (left != null)  adjacentTiles.Add(left);
        return adjacentTiles.ToArray();
    }

    public TileScript[] GetAdjacentTilesAndDiagonally() {
        var upLeft = GetUpLeftTile();
        var upRight = GetUpRightTile();
        var downLeft = GetDownLeftTile();
        var downRight = GetDownRightTile();
        List<TileScript> diagonalTiles = new List<TileScript>();
        if (upLeft != null)     diagonalTiles.Add(upLeft);
        if (upRight != null)    diagonalTiles.Add(upRight);
        if (downLeft != null)   diagonalTiles.Add(downLeft);
        if (downRight != null)  diagonalTiles.Add(downRight);
        diagonalTiles.AddRange(GetAdjacentTiles());
        return diagonalTiles.ToArray();
    }

    const float DEFAULT_VAL = -420.1337f;
    public Vector3 GetPosition(float defaultX=DEFAULT_VAL, float defaultY=DEFAULT_VAL, float defaultZ=DEFAULT_VAL) {
        return new Vector3(
            (defaultX == DEFAULT_VAL)? (gameObject.transform.position.x) :(defaultX),
            (defaultY == DEFAULT_VAL)? (gameObject.transform.position.y) :(defaultY),
            (defaultZ == DEFAULT_VAL)? (gameObject.transform.position.z) :(defaultZ)
        );
    }

    public void Initialize(float x, float y, float z, int i, int j, TileType type) {
        row = i;
        col = j;
        var yOffset = 0.05f;
        if (type == TileType.Tree) {
            type = TileType.Grass;
            tree = Instantiate(Prefabs.self.treePrefab);
            tree.tag = "Plant";
            tree.transform.position = new Vector3(x, y, z);
            var theRotation = UnityEngine.Random.Range(0, 360);
            tree.transform.eulerAngles = new Vector3(0, theRotation, 0);
            if (Random.Range(1, 3) == 1) {
                var leaves = Instantiate(Prefabs.self.leavesPrefab);
                leaves.tag = "Particles";
                PlaneScript.self.allLeaves.Add(leaves);
                leaves.transform.position = new Vector3(x, y + K.LEAVES_HEIGHT, z);
                leaves.transform.eulerAngles = new Vector3(0, PlaneScript.self.leavesRotation, 0);
            }
        }
        this.type = type;
        gameObject.transform.position = new Vector3(x, y, z);
        var material = Materials.self.grassMaterial;
        if (type == TileType.Grass) {
            gameObject.transform.localScale += new Vector3(0, yOffset, 0);
            gameObject.transform.position += new Vector3(0, yOffset / 2, 0);
            if (Random.Range(1, 3) == 1) {
                grassOn = Instantiate(Prefabs.self.grassPrefab);
                grassOn.tag = "Plant";
                grassOn.transform.position = new Vector3(transform.position.x, transform.position.y + K.ANIMAL_FEET_HEIGHT, transform.position.z);
                //grassOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
                float theRotation = UnityEngine.Random.Range(0, 360);
                grassOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
            }
            if (Random.Range(1, 5) == 1) {
                flowersOn = Instantiate(Prefabs.self.flowersPrefab);
                flowersOn.tag = "Plant";
                flowersOn.transform.position = new Vector3(transform.position.x, transform.position.y + K.ANIMAL_FEET_HEIGHT, transform.position.z);
                float theRotation = UnityEngine.Random.Range(0, 360);
                flowersOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
            }
        }
        if (type == TileType.Sand) {
            material = Materials.self.sandMaterial;
        }
        if (type == TileType.Water) {
            material = Materials.self.waterMaterial;
            gameObject.transform.localScale -= new Vector3(0, yOffset, 0);
            gameObject.transform.position -= new Vector3(0, yOffset / 2, 0);
            isWater = true;
            floatAmount = Random.Range(-floatLimit, floatLimit);
            transform.position += new Vector3(0, floatAmount, 0);
            if (Random.Range(1, 2) == 1) goingDown = true;
        }
        SetMaterial(material);
        
    }

    void Update() {
        if (!isWater) return;
        if (TileScript.isWaterAnimated == false) return;
        var extraFloat = Time.deltaTime * floatSpeed;
        if (goingDown) {
            transform.position -= new Vector3(0, extraFloat, 0);
            floatAmount -= extraFloat;
            if (floatAmount <= -floatLimit) goingDown = false;
        } else {
            transform.position += new Vector3(0, extraFloat, 0);
            floatAmount += extraFloat;
            if (floatAmount >= floatLimit) goingDown = true;
        }
    }

    public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }

    public Direction GetDirectionToAdjacentTile(TileScript adjacentTile) {
        if (adjacentTile == null) return Direction.None;
        if (adjacentTile == this) return Direction.None;
        var up    = GetUpTile();
        var right = GetRightTile();
        var down  = GetDownTile();
        var left  = GetLeftTile();
        if (adjacentTile == up)     return Direction.Up;
        if (adjacentTile == right)  return Direction.Right;
        if (adjacentTile == down)   return Direction.Down;
        if (adjacentTile == left)   return Direction.Left;
        var stackTrace = new System.Diagnostics.StackTrace();
        print(stackTrace);
        throw new System.Exception($"Error for tile ({row}, {col}): adjacentTile ({adjacentTile.row}, {adjacentTile.col}) given to GetDirectionToAdjacentTile is not adjacent!");
    }

    public void KillPlant() {
        if (HasPlant()) {
            var particles = Instantiate(Prefabs.self.carrotParticlesPrefab);    // It will autodestruct because it has a script, no worries
            particles.transform.position = plantOn.transform.position + new Vector3(0, 0.5f, 0);
            Destroy(plantOn);
        }
    }

    public void _MarkBlue() {
        var indicator = Instantiate(Prefabs.self._tileIndicatorPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
    public void _MarkRed() {
        var indicator = Instantiate(Prefabs.self._tileIndicatorRedPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
    public void _MarkYellow() {
        var indicator = Instantiate(Prefabs.self._tileIndicatorYellowPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
}
