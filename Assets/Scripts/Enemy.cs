using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int color_number = 0;
    public Database database;
    public Animator anim;
    public AudioSource[] missaudio;
    // Start is called before the first frame update
    public void Start()
    {
        missaudio = GameObject.Find("AudioManager").GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public void FixedUpdate()
    {
        transform.Translate(new Vector2((-(16 / (60 / ((float)database.BPM / 4)))) * Time.deltaTime, 0));
        if (Mathf.Floor(transform.position.x) == -10)
        {
            Destroy(gameObject);
        }
    }
    public void good()
    {
        if (gameObject.tag == "Enemy")
        {
            database.defeated_enemies += 1;
            anim.SetTrigger("enemy_yarare");
            if (color_number != 0)
            {
                database.add_defeat_color_number(color_number);
                database.charge_HP(1);
            }
        }
        if (gameObject.tag == "Obstacle")
        {

        }
    }

    public void miss()
    {
        database.charge_HP(-1);
        if(Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            missaudio[1].Play();
        }
        else
        {
            missaudio[2].Play();
        }
    }
}
