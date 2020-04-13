using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class MapSegment : MonoBehaviour
    {
        public bool isRightGoal;
        public bool isLeftGoal;
        MapManager mapManager; 
        public BoxCollider2D rightEntry;
        public BoxCollider2D leftEntry;
        public bool scored;
        // Start is called before the first frame update
        void Start()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
        }


        public void RightScore()
        {

        }
        public void LeftScore()
        {
            
        }

        public void SwitchRight()
        {
            mapManager.SwitchRight();
        }
        public void SwitchLeft()
        {
            mapManager.SwitchLeft();
        }
    }
}
