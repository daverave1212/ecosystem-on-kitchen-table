using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public static Spawner Instance {get; private set;} = null;

    public static GameObject SpawnPlant(Vector3 position) {
        var plantPrefab = Prefabs.Instance.plantPrefab;
        var plantOn = Instantiate(Prefabs.Instance.plantPrefab);
        plantOn.tag = "Plant";
        plantOn.transform.position = position;
        return plantOn;
    }

    public static RabbitScript SpawnRabbit(TileScript onWhichTile, RabbitScript[] parents = null) {
        if (!onWhichTile.IsFree()) throw new System.Exception($"Error: Tile to spawn rabbit ({onWhichTile.row},{onWhichTile.col}) is not free!");
        PlaneScript.Instance._nRabbits ++;
        var theRabbit = Instantiate(Prefabs.Instance.rabbitPrefab);
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
        PlaneScript.Instance._nFoxes ++;
        var theFox = Instantiate(Prefabs.Instance.foxPrefab);
        theFox.GetComponent<Animator>().Play("Spawn");
        theFox.tag = "Animal";
        var foxScript = theFox.GetComponent<FoxScript>();
        foxScript.PutOnTile(onWhichTile);
        foxScript.parents = parents;
        PlaneScript.foxes.Add(foxScript);
        return foxScript;
    }

    void Start() {
        if (Instance != null) throw new System.Exception("Spawner singleton already instantiated!");
        Instance = this;
    }

}
