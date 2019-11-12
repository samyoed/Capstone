using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
   public class BallScript : MonoBehaviour
   {
       public bool isHit;
       public float mag = 25;
       public float pullMag = 30;
       public float magAdd;

       public bool isPush;

       public GameObject player;

       public ParticleSystem partSys;
       public Rigidbody2D rb;

       public float maxVelocity = 100f;


       public float vel;
        public float playerMult = .1f;

       void Start()
       {
            partSys = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            rb = gameObject.GetComponent<Rigidbody2D>();
       }

       void FixedUpdate()
       {
           rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
           vel = rb.velocity.magnitude;
       }

        void OnCollisionEnter2D (Collision2D other)
        {
            //ball collision
            if(other.gameObject.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();
               magAdd = player.currVel;

               Vector3 force = transform.position - other.transform.position;

               force.Normalize ();
               GetComponent<Rigidbody2D> ().AddForce (force * (mag*magAdd), ForceMode2D.Impulse);

               player.xEdit = -force.x * vel * playerMult;
               player.yEdit = -force.y * vel * playerMult;
               player.currentLerpTime = 0;
               other.gameObject.GetComponent<Rigidbody2D>().AddForce (-force * (mag*magAdd), ForceMode2D.Impulse);
            }
            if(other.gameObject.CompareTag("Goal"))
            {
               StartCoroutine(particles());
            }
        }

       void OnTriggerEnter2D (Collider2D other)
        {
            isHit = false;
        }
   
        void OnTriggerExit2D (Collider2D other)
        {  
            if(other.gameObject.CompareTag("Weapon"))
            {

            isHit = true;
            var force = transform.position - other.transform.position;
   
            force.Normalize ();

			int pushPull;
			if(other.gameObject.GetComponent<SingleBullet>().isPush)
				pushPull = -1;
			else
				pushPull = 1;

           GetComponent<Rigidbody2D> ().AddForce (pushPull * force * mag* 10, ForceMode2D.Impulse);

		   }
        }

        IEnumerator particles()
        {
            partSys.Play();
            Time.timeScale = .1f;
            yield return new WaitForSeconds(.0005f*vel);
            partSys.Stop();
            Time.timeScale = 1f;
        }
    }
}
