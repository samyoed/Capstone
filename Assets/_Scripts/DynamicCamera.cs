using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace ballGame
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

        public Vector3 finalFinalPosition;




        void Start()
        {

            cam = GetComponent<Camera>();
            //cam.orthographicSize = 100;

            transform.position = PersistentData.lastSceneCameraPosition;
        }
        void Awake()
        {
            foreach(Transform child in GameObject.Find("TargetableObjects").transform)
            {
                targets.Add(child);
            }
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
            Vector3 finalPosition = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
            finalFinalPosition = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
            transform.position = finalFinalPosition;
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
