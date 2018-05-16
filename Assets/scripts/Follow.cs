using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{
    public Transform[] targets;
    public Transform leftEdge, rightEdge, topEdge, bottomEdge;

    private Camera camera;
    private Vector3 moveVelocity;

    public float speed = 2.0f;
    public float dampTime = 0.2f;
    public float vertExtent;
    public float horzExtent;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        targets = new Transform[2];
    }

    public void setTargets(Transform player1, Transform player2)
    {
        targets[0] = player1;
        targets[1] = player2;
    }

    void FixedUpdate()
    {
        vertExtent = camera.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        float interpolation = speed * Time.deltaTime;

        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += targets[i].position;
            numTargets++;
        }
        if (numTargets > 0)
            averagePos /= numTargets;

        //position.y = Mathf.Lerp(this.transform.position.y, targets.transform.position.y, interpolation);
        //position.x = Mathf.Lerp(this.transform.position.x, targets.transform.position.x, interpolation);

        float highest = 0;
        foreach(Transform target in targets)
        {
            if (target.transform.position.y > highest)
                highest = target.transform.position.y;
        }

        //averagePos.y = highest;

        Vector3 pos = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), averagePos, ref moveVelocity, dampTime);
        pos.x = Mathf.Clamp(pos.x, leftEdge.position.x + horzExtent, rightEdge.position.x - horzExtent);
        pos.y = Mathf.Clamp(pos.y, bottomEdge.position.y + vertExtent, topEdge.position.y - vertExtent);
        transform.position = pos;
    }
}