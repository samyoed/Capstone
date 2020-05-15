using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

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
        public TextMeshProUGUI countDownText;
        public CameraNew virtCam;
        public GameObject whiteWinText, blackWinText;
        int mapObjectIdx = 0;

        public enum GameState {COUNTING, PAUSED, PLAYING, GAMEOVER};
        public GameState gameState;

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
            gameState = GameState.COUNTING;
            StartCoroutine(CountingDown());

            // foreach(Room room in mapObject)
            //     mapList.Add(room);
            foreach(string str in Input.GetJoystickNames())
            {
                print(str);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                

            switch(gameState)
            {
                case GameState.COUNTING:
                break;
                case GameState.PAUSED:
                Time.timeScale = 0;
                break;
                case GameState.PLAYING:
                break;
                case GameState.GAMEOVER:
                break;

            }

            
        }
        public void RightScore()
        {
            teamRightScore++;
            //Camera.main.GetComponent<CameraNew>().FadeToBlack();
            // mapManager.initMap();
            // mapObject = mapManager.mapObject;
            // Vector3 mapPosEdit = mapManager.mapObject.transform.position;
            // Vector3 mapAdd = new Vector3(112, 0, 0);
            // mapManager.mapObject.transform.position = mapPosEdit + mapAdd;
            // virtCam.UpdatePosition();
            blackWinText.SetActive(true);

            StartCoroutine(virtCam.FadeToGrey());
            
        }
        public void LeftScore()
        {
            teamLeftScore++;
            //Camera.main.GetComponent<CameraNew>().FadeToBlack();
            //mapManager.initMap();
            //mapObject = mapManager.mapObject;
            //Vector3 mapPosEdit = mapManager.mapObject.transform.position;
            //Vector3 mapAdd = new Vector3(-112, 0, 0);
            //mapManager.mapObject.transform.position = mapPosEdit + mapAdd;
            //virtCam.UpdatePosition();
            whiteWinText.SetActive(true);
            StartCoroutine(virtCam.FadeToGrey());
            

        }

        IEnumerator CountingDown()
        {
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            countDownText.text = "GO";
            countDownText.DOFade(0, 1);

            gameState = GameState.PLAYING;

            
        }
        
    }
}
