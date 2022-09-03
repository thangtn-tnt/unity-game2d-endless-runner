using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region components
    private Rigidbody2D rigidBody;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Coroutine hurtAnimCoroutine;
    #endregion    

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float knockbackPower;

    private bool canBeKnocked = true;
    private bool isKnocked;

    #region
    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveSpeed;

    private float initMoveSpeed;
    private float speedMiles;
    private float initSpeedIncreaseMiles;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedIncreaseMiles;

    private bool isRunning;

    private bool canRun;
    private bool canRoll;
    #endregion

    #region
    [Header("Jump Ability")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private float initJumpForce;

    private bool canDoubleJump;
    #endregion

    #region
    [Header("Slide Ability")]
    [SerializeField] private float slidingTime;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slideSpeedMultiplier;

    private float slidingBegunTime;

    private bool canSlide;

    private bool isSliding;
    #endregion


    #region
    [Header("Climb Ability")]
    [SerializeField] private Transform ledgeCheck;

    [SerializeField] private float ledgeClimb_Xoffset1 = 0f;
    [SerializeField] private float ledgeClimb_Yoffset1 = 0f;
    [SerializeField] private float ledgeClimb_Xoffset2 = 0f;
    [SerializeField] private float ledgeClimb_Yoffset2 = 0f;


    private Vector2 ledgePosBot;
    private Vector2 ledgePos1; // position to hold the player before animations end
    private Vector2 ledgePos2; // position where to move the player after animations end

    private bool canClimbLedge;

    #endregion

    #region
    [Header("Collision Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform bottomWallCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;

    private bool isGrounded;
    private bool isWallDetected;
    private bool isBottomWallDetected;
    private bool isCeilingDetected;
    private bool isTouchingLedge;
    private bool isLedgeDetected;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initPlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !isKnocked)
        {
            canRun = true;
        }

        checkForRun();
        checkForJump();
        checkForSlide();
        checkForSpeedingUp();
        checkForLedgeClimb();

        checkForCollisions();
        AnimationController();
    }

    private void initPlayerInfo()
    {        
        initMoveSpeed = moveSpeed;
        initJumpForce = jumpForce;
        initSpeedIncreaseMiles = speedIncreaseMiles;

        speedMiles = initSpeedIncreaseMiles;
    }

    private void AnimationController()
    {
        anim.SetFloat("xVelocity", rigidBody.velocity.x);
        anim.SetFloat("yVelocity", rigidBody.velocity.y);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimbLedge);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("canRoll", canRoll);
        anim.SetBool("isKnocked", isKnocked);

        if (rigidBody.velocity.y < -25)
        {
            canRoll = true;
        }
    }

    private void rollAnimationFinished()
    {
        canRoll = false;
    }

    private void knockbackAnimationFinished()
    {
        isKnocked = false;        
        canRun = true;
    }

    public void knockback()
    {
        if (canBeKnocked)
        {
            isKnocked = true;
            hurtVFX();
        }
    }

    private void checkForRun()
    {
        if (isKnocked & canBeKnocked)
        {
            canBeKnocked = false;
            canRun = false;
            rigidBody.velocity = knockbackDirection * knockbackPower;
        }
        if (canRun)
        {
            if (isBottomWallDetected || isWallDetected && !isSliding)
            {
                resetMoveSpeed();
            }
            else if (isSliding)
            {
                rigidBody.velocity = new Vector2(moveSpeed * slideSpeedMultiplier, rigidBody.velocity.y);
            }
            else
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            }
        }

        if (rigidBody.velocity.x > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void checkForJump()
    {
        if (Input.GetKeyDown("space") && !isKnocked)
        {
            if (isGrounded)
            {
                jumpForce = initJumpForce;
                canDoubleJump = true;
                jump();
            }
            else if (canDoubleJump)
            {
                jumpForce = doubleJumpForce;
                canDoubleJump = false;
                jump();
            }
        }
    }

    private void checkForSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && canSlide && isGrounded && rigidBody.velocity.x >= initMoveSpeed)
        {
            isSliding = true;
            canSlide = false;
            slidingBegunTime = Time.time;
        }

        if (Time.time > slidingBegunTime + slidingTime && !isCeilingDetected)
        {
            isSliding = false; // make sliding is over
        }

        if (Time.time > slidingBegunTime + slidingTime + slidingCooldown)
        {
            canSlide = true;
        }
    }

    private void checkForSpeedingUp()
    {
        if (transform.position.x > speedMiles)
        {
            speedMiles += speedIncreaseMiles;

            moveSpeed *= speedMultiplier;

            speedIncreaseMiles *= speedMultiplier;

            if (moveSpeed > maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
            }

        }
    }

    private void checkForLedgeClimb()
    {
        if (isLedgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            ledgePos1 = new Vector2(ledgePosBot.x + wallCheckDistance + ledgeClimb_Xoffset1,
                ledgePosBot.y + ledgeClimb_Yoffset1);
            ledgePos2 = new Vector2(ledgePosBot.x + wallCheckDistance + ledgeClimb_Xoffset2,
                            ledgePosBot.y + ledgeClimb_Yoffset2);
            canRun = false;
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    private void checkIfLedgeClimbFinished()
    {
        transform.position = ledgePos2; // player's place from his transform.position to ledgePos2
        canClimbLedge = false; // prevent to play ledge climb animation again
        canRun = true; // allows player to run again
        isLedgeDetected = false; // reset boolean isLedgeDetected
    }

    private void checkForCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isBottomWallDetected = Physics2D.Raycast(bottomWallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
        isCeilingDetected = Physics2D.Raycast(ceilingCheck.position, Vector2.up, wallCheckDistance + 0.5f, whatIsGround);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        #region ledgeCheckBugFix

        if (rigidBody.velocity.y < 0)
        {
            ledgeClimb_Yoffset1 = -1.15f;
        }
        else
        {
            ledgeClimb_Yoffset1 = -0.75f;
        }

        #endregion

        if (isWallDetected && !isTouchingLedge && !isLedgeDetected)
        {
            isLedgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    private void resetMoveSpeed()
    {
        moveSpeed = initMoveSpeed;
        speedIncreaseMiles = initSpeedIncreaseMiles;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance,
            wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance,
            ledgeCheck.position.y, ledgeCheck.position.z));

        Gizmos.DrawLine(bottomWallCheck.position, new Vector3(bottomWallCheck.position.x + wallCheckDistance,
                bottomWallCheck.position.y, bottomWallCheck.position.z));

        Gizmos.DrawLine(ceilingCheck.position, new Vector3(ceilingCheck.position.x, ceilingCheck.position.y + wallCheckDistance, ceilingCheck.position.z));
    }

    private IEnumerator hurtVFXRoutine()
    {
        Color originalColor = spriteRenderer.color;
        Color darkenColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.6f);
        
        spriteRenderer.color = darkenColor;

        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;        

        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = darkenColor;

        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = darkenColor;

        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = darkenColor;

        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(0.2f);

        canBeKnocked = true; //make player vulnerable again

        hurtAnimCoroutine = null; // stop coroutine
    }

    private void hurtVFX()
    {
        // stop activated coroutine before active new one
        if (hurtAnimCoroutine is not null) 
        {
            StopCoroutine(hurtAnimCoroutine);
        }

        // start coroutine with reference to it
        hurtAnimCoroutine = StartCoroutine(hurtVFXRoutine());
    }
}
