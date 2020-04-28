using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ballGame
{
    public class ProgressSlider : MonoBehaviour
    {

        Slider slider;
        MapManager mapManager;


        void Start()
        {
            slider = GetComponent<Slider>();
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
            slider.maxValue = mapManager.roomList.Count - 1;
            slider.value = slider.maxValue/2;
        }

        public void UpdateSlider(float finish)
        {
            DOTween.To(()=>slider.value, x=>slider.value = x, finish, .3f);
        }

    }
}
