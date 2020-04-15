using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class MapSide : MonoBehaviour
    {
        public bool isRight;
        Room room;
        // Start is called before the first frame update
        void Start()
        {
            room = transform.parent.GetComponent<Room>();
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.CompareTag("Ball"))
            {
                //for if its a goal
                if((room.isRightGoal && isRight) || (room.isLeftGoal && !isRight))
                {
                    room.scored = true;
                    room.TeamScore();
                }

                //if its just a transition
                else if(!room.isRightGoal && isRight)
                    room.SwitchRight();
                else if(!room.isLeftGoal && !isRight)
                    room.SwitchLeft();
            }

            print("hello");
        }
    }
}
