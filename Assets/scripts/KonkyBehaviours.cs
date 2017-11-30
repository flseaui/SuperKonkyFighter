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
        16, //KONKY ELBOW
        24, //KONKY PUNCH 
        34, //KONKY roundhouse
        12, //konky medium crouch
        15, //Konky low punch
        27, //konky low heavy
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
