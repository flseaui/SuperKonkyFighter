using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konky : Behaviors{

    int attackState;

    int[,] moveMap = new int[3,9]{
        {
            -1,-1,-1,
            -1, 0,-1,
            -1,-1,-1
        },
        {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        },
        {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        },
    };

    int[] frames = new int[]
    {
        40 //KONKY ELBOW
    };

}
