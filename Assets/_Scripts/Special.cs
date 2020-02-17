using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class Special : MonoBehaviour
    {
        public enum special{Grapple, Shoot, DashAttack};

        public special thisSpecial;


        //for homing bullets
        public GameObject projectile;
        public float strength;
        private GameObject bullet;
        public Transform target;
        public Rigidbody2D rigidBody;
        public float angleSpeed;
        public float speed;
        public Color color;


        // Update is called once per frame
        public void activateSpecial()
        {
            switch(thisSpecial)
            {
                case special.Grapple:
                    grapple();
                break;
                case special.Shoot:
                    shoot();
                break;
                case special.DashAttack:
                    moveAttack();
                break;

                default:
                break;
            }
        }

        void grapple()
        {

        }

        void shoot()
        {
           
            bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<SingleBullet>().movementSpeed = speed;
            bullet.GetComponent<SingleBullet>().angleChangingSpeed = angleSpeed;
            bullet.GetComponent<SingleBullet>().strength = strength;
            //bullet.GetComponent<Rigidbody2D>().velocity = direc.normalized * speed; 
            bullet.GetComponent<TrailRenderer>().startColor = color;
            bullet.GetComponent<TrailRenderer>().endColor = color;
            Destroy(bullet, 1);

        }

        void moveAttack()
        {
            
        }

    }
}
