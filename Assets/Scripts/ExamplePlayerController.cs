using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayerController : MonoBehaviour
{
    //Design Variables
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float detectorRadius = 0.15f;
    [SerializeField] private AudioSource deathSound;


    //Component Varaibles
    private Rigidbody2D rb;
    private Animator anim;

    //Holder Variables
    private float horizontalInput;
    private bool isGrounded;
    private bool canJump;
    private bool canDoubleJump;


    // Start is called before the first frame update
    private void Start()
    {
        //Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        //Get the animator component
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("canDoubleJump: " + canDoubleJump);
        //Check for grounded every frame
        GroundCheck();
        //Get the horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        FlipSprite();
        if(rb.velocity.y < -1f)
        {
            Fall();
        }
        //Detect spacebar input and jump if grounded
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Jump();
        }        
        else if(Input.GetKeyDown(KeyCode.Space) && !isGrounded && canDoubleJump)
        {
            DoubleJump();
        }
    }

    //FixedUpdate is called once per physics update
    private void FixedUpdate() 
    {
        //Move the player horizontally according to the horizontal input we got from the Update function
        rb.velocity = new Vector2(horizontalInput * playerSpeed, rb.velocity.y);
    }

    private void Fall()
    {
        canJump = false;
        anim.SetBool("isFalling", true);
    }

    //We use Add force to jump
    private void Jump()
    {
        canJump = false;
        anim.SetBool("isFalling", false);
        anim.SetTrigger("isJumping");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        Debug.Log("Double Jump");
        canJump = false;
        canDoubleJump = false;
        anim.SetBool("isFalling", false);
        anim.SetTrigger("isDoubleJumping");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //GroundCheck is used to check if the player is on the ground
    private void GroundCheck()
    {
        //The isJumping parameter is used to trigger the jump animation
        //It will be opposite of the isGrounded parameter
        if(isGrounded)
        {
            anim.SetBool("isFalling", false);
            canJump = true;
            canDoubleJump = true;
        }
        else
        {
            canJump = false;
        }
        
        //Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(transform.position, detectorRadius, LayerMask.GetMask("Ground"));
        
    }

    //Flip the sprite according to the horizontal input
    private void FlipSprite()
    {
        //if the horizontal input is positive, flip the sprite to the right
        if(horizontalInput > 0)
        {
            //Set the isRunning parameter to true to trigger the running animation
            anim.SetBool("isRunning", true);
            //Set the localScale to 1,1,1 to flip the sprite to the right
            transform.localScale = new Vector3(1, 1, 1);
        }
        //if the horizontal input is negative, flip the sprite to the left
        else if(horizontalInput < 0)
        {
            //Set the isRunning parameter to true to trigger the running animation
            anim.SetBool("isRunning", true);
            //Set the localScale to -1,1,1 to flip the sprite to the left
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //Set the isRunning parameter to false to stop the running animation
            anim.SetBool("isRunning", false);
        }
    }

    //This function is called when the player dies
    public void PlayerDeath()
    {
        deathSound.Play();
        this.enabled = false;
    }

    //This function is called when the player wins
    public void PlayerWin()
    {
        this.enabled = false;
    }



    //OnDrawGizmosSelected is called when the object is selected in the editor
    private void OnDrawGizmosSelected() 
    {
        //This is used to draw the detectorRadius in the editor
        Gizmos.DrawWireSphere(transform.position, detectorRadius);
    }
}
