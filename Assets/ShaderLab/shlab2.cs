using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shlab2 : MonoBehaviour
{
    
    public float lerpTime = 1.25f;
        float currentLerpTime;
        Vector3 startPos;

        public float offset;
        public bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;
        startPos = new Vector3(0,0,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            float t = currentLerpTime / (lerpTime/5f);
            t = t*t*t * (t * (6f*t - 15f) + 10f);

            transform.position = Vector3.Lerp(new Vector3(startPos.x + offset, startPos.y, startPos.z), startPos, t);
            
            //Time.timeScale = 0.2f;
        }else
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime) 
            {
                currentLerpTime = lerpTime;
            }
            float t = currentLerpTime / lerpTime;
            t = t*t*t * (t * (6f*t - 15f) + 10f);

            transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x + offset, startPos.y, startPos.z), t);

            //Time.timeScale = 1f;
        }
    }
    public void reset()
    {
        currentLerpTime = 0;
    }
}
