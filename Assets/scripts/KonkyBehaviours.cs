using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

    static int[,] konkyMoveMap = new int[3, 9]{
        {
            4 ,4 ,4,
            0 ,0 , -1,
            -1 ,-1 ,-1
        },
        {
             3, 3, 3,
            -1, 1, 1,
            -1,-1, 3
        },
        {
            5,5,5,
            2,2,2,
            -1,-1,-1
        },
    };

    static  int[] konkyFrames = new int[]
    {
        8, //KONKY ELBOW
        16, //KONKY PUNCH 
        32, //KONKY roundhouse
        12, //konky low kick uan
        15, //Konky low punch
        38, //konky low
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
