using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotParticles : MonoBehaviour
{

    void DestroySelf() {
        Destroy(gameObject);
    }

    void Start() {
        Invoke("DestroySelf", 2.0f);
    }
}
