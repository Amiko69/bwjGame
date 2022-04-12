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

    float SWITCH_COOLDOWN = 1f;
    float DEATH_COOLDOWN = 0.1f;
    float WALK_COOLDOWN = 0.3f;

    float switchCurrentCoolDown = 0f;
    float deathCurrentCheckCoolDown = 0f;
    float walkCurrentCoolDown = 0f;

    bool isRightFootstep;

    public AudioClip flipAudio;
    public AudioClip[] walkAudios;
    public AudioClip[] jumpAudios;
    public AudioClip dieAudio;
    public AudioClip usePortal;

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
    public JumpingStates jumpState = JumpingStates.ONE_JUMP;

    public SpriteRenderer playerSpriteRenderer;
    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public AnimationClip switchClip;
    public AudioSource playerAudioSource;
    
    private Gear currentGear;
    public LevelGenerator levelGenerator;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isPlayerDeath)
        {
            UpdateCooldowns();
            HandleInput();
            CheckDeath();
        }
    }

    void UpdateCooldowns()
    {
        switchCurrentCoolDown -= Time.deltaTime;
        deathCurrentCheckCoolDown -= Time.deltaTime;
        walkCurrentCoolDown -= Time.deltaTime;
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

        if (walkCurrentCoolDown <= 0 && jumpState == JumpingStates.GROUNDED)
        {
            if (isRightFootstep)
            {
                PlayAudio(walkAudios[Random.Range(0,2)]);
            }
            else
            {
                PlayAudio(walkAudios[Random.Range(2,4)]);           
            }
            walkCurrentCoolDown = WALK_COOLDOWN;
            isRightFootstep = !isRightFootstep;
        }
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
                if (jumpState == JumpingStates.ONE_JUMP)
                {
                    PlayAudio(jumpAudios[Random.Range(0,2)]);
                }
                else
                {
                    PlayAudio(jumpAudios[Random.Range(2,4)]);
                }
                // currentGear = levelGenerator.NeareastGearFromPlayer();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && switchCurrentCoolDown <= 0)
        {
            StartCoroutine(FlipPlayer());
            switchCurrentCoolDown = SWITCH_COOLDOWN;
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
        PlayAudio(flipAudio);
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
            PlayAudio(usePortal);
        }
    }

    void CheckDeath()
    {
        if (deathCurrentCheckCoolDown <= 0)
        {
            float distanceBetweenPositions = Vector2.Distance(transform.position, lastPosition);
            if (distanceBetweenPositions < 0.075f || distanceBetweenPositions > 15f)
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
                PlayAudio(dieAudio);
            }
            lastPosition = transform.position;
            deathCurrentCheckCoolDown = DEATH_COOLDOWN;
        }
    }

    void PlayAudio (AudioClip audioClip)
    {
        playerAudioSource.clip = audioClip;
        playerAudioSource.Play();
    }
}