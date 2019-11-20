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

        public int winInt = 3;

        public Text team1;
        public Text team2;

        void LateUpdate()
        {
            if(team1Health <= 0)
                team2Win();
            if(team2Health <= 0)
                team1Win();

            team1.text = "" + team1Health;
            team2.text = "" + team2Health;
        }

        void team1Win()
        {
            if(Time.timeScale == 1f)
                SceneManager.LoadScene("Main Scene");
        }
        void team2Win()
        {
            if(Time.timeScale == 1f)
                SceneManager.LoadScene("Main Scene");
        }

    }
}
