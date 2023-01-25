using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Player properties
    public float movSpeed;
    public float jumpForce;
    public float flutterForce;
    public float moveDirection;

    private Rigidbody2D rb;

    //Sprite direction
    private bool facingRight = true;

    //Ground Check variables
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //Jump counters (unused)
    private int extraJumps;
    public int extraJumpValue;

    //Jump Display variables
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;

    //Flutter Display variables
    private float flutterTimeCounter;
    public float flutterTime;
    public bool isFlutter;

    public float rigidbodyVelocity;

    //Flutter and Jump bar sprites
    public Image flutterFill;
    public Image jumpFill;

    void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {   
        //Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Horizontal movement
        moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * movSpeed, rb.velocity.y);

        //Adjust player's sprite
        if (facingRight == false && moveDirection > 0)
        {
            flipSprite();
        }
        else if (facingRight == true && moveDirection < 0)
        {
            flipSprite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Reset extra jump counter when the player is touching the ground
        /*
        if(isGrounded == true)
        {
            extraJumps = extraJumpValue;
        }
        */

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        //Flutter Jump (when falling)
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            isFlutter = true;
            flutterTimeCounter = flutterTime;
            rb.AddForce(Vector2.up * flutterForce);
        }

        //Flutter Jump (mid Jump)
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }

        //Flutter Jump (mid Flutter Jump)
        if (Input.GetKey(KeyCode.Space) && isFlutter == true)
        {
            if (flutterTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * flutterForce);
                
                //Limit vertical velocity
                if(rb.velocity.y > 15)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 15);
                }

                flutterTimeCounter -= Time.deltaTime;
            }
            else
            {
                isFlutter = false;
            }
        }

        //Reset player status flags
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            isFlutter = false;
        }

        rigidbodyVelocity = rb.velocity.y;

        //Jump display
        if(isJumping)
        {
            jumpFill.fillAmount = 1 - jumpTimeCounter / jumpTime;
        }
        else
        {
            jumpFill.fillAmount = 0;
        }

        //Flutter Jump display
        if(isFlutter)
        {
            flutterFill.fillAmount = 1 - flutterTimeCounter / flutterTime;
        }
        else
        {
            flutterFill.fillAmount = 0;
        }
    }

    //Function to flip the Player's sprite according to the movement direction
    void flipSprite()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
