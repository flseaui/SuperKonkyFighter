using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealHitboxScript : MonoBehaviour
{

    string tag;
    ContactPoint2D[] contactPoints;

    private void OnCollisionEnter2D(Collision2D col)
    {
        col.GetContacts(contactPoints);
        Debug.Log("testin with ya boi scott: " + contactPoints[0]);
    }
}
