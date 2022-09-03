using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        transform.Translate(new Vector2(-1 * Time.deltaTime, 0));
    }
    public void good()
    {
        anim.SetTrigger("enemy_yarare");
    }

    public void miss()
    {
        
    }

    public void OnBecameInvisible()
    {
        StartCoroutine("sleep");
        Destroy(gameObject);   
    }

    private IEnumerator sleep()
    {
        yield return new WaitForSeconds(0.2f);
    } 
}
