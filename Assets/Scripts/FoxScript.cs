using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : AnimalScript
{
    void Start() {
		name = "Fox";
        speed = 0.95f;
		GetComponent<Animator>().Play("Idle");
        InvokeRepeating("Tick", speed, speed);
	}

    public void Tick() {
        var mood = GetMood();
        var direction = FindPath(mood);
        MoveInDirection(direction);
    }

    int TryEat() {
        if (EatAdjacentAndDiagonalRabbitIfNear()) {
            return K.NONE;
        } else {
            var tileToGo = BFSearcher.Find(tileOn, K.FIND_FOOD_RABBIT);
            var directionToGo = tileOn.GetDirectionToAdjacentTile(tileToGo);
            print($"Fox: going to direction {K.directionToString[directionToGo]}");
            return directionToGo;
        }
    }


    int TryMate() {
        var nearbyFox = GetAdjacentFox();
        if (nearbyFox != null && nearbyFox == myMate) {
            LookAtAnimal(myMate);
            SpawnLoveParticles();
            if (makesFirstStep) {
                var babyFoxTile = GetRandomAvailableAdjacentTile();
                if (babyFoxTile != null) {
                    MakeBabyWithMyMateAndClear(babyFoxTile);
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
                mateMeetingTile = BFSearcher.Find(startTile: tileOn, findWhat: K.FIND_MATE_FOX, onlyToMiddle: true, foundAnimalCallback: delegate(AnimalScript a) {
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
        }
		return GetRandomAvailableDirection();
	}

}
