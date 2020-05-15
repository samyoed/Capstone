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
        public GameObject grey;
        public TextMeshProUGUI rightScoreText;
        public TextMeshProUGUI leftScoreText;
    
        void Awake()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
        }
        void Start()
        {
            black.DOFade(0, 1);
        }
    
        public void UpdatePosition()
        {
            Vector3 newPosition = mapManager.currentSegment.transform.position;
            //print(mapManager.currentSegment.transform.position);
            newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            transform.DOLocalMove(newPosition, .75f);
        }

        // public void FadeToBlack()
        // {
        //     black.DOFade(255, 200);
        //     rightScoreText.DOFade(255, 200);
        //     leftScoreText.DOFade(255, 200);

        // }


        public IEnumerator FadeToGrey()
        {
            yield return new WaitForSeconds(.1f);
            //black.DOFade(1,200);
            grey.SetActive(true);
            DOTween.To(()=> grey.GetComponent<CanvasGroup>().alpha, x=> grey.GetComponent<CanvasGroup>().alpha = x, 1, 2);
        }
    }
}
