using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ballGame
{
    public class BulletScript : MonoBehaviour
    {
        public GameObject projectile;
        private GameObject bullet;
        public float speed = 200;
        private bool hit;
        string shoot;
        string change;
        string targetSwitch;

        public bool isPush;

        public Color color;
        public Player player;

        public GameObject currentTarget;

        public List<Transform> targetList;
        public int currentTargetIndex;

        void Start()
        {
            shoot = GetComponent<Player>().shootInput;
            change = GetComponent<Player>().changeInput;
            targetSwitch = GetComponent<Player>().targetSwitchInput;
            
            player = GetComponent<Player>();

            currentTarget = GameObject.Find("Ball");
            currentTargetIndex = 0;
            //gets the children of object "Targetable Objects"
            foreach(Transform child in GameObject.Find("TargetableObjects").transform)
                targetList.Add(child);
        }
        void Update () 
        {
            if(Input.GetButtonDown(change))
                isPush = !isPush;

            //changes target on button press
            if(Input.GetButtonDown(targetSwitch))
            {
                currentTargetIndex++;
                if(currentTargetIndex >= targetList.Count)
                    currentTargetIndex = 0;
                currentTarget = targetList[currentTargetIndex].gameObject;

                if(currentTarget == gameObject)
                    currentTargetIndex++;
                if(currentTargetIndex >= targetList.Count)
                    currentTargetIndex = 0;
                currentTarget = targetList[currentTargetIndex].gameObject;
            }
            // Put this in your update function
            if (Input.GetButtonDown(shoot) && GetComponent<Player>().dashDirec == Player.direction.none) 
            {
                // Instantiate the projectile at the position and rotation of this transform
                //GameObject bullet;
                bullet = Instantiate(projectile, transform.position, transform.rotation);
                bullet.GetComponent<SingleBullet>().isPush = isPush;
                bullet.GetComponent<Rigidbody2D>().velocity = (currentTarget.transform.position - transform.position).normalized * speed; 
                bullet.GetComponent<TrailRenderer>().startColor = color;
                bullet.GetComponent<TrailRenderer>().endColor = color;

                if(hit == true)
                {
                    Destroy(bullet.gameObject);
                }
                else
                {
                    Destroy (bullet.gameObject, 1);
                    hit = false;
                }
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                Destroy(bullet);
                print("hello");
            }
        }
    }
}
