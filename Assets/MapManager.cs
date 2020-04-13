using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class MapManager : MonoBehaviour
    {
        public List<MapSegment> segmentList;
        public List<List<MapSegment>> mapList;
        public List<Transform> playerList;
        public MapSegment currentSegment;
        public Transform cameraTransform;
        private Vector3 playerOffset;

        int currentIndex;


        void Start()
        {
            cameraTransform = Camera.main.transform;
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
        void PlayerTeleport()
        {
            

            foreach(Transform player in playerList)
                player.transform.position = new Vector3(player.position.x - playerOffset.x + cameraTransform.position.x,
                                                        player.position.y - playerOffset.y + cameraTransform.position.y,
                                                        player.position.z);


            playerOffset = cameraTransform.position;
            print("teleport");
        }


        //for the small delay in switching rooms
        IEnumerator SwitchLeftCo()
        {
            foreach(MapSegment mapSeg in segmentList)
                foreach(Collider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = false;

            currentIndex--;
            currentSegment = segmentList[currentIndex];
            yield return new WaitForSeconds(1.5f);

            PlayerTeleport();        //teleporting players to new room


            foreach(MapSegment mapSeg in segmentList)
                foreach(Collider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = true;
        }

        IEnumerator SwitchRightCo()
        {
            foreach(MapSegment mapSeg in segmentList)
                foreach(Collider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = false;
            currentIndex++;
            currentSegment = segmentList[currentIndex];

            yield return new WaitForSeconds(1.5f);

            PlayerTeleport();        //teleporting players to new room


            foreach(MapSegment mapSeg in segmentList)
                foreach(Collider2D boxColl in mapSeg.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = true;
        }
        IEnumerator SwitchDown()
        {
            
            yield return new WaitForSeconds(2);
        }
    }    
}   
