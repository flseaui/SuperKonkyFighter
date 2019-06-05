using UnityEngine;

namespace UI
{
    public class CameraControl : MonoBehaviour
    {
        public float DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        public Transform[] Targets; 


        private Camera _camera;                       
        private float _zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
        private Vector3 _moveVelocity;                 // Reference velocity for the smooth damping of the position.
        private Vector3 _desiredPosition;              // The position the camera is moving towards.

        private float _rightBound;
        private float _leftBound;
        private float _topBound;
        private float _bottomBound;

        public float MapX = 61.4f;
        public float MapY = 41.4f;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        private float _vertExtent;
        private float _horzExtent;

        public Transform LeftEdge, RightEdge;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
            Targets = new Transform[2];
        }

        public void SetTargets(Transform player1, Transform player2)
        {
            Targets[0] = player1;
            Targets[1] = player2;
        }

        private void FixedUpdate()
        {
            _vertExtent = _camera.orthographicSize;
            _horzExtent = _vertExtent * Screen.width / Screen.height;

            // Calculations assume map is position at the origin
            _minX = _horzExtent - MapX / 2.0f;
            _maxX = MapX / 2.0f - _horzExtent;
            _minY = _vertExtent - MapY / 2.0f;
            _maxY = MapY / 2.0f - _vertExtent;

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
            Vector3 pos = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), _desiredPosition, ref _moveVelocity, DampTime);
            pos.x = Mathf.Clamp(pos.x, LeftEdge.position.x + _horzExtent, RightEdge.position.x - _horzExtent);
            //pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }


        private void FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            // Go through all the targets and add their positions together.
            for (int i = 0; i < Targets.Length; i++)
            {
                // If the target isn't active, go on to the next one.
                if (!Targets[i].gameObject.activeSelf)
                    continue;

                // Add to the average and increment the number of targets in the average.
                averagePos += Targets[i].position;
                numTargets++;
            }

            // If there are targets divide the sum of the positions by the number of them to find the average.
            if (numTargets > 0)
                averagePos /= numTargets;

            // Keep the same y value.
            averagePos.y = transform.position.y;

            // The desired position is the average position;
            _desiredPosition = averagePos;
        }


        private void Zoom()
        {
            float requiredSize = FindRequiredSize();
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, DampTime);
        }


        private float FindRequiredSize()
        {
            Vector3 desiredLocalPos = transform.InverseTransformPoint(_desiredPosition);

            float size = 0f;

            for (int i = 0; i < Targets.Length; i++)
            {
                if (!Targets[i].gameObject.activeSelf)
                    continue;

                Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);
            
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
            }

            size += ScreenEdgeBuffer;
            size = Mathf.Max(size, MinSize);

            return size;
        }


        public void SetStartPositionAndSize()
        {
            // Find the desired position.
            FindAveragePosition();

            // Set the camera's position to the desired position without damping.
            transform.position = _desiredPosition;

            // Find and set the required size of the camera.
            _camera.orthographicSize = FindRequiredSize();
        }
    }
}
