using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ballGame
{
    public class MapManager : MonoBehaviour
    {
        public Transform mapObject; // stores the current map
        public List<Transform> fullMapList;
        public List<Room> roomList;
        //public List<List<MapSegment>> mapList;
        public List<Transform> playerList;
        public Room currentSegment;
        public Transform cameraTransform;
        public CameraNew virtCam;
        public Vector3 playerOffset;

        int currentIndex;
        int mapObjectIdx = 0;


        void Awake()
        {
            cameraTransform = Camera.main.transform;
            //make list of all the maps 

            foreach(Transform child in transform)
                fullMapList.Add(child);
            // make the current map the first one

            initMap();
        }

        void Start()
        {
            virtCam.UpdatePosition();
        }

        public void initMap()
        {
             mapObject = fullMapList[mapObjectIdx];
             roomList.Clear();
            //make list of all the rooms
            Room[] segTempList = mapObject.GetComponentsInChildren<Room>();
            foreach(Room room in segTempList)
                roomList.Add(room);

            currentIndex = roomList.Count/2;
            currentSegment = roomList[currentIndex];
            mapObjectIdx++;
            

        }
        public void SwitchRight()
        {
            StartCoroutine(SwitchRightCo());
            virtCam.UpdatePosition();
            StartCoroutine(GoalSlow());
        }
        public void SwitchLeft()
        {
            StartCoroutine(SwitchLeftCo());
            virtCam.UpdatePosition();
            StartCoroutine(GoalSlow());
        }
        void PlayerTeleport()
        {
            foreach(Transform player in playerList)
            {


                float playerXPos = player.position.x - playerOffset.x;
                int notEmptyIdxRight = Mathf.RoundToInt(playerXPos/2) + 28;
                int notEmptyIdxLeft = notEmptyIdxRight;




                print("unedited" + notEmptyIdxRight);

                int rightCount = 0;
                int leftCount = 0;
                while(currentSegment.floorNotEmpty[notEmptyIdxRight] && notEmptyIdxRight < 55)
                {
                    rightCount++;
                    notEmptyIdxRight++;
                }
                while(currentSegment.floorNotEmpty[notEmptyIdxLeft] && notEmptyIdxLeft > 0)
                {
                    leftCount++;
                    notEmptyIdxLeft--;
                }
                if(rightCount < leftCount)
                {
                    playerXPos = (notEmptyIdxRight - 28) * 2;
                }
                else if(rightCount >= leftCount)
                {
                    playerXPos = (notEmptyIdxLeft - 28) * 2;
                }



                print("edited" + playerXPos + "\nleft" + notEmptyIdxLeft + "  right" + notEmptyIdxRight);



                Vector3 tempPos =  new Vector3(player.position.x - playerOffset.x + cameraTransform.position.x,
                                                        player.position.y - playerOffset.y + cameraTransform.position.y,
                                                        player.position.z);
                
                player.transform.position = new Vector3(playerXPos + cameraTransform.position.x,
                                                        player.position.y - playerOffset.y + cameraTransform.position.y,
                                                        player.position.z);

            }
            playerOffset = cameraTransform.position;
            print("teleport");
        }
        //for the small delay in switching rooms
        IEnumerator SwitchLeftCo()
        {
            foreach(Room room in roomList)
                foreach(Collider2D boxColl in room.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = false;

            currentIndex--;
            currentSegment = roomList[currentIndex];
            yield return new WaitForSeconds(1.5f);

            PlayerTeleport();        //teleporting players to new room


            foreach(Room room in roomList)
                foreach(Collider2D boxColl in room.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = true;
        }

        IEnumerator SwitchRightCo()
        {
            foreach(Room room in roomList)
                foreach(Collider2D boxColl in room.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = false;
            currentIndex++;
            currentSegment = roomList[currentIndex];

            yield return new WaitForSeconds(1.5f);

            PlayerTeleport();        //teleporting players to new room


            foreach(Room room in roomList)
                foreach(Collider2D boxColl in room.transform.GetComponentsInChildren<BoxCollider2D>())
                    boxColl.enabled = true;
        }
        IEnumerator SwitchDown()
        {
            yield return new WaitForSeconds(2);
        }
        IEnumerator GoalSlow()
        {
            DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, .1f, .1f);
            yield return new WaitForSeconds(.1f);
            DOTween.To(()=>Time.timeScale, x=>Time.timeScale = x, 1f, .1f);

        }
    }    
}   
