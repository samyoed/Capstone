using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    [RequireComponent(typeof(PlayerControl2))]
    public class Player : MonoBehaviour
    {
        public enum state
        {
            DASH, ATTACK, SPECIAL, DEFAULT
        }

        public state phase;

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
        public float jumpMultiplier = 2f;

        Vector3 velocity;
        float velocityXSmoothing;

        PlayerControl2 controller;
        SpriteRenderer sr;

        //moveDash variables
        float dashTimer = 0;
        public float dashTimerMax = 10f;
        public float dashSpeed = 90;
        public float dashTempSpeed = 0;

        float attackTimer = 0;
        public float attackTimerMax = 10f;
        public float attackSpeed = 0;
        public float attackTempSpeed = 0;

        public int currentDashCount = 0;
        public int dashNum = 2;

        float specialTimer = 0;
        public float specialTimerMax = 10;

        //attackdash Variables
        public Vector2 input;
        public direction dashDirec;
        public direction direc;

        public GameObject partsys;
        public GameObject ball;

        public int fastGravMult;
        public float fastGrav;
        public float normGravTemp;

        public float currVel;
        public Vector3 prevPos; 

        // custom controls
        public string horizInput;
        public string vertInput;
        public string actionInput;
        public string attackInput;
        public string specialInput;

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
        //controls how strong the bullets are
        public float bulletMult;

        public float lerpTime = .5f;
        public float currentLerpTime;

        public GhostScript ghost;

        public float ballShootDiminisher;
        public float ballShootDiminisherTemp;

        public bool hasSpecial = false;
        //the radius of the circle collider for hit diminishing
        //public float circleDiameter;

        public Transform circColl;

        public Vector2 lockedDashDirec;

        void Start()
        {
            controller = GetComponent<PlayerControl2>();
            sr = GetComponent<SpriteRenderer>();
            circColl = gameObject.transform.GetChild(1);

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            //minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
            print ("Gravity:" + gravity + "jumpVelocity" + jumpVelocity);

            //fastGrav = gravity*fastGravMult;
            normGravTemp = gravity;

            currentScale = transform.localScale;

            ball = GameObject.Find("Ball");
            //circleDiameter = circColl.radius*2;

            dashTimer = 100;
            attackTimer = 100;
            phase = state.DEFAULT;
        }

        void Update()
        {
//-----------------------------RUN BEFORE EVERY FRAME----------------------------------
            if(input.x < -0.01)
                isFacingRight = false;
            else if(input.x > 0.01)
                isFacingRight = true;

            if(isFacingRight)
                transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
            if(!isFacingRight)
                transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);


            if(hasSpecial)
               transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();

            if(currentDashCount == 0)
                sr.color = Color.cyan;
            else 
                sr.color = Color.white;

            //input
            input = new Vector2(Mathf.Round(Input.GetAxisRaw(horizInput)), Mathf.Round(Input.GetAxisRaw(vertInput)));

//-----------------------------STATE MACHINE-------------------------------------------
            switch(phase)
            {
                //----------------------------------ATTACKING---------------------------
                case state.ATTACK:

                    attackTimer ++;
                    Vector2 attInput;

                    if(lockedDashDirec.x == 0 && lockedDashDirec.y == 0 && isFacingRight)
                    {
                        attInput = new Vector2(1f,0f);
                        attackTempSpeed = attackSpeed *.8f;
                    }
                    else if(lockedDashDirec.x == 0 && lockedDashDirec.y == 0 && !isFacingRight)
                    {
                        attInput = new Vector2(-1f,0f);
                        attackTempSpeed = attackSpeed *.8f;
                    }
                    //high jump
                    else if(lockedDashDirec.x == 0 && lockedDashDirec.y == 1)
                    {
                        attInput = input;
                        attackTempSpeed = attackSpeed*Mathf.Sqrt(2);
                    }
                    else
                    {
                        //for NON diagonal direction
                        if(Mathf.Abs(lockedDashDirec.x) + Mathf.Abs(lockedDashDirec.y) != 2)
                            attackTempSpeed = attackSpeed*Mathf.Sqrt(2);
                        //for diagonal direction
                        else 
                            attackTempSpeed = attackSpeed;
                        attInput = input;
                    }
                    velocity = (new Vector3(attInput.x, attInput.y, 0)) * attackTempSpeed;

                    gravity = 0;
                    if(attackTimer > attackTimerMax)
                    {
                        phase = state.DEFAULT;
                        gravity = normGravTemp;
                        currentDashCount--;
                        circColl.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                        circColl.transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = false;
                        break;
                    }
                    break;
                //----------------------------------DASHING-----------------------------
                case state.DASH:
                    
                    dashTimer ++;
                    Vector2 dashInput;

                    //low jump
                    if(lockedDashDirec.x == 0 && lockedDashDirec.y == 0)
                    {
                        dashInput = new Vector2(0f,1f);
                        dashTempSpeed = dashSpeed *.8f;
                    }
                    //high jump
                    else if(lockedDashDirec.x == 0 && lockedDashDirec.y == 1)
                    {
                        dashInput = input;
                        dashTempSpeed = dashSpeed*Mathf.Sqrt(2);
                    }
                    else
                    {
                        //for NON diagonal direction
                        if(Mathf.Abs(lockedDashDirec.x) + Mathf.Abs(lockedDashDirec.y) != 2)
                            dashTempSpeed = dashSpeed*Mathf.Sqrt(2);
                        //for diagonal direction
                        else 
                            dashTempSpeed = dashSpeed;
                        dashInput = input;
                    }
                    velocity = (new Vector3(dashInput.x, dashInput.y, 0)) * dashTempSpeed;

                    gravity = 0;
                    if(dashTimer > dashTimerMax)
                    {
                        phase = state.DEFAULT;
                        gravity = normGravTemp;
                        currentDashCount--;
                        break;
                    }
                break;
                //-----------------------------------SPECIAL-----------------------------
                case state.SPECIAL:

                    specialTimer ++;

                    if(specialTimer > specialTimerMax)
                    {
                        phase = state.DEFAULT;
                        break;
                    }
                    break;
                //-----------------------------------DEFAULT-----------------------------
                case state.DEFAULT:
                     //dashes when button is pressed and there are dashes available
                    if(Input.GetButtonDown(actionInput) && currentDashCount > 0)
                    {
                        lockedDashDirec = input;
                        phase = state.DASH;
                        print("hello?" + Time.time);
                        dashTimer = 0;
                    }
                    if(Input.GetButtonDown(attackInput) && currentDashCount > 0)
                    {
                        lockedDashDirec = input;
                        phase = state.ATTACK;
                        attackTimer = 0;
                        Vector3 circCollRot = Vector3.zero;

                        circColl.transform.GetChild(0).GetComponent<CapsuleCollider2D>().size = new Vector2(5f,8f);


                        if(input.x == 0 && input.y == 0 && isFacingRight)
                            circCollRot.z = -90f;       //if no direction inputted
                        else if(input.x == 0 && input.y == 0 && !isFacingRight)
                            circCollRot.z = 90f;
                        else if(input.x == 0 && input.y == 1)
                            circCollRot.z = 0f;          //if up input
                        else if(input.x == 1 && input.y == 1)
                            circCollRot.z = -45f;     //if up right input
                        else if(input.x == 1 && input.y == 0)
                            circCollRot.z = -90f;      //if right input
                        else if(input.x == 1 && input.y == -1)
                            circCollRot.z = -135f;   //if down right input
                        else if(input.x == 0 && input.y == -1)
                            circCollRot.z = -180f;        //if down input
                        else if(input.x == -1 && input.y == -1)
                            circCollRot.z = -225f;    //if down left input
                        else if(input.x == -1 && input.y == 0)
                            circCollRot.z = -270f;        //if left input
                        else if(input.x == -1 && input.y == 1)
                            circCollRot.z = -315f;      //if up left input

                        circColl.eulerAngles = circCollRot;
                        circColl.transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = true;
                        circColl.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
                    }
                    if(Input.GetButtonDown(specialInput) && hasSpecial)
                    {
                        phase = state.SPECIAL;
                        specialTimer = 0;
                    }
        
                    //regain dashes once colliding with the ground
                    if(controller.collisions.below)
                        currentDashCount = dashNum;

                        //smooths x movement and overall x movement
                    float targetVelocityX = input.x * moveSpeed;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, 
                                                      ref velocityXSmoothing,
                                                     (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

                    velocity.y += gravity * 1/60;

                break;
            }


//-----------------------------RUN AFTER EVERY FRAME-------------------------------------

            if((controller.collisions.above || controller.collisions.below))
            {       
                velocity.y = 0;
            }
            
                
            controller.Move(velocity /60);

        }

        void FixedUpdate()
        {
            switch(phase)
            {
                case state.ATTACK:
                break;
                case state.DASH:


                
                break;
                case state.DEFAULT:
                break;
            }




            //turn on trail when dashing
            dashTrail();
            currentDashDirec();
            //ballShootDiminisherTemp = Vector3.Distance(transform.position, ball.transform.position);
        }

        void OnTriggerExit2D (Collider2D other)
        {  
            if(other.gameObject.CompareTag("Weapon") && other.gameObject.GetComponent<SingleBullet>().currentPlayer != gameObject)
            {
                var force = transform.position - other.transform.position;
                float str = other.gameObject.GetComponent<SingleBullet>().strength;
    
                force.Normalize ();

			    int pushPull;
			    if(other.gameObject.GetComponent<SingleBullet>().isPush)
			    	pushPull = -1;
			    else
			    	pushPull = 1;

                Vector3 bulletVec = (pushPull * force*2);
                xEdit = bulletVec.x;
                yEdit = bulletVec.y;
                currentLerpTime = 0;
                print("hello");

                tempEdit = new Vector3(xEdit, yEdit, 0);
		   }
        }

        // void dashDimSet()
        // {
        //     if(ballShootDiminisherTemp < circleDiameter/3)
        //         ballShootDiminisher = 1;
        //     if(circleDiameter/3 < ballShootDiminisherTemp && ballShootDiminisherTemp < circleDiameter/(2/3))
        //         ballShootDiminisher = 2;
        //     if(ballShootDiminisher > circleDiameter/(2/3))
        //         ballShootDiminisher = 3;
        // }

        // void calculateDirec()
        // {
        //     switch(direc)
        //     {
        //         case direction.up:
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
        //             break;
        //     }      
        // }
        //for ball velocity
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
            if(phase == state.DASH)
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
