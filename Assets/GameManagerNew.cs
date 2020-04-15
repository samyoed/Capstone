using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace ballGame
{
    public class GameManagerNew : MonoBehaviour
    {
        public int teamLeftScore;
        public int teamRightScore;
        public Transform mapObject;
        public MapManager mapManager;
        public List<Room> mapList = new List<Room>();
        public TextMeshProUGUI rightScoreText;
        public TextMeshProUGUI leftScoreText;
        int mapObjectIdx = 0;

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
            

            // foreach(Room room in mapObject)
            //     mapList.Add(room);
            
        }

        // Update is called once per frame
        void Update()
        {
            

            
        }
        public void RightScore()
        {
            teamRightScore++;
            Camera.main.GetComponent<CameraNew>().FadeToBlack();
            mapManager.initMap();
            mapObject = mapManager.mapObject;
            Vector3 mapPosEdit = mapManager.mapObject.transform.position;
            Vector3 mapAdd = new Vector3(112, 0, 0);
            mapManager.mapObject.transform.position = mapPosEdit + mapAdd;
        }
        public void LeftScore()
        {
            teamLeftScore++;
            Camera.main.GetComponent<CameraNew>().FadeToBlack();
            mapManager.initMap();
            mapObject = mapManager.mapObject;
            Vector3 mapPosEdit = mapManager.mapObject.transform.position;
            Vector3 mapAdd = new Vector3(-112, 0, 0);
            mapManager.mapObject.transform.position = mapPosEdit + mapAdd;
            

        }
        
    }
}
