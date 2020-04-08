using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public List<MapSegment> segmentList;
    public MapSegment currentSegment;
    
    int currentIndex;


    void Start()
    {
        //make list of all the map segments
        MapSegment[] segTempList = GetComponentsInChildren<MapSegment>();
        foreach(MapSegment mapSeg in segTempList)
            segmentList.Add(mapSeg);

        currentIndex = segmentList.Count/2;
        currentSegment = segmentList[currentIndex];
    }

    public void SwitchRight()
    {
        if(!currentSegment.isRightGoal)
            StartCoroutine(SwitchRightCo());
    }
    public void SwitchLeft()
    {
        if(!currentSegment.isLeftGoal)
            StartCoroutine(SwitchLeftCo());
    }

    

    IEnumerator SwitchLeftCo()
    {
        foreach(MapSegment mapSeg in segmentList)
            foreach(BoxCollider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                boxColl.enabled = false;
        
        currentIndex--;
        currentSegment = segmentList[currentIndex];
        yield return new WaitForSeconds(2);

        foreach(MapSegment mapSeg in segmentList)
            foreach(BoxCollider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                boxColl.enabled = true;
    }

    IEnumerator SwitchRightCo()
    {
        foreach(MapSegment mapSeg in segmentList)
            foreach(BoxCollider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                boxColl.enabled = false;
        currentIndex++;
        currentSegment = segmentList[currentIndex];
        yield return new WaitForSeconds(2);

        foreach(MapSegment mapSeg in segmentList)
            foreach(BoxCollider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                boxColl.enabled = true;
    }
}
