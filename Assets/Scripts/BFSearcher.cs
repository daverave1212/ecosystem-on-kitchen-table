using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSearcher : MonoBehaviour {
    
    public static TileScript Find(TileScript startTile, int findWhat) {    // Returns the tile to move to to find a plant
        var visited = new HashSet<TileScript>();
        var toVisit = new Queue<TileScript>();
        var prevTile = new Dictionary<TileScript, TileScript>();

        System.Func<TileScript, bool> checkFindCondition;
        if (findWhat == K.FIND_PLANT)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasPlant(); };
        else if (findWhat == K.FIND_RABBIT)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasRabbit(); };
        else if (findWhat == K.FIND_FOX)
            checkFindCondition = delegate(TileScript tileToCheck) { return tileToCheck.HasFox(); };
        else
            throw new System.Exception($"Error! findWhat not given a correct argument! (was given '{findWhat}')");

        bool IsVisited(TileScript tile) { return visited.Contains(tile); }
        void SetPrevTile(TileScript forTile, TileScript tile) {
            // print($"    Setting prev tile for ({forTile.row}, {forTile.col}) to ({tile.row}, {tile.col})");
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

        TileScript TraceBack(TileScript fromTile) {
            var currentTile = fromTile;
            while (true) {
                if (currentTile == null || GetPrevTile(currentTile) == null) {
                    print("ERROR: This should not have happened!");
                    return null;
                } else {
                    if (GetPrevTile(currentTile) == startTile) return currentTile;
                    currentTile = GetPrevTile(currentTile);
                }
            }
        }

        toVisit.Enqueue(startTile);
        while (toVisit.Count > 0) {
            var currentTile = toVisit.Dequeue();
            var adjacentTiles = currentTile.GetAdjacentTiles();
            foreach (var tile in adjacentTiles) {
                if (IsVisited(tile)) continue;
                if (checkFindCondition(tile)) {
                    print("Hah! Caught one!");
                    return TraceBack(currentTile);
                }
                if (tile.IsOccupied()) continue;
                SetPrevTile(tile, currentTile);
                toVisit.Enqueue(tile);
                visited.Add(tile);
            }
        }
        print("Nothing found... weird");
        return null;
    }

}
