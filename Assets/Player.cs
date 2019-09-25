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
    float maxJumpVelocity;
    float minJumpVelocity;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    Vector3 velocity;
    float velocityXSmoothing;

    PlayerControl2 controller;
    void Start()
    {
        controller = GetComponent<PlayerControl2>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
        print ("Gravity:" + gravity + "jumpVelocity" + maxJumpVelocity);
    }

    void Update()
    {

        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.C) && controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
    

        print("Y velocity: " + velocity.y);
        if (velocity.y < 0) 
		    velocity.y += gravity  * (fallMultiplier - 1) * Time.deltaTime;
		else if (velocity.y > 0 && !Input.GetKey(KeyCode.C))
			velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;


        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
