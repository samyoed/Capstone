using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ballGame
{
    public class GameManager : MonoBehaviour
    {
        public float team1Health;
        public float team2Health;

        public bool goal1Broken = false;
        public bool goal2Broken = false;

        public float timeScale;

        public int winInt = 3;

        public TextMesh team1;
        public TextMesh team2;

        void Start()
        {
            Application.targetFrameRate = 60;
        }

        void LateUpdate()
        {
            if(PersistentData.team1Points >= winInt)
                team1Win();
            if(PersistentData.team2Points >= winInt)
                team2Win();

            // if(team1Health <= 0 && PersistentData.team1Points < winInt)
            //     team2Point();
            // if(team2Health <= 0 && PersistentData.team2Points < winInt)
            //     team1Point();

            if(team1Health <= 0)
                team1Point();
            if(team2Health <= 0)
                team2Point();



            team1.text = "" + team1Health + "/ " + PersistentData.team1Points;
            team2.text = "" + team2Health + "/ " + PersistentData.team2Points;
        }

        void team1Point()
        {
            if(Time.timeScale == 1f)
            {
                PersistentData.team1Points++;
                SceneManager.LoadScene("Main Scene");
            }
        }
        void team2Point()
        {
            if(Time.timeScale == 1f)
            {
                PersistentData.team2Points++;
                SceneManager.LoadScene("Main Scene");
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
