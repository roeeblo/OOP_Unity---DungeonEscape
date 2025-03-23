using System.Collections;
using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    public int Health { get; set; }
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public override void Init()
    {
        base.Init();
        Health = base.health;
        audioSource = GetComponent<AudioSource>();
    }

    public override void MoveToPoint()
    {
        if (!isdead)
        {
            base.MoveToPoint();
        }
    }

    public void Damage()
    {
        if (isdead) return;

        Health--;

        if (Health < 1)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("hitanim");
            ishit = true;
            animator.SetBool("incombat", true);
            PlayHitSound();
        }
    }

    private void Die()
    {
        isdead = true;
        animator.SetTrigger("Death");
        PlayDeathSound();
        StartCoroutine(loot());
        StartCoroutine(RespawnAfterDelay(20f));
    }

    public override IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isdead = false;
        animator.ResetTrigger("Death");
        animator.SetTrigger("alive");
        gameObject.SetActive(true);
        Init();
    }

    private void PlayHitSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
    }
}
