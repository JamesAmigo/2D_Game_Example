using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemPlayerController : MonoBehaviour
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

    //The canJump and doubleJump variables work kind of like skillpoints
    //The player can jump once and double jump once on Start
    //After jumping once, the player can't jump again until they touch the ground, however they can still double jump
    //After double jumping, the player can't dounble jump again until they touch the ground
    //When the player touches the ground, they can jump and double jump again
    //If you fall of a platform, you will not be able to jump, however you can still double jump
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

        if(rb.velocity.y < -1f)
        {
            Fall();
        }
        
    }

    //FixedUpdate is called once per physics update
    private void FixedUpdate() 
    {
        //Move the player horizontally according to the horizontal input we got from the Update function
        rb.velocity = new Vector2(horizontalInput * playerSpeed, rb.velocity.y);
    }

    //Input System Move Input Detection
    public void MoveEvent(InputAction.CallbackContext context)
    {
        //Get the horizontal input
        horizontalInput = context.ReadValue<Vector2>().x;
        FlipSprite();
    }
    //Input System Jump Input Detection
    public void JumpEvent(InputAction.CallbackContext context)
    {
        //Detect spacebar input and jump if grounded
        if(context.started && isGrounded && canJump)
        {
            Jump();
        }        
        else if(context.started && !isGrounded && canDoubleJump)
        {
            DoubleJump();
        }
    }

    //Get calls when the player is falling
    private void Fall()
    {
        //When you are falling, you can't jump again, no matter if you are falling from previous jump or falling from a platform
        //You can still double jump if you haven't double jumped yet
        canJump = false;
        //Set the isFalling parameter to true to trigger the falling animation
        anim.SetBool("isFalling", true);
    }

    //Gets called when the player jumps
    private void Jump()
    {
        //After jumping, the player can't jump again until they touch the ground
        canJump = false;
        //Set the isFalling parameter to false to stop the falling animation
        anim.SetBool("isFalling", false);
        //Trigger the isJumping parameter to trigger the jump animation
        anim.SetTrigger("isJumping");
        //We use velocity instead of AddForce to get a more consistent jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    //Gets called when the player double jumps
    private void DoubleJump()
    {
        //After double jumping, the player can't jump again until they touch the ground
        canDoubleJump = false;
        //Set the isFalling parameter to false to stop the falling animation
        anim.SetBool("isFalling", false);
        //Trigger the isDoubleJumping parameter to trigger the double jump animation
        anim.SetTrigger("isDoubleJumping");
        //We use velocity instead of AddForce to get a more consistent jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //GroundCheck is used to check if the player is on the ground
    private void GroundCheck()
    {
        if(isGrounded)
        {
            //If the player is on the ground, set the isFalling parameter to false to stop the falling animation
            anim.SetBool("isFalling", false);
            //If the player is on the ground, they can jump again
            canJump = true;
            //If the player is on the ground, they can double jump again
            canDoubleJump = true;
        }
        else
        {
            //If the player is not on the ground, they can't jump again but still got a chance of double jumping
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

    //OnDrawGizmosSelected is called when the object is selected in the editor
    private void OnDrawGizmosSelected() 
    {
        //This is used to draw the detectorRadius in the editor
        Gizmos.DrawWireSphere(transform.position, detectorRadius);
    }
}
