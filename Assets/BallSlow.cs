using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BallSlow : MonoBehaviour
{
    float currentTimeScale = 1;
    
    void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.CompareTag("Ball"))
        {
            //Time.timeScale = .5f;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Ball"))
        {
            Time.timeScale = 1f;
        }
    }
}
