using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    DialogueManager DialogueManager;

    //paramaters
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float gravity = 1f;
    [SerializeField] float wallJumpXSpeed;
    [SerializeField] float wallJumpYSpeed;
    [SerializeField] float aerialDriftSpeed;
    [SerializeField] float wallSlideGravity;
    [SerializeField] float maxYVelocity = 30f;
    [SerializeField] float shortDriftThreshold;
    [SerializeField] float shortDriftSpeed;

    [SerializeField] Vector2 deathkick = new Vector2(5f, 5f);

    //public values
    public int knowledgeBytes = 0;

    //colliders
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D feetCollider;

    //states
    bool isAlive = true;
    public bool controllable = true;
    public Vector3 spawnPosition;

    //cached component references
    Rigidbody2D myRigidbody;
    Animator myAnimator;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        DialogueManager = FindObjectOfType<DialogueManager>();

        print("playerSpawnPosition: " + spawnPosition);
        spawnPosition = transform.position;
    }
    void Update()
    {
        if (!controllable)
        {
            myAnimator.SetBool("Running", false);
            return;
        }
        if (!isAlive || !controllable) { return; }
        Run();
        Drift();
        Jump();
        ClimbLadder();
        FlipSprite();
        WallJump();
        WallSlide();
        ClampVelocity();
        Die();
    }

    private void ClimbLadder()
    {
        if (!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidbody.gravityScale = gravity;
            return;
        }

        myRigidbody.velocity = new Vector2(climbSpeed * Input.GetAxis("Horizontal"), climbSpeed * Input.GetAxis("Vertical"));
        myRigidbody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", hasVerticalSpeed);
    }

    private void Run()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * runSpeed, myRigidbody.velocity.y);
            bool playerIsMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", playerIsMoving);
        }
    }

    private void Jump()
    {
        bool jumpButtonPressedAndTouchingGround = Input.GetButtonDown("Jump") && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (jumpButtonPressedAndTouchingGround)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        }
    }

    private void FlipSprite()
    {
        bool playerIsMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void WallJump()
    {
        bool canWallJump = Input.GetButtonDown("Jump") && bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (canWallJump)
        {
            print("walljump");
            myRigidbody.velocity = new Vector2(wallJumpXSpeed * Mathf.Sign(transform.localScale.x * -1f), wallJumpYSpeed);
        }
    }

    private void Drift()
    {
        BackDrift(); //used for making a jump shorter than it otherwise would be if the player couldn't control the jump once airborn
        ShortDrift(); //used for letting the player move slightly forward even when jumping from a standstill
    }

    private void BackDrift()
    {
        bool canDrift =
         !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && //is airborn
         (int)Mathf.Sign(Input.GetAxis("Horizontal")) != (int)Mathf.Sign(myRigidbody.velocity.x); //player is holding direction opposite to which way the character is going

        if (canDrift)
        {
            Vector3 driftForce = new Vector3(aerialDriftSpeed * Input.GetAxis("Horizontal"), 0, 0);
            myRigidbody.AddForce(driftForce, ForceMode2D.Force);
            bool playerIsMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", playerIsMoving);
        }
    }

    private void ShortDrift()
    {
        //print(myRigidbody.velocity.x);
        bool canDrift =
            !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && //is airborn
            Mathf.Abs(myRigidbody.velocity.x) < shortDriftThreshold;//player is moving slowly enough to be able to use this
        if (canDrift)
        {
            Vector3 driftForce = new Vector3(shortDriftSpeed * Input.GetAxis("Horizontal"), 0, 0);
            myRigidbody.AddForce(driftForce, ForceMode2D.Force);
            bool playerIsMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", playerIsMoving);
        }
    }

    private void WallSlide()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && myRigidbody.velocity.y < 0)
        {
            myRigidbody.gravityScale = wallSlideGravity;
        }
    }

    private void ClampVelocity()
    {
        //print(myRigidbody.velocity.y);
        if(myRigidbody.velocity.y < -maxYVelocity)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -maxYVelocity);
        }
    }

    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            myRigidbody.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }

    }

    public void SetCheckpoint(Vector3 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        isAlive = true;
        myAnimator.SetTrigger("Respawn");
    }

    public void SetupSpawnPosition(Vector3 vector)
    {
        spawnPosition = vector;
        transform.position = spawnPosition;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //print(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //print(collision);
    }
}
