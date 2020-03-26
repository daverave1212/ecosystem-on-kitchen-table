using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript : AnimalScript
{

    void Start() {
        name = "Bunny";
    }


    // TO DO: Return the direction to move to
    int FindPath() {
        return AnimalScript.NONE;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
