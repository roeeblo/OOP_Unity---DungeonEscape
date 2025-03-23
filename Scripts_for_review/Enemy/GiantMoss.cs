using System.Collections;
using UnityEngine;

public class GiantMoss : Enemy, IDamageable
{
    public int Health { get; set; }

    public AudioClip hitSound;
    public AudioClip dieSound;
    private AudioSource audioSource;

    public override void Init()
    {
        base.Init();
        Health = base.health;
        audioSource = GetComponent<AudioSource>();
    }

    public override void MoveToPoint()
    {
        base.MoveToPoint();
    }

    public void Damage()
    {
        if (isdead) return;

        Health--;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("hitanim");
            ishit = true;
            animator.SetBool("incombat", true);
            PlaySound(hitSound);
        }
    }

    private void Die()
    {
        isdead = true;
        animator.SetTrigger("Death");
        PlaySound(dieSound);
        StartCoroutine(loot());

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(RespawnAfterDelay(20f));
    }

    public override IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Health = base.health;
        isdead = false;

        GetComponent<Collider2D>().enabled = true;

        transform.position = pointA.position;
        animator.ResetTrigger("Death");
        animator.SetTrigger("alive");
        animator.SetBool("incombat", false);
        gameObject.SetActive(true);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
