using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool isHit;
    public float mag = 25;
    public float magAdd;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        magAdd = player.GetComponent<Player>().currVel;
    }

     void OnCollisionEnter2D (Collision2D other)
     {

        if(other.gameObject.CompareTag("Player"))
        {
            //magAdd = player.GetComponent<Player>().currVel;

            var force = transform.position - other.transform.position;
 
            //magAdd = Vector3.Magnitude(other.gameObject.GetComponent<Rigidbody2D>().velocity);

            force.Normalize ();
            GetComponent<Rigidbody2D> ().AddForce (force * (magAdd), ForceMode2D.Impulse);

            print("hello?");
        }
     }




    void OnTriggerEnter2D (Collider2D other)
     {
         isHit = false;
     }
 
     void OnTriggerExit2D (Collider2D other)
     {
         isHit = true;
 
         var force = transform.position - other.transform.position;
 
         force.Normalize ();
         GetComponent<Rigidbody2D> ().AddForce (-force * mag, ForceMode2D.Impulse);
 
 
     }
}
