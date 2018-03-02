using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBox : MonoBehaviour {

    private PlayerScript s;
    private PlayerScript os;

    public void Start()
    {
        s = GetComponentInParent<PlayerScript>();
        os = s.otherPlayer.GetComponent<PlayerScript>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!col.collider.CompareTag(tag) && (col.collider.CompareTag("collisionHitbox1") || col.collider.CompareTag("collisionHitbox2")))
        {

        }
    }
}
