using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class TargetMarkerScript : MonoBehaviour
    {
        public Vector3 playerOffset;
        public Vector3 goalOffset;
        public Vector3 ballOffset;
        Vector3 offset;
        public BulletScript bulletScript;

        void Update()
        {
            if(bulletScript.currentTarget.CompareTag("Player"))
                offset = playerOffset;
            if(bulletScript.currentTarget.CompareTag("Goal"))
                offset = goalOffset;
            if(bulletScript.currentTarget.CompareTag("Ball"))
                offset = ballOffset;

            transform.position = bulletScript.currentTarget.transform.position + offset;


        }
    }
}
