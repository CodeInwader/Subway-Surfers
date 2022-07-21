using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum position { Middle, Left, Right }

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController Controller;
    public float RunSpeed;
    Vector3 move;
    Vector3 velocity = Vector3.zero;
    Vector3 sideMove = Vector3.zero;
    position currentPosition = position.Middle;
    float newXPos = 0f;
    public float XValue;
    private float x;
    public float SwipeSpeed = 30f;
    bool canTurn;
    public GameObject RaycastPoint;
   

    [Header("Gravity/Jump")]
    private bool m_IsGrounded;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Swipe")]
    Vector2 startTouchPosition;
    Vector2 lastTouchPosition;
    Vector2 currentTouchPosition;
    public float SwipeRange;
    public float Touchrange;
    public float CheckDistance;
    bool stopTouch = false;

    [Header("Animations")]
    public Animator animator;


    [Header("Audio")]
    public AudioSource HitSound;
    public AudioSource SwipeSound;
    public AudioSource JumpAndSlide;
    public AudioSource Music;

    [Header("Script Reference")]
    public UIAndScoreManager Uiandscoremanager;


    public bool maxIsDeadOrPaused = false;
    int distanceCheck = 5;




    void Start()
    {
        move = transform.forward;
        transform.position = Vector3.zero;

        Application.targetFrameRate = 60;

        Music.Play();
    }

    
   
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Swipe();
        MoveLeftRight();

        // Gravitace 1/2 * g * t^2
        velocity.y += gravity * Time.deltaTime;

        //Ground Check
        m_IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (m_IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

       

    }

    void Swipe()
    {
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !maxIsDeadOrPaused) //Touch Input
        {
            startTouchPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !maxIsDeadOrPaused) // Mouse Input
        {
            startTouchPosition = Input.mousePosition;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !maxIsDeadOrPaused) //Touch Input
        {
            currentTouchPosition = Input.GetTouch(0).position;
            Vector2 moveDistance = currentTouchPosition - startTouchPosition;

            GetDir(moveDistance);
           
        }
        else if (Input.GetKey(KeyCode.Mouse0) && !maxIsDeadOrPaused) // Mouse Input
        {
            currentTouchPosition = Input.mousePosition;
            Vector3 moveDistance = currentTouchPosition - startTouchPosition;

            GetDir(moveDistance);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !maxIsDeadOrPaused) //Touch Input
        {
            stopTouch = false;

            lastTouchPosition = Input.GetTouch(0).position;

            Vector2 distance = lastTouchPosition - startTouchPosition;

            if(Mathf.Abs(distance.x) < Touchrange && Mathf.Abs(distance.y) < Touchrange && !maxIsDeadOrPaused)
            {
                //Touch
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && !maxIsDeadOrPaused) // Mouse Input
        {
            stopTouch = false;

            lastTouchPosition = Input.mousePosition;

            Vector2 distance = lastTouchPosition - startTouchPosition;

            if (Mathf.Abs(distance.x) < Touchrange && Mathf.Abs(distance.y) < Touchrange && !maxIsDeadOrPaused)
            {
                //Touch
            }
        }
    }

    void GetDir(Vector2 moveDistance)
    {
        if (!stopTouch)
        {
            if (moveDistance.x < -SwipeRange)
            {
                RaycastHit hit;
               
                if (Physics.Raycast(RaycastPoint.transform.position, transform.TransformDirection(Vector3.right * -1), out hit,  Mathf.Infinity)) //Detection if player can turn left
                {
                   
                    if(hit.distance < distanceCheck) //Kontroluji jestli je vzdalenost Raycastu jen pres jedno pole
                    {
                      if (hit.transform.tag == "Wagon" || hit.transform.tag == "Barier"){
                          canTurn = false;
                          HitSound.Play();

                        }
                        else
                        {
                         canTurn = true;
                        }

                    }
                    else
                    {
                        canTurn = true;

                    }

                }

                //Left
                if (canTurn)
                {
                    SwipeSound.Play();

                    if (currentPosition == position.Middle)
                    {
                        currentPosition = position.Left;
                        newXPos = -XValue;
                        
                    }
                    else if (currentPosition == position.Right)
                    {
                        currentPosition = position.Middle;
                        newXPos = 0;
                       
                    }
                }
                
                animator.Play("MaxSneakers-Left");

                stopTouch = true;
            }
            else if (moveDistance.x > SwipeRange)
            {
                RaycastHit hit;

                if (Physics.Raycast(RaycastPoint.transform.position, transform.TransformDirection(Vector3.right), out hit,  Mathf.Infinity)) //Detection if player can turn right
                {
                  
                    if(hit.distance < distanceCheck) //Kontroluji jestli je vzdalenost Raycastu jen pres jedno pole
                    {
                      if (hit.transform.tag == "Wagon" || hit.transform.tag == "Barier"){
                          canTurn = false;
                          HitSound.Play();

                        }
                        else
                        {
                         canTurn = true;
                        }

                    }
                    else
                    {
                        canTurn = true;

                    }
                }
                

                //Right
                if (canTurn)
                {
                    SwipeSound.Play();

                    if (currentPosition == position.Middle)
                    {
                        newXPos = XValue;
                        currentPosition = position.Right;
                       
                    }
                    else if (currentPosition == position.Left && canTurn == true)
                    {
                        newXPos = 0;
                        currentPosition = position.Middle;
                        
                    }
                }

                animator.Play("MaxSneakers-Right");

                Debug.Log("Right");
                stopTouch = true;
            }
            else if (moveDistance.y > SwipeRange)
            {
                //Up
                Debug.Log("Up");
                if (m_IsGrounded)
                {
                    JumpAndSlide.Play();
                    velocity.y = Mathf.Sqrt(jumpForce * -gravity );
                    animator.Play("MaxSneakers-JumpA");
                }

                stopTouch = true;
            }
            else if (moveDistance.y < -SwipeRange)
            {
                //Down

                JumpAndSlide.Play();

                if (!m_IsGrounded)
                {
                    velocity.y = -50f;
                    animator.Play("MaxSneakers-Scroll");
                    
                }
                else
                {
                    animator.Play("MaxSneakers-Scroll");
                }
                Controller.height = 0.5f;
                Controller.center = new Vector3(0, 0.28f, 0);

                stopTouch = true;
            }
        }
    }

    void Move()
    {
        

        //Forward Move
        Controller.Move(RunSpeed * move * Time.deltaTime);

        //Gravitation, Up, Down
        Controller.Move(velocity * Time.deltaTime);
       
    }

    void MoveLeftRight()
    {
        
        x = Mathf.Lerp(x, newXPos, 0.2f);

        //Move left, right
        Controller.Move((x - transform.position.x) * Vector3.right);
        
    }

    public void EndOfSlide()
    {
        Controller.height = 0.94f;
        Controller.center = new Vector3(0, 0.49f, 0);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Barier")
        {
           
            UIAndScoreManager.GameHasStarted = false;
            Die();
        }
        
        
    }

    void Die()
    {
       if(!maxIsDeadOrPaused)
        {
            maxIsDeadOrPaused = true;
            animator.Play("MaxDead");
            RunSpeed = 0;
            Music.Pause();
            Uiandscoremanager.Dead();
        
           
        }
        
        
    }
    
    public void Pause()
    {
        maxIsDeadOrPaused = true;
        Music.Pause();
    }

    public void Resume()
    {
        maxIsDeadOrPaused = false;
        Music.Play();
    }

   
}
