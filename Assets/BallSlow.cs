using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BallSlow : MonoBehaviour
{
    float currentTimeScale = 1;
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        // if(coll.CompareTag("Ball"))
        // {
        //     DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, .5f, .25f);
        // }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Ball"))
        {
            Time.timeScale = 1f;
        }
    }
}
