using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class GoalScript : MonoBehaviour
    {
        public GameObject gameManager;
        public bool isTeam1;
        public float moveDist;

        public float lerpTime = 1.25f;
        float currentLerpTime;
        Vector3 startPos;

        public Vector3 position1;
        public Vector3 position2;
        public Vector3 position3;

        public float healthMult = .5f;

        public bool goingUp;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            startPos = position2;
            position1 = new Vector3(transform.position.x, position1.y, position1.z);
            position2 = new Vector3(transform.position.x, position2.y, position2.z);
            position3 = new Vector3(transform.position.x, position3.y, position3.z);
            
        }
        void moveGoal(Vector3 position)
        {
            currentLerpTime += Time.deltaTime;
                if (currentLerpTime > lerpTime) 
                {
                    currentLerpTime = lerpTime;
                }
                float t = currentLerpTime / lerpTime;
                t = t*t*t * (t * (6f*t - 15f) + 10f);
                
                transform.position = Vector3.Lerp(startPos, position, t);
        }
    }
}
