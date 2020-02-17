using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class SingleBullet : MonoBehaviour
    {
        public float strength;
        ParticleSystem partSys;

        public Transform target;
        public Rigidbody2D rigidBody;
        public float angleChangingSpeed;
        public float movementSpeed = 100;

        Vector2 initDirection;


        void Awake()
        {
            partSys = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            target = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
            rigidBody = GetComponent<Rigidbody2D>();
            initDirection = ((Vector2)target.position - rigidBody.position);
            initDirection.Normalize();
        }


        void Update()
        {
            Vector2 direction = (Vector2)target.position - rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross (direction,transform.rotation* initDirection).z;
            rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
            rigidBody.velocity = transform.rotation * initDirection * movementSpeed;
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            // if(coll.gameObject.CompareTag("Player"))
            //     StartCoroutine(particles());
            // else
            //     Destroy(this.gameObject);
        }

        IEnumerator particles()
        {
            partSys.Play();
            yield return new WaitForSeconds(.01f);
            partSys.Stop();
        }    
    }

    
}
