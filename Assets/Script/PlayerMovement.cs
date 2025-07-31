using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool toggleCrouch = true;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private bool isCrouching;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        bool sPressed = Input.GetKey("s");  
        bool sKeyCode = Input.GetKey(KeyCode.S);

        if (sPressed || sKeyCode)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (!isCrouching)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {   
            body.velocity = new Vector2(0, body.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();
        anim.SetBool("Walk", horizontalInput != 0 && !isCrouching);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Crouch", isCrouching);
    }
  
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
        grounded = false;
    }
    private void HandleCrouchInput()
    {
        if (toggleCrouch)
        {
            if (Input.GetKeyDown(KeyCode.S) && grounded)
            {
                isCrouching = !isCrouching; 
            }
        }
        else
        {
            isCrouching = Input.GetKey(KeyCode.S) && grounded;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}