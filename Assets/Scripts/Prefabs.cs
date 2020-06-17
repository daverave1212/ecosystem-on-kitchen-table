using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour {

    public static Prefabs Instance  {get; private set;} = null;

    public GameObject treePrefab;

    public GameObject _tileIndicatorPrefab;
    public GameObject _tileIndicatorRedPrefab;
    public GameObject _tileIndicatorYellowPrefab;

    public GameObject tilePrefab;
    public GameObject plantPrefab;
    public GameObject grassPrefab;
    public GameObject flowersPrefab;

    public GameObject rabbitPrefab;
    public GameObject foxPrefab;

    public GameObject leavesPrefab;
    public GameObject windPrefab;

    public GameObject carrotParticlesPrefab;
    public GameObject heartParticlesPrefab;
    public GameObject meatParticlesPrefab;
    public GameObject skullParticlesPrefab;

    void Awake() {
        if (Instance != null) throw new System.Exception("Prefabs singleton already instantiated!");
        Instance = this;
    }


}
