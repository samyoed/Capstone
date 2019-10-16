using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 velocity;
    float velocityXSmoothing;

    PlayerControl2 controller;

    float jumpBuffCount = 0;
    public float jumpBuffMax = 10f;

    float dashTimer = 0;
    public float dashTimerMax = 10f;
    public float dashSpeed = 90;
    public float dashTempSpeed = 0;
    public bool isDashing;
    public int currentDashCount = 0;
    public int dashNum = 2;


    public Vector2 input;
    public direction dashDirec;

    //Time warp setup
    public GameObject partsys;

    public LinkedList<Vector3> timeMani;
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

    //public List<Transform> shotTargets;

    public GameObject currTarget;
    private Vector3 currTargetPos;


    void Start()
    {
        controller = GetComponent<PlayerControl2>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
        print ("Gravity:" + gravity + "jumpVelocity" + jumpVelocity);

        
        fastGrav = gravity*fastGravMult;
        normGravTemp = gravity;

        //TIME MANIPULATION THING
        timeMani = new LinkedList<Vector3>();
    }

    public enum direction
    { up, upRight, right, downRight, down, downLeft, left, upLeft, none }

    void Update()
    {
        // if(Input.GetButton("Dash"))
        // {
        //     Time.timeScale = 0.1f;
        // }
        // else 
        //     Time.timeScale = 1f;

        //find direction between player and target
        currTargetPos = currTarget.transform.position;
        Vector2 dir = (currTargetPos - transform.position).normalized;

        if(Input.GetButton("Freeze"))
            isChoosingTime = true;
        else
            isChoosingTime = false;


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

            if(isDashing && (dashDirec == direction.none))
                GetComponent<FirePart>()._clone_ps_em.enabled = true;
            else
                GetComponent<FirePart>()._clone_ps_em.enabled = false;


            //turn on trail when dashing
            dashTrail();

            //dashes when button is pressed and there are dashes available
            if(Input.GetButtonDown("Dash") && currentDashCount > 0 && !controller.collisions.below)
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

            input = new Vector2(Mathf.Round(Input.GetAxisRaw("Horizontal")), Mathf.Round(Input.GetAxisRaw("Vertical")));


            //jump buffer system
            if(jumpBuffCount < (jumpBuffMax/100))
            {
                jumpBuffCount += Time.deltaTime;
                if(controller.collisions.below)
                {
                    velocity.y = jumpVelocity;
                    
                }
            }
            if(Input.GetButtonDown("Jump"))
                jumpBuffCount = 0;

            // if (velocity.y < 0) 
		    //     velocity.y += gravity  * (fallMultiplier - 1) * Time.deltaTime;
		    // else if (velocity.y > 0 && !Input.GetButton("Jump"))
		    // 	velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;

            if (velocity.y < 0) 
		        velocity.y += gravity  * (fallMultiplier - 1) * Time.deltaTime;
            else if (velocity.y > 0)
		    	velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;

            if(isDashing)
            {
                gravity = 0;
            }


            //fastfall 
            
            if(input.y == -1)
            {
                gravity = fastGrav;
            }
            else
            {
                gravity = normGravTemp;
            }

            //smooths x movement
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            //continue sampling time if 
            sampleTimeCurrent += Time.deltaTime;
            if(sampleTimeCurrent >= sampleTime)
            {
                timeMani.AddFirst(transform.position);
                if(timeMani.Count < timeSampleRate)
                    timeMani.RemoveLast();
                sampleTimeCurrent = 0;
            }
        }
        else if(isChoosingTime)
        {
            current = timeMani.First;

            if(tWarpTimer < tWarpTimerMax)
            {
                tWarpTimer += Time.deltaTime;
            }
            else if(input.x == 1f && current.Next != null)
            {
                current = current.Next; 
                tWarpTimer = 0;
            }
            else if(input.x == -1f && current.Previous != null)
            {
                current = current.Previous;
                tWarpTimer = 0;
            }
            this.transform.position = current.Value;

            print(current.Value);
            
        }

        
        

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
            GetComponent<TrailRenderer>().material.color = new Color(0,255,255,255);
            partsys.GetComponent<ParticleSystem>().Play();
        }
        else 
        {
            GetComponent<TrailRenderer>().material.color = Color.blue;
            partsys.GetComponent<ParticleSystem>().Stop();
        }
    }

}
