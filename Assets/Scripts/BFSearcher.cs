using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSearcher : MonoBehaviour {

    public enum FindWhat {
        MateRabbit,
        FoodRabbit,
        MateFox,
        Plant,
        SpecificTile
    }

    
    // Usually, finds a path to the given thing and returns the first tile that the animal should take to get there.
    // But, if 'onlyToMiddle' is given as true, then it returns the tile at the middle of the path to the objective.
    public static TileScript Find(TileScript startTile, FindWhat findWhat, bool onlyToMiddle = false, Action<AnimalScript> foundAnimalCallback = null, TileScript whichSpecificTile = null, Action<TileScript> middleTileCallback = null) {    // Returns the tile to move to to find a plant
        var visited = new HashSet<TileScript>();
        var toVisit = new Queue<TileScript>();
        var prevTile = new Dictionary<TileScript, TileScript>();

        Func<TileScript, bool> checkFindCondition;
        if (findWhat == FindWhat.Plant)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasPlant(); };
        else if (findWhat == FindWhat.MateRabbit) {
            checkFindCondition = delegate(TileScript tileToCheck) {
                return tileToCheck.HasAnimal("Bunny") && tileToCheck.animalOn.IsReadyToMate();
            };
        } else if (findWhat == FindWhat.MateFox)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasAnimal("Fox") && tileToCheck.animalOn.IsReadyToMate(); };
        else if (findWhat == FindWhat.FoodRabbit)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasAnimal("Bunny"); };
        else if (findWhat == FindWhat.SpecificTile) {
            if (whichSpecificTile == null) {
                throw new Exception("ERROR: No specific tile given to BFSearcher.Find");
            }
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck == whichSpecificTile; };
        } else
            throw new System.Exception($"Error! findWhat not given a correct argument! (was given '{findWhat}')");

        bool IsVisited(TileScript tile) { return visited.Contains(tile); }
        void SetPrevTile(TileScript forTile, TileScript tile) {
            if (prevTile.ContainsKey(forTile)) {
                prevTile[forTile] = tile;
            } else {
                prevTile.Add(forTile, tile); 
            }
        }
        TileScript GetPrevTile(TileScript tile) {
            if (prevTile.ContainsKey(tile)) {
                return prevTile[tile];
            } else {
                return null;
            }
        }

        TileScript TraceBack(TileScript fromTile, int tileLimit = 9999) {
            var currentTile = fromTile;
            int counter = 0;
            while (true) {
                if (currentTile == null) {
                    throw new Exception("This should not have happened!");
                }
                if (GetPrevTile(currentTile) == null) {
                    return currentTile;
                } else {
                    //currentTile._MarkYellow();
                    if (counter >= tileLimit) return currentTile;
                    if (GetPrevTile(currentTile) == startTile) return currentTile;
                    currentTile = GetPrevTile(currentTile);
                    counter ++;
                }
            }
        }

        int CountBack(TileScript fromTile) {
            var currentTile = fromTile;
            int counter = 0;
            while (true) {
                if (currentTile == null) {
                    return counter;
                } else if (GetPrevTile(currentTile) == null) {
                    return counter;
                } else {
                    if (GetPrevTile(currentTile) == startTile) return counter;
                    //currentTile._MarkYellow();
                    currentTile = GetPrevTile(currentTile);
                    counter ++;
                }
            }
        }

        visited.Add(startTile);
        toVisit.Enqueue(startTile);
        while (toVisit.Count > 0) {
            var currentTile = toVisit.Dequeue();
            var adjacentTiles = currentTile.GetAdjacentTiles();
            foreach (var tile in adjacentTiles) {
                if (IsVisited(tile)) continue;
                SetPrevTile(tile, currentTile);
                if (checkFindCondition(tile)) {
                    if (!onlyToMiddle) {
                        if (tile.HasAnimal() && foundAnimalCallback != null) {
                            foundAnimalCallback(tile.animalOn);
                        }
                        return TraceBack(currentTile);
                    } else {
                        var animalTile = tile;
                        foundAnimalCallback(animalTile.animalOn);
                        var nTilesInPath = CountBack(tile);
                        return TraceBack(animalTile, nTilesInPath / 2);
                    }
                }
                if (tile.IsOccupied()) continue;                
                toVisit.Enqueue(tile);
                visited.Add(tile);
            }
        }
        return null;
    }

}
