using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealHitboxScript : MonoBehaviour
{

    string tag;
    ContactPoint2D[] contactPoints;

    public void OnCollisionStay2D(Collision2D col)
    {
        col.GetContacts(contactPoints);
        Debug.Log("testin with ya boi scott: " + contactPoints[0]);
    }
}
