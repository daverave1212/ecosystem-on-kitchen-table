using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

	void Start() {
		name = "Bunny";
		GetComponent<Animator>().Play("Idle");
		//Invoke("_MoveToSomewhere", 1.5f);
        InvokeRepeating("Tick", speed, speed);
	}

	void _MoveToSomewhere() {
		print(gameObject.transform.position);
		var toPos = gameObject.transform.position + new Vector3(4, 0, 3);
		MoveToPosition(toPos);
	}


    public void Tick() {
        var mood = GetMood();
        if (mood == K.EAT) {
            if (EatAdjacentPlantIfNear()) return;
        }
        var direction = FindPath(mood);
        MoveInDirection(direction);
    }

    int FindPath(int mood) {
		if (mood == K.NO_MOOD) {
			return K.NONE;
		} else if (mood == K.EAT) {
            if (EatAdjacentPlantIfNear()) {
                return K.NONE;
            } else {
                return tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_PLANT));
            }
		} else if (mood == K.MATE) {
            var nearbyRabbit = GetAdjacentRabbit();
            if (nearbyRabbit != null) {
                print("nice");
                return K.NONE;
            } else {
                return tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_RABBIT));
            }
        } else if (mood == K.RUN) {
			// Don't make this one for now
		}
		return K.NONE;
	}

	// Update is called once per frame
	void Update() {
        if (Input.GetKeyDown("space")) {
            print("Finding path to closest plant:");
            print(FindPath(K.EAT));
        }
        try {
            
        } catch (System.Exception e) {
            print("EXCEPTION!!!");
            print(e.ToString());
        }
        
    }
}