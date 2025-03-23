using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEffect : MonoBehaviour
{

    private Spider spider;
    void Start()
    {
        spider = transform.parent.GetComponent<Spider>();
    }

    public void Fire()
    {
        spider.Attack();
    }
   
}
