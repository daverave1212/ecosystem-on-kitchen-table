using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour {

    public static Materials self;

    public Material grassMaterial;
    public Material waterMaterial;
    public Material sandMaterial;

    void Start() {
        self = this;
    }

}
