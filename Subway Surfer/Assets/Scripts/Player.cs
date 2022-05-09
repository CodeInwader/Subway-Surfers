using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum position { Midle, Left, Right }

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController Controller;
    public float RunSpeed;
    Vector3 move;
    Vector3 m_Velocity = Vector3.zero;
    Vector3 sideMove = Vector3.zero;
    position currentPosition = position.Midle;
    float NewXPos = 0f;
    public float XValue;
    private float x;
    public float swipeSpeed = 30f;
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
    bool stopTouch = false;

    [Header("Animations")]
    public Animator animator;

    [Header("Audio")]
    public AudioClip HitSound;
    




    // Start is called before the first frame update
    void Start()
    {
        move = transform.forward;
        transform.position = Vector3.zero;
       
    }

    // Update is called once per frame
   
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
         Swipe();
        // Gravitace 1/2 * g * t^2
        m_Velocity.y += gravity * Time.deltaTime;

        //Ground Check
        m_IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -2f;
        }

       

    }

    void Swipe()
    {
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) //Touch Input
        {
            startTouchPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0)) // Mouse Input
        {
            startTouchPosition = Input.mousePosition;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) //Touch Input
        {
            currentTouchPosition = Input.GetTouch(0).position;
            Vector2 moveDistance = currentTouchPosition - startTouchPosition;

            GetDir(moveDistance);
           
        }
        else if (Input.GetKey(KeyCode.Mouse0)) // Mouse Input
        {
            currentTouchPosition = Input.mousePosition;
            Vector3 moveDistance = currentTouchPosition - startTouchPosition;

            GetDir(moveDistance);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) //Touch Input
        {
            stopTouch = false;

            lastTouchPosition = Input.GetTouch(0).position;

            Vector2 distance = lastTouchPosition - startTouchPosition;

            if(Mathf.Abs(distance.x) < Touchrange && Mathf.Abs(distance.y) < Touchrange)
            {
                //Touch
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) // Mouse Input
        {
            stopTouch = false;

            lastTouchPosition = Input.mousePosition;

            Vector2 distance = lastTouchPosition - startTouchPosition;

            if (Mathf.Abs(distance.x) < Touchrange && Mathf.Abs(distance.y) < Touchrange)
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
               
                if (Physics.Raycast(RaycastPoint.transform.position, transform.TransformDirection(Vector3.right * -1), out hit, Mathf.Infinity)) //Detection if player can turn left
                {
                    
                    if (hit.transform.tag == "Barier")
                   {
                        canTurn = false;
                        
                   }
                    else
                   {
                        canTurn = true;
                   }
                }

                //Left
                if (canTurn)
                {
                    if (currentPosition == position.Midle)
                    {
                        currentPosition = position.Left;
                        NewXPos = -XValue;
                    }
                    else if (currentPosition == position.Right)
                    {
                        currentPosition = position.Midle;
                        NewXPos = 0;
                    }
                }
                
                Debug.Log("Left");
                animator.Play("MaxSneakers-Left");

                stopTouch = true;
            }
            else if (moveDistance.x > SwipeRange)
            {
                RaycastHit hit;

                if (Physics.Raycast(RaycastPoint.transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity)) //Detection if player can turn right
                {
                   
                    if (hit.transform.tag == "Barier")
                    {
                        canTurn = false;
                        
                    }
                    else
                    {
                        canTurn = true;
                    }
                }
                

                //Right
                if (canTurn)
                {
                    if (currentPosition == position.Midle)
                    {
                        NewXPos = XValue;
                        currentPosition = position.Right;
                    }
                    else if (currentPosition == position.Left && canTurn == true)
                    {
                        NewXPos = 0;
                        currentPosition = position.Midle;
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
                    m_Velocity.y = Mathf.Sqrt(jumpForce * -gravity );
                    animator.Play("MaxSneakers-JumpA");
                }

                stopTouch = true;
            }
            else if (moveDistance.y < -SwipeRange)
            {
                //Down

                if (!m_IsGrounded)
                {
                    m_Velocity.y = -50f;
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
        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * swipeSpeed);

        //Forward Move
        Controller.Move(RunSpeed * move * Time.deltaTime);

        //Gravitation, Up, Down
        Controller.Move(m_Velocity * Time.deltaTime);

        //Move left, right
        Controller.Move((x - transform.position.x) * Vector3.right);
    }


    public void EndOfSlide()
    {
        Controller.height = 0.94f;
        Controller.center = new Vector3(0, 0.49f, 0);
    }


}
