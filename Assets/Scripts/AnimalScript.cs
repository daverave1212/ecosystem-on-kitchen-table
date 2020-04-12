using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{

    public static int NONE   = 0;

    public static int UP     = 1;
    public static int RIGHT  = 2;
    public static int DOWN   = 3;
    public static int LEFT   = 4;

    public static int NO_MOOD = 0;
    public static int EAT   = 1;
    public static int MATE  = 2;
    public static int RUN   = 3;

    public string name = "Animal";  // Name of the species
    public float speed = 1;         // Ticks every <speed> seconds
    public bool isFemale = false;
    public int maxHunger = 100;
    public int currentHunger = 50;  // == maxHunger means it is perfectly saturated
    public int maxHappiness = 100;
    public int currentHappiness = 25;   // == maxHappiness means it will make babies!
    public float size = 1;
    public TileScript tileOn;
    
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
        if (IsHungry()) return EAT;
        if (IsReadyToMate()) return MATE;
        return NO_MOOD;
    }


    // To do: leap to tile
    public void LeapToTile(TileScript tile) {
        // Don't forget to clear tileOn and animalOn
    }

    // To do: move one tile in its current direction
    public void Tick() {
        int directionToMoveTo = FindPath();
        TileScript tileToMoveTo = tileOn.GetAdjacentTile(directionToMoveTo);
        LeapToTile(tileToMoveTo);
    }

    public void MoveToPosition(Vector3 to) {
        //var q = Quaternion.LookRotation(to - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 0);
        transform.LookAt(to);
        GetComponent<Animator>().Play("Run");
        GetComponent<Slider>().SlideTo(to, delegate (int param) {
            print("Done moving");
            GetComponent<Animator>().Play("Idle");
        });

    }


    public void MoveToTile(GameObject tile) {
        // TODO: clear tileOn and animalOn
        var to = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        MoveToPosition(to);
    }

    public void PutOnTile(GameObject tile) {
        gameObject.transform.position = new Vector3(tile.transform.position.x, PlaneScript.ANIMAL_FEET_HEIGHT, tile.transform.position.y);
        tileOn = tile.GetComponent<TileScript>();
        tileOn.animalOn = this;
    }
    public void PutOnTile(TileScript tile) {
        gameObject.transform.position = new Vector3(tile.gameObject.transform.position.x, PlaneScript.ANIMAL_FEET_HEIGHT, tile.gameObject.transform.position.y);
        tileOn = tile;
        tileOn.animalOn = this;
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
