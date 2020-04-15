using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript: AnimalScript {

	void Start() {
		name = "Bunny";
		GetComponent < Animator > ().Play("Idle");
		//Invoke("MoveToSomewhere", 1.5f);
	}

	void MoveToSomewhere() {
		print(gameObject.transform.position);
		var toPos = gameObject.transform.position + new Vector3(4, 0, 3);
		MoveToPosition(toPos);
	}

	// TO DO: Return the direction to move to

	int calculeazaDistanta(int liniePlanta, int colPlanta, int linieIepure, int colIepure) {
		int distanta = 0;
		while (liniePlanta != linieIepure && colPlanta != colIepure) {
			distanta++;
			if (liniePlanta - linieIepure < 0) {
				if (! (PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent < TileScript > ().IsOccupied())) {
					linieIepure = linieIepure - 1;
					continue;
				} else {
					if (colPlanta - colIepure < 0) {
						if (! (PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent < TileScript > ().IsOccupied())) {
							colIepure = colIepure - 1;
							continue;
						} else {
							if (! (PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent < TileScript > ().IsOccupied())) {
								colIepure = colIepure + 1;
								continue;
							}
						}
					}
				}
				linieIepure = linieIepure + 1;
				continue;
			}
			else if (liniePlanta - linieIepure > 0) {
				if (! (PlaneScript.tiles[linieIepure + 1, colIepure].GetComponent < TileScript > ().IsOccupied())) {
					linieIepure = linieIepure + 1;
					continue;
				}
				else {
					if (colPlanta - colIepure < 0) {
						if (! (PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent < TileScript > ().IsOccupied())) {
							colIepure = colIepure - 1;
							continue;
						}
						else {
							if (! (PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent < TileScript > ().IsOccupied())) {
								colIepure = colIepure + 1;
								continue;
							}
						}
					}
				}
				linieIepure = linieIepure - 1;
				continue;
			}
			else {
				if (! (PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent < TileScript > ().IsOccupied())) {
					colIepure = colIepure - 1;
					continue;
				} else {
					if (! (PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent < TileScript > ().IsOccupied())) {
						colIepure = colIepure + 1;
						continue;
					}
				}
				if (! (PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent < TileScript > ().IsOccupied())) {
					linieIepure = linieIepure - 1;
					continue;
				}
				linieIepure = linieIepure + 1;
				continue;

			}

		}
		return distanta;
	}


	int spuneDrumul(int liniePlanta, int colPlanta, int linieIepure, int colIepure) {

		if (liniePlanta - linieIepure < 0) {
			if (! (PlaneScript.tiles[liniePlanta + 1, colPlanta].GetComponent < TileScript > ().IsOccupied())) {
				spuneDrumul(liniePlanta + 1, colPlanta, linieIepure, colIepure);
				print("UP");
				return UP;
			} else {
				if (colPlanta - colIepure < 0) {
					if (! (PlaneScript.tiles[liniePlanta, colPlanta + 1].GetComponent < TileScript > ().IsOccupied())) {
						spuneDrumul(liniePlanta, colPlanta + 1, linieIepure, colIepure);
						print("LEFT");
						return LEFT;
					} else {
						if (! (PlaneScript.tiles[liniePlanta, colPlanta - 1].GetComponent < TileScript > ().IsOccupied())) {
							spuneDrumul(liniePlanta, colPlanta - 1, linieIepure, colIepure);
							print("RIGHT");
							return RIGHT;
						}
					}
				}
			}
			spuneDrumul(liniePlanta - 1, colPlanta, linieIepure, colIepure);
			print("DOWN");
			return DOWN;
		}
		else if (liniePlanta - linieIepure > 0) {
			if (! (PlaneScript.tiles[liniePlanta - 1, colPlanta].GetComponent < TileScript > ().IsOccupied())) {
				spuneDrumul(liniePlanta - 1, colPlanta, linieIepure, colIepure);
				print("DOWN");
				return DOWN;
			}
			else {
				if (colPlanta - colIepure < 0) {
					if (! (PlaneScript.tiles[liniePlanta, colPlanta + 1].GetComponent < TileScript > ().IsOccupied())) {
						spuneDrumul(liniePlanta, colPlanta + 1, linieIepure, colIepure);
						print("LEFT");
						return LEFT;
					}
					else {
						if (! (PlaneScript.tiles[liniePlanta, colPlanta - 1].GetComponent < TileScript > ().IsOccupied())) {
							spuneDrumul(liniePlanta, colPlanta - 1, linieIepure, colIepure);
							print("RIGTH");
							return RIGHT;
						}
					}
				}
			}
			spuneDrumul(liniePlanta - 1, colPlanta, linieIepure, colIepure);
			print("UP");
			return UP;
		} else {
			if (! (PlaneScript.tiles[liniePlanta, colPlanta - 1].GetComponent < TileScript > ().IsOccupied())) {
				spuneDrumul(liniePlanta, colPlanta - 1, linieIepure, colIepure);
				print("RIGHT");
				return RIGHT;
			} else {
				if (! (PlaneScript.tiles[liniePlanta, colPlanta + 1].GetComponent < TileScript > ().IsOccupied())) {
					spuneDrumul(liniePlanta + 1, colPlanta, linieIepure, colIepure);
					print("LEFT");
					return LEFT;
				}
			}
			if (! (PlaneScript.tiles[liniePlanta + 1, colPlanta].GetComponent < TileScript > ().IsOccupied())) {
				spuneDrumul(liniePlanta + 1, colPlanta, linieIepure, colIepure);
				print("UP");
				return UP;
			}
			spuneDrumul(liniePlanta - 1, colPlanta, linieIepure, colIepure);
			print("DOWN");
			return DOWN;

		}

	}
	int closestPlant(int linie, int col) {
		/*if(PlaneScript.self.tiles[linie+1,col].GetComponent<TileScript>().HasPlant())
        {
            return UP;

        }
        if (PlaneScript.self.tiles[linie , col+1].GetComponent<TileScript>().HasPlant())
        {
            return RIGHT;

        }
        if (PlaneScript.self.tiles[linie , col-1].GetComponent<TileScript>().HasPlant())
        {
            return LEFT;
        }

        if (PlaneScript.self.tiles[linie - 1, col].GetComponent<TileScript>().HasPlant())
        {
            return DOWN;

        }*/
		int min = 1000;
		int distantacurenta;
		int pozLinie = 0;
		int pozCol = 0;
		for (int i = 1; i <= 32; i++)
		    for (int j = 1; j <= 32; j++) {
			    if (PlaneScript.tiles[i, j].GetComponent < TileScript > ().HasPlant()) {
				    distantacurenta = calculeazaDistanta(i, j, linie, col);
				    if (min > distantacurenta) {
					    pozLinie = i;
					    pozCol = j;
					    min = distantacurenta;
				    }
			    }
		    }
		return spuneDrumul(pozLinie, pozCol, linie, col);
	}

	int FindPath(int mood) {
		if (mood == NO_MOOD) {
			return NONE;
		} else if (mood == EAT) {
			// Find path to closest plant
			// return the direction to go to (UP, DOWN, LEFT, RIGHT)
			return closestPlant(tileOn.row, tileOn.col);
		} else if (mood == MATE) {
			// Find path to closest rabbit who is also ready to mate
		} else if (mood == RUN) {
			// Don't make this one for now
		}
		return NONE;
	}

	// Update is called once per frame
	void Update() {
        try {
            if (Input.GetKeyDown("space")) {
                print("Finding path to closest plant:");
                print(FindPath(EAT));
            }
        } catch (System.Exception e) {
            print("EXCEPTION!!!");
            print(e.ToString());
        }
        
    }
}