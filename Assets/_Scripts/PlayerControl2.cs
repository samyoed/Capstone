﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerControl2 : MonoBehaviour
    {
        public LayerMask collisionMask;

        public float skinWidth = .1f;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        float horizontalRaySpacing;
        float verticalRaySpacing;

        BoxCollider2D collide;
        RaycastOrigins raycastOrigins;
        public CollisionInfo collisions;

        void Start()
        {
            collide = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            collisions.Reset();
            if(velocity.x != 0)
                HorizontalCollisions (ref velocity);
            if(velocity.y != 0)
                VerticalCollisions(ref velocity);

            transform.Translate(velocity);
        }


        void HorizontalCollisions(ref Vector3 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for(int x = 0; x < horizontalRayCount; x++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if(hit)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;

                }
            }
        }

        void VerticalCollisions(ref Vector3 velocity)
        {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for(int x = 0; x < verticalRayCount; x++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft:raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * x + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if(hit)
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;

                }
            }
        }

        void UpdateRaycastOrigins()
        {
            Bounds bounds = collide.bounds;
            bounds.Expand (skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        void CalculateRaySpacing()
        {
            Bounds bounds = collide.bounds;
            bounds.Expand (skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount , 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        struct RaycastOrigins 
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }

        //organizational purposes
        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public void Reset()
            {
                above = below = false;
                left = right = false;
            }
        }
    }
}
