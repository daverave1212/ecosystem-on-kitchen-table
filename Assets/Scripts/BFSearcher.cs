using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSearcher : MonoBehaviour {
    
    // Usually, finds a path to the given thing and returns the first tile that the animal should take to get there.
    // But, if 'onlyToMiddle' is given as true, then it returns the tile at the middle of the path to the objective.
    public static TileScript Find(TileScript startTile, int findWhat, bool onlyToMiddle = false, Action<AnimalScript> foundAnimalCallback = null, TileScript whichSpecificTile = null, Action<TileScript> middleTileCallback = null) {    // Returns the tile to move to to find a plant
        var visited = new HashSet<TileScript>();
        var toVisit = new Queue<TileScript>();
        var prevTile = new Dictionary<TileScript, TileScript>();

        Func<TileScript, bool> checkFindCondition;
        if (findWhat == K.FIND_PLANT)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasPlant(); };
        else if (findWhat == K.FIND_MATE_RABBIT) {
            print("Delegating correctly.");
            checkFindCondition = delegate(TileScript tileToCheck) {
                if (tileToCheck.HasRabbit()) print($"Tile {tileToCheck.row}, {tileToCheck.col} definitely has a rabbit");
                return tileToCheck.HasRabbit() && tileToCheck.animalOn.IsReadyToMate();
            };
        } else if (findWhat == K.FIND_MATE_FOX)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasFox() && tileToCheck.animalOn.IsReadyToMate(); };
        else if (findWhat == K.FIND_FOOD_RABBIT)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasFox(); };
        else if (findWhat == K.FIND_SPECIFIC_TILE) {
            if (whichSpecificTile == null) {
                print("ERROR: No specific tile given to BFSearcher.Find");
                return null;
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
                if (currentTile == null || GetPrevTile(currentTile) == null) {
                    print("ERROR: This should not have happened!");
                    return null;
                } else {
                    currentTile._MarkYellow();
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
                    print("Null. No more...");
                    return counter;
                } else if (GetPrevTile(currentTile) == null) {
                    print("Prev null. No more...");
                    return counter;
                } else {
                    if (GetPrevTile(currentTile) == startTile) return counter;
                    currentTile._MarkYellow();
                    currentTile = GetPrevTile(currentTile);
                    counter ++;
                }
            }
        }


        print("Starting at blue.");
        startTile._MarkBlue();
        visited.Add(startTile);
        toVisit.Enqueue(startTile);
        while (toVisit.Count > 0) {
            var currentTile = toVisit.Dequeue();
            var adjacentTiles = currentTile.GetAdjacentTiles();
            foreach (var tile in adjacentTiles) {
                if (IsVisited(tile)) continue;
                SetPrevTile(tile, currentTile);
                if (checkFindCondition(tile)) {
                    print("Found tile which marks condition: red");
                    tile._MarkRed();
                    if (!onlyToMiddle) {
                        if (tile.HasAnimal() && foundAnimalCallback != null) {
                            print("Calling callback");
                            foundAnimalCallback(tile.animalOn);
                        }
                        return TraceBack(currentTile);
                    } else {
                        print("Going to middle.");  
                        var animalTile = tile;
                        foundAnimalCallback(animalTile.animalOn);
                        var nTilesInPath = CountBack(tile);
                        print($"Found {nTilesInPath} in the path.");
                        return TraceBack(animalTile, nTilesInPath / 2);
                    }
                }
                if (tile.IsOccupied()) continue;                
                toVisit.Enqueue(tile);
                visited.Add(tile);
            }
        }
        print("Nothing found... weird");
        return null;
    }

}
