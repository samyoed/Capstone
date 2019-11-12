using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class SingleBullet : MonoBehaviour
    {
        public bool isPush;
        ParticleSystem partSys;

        void Awake()
        {
            partSys = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if(!coll.gameObject.CompareTag("Player"))
                StartCoroutine(particles());
        }



        IEnumerator particles()
        {
            partSys.Play();
            yield return new WaitForSeconds(.1f);
            partSys.Stop();


        }    
    }

    
}
