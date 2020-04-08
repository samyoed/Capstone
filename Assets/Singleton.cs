using UnityEngine;
using System.Collections;

public class Singleton: MonoBehaviour
{
    public static Singleton instance;
    public bool isPersistant;
    
    public virtual void Awake() 
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
        
    }
}