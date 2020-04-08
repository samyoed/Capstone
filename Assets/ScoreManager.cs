using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


namespace ballGame
{
    class Score
    {
        public Vector3 startPosition;
        public TextMeshProUGUI textMesh;

        public Score (TextMeshProUGUI text, Vector3 startPos)
        {
            startPosition = startPos;
            textMesh = text;
        }
    }
    
    public class ScoreManager : MonoBehaviour
    {
        TextMeshProUGUI[] textMeshList;
        List<Score> scoreList = new List<Score>();

        public TextMeshProUGUI text1;
        public TextMeshProUGUI text2;

        public Animator anim1;
        public Animator anim2;

        public Vector3 startPos1;
        public Vector3 startPos2;
        

        public int textMove;
        void Start()
        {
            textMeshList =  transform.GetComponentsInChildren<TextMeshProUGUI>();



            foreach (TextMeshProUGUI textMesh in textMeshList)
            {
                scoreList.Add(new Score(textMesh, textMesh.transform.position));
                Vector3 startPosTemp = textMesh.transform.position;
                if(textMesh.transform.GetComponent<scoreScript>().isTeam1)
                {
                    text1 = textMesh;
                    startPos1 = textMesh.transform.position;
                    textMesh.transform.position += new Vector3(textMove, 0, 0);
                    
                }
                else
                {
                    text2 = textMesh;
                    startPos2 = textMesh.transform.position;
                    textMesh.transform.position -= new Vector3(textMove, 0, 0);
                    
                }
            }
        }

        void Update()
        {
            text1.text = PersistentData.team1Points + "";
            text2.text = PersistentData.team2Points + "";


        }

        public void reset()
        {
            anim1.Play("ScoreFade1");
            anim2.Play("ScoreFade2");
            text1.transform.DOMoveX(text1.transform.position.x - textMove, .1f).SetEase(Ease.OutSine);
            text2.transform.DOMoveX(text2.transform.position.x + textMove, .1f).SetEase(Ease.OutSine);
        }
    }
}
