﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : AnimalScript
{
    void Start() {
        name = "Fox";
        GetComponent<Animator>().Play("Idle");
        Invoke("MoveToSomewhere", -1.5f);
    }

    void MoveToSomewhere() {
        var toPos = gameObject.transform.position + new Vector3(4, 0, 3);
        MoveToPosition(toPos);
    }
    int calculeazaDistanta(int liniePlanta, int colPlanta, int linieIepure, int colIepure, int ultimaMutare)
    {
        int distanta = 0;
        while (liniePlanta != linieIepure || colPlanta != colIepure)
        {
            if ((liniePlanta == linieIepure + 1 && colPlanta == colIepure) || (liniePlanta == linieIepure - 1 && colPlanta == colIepure) || (liniePlanta == linieIepure && colPlanta == colIepure + 1) || (liniePlanta == linieIepure && colPlanta == colIepure - 1))
            {
                break;
            }
            distanta++;
            if (distanta == 300)
                return distanta;
            if (liniePlanta - linieIepure < 0)
            {
                if (linieIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.UP)
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
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.RIGHT)
                            {
                                colIepure = colIepure - 1;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
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
            else if (liniePlanta - linieIepure > 0)
            {
                if (linieIepure < 31)
                {
                    if ((!(PlaneScript.tiles[linieIepure + 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.DOWN)
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
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
                            {
                                colIepure = colIepure - 1;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.DOWN)
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
            else
            {
                if (colIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
                    {
                        colIepure = colIepure - 1;
                        continue;
                    }
                }
                else
                {
                    if (colIepure < 31)
                    {
                        if (!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied()) && ultimaMutare != K.DOWN)
                        {
                            colIepure = colIepure + 1;
                            continue;
                        }
                    }
                }
                if (linieIepure > 0)
                {
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.UP)
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



    int spuneDrumul(int liniePlanta, int colPlanta, int linieIepure, int colIepure, int[] pozitii, int ultimaMutare)
    {
        int pozitie = -1;

        while (liniePlanta != linieIepure || colPlanta != colIepure)
        {
            if ((liniePlanta == linieIepure + 1 && colPlanta == colIepure) || (liniePlanta == linieIepure - 1 && colPlanta == colIepure) || (liniePlanta == linieIepure && colPlanta == colIepure + 1) || (liniePlanta == linieIepure && colPlanta == colIepure - 1))
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
                    if ((!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.UP)
                    {
                        linieIepure = linieIepure - 1;
                        print("K.DOWN");
                        pozitii[pozitie] = K.DOWN;
                        ultimaMutare = K.DOWN;
                        continue;
                    }
                }
                else
                {
                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.RIGHT)
                            {
                                colIepure = colIepure - 1;
                                print("K.LEFT");
                                pozitii[pozitie] = K.LEFT;
                                ultimaMutare = K.LEFT;
                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
                                {
                                    colIepure = colIepure + 1;
                                    print("RIGTH");
                                    pozitii[pozitie] = K.RIGHT;
                                    ultimaMutare = K.RIGHT;
                                    continue;
                                }
                            }
                        }
                    }
                }
                linieIepure = linieIepure + 1;
                print("K.UP");
                pozitii[pozitie] = K.UP;
                ultimaMutare = K.UP;

                continue;
            }
            else if (liniePlanta - linieIepure > 0)
            {
                if (linieIepure < 31)
                {
                    if ((!(PlaneScript.tiles[linieIepure + 1, colIepure].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.DOWN)
                    {
                        linieIepure = linieIepure + 1;
                        print("K.UP");
                        pozitii[pozitie] = K.UP;
                        ultimaMutare = K.UP;

                        continue;
                    }
                }
                else
                {

                    if (colPlanta - colIepure < 0)
                    {
                        if (colIepure > 0)
                        {
                            if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.RIGHT)
                            {
                                colIepure = colIepure - 1;
                                print("K.LEFT");
                                pozitii[pozitie] = K.LEFT;
                                ultimaMutare = K.LEFT;

                                continue;
                            }
                        }
                        else
                        {
                            if (colIepure < 31)
                            {
                                if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
                                {
                                    colIepure = colIepure + 1;
                                    print("RIGTH");
                                    pozitii[pozitie] = K.RIGHT;
                                    ultimaMutare = K.RIGHT;

                                    continue;
                                }
                            }
                        }
                    }
                }
                linieIepure = linieIepure - 1;
                print("K.DOWN");
                pozitii[pozitie] = K.DOWN;
                ultimaMutare = K.DOWN;

                continue;
            }
            else
            {
                if (colPlanta - colIepure < 0)
                {
                    if (colIepure > 0)
                    {
                        if ((!(PlaneScript.tiles[linieIepure, colIepure - 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.RIGHT)
                        {
                            colIepure = colIepure - 1;
                            print("K.LEFT");
                            pozitii[pozitie] = K.LEFT;
                            ultimaMutare = K.LEFT;

                            continue;
                        }
                    }
                }
                else
                {
                    if (colIepure < 31)
                    {
                        if ((!(PlaneScript.tiles[linieIepure, colIepure + 1].GetComponent<TileScript>().IsOccupied())) && ultimaMutare != K.LEFT)
                        {
                            colIepure = colIepure + 1;
                            print("RIGTH");
                            pozitii[pozitie] = K.RIGHT;
                            ultimaMutare = K.RIGHT;

                            continue;
                        }
                    }
                }
                if (linieIepure > 0)
                {
                    if (!(PlaneScript.tiles[linieIepure - 1, colIepure].GetComponent<TileScript>().IsOccupied()))
                    {
                        linieIepure = linieIepure - 1;
                        print("K.DOWN");
                        pozitii[pozitie] = K.DOWN;
                        ultimaMutare = K.DOWN;

                        continue;
                    }
                }
                linieIepure = linieIepure + 1;
                print("K.UP");
                pozitii[pozitie] = K.UP;
                ultimaMutare = K.UP;

                continue;

            }

        }

        return pozitie;

    }

    int closestRabbit(int linie, int col)
    {
        print("INCEPE");
        /*if(PlaneScript.self.tiles[linie+1,col].GetComponent<TileScript>().HasPlant())
        {
            return K.UP;

        }
        if (PlaneScript.self.tiles[linie , col+1].GetComponent<TileScript>().HasPlant())
        {
            return K.RIGHT;

        }
        if (PlaneScript.self.tiles[linie , col-1].GetComponent<TileScript>().HasPlant())
        {
            return K.LEFT;
        }

        if (PlaneScript.self.tiles[linie - 1, col].GetComponent<TileScript>().HasPlant())
        {
            return K.DOWN;

        }*/
        int min = 1000;
        int distantacurenta;
        int pozLinie = 0;
        int pozCol = 0;
        for (int i = 0; i <= 31; i++)
            for (int j = 0; j <= 31; j++)
            {
                if (PlaneScript.tiles[i, j].GetComponent<TileScript>().animalOn.name == "Bunny")
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
    int closestFox(int linie, int col)
    {
        print("INCEPE");
        /*if(PlaneScript.self.tiles[linie+1,col].GetComponent<TileScript>().HasPlant())
        {
            return K.UP;

        }
        if (PlaneScript.self.tiles[linie , col+1].GetComponent<TileScript>().HasPlant())
        {
            return K.RIGHT;

        }
        if (PlaneScript.self.tiles[linie , col-1].GetComponent<TileScript>().HasPlant())
        {
            return K.LEFT;
        }

        if (PlaneScript.self.tiles[linie - 1, col].GetComponent<TileScript>().HasPlant())
        {
            return K.DOWN;

        }*/
        int min = 1000;
        int distantacurenta;
        int pozLinie = 0;
        int pozCol = 0;
        for (int i = 0; i <= 31; i++)
            for (int j = 0; j <= 31; j++)
            {
                if (PlaneScript.tiles[i, j].GetComponent<TileScript>().animalOn.name == "Fox" && PlaneScript.tiles[i, j].GetComponent<TileScript>().animalOn.IsReadyToMate())
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

    // TO DO: Return the direction to move to
    int FindPath(int mood) {
        if (mood == K.NO_MOOD) {
            return K.NONE;
        } else if (mood == K.EAT) {
            // Find path to closest rabbit
            // return the direction to go to (K.UP, K.DOWN, K.LEFT, K.RIGHT)
            return closestRabbit(tileOn.row, tileOn.col);
        } else if (mood == K.MATE) {
            // Find path to closest fox who is also ready to mate
            return closestFox(tileOn.row, tileOn.col);
        } else if (mood == K.RUN) {
            // Don't make this one for now
        }
        return K.NONE;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
