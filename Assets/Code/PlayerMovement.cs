using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{

    private float horizontal;
    public float speed = 8f;
    public float jumpForce = 16f;
    //public float gravityModifier;
    private bool facingRight = true;

    public bool isJumping = false;
    public bool isGrounded = true;
    public int fallMultiplier = 3;
    public float movingJumpBonus = 2f;
    

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
    }

    /*private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, groundLayer);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checking if on the ground or on an animal
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Animal"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void Flip()
    {
        if (facingRight && horizontal < 0f || !facingRight && horizontal > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //Player Jump

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            float extraJump = Mathf.Abs(horizontal) > 0 ? movingJumpBonus : 0f;
            isJumping = true;
            isGrounded = false;
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce + extraJump);
        }

        if (Input.GetKeyUp(KeyCode.Space) && playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
        }

        if (playerRb.velocity.y < 0f)
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }


        /*if (Input.GetKeyUp(KeyCode.Space) && playerRb.velocity.y > 0f)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
        }*/

    }
}
