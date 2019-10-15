using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManiScript : MonoBehaviour
{
    LinkedList<Vector3> timeMani;
    LinkedListNode<Vector3> current;

    public Transform player;
    public int timeSampleRate = 50; //how many samples are able to be stored
    public float sampleTime = .1f; //amount of time between each sample
    float sampleTimeCurrent = 0;
    public bool isChoosingTime = false;

    public float scrollSpeed = 1;

    public float tWarpTimer = 0;
    public float tWarpTimerMax = 5;

    void Start()
    {
        timeMani = new LinkedList<Vector3>();
    }

    void Update()
    {

        if(!isChoosingTime)
        {
            sampleTimeCurrent += Time.deltaTime;
            if(sampleTimeCurrent >= sampleTime)
            {
                timeMani.AddFirst(player.position);
                if(timeMani.Count < timeSampleRate)
                    timeMani.RemoveLast();
            }
        }
        
        if(Input.GetKey(KeyCode.P))
        {
            isChoosingTime = true;
            current = timeMani.First;

            if(tWarpTimer < tWarpTimerMax)
            {
                tWarpTimer += Time.deltaTime;

            }
        }
        else
        {
            isChoosingTime = false;
        }
        

    }
}

