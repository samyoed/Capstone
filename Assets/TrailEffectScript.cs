
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffectScript : MonoBehaviour {

	//Settings
    private int delay;
    private GameObject trail_main;
    private GameObject[] trail;

	void Awake () {
        delay = 0;
		setEffect(5, 0.65f);
	}
	
	void Update () {
        if (delay <= 0){
		    for (int i = trail.Length - 1; i > 0; i--){
                trail[i].transform.position = new Vector3(trail[i - 1].transform.position.x, trail[i - 1].transform.position.y, trail[i - 1].transform.position.z);
                trail[i].transform.eulerAngles = new Vector3(trail[i - 1].transform.eulerAngles.x, trail[i - 1].transform.eulerAngles.y, trail[i - 1].transform.eulerAngles.z);
            }
            trail[0].transform.position = transform.position;
            trail[0].transform.eulerAngles = transform.eulerAngles;
            delay = 0;
        }
        else {
            delay--;
        }
	}

    void OnDestroy() {
        if (trail_main != null){
            Destroy(trail_main.gameObject);
        }
    }

    public void setEffect(int trail_length, float shrink_multiplier){
        trail = new GameObject[trail_length];

        float alpha = 1f;
        GameObject trail_effect_main = new GameObject("trail_effect");
        for (int i = 0; i < trail.Length; i++){
            GameObject trail_effect_obj = new GameObject("trail_effect_" + i);
            trail_effect_obj.transform.position = transform.position;
            trail_effect_obj.transform.eulerAngles = transform.eulerAngles;
            trail_effect_obj.transform.localScale = new Vector3(alpha * 6, alpha, 1f);
            trail_effect_obj.transform.parent = trail_effect_main.transform;

            trail_effect_obj.AddComponent<SpriteRenderer>();
            trail_effect_obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            trail_effect_obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
            alpha *= shrink_multiplier;
            trail[i] = trail_effect_obj;
        }

        if (trail_main != null){
            Destroy(trail_main.gameObject);
        }
        trail_main = trail_effect_main;
    }

}
