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
    public int dashSpeed = 5;

    public Vector2 input;
    public direction dashDirec;




    void Start()
    {
        controller = GetComponent<PlayerControl2>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
        print ("Gravity:" + gravity + "jumpVelocity" + jumpVelocity);
    }

    public enum direction
    { up, upRight, right, downRight, down, downLeft, left, upLeft, none }

    void Update()
    {
        currentDashDirec();


        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //jump buffer system
        if(jumpBuffCount < (jumpBuffMax))
        {
            jumpBuffCount += 1;
            if(controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }
        if(Input.GetButtonDown("Jump"))
            jumpBuffCount = 0;

        if (velocity.y < 0) 
		    velocity.y += gravity  * (fallMultiplier - 1) * Time.deltaTime;
		else if (velocity.y > 0 && !Input.GetButton("Jump"))
			velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;


        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if(Input.GetKeyDown(KeyCode.C))
        {
            velocity = (new Vector3(1, 1, 0)) * dashSpeed;
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

}
