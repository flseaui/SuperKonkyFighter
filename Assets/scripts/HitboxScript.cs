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
    }

    void OnTriggerExit2D(Collider2D other)
    {
        hurt = false;
    }
}
