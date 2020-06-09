using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public Material grassMaterial;
    public Material waterMaterial;
    public Material sandMaterial;

    public GameObject treePrefab;

    public static string GRASS = "grass";
    public static string WATER = "water";
    public static string SAND  = "sand";
    public static bool isWaterAnimated = true;



    public string type = "grass";
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
    public bool HasRabbit() { return HasAnimal() && animalOn.name == "Bunny";}
    public bool HasFox() { return HasAnimal() && animalOn.name == "Fox";}
    public bool HasPlant() { return plantOn != null; }
    public bool HasTree() { return tree != null; }
    public bool IsOccupied() { return type == "water" || HasAnimal() || HasPlant() || HasTree(); }
    public bool IsFree() { return !IsOccupied(); }
    public void SpawnPlant() {
        var plantPrefab = PlaneScript.self.plantPrefab;
        plantOn = Instantiate(plantPrefab);
        plantOn.tag = "Plant";
        plantOn.transform.position = this.transform.position;
    }

    
    public TileScript GetLeftTile()  { return PlaneScript.GetTileScript(row, col - 1); }
    public TileScript GetDownTile()  { return PlaneScript.GetTileScript(row, col + 1); }
    public TileScript GetRightTile() { return PlaneScript.GetTileScript(row + 1, col); }
    public TileScript GetUpTile()    { return PlaneScript.GetTileScript(row - 1, col); }
    public TileScript GetUpLeftTile()  { return PlaneScript.GetTileScript(row - 1, col - 1); }
    public TileScript GetUpRightTile()  { return PlaneScript.GetTileScript(row - 1, col + 1); }
    public TileScript GetDownLeftTile()  { return PlaneScript.GetTileScript(row + 1, col - 1); }
    public TileScript GetDownRightTile()  { return PlaneScript.GetTileScript(row + 1, col + 1); }

    public TileScript GetAdjacentTile(int direction) {  // direction = AnimalScript.UP or DOWN or etc
        if (direction == K.UP)      return GetUpTile();
        if (direction == K.RIGHT)   return GetRightTile();
        if (direction == K.DOWN)    return GetDownTile();
        if (direction == K.LEFT)    return GetLeftTile();
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

    public void Initialize(float x, float y, float z, int i, int j, string type) {
        row = i;
        col = j;
        var yOffset = 0.05f;
        if (type == "tree") {
            type = "grass";
            tree = Instantiate(treePrefab);
            tree.tag = "Plant";
            tree.transform.position = new Vector3(x, y, z);
            var theRotation = UnityEngine.Random.Range(0, 360);
            tree.transform.eulerAngles = new Vector3(0, theRotation, 0);
            if (Random.Range(1, 3) == 1) {
                var leaves = Instantiate(PlaneScript.self.leavesPrefab);
                PlaneScript.self.allLeaves.Add(leaves);
                leaves.transform.position = new Vector3(x, y + K.LEAVES_HEIGHT, z);
                leaves.transform.eulerAngles = new Vector3(0, PlaneScript.self.leavesRotation, 0);
            }
        }
        this.type = type;
        gameObject.transform.position = new Vector3(x, y, z);
        var material = grassMaterial;
        if (type == "grass") {
            gameObject.transform.localScale += new Vector3(0, yOffset, 0);
            gameObject.transform.position += new Vector3(0, yOffset / 2, 0);
            if (Random.Range(1, 3) == 1) {
                grassOn = Instantiate(PlaneScript.self.grassPrefab);
                grassOn.transform.position = new Vector3(transform.position.x, transform.position.y + K.ANIMAL_FEET_HEIGHT, transform.position.z);
                //grassOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
                float theRotation = UnityEngine.Random.Range(0, 360);
                grassOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
            }
            if (Random.Range(1, 5) == 1) {
                flowersOn = Instantiate(PlaneScript.self.flowersPrefab);
                flowersOn.tag = "Plant";
                flowersOn.transform.position = new Vector3(transform.position.x, transform.position.y + K.ANIMAL_FEET_HEIGHT, transform.position.z);
                float theRotation = UnityEngine.Random.Range(0, 360);
                flowersOn.transform.eulerAngles = new Vector3(0, theRotation, 0);
            }
        }
        if (type == "sand") {
            material = sandMaterial;
        }
        if (type == "water") {
            material = waterMaterial;
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

    public int GetDirectionToAdjacentTile(TileScript adjacentTile) {
        if (adjacentTile == null) return K.NONE;
        var up    = GetUpTile();
        var right = GetRightTile();
        var down  = GetDownTile();
        var left  = GetLeftTile();
        if (adjacentTile == up)     return K.UP;
        if (adjacentTile == right)  return K.RIGHT;
        if (adjacentTile == down)   return K.DOWN;
        if (adjacentTile == left)   return K.LEFT;
        var stackTrace = new System.Diagnostics.StackTrace();
        print(stackTrace);
        throw new System.Exception($"Error for tile ({row}, {col}): adjacentTile ({adjacentTile.row}, {adjacentTile.col}) given to GetDirectionToAdjacentTile is not adjacent!");
    }

    public void KillPlant() {
        if (HasPlant()) {
            var particles = Instantiate(PlaneScript.self.carrotParticlesPrefab);    // It will autodestruct because it has a script, no worries
            particles.transform.position = plantOn.transform.position + new Vector3(0, 0.5f, 0);
            Destroy(plantOn);
        }
    }

    public void _MarkBlue() {
        var indicator = Instantiate(PlaneScript.self._tileIndicatorPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
    public void _MarkRed() {
        var indicator = Instantiate(PlaneScript.self._tileIndicatorRedPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
    public void _MarkYellow() {
        var indicator = Instantiate(PlaneScript.self._tileIndicatorYellowPrefab);
        indicator.transform.position = gameObject.transform.position;
    }
}
