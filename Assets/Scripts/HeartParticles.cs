using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartParticles : MonoBehaviour
{

    void DestroySelf() {
        Destroy(gameObject);
    }

    void Start() {
        Invoke("DestroySelf", 5.0f);
    }
}
