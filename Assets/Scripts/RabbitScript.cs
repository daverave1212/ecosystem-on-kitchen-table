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
    int FindPath(int mood) {
        if (mood == NO_MOOD) {
            return NONE;
        } else if (mood == EAT) {
            // Find path to closest plant
            // return the direction to go to (UP, DOWN, LEFT, RIGHT)
        } else if (mood == MATE) {
            // Find path to closest rabbit who is also ready to mate
        } else if (mood == RUN) {
            // Don't make this one for now
        }
        return NONE;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
