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

	int calculeazaDistanta(int liniePlanta, int colPlanta, int linieIepure, int colIepure,int ultimaMutare) {
		int distanta = 0;
		while (liniePlanta != linieIepure || colPlanta != colIepure) {
            if ((liniePlanta == linieIepure + 1 && colPlanta == colIepure) || (liniePlanta == linieIepure - 1 && colPlanta == colIepure) || (liniePlanta == linieIepure && colPlanta == colIepure + 1) || (liniePlanta == linieIepure && colPlanta == colIepure - 1))
            {
                break;
            }
            distanta++;
            if (distanta == 300)
                return distanta;
			if (liniePlanta - linieIepure < 0) {
                if (linieIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied()))&&ultimaMutare != UP)
                    {
                        linieIepure = linieIepure - 1;

                        continue;
                    }
                }
                else
                {
                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != RIGHT)
                            {
                                colIepure = colIepure - 1;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                                {
                                    colIepure = colIepure + 1;
                                    continue;
                                }
                            }
                        }
                    }
                }
				linieIepure = linieIepure + 1;
				continue;
			}
			else if (liniePlanta - linieIepure > 0) {
                if (linieIepure < 31)
                {
                    if ((!(PlaneScript.tiles[linieIepure + 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != DOWN)
                    {
                        linieIepure = linieIepure + 1;
                        continue;
                    }
                }
                else
                {
                    
                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                            {
                                colIepure = colIepure - 1;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != DOWN)
                                {
                                    colIepure = colIepure + 1;
                                    continue;
                                }
                            }
                        }
                    }
                }
				linieIepure = linieIepure - 1;
				continue;
			}
			else {
                if (colIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                    {
                        colIepure = colIepure - 1;
                        continue;
                    }
                }
                else
                {
                    if (colIepure < 31)
                    {
                        if (!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied()) && ultimaMutare != DOWN)
                        {
                            colIepure = colIepure + 1;
                            continue;
                        }
                    }
                }
                if (linieIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != UP)
                    {
                        linieIepure = linieIepure - 1;
                        continue;
                    }
                }
				linieIepure = linieIepure + 1;
				continue;

			}

		}
		return distanta;
	}

    int spuneDrumul(int liniePlanta, int colPlanta, int linieIepure, int colIepure, int[] pozitii, int ultimaMutare) {
        int pozitie = -1;
      
        while (liniePlanta != linieIepure || colPlanta != colIepure)
        {
            if((liniePlanta == linieIepure+1 && colPlanta == colIepure)|| (liniePlanta == linieIepure - 1 && colPlanta == colIepure)|| (liniePlanta == linieIepure  && colPlanta == colIepure + 1)|| (liniePlanta == linieIepure  && colPlanta == colIepure - 1))
            {
                break;
            }
          
            pozitie++;
            //if (distanta == 300)
             //   return distanta;
            if (liniePlanta - linieIepure < 0)
            {
                if (linieIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied()))&&ultimaMutare!= UP)
                    {
                        linieIepure = linieIepure - 1;
                        print("DOWN");
                        pozitii[pozitie] = DOWN;
                        ultimaMutare = DOWN;
                        continue;
                    }
                }
                else
                {
                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied()))&& ultimaMutare != RIGHT)
                            {
                                colIepure = colIepure - 1;
                                print("LEFT");
                                pozitii[pozitie] = LEFT;
                                ultimaMutare = LEFT;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                                {
                                    colIepure = colIepure + 1;
                                    print("RIGTH");
                                    pozitii[pozitie] = RIGHT;
                                    ultimaMutare = RIGHT;
                                    continue;
                                }
                            }
                        }
                    }
                }
                linieIepure = linieIepure + 1;
                print("UP");
                pozitii[pozitie] = UP;
                ultimaMutare = UP;

                continue;
            }
            else if (liniePlanta - linieIepure > 0)
            {
                if (linieIepure < 31)
                {
                    if ((!(PlaneScript.tiles[linieIepure + 1, colIepure].GetComponent<TileScript>().IsOccupied()))&&ultimaMutare != DOWN)
                    {
                        linieIepure = linieIepure + 1;
                        print("UP");
                        pozitii[pozitie] = UP;
                        ultimaMutare = UP;

                        continue;
                    }
                }
                else
                {

                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != RIGHT)
                            {
                                colIepure = colIepure - 1;
                                print("LEFT");
                                pozitii[pozitie] = LEFT;
                                ultimaMutare = LEFT;

                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                                {
                                    colIepure = colIepure + 1;
                                    print("RIGTH");
                                    pozitii[pozitie] = RIGHT;
                                    ultimaMutare = RIGHT;

                                    continue;
                                }
                            }
                        }
                    }
                }
                linieIepure = linieIepure - 1;
                print("DOWN");
                pozitii[pozitie] = DOWN;
                ultimaMutare = DOWN;

                continue;
            }
            else
            {
                if (colPlanta - colIepure < 0)
                {
                    if (colIepure > 0)
                    {
                        if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != RIGHT)
                        {
                            colIepure = colIepure - 1;
                            print("LEFT");
                            pozitii[pozitie] = LEFT;
                            ultimaMutare = LEFT;

                            continue;
                        }
                    }
                }
                else
                {
                    if (colIepure < 31)
                    {
                        if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != LEFT)
                        {
                            colIepure = colIepure + 1;
                            print("RIGTH");
                            pozitii[pozitie] = RIGHT;
                            ultimaMutare = RIGHT;

                            continue;
                        }
                    }
                }
                if (linieIepure > 0)
                {
                    if (!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied()))
                    {
                        linieIepure = linieIepure - 1;
                        print("DOWN");
                        pozitii[pozitie] = DOWN;
                        ultimaMutare = DOWN;

                        continue;
                    }
                }
                linieIepure = linieIepure + 1;
               print("UP");
                pozitii[pozitie] = UP;
                ultimaMutare = UP;

                continue;

            }

        }

        return pozitie;

	}
   
	int closestPlant(int linie, int col) {
        print("INCEPE");
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
		for (int i = 0; i <= 31; i++)
		    for (int j = 0; j <= 31; j++) {
			    if (PlaneScript.tiles[i, j].GetComponent < TileScript > ().HasPlant()) {
				    distantacurenta = calculeazaDistanta(i, j, linie, col,0);
				    if (min > distantacurenta) {
					    pozLinie = i;
					    pozCol = j;
					    min = distantacurenta;
				    }
			    }
		    }
        print(pozLinie+" "+pozCol+ "POz iepure:"+linie+" "+col);
        int[] pozitii=new int[100];
        int pozitie;
	    pozitie=spuneDrumul(pozLinie, pozCol, linie, col , pozitii, 0);
        
        for(int i = 0; i <= pozitie; i++)
        {
            print(pozitii[i]);
        }
        return pozitii[0];
	}

    int closestRabbit(int linie, int col)
    {
        print("INCEPE");
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
        for (int i = 0; i <= 31; i++)
            for (int j = 0; j <= 31; j++)
            {
               if (PlaneScript.tiles[i, j].GetComponent<TileScript>().animalOn.name == "Bunny" && PlaneScript.tiles[i, j].GetComponent<TileScript>().animalOn.IsReadyToMate())
                {
                    distantacurenta = calculeazaDistanta(i, j, linie, col, 0);
                    if (min > distantacurenta)
                    {
                        pozLinie = i;
                        pozCol = j;
                        min = distantacurenta;
                    }
                }
            }
        print(pozLinie + " " + pozCol + "POz iepure:" + linie + " " + col);
        int[] pozitii = new int[100];
        int pozitie;
        pozitie = spuneDrumul(pozLinie, pozCol, linie, col, pozitii, 0);

        for (int i = 0; i <= pozitie; i++)
        {
            print(pozitii[i]);
        }
        return pozitii[0];
    }

    public void Tick() {
        var mood = GetMood();
        var direction = FindPath(mood);
        MoveInDirection(direction);
    }

    int FindPath(int mood) {
		if (mood == NO_MOOD) {
			return NONE;
		} else if (mood == EAT) {
			return closestPlant(tileOn.row, tileOn.col);
		} else if (mood == MATE) {
            return closestRabbit(tileOn.row, tileOn.col);
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