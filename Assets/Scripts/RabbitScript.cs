using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript : AnimalScript
{

    void Start() {
        name = "Bunny";
        GetComponent<Animator>().Play("Idle");
        Invoke("MoveToSomewhere", 1.5f);
    }

    void MoveToSomewhere() {
        print(gameObject.transform.position);
        var toPos = gameObject.transform.position + new Vector3(4, 0, 3);
        MoveToPosition(toPos);
    }

    // TO DO: Return the direction to move to
    int FindPath() {
        return AnimalScript.NONE;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
