using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    const string FLIP_ANIMATION = "flipAnimation";
    const string JUMP_ANIMATION = "Jump";
    const string TURN_ON_COLLIDER = "turnOnCollider";
    const string FLIP_PLAYER = "FlipPlayer";
    const string FLIP_PARAMETER = "changeDirection";
    const float SWITCH_TIME = 1.0f;
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

    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public AnimationClip switchClip;
    public GameObject gear;

    void Update()
    {

        switchCoolDown -= Time.deltaTime;
        HandleInput();
        if (Input.GetKeyDown(KeyCode.DownArrow) && switchCoolDown <= 0)
        {
            StartCoroutine(FlipPlayer());
            switchCoolDown = 3;
        }
    }

    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        if (!isSwitching)
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
            //make a new vector3 which represent the velocity.
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
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag(FLIP_PLAYER))
    //     {
    //         StartCoroutine(FlipPlayer());
    //     }
    // }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == gear)
        {
            jumpState = JumpingStates.GROUNDED;
        }
    }

    IEnumerator FlipPlayer()
    {
        isSwitching = true;
        float ColliderY = playerCollider.size.y;
        float ColliderX = playerCollider.size.x;

        playerAnimator.SetBool(FLIP_PARAMETER, true);

        yield return new WaitForSeconds(.4f);
        playerCollider.size = new Vector2(ColliderX, ColliderY - 1.5f);
        yield return new WaitForSeconds(1);



        playerAnimator.SetBool(FLIP_PARAMETER, false);

        yield return new WaitForSeconds(switchClip.length * 2 - 1.8f);
        playerCollider.size = new Vector2(ColliderX, ColliderY);


        isSwitching = false;



    }








}