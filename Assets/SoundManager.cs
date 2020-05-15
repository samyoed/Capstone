using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip jumpSound, bounceSound, hitSound;
    public static AudioSource audioSource;




    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Jump2");
        bounceSound = Resources.Load<AudioClip>("Hit_Hurt7");
        hitSound = Resources.Load<AudioClip>("Hit_Hurt8");

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
public static void PlaySound (string clip)
{
    switch(clip)
    {
        case "jump":
            audioSource.PlayOneShot(jumpSound);
        break;
        case "bounce":
            audioSource.PlayOneShot(bounceSound);
        break;
        case "hit":
            audioSource.PlayOneShot(hitSound);
        break;
    }
}

}
