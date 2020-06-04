using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

    public int _id = 0;

	void Start() {
		name = "Bunny";
		GetComponent<Animator>().Play("Idle");
        InvokeRepeating("Tick", speed, speed);
	}

    public void Tick() {
        var mood = GetMood();
        if (mood == K.EAT) {
            if (EatAdjacentPlantIfNear()) return;
        }
        var direction = FindPath(mood);
        MoveInDirection(direction);
    }

    int TryEat() {
        if (EatAdjacentPlantIfNear()) {
            return K.NONE;
        } else {
            return tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_PLANT));
        }
    }

    int TryMate() {
        print("Trying to mate.");
        var nearbyRabbit = GetAdjacentRabbit();
        if (nearbyRabbit != null && nearbyRabbit == myMate) {
            LookAtAnimal(myMate);
            var particles = Instantiate(PlaneScript.self.heartParticlesPrefab);    // It will autodestruct because it has a script, no worries
            particles.transform.position = new Vector3(
                (gameObject.transform.position.x + myMate.transform.position.x) / 2,
                (gameObject.transform.position.y + myMate.transform.position.y) / 2,
                (gameObject.transform.position.z + myMate.transform.position.z) / 2
            );
            if (makesFirstStep) {
                var babyRabbitTile = GetRandomAvailableAdjacentTile();
                if (babyRabbitTile != null) {
                    var ourBaby = PlaneScript.SpawnRabbit(babyRabbitTile, new[] {(RabbitScript)this, (RabbitScript)myMate});
                    ourBaby.CreateAsOffspring(this, myMate);
                    ourBaby.Mutate();
                    AddHunger(-50);
                    AddHappiness(-50);
                    myMate.AddHunger(-50);
                    myMate.AddHappiness(-50);
                    myMate.isMeetingMate = false;
                    myMate.mateMeetingTile = null;
                    myMate.makesFirstStep = false;
                    myMate.myMate = null;
                    isMeetingMate = false;
                    makesFirstStep = false;
                    mateMeetingTile = null;
                    myMate = null;
                    return K.NONE;
                }
            }
            return K.NONE;
        } else {
            if (isMeetingMate) {
                if (mateMeetingTile == null) {
                    print($"Hmm null mate meeting tile. Am I first steppa? {makesFirstStep}");
                } else {
                    print($"Mate meeting tile seems ok. First steppa: {makesFirstStep}");
                }
                var directionToMoveTo = tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_SPECIFIC_TILE, whichSpecificTile: mateMeetingTile));
                if (directionToMoveTo == K.NONE && makesFirstStep) {
                    return tileOn.GetDirectionToAdjacentTile(mateMeetingTile);
                } else {
                    return directionToMoveTo;
                }
            } else {
                myMate = null;            
                mateMeetingTile = BFSearcher.Find(startTile: tileOn, findWhat: K.FIND_MATE_RABBIT, onlyToMiddle: true, foundAnimalCallback: delegate(AnimalScript a) {
                    if (a == null) print("What the fuck?");
                    RabbitScript rabbitFound = (RabbitScript) a;
                    myMate = a;
                });
                if (mateMeetingTile == null) {
                    print("Found no mate :(");
                    return GetRandomAvailableDirection();
                } else {
                    print("Found a mate :D");
                }
                isMeetingMate = true;
                makesFirstStep = true;
                mateMeetingTile._MarkYellow();
                myMate.isMeetingMate = true;
                myMate.mateMeetingTile = mateMeetingTile;
                myMate.makesFirstStep = false;
                myMate.myMate = this;
                var directionToMoveTo = tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_SPECIFIC_TILE, whichSpecificTile: mateMeetingTile));
                return directionToMoveTo;
            }   
        }
    }

    int FindPath(int mood) {
		if (mood == K.NO_MOOD) {
			return GetRandomAvailableDirection();
		} else if (mood == K.EAT) {
            return TryEat();
		} else if (mood == K.MATE) {
            return TryMate();
        } else if (mood == K.RUN) {
			// Don't make this one for now
		}
		return GetRandomAvailableDirection();
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