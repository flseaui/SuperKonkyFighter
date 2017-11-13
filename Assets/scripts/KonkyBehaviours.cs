using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

    static int[,] konkyMoveMap = new int[3, 9]{
        {
            0,0,0,
            0, 0,0,
            -0,-0,0
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

    static  int[] konkyFrames = new int[]
    {
        12, //KONKY ELBOW 8
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
