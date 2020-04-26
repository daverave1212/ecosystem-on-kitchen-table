using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{
    public bool _debug = true;

    public static int NONE   = 0;

    public static int UP     = 1;
    public static int RIGHT  = 2;
    public static int DOWN   = 3;
    public static int LEFT   = 4;

    public static int NO_MOOD = 0;
    public static int EAT   = 1;
    public static int MATE  = 2;
    public static int RUN   = 3;

    public static string[] moodToString = {"NO_MOOD", "EAT", "MATE", "RUN"};
    public static string[] directionToString = {"NONE", "UP", "RIGHT", "DOWN", "LEFT"};

    public string name = "Animal";  // Name of the species
    public float speed = 2;         // Ticks every <speed> seconds
    public bool isFemale = false;
    public int maxHunger = 100;
    public int currentHunger = 50;  // == maxHunger means it is perfectly saturated
    public int maxHappiness = 100;
    public int currentHappiness = 25;   // == maxHappiness means it will make babies!
    public float size = 1;
    public TileScript tileOn;

    private void updateTile(TileScript newTile) {
        if (tileOn != null) tileOn.animalOn = null;
        if (_debug) tileOn?.SetMaterial(PlaneScript.self._blueMaterial);
        tileOn = newTile;
        if (_debug) tileOn?.SetMaterial(PlaneScript.self._magentaMaterial);
        newTile.animalOn = this;
    }
    
    public int FindPath() {
        print("FindPath not overriden!!");
        return NONE;
    }

    public bool IsHungry() { return currentHunger <= maxHunger / 2; }
    public bool IsReadyToMate() { return currentHappiness >= maxHappiness; }
    public Vector2Int GetPositionInMatrix() {
        return new Vector2Int(tileOn.row, tileOn.col);
    }

    public int GetMood() {
        if (IsHungry()) {
            print("I is hungry..");
            return EAT;
        }
        if (IsReadyToMate()) {
            print("I want luv..");
            return MATE;
        }
        print("Im ok :3");
        return NO_MOOD;
    }

    // To do: move one tile in its current direction
    public void Tick() { print("Animal tick not overriden!!!"); }

    public void MoveToPosition(Vector3 to) {
        transform.LookAt(to);
        GetComponent<Animator>().Play("Run");
        GetComponent<Slider>().SlideTo(to, delegate (int param) {
            print("Done moving");
            //GetComponent<Animator>().Play("Idle");
        });

    }
    public void MoveToTile(GameObject tileObject) {
        var tile = tileObject.GetComponent<TileScript>();
        MoveToTile(tile);
        
    }
    public void MoveToTile(TileScript tile) {
        updateTile(tile);
        var to = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        MoveToPosition(to);
    }
    public void MoveInDirection(int direction) {
        var adjacentTile = tileOn.GetAdjacentTile(direction);
        if (adjacentTile == null) {
            print("Null adjacent tile when moving in direction " + directionToString[direction]);
        }
        if (adjacentTile != tileOn) {
            MoveToTile(adjacentTile);
        }
    }

    public void PutOnTile(GameObject tileObject) {
        gameObject.transform.position = new Vector3(tileObject.transform.position.x, PlaneScript.ANIMAL_FEET_HEIGHT, tileObject.transform.position.y);
        var tile = tileObject.GetComponent<TileScript>();
        updateTile(tile);
    }

    void _ToThatTile() {
        print("Movin");
        gameObject.transform.position = tileOn.GetPosition(defaultY : PlaneScript.ANIMAL_FEET_HEIGHT);
    }
    public void PutOnTile(TileScript tile) {
        updateTile(tile);
        gameObject.transform.position = tileOn.GetPosition(defaultY : PlaneScript.ANIMAL_FEET_HEIGHT);
        //Invoke("_ToThatTile", 2);
    }

    public void CreateAsOffspring(AnimalScript mother, AnimalScript father) {
        maxHunger = (mother.maxHunger + father.maxHunger) / 2;
        maxHappiness = (mother.maxHappiness + father.maxHappiness) / 2;
        size = (mother.size + father.size) / 2;
        speed = (mother.speed + father.speed) / 2;
    }

    public void Mutate() {
        System.Random rand = new System.Random();
        float generateMutationNumber() { return (float)rand.Next(9, 12) * 10.0f; }
        speed *= generateMutationNumber();
        maxHunger = (int) (maxHunger * generateMutationNumber());
        maxHappiness = (int) (maxHappiness * generateMutationNumber());
        size *= generateMutationNumber();
        if (generateMutationNumber() > 1) isFemale = true;
        gameObject.transform.localScale = new Vector3(size, size, size);
    }
    
}
