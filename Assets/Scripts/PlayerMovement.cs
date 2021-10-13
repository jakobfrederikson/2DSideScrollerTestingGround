using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    float moveInput;

    public float jumpForce;
    public float jumpTimeCounter;
    private float jumpTime;
    private bool canDoubleJump;
    public static bool hasDoubleJumpPowerUp;

    private bool onGround;
    public LayerMask whatIsGround;
    private bool isJumping;

    public Transform feetPos;
    private Vector2 boxSize;

    private bool hasPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        hasDoubleJumpPowerUp = false;
        jumpTime = jumpTimeCounter;
        rb = GetComponent<Rigidbody2D>();
        boxSize = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapBox(feetPos.position, boxSize, 0f, whatIsGround);

        // Flip character sprite depending on which direction they're running.
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // Player jump.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = Vector2.up * jumpForce;
                canDoubleJump = hasDoubleJumpPowerUp;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    isJumping = true;        
                    jumpTimeCounter = jumpTime;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.velocity = Vector2.up * jumpForce;
                }
            }
        }

        // Player can hold jump to go a little higher.
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Block player from jumping again if they release the space key.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}

