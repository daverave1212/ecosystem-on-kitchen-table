﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    private void RandomGridGenerator(ref string[,] gameMap, int gridSize, int localGridSize, int x, int y, int w, Random rnd) {
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
                    gameMap[i,j] = "water";
            }
    }

    /*
    This is the function that you call to generate the random grid; returns a string two-dimensional array that has the values "grass" and "water";
    gridSize is the size of the game grid;
    maxLakeSize represents the number of squares around the lake center that may be changed to "water"; each square within the mini-grid is given a score between 0 and 99;
    maxNumberOfLakes represents the number of lakes to be generated; some lakes may overlap, creating larger lakes;
    w is the minimum weigth needed for a square to become water, providing grater control on lake generation; w needs to be between 0 and 99 (smaller w for larger lakes).
    */
    public string[,] Generate(int gridSize, int maxLakeSize, int maxNumberOfLakes, int w) {
        Random rnd = new Random();
        string [,] gameMap = new string [gridSize, gridSize];
        // Initialize the grid
        for(int i = 0; i < gridSize; i++)
            for(int j = 0; j < gridSize; j++)
                gameMap[i,j] = "grass";
        for(int i = 0; i <= maxNumberOfLakes; i++) {
            // Generate lake center positions
            int x = rnd.Next(0, gridSize-1);
            int y = rnd.Next(0, gridSize-1);
            // Generate random lake mini-grid around it
            RandomGridGenerator(ref gameMap, gridSize, maxLakeSize, x, y, w, rnd);
        }
        return gameMap;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
