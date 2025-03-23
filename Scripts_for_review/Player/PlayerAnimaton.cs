using UnityEngine;

public class PlayerAnimaton : MonoBehaviour
{
    private SpriteRenderer swordArcSprite;
    private Animator animator;
    private Animator Swordanimator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private AnimationClip attackAnimationClip;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Swordanimator = transform.GetChild(1).GetComponent<Animator>();
        spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void Run(float move)
    {
        animator.SetFloat("Move", Mathf.Abs(move));
    }

    public void Jumping(bool jump)
    {
        animator.SetBool("Jumping", jump);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        Swordanimator.SetTrigger("Swordarc");
    }

    public float GetAttackAnimationDuration()
    {
        if (attackAnimationClip != null)
        {
            return attackAnimationClip.length;
        }
        else
        {
            Debug.LogWarning("Attack Animation Clip is not assigned.");
            return 0f;
        }
    }
    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
