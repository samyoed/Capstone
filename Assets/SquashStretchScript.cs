using UnityEngine;

namespace ballGame
{
    public class SquashStretchScript : MonoBehaviour
    {

    	public float bias = 1f;
    	public float strength = 1f;


    	private Vector3 startScale;

    	private void Start ()
    	{
    		startScale = transform.localScale;
    	}

    	private void Update ()
    	{
    		var velocity = this.GetComponent<Player>().velocity.magnitude;

    		if (Mathf.Approximately (velocity, 0f))
    			return;

    		var amount = velocity * strength + bias;
    		var inverseAmount = (1f / amount) * startScale.magnitude;

    		transform.localScale = new Vector3 (amount, inverseAmount, 1f);
    		transform.localScale = new Vector3 (inverseAmount, amount, 1f);

    		
    	}
    }
}