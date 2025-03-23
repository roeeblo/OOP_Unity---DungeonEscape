using System.Collections;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    public GameObject AcidEffect;
    public int Health { get; set; }

    public AudioClip attackClip; 
    public AudioClip deathClip; 
    public float attackRange = 30f; 
    private AudioSource audioSource;
    private Transform playerTransform;

    public override void Init()
    {
        base.Init();
        Health = base.health;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override void Update()
    {
    }

    public void Damage()
    {
        if (isdead) return;

        Health--;

        if (Health < 1)
        {
            isdead = true;
            animator.SetTrigger("Death");
            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }
            StartCoroutine(loot());
            StartCoroutine(RespawnAfterDelay(20f));
        }
        else
        {
            animator.SetTrigger("hitanim");
            ishit = true;
            animator.SetBool("incombat", true);
        }
    }

    public override void Attack()
    {
        Instantiate(AcidEffect, transform.position, Quaternion.identity);

        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= attackRange)
            {
                if (audioSource != null && attackClip != null)
                {
                    audioSource.PlayOneShot(attackClip);
                }
            }
        }
    }

    public override void MoveToPoint()
    {
        // empty to make sure spider doesn't move
    }

    public override IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isdead = false;
        animator.ResetTrigger("Death");
        animator.SetTrigger("alive");
        gameObject.SetActive(true);
    }
}
