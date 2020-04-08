using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSide : MonoBehaviour
{
    public bool isRight;
    MapSegment mapSegment;
    // Start is called before the first frame update
    void Start()
    {
        mapSegment = transform.parent.GetComponent<MapSegment>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Ball"))
        {
            if(isRight)
                mapSegment.SwitchRight();
            else
                mapSegment.SwitchLeft();
        }
    }
}
