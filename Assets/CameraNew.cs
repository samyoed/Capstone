using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ballGame
{
    public class CameraNew : MonoBehaviour
    {
        public MapManager mapManager;
        public Image black;
    
        void Start()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
        }
    
        public void UpdatePosition()
        {
            Vector3 newPosition = mapManager.currentSegment.transform.position;
            newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            transform.DOLocalMove(newPosition, .75f);
        }

        public void FadeToBlack()
        {
            black.DOFade(255, 200);
        }
    }
}
