using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ballGame
{
    public class GameManager : MonoBehaviour
    {
        public float team1Health;
        public float team2Health;

        public float timeScale;

        public int winInt = 3;

        public TextMesh team1;
        public TextMesh team2;

        public bool paused = false;
        public BallScript ballScript;
        public Transform mainCam;
        
        public bool pointAdded = false;

        void Start()
        {
            Application.targetFrameRate = 60;
            ballScript = GameObject.Find("Ball").GetComponent<BallScript>();
            mainCam = GameObject.FindWithTag("MainCamera").transform;
        }


        void Update()
        {
            if(paused)
            {
                Time.timeScale = 0;
            }

            if(team1Health <= 0 && !pointAdded)
            {
                pointAdded = true;
                PersistentData.team1Points++;
            }
            if(team2Health <= 0 && !pointAdded)
            {
                pointAdded = true;
                PersistentData.team2Points++;
            }

        }


        void LateUpdate()
        {
            if(PersistentData.team1Points >= winInt)
                team1Win();
            if(PersistentData.team2Points >= winInt)
                team2Win();

            if(team1Health <= 0)
                teamPoint(1);
            if(team2Health <= 0)
                teamPoint(2);

            team1.text = "" + team1Health + "/ " + PersistentData.team1Points;
            team2.text = "" + team2Health + "/ " + PersistentData.team2Points;
        }

        void teamPoint(int teamNum)
        {
            //ballScript.timeSlowTime = .003f;
            if(Time.timeScale == 1f)
            {
                SceneManager.LoadScene("Main Scene");
                PersistentData.lastSceneCameraPosition = mainCam.transform.position;
            }
        }

        void team1Win()
        {
            team1.text = "I win yay";
            PersistentData.team1Points = 0;
        }
        void team2Win()
        {
            team1.text = "I win yay";
            PersistentData.team2Points = 0;
        }

    }
}
