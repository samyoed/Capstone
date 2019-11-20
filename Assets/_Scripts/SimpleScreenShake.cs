using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class SimpleScreenShake : MonoBehaviour
    {
        public Vector3 originalCameraPosition;

        float shakeAmt;

        public Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
            originalCameraPosition = mainCamera.transform.position;
        }

        void OnCollisionEnter2D(Collision2D coll) 
        {
            if(coll.gameObject.CompareTag("Goal"))
            {
                shakeAmt = coll.relativeVelocity.magnitude * .0005f;
                InvokeRepeating("CameraShake", 0, .0005f);
                Invoke("StopShaking", 0.3f);
            }

        }

        void CameraShake()
        {
            if(shakeAmt>0) 
            {
                float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
                Vector3 pp = mainCamera.transform.position;
                pp.x+= quakeAmt; // can also add to x and/or z
                pp.y+= quakeAmt;
                mainCamera.transform.position = pp;
            }
        }

        void StopShaking()
        {
            CancelInvoke("CameraShake");
            mainCamera.transform.position = new Vector3(mainCamera.GetComponent<DynamicCamera>().finalFinalPosition.x,
                                                        originalCameraPosition.y, 
                                                        originalCameraPosition.z);
        }
    }
}
