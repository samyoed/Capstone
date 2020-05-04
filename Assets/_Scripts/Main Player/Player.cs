using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace ballGame
{
    [RequireComponent(typeof(PlayerControl2))]
    public class Player : MonoBehaviour
    {
        public enum state
        {
            DASH, ATTACK, SPECIAL, DEFAULT
        }

        public state phase;

        public float maxJumpHeight = 4;
        public float timeToJumpApex = .4f;
        float accelerationTimeAirborne = .3f;
        float accelerationTimeGrounded = .1f;

        public float moveSpeed = 6;

        float gravity;

        Vector3 velocity;
        float velocityXSmoothing;

        //moveDash variables
        float dashTimer = 0;
        public float dashTimerMax = 10f;
        public float dashSpeed = 90;
        public float dashTempSpeed = 0;

        float attackTimer = 0;
        public float attackTimerMax = 10f;
        public float attackSpeed = 0;
        public float attackTempSpeed = 0;

        public int currentDashCount = 0;
        private int dashNum = 2;

        float specialTimer = 0;
        public float specialTimerMax = 10;

        //attackdash Variables
        public Vector2 input;
        public direction dashDirec;

        private float normGravTemp;

        public float currVel;
        public Vector3 prevPos; 

        // custom controls
        public int playerID;
        private Rewired.Player playInput;
        public string horizInput;
        public string vertInput;
        public string actionInput;
        public string attackInput;
        public string specialInput;

        public Color defColor;
        public Color dashColor;

        bool isFacingRight;
        Vector3 currentScale;

        //for hit physics
        public float xEdit;
        public float yEdit;
        public float editAccel = 1;
        public Vector3 edit;
        public Vector3 tempEdit;
        //controls how strong the bullets are
        public float bulletMult;

        //for ghost behind player when dashing
        public GhostScript ghost;

        public float ballShootDiminisher;
        public float ballShootDiminisherTemp;

        public bool hasSpecial = false;

        public Animator _animator;

        //component references
        private CircleCollider2D _PlayerCircColl;
        private CapsuleCollider2D _PlayerCapsuleColl;
        
        private PlayerControl2 _Controller;
        private Special _Special;
        private SpriteRenderer _Sr;
        public ParticleSystem _PartSys;
        public GameObject _Ball;
        public GameManagerNew _gameManager;


        public Vector2 lockedDashDirec;
        int tempAttackInt = 0;

        public bool canMove;


        void Awake()
        {
            playInput = ReInput.players.GetPlayer(playerID);
        }

        void Start()
        {
            _Controller = GetComponent<PlayerControl2>();
            _Sr = GetComponent<SpriteRenderer>();
            _PlayerCircColl = transform.GetChild(1).GetChild(0).GetComponent<CircleCollider2D>();
            _PlayerCapsuleColl = transform.GetChild(1).GetChild(0).GetComponent<CapsuleCollider2D>();
            _Special = GetComponent<Special>();
            _PartSys = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
            _Ball = GameObject.Find("Ball");
            _animator = GetComponent<Animator>();
            _gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManagerNew>();

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            normGravTemp = gravity;
            currentScale = transform.localScale;
            
            //initialize timers
            dashTimer = 100;
            attackTimer = 100;

            //initialize phase
            phase = state.DEFAULT;
        }

        void Update()
        {
//-----------------------------RUN BEFORE EVERY FRAME----------------------------------
            if(_gameManager.gameState == GameManagerNew.GameState.PAUSED ||
               _gameManager.gameState == GameManagerNew.GameState.COUNTING ||
               _gameManager.gameState == GameManagerNew.GameState.GAMEOVER)
                canMove = false;
            else
                canMove = true;


            if(input.x < -0.01)
                isFacingRight = false;
            else if(input.x > 0.01)
                isFacingRight = true;

            if(isFacingRight)
                transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
            if(!isFacingRight)
                transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);

            // if(hasSpecial)
            //    _PartSys.Play();

            if(currentDashCount == 0)
                _Sr.color = Color.grey;
            else 
                _Sr.color = Color.white;

            //input
            if(canMove)
                input = new Vector2(playInput.GetAxisRaw("Horizontal"), playInput.GetAxisRaw("Vertical"));

//-----------------------------STATE MACHINE-------------------------------------------
            switch(phase)
            {   //----------------------------------ATTACKING---------------------------
                case state.ATTACK:

                    attackTimer ++;
                    velocity = (new Vector3(lockedDashDirec.x, lockedDashDirec.y, 0)) * attackSpeed;

                    gravity = 0;
                    if(attackTimer > attackTimerMax)
                    {
                        phase = state.DEFAULT;
                        gravity = normGravTemp;
                        //currentDashCount--;
                        _PlayerCircColl.enabled = true;
                        _PlayerCapsuleColl.enabled = false;
                        tempAttackInt = 0;

                        break;
                    }
                    break;
                //----------------------------------DASHING-----------------------------
                case state.DASH:
                    
                    dashTimer ++;
                    velocity = (new Vector3(lockedDashDirec.x, lockedDashDirec.y, 0)) * dashSpeed;

                    gravity = 0;
                    if(dashTimer > dashTimerMax)
                    {
                        phase = state.DEFAULT;
                        gravity = normGravTemp;
                        currentDashCount--;
                        break;
                    }
                break;
                //-----------------------------------SPECIAL-----------------------------
                case state.SPECIAL:

                    specialTimer ++;
                    _Special.activateSpecial();

                    if(specialTimer > specialTimerMax)
                    {
                        phase = state.DEFAULT;
                        hasSpecial = false;
                        break;
                    }
                    break;
                //-----------------------------------DEFAULT-----------------------------
                case state.DEFAULT:
                     //dashes when button is pressed and there are dashes available
                    if(Input.GetButtonDown(actionInput) && currentDashCount > 0 && canMove)
                    {
                        lockedDashDirec = input.normalized;
                        phase = state.DASH;
                        dashTimer = 0;
                    }
                    if(Input.GetButtonDown(attackInput) && canMove)
                    {
                        lockedDashDirec = input.normalized;
                        phase = state.ATTACK;
                        attackTimer = 0;
                        Vector3 circCollRot = Vector3.zero;

                        _PlayerCapsuleColl.size = new Vector2(6f,10f);

                        Vector2 roundedInput = new Vector2(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));

                        //for 
                        if(roundedInput.x == 0 && roundedInput.y == 0 && isFacingRight)
                        {
                            circCollRot.z = -90f;       //if no direction inputted
                            tempAttackInt = 3;
                        }
                        else if(roundedInput.x == 0 && roundedInput.y == 0 && !isFacingRight)
                        {    
                            circCollRot.z = 90f;
                            tempAttackInt = 3;
                        }
                        else if(roundedInput.x == 0 && roundedInput.y == 1)
                        {
                            circCollRot.z = 0f;          //if up input
                            tempAttackInt = 1;
                        }    
                        else if(roundedInput.x == 1 && roundedInput.y == 1)
                        {
                            circCollRot.z = -45f;     //if up right input
                            tempAttackInt = 2;
                        }
                        else if(roundedInput.x == 1 && roundedInput.y == 0)
                        {
                            circCollRot.z = -90f;      //if right input
                            tempAttackInt = 3;
                        }
                        else if(roundedInput.x == 1 && roundedInput.y == -1)
                        {
                            circCollRot.z = -135f;   //if down right input
                            tempAttackInt = 4;
                        }
                        else if(roundedInput.x == 0 && roundedInput.y == -1)
                        {
                            circCollRot.z = -180f;        //if down input
                            tempAttackInt = 5;
                        }
                        else if(roundedInput.x == -1 && roundedInput.y == -1)
                        {
                            circCollRot.z = -225f;    //if down left input
                            tempAttackInt = 4;
                        }
                        else if(roundedInput.x == -1 && roundedInput.y == 0)
                        {
                            circCollRot.z = -270f;        //if left input
                            tempAttackInt = 3;
                        }
                        else if(roundedInput.x == -1 && roundedInput.y == 1)
                        {
                            circCollRot.z = -315f;      //if up left input
                            tempAttackInt = 2;
                        }
                        _PlayerCircColl.transform.eulerAngles = circCollRot;
                        _PlayerCapsuleColl.enabled = true;
                        _PlayerCircColl.enabled = false;
                    }
                    if(Input.GetButtonDown(specialInput) && hasSpecial && canMove)
                    {
                        phase = state.SPECIAL;
                        specialTimer = 0;
                    }
        
                    //regain dashes once colliding with the ground
                    if(_Controller.collisions.below)
                        currentDashCount = dashNum;

                        //smooths x movement and overall x movement
                    float targetVelocityX = Mathf.Round(input.x) * moveSpeed;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, 
                                                      ref velocityXSmoothing,
                                                     (_Controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

                    velocity.y += gravity *(1/60f)*Time.timeScale;
                break;
            }


//-----------------------------RUN AFTER EVERY FRAME-------------------------------------

            if((_Controller.collisions.above || _Controller.collisions.below))
            {       
                velocity.y = 0;
            }
            
                
            _Controller.Move(velocity *(1/60f)*Time.timeScale);

//----------------------------ANIMATIONS-------------------------------------------------
            _animator.SetFloat("Speed", Mathf.Abs(input.x));
            _animator.SetFloat("Y Velocity", velocity.y);
            _animator.SetBool("OnGround", _Controller.collisions.below);
            _animator.SetInteger("AtkDirection", tempAttackInt);




        }

        void FixedUpdate()
        {
            //turn on trail when dashing
            dashTrail();
            currentDashDirec();
            //ballShootDiminisherTemp = Vector3.Distance(transform.position, ball.transform.position);
        }

        void OnTriggerExit2D (Collider2D other)
        {  
        //     if(other.gameObject.CompareTag("Weapon") && other.gameObject.GetComponent<SingleBullet>().currentPlayer != gameObject)
        //     {
        //         var force = transform.position - other.transform.position;
        //         float str = other.gameObject.GetComponent<SingleBullet>().strength;
    
        //         force.Normalize ();

		// 	    int pushPull;
		// 	    if(other.gameObject.GetComponent<SingleBullet>().isPush)
		// 	    	pushPull = -1;
		// 	    else
		// 	    	pushPull = 1;

        //         Vector3 bulletVec = (pushPull * force*2);
        //         xEdit = bulletVec.x;
        //         yEdit = bulletVec.y;
        //         currentLerpTime = 0;
        //         print("hello");

        //         tempEdit = new Vector3(xEdit, yEdit, 0);
		//    }
        }

        // void dashDimSet()
        // {
        //     if(ballShootDiminisherTemp < circleDiameter/3)
        //         ballShootDiminisher = 1;
        //     if(circleDiameter/3 < ballShootDiminisherTemp && ballShootDiminisherTemp < circleDiameter/(2/3))
        //         ballShootDiminisher = 2;
        //     if(ballShootDiminisher > circleDiameter/(2/3))
        //         ballShootDiminisher = 3;
        // }

        // void calculateDirec()
        // {
        //     switch(direc)
        //     {
        //         case direction.up:
        //         input.x = 0; input.y = 1;
        //             break;
        //         case direction.upRight:
        //         input.x = 1; input.y = 1;
        //             break;
        //         case direction.right:
        //         input.x = 1; input.y = 0;
        //             break;
        //         case direction.downRight:
        //         input.x = 1; input.y = -1;
        //             break;
        //         case direction.down:
        //         input.x = 0; input.y = -1;
        //             break;
        //         case direction.downLeft:
        //         input.x = -1; input.y = -1;
        //             break;
        //         case direction.left:
        //         input.x = -1; input.y = 0;
        //             break;
        //         case direction.upLeft:
        //         input.x = -1; input.y = 1;
        //             break;
        //         default:
        //             break;
        //     }      
        // }
        //for ball velocity
        void currentDashDirec()
        {
            if(input.x == 0 && input.y == 0)
                dashDirec = direction.none;        //if no direction inputted
            else if(input.x == 0 && input.y == 1)
                dashDirec = direction.up;          //if up input
            else if(input.x == 1 && input.y == 1)
                dashDirec = direction.upRight;     //if up right input
            else if(input.x == 1 && input.y == 0)
                dashDirec = direction.right;       //if right input
            else if(input.x == 1 && input.y == -1)
                dashDirec = direction.downRight;   //if down right input
            else if(input.x == 0 && input.y == -1)
                dashDirec = direction.down;        //if down input
            else if(input.x == -1 && input.y == -1)
                dashDirec = direction.downLeft;    //if down left input
            else if(input.x == -1 && input.y == 0)
                dashDirec = direction.left;        //if left input
            else if(input.x == -1 && input.y == 1)
                dashDirec = direction.upLeft;      //if up left input
        }
        void dashTrail()
        {
            if(phase == state.DASH)
            {
                _PartSys.Play();
            }
            else 
            {
                _PartSys.GetComponent<ParticleSystem>().Stop();
            }
        }

    }
}
