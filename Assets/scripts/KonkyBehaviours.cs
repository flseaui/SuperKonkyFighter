using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

    static int[,] konkyMoveMap = new int[3, 9]{
        {
            4, 4, 4,
            0, 0, 0,
            7, 7, 7,
        },
        {
            5, 5, 5,
            1, 1, 2,
            8, 8, 8,
        },
        {
            6, 6, 6,
            3, 3, 3,
            9, 9, 9,
        },
    };

    static int[,] konkyFrames = new int[,]
    {
        {15,  8},//5L  0
        {20, 12},//5M  1
        {34, 18},//6M  2
        {30, 17},//5H  3
        {19, 10},//2L  4
        {24, 13},//2M  5
        {20, 23},//2H  6
        {16,  7},//jL  7
        {26, 13},//jM  8
        {12, 12},//jH  9
        {52, 40},//1S  10
        {26, 13},//6S  11
        {47, 34},//3S  12
        {49, 34},//2S  13
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
