using UnityEngine;

public class NewMovement : MonoBehaviour
{
    float speed = .14f;
    bool justJumped = false;
    float coolDown = 0;
    /*bool justMovedR = false;
    bool justMovedL = false;*/

    CapsuleCollider2D collider1;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    GameObject sphere;
    Animator animator;

    bool bw;

    void Awake()
    {
        bw = false;
        sphere = GameObject.FindGameObjectWithTag("s");
        rb = GetComponent<Rigidbody2D>();
        collider1 = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        coolDown -= Time.deltaTime;
        HandleInput();
        Debug.DrawRay(collider1.bounds.center, rb.velocity, Color.green);
    }

    private void FixedUpdate()
    {
        float xdif = -sphere.transform.position.x + transform.position.x;
        float ydif = -sphere.transform.position.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        transform.transform.rotation = Quaternion.Euler(0, 0, -angle);

        if (bw)
        {
            rb.AddForce(transform.up * 15);
        }
        else
        {
            rb.AddForce(-transform.up * 15);
        }

        if (justJumped)
        {
            if (bw)
            {
                rb.AddForce(-transform.up * 10, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
            }

            animator.Play("jump");

            justJumped = false;
        }
        transform.position += transform.right * speed;
        if (rb.velocity.x > 8)
        {
            rb.velocity = new Vector2(8, rb.velocity.y);
        }
        if (rb.velocity.y > 8)
        {
            rb.velocity = new Vector2(rb.velocity.x, 8);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            justJumped = true;
        }

        if (Input.GetMouseButtonDown(0) && coolDown <= 0)
        {
            bw = !bw;
            if (bw)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                rb.AddForce(-transform.up * 10, ForceMode2D.Impulse);
                Invoke("turnOnCollider", 0.2f);
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
                rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
                Invoke("turnOnCollider", 0.2f);
            }

            coolDown = 2;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycast;
        if (bw)
        {
            raycast = Physics2D.Raycast(collider1.bounds.center, transform.up, (transform.localScale.y / 2 + .2f), groundLayer);
        }
        else
        {
            raycast = Physics2D.Raycast(collider1.bounds.center, -transform.up, (transform.localScale.y / 2 + .2f), groundLayer);
        }
        if (raycast.collider != null) return true;
        else return false;
    }

    private void turnOnCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
