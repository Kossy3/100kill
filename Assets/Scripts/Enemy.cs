using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public Database database;
    Animator anim;
    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public void FixedUpdate()
    {
        transform.Translate(new Vector2( (float)(-(16 / (60 / (database.BPM / 4)))) * Time.deltaTime, 0));
        if (Mathf.Floor(transform.position.x) == -10)
        {
            Destroy(gameObject);
        }
    }
    public void good()
    {
        anim.SetTrigger("enemy_yarare");
    }

    public void miss()
    {
        
    }

}
