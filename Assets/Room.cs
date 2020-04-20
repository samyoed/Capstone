using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class Room : MonoBehaviour
    {
        public bool isRightGoal;
        public bool isLeftGoal;
        MapManager mapManager; 
        GameManagerNew gameManager;
        public BoxCollider2D rightEntry;
        public BoxCollider2D leftEntry;
        public bool scored;
        public bool[] floorNotEmpty = new bool[56];
        
        // Start is called before the first frame update
        void Start()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
            gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManagerNew>();
        }

        void Update()
        {

        }

        public void TeamScore()
        {
            scored = false;
            if(isRightGoal && !isLeftGoal)
                gameManager.RightScore();
                
            else if(!isRightGoal && isLeftGoal)
                gameManager.LeftScore();

            
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
