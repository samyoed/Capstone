﻿using System.Collections;
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
        public ProgressSlider progressSlider;

        public int currentIndex;
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
                float playerYPos = player.position.y - playerOffset.y;
                int playerXCoord = Mathf.RoundToInt(playerXPos/2) + 28;
                int playerYCoord = Mathf.RoundToInt(playerYPos/2) + 18;


                if(currentSegment.tileArr[playerXCoord, playerYCoord])
                {
                    int vacantUp = playerYCoord, vacantDown = playerYCoord;
                    int countUp = 0, countDown = 0;
                    bool specialCase = false;
                    bool useUp = false;

                    while(currentSegment.tileArr[playerXCoord,vacantDown]) //traversing down
                    {
                        if(vacantDown == 0)
                        {
                            useUp = true;
                            specialCase = true;
                            break;
                        }
                        countDown++;
                        vacantDown--;
                    }
                    while(currentSegment.tileArr[playerXCoord,vacantUp]) //traversing up
                    {
                        if(vacantUp == 35)
                        {
                            useUp = false;
                            specialCase = true;
                            break;
                        }
                        countUp++;
                        vacantUp++;
                        print(vacantUp);
                    }


                    if(!specialCase)
                    {
                        if(countDown > countUp)
                            useUp = false;
                        else
                            useUp = true;
                    }
                    if(useUp)
                        playerYPos = (vacantUp - 16) * 2;
                    else
                        playerYPos = (vacantDown - 20) * 2;



                }
                player.transform.position = new Vector2(playerXPos + cameraTransform.position.x,
                                                        playerYPos + cameraTransform.position.y);
                
                
                //int notEmptyIdxRight = Mathf.RoundToInt(playerXPos/2) + 28;
                // int notEmptyIdxLeft = notEmptyIdxRight;




                // print("unedited" + notEmptyIdxRight);

                // int rightCount = 0;
                // int leftCount = 0;
                // bool useRight = true;
                // bool specialCase = false;
                // while(currentSegment.floorNotEmpty[notEmptyIdxRight])
                // {
                //     if(notEmptyIdxRight >= 55)
                //     {
                //         useRight = false;
                //         specialCase = true;
                //         break;
                //     }

                //     rightCount++;
                //     notEmptyIdxRight++;
                // }
                // while(currentSegment.floorNotEmpty[notEmptyIdxLeft])
                // {
                //     if(notEmptyIdxLeft <= 0)
                //     {
                //         useRight = true;
                //         specialCase = true;
                //         break;
                //     }
                //     leftCount++;
                //     notEmptyIdxLeft--;
                // }
                // if(!specialCase)
                // {
                //     if(rightCount < leftCount)
                //         useRight = true;
                //     else
                //         useRight = false;
                // }
                // if(useRight)
                //     playerXPos = (notEmptyIdxRight - 28) * 2;
                // else
                //     playerXPos = (notEmptyIdxLeft - 28) * 2;


                // print("edited" + playerXPos + "\nleft" + notEmptyIdxLeft + "  right" + notEmptyIdxRight);



                // Vector3 tempPos =  new Vector3(player.position.x - playerOffset.x + cameraTransform.position.x,
                //                                         player.position.y - playerOffset.y + cameraTransform.position.y,
                //                                         player.position.z);
                
                // player.transform.position = new Vector3(playerXPos + cameraTransform.position.x,
                //                                         player.position.y - playerOffset.y + cameraTransform.position.y,
                //                                         player.position.z);

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
            progressSlider.UpdateSlider(currentIndex);
            
            currentSegment = roomList[currentIndex];
            
            yield return new WaitForSeconds(1.5f);
            print("what?: " + currentIndex);

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
            progressSlider.UpdateSlider(currentIndex);


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
