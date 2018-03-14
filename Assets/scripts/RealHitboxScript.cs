using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealHitboxScript : MonoBehaviour
{
    string tag;
    public bool hurt;

    void OnTriggerEnter2D(Collider2D col)
    {
        hurt = true;
        Debug.Log("hittin with ya boi scott");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hurt = false;
    }
}
