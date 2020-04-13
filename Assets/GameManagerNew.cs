using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class GameManagerNew : MonoBehaviour
    {
        public int teamLeftScore;
        public int teamRightScore;
        public MapManager mapManager;
        public List<MapSegment> mapList = new List<MapSegment>();
        public MapSegment[] goalList = new MapSegment[2];

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();


            foreach(MapSegment mapSeg in mapManager.GetComponentsInChildren<MapSegment>())
                mapList.Add(mapSeg);
            foreach(MapSegment mapSeg in mapList)
                if(mapSeg.isLeftGoal)
                    goalList[0] = mapSeg;
                else if(mapSeg.isRightGoal)
                    goalList[1] = mapSeg;
            
        }

        // Update is called once per frame
        void Update()
        {
            foreach(MapSegment mapSeg in goalList)
            {
                if(mapSeg.isRightGoal && mapSeg.scored)
                {
                    teamRightScore++;
                    mapSeg.scored = false;
                }
                if(mapSeg.isLeftGoal && mapSeg.scored)
                {
                    teamLeftScore++;
                    mapSeg.scored = false;
                }
            }
            
        }
    }
}
