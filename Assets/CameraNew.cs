using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraNew : MonoBehaviour
{
    public MapManager mapManager;

    void Start()
    {
        mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
    }

    void Update()
    {

        Vector3 newPosition = mapManager.currentSegment.transform.position;
        newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        transform.DOLocalMove(newPosition, 1.5f);
    }
}
