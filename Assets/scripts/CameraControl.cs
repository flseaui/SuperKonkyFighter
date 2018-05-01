using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float screenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float minSize = 6.5f;                  // The smallest orthographic size the camera can be.
    public Transform[] targets; 


    private Camera camera;                       
    private float zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 desiredPosition;              // The position the camera is moving towards.

    private float rightBound;
    private float leftBound;
    private float topBound;
    private float bottomBound;

    public float mapX = 61.4f;
    public float mapY = 41.4f;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float vertExtent;
    private float horzExtent;

    public Transform leftEdge, rightEdge;

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

    private void FixedUpdate()
    {
        vertExtent = camera.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - horzExtent;
        minY = vertExtent - mapY / 2.0f;
        maxY = mapY / 2.0f - vertExtent;

        // Move the camera towards a desired position.
        Move();

        // Change the size of the camera based.
        //Zoom();
    }


    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        Vector3 pos = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), desiredPosition, ref moveVelocity, dampTime);
        pos.x = Mathf.Clamp(pos.x, leftEdge.position.x + horzExtent, rightEdge.position.x - horzExtent);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += targets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        averagePos.y = transform.position.y;

        // The desired position is the average position;
        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);
            
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / camera.aspect);
        }

        size += screenEdgeBuffer;
        size = Mathf.Max(size, minSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = desiredPosition;

        // Find and set the required size of the camera.
        camera.orthographicSize = FindRequiredSize();
    }
}
