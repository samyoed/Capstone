using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ballGame
{
    public class BallScript : MonoBehaviour
    {
        public bool isHit;
        public float moveMag = 25;
        public float attackMag = 50;
        public float pullMag = 30;
        public float magAdd;

        public bool isPush;

        public GameObject player;
        public GameObject quad1;
        public GameObject quad2;

        public ParticleSystem partSys;
        public Rigidbody2D rb;

        public Vector3 input;

        public float maxVelocity = 100f;

        List<Transform> currentPlayerList;
        //Transform[] currentPlayerArr;
        Transform closestPlayer;

        public float vel;
        public float playerMult = .1f;

       void Start()
       {
            partSys = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            rb = gameObject.GetComponent<Rigidbody2D>();
            currentPlayerList = new List<Transform>();

            //gets list of players
            foreach(Transform child in GameObject.Find("TargetableObjects").transform)
            {
                if(child != this.transform)
                    currentPlayerList.Add(child);
            }
       }

       void FixedUpdate()
       {
           rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
           vel = rb.velocity.magnitude;
           //finds closest player to ball for camera
           //closestPlayer = GetClosestPlayer(currentPlayerList);

       }

        void OnCollisionEnter2D (Collision2D other)
        {
            //ball collision
            // if(other.gameObject.CompareTag("Player"))
            // {
            //     Player player = other.gameObject.GetComponent<Player>();

            //     if(player.isDashing)
            //     {
            //         switch(player.dashDirec)
            //         {
            //             case direction.up:
            //         input.x = 0; input.y = 1;
            //             break;
            //         case direction.upRight:
            //         input.x = 1; input.y = 1;
            //             break;
            //         case direction.right:
            //         input.x = 1; input.y = 0;
            //             break;
            //         case direction.downRight:
            //         input.x = 1; input.y = -1;
            //             break;
            //         case direction.down:
            //         input.x = 0; input.y = -1;
            //             break;
            //         case direction.downLeft:
            //         input.x = -1; input.y = -1;
            //             break;
            //         case direction.left:
            //         input.x = -1; input.y = 0;
            //             break;
            //         case direction.upLeft:
            //         input.x = -1; input.y = 1;
            //             break;
            //         default:
            //         input.x = 1; input.y = 1;
            //             break;
            //         }
                    

            //         Vector3 force = Vector3.Normalize(input);
            //         //GetComponent<Rigidbody2D> ().AddForce (force * mag, ForceMode2D.Impulse);
            //         GetComponent<Rigidbody2D> ().velocity =  force * mag;

            //         print("SHOOOT");
            //     }
            //     else
            //     {
            //         //for collision with player
            //         magAdd = player.currVel;

            //         Vector3 force = transform.position - other.transform.position;

            //         force.Normalize ();
            //         GetComponent<Rigidbody2D> ().AddForce (force * (magAdd), ForceMode2D.Impulse);

            //         player.xEdit = -force.x * vel * playerMult;
            //         player.yEdit = -force.y * vel * playerMult;
            //         player.currentLerpTime = 0;
            //         //other.gameObject.GetComponent<Rigidbody2D>().AddForce (-force * (mag*magAdd), ForceMode2D.Impulse);
            //     }
            // }
            if(other.gameObject.CompareTag("Goal"))
            {
               StartCoroutine(particles(other.gameObject.GetComponent<GoalScript>().isTeam1));
            }
        }
   
        void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.name == "Collider")
            {
                Player player = other.transform.parent.parent.GetComponent<Player>();

                if(player.phase == Player.state.ATTACK)
                {
                    input = player.input;

                    //float dist = Vector3.Distance(player.transform.position, transform.position);
                    //float distDim = player.ballShootDiminisher;
                    player.hasSpecial = true;

                    Vector3 force = Vector3.Normalize(input);
                    //GetComponent<Rigidbody2D> ().AddForce (force * mag, ForceMode2D.Impulse);
                    GetComponent<Rigidbody2D> ().velocity =  (force * attackMag);

                    print("SHOOOT");
                }

                if(player.phase == Player.state.DASH)
                {

                    input = player.input;
                    Vector2 force = input.normalized;
                    GetComponent<Rigidbody2D> ().velocity =  GetComponent<Rigidbody2D>().velocity + (force * moveMag);

                    player.hasSpecial = true;

                }
            }
        }

        Transform GetClosestPlayer (List<Transform> players)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach(Transform potentialTarget in players)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
     
            return bestTarget;
        }

        void OnTriggerExit2D (Collider2D other)
        {  
            if(other.gameObject.CompareTag("Weapon"))
            {
                var force = transform.position - other.transform.position;
                float str = other.gameObject.GetComponent<SingleBullet>().strength;
    
                force.Normalize ();

			    int pushPull;
			    if(other.gameObject.GetComponent<SingleBullet>().isPush)
			    	pushPull = -1;
			    else
			    	pushPull = 1;

                GetComponent<Rigidbody2D> ().AddForce (pushPull * force * str, ForceMode2D.Impulse);
		    }
            if(other.gameObject.name == "Collider")
            {
                other.transform.parent.parent.GetComponent<Player>().hasSpecial = true;
            }
        }

        IEnumerator particles(bool isTeam1)
        {
            partSys.Play();
            DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, .1f, .1f);

            
                if(isTeam1)
                {
                    //quad1.SetActive(true);
                    quad1.GetComponent<shlab2>().reset();
                    quad1.GetComponent<shlab2>().ballVelTemp = vel/100;
                    quad1.GetComponent<shlab2>().isHit = true;
                }
                else
                {
                    //quad2.SetActive(true);
                    quad2.GetComponent<shlab2>().reset();
                    quad2.GetComponent<shlab2>().ballVelTemp = vel/100;
                    quad2.GetComponent<shlab2>().isHit = true;
                }
            
            yield return new WaitForSeconds(.0005f*vel);
            partSys.Stop();
            DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, 1f, .1f);

            
                if(isTeam1)
                {
                    //quad1.SetActive(false);
                    quad1.GetComponent<shlab2>().reset();
                    quad1.GetComponent<shlab2>().isHit = false;
                }
                else
                {
                    //quad2.SetActive(false);
                    quad2.GetComponent<shlab2>().reset();
                    quad2.GetComponent<shlab2>().isHit = false;
                }
            
        }
    }
}
