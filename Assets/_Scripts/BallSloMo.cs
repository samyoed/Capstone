using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSloMo : MonoBehaviour
{
    public float timeSlow = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            Time.timeScale = timeSlow;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Time.timeScale = 1;
    }
}
