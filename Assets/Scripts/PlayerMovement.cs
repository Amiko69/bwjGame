using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string FLIP_ANIMATION = "flipAnimation";
    const string JUMP_ANIMATION_WHITE = "JumpAnimationWhite";
    const string JUMP_ANIMATION_BLACK = "JumpAnimationBlack";
    const string DEATH_ANIMATION_WHITE = "DeathAnimationWhite";
    const string DEATH_ANIMATION_BLACK = "DeathAnimationBlack";
    const string TURN_ON_COLLIDER = "turnOnCollider";
    const string FLIP_PLAYER = "FlipPlayer";
    const string FLIP_PARAMETER = "changeDirection";
    const float SWITCH_TIME = 1.167f / 2;
    const float GRAVITY = 30f;
    const float SPEED = .14f;
    const float JUMP_FORCE = 11f;

    float switchCoolDown = 0f;
    float deathCheckCoolDown = 0f;

    Vector2 lastPosition;

    bool isDownwards = false;
    public bool isSwitching = false;
    bool isPlayerDeath;

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
    
    private Gear currentGear;
    public LevelGenerator levelGenerator;

    void Update()
    {
        if (!isPlayerDeath)
        {
            switchCoolDown -= Time.deltaTime;
            deathCheckCoolDown -= Time.deltaTime;
            HandleInput();
            if (deathCheckCoolDown <= 0)
            {
                CheckDeath();
                deathCheckCoolDown = 0.1f;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isPlayerDeath)
        {
            HandleRotation();
            HandleMovement();
            HandleGravity();
        }
    }

    public void AssignGear(Gear newGear)
    {
        currentGear = newGear;
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
        float xdif = -currentGear.transform.parent.position.x + transform.position.x;
        float ydif = -currentGear.transform.parent.position.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 playerVelocityV3 = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y);

            if (jumpState != JumpingStates.DOUBLE_JUMP)
            {
                if (isDownwards)
                {
                    playerRigidbody.AddForce(-transform.up * JUMP_FORCE - playerVelocityV3, ForceMode2D.Impulse);
                    playerAnimator.Play(JUMP_ANIMATION_WHITE, 0, 0f);
                }
                else
                {
                    playerRigidbody.AddForce(transform.up * JUMP_FORCE - playerVelocityV3, ForceMode2D.Impulse);
                    playerAnimator.Play(JUMP_ANIMATION_BLACK, 0, 0f);
                }
                jumpState++;
                // currentGear = levelGenerator.NeareastGearFromPlayer();
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
        if (!isPlayerDeath)
        {
            currentGear.GetComponent<EdgeCollider2D>().enabled = false;
            if (isDownwards)
            {
                // RaycastHit2D hit;
                // hit = Physics2D.Raycast(transform.position, -transform.up);
                // if (hit.collider != null)
                // {
                //     Debug.DrawRay(transform.position, transform.up * hit.distance, Color.red);
                //     Debug.Log("Did Hit " + hit.collider.transform.position);
                //     transform.Translate(hit.collider.transform.position + transform.up * 2f);
                // }
                transform.Translate(transform.up * 3f);
            }
            else
            {
                // RaycastHit2D hit;
                // hit = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, 8);
                // if (hit.collider != null)
                // {
                //     Debug.Log("Did Hit " + hit.collider.name + " " + hit.collider.transform.position);
                //     transform.Translate((Vector3)hit.collider.ClosestPoint(transform.position) - transform.up * 2f);
                // }
                transform.Translate(-transform.up * 3f);
            }
            playerSpriteRenderer.flipY = !playerSpriteRenderer.flipY;   
            isDownwards = !isDownwards;
            currentGear.GetComponent<EdgeCollider2D>().enabled = true;
            playerAnimator.SetBool(FLIP_PARAMETER, false);
            isSwitching = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Portal")
        {
            AssignGear(levelGenerator.NextGear());
            col.gameObject.name = "UsedPortal";
        }
    }

    void CheckDeath()
    {
        Debug.Log(Vector2.Distance(transform.position, lastPosition));
        if (Vector2.Distance(transform.position, lastPosition) < 0.1f)
        {
            if (isDownwards)
            {
                playerAnimator.Play(DEATH_ANIMATION_BLACK);
            }
            else
            {
                playerAnimator.Play(DEATH_ANIMATION_WHITE);
            }
            isPlayerDeath = true;
        }
        lastPosition = transform.position;
    }
}