using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    //Settings
    private float damage_val;

    void Update () {

		if (Vector2.Distance(transform.position, Camera.main.transform.position) > 16f){
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(gameObject);
    }
    

    public float damage {
        set {
            damage_val = value;
        }
    }

}
