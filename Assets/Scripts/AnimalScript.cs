using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimalScript : MonoBehaviour
{
    public bool _debug = false;


    public AnimalScript[] parents;
    public string name = "Animal";  // Name of the species
    public float speed = 1;         // Ticks every <speed> seconds
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

    public void Kill() {
        if (tileOn != null) tileOn.animalOn = null;
        if (isMeetingMate && myMate != null) {
            myMate.isMeetingMate = false;
            myMate.myMate = null;
            myMate.makesFirstStep = false;
            myMate.mateMeetingTile = null;
        }
        if (name == "Bunny") {
            PlaneScript.rabbits.Remove((RabbitScript) this);
        } else if (name == "Fox") {
            PlaneScript.foxes.Remove((FoxScript) this);
        } else {
            throw new Exception($"Error killing animal with name {name}; No such animal type exists.");
        }
        var theTile = tileOn;
        SpawnSkullParticles();
        Destroy(gameObject);
        theTile.SpawnPlant();
    }

    public void Explode() {
        SpawnMeatParticles();
        Kill();
    }

    public void SpawnMeatParticles() {
        var particles = Instantiate(PlaneScript.self.meatParticlesPrefab);    // It will autodestruct because it has a script, no worries
        particles.transform.position = new Vector3(
            gameObject.transform.position.x,
            gameObject.transform.position.y + K.ANIMAL_FEET_HEIGHT,
            gameObject.transform.position.z
        );
    }

    public void SpawnSkullParticles() {
        var particles = Instantiate(PlaneScript.self.skullParticlesPrefab);    // It will autodestruct because it has a script, no worries
        particles.transform.position = new Vector3(
            gameObject.transform.position.x,
            gameObject.transform.position.y + K.ANIMAL_FEET_HEIGHT,
            gameObject.transform.position.z
        );
    }

    private void updateTile(TileScript newTile) {
        if (tileOn != null) tileOn.animalOn = null;
        tileOn = newTile;
        newTile.animalOn = this;
    }

    public Vector2Int GetPositionInMatrix() { return new Vector2Int(tileOn.row, tileOn.col); }

    public int GetMood() {
        if (IsReadyToMate()) {
            return K.MATE;
        }
        if (IsHungry()) {
            return K.EAT;
        }
        AddHappiness(5);
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
            GetComponent<Animator>().Play("Idle");
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
        gameObject.transform.position = tileOn.GetPosition(defaultY : K.ANIMAL_FEET_HEIGHT);
    }

    public void PutOnTile(TileScript tile) {
        updateTile(tile);
        gameObject.transform.position = tileOn.GetPosition(defaultY : K.ANIMAL_FEET_HEIGHT);
    }


    private void ResetSize() { transform.localScale = new Vector3(size, size, size); }
    public void CreateAsOffspring(AnimalScript mother, AnimalScript father) {
        maxHunger = (mother.maxHunger + father.maxHunger) / 2;
        maxHappiness = (mother.maxHappiness + father.maxHappiness) / 2;
        size = (mother.size + father.size) / 2;
        speed = (mother.speed + father.speed) / 2;
        transform.localScale = new Vector3(size/2, size/2, size/2);
        Invoke("ResetSize", 20);
    }

    public void Mutate() {
        System.Random rand = new System.Random();
        float generateMutationNumber() { return (float)rand.Next(9, 12) * 0.1f; }
        speed *= generateMutationNumber();
        maxHunger = (int) (maxHunger * generateMutationNumber());
        maxHappiness = (int) (maxHappiness * generateMutationNumber());
        size *= generateMutationNumber();
        if (generateMutationNumber() > 1) isFemale = true;
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

    public bool EatAdjacentAndDiagonalRabbitIfNear() {
        var rabbit = GetAdjacentAndDiagonalRabbit();
        if (rabbit == null) return false;
        LookAtAnimal(rabbit);
        rabbit.Explode();
        AddHunger(75);
        return true;
    }

    public AnimalScript GetAdjacentRabbit() {   // Gets the first found adjacent rabbit
        var adjacentTiles = tileOn.GetAdjacentTiles();
        foreach (var tile in adjacentTiles) {
            if (tile.HasRabbit()) {
                print($"Tile {tile.row}, {tile.col} definitely has a rabbit.");
                return tile.animalOn;
            }
        }
        return null;
    }

    public AnimalScript GetAdjacentAndDiagonalRabbit() {   // Gets the first found adjacent rabbit
        var adjacentTiles = tileOn.GetAdjacentTilesAndDiagonally();
        foreach (var tile in adjacentTiles) {
            if (tile.HasRabbit()) {
                return tile.animalOn;
            }
        }
        return null;
    }

    public AnimalScript GetAdjacentFox() {
        var adjacentTiles = tileOn.GetAdjacentTiles();
        foreach (var tile in adjacentTiles) {
            if (tile.HasFox()) {
                print($"Tile {tile.row}, {tile.col} definitely has a fox.");
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

    public void SpawnLoveParticles() {
        var particles = Instantiate(PlaneScript.self.heartParticlesPrefab);    // It will autodestruct because it has a script, no worries
        particles.transform.position = new Vector3(
            (gameObject.transform.position.x + myMate.transform.position.x) / 2,
            (gameObject.transform.position.y + myMate.transform.position.y) / 2,
            (gameObject.transform.position.z + myMate.transform.position.z) / 2
        );
    }



    public void MakeBabyWithMyMateAndClear(TileScript spawnBabyOnWhichTile) {
        AnimalScript ourBaby = null;
        if (name == "Bunny")
            ourBaby = PlaneScript.SpawnRabbit(spawnBabyOnWhichTile, new[] {(RabbitScript)this, (RabbitScript)myMate});
        else if (name == "Fox")
            ourBaby = PlaneScript.SpawnFox(spawnBabyOnWhichTile, new[] {(FoxScript)this, (FoxScript)myMate});
        ourBaby.CreateAsOffspring(this, myMate);
        ourBaby.Mutate();
        AddHunger(-25);
        AddHappiness(-75);
        myMate.AddHunger(-25);
        myMate.AddHappiness(-75);
        myMate.isMeetingMate = false;
        myMate.mateMeetingTile = null;
        myMate.makesFirstStep = false;
        myMate.myMate = null;
        isMeetingMate = false;
        makesFirstStep = false;
        mateMeetingTile = null;
        myMate = null;
    }

    
}
