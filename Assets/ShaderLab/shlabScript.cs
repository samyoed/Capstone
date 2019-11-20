using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shlabScript : MonoBehaviour
{

    public GameObject filter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(slowMo());
        }

    }
    IEnumerator slowMo()
    {
        filter.GetComponent<shlab2>().reset();
        filter.GetComponent<shlab2>().isHit = true;
        yield return new WaitForSeconds(.08f);
        filter.GetComponent<shlab2>().reset();
        filter.GetComponent<shlab2>().isHit = false;
    }



}
