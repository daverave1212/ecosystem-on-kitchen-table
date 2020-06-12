using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

    public int _id = 0;
    public bool _printsTick = false;

	void Start() {
        mateFindType = K.FIND_MATE_RABBIT;
		Initialize("Bunny");
	}

    int TryEat() {
        if (EatAdjacentPlantIfNear()) {
            return K.NONE;
        } else {
            return tileOn.GetDirectionToAdjacentTile(BFSearcher.Find(tileOn, K.FIND_PLANT));
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



    public override int FindPath(int mood) {
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