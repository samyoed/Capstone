using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame{
    public class GhostScript : MonoBehaviour
    {
        public float ghostDelay;
        private float ghostDelaySeconds;
        public GameObject ghost;
        public bool makeGhost;

        // Start is called before the first frame update
        void Start()
        {
            ghostDelaySeconds = ghostDelay;
        }

        // Update is called once per frame
        void Update()
        {
            if(GetComponent<Player>().isDashing)
            {
                if(ghostDelaySeconds > 0)
                {
                    ghostDelaySeconds -= Time.deltaTime;
                }
                else
                {
                    //Generate a ghost
                    ghostDelaySeconds = ghostDelay;
                    GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                    Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                    currentGhost.transform.localScale = transform.localScale;
                    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                    Destroy(currentGhost, 1f);
                }
            }
        }
    }
}
