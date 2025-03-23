using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool canattack = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null && canattack)
        {
            hit.Damage();
            canattack = false; 

            StartCoroutine(ResetCanAttack());
        }
    }

    IEnumerator ResetCanAttack()
    {
        yield return new WaitForSeconds(0.4f); 
        canattack = true; 
    }
}
