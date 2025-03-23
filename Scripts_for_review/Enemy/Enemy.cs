using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject DiamondPrefab;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Transform pointA, pointB;
    public int gems;
    protected Vector3 curretarget;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Player player;
    protected bool ishit = false;
    protected bool isdead = false;
    private bool isAttacking = false;

    public virtual void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("idleanim") && !animator.GetBool("incombat"))
        {
            return;
        }
        if (!isdead)
        {
            MoveToPoint();
        }
    }

    public virtual void MoveToPoint()
    {
        if (transform.position == pointA.position)
        {
            curretarget = pointB.position;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetTrigger("idleanim");
        }
        else if (transform.position == pointB.position)
        {
            curretarget = pointA.position;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetTrigger("idleanim");
        }

        if (!ishit)
        {
            transform.position = Vector3.MoveTowards(transform.position, curretarget, speed * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.localPosition, player.transform.localPosition);

        if (distance > 5.0f)
        {
            ishit = false;
            animator.SetBool("incombat", false);
        }

        Vector3 direction = player.transform.localPosition - transform.localPosition;
        if (direction.x > 0 && animator.GetBool("incombat"))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0 && animator.GetBool("incombat"))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected IEnumerator loot()
    {
        for (int i = 0; i < gems; i++)
        {
            Instantiate(DiamondPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public virtual void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;

        animator.SetTrigger("hitanim");

        StartCoroutine(ResetAttackFlag());
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    public virtual IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isdead = false;
        animator.ResetTrigger("Death");
        animator.SetTrigger("alive");
        gameObject.SetActive(true);
    }
}
