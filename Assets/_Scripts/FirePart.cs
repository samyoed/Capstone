﻿using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class FirePart : MonoBehaviour {
     
     public Transform effectPrefab;
     //public Camera mainCamera;
     private Transform _clone;
     private ParticleSystem _clone_ps;
     public ParticleSystem.EmissionModule _clone_ps_em;
     Quaternion currentRotation;
     
     void OnEnable () {
         //"Camera.main" is pricey, use it only when you run out of option
         
         _clone = Instantiate(effectPrefab);
         _clone.parent = transform;
         _clone.localPosition = Vector3.forward * 2f;

        currentRotation.eulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 90, transform.localEulerAngles.z);
         _clone.localRotation = currentRotation;
         _clone_ps = _clone.GetComponent<ParticleSystem>();
         _clone_ps.Play();
         _clone_ps_em = _clone_ps.emission;
         _clone_ps_em.enabled = false;
     }
     
     void Update () {
         if (Input.GetMouseButton(0)) {
             _clone_ps_em.enabled = true;
             //Calling ps.Play during Update will cause the effect to reset itself repeatedly.
         } else {
             _clone_ps_em.enabled = false;
         }
     }
 }