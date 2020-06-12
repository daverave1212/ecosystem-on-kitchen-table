using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

    public int _id = 0;
    public bool _printsTick = false;

	void Start() {
        mateFindType = BFSearcher.FindWhat.MateRabbit;
		Initialize("Bunny");
	}

    Direction TryEat() {
        if (EatAdjacentPlantIfNear()) {
            return Direction.None;
        } else {
            return tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, BFSearcher.FindWhat.Plant));
        }
    }

    protected override AnimalScript SpawnBaby(TileScript spawnBabyOnWhichTile) {
        return Spawner.SpawnRabbit(spawnBabyOnWhichTile, new[] {(RabbitScript)this, (RabbitScript)myMate});
    }

    protected override AnimalScript GetAdjacentMate() {
        return GetAdjacentAnimal("Bunny");
    }

    public override void Destruct() {
        PlaneScript.rabbits.Remove((RabbitScript) this);
    }



    public override Direction FindPath(Mood mood) {
		if (mood == Mood.None) {
			return GetRandomAvailableDirection();
		} else if (mood == Mood.Eat) {
            return TryEat();
		} else if (mood == Mood.Mate) {
            return TryMate();
        } else if (mood == Mood.Run) {
			// Don't make this one for now
		}
		return GetRandomAvailableDirection();
	}

}