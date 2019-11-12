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
        
        int currentPosition = 2; //can either be 1 2 or 3

        public Vector3 position1;
        public Vector3 position2;
        public Vector3 position3;

        public float healthMult = .1f;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            startPos = position2;
            position1 = new Vector3(transform.position.x, position1.y, position1.z);
            position2 = new Vector3(transform.position.x, position2.y, position2.z);
            position3 = new Vector3(transform.position.x, position3.y, position3.z);
            
        }

        void Update()
        {
            switch(currentPosition)
            {
                case 1:
                    moveGoal(position1);
                    break;
                case 2:
                    moveGoal(position2);
                    break;
                case 3:
                    moveGoal(position3);
                    break;
            }
        }


        void scoreEdit(float vel)
        {
            if(isTeam1)
                gameManager.GetComponent<GameManager>().team1Health -= vel;
            else
                gameManager.GetComponent<GameManager>().team2Health -= vel;
        
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

        void OnCollisionEnter2D(Collision2D coll)
        {
            if(coll.gameObject.CompareTag("Ball"))
            {
                float veloc = coll.gameObject.GetComponent<BallScript>().vel * healthMult;
                scoreEdit(veloc);
            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Ball"))
            {
                Time.timeScale = 0.2f;
            }
        }

        void OnTriggerExit2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Ball"))
            {
                Time.timeScale = 1f;
            }
            if(coll.gameObject.CompareTag("Weapon"))
            {
                if(coll.gameObject.GetComponent<SingleBullet>().isPush)
                {
                    if(currentPosition > 1)
                    {
                        currentLerpTime = 0;
                        startPos = transform.position;
                        currentPosition -= 1;
                    }
                }
                else
                {
                    if(currentPosition < 3)
                    {
                        currentLerpTime = 0;
                        startPos = transform.position;
                        currentPosition += 1;
                    }
                }
            }
        }
    }
}
