using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    private void RandomGridGenerator(ref TileScript.TileType[,] gameMap, int gridSize, int localGridSize, int x, int y, int w, System.Random rnd) {
        // Check if lake falls outside of grid and create start and end positions for x and y
        int xStart = Math.Max(x - localGridSize, 0);
        int yStart = Math.Max(y - localGridSize, 0);
        int xEnd = Math.Min(x + localGridSize, gridSize-1);
        int yEnd = Math.Min(y + localGridSize, gridSize-1);

        int maxDistance = localGridSize * 2;
        for(int i = xStart; i <= xEnd; i++)
            for(int j = yStart; j <= yEnd; j++) {
                // To minimize dispersion, get a proximity to center score for the square
                int distance = Math.Abs(i - x) + Math.Abs(j - y);
                float score = 1 - (distance / maxDistance);
                // For each square inside the mini-grid generate a random weigth and compare that to w
                if(score * (rnd.Next(0, 99)) >= w)
                    gameMap[i,j] = TileScript.TileType.Water;
            }
    }

    private void addSand(ref TileScript.TileType[,] gameMap, int gridSize) {
        for(int i = 0; i < gridSize; i++)
            for(int j = 0; j < gridSize; j++) {
                if(gameMap[i,j] == TileScript.TileType.Grass) {
                    if(i > 0 && gameMap[i-1,j] == TileScript.TileType.Water)
                        gameMap[i,j] = TileScript.TileType.Sand;
                    if(j > 0 && gameMap[i,j-1] == TileScript.TileType.Water)
                        gameMap[i,j] = TileScript.TileType.Sand;
                    if(i < gridSize-1 && gameMap[i+1,j] == TileScript.TileType.Water)
                        gameMap[i,j] = TileScript.TileType.Sand;
                    if(j < gridSize-1 && gameMap[i,j+1] == TileScript.TileType.Water)
                        gameMap[i,j] = TileScript.TileType.Sand;
                }
            }
    }

    private void addTrees(ref TileScript.TileType[,] gameMap, int gridSize, int numberOfTrees, System.Random rnd) {
        int n = 0;
        List<int> xPoz = new List<int>();
        List<int> yPoz = new List<int>();
        // Mark all possible tree positions
        for(int i = 0; i < gridSize; i++)
            for(int j = 0; j < gridSize; j++)
                if(gameMap[i,j] == TileScript.TileType.Grass) {
                    xPoz.Add(i);
                    yPoz.Add(j);
                    n++;
                }
        // If more trees are reqested than available space, shrink tree size to maximum allowed
        if(n < numberOfTrees)
            numberOfTrees = n;
        // Choose random values for tree positions than pop them from the list
        for(int i = 0; i < numberOfTrees; i++) {
            int k = rnd.Next(0, n-1);
            gameMap[xPoz[k],yPoz[k]] = TileScript.TileType.Tree;
            xPoz.RemoveAt(k);
            yPoz.RemoveAt(k);
            n = n-1;
        }
    }

    private void removeSandFromWater(ref TileScript.TileType[,] gameMap, int gridSize)
    {
        int j = 0;
        int begin = 0;
        int last = 0;
        bool isBigger = false;
		while(j < gridSize) {
			for(int i = 0; i < gridSize; i++)
            {
            	if (gameMap[i, j] == TileScript.TileType.Sand && isBigger == false)
            	{
                	begin = i;
                	isBigger = true;
                    continue;
                }
				if (gameMap[i, j] == TileScript.TileType.Sand)
            	{
                	last = i;
            	}
        	}
            
            for (int l = begin + 1; l < last; l++)
            {
                gameMap[l, j] = TileScript.TileType.Water;
            }
			//print("Value of begin: " + begin);
            //print("Value of last: " + last);
			j++;
			begin = 0;
			last = 0;	
			isBigger = false;
		}
    }

    private void addSandNearWater(ref TileScript.TileType[,] gameMap, int gridSize)
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (i < gridSize - 1 && j < gridSize - 1)
                {
                    if ((gameMap[i, j] == TileScript.TileType.Grass && gameMap[i + 1, j] == TileScript.TileType.Water) ||
                        (gameMap[i, j] == TileScript.TileType.Grass && gameMap[i, j + 1] == TileScript.TileType.Water))
                    {
                        gameMap[i, j] = TileScript.TileType.Sand;
                    }
                    
                    else if ((gameMap[i, j] == TileScript.TileType.Water && gameMap[i + 1, j] == TileScript.TileType.Grass) ||
                        (gameMap[i, j] == TileScript.TileType.Water && gameMap[i, j + 1] == TileScript.TileType.Grass))
                    {
                        gameMap[i, j] = TileScript.TileType.Sand;
                    }
                } 
            }
        }
    }

    /*
    This is the function that you call to generate the random grid; returns a string two-dimensional array that has the values "grass", "water" and "sand";
    gridSize is the size of the game grid;
    maxLakeSize represents the number of squares around the lake center that may be changed to "water"; each square within the mini-grid is given a score between 0 and 99;
    maxNumberOfLakes represents the number of lakes to be generated; some lakes may overlap, creating larger lakes;
    w is the minimum weigth needed for a square to become water, providing grater control on lake generation; w needs to be between 0 and 99 (smaller w for larger lakes);
    numberOfTrees is the number of trees to be added; if there isn't enough space, all possible space is filled and the two dimensional array is returned as is.
    */
    public TileScript.TileType[,] Generate(int gridSize, int maxLakeSize, int maxNumberOfLakes, int w, int numberOfTrees) {
        System.Random rnd = new System.Random();
        TileScript.TileType [,] gameMap = new TileScript.TileType [gridSize, gridSize];
        // Initialize the grid
        for(int i = 0; i < gridSize; i++)
            for(int j = 0; j < gridSize; j++)
                gameMap[i,j] = TileScript.TileType.Grass;
        for(int i = 0; i <= maxNumberOfLakes; i++) {
            // Generate lake center positions
            int x = rnd.Next(0, gridSize-1);
            int y = rnd.Next(0, gridSize-1);
            // Generate random lake mini-grid around it
            RandomGridGenerator(ref gameMap, gridSize, maxLakeSize, x, y, w, rnd);
        }
        addSand(ref gameMap, gridSize);
        addTrees(ref gameMap, gridSize, numberOfTrees, rnd);
        removeSandFromWater(ref gameMap, gridSize);
        addSandNearWater(ref gameMap, gridSize);
        return gameMap;
    }
}
