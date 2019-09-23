using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spaceCadet
{
    [RequireComponent(typeof(Camera))]
    public class DynamicCamera : MonoBehaviour
    {
        public List<Transform> targets;
        public Vector3 offset;
        public float smoothTime = .5f;
        private Vector3 velocity;

        public float minZoom = 10f;
        public float maxZoom = 30f;
        public float zoomLimiter = 50f;
        private Camera cam;

        void Start()
        {
            cam = GetComponent<Camera>();
            //cam.orthographicSize = 100;
        }

        void LateUpdate()
        {
            if(targets.Count == 0)
                return;
            Move();
            Zoom();
        }  

        void Move()
        {
            Vector3 centerPoint = GetCenterPoint();

            Vector3 newPosition = centerPoint + offset;

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
        
        void Zoom()
        {
            float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        }

        float GetGreatestDistance()
        {
            var bounds = new Bounds (targets[0].position, Vector3.zero);
            for(int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.size.x;
        }


        Vector3 GetCenterPoint()
        {
            if (targets.Count == 1)
            {
                return targets[0].position;
            }

            var bounds  = new Bounds(targets[0].position, Vector3.zero);
            for(int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
    }
}
