using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour {

    public static Materials Instance {get; private set;} = null;

    public Material grassMaterial;
    public Material waterMaterial;
    public Material sandMaterial;

    void Awake() {
        if (Instance != null) throw new System.Exception("Materials singleton already instantiated!");
        Instance = this;
    }

}
