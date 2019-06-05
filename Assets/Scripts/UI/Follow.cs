using UnityEngine;

namespace UI
{
    public class Follow : MonoBehaviour
    {
        public Transform[] Targets;
        public Transform LeftEdge, RightEdge, TopEdge, BottomEdge;

        private Camera _camera;
        private Vector3 _moveVelocity;

        public float Speed = 2.0f;
        public float DampTime = 0.2f;
        public float VertExtent;
        public float HorzExtent;

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

        void FixedUpdate()
        {
            VertExtent = _camera.orthographicSize;
            HorzExtent = VertExtent * Screen.width / Screen.height;

            float interpolation = Speed * Time.deltaTime;

            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < Targets.Length; i++)
            {
                // If the target isn't active, go on to the next one.
                if (!Targets[i].gameObject.activeSelf)
                    continue;

                // Add to the average and increment the number of targets in the average.
                averagePos += Targets[i].position;
                numTargets++;
            }
            if (numTargets > 0)
                averagePos /= numTargets;

            //position.y = Mathf.Lerp(this.transform.position.y, targets.transform.position.y, interpolation);
            //position.x = Mathf.Lerp(this.transform.position.x, targets.transform.position.x, interpolation);

            float highest = 0;
            foreach(Transform target in Targets)
            {
                if (target.transform.position.y > highest)
                    highest = target.transform.position.y;
            }

            //averagePos.y = highest;

            Vector3 pos = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), averagePos, ref _moveVelocity, DampTime);
            pos.x = Mathf.Clamp(pos.x, LeftEdge.position.x + HorzExtent, RightEdge.position.x - HorzExtent);
            pos.y = Mathf.Clamp(pos.y, BottomEdge.position.y + VertExtent, TopEdge.position.y - VertExtent);
            transform.position = pos;
        }
    }
}