using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string FLIP_ANIMATION = "flipAnimation";
    const string JUMP_ANIMATION = "Jump";
    const string TURN_ON_COLLIDER = "turnOnCollider";
    const string FLIP_PLAYER = "FlipPlayer";
    const float SWITCH_TIME = 1.0f;
    const float GRAVITY = 30f;
    const float SPEED = .14f;
    const float JUMP_FORCE = 10f;

    bool isDownwards = false;
    
    public enum JumpingStates
    {
        GROUNDED,
        ONE_JUMP,
        DOUBLE_JUMP
    }
    public JumpingStates jumpState = JumpingStates.GROUNDED;

    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public GameObject gear;

    void Update ()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        transform.position += transform.right * SPEED;
    }

    void HandleGravity()
    {
        if (isDownwards)
        {
            playerRigidbody.AddForce(transform.up * GRAVITY);
        }
        else
        {
            playerRigidbody.AddForce(-transform.up * GRAVITY);
        }
    }

    void HandleRotation()
    {
        float xdif = -gear.transform.position.x + transform.position.x;
        float ydif = -gear.transform.position.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpState != JumpingStates.DOUBLE_JUMP)
            {
                if (isDownwards)
                {
                    playerRigidbody.AddForce(-transform.up * JUMP_FORCE, ForceMode2D.Impulse);
                }
                else
                {
                    playerRigidbody.AddForce(transform.up * JUMP_FORCE, ForceMode2D.Impulse);
                }
                jumpState++;
                playerAnimator.Play(JUMP_ANIMATION, 0, 0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(FLIP_PLAYER))
        {
            StartCoroutine(FlipPlayer());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == gear)
        {
            jumpState = JumpingStates.GROUNDED;
        }
    }

    IEnumerator FlipPlayer()
    {
        playerAnimator.SetBool(FLIP_ANIMATION, true);
        yield return new WaitForSeconds(SWITCH_TIME);
        isDownwards = !isDownwards;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
        transform.Translate(transform.up * 10);
        // if (isDownwards)
        // {
        //     playerRigidbody.AddForce(-transform.up * 10, ForceMode2D.Impulse);
        // }
        // else
        // {
        //     playerRigidbody.AddForce(transform.up * 10, ForceMode2D.Impulse);
        // }
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider2D>().enabled = true;
        playerAnimator.SetBool(FLIP_ANIMATION, false);
    }
}