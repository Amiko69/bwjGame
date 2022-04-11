using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string FLIP_ANIMATION = "flipAnimation";
    const string JUMP_ANIMATION = "Jump";
    const string TURN_ON_COLLIDER = "turnOnCollider";
    const string FLIP_PLAYER = "FlipPlayer";
    const string FLIP_PARAMETER = "changeDirection";
    const float SWITCH_TIME = 1.167f;
    const float GRAVITY = 30f;
    const float SPEED = .14f;
    const float JUMP_FORCE = 11f;
    float switchCoolDown = 0f;

    bool isDownwards = false;
    public bool isSwitching = false;

    public enum JumpingStates
    {
        GROUNDED,
        ONE_JUMP,
        DOUBLE_JUMP
    }
    public JumpingStates jumpState = JumpingStates.GROUNDED;

    public SpriteRenderer playerSpriteRenderer;
    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public AnimationClip switchClip;
    // public Gear currentGear;
    public GameObject currentGear;

    void Update()
    {
        switchCoolDown -= Time.deltaTime;
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }

    // public void AssignGear(Gear newGear)
    // {
    //     currentGear = newGear;
    // }

    void HandleMovement()
    {
        // transform.position += transform.right * SPEED;
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
        float xdif = -currentGear.transform.position.x + transform.position.x;
        float ydif = -currentGear.transform.position.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 playerVelocityV3 = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y);

            if (jumpState != JumpingStates.DOUBLE_JUMP)
            {
                if (isDownwards)
                {
                    playerRigidbody.AddForce(-transform.up * JUMP_FORCE - playerVelocityV3, ForceMode2D.Impulse);
                }
                else
                {
                    playerRigidbody.AddForce(transform.up * JUMP_FORCE - playerVelocityV3, ForceMode2D.Impulse);
                }
                jumpState++;
                playerAnimator.Play(JUMP_ANIMATION, 0, 0f);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && switchCoolDown <= 0)
        {
            StartCoroutine(FlipPlayer());
            switchCoolDown = 3;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == currentGear.gameObject)
        {
            jumpState = JumpingStates.GROUNDED;
        }
    }

    IEnumerator FlipPlayer()
    {
        // Change assigned gear if in portal

        isSwitching = true;
        playerAnimator.SetBool(FLIP_PARAMETER, true);    
        yield return new WaitForSeconds(SWITCH_TIME);
        currentGear.GetComponent<EdgeCollider2D>().enabled = false;
        if (isDownwards)
        {
            transform.Translate(transform.up * 2f);
            playerSpriteRenderer.flipY = !playerSpriteRenderer.flipY;
        }
        else
        {
            transform.Translate(-transform.up * 2f);
            playerSpriteRenderer.flipY = !playerSpriteRenderer.flipY;
        }
        isDownwards = !isDownwards;
        currentGear.GetComponent<EdgeCollider2D>().enabled = true;
        playerAnimator.SetBool(FLIP_PARAMETER, false);
        isSwitching = false;
    }
}