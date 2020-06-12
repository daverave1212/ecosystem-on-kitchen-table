using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : AnimalScript
{

    void Start() {
        speed = 0.95f;
        mateFindType = BFSearcher.FindWhat.MateFox;
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

    Direction TryEat() {
        if (EatAdjacentAndDiagonalAnimalIfNear("Bunny")) {
            return Direction.None;
        } else {
            var tileToGo = BFSearcher.Find(tileOn, BFSearcher.FindWhat.FoodRabbit);
            var directionToGo = tileOn.GetDirectionToAdjacentTile(tileToGo);
            return directionToGo;
        }
    }

    public override Direction FindPath(Mood mood) {
		if (mood == Mood.None) {
			return GetRandomAvailableDirection();
		} else if (mood == Mood.Eat) {
            return TryEat();
		} else if (mood == Mood.Mate) {
            return TryMate();
        }
		return GetRandomAvailableDirection();
	}

}
