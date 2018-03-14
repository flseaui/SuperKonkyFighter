using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    string tag;
    public bool hurt;

    void OnTriggerEnter2D(Collider2D col)
    {
        hurt = true;
        Debug.Log("hittin with ya boi scott");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        hurt = false;
    }
}
