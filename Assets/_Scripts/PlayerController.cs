using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace spaceCadet
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float jumpHeight = 2f;
        public float dashStrength = 2f;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;

        private Rigidbody2D _body;
        public Vector2 _inputs = Vector2.zero;
        public bool _isGrounded = false;
        public bool _isFacingRight = true;
        public bool canDash = true;

        public int jumpCount = 0;
        public GameObject gun;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            //------------------HORIZ INPUTS-------------------
            _inputs = Vector2.zero;
            _inputs.x = Input.GetAxisRaw("Horizontal");
            
            //Direction Check
            if(_body.velocity.x > 0)
                _isFacingRight = true;
            else if(_body.velocity.x < 0)
                _isFacingRight = false;
            //----------------------JUMP-----------------------
            if (Input.GetButtonDown("Jump") && jumpCount < 3)
            {
                _body.AddForce(Vector3.up *jumpHeight, ForceMode2D.Impulse);
                jumpCount++;
            }
            if (_body.velocity.y < 0) //
			    _body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		    else if (_body.velocity.y > 0 && !Input.GetButton("Jump"))
			    _body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            //----------------------DASH---------------------------
            if(Input.GetButtonDown("Dash") && canDash == true)
            {
                Vector2 dashDirect;
                if(_isFacingRight)
                    dashDirect = Vector2.right;
                else
                    dashDirect = Vector2.left;

                _body.AddForce(dashDirect * dashStrength, ForceMode2D.Impulse);
                StartCoroutine(DashTimer());
            }
             
            if(Input.GetButtonDown("Fire1"))
            {
                gun.GetComponent<GunBehavior>().useWeapon();
            }



        }

        void FixedUpdate()
        {
            //--------------------HORIZ UPDATES---------------------------
            Vector2 newPos = new Vector2(_inputs.x * speed, 0);
            _body.AddForce(newPos);
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            //----------------------GROUND DETECTION-----------------------
            if (coll.gameObject.tag == "Ground")
            {
                _isGrounded = true;
                jumpCount = 0;
            }
        }

        void OnCollisionExit2D(Collision2D coll)
        {
            if(coll.gameObject.tag == "Ground")
            _isGrounded = false;
        }
        IEnumerator DashTimer()
        {
            canDash = false;
            yield return new WaitForSecondsRealtime(.5f);
            canDash = true;

        }
    }
}
