using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    [RequireComponent(typeof(PlayerControl2))]
    public class Player : MonoBehaviour
    {

        public float maxJumpHeight = 4;
        public float minJumpHeight = 1;
        public float timeToJumpApex = .4f;
        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .1f;

        public float moveSpeed = 6;

        float gravity;
        float jumpVelocity;
        float minJumpVelocity;

        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;

        public Vector3 velocity;
        float velocityXSmoothing;

        PlayerControl2 controller;

        float jumpBuffCount = 0;
        public float jumpBuffMax = 10f;

        float dashTimer = 0;
        public float dashTimerMax = 10f;
        public float dashSpeed = 90;
        public float dashTempSpeed = 0;
        public bool isDashing = false;
        public int currentDashCount = 0;
        public int dashNum = 2;


        public Vector2 input;
        public direction dashDirec;

        //Time warp setup
        public GameObject partsys;

        //public LinkedList<Vector3> timeMani;
        //public int warpCount = 0;

        public LinkedListNode<Vector3> current;

        //public Transform player;
        public int timeSampleRate = 50; //how many samples are able to be stored
        public float sampleTime = .1f; //amount of time between each sample
        float sampleTimeCurrent = 0;
        public bool isChoosingTime = false;


        public float tWarpTimer = 0;
        public float tWarpTimerMax = 5;

        public int fastGravMult;
        public float fastGrav;
        public float normGravTemp;

        public float currVel;
        public Vector3 prevPos; 


        // custom controls
        public string horizInput;
        public string vertInput;
        public string actionInput;
        public string changeInput;
        public string targetSwitchInput;
        public string shootInput;

        public Color defColor;
        public Color dashColor;

        bool isFacingRight;
        Vector3 currentScale;

        //for hit physics
        public float xEdit;
        public float yEdit;
        public float editAccel = 1;
        public Vector3 edit;
        public Vector3 tempEdit;

        
        public float lerpTime = .5f;
        public float currentLerpTime;

        public enum direction
        { up, upRight, right, downRight, down, downLeft, left, upLeft, none }


        public GhostScript ghost;


        void Start()
        {
            controller = GetComponent<PlayerControl2>();

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            //minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
            print ("Gravity:" + gravity + "jumpVelocity" + jumpVelocity);


            fastGrav = gravity*fastGravMult;
            normGravTemp = gravity;

            currentScale = transform.localScale;
            //TIME MANIPULATION THING
            //timeMani = new LinkedList<Vector3>();
        }

        void Update()
        {
            if(!isChoosingTime)
            {
                currVel = ((transform.position - prevPos).magnitude) / Time.deltaTime;
                prevPos = transform.position;

                currentDashDirec();

                //limits time in air
                isDashing = false;

                //Sets the time and length of dash
                if(dashTimer < (dashTimerMax/100))
                {
                    dashTimer += Time.deltaTime;

                    dashTempSpeed = dashSpeed;

                    if(Mathf.Abs(input.x) + Mathf.Abs(input.y) != 2)
                        dashTempSpeed = dashSpeed*Mathf.Sqrt(2);

                    velocity = (new Vector3(input.x, input.y, 0)) * dashTempSpeed;
                    isDashing = true;
                }

                

                //dashes when button is pressed and there are dashes available
                if(Input.GetButtonDown(actionInput) && currentDashCount > 0 && !controller.collisions.below && !isDashing)
                {
                    dashTimer = 0;
                    currentDashCount--;
                }

                if((controller.collisions.above || controller.collisions.below) && isDashing == false)
                {
                velocity.y = 0;
                }

                if(controller.collisions.below)
                    currentDashCount = 4;

                input = new Vector2(Mathf.Round(Input.GetAxisRaw(horizInput)), Mathf.Round(Input.GetAxisRaw(vertInput)));


                //jump buffer system
                if(jumpBuffCount < (jumpBuffMax/100))
                {
                    jumpBuffCount += Time.deltaTime;
                    if(controller.collisions.below)
                    {
                        velocity.y = jumpVelocity;

                    }
                }
                if(Input.GetButtonDown(actionInput))
                    jumpBuffCount = 0;

                if (velocity.y < 0) 
    		        velocity.y += gravity  * (fallMultiplier - 1) * Time.deltaTime;
                else if (velocity.y > 0)
    		    	velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;

                //fastfall 

                if(input.y == -1)
                {
                    gravity = fastGrav;
                }
                else
                {
                    gravity = normGravTemp;
                }

                if(isDashing)
                {
                    gravity = 0;
                }
                
                tempEdit = new Vector3(xEdit, yEdit, 0);


                currentLerpTime += Time.deltaTime;
                if (currentLerpTime > lerpTime) 
                {
                    currentLerpTime = lerpTime;
                }
                float t = currentLerpTime / lerpTime;
                t = t*t*t * (t * (6f*t - 15f) + 10f);
                
                edit = Vector3.Lerp(tempEdit, Vector3.zero, t);




                //smooths x movement and overall x movement
                float targetVelocityX = input.x * moveSpeed;
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, 
                                              ref velocityXSmoothing,
                                             (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime + edit);


                if(velocity.x < 0)
                    isFacingRight = false;
                else if(velocity.x > 0)
                    isFacingRight = true;

                if(isFacingRight)
                    transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
                if(!isFacingRight)
                    transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);

                
            }

        }

        void FixedUpdate()
        {
            //turn on trail when dashing
                dashTrail();
        }


        void currentDashDirec()
        {
            if(input.x == 0 && input.y == 0)
                dashDirec = direction.none;        //if no direction inputted
            else if(input.x == 0 && input.y == 1)
                dashDirec = direction.up;          //if up input
            else if(input.x == 1 && input.y == 1)
                dashDirec = direction.upRight;     //if up right input
            else if(input.x == 1 && input.y == 0)
                dashDirec = direction.right;       //if right input
            else if(input.x == 1 && input.y == -1)
                dashDirec = direction.downRight;   //if down right input
            else if(input.x == 0 && input.y == -1)
                dashDirec = direction.down;        //if down input
            else if(input.x == -1 && input.y == -1)
                dashDirec = direction.downLeft;    //if down left input
            else if(input.x == -1 && input.y == 0)
                dashDirec = direction.left;        //if left input
            else if(input.x == -1 && input.y == 1)
                dashDirec = direction.upLeft;      //if up left input
        }
        void dashTrail()
        {
            if(isDashing)
            {
                partsys.GetComponent<ParticleSystem>().Play();
            }
            else 
            {
                partsys.GetComponent<ParticleSystem>().Stop();
            }
        }

    }
}
