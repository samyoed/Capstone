using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ballGame
{
    public class CameraNew : MonoBehaviour
    {
        public MapManager mapManager;
        public Image black;
        public TextMeshProUGUI rightScoreText;
        public TextMeshProUGUI leftScoreText;
    
        void Awake()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
        }
        void Start()
        {
           
        }
    
        public void UpdatePosition()
        {
            Vector3 newPosition = mapManager.currentSegment.transform.position;
            print(mapManager.currentSegment.transform.position);
            newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            transform.DOLocalMove(newPosition, .75f);
        }

        public void FadeToBlack()
        {
            black.DOFade(255, 200);
            rightScoreText.DOFade(255,200);
            leftScoreText.DOFade(255, 200);

        }
    }
}
