using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public static Spawner self;

    public static GameObject SpawnPlant(Vector3 position) {
        var plantPrefab = Prefabs.self.plantPrefab;
        var plantOn = Instantiate(Prefabs.self.plantPrefab);
        plantOn.tag = "Plant";
        plantOn.transform.position = position;
        return plantOn;
    }

    public static RabbitScript SpawnRabbit(TileScript onWhichTile, RabbitScript[] parents = null) {
        if (!onWhichTile.IsFree()) throw new System.Exception($"Error: Tile to spawn rabbit ({onWhichTile.row},{onWhichTile.col}) is not free!");
        PlaneScript.self._nRabbits ++;
        var theRabbit = Instantiate(Prefabs.self.rabbitPrefab);
        theRabbit.GetComponent<Animator>().Play("Spawn");
        theRabbit.tag = "Animal";
        var rabbitScript = theRabbit.GetComponent<RabbitScript>();
        rabbitScript.PutOnTile(onWhichTile);
        rabbitScript.parents = parents;
        PlaneScript.rabbits.Add(rabbitScript);
        return rabbitScript;
    }

    public static FoxScript SpawnFox(TileScript onWhichTile, FoxScript[] parents = null) {
        if (!onWhichTile.IsFree()) throw new System.Exception("Error: Tile to spawn fox is not free!");
        PlaneScript.self._nFoxes ++;
        var theFox = Instantiate(Prefabs.self.foxPrefab);
        theFox.GetComponent<Animator>().Play("Spawn");
        theFox.tag = "Animal";
        var foxScript = theFox.GetComponent<FoxScript>();
        foxScript.PutOnTile(onWhichTile);
        foxScript.parents = parents;
        PlaneScript.foxes.Add(foxScript);
        return foxScript;
    }

    void Start()
    {
        self = this;
    }

}
