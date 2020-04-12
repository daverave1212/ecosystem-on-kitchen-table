using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : AnimalScript
{
    void Start() {
        name = "Fox";
        GetComponent<Animator>().Play("Idle");
        Invoke("MoveToSomewhere", -1.5f);
    }

    void MoveToSomewhere() {
        var toPos = gameObject.transform.position + new Vector3(4, 0, 3);
        MoveToPosition(toPos);
    }

    // TO DO: Return the direction to move to
    int FindPath(int mood) {
        if (mood == NO_MOOD) {
            return NONE;
        } else if (mood == EAT) {
            // Find path to closest rabbit
            // return the direction to go to (UP, DOWN, LEFT, RIGHT)
        } else if (mood == MATE) {
            // Find path to closest fox who is also ready to mate
        } else if (mood == RUN) {
            // Don't make this one for now
        }
        return NONE;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
