using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

    static int[,] konkyMoveMap = new int[3, 9]{
        {
            -1 ,-1 ,-1,
            0 ,0 , -1,
            -1 ,-1 ,-1
        },
        {
             3, 3, 3,
            -1, 1, 1,
            -1,-1, 3
        },
        {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        },
    };

    static  int[] konkyFrames = new int[]
    {
        8, //KONKY ELBOW
        16, //KONKY PUNCH 
        0, //kek not implem
        12, //konky low kick uan
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
