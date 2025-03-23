using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
   private Rigidbody2D rb;
   private void Start()

    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-200, 200), Random.Range(500, 600)), ForceMode2D.Force);
    }

   public int gems = 1;

   private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player")
        {
           Player player = other.GetComponent<Player>();
           if (player != null)

           {
              player.AddGems(gems);
               Destroy(this.gameObject);
           }
        }

    }
}
