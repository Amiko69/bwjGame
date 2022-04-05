using UnityEngine;

public class controller : MonoBehaviour
{
    float speed = .14f;
    bool justJumped = false;
    bool justMovedR = false;
    bool justMovedL = false;    

    CapsuleCollider2D collider1;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    GameObject sphere;

    void Awake()
    {
        sphere = GameObject.FindGameObjectWithTag("s");
        rb = GetComponent<Rigidbody2D>();
        collider1 = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        HandleInput();
        Debug.DrawRay(collider1.bounds.center, rb.velocity, Color.green);
    }

    private void FixedUpdate()
    {
        float xdif = -sphere.transform.position.x + transform.position.x;
        float ydif = -sphere.transform.position.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        transform.transform.rotation = Quaternion.Euler(0, 0, -angle);

        rb.AddForce(transform.up * 10);

        if (justJumped)
        {
            rb.AddForce(transform.up * 8, ForceMode2D.Impulse);
            justJumped = false;
        }
        if (justMovedR) { transform.position += transform.right * speed ; justMovedR = false; }
        if (justMovedL) { transform.position += transform.right * -speed ; justMovedL = false; }

    }

    void HandleInput()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            justJumped = true;
            
        }

        
        if (Input.GetKey(KeyCode.D))
        {
            justMovedR = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            justMovedL = true;
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycast = Physics2D.Raycast(collider1.bounds.center, -transform.up,(transform.localScale.y / 2 + .2f), groundLayer);
        if (raycast.collider != null) return true;
        else return false;
    }
}
