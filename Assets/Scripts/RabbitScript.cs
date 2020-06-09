using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

    public int _id = 0;
    public bool _printsTick = false;

	void Start() {
		name = "Bunny";
		GetComponent<Animator>().Play("Idle");
        InvokeRepeating("Tick", speed, speed);
	}

    public void Tick() {
        if (currentHunger <= 0) {
            Kill();
        }
        var mood = GetMood();
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
        var nearbyRabbit = GetAdjacentRabbit();
        if (nearbyRabbit != null && nearbyRabbit == myMate) {
            LookAtAnimal(myMate);
            SpawnLoveParticles();
            if (makesFirstStep) {
                var babyRabbitTile = GetRandomAvailableAdjacentTile();
                if (babyRabbitTile != null) {
                    MakeBabyWithMyMateAndClear(babyRabbitTile);
                    return K.NONE;
                }
            }
            return K.NONE;
        } else {
            if (isMeetingMate) {
                var directionToMoveTo = tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_SPECIFIC_TILE, whichSpecificTile: mateMeetingTile));
                if (directionToMoveTo == K.NONE && makesFirstStep) {
                    return tileOn.GetDirectionToAdjacentTile(mateMeetingTile);
                } else {
                    return directionToMoveTo;
                }
            } else {
                myMate = null;
                mateMeetingTile = BFSearcher.Find(startTile: tileOn, findWhat: K.FIND_MATE_RABBIT, onlyToMiddle: true, foundAnimalCallback: delegate(AnimalScript a) {
                    myMate = a;
                });
                if (mateMeetingTile == null)
                    return GetRandomAvailableDirection();
                isMeetingMate = true;
                makesFirstStep = true;
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

}