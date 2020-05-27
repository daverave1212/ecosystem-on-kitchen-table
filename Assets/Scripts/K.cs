using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K : MonoBehaviour
{

    public static float TILE_SIZE_X = 1.0f;
    public static float TILE_SIZE_Y = 0.25f;
    public static float TILE_SIZE_Z = 1.0f;
    public static float ANIMAL_FEET_HEIGHT = 0.25f;

    public static int NONE   = 0;
    public static int UP     = 1;
    public static int RIGHT  = 2;
    public static int DOWN   = 3;
    public static int LEFT   = 4;

    public static int NO_MOOD = 0;
    public static int EAT   = 1;
    public static int MATE  = 2;
    public static int RUN   = 3;

    public static string[] moodToString = {"NO_MOOD", "EAT", "MATE", "RUN"};
    public static string[] directionToString = {"NONE", "UP", "RIGHT", "DOWN", "LEFT"};

    public static int FIND_RABBIT = 1;
    public static int FIND_FOX = 2;
    public static int FIND_PLANT = 3;

}
