using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class MapSide : MonoBehaviour
    {
        public bool isRight;
        MapSegment mapSegment;
        // Start is called before the first frame update
        void Start()
        {
            mapSegment = transform.parent.GetComponent<MapSegment>();
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.CompareTag("Ball"))
            {
                //for if its a goal
                if(mapSegment.isRightGoal && isRight)
                    mapSegment.scored = true;
                else if(mapSegment.isLeftGoal && !isRight)
                    mapSegment.scored = true;

                //if its just a transition
                else if(!mapSegment.isRightGoal && isRight)
                    mapSegment.SwitchRight();
                else if(!mapSegment.isLeftGoal && !isRight)
                    mapSegment.SwitchLeft();
            }
        }
    }
}
