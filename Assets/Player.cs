using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControl2))]
public class Player : MonoBehaviour
{

    float gravity = -20;
    Vector3 velocity;

    PlayerControl2 controller;
    void Start()
    {
        controller = GetComponent<PlayerControl2>();
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
