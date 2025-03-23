using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int diamonds;
    public int Health { get; set; }

    [SerializeField]
    public float jumpForce = 10f;
    [SerializeField]
    public float speed = 5f;
    [SerializeField]
    public float attackCooldown = 1f;
    [SerializeField]
    private Transform rightFoot;
    [SerializeField]
    private Transform leftFoot;

    // Add audio clips
    public AudioClip coinPickupSound;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip jumpSound;
    public AudioClip shopItemSound;
    public AudioClip attackSound;

    private bool grounded = false;
    private bool facingRight = true;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isDead = false;

    private Vector3 originalPosition;
    private PlayerAnimaton playerAnimation;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sword;
    private Rigidbody2D rigid;
    private AudioSource audioSource;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimaton>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        sword = transform.GetChild(1).GetComponent<SpriteRenderer>();
        sword.enabled = false;
        Health = 4;
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!isAttacking)
        {
            Movement();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && grounded && canAttack)
        {
            StartCoroutine(Attacking());
        }

        grounded = Grounded();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    IEnumerator Attacking()
    {
        isAttacking = true;
        canAttack = false;
        rigid.velocity = Vector2.zero;
        sword.enabled = true;
        playerAnimation.Attack();
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        yield return new WaitForSeconds(playerAnimation.GetAttackAnimationDuration());

        sword.enabled = false;
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }

    void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Flip(moveInput);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
        playerAnimation.Run(moveInput);
    }

    bool Grounded()
    {
        Vector2 rayStart = new Vector2((rightFoot.position.x + leftFoot.position.x) / 2, rightFoot.position.y);
        Vector2 rayEnd = new Vector2(rayStart.x + (0.24f - (-0.24f)) / 2, rayStart.y);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, Mathf.Abs(rayEnd.x - rayStart.x), 1 << 22);

        Debug.DrawRay(rayStart, Vector2.down * Mathf.Abs(rayEnd.x - rayStart.x), Color.green);

        return hit.collider != null;
    }

    void Jump()
    {
        if (grounded)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            playerAnimation.Jumping(true);
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
            StartCoroutine(ResetJumperRoutine());
        }
    }

    void Flip(float move)
    {
        if (move > 0 && !facingRight)
        {
            FlipPlayer();
            sword.flipX = false;
            sword.flipY = false;
            rightFoot.localPosition = new Vector3(0.24f, rightFoot.localPosition.y, rightFoot.localPosition.z);
            leftFoot.localPosition = new Vector3(-0.24f, leftFoot.localPosition.y, leftFoot.localPosition.z);
        }
        else if (move < 0 && facingRight)
        {
            FlipPlayer();
            sword.flipX = true;
            sword.flipY = true;
            rightFoot.localPosition = new Vector3(-0.24f, rightFoot.localPosition.y, rightFoot.localPosition.z);
            leftFoot.localPosition = new Vector3(0.24f, leftFoot.localPosition.y, leftFoot.localPosition.z);
        }
    }

    void FlipPlayer()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;

        Vector3 newX = spriteRenderer.transform.localPosition;
        newX.x = facingRight ? 0.09f : -0.09f;
        spriteRenderer.transform.localPosition = newX;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
        rigid.velocity = Vector2.zero;
    }

    public void Damage()
    {
        if (isDead)
        {
            return;
        }

        if (Health < 1)
        {
            isDead = true;
            playerAnimation.Death();
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            return;
        }

        Health--;
        UIManager.Instance.UpdateLives(Health);
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (Health < 1)
        {
            isDead = true;
            playerAnimation.Death();
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
        }
    }

    public void AddGems(int amount)
    {
        diamonds += amount;
        UIManager.Instance.UpdateGemCount(diamonds);
        if (coinPickupSound != null)
        {
            audioSource.PlayOneShot(coinPickupSound);
        }
    }

    IEnumerator ResetJumperRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (!Grounded())
        {
            yield return null;
        }
        playerAnimation.Jumping(false);
    }
}
