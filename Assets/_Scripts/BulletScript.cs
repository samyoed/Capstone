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
        string shoot;
        string change;
        string currentTargetSwitch;

        public bool isPush;
        public float bulletStr;

        public Color color;
        public Player player;

        public GameObject currentTarget;

        public List<Transform> currentTargetList;
        public int currentTargetIndex;

        public float shootTime = 0;
        public float shootTimeMax = 1;
        public bool canShoot = false;
        public Vector3 offset;

        void Start()
        {
            shoot = GetComponent<Player>().shootInput;
            change = GetComponent<Player>().changeInput;
            currentTargetSwitch = GetComponent<Player>().targetSwitchInput;
            
            player = GetComponent<Player>();

            currentTarget = GameObject.Find("Ball");
            currentTargetIndex = 0;
            //gets the children of object "Targetable Objects"
            foreach(Transform child in GameObject.Find("TargetableObjects").transform)
                currentTargetList.Add(child);
        }
        void Update () 
        {
            if(Input.GetButtonDown(change))
                isPush = !isPush;

            //changes currentTarget on button press
            if(Input.GetButtonDown(currentTargetSwitch))
            {
                currentTargetIndex++;
                if(currentTargetIndex >= currentTargetList.Count)
                    currentTargetIndex = 0;
                currentTarget = currentTargetList[currentTargetIndex].gameObject;

                if(currentTarget == gameObject)
                    currentTargetIndex++;
                if(currentTargetIndex >= currentTargetList.Count)
                    currentTargetIndex = 0;
                currentTarget = currentTargetList[currentTargetIndex].gameObject;
            }

            if(shootTime < shootTimeMax)
            {
                shootTime += Time.deltaTime;
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }


            if(Input.GetButton(shoot))
            {
                if(canShoot)
                {
                    AcquireTargetLock(currentTarget);
                    shootTime = 0;
                }
                    
            }

            //shoot projectile
            if (Input.GetButtonDown(shoot) && canShoot) 
            {
                AcquireTargetLock(currentTarget);
                shootTime = 0;
            }
        }

        void Fire(Vector3 direc)
        {
            bullet = Instantiate(projectile, transform.position + offset, transform.rotation);
            bullet.GetComponent<SingleBullet>().isPush = isPush;
            bullet.GetComponent<SingleBullet>().strength = bulletStr;
            bullet.GetComponent<SingleBullet>().currentPlayer = gameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = direc.normalized * speed; 
            bullet.GetComponent<TrailRenderer>().startColor = color;
            bullet.GetComponent<TrailRenderer>().endColor = color;
            Destroy (bullet.gameObject, 1);
        }

        void AcquireTargetLock(GameObject currentTarget){
            bool acquireTargetLockSuccess;
            Vector3 currentTargetVelocity = Vector3.zero;
            if(currentTarget.CompareTag("Ball"))
                currentTargetVelocity = currentTarget.GetComponent<Rigidbody2D>().velocity;
            if(currentTarget.CompareTag("Player"))
                currentTargetVelocity = currentTarget.GetComponent<Player>().velocity;
            
                
            Vector3 direction = CalculateInterceptCourse (currentTarget.transform.position, currentTargetVelocity, transform.position, speed, out acquireTargetLockSuccess);
            if (acquireTargetLockSuccess) 
            {
                Fire(direction);
            }
        }

        public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed, out bool aSuccess)
        {
            aSuccess = true;
            Vector3 currentTargetDir = aTargetPos - aInterceptorPos;
            float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
            float tSpeed2 = aTargetSpeed.sqrMagnitude;
            float fDot1 = Vector3.Dot(currentTargetDir, aTargetSpeed);
            float currentTargetDist2 = currentTargetDir.sqrMagnitude;
            float d = (fDot1 * fDot1) - currentTargetDist2 * (tSpeed2 - iSpeed2);
            if (d < 0.1f)
                aSuccess = false;
            float sqrt = Mathf.Sqrt(d);
            float S1 = (-fDot1 - sqrt) / currentTargetDist2;
            float S2 = (-fDot1 + sqrt) / currentTargetDist2;
            if (S1 < 0.0001f)
            {
                if (S2 < 0.0001f)
                return Vector3.zero;
                else
                return (S2) * currentTargetDir + aTargetSpeed;
            }
            else if (S2 < 0.0001f)
                return (S1) * currentTargetDir + aTargetSpeed;
            else if (S1 < S2)
                return (S2) * currentTargetDir + aTargetSpeed;
            else
                return (S1) * currentTargetDir + aTargetSpeed;
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
