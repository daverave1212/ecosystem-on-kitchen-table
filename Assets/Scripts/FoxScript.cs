using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : AnimalScript
{

    void Start() {
        speed = 1.25f;
        mateFindType = K.FIND_MATE_FOX;
		Initialize("Fox");
	}

    protected override AnimalScript SpawnBaby(TileScript spawnBabyOnWhichTile) {
        return Spawner.SpawnFox(spawnBabyOnWhichTile, new[] {(FoxScript)this, (FoxScript)myMate});
    }

    protected override AnimalScript GetAdjacentMate() {
        return GetAdjacentAnimal("Fox");
    }

    public override void Destruct() {
        PlaneScript.foxes.Remove((FoxScript) this);
    }

    int TryEat() {
        if (EatAdjacentAndDiagonalAnimalIfNear("Rabbit")) {
            return K.NONE;
        } else {
            var tileToGo = BFSearcher.Find(tileOn, K.FIND_FOOD_RABBIT);
            var directionToGo = tileOn.GetDirectionToAdjacentTile(tileToGo);
            print($"Fox: going to direction {K.directionToString[directionToGo]}");
            return directionToGo;
        }
    }

    public override int FindPath(int mood) {
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
