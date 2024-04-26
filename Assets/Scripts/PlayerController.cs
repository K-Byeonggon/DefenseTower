using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 inputVector = Vector2.zero;
    [SerializeField] private float jumpForce = 800f;
    [SerializeField] private int jumpCount = 0;
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool attacked = false;
    public bool Attacked {  get { return attacked; } set { attacked = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public int JumpCount { get { return jumpCount; } set { jumpCount = value; } }
    private PlayerInput playerInput;
    private Rigidbody2D playerRigidbody;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = inputVector*moveSpeed*Time.deltaTime;
        isRunning = movement != Vector2.zero;
        transform.Translate(movement);
        SetAnimation();
    }

    private void OnMove(InputValue inputValue)
    {
        if(PlayerOnHitManager.instance.playerStuned || PlayerOnHitManager.instance.playerFainted)
        {
            inputVector = Vector2.zero;
            return;
        }
        else
        {
            inputVector = inputValue.Get<Vector2>();
        }
        if (inputValue.Get<Vector2>().x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputValue.Get<Vector2>().x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnJump(InputValue inputValue)
    {
        bool isSkeyPressed = playerInput.actions["Down"].ReadValue<float>() > 0;

        if(PlayerOnHitManager.instance.playerStuned || PlayerOnHitManager.instance.playerFainted)
        {
            return;
        }

        if (inputValue.Get() != null && jumpCount < 2 && !isSkeyPressed)
        {
            SoundManager.instance.PlaySound("Jump");
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(Vector2.up * jumpForce);
        }
        else if(playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity / 2;
        }
    }
    
    private void SetAnimation()
    {
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isStuned", PlayerOnHitManager.instance.playerFainted);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7)
        {
            jumpCount = 0;
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Platform" || collision.collider.tag == "OneWayPlatform") isJumping = true;
        
    }
}
