using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // used to add parallax to background
    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;

    public float backgroundSize;
    public float parallaxSpeed;
    public float zPos;

    
    private void Start()
    {
           
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for(int k = 0; k < transform.childCount; k++)
        {
            layers[k] = transform.GetChild(k);
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1;


    }

    private void Update()
    {
        float diffX = cameraTransform.position.x - lastCameraX;
        transform.position += Vector3.right * (diffX * parallaxSpeed);
        lastCameraX = cameraTransform.position.x;

        transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            ScrollLeft();

        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            ScrollRight();

    }



    private void ScrollLeft() // call if going left
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = new Vector3(layers[leftIndex].position.x - backgroundSize, transform.position.y, zPos);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    private void ScrollRight() // call if going right
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = new Vector3(layers[rightIndex].position.x + backgroundSize, transform.position.y, zPos);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }




}