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

        //public ParticleSystem partSys;
        public Rigidbody2D rb;
        public GameManager gameManager;
        public ScoreManager scoreManager;

        public Vector3 input;

        public float maxVelocity = 100f;

        List<Transform> currentPlayerList;
        //Transform[] currentPlayerArr;
        Transform closestPlayer;


        public float timeSlowTime = .0009f;

        public float vel;
        public float playerMult = .1f;

        public float healthMult;

        bool hasSwappedBlack = false;
        bool hasSwappedWhite = false;
        Renderer renderer;


        void Start()
        {
            //partSys = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            rb = gameObject.GetComponent<Rigidbody2D>();
            currentPlayerList = new List<Transform>();
            renderer = GetComponent<Renderer>();

            //gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
            //scoreManager = GameObject.FindWithTag("Score Manager").GetComponent<ScoreManager>();

            //gets list of players
            // foreach(Transform child in GameObject.Find("TargetableObjects").transform)
            // {
            //     if(child != this.transform)
            //         currentPlayerList.Add(child);
            // }
        }

        void Update()
        {
            if(transform.position.x < 0 && !hasSwappedBlack)
            {

                renderer.material.DOColor(Color.black, .3f);
                hasSwappedBlack = true;
                hasSwappedWhite = false;

            }
            if(transform.position.x > 0 && !hasSwappedWhite)
            {
                renderer.material.DOColor(Color.white, .3f);
                hasSwappedBlack = false;
                hasSwappedWhite = true;
            }
        }

        void FixedUpdate()
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            vel = rb.velocity.magnitude;
           //finds closest player to ball for camera
           //closestPlayer = GetClosestPlayer(currentPlayerList);
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
    
                // force.Normalize ();

			    // int pushPull;
			    // // if(other.gameObject.GetComponent<SingleBullet>().isPush)
			    // // 	pushPull = -1;
			    // // else
			    // // 	pushPull = 1;

                print("what?");
                GetComponent<Rigidbody2D> ().AddForce (-force * str);
		    }
            if(other.gameObject.name == "Collider")
            {
                other.transform.parent.parent.GetComponent<Player>().hasSpecial = true;
            }
        }

        // IEnumerator particles(bool isTeam1)
        // {
        //     //partSys.Play();
        //     if(gameManager.team1Health < 0 || gameManager.team2Health < 0)
        //     {
        //         scoreManager.reset();
        //         timeSlowTime = 0.004f;
        //     }

        //     //DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, .1f, .1f);

        //         if(isTeam1)
        //         {
        //             //quad1.SetActive(true);
        //             quad1.GetComponent<shlab2>().reset();
        //             quad1.GetComponent<shlab2>().ballVelTemp = vel/100;
        //             quad1.GetComponent<shlab2>().isHit = true;
        //         }
        //         else
        //         {
        //             //quad2.SetActive(true);
        //             quad2.GetComponent<shlab2>().reset();
        //             quad2.GetComponent<shlab2>().ballVelTemp = vel/100;
        //             quad2.GetComponent<shlab2>().isHit = true;
        //         }
            
        //     yield return new WaitForSeconds(timeSlowTime*vel);
        //     partSys.Stop();
        //     //DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, 1f, .1f);

            
        //         if(isTeam1)
        //         {
        //             //quad1.SetActive(false);
        //             quad1.GetComponent<shlab2>().reset();
        //             quad1.GetComponent<shlab2>().isHit = false;
        //         }
        //         else
        //         {
        //             //quad2.SetActive(false);
        //             quad2.GetComponent<shlab2>().reset();
        //             quad2.GetComponent<shlab2>().isHit = false;
        //         }
            
        // }
    }
}
