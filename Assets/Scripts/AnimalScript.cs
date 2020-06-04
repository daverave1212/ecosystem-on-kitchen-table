using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimalScript : MonoBehaviour
{
    public bool _debug = false;


    public AnimalScript[] parents;
    public string name = "Animal";  // Name of the species
    public float speed = 2;         // Ticks every <speed> seconds
    public bool isFemale = false;
    public int maxHunger = 100;
    public int currentHunger = 50;  // == maxHunger means it is perfectly saturated
    public int maxHappiness = 100;
    public int currentHappiness = 50;   // == maxHappiness means it will make babies!
    public float size = 1;
    public TileScript tileOn;

    public AnimalScript myMate = null;
    public bool isMeetingMate = false;          // When an animal wants to mate and another one is available, they will meet in the middle
    public TileScript mateMeetingTile = null;
    public bool makesFirstStep = false;

    public void AddHunger(int amount) { currentHunger = currentHunger + amount; }
    public void AddHappiness(int amount) { currentHappiness += amount; }
    public bool IsHungry() { return currentHunger < ((float) maxHunger) * 0.75; }
    public bool IsReadyToMate() { return currentHappiness >= maxHappiness; }

    private void updateTile(TileScript newTile) {
        if (tileOn != null) tileOn.animalOn = null;
        tileOn = newTile;
        newTile.animalOn = this;
    }

    public Vector2Int GetPositionInMatrix() { return new Vector2Int(tileOn.row, tileOn.col); }

    public int GetMood() {
        if (IsReadyToMate()) {
            print("I want luv..");
            return K.MATE;
        }
        if (IsHungry()) {
            print("I is hungry..");
            return K.EAT;
        }
        AddHappiness(5);
        print("Getting happier...");
        return K.NO_MOOD;
    }


    // Override these two functions
    public void Tick() { print("Animal tick not overriden!!!"); }
    public int FindPath() {
        print("FindPath not overriden!!");
        return K.NONE;
    }


    public void MoveToPosition(Vector3 to) {
        transform.LookAt(to);
        GetComponent<Animator>().Play("Run");
        GetComponent<Slider>().SlideTo(to, delegate (int param) {
            //GetComponent<Animator>().Play("Idle");
        });

    }
    public void MoveToTile(GameObject tileObject) {
        var tile = tileObject.GetComponent<TileScript>();
        MoveToTile(tile);
        
    }
    public void MoveToTile(TileScript tile) {
        if (tile == tileOn) return;
        updateTile(tile);
        var to = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        MoveToPosition(to);
    }
    public void MoveInDirection(int direction) {
        var adjacentTile = tileOn.GetAdjacentTile(direction);
        if (adjacentTile == null) {
            print("Null adjacent tile when moving in direction " + K.directionToString[direction]);
        }
        if (adjacentTile != tileOn) {
            MoveToTile(adjacentTile);
        }
    }

    public void PutOnTile(GameObject tileObject) {
        gameObject.transform.position = new Vector3(tileObject.transform.position.x, K.ANIMAL_FEET_HEIGHT, tileObject.transform.position.y);
        var tile = tileObject.GetComponent<TileScript>();
        updateTile(tile);
    }

    void _ToThatTile() {
        print("Movin");
        gameObject.transform.position = tileOn.GetPosition(defaultY : K.ANIMAL_FEET_HEIGHT);
    }
    public void PutOnTile(TileScript tile) {
        updateTile(tile);
        gameObject.transform.position = tileOn.GetPosition(defaultY : K.ANIMAL_FEET_HEIGHT);
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

    public bool EatAdjacentPlantIfNear() {    // If the animal is near a plant, it eats it; returns true if it ate a plant
        var adjacentTiles = tileOn.GetAdjacentTiles();
        foreach (var tile in adjacentTiles) {
            if (tile.HasPlant()) {
                transform.LookAt(tile.plantOn.transform);
                tile.KillPlant();
                AddHunger(50);
                return true;
            }
        }
        return false;
    }

    public AnimalScript GetAdjacentRabbit() {
        var adjacentTiles = tileOn.GetAdjacentTiles();
        foreach (var tile in adjacentTiles) {
            if (tile.HasRabbit()) {
                print($"Tile {tile.row}, {tile.col} definitely has a rabbit.");
                return tile.animalOn;
            }
        }
        return null;
    }

    public void LookAtAnimal(AnimalScript animal) {
        gameObject.transform.LookAt(animal.transform);
    }

    public TileScript GetRandomAvailableAdjacentTile() {
        var adjacentTiles = tileOn.GetAdjacentTiles();
        if (adjacentTiles.Length > 0) {
            return adjacentTiles[UnityEngine.Random.Range(0, adjacentTiles.Length)];
        }
        return null;
    }

    public int GetRandomAvailableDirection() {
        var adjacentTiles = tileOn.GetAdjacentTiles();
        for (int i = 0; i < adjacentTiles.Length; i++) {
             int rnd = UnityEngine.Random.Range(0, adjacentTiles.Length);
             var aux = adjacentTiles[rnd];
             adjacentTiles[rnd] = adjacentTiles[i];
             adjacentTiles[i] = aux;
        }
        foreach (var tile in adjacentTiles)
            if (tile.IsFree()) return tileOn.GetDirectionToAdjacentTile(tile);
        return K.NONE;
    }

    
}
